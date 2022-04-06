using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Application.Common.Models;
using Client.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Infrastructure.Authentication
{
    public class AppStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AppStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(token), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public ClaimsPrincipal AuthenticationStateUser { get; set; }
        public ClaimsIdentity AuthenticatedUserClaims { get; set; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedTokenHandler = await _localStorage.GetItemAsync<AuthTokenHandler>(StorageConstants.Local.AuthTokenHandler);
            if (savedTokenHandler == null || !savedTokenHandler.IsValid())
            {
                AuthenticatedUserClaims = new ClaimsIdentity();
                AuthenticationStateUser = new ClaimsPrincipal(AuthenticatedUserClaims);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedTokenHandler.Token);
            var claimsIdentity = new ClaimsIdentity(GetClaimsFromJwt(savedTokenHandler.Token), "jwt");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var state = new AuthenticationState(claimsPrincipal);
            AuthenticatedUserClaims = claimsIdentity;
            AuthenticationStateUser = state.User;
            return state;
        }

        private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

                if (roles != null)
                {
                    if (roles.ToString().Trim().StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                        claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                    }

                    keyValuePairs.Remove(ClaimTypes.Role);
                }

                keyValuePairs.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);
                if (permissions != null)
                {
                    if (permissions.ToString().Trim().StartsWith("["))
                    {
                        var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());
                        claims.AddRange(parsedPermissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission)));
                    }
                    else
                    {
                        claims.Add(new Claim(ApplicationClaimTypes.Permission, permissions.ToString()));
                    }
                    keyValuePairs.Remove(ApplicationClaimTypes.Permission);
                }

                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}