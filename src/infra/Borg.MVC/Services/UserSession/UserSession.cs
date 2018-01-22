using Borg.Infra;
using Borg.Infra.DTO;

using Borg.MVC.BuildingBlocks;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Borg.MVC.Services.UserSession
{
    public abstract class UserSession : Tidings, IUserSession, ICanContextualize
    {
        protected const string SettingsCookieName = "Borg.UserSession"; //TODO: retrieve from settings
        protected const string SessionStartKey = "Borg.SessionStartKey";//TODO: retrieve from settings

        protected UserSession(IHttpContextAccessor httpContextAccessor, ISerializer serializer)
        {
            Preconditions.NotNull(httpContextAccessor, nameof(httpContextAccessor));
            Preconditions.NotNull(serializer, nameof(serializer));
            HttpContext = httpContextAccessor.HttpContext;
            Serializer = serializer;
            SessionId = HttpContext.Session.Id;
            ReadState();
            SaveState();
        }

        #region IUserSession

        public void StartSession()
        {
            Remove(SessionStartKey);
            SaveState();
        }

        public void StopSession()
        {
            Remove(SessionStartKey);
            HttpContext.Response.Cookies.Delete(SettingsCookieName);
        }

        public DateTimeOffset SessionStart
        {
            get
            {
                if (!ContainsKey(SessionStartKey))
                {
                    var tiding = new Tiding(SessionStartKey);
                    tiding.SetValue(DateTimeOffset.UtcNow, Serializer);
                    Add(tiding);
                    SaveState();
                }
                return RootByKey[SessionStartKey].GetValue<DateTimeOffset>(Serializer);
            }
        }

        public string UserIdentifier => !IsAuthenticated() ? string.Empty : HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        public string UserName => !IsAuthenticated() ? string.Empty : HttpContext.User.Identity.Name;
        private string _displayName;
        private string GetDiplayName()
        {
            if (string.IsNullOrWhiteSpace(_displayName))
            {
                _displayName = !IsAuthenticated()
                    ? string.Empty
                    : HttpContext.User.Claims.Any(x => x.Type == ClaimTypes.Surname) &&
                      HttpContext.User.Claims.Any(x => x.Type == ClaimTypes.GivenName)
                        ? $"{HttpContext.User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value} {HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Surname).Value}"
                        : HttpContext.User.Identity.Name;
            }
            return _displayName;
        }
        public string DisplayName => GetDiplayName();
  

        private string _avatar;
        private string GetAvatar()
        {
            if (string.IsNullOrWhiteSpace(_avatar))
            {
                _avatar = !IsAuthenticated()
                    ? string.Empty
                    : HttpContext.User.Claims.Any(x => x.Type ==  BorgClaimTypes.Avatar)
                        ? HttpContext.User.Claims.First(x => x.Type == BorgClaimTypes.Avatar).Value
                        : "";
            }
            return _avatar;
        }

        public string Avatar => GetAvatar();








        public bool IsAuthenticated()
        {
            return HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated;
        }

        public string SessionId { get; }

        public T Setting<T>(string key, T value)
        {
            Preconditions.NotEmpty(key, nameof(key));
            T setting = default(T);
            if (value != null)
            {
                setting = value;
                if (ContainsKey(key))
                {
                    RootByKey[key].SetValue(setting, Serializer);
                }
                else
                {
                    var tiding = new Tiding(key);
                    tiding.SetValue(setting, Serializer);
                    Add(tiding);
                }
                SaveState();
            }
            else
            {
                if (ContainsKey(key))
                {
                    setting = RootByKey[key].GetValue<T>(Serializer);
                }
            }
            return setting;
        }

        public T Setting<T>(string key)
        {
            Preconditions.NotEmpty(key, nameof(key));
            T setting = default(T);
            if (ContainsKey(key))
            {
                setting = RootByKey[key].GetValue<T>(Serializer);
            }
            return setting;
        }

        #endregion IUserSession

        #region ICanContextualize

        public abstract bool ContextAcquired { get; protected set; }

        #endregion ICanContextualize

        protected virtual HttpContext HttpContext { get; }

        protected virtual ISerializer Serializer { get; }

        protected virtual void SaveState()
        {
            string data = Serializer.SerializeToString(this as Tidings);
            CookieOptions options = new CookieOptions { HttpOnly = true };
            HttpContext.Response.Cookies.Append(SettingsCookieName, data, options);
        }

        protected virtual void ReadState()
        {
            if (HttpContext.Request.Cookies.ContainsKey(SettingsCookieName))
            {
                var jsonData = HttpContext.Request.Cookies[SettingsCookieName];
                Tidings data = Serializer.Deserialize<Tidings>(jsonData);
                Clear();
                foreach (var tiding in data)
                {
                    Add(tiding);
                }
            }
        }
    }
}