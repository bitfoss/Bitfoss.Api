using System.Collections.Generic;
using Bitfoss.Api.Auth;

namespace Bitfoss.Api.Models.Options
{
    public class ApiClientsOptions
    {
        public IEnumerable<ApiClient> ApiClients { get; set; }
    }
}