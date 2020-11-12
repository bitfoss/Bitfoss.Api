using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Bitfoss.Api.Auth;
using Bitfoss.Api.Models.Options;

namespace Bitfoss.Api.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly ApiClientsOptions _options;

        public ApiClientService(IOptions<ApiClientsOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _ = _options.ApiClients ?? throw new ArgumentNullException(nameof(_options.ApiClients));
        }

        public ApiClient GetApiClient(string apiKey)
        {
            return _options.ApiClients.FirstOrDefault(client => client.Key == apiKey)
                ?? throw new Exception("Unknown api key");
        }
    }
}