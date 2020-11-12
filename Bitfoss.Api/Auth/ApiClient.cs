using System;
using System.Linq;
using System.Collections.Generic;

namespace Bitfoss.Api.Auth
{
    public class ApiClient
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public IEnumerable<string> Permissions { get; set; }

        public bool HasPermission(string permission)
        {
            _ = permission ?? throw new ArgumentNullException(nameof(permission));

            if (Permissions == default)
            {
                return false;
            }

            return Permissions.Contains(permission);
        }
    }
}
