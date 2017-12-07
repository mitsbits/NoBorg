using Borg;
using Borg.Infra;
using Borg.Infra.DAL;
using Borg.Infra.DTO;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Web.Domain;
using Timesheets.Web.Features.Taxonomies.Events;
using Timesheets.Web.Features.Taxonomies.Requests;

namespace Timesheets.Web.Features.Taxonomies.Services
{
    public class TaxonomyService : ITaxonomyService, IDisposable, IAsyncNotificationHandler<TaxonomyTreeChangeEvent>
    {
        private readonly IMediator _dispatcher;
        private Tidings _sourceTree;
        private IEnumerable<Tiding> _sourceTreeFlat;
        private bool _valid = false;
        private readonly AsyncLock _lock = new AsyncLock();
        private readonly ILogger Logger;

        public TaxonomyService(ILoggerFactory loggerFactory, IMediator dispatcher)
        {
            _dispatcher = dispatcher;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<IEnumerable<Tiding>> FlatTree()
        {
            if (_valid && _sourceTreeFlat != null) return _sourceTreeFlat;
            await Populate();
            return _sourceTreeFlat;
        }

        public async Task<Tidings> Tree()
        {
            if (_valid && _sourceTree != null) return _sourceTree;
            await Populate();
            return _sourceTree;
        }

        public Task<Tidings> Tree(IEnumerable<Taxonomy> items)
        {
            return Task.FromResult(ProduceTree(items));
        }

        public async Task Invalidate()
        {
            using (await _lock.LockAsync())
            {
                _valid = false;
            }
        }

        public void Dispose()
        {
            Logger.LogDebug(default(EventId), "Cache disposes");
            if (_sourceTree == null) return;
            _sourceTree.Clear();
            _sourceTree = null;
            _sourceTreeFlat = null;
        }

        public async Task Handle(TaxonomyTreeChangeEvent notification)
        {
            Logger.LogDebug(default(EventId), "Cache will invalidate because of {@event}", notification);
            await Invalidate().AnyContext();
        }

        private static Tidings ProduceTree(IEnumerable<Taxonomy> taxonomies)
        {
            var result = new Tidings();
            if (taxonomies == null || !taxonomies.Any(x => x.IsRoot)) return result;
            var roots = taxonomies.Where(x => x.IsRoot);

            var i = 1;
            foreach (var root in roots)
            {
                result.Add(Produce(root, taxonomies, 1));
            }
            return result;
        }

        private static Tiding Produce(Taxonomy root, IEnumerable<Taxonomy> source, int level)
        {
            var r = new Tiding(root.Id.ToString(), root.DisplayName) { Weight = level, Flag = root.IsEnabled.ToString() };
            foreach (var taxonomy in source.Where(x => x.ParentId.Equals(root.Id)))
            {
                r.Children.Add(Produce(taxonomy, source, level + 1));
            }
            return r;
        }

        private async Task Populate()
        {
            var w = new Stopwatch();
            w.Start();
            Logger.LogDebug(default(EventId), "Cache populates");
            var result = await _dispatcher.Send(new TaxonomiesTreeRequest());
            if (result.Succeded)
            {
                var tidings = (result as QueryResult<Tidings>)?.Payload;
                using (await _lock.LockAsync())
                {
                    if (tidings != null) _sourceTree = tidings;
                    _sourceTreeFlat = _sourceTree.Flatten();
                    _valid = true;
                }
            }
            w.Stop();
            Logger.LogDebug(default(EventId), "Cache populated, ellapsed : {ellapsed}", w.Elapsed);
        }
    }
}