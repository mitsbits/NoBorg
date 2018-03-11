using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Borg.Infra.DDD.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Borg.Infra.DAL
{
    public interface IIStaticStore
    {
        IStaticEntityStore<TEntity> EntityStore<TEntity>() where TEntity : IEntity;
    }
    public interface IStaticEntityStore<out TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> Query { get; }
    }

    public abstract class StaticEntityStore<TEntity> : IStaticEntityStore<TEntity> where TEntity : IEntity
    {
        static ReaderWriterLock _rwl = new ReaderWriterLock();
        private const int _timeOut = 100;
        private readonly List<TEntity> _data = new List<TEntity>();
        protected readonly ILogger _logger;
        protected StaticEntityStore(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }
        protected virtual Task Populate(IEnumerable<TEntity> collection)
        {
            _logger.Info("Populating static store for {type}", typeof(TEntity).FullName);
            var watch = Stopwatch.StartNew();
            try
            {
                _rwl.AcquireReaderLock(_timeOut);
                try
                {
                    _data.Clear();
                    _data.AddRange(collection);
                }
                finally
                {
                    _rwl.ReleaseReaderLock();
                    var ellapsed = watch.Elapsed;
                    watch.Stop();
                    watch = null;
                    _logger.Info("Populatied static store for {type} in {time}", typeof(TEntity).FullName, ellapsed.ToString("G"));
                }
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, "Failed to load static store for {type} - {reason}", typeof(TEntity).FullName, ex.ToString());
            }
            return Task.CompletedTask;
        }
        public IQueryable<TEntity> Query { get; }
    }
}
