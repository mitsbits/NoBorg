using Borg.Infra.Configuration.Contracts;
using System.Collections.Generic;

namespace Borg.Infra
{
    public class BorgSettings : ISettingsProvider<StorageSettings>, 
        ISettingsProvider<TenantSettings>, 
        ISettingsProvider<PaginationInfoStyle>,
        ISettingsProvider<VisualSettings>
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public PaginationInfoStyle PaginationInfoStyle { get; set; } = new PaginationInfoStyle();
        public StorageSettings Storage { get; set; } = new StorageSettings();
        public TenantSettings Tenant { get; set; } = new TenantSettings();

        //public AuthSettings Auth { get; set; }
        public VisualSettings Visual { get; set; }

        StorageSettings ISettingsProvider<StorageSettings>.Config => Storage;

        TenantSettings ISettingsProvider<TenantSettings>.Config => Tenant;

        PaginationInfoStyle ISettingsProvider<PaginationInfoStyle>.Config => PaginationInfoStyle;

        VisualSettings ISettingsProvider<VisualSettings>.Config => Visual;
    }

    public class StorageSettings : ISettings
    {
        public string ImagesCacheEndpoint { get; set; } = "http://127.0.0.1:10000/devstoreaccount1";
        public string ImagesCacheFolder { get; set; } = "cache";
        public string Folder { get; set; } = "assets";
        public string AssetStoreContainer { get; set; }
        public string AzureStorageConnection { get; set; }
    }

    public class TenantSettings : ISettings
    {
        public string ServiceTag { get; set; }
        public string Endpoint { get; set; }
        public string GoogleAnalyticsTrackingId { get; set; }
    }

    public class AuthSettings : ISettings
    {
        public bool ActivateOnRegisterRequest { get; set; } = false;
        public string LoginPath { get; set; } = "/login";
        public string LogoutPath { get; set; } = "/logout";
        public string AccessDeniedPath { get; set; } = "/denied";
        public DefaultUserSettings DefaultUser { get; set; } = new DefaultUserSettings();

        public class DefaultUserSettings : ISettings
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string[] Roles { get; set; } = new string[0];
        }
    }

    public class VisualSettings : ISettings
    {
        public Dictionary<string, int> WidthPixels { get; set; } = new Dictionary<string, int>();
    }
}