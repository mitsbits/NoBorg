using Borg.Infra;
using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;

using Borg.MVC.Services.ServerResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Borg.MVC.Services.UserSession
{
    public class BorgUserSession : UserSession, ISessionServerResponseProvider, ICanContextualize, ICanContextualizeFromController, IContextAwareUserSession
    {
        private readonly ISessionServerResponseProvider _srprovider;
        //private readonly Func<string, IFileStorage> _storageFactory;

        public BorgUserSession(IHttpContextAccessor httpContextAccessor, ISerializer serializer, ISessionServerResponseProvider srprovider/*, Func<string, IFileStorage> storageFactory*/) : base(httpContextAccessor, serializer)
        {
            Preconditions.NotNull(srprovider, nameof(srprovider));
            //Preconditions.NotNull(storageFactory, nameof(storageFactory));
            _srprovider = srprovider;
            //_storageFactory = storageFactory;
        }

        #region IServerResponseProvider

        public void Push(ServerResponse message)
        {
            _srprovider.Push(message);
        }

        public ServerResponse Pop()
        {
            return _srprovider.Pop();
        }

        public IReadOnlyCollection<ServerResponse> Messages => _srprovider.Messages;

        #endregion IServerResponseProvider

        //#region IUserSessionStorage

        //private IScopedFileStorage _userStorage = null;

        //public IScopedFileStorage UserStorage
        //{
        //    get
        //    {
        //        if (_userStorage != null) return _userStorage;
        //        var prefix = IsAuthenticated() ? $"sessions/{SessionId}" : "sessions/_public";
        //        _userStorage = _storageFactory.Invoke("temp").Scope(prefix) as IScopedFileStorage;
        //        return _userStorage;
        //    }
        //}

        //#endregion IUserSessionStorage

        #region ICanContextualize

        public override bool ContextAcquired { get; protected set; } = false;

        #endregion ICanContextualize

        #region ICanContextualizeFromController

        public void Contextualize(Controller controller)
        {
            if (ContextAcquired) return;
            ContextAcquired = _srprovider.TryContextualize(controller);
        }

        #endregion ICanContextualizeFromController

        //#region IFileStorage

        //public Task<Stream> GetFileStream(string path, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return UserStorage.GetFileStream(path, cancellationToken);
        //}

        //public Task<IFileSpec> GetFileInfo(string path, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return UserStorage.GetFileInfo(path, cancellationToken);
        //}

        //public Task<bool> Exists(string path, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return UserStorage.Exists(path, cancellationToken);
        //}

        //public Task<bool> SaveFile(string path, Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return UserStorage.SaveFile(path, stream, cancellationToken);
        //}

        //public Task<bool> RenameFile(string path, string newpath, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return UserStorage.RenameFile(path, newpath, cancellationToken);
        //}

        //public Task<bool> CopyFile(string path, string targetpath, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return UserStorage.CopyFile(path, targetpath, cancellationToken);
        //}

        //public Task<bool> DeleteFile(string path, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return UserStorage.DeleteFile(path, cancellationToken);
        //}

        //public Task<IEnumerable<IFileSpec>> GetFileList(string searchPattern = null, int? limit = null, int? skip = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return UserStorage.GetFileList(searchPattern, limit, skip, cancellationToken);
        //}

        //#endregion IFileStorage

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //UserStorage.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BorgUserSession() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}