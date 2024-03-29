﻿using Borg.Infra;
using Borg.Infra.DTO;

using Borg.MVC.BuildingBlocks;
using Borg.MVC.BuildingBlocks.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Preconditions.NotNull(serializer, nameof(serializer));
            _serializer = serializer;
        }

        public void Push(ServerResponse message)
        {
            if (!_loaded) throw new Exception(nameof(_context));
            Preconditions.NotNull(message, nameof(message));
            _bucket.Add(message);
            _context[_key] = _serializer.SerializeToString(_bucket);
        }

        public IReadOnlyCollection<ServerResponse> Messages => _bucket;

        public ServerResponse Pop()
        {
            if (!_bucket.Any()) return null;
            var result = _bucket.First();
            _bucket.Remove(result);
            _context[_key] = _serializer.SerializeToString(_bucket);
            return result;
        }

        public void Contextualize(ViewContext context)
        {
            Preconditions.NotNull(context, nameof(context));
            _context = context.TempData;
            Load();
        }

        public void Contextualize(Controller context)
        {
            Preconditions.NotNull(context, nameof(context));
            _context = context.TempData;
            Load();
        }

        private void Load()
        {
            if (_loaded) return;
            _bucket.Clear();
            if (_context != null && _context.ContainsKey(_key))
            {
                var bucket = _serializer.Deserialize<HashSet<ServerResponse>>(_context[_key].ToString());
                if (bucket == null || !bucket.Any())
                {
                    _loaded = true;
                    return;
                }
                foreach (var message in bucket)
                {
                    _bucket.Add(message);
                }
            }
            _loaded = true;
        }
    }
}