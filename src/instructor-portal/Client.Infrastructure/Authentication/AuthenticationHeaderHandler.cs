using Blazored.LocalStorage;
using Application.Common.Models;
using Client.Infrastructure.Constants;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Infrastructure.Authentication
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthenticationHeaderHandler(ILocalStorageService localStorage)
            => this._localStorage = localStorage;

        protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                var savedToken = await this._localStorage.GetItemAsync<AuthTokenHandler>(StorageConstants.Local.AuthTokenHandler);

                if (savedToken != null && savedToken.IsValid())
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken.Token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}