using Borg.Infra;
using Borg.Infra.Configuration.Contracts;
using Borg.Platform.Identity.Configuration;

namespace Borg.Bookstore.Configuration
{
    public class ApplicationConfig : BorgSettings, ISettingsProvider<IdentityConfig>
    {
        public string ApplicationName { get; set; }
        public string ApplicationEndpoint { get; set; }
        public string SqlConnectionString { get; set; }
        public IdentityConfig Identity { get; set; } = new IdentityConfig();

        IdentityConfig ISettingsProvider<IdentityConfig>.Config => Identity;
    }
}