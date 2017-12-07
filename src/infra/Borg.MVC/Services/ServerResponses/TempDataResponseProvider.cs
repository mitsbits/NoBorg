using System;
using System.Collections.Generic;
using System.Linq;
using Borg.Infra;
using Borg.Infra.DTO;
using Borg.Infra.Serializer;
using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Borg.MVC.Services.ServerResponses
{
    public class TempDataResponseProvider : ISessionServerResponseProvider, ICanContextualizeFromController, IViewContextAware
    {
        private readonly ISerializer _serializer;
        private static readonly string _key;

        private readonly HashSet<ServerResponse> _bucket = new HashSet<ServerResponse>();

        private ITempDataDictionary _context;
        private bool _loaded;

        static TempDataResponseProvider()
        {
            _key = $"TEMP_DATA_KEY:{typeof(TempDataResponseProvider).Name}";
        }

        bool ICanContextualize.ContextAcquired => _loaded;

        public TempDataResponseProvider(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void Push(ServerResponse message)
        {
            if (!_loaded) throw new Exception(nameof(_context));
            _bucket.Add(message);
            _context[_key] = _serializer.SerializeToString(_bucket);
        }

        public IReadOnlyCollection<ServerResponse> Messages => _bucket;


        public ServerResponse Pop()
        {
            if (!_bucket.Any()) return null;
            var result = _bucket.First();
            _bucket.Remove(result);
            return result;
        }

        public void Contextualize(ViewContext context)
        {
            _context = context.TempData;
            Load();
        }

        public void Contextualize(Controller context)
        {
            _context = context.TempData;
            Load();
        }

        private void Load()
        {
            if(_loaded) return;
            _bucket.Clear();
            if (_context != null && _context.ContainsKey(_key))
            {
                var bucket = _serializer.Deserialize<HashSet<ServerResponse>>(_context[_key].ToString());
                if (bucket == null || !bucket.Any()) return;
                foreach (var message in bucket)
                {
                    _bucket.Add(message);
                }
            }
            _loaded = true;
        }
    }
}
