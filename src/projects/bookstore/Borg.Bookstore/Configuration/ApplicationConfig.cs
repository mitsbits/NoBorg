using Borg.Infra;
using Borg.Infra.Configuration.Contracts;
using Borg.Platform.Identity.Configuration;

namespace Borg.Bookstore.Configuration
{
    public class ApplicationConfig : BorgSettings, ISettingsProvider<IdentityConfig>
    {

        public IdentityConfig Identity { get; set; } = new IdentityConfig();

        IdentityConfig ISettingsProvider<IdentityConfig>.Config => Identity;
    }
}