//-----------------------------------------------------------------------
// <copyright file="BearerTokenRequest.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Responses;

    internal class BearerTokenRequest
    {
        private static readonly Uri TokenUri = new Uri("https://api.twitter.com/oauth2/token");

        private readonly ApplicationCredentials credentials;
        private readonly HttpClient httpClient;

        internal BearerTokenRequest(ApplicationCredentials credentials, HttpClient httpClient)
        {
            if (credentials == null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            this.credentials = credentials;
            this.httpClient = httpClient;
        }

        internal HttpRequestMessage HttpRequest
        {
            get
            {
                HttpRequestMessage request = this.CreateRequest();
                this.AddAuthorizationHeader(request);
                SetRequestBody(request);
                return request;
            }
        }

        internal async Task<BearerToken> SendAsync()
        {
            var response = await this.httpClient.SendAsync(this.HttpRequest);
            var tokenResponse = await response.Content.ReadAsAsync<BearerTokenResponse>();

            if (tokenResponse.token_type != "bearer")
            {
                throw new InvalidOperationException($"Unexpected token_type {tokenResponse.token_type}");
            }

            return new BearerToken(tokenResponse.access_token);
        }

        private static void SetRequestBody(HttpRequestMessage request)
        {
            HttpContent payload = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });
            payload.Headers.Clear();
            payload.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");

            request.Content = payload;
        }

        private HttpRequestMessage CreateRequest()
        {
            return new HttpRequestMessage(HttpMethod.Post, TokenUri);
        }

        private void AddAuthorizationHeader(HttpRequestMessage request)
        {
            string authorizationHeaderValue = string.Format("Basic {0}", this.credentials.EncodedValue);
            request.Headers.Add("Authorization", authorizationHeaderValue);
        }
    }
}
