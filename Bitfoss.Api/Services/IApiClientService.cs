using Bitfoss.Api.Auth;

namespace Bitfoss.Api.Services
{
    public interface IApiClientService
    {
        ApiClient GetApiClient(string apiKey);
    }
}