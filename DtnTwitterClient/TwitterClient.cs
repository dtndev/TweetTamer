//-----------------------------------------------------------------------
// <copyright file="TwitterClient.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public class TwitterClient
    {
        private readonly HttpClient httpClient;
        private readonly ApplicationCredentials credentials;

        private BearerToken bearerToken;

        public TwitterClient(ApplicationCredentials credentials)
            : this(CreateHttpClient(),  credentials)
        {
        }

        internal TwitterClient(HttpClient httpClient, ApplicationCredentials credentials)
        {
            this.httpClient = httpClient;
            this.credentials = credentials;
        }

        public async Task<TResult> SendAsync<TResult>(TwitterRequest<TResult> request)
        {
            HttpRequestMessage httpRequest = await request.CreateHttpRequest(this);
            HttpResponseMessage httpResponse = await this.httpClient.SendAsync(httpRequest);
            return await request.ProcessResponse(httpResponse);
        }

        internal async Task<BearerToken> CurrentBearerToken()
        {
            if (this.bearerToken == null)
            {
                this.bearerToken = await this.GetNewBearerToken();
            }

            return this.bearerToken;
        }

        private static HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            return client;
        }

        private async Task<BearerToken> GetNewBearerToken()
        {
            var tokenRequest = new BearerTokenRequest(this.credentials, this.httpClient);
            return await tokenRequest.SendAsync();
        }
    }
}
