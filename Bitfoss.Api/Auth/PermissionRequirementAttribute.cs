using Microsoft.AspNetCore.Mvc;

namespace Bitfoss.Api.Auth
{
    public class PermissionRequirementAttribute : TypeFilterAttribute
    {
        public PermissionRequirementAttribute(string requiredPermission)
            : base(typeof(PermissionRequirementFilter))
        {
            Arguments = new object[] { requiredPermission };
        }
    }
}
