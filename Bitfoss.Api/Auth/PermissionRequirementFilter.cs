using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Bitfoss.Api.Services;

namespace Bitfoss.Api.Auth
{
    public class PermissionRequirementFilter : IAuthorizationFilter
    {
        private readonly ILogger<PermissionRequirementFilter> _logger;

        private readonly IApiClientService _apiClientService;

        private readonly string _requiredPermission;

        public PermissionRequirementFilter(
            ILogger<PermissionRequirementFilter> logger,
            IApiClientService apiClientService,
            string requiredPermission)
        {
            _logger = logger;
            _apiClientService = apiClientService;
            _requiredPermission = requiredPermission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthorized = false;

            try
            {
                var apiKey = GetApiKey(context);
                var client = _apiClientService.GetApiClient(apiKey);
                isAuthorized = client.HasPermission(_requiredPermission);

                if (!isAuthorized)
                {
                    _logger.LogWarning($"{client.Name} does not have '{_requiredPermission}' permission");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed during authorization");
            }

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedObjectResult("unauthorized");
            }
        }

        private string GetApiKey(AuthorizationFilterContext context)
        {
            var apiKey = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("Api key not in 'Authorization' header");
            }

            return apiKey;
        }
    }
}
