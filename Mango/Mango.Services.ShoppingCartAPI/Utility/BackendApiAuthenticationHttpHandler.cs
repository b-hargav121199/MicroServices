﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Utility
{
    public class BackendApiAuthenticationHttpHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        public BackendApiAuthenticationHttpHandler(IHttpContextAccessor accessor)
        {
                _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _accessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
