using System;

namespace Borg.Infra.DTO
{
    public class ServerResponse<TKey> : ServerResponse, IServerResponse<TKey> where TKey : IEquatable<TKey>
    {
        public ServerResponse(TKey etag, ResponseStatus status, string title, string message = "") : base(status, title,
            message)
        {
            ETag = etag;
        }

        public ServerResponse(TKey etag, Exception ex) : base(ex)
        {
            ETag = etag;
        }

        public virtual TKey ETag
        {
            get => (TKey)Convert.ChangeType(GetOrDefault(nameof(ETag)), typeof(TKey));
            protected set => AddOrUpdate(nameof(ETag), value.ToString());
        }
    }

    public class ServerResponse : Tidings, IServerResponse
    {
        public ServerResponse()
        {
        }

        public ServerResponse(ResponseStatus status, string title, string message = "") : this()
        {
            Status = status;
            Title = title;
            Message = message;
        }

        public ServerResponse(Exception ex) : this(ResponseStatus.Error, ex.GetType().ToString(), ex.Message)
        {
        }

        public ResponseStatus Status
        {
            get
            {
                ResponseStatus output;
                Enum.TryParse(GetOrDefault(nameof(Status)), out output);
                return output;
            }
            set => AddOrUpdate(nameof(Status), value.ToString());
        }

        public string Title
        {
            get => GetOrDefault(nameof(Title));
            set => AddOrUpdate(nameof(Title), value);
        }

        public string Message
        {
            get => GetOrDefault(nameof(Message));
            set => AddOrUpdate(nameof(Message), value);
        }

        #region Private

        protected string GetOrDefault(string key)
        {
            return ContainsKey(key) ? this[key] : string.Empty;
        }

        protected void AddOrUpdate(string key, string value)
        {
            if (Keys.Contains(key))
                this[key] = value;
            else
                Add(key, value);
        }

        #endregion Private
    }
}