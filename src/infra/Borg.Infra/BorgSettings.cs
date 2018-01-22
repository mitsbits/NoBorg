using System.Collections.Generic;

namespace Borg.Infra
{
    public class BorgSettings
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public PaginationInfoStyle PaginationInfoStyle { get; set; }
        public StorageSettings Storage { get; set; }
        public TenantSettings Tenant { get; set; }
        public AuthSettings Auth { get; set; }
    }

    public class StorageSettings
    {
        public string Folder { get; set; }
        public string AssetStoreContainer { get; set; }
        public string AzureStorageConnection { get; set; }
    }

    public class TenantSettings
    {
        public string ServiceTag { get; set; }
        public string Endpoint { get; set; }
    }

    public class AuthSettings
    {
        public bool ActivateOnRegisterRequest { get; set; } = false;
        public string LoginPath { get; set; } = "/login";
        public string LogoutPath { get; set; } = "/logout";
        public string AccessDeniedPath { get; set; } = "/denied";
        public DefaultUserSettings DefaultUser { get; set; } = new DefaultUserSettings();

        public class DefaultUserSettings
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string[] Roles { get; set; } = new string[0];
        }
    }
}