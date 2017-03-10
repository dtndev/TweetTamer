namespace DtnTwitterClient.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;
    using Utilities;
    using Xunit;

    public class RetrieveUserTimelineTests
    {
        private const string ValidResponse = @"[{""id_str"":""123"",""text"":""A Tweet"",""entities"":{""urls"":[{""url"":""http://www.abc.com"",""display_url"":""http://www.abc.com"",""expanded_url"":""http://www.abc.com""}]}},
                               {""id_str"":""456"",""text"":""Another Tweet"",""entities"":{""urls"":[{""url"":""http://www.def.com"",""display_url"":""http://www.def.com"",""expanded_url"":""http://www.def.com""}]}}]";

        private const string ResponseWithEntitiesMissing = @"[{""id_str"":""123"",""text"":""A Tweet"",""entities"":{""urls"":[{""url"":""http://www.abc.com"",""display_url"":""http://www.abc.com"",""expanded_url"":""http://www.abc.com""}]}},
                               {""id_str"":""456"",""text"":""Another Tweet""}]";

        private const string ResponseWithUrlsMissing = @"[{""id_str"":""123"",""text"":""A Tweet"",""entities"":{""urls"":[{""url"":""http://www.abc.com"",""display_url"":""http://www.abc.com"",""expanded_url"":""http://www.abc.com""}]}},
                               {""id_str"":""456"",""text"":""Another Tweet"",""entities"":{}}]";

        [Fact]
        public async Task HttpRequestContainsCorrectAuthorizationHeader()
        {
            Action<HttpRequestMessage> requestAssertions =
                r =>
                {
                    r.Headers.Contains("Authorization").Should().BeTrue();
                    r.Headers.GetValues("Authorization").First().Should().Be($"Bearer {HttpResponseFactory.DummyBearerTokenValue}");
                };
            var handler = new AssertingUserTimelineMessageHandler(requestAssertions);
            TwitterClient twitter = CreateTwitterClient(handler);

            var request = new UserTimelineRequest("dtndev");

            await twitter.SendAsync(request);
        }

        [Fact]
        public async Task HttpRequestUriContainsUsername()
        {
            string username = "dtndev";
            Action<HttpRequestMessage> requestAssertions =
                r =>
                {
                    r.RequestUri.Query.Contains($"screen_name={username}").Should().BeTrue();
                };
            var handler = new AssertingUserTimelineMessageHandler(requestAssertions);
            TwitterClient twitter = CreateTwitterClient(handler);

            var request = new UserTimelineRequest(username);

            await twitter.SendAsync(request);
        }

        [Fact]
        public async Task ValidResponseIsParsedCorrectly()
        {
            TwitterClient twitter = CreateValidResponseTwitterClient();

            var request = new UserTimelineRequest("dtndev");
            IEnumerable<Tweet> tweets = await twitter.SendAsync(request);

            tweets.Count().Should().Be(2);

            tweets.First().Id.Should().Be(123);
            tweets.First().Text.Should().Be("A Tweet");
            tweets.First().ContainsUrls.Should().BeTrue();
            tweets.First().Urls.First().AbsoluteUri.Should().Be("http://www.abc.com/");

            tweets.Last().Id.Should().Be(456);
            tweets.Last().Text.Should().Be("Another Tweet");
            tweets.Last().ContainsUrls.Should().BeTrue();
            tweets.Last().Urls.First().AbsoluteUri.Should().Be("http://www.def.com/");
        }

        [Fact]
        public void ResponseMissingEntitiesCausesArgumentException()
        {
            TwitterClient twitter = CreateMissingEntitiesResponseTwitterClient();

            var request = new UserTimelineRequest("dtndev");

            Func<Task> send = async () => await twitter.SendAsync(request);

            send.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void ResponseMissingUrlsCausesArgumentException()
        {
            TwitterClient twitter = CreateMissingUrlsResponseTwitterClient();

            var request = new UserTimelineRequest("dtndev");

            Func<Task> send = async () => await twitter.SendAsync(request);

            send.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void NullUsernameThrowsArgumentNullException()
        {
            Action constructor = () => new UserTimelineRequest(null);

            constructor.ShouldThrow<ArgumentNullException>();
        }

        private static TwitterClient CreateValidResponseTwitterClient()
        {
            return CreateTwitterClient(new CannedResponseMessageHandler(ValidResponse));
        }

        private static TwitterClient CreateMissingEntitiesResponseTwitterClient()
        {
            return CreateTwitterClient(new CannedResponseMessageHandler(ResponseWithEntitiesMissing));
        }

        private static TwitterClient CreateMissingUrlsResponseTwitterClient()
        {
            return CreateTwitterClient(new CannedResponseMessageHandler(ResponseWithUrlsMissing));
        }

        private static TwitterClient CreateTwitterClient(HttpMessageHandler messageHandler)
        {
            var httpClient = new HttpClient(messageHandler);
            var credentials = new ApplicationCredentials("key", "secret");

            return new TwitterClient(httpClient, credentials);
        }

        private static Task<HttpResponseMessage> CreateResponseTask(HttpRequestMessage request, string response)
        {
            if (RequestIsForABearerToken(request))
            {
                return HttpResponseFactory.CreateDummyBearerTokenHttpResponseTask();
            }
            else
            {
                return HttpResponseFactory.CreateHttpResponseTaskFromJsonString(response);
            }
        }

        private static bool RequestIsForABearerToken(HttpRequestMessage request)
        {
            return request.RequestUri.AbsoluteUri.Contains("oauth2/token");
        }

        private class CannedResponseMessageHandler : HttpMessageHandler
        {
            private readonly string response;

            public CannedResponseMessageHandler(string response)
            {
                this.response = response;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return RetrieveUserTimelineTests.CreateResponseTask(request, this.response);
            }
        }

        private class AssertingUserTimelineMessageHandler : AssertingMessageHandler
        {
            public AssertingUserTimelineMessageHandler(Action<HttpRequestMessage> requestAssertions)
                : base(
                      r => 
                        {
                            if (!RetrieveUserTimelineTests.RequestIsForABearerToken(r))
                            {
                                requestAssertions(r);
                            }
                        })
            {
            }

            internal override Task<HttpResponseMessage> CreateResponse(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return RetrieveUserTimelineTests.CreateResponseTask(request, RetrieveUserTimelineTests.ValidResponse);
            }
        }
    }
}