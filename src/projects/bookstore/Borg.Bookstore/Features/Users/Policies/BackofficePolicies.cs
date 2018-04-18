using Borg.CMS;
using Borg.Platform.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Borg.Bookstore.Features.Users.Policies
{
    public class BackofficePolicies
    {
        public const string ContentEditor = nameof(ContentEditor);
        public const string UserManagement = nameof(UserManagement);
        public const string Backoffice = nameof(Backoffice);

        public static IDictionary<string, AuthorizationPolicy> GetPolicies()
        {
            var result = new Dictionary<string, AuthorizationPolicy>
            {
                {
                    ContentEditor,
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireAssertion(c => ContentEditorHandler(c)).Build()
                },
                {
                    UserManagement,
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireAssertion(c => UserManagementHandler(c)).Build()
                },
                {
                    Backoffice,
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireAssertion(c => BackofficeHandler(c)).Build()
                }
            };
            return result;
        }

        private static bool ContentEditorHandler(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var isAdmin = authorizationHandlerContext.User.IsInAnyRole(SystemRoles.Admin.ToString(), SystemRoles.Developer.ToString());
            if (isAdmin) return true;
            var isEditor = authorizationHandlerContext.User.IsInAnyRole(CmsRoles.Author.ToString(), CmsRoles.Editor.ToString()) &&
                           authorizationHandlerContext.User.IsInRole(SystemRoles.Writer.ToString());
            return isEditor;
        }

        private static bool UserManagementHandler(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var isAdmin = authorizationHandlerContext.User.IsInAnyRole(SystemRoles.Admin.ToString(), SystemRoles.Developer.ToString());
            if (isAdmin) return true;
            var isEditor = authorizationHandlerContext.User.IsInAnyRole(CmsRoles.Manager.ToString());
            return isEditor;
        }

        private static bool BackofficeHandler(AuthorizationHandlerContext authorizationHandlerContext)
        {
            var isAdmin = authorizationHandlerContext.User.IsInAnyRole(SystemRoles.Admin.ToString(), SystemRoles.Developer.ToString());
            if (isAdmin) return true;
            var isBackoffice = authorizationHandlerContext.User.IsInAnyRole(CmsRoles.Backoffice.ToString());
            return isBackoffice;
        }
    }
}