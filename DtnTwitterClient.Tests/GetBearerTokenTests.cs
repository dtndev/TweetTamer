//-----------------------------------------------------------------------
// <copyright file="BearerTokenTests.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient.Tests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;
    using Utilities;
    using Xunit;

    public class GetBearerTokenTests
    {
        private const string Key = "key";
        private const string Secret = "secret";

        [Fact]
        public async Task HttpRequestContainsCorrectAuthorizationHeader()
        {
            Action<HttpRequestMessage> requestAssertions =
                r =>
                {
                    r.Headers.Contains("Authorization").Should().BeTrue();
                    r.Headers.GetValues("Authorization").First().Should().Be("Basic a2V5OnNlY3JldA==");
                };

            var handler = new AssertingBearerTokenMessageHandler(requestAssertions);
            BearerTokenRequest request = CreateRequest(handler);

            await request.SendAsync();
        }

        [Fact]
        public async Task HttpRequestContainsCorrectPayload()
        {
            Action<HttpRequestMessage> requestAssertions =
                async r =>
                {
                    r.Content.Headers.Contains("Content-Type").Should().BeTrue();
                    r.Content.Headers.GetValues("Content-Type").First().Should().Be("application/x-www-form-urlencoded; charset=UTF-8");

                    var formContent = r.Content as FormUrlEncodedContent;
                    formContent.Should().NotBeNull();
                    var data = await formContent.ReadAsFormDataAsync();
                    data["grant_type"].Should().Be("client_credentials");
                };

            var handler = new AssertingBearerTokenMessageHandler(requestAssertions);
            BearerTokenRequest request = CreateRequest(handler);

            await request.SendAsync();
        }

        [Fact]
        public async Task ValidResponseIsCorrectlyParsed()
        {
            string tokenType = "bearer";
            string tokenValue = "ty894u89u89tu389ut";

            var handler = new DummyMessageHandler(tokenType, tokenValue);
            BearerTokenRequest request = CreateRequest(handler);

            BearerToken token = await request.SendAsync();

            token.Value.Should().Be(tokenValue);
        }

        [Fact]
        public void InvalidTokenResponseCausesException()
        {
            string tokenType = "invalid";
            string tokenValue = "b340u90u2690uu5";

            var handler = new DummyMessageHandler(tokenType, tokenValue);
            BearerTokenRequest request = CreateRequest(handler);

            Func<Task> send = async () => await request.SendAsync();

            send.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void NullCredentialsThrowsArgumentNullException()
        {
            Action constructor = () => new BearerTokenRequest(null, new HttpClient());

            constructor.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void NullHttpClientThrowsArgumentNullException()
        {
            Action constructor = () => new BearerTokenRequest(new ApplicationCredentials("key", "secret"), null);

            constructor.ShouldThrow<ArgumentNullException>();
        }

        private static BearerTokenRequest CreateRequest(HttpMessageHandler httpMessageHandler)
        {
            var httpClient = new HttpClient(httpMessageHandler);
            var credentials = new ApplicationCredentials(Key, Secret);

            return new BearerTokenRequest(credentials, httpClient);
        }

        private class AssertingBearerTokenMessageHandler : AssertingMessageHandler
        {
            public AssertingBearerTokenMessageHandler(Action<HttpRequestMessage> requestAssertions) 
                : base(requestAssertions)
            {
            }

            internal override Task<HttpResponseMessage> CreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return HttpResponseFactory.CreateDummyBearerTokenHttpResponseTask();
            }
        }

        private class DummyMessageHandler : HttpMessageHandler
        {
            private readonly string tokenType;
            private readonly string tokenValue;

            public DummyMessageHandler(string tokenType, string tokenValue)
            {
                this.tokenType = tokenType;
                this.tokenValue = tokenValue;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return HttpResponseFactory.CreateBearerTokenHttpResponseTask(this.tokenType, this.tokenValue);
            }
        }
    }
}