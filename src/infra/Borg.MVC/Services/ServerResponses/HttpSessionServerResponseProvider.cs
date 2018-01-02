using Borg.Infra;
using Borg.Infra.DTO;
using Borg.Infra.Serializer;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.Services.ServerResponses
{
    public class HttpSessionServerResponseProvider : ISessionServerResponseProvider
    {
        private readonly ISerializer _serializer;
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly List<ServerResponse> _bucket;

        private readonly string _key;

        public HttpSessionServerResponseProvider(ISerializer serializer, IHttpContextAccessor contextAccessor)
        {
            Preconditions.NotNull(serializer, nameof(serializer));
            Preconditions.NotNull(contextAccessor, nameof(contextAccessor));
            _bucket = new List<ServerResponse>();
            _serializer = serializer;
            _contextAccessor = contextAccessor;

            GuardSession();

            _key = $"{nameof(HttpSessionServerResponseProvider)}:{Session.Id}";
        }

        public void Push(ServerResponse message)
        {
            Preconditions.NotNull(message, nameof(message));
            Load();
            if (_bucket.Contains(message)) return;
            _bucket.Add(message);
            Persist();
        }

        public IReadOnlyCollection<ServerResponse> Messages => _bucket;

        public ServerResponse Pop()
        {
            Load();
            if (!_bucket.Any()) return null;
            var result = _bucket.First();
            _bucket.RemoveAt(0);
            Persist();
            return result;
        }

        private void Persist()
        {
            Session.Set(_key, _serializer.Serialize(_bucket));
        }

        private bool _loaded;

        private void Load()
        {
            if (_loaded) return;
            if (Session.Keys.Contains(_key))
            {
                var bucket = _serializer.Deserialize<List<ServerResponse>>(Session.Get(_key));
                _bucket.Clear();
                _bucket.AddRange(bucket);
            }
            _loaded = true;
        }

        private ISession _session;

        private ISession Session
        {
            get
            {
                if (_session != null) return _session;
                GuardSession();
                _session = _contextAccessor.HttpContext.Session;
                return _session;
            }
        }

        bool ICanContextualize.ContextAcquired => _loaded;

        private void GuardSession()
        {
            if (_contextAccessor?.HttpContext == null || !_contextAccessor.HttpContext.Session.IsAvailable) throw new BorgApplicationException(nameof(_contextAccessor.HttpContext.Session));
        }
    }
}