using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using Borg.Infra;

namespace Borg.Cms.Basic.Lib.Features.Auth
{
    public interface IClaimTypeDisplayProvider
    {
        string Display(string claimType);
    }

    public class ClaimTypeDisplayProvider : IClaimTypeDisplayProvider
    {
        private static readonly Lazy<ConcurrentDictionary<string, string>> _dict = new Lazy<ConcurrentDictionary<string, string>>(() =>
        {
            var dx = new Dictionary<string, string>
            {
                {ClaimTypes.Surname, "Last Name"},
                {ClaimTypes.GivenName, "First Name"},
                {BorgClaimTypes.Avatar, "Avatar" }
            };
            return new ConcurrentDictionary<string, string>(dx);
        });

        private static ConcurrentDictionary<string, string> Source => _dict.Value;

        public string Display(string claimType)
        {
            return Source.ContainsKey(claimType) ? Source[claimType] : claimType;
        }
    }
}