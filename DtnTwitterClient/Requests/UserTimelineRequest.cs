//-----------------------------------------------------------------------
// <copyright file="UserTimelineRequest.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Responses;

    /// <summary>
    /// A request for tweets created by the specified user.
    /// </summary>
    public sealed class UserTimelineRequest : TwitterRequest<IEnumerable<Tweet>>
    {
        private static readonly Uri UserTimelineUri = new Uri("https://api.twitter.com/1.1/statuses/user_timeline.json");

        public UserTimelineRequest(string username)
        { 
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            this.Username = username;
        }

        public string Username { get; }

        protected internal override async Task<HttpRequestMessage> CreateHttpRequest(TwitterClient twitter)
        { 
            HttpRequestMessage request = this.CreateRequest();
            await this.AddAuthorizationHeader(request, twitter);

            return request;
        }

        protected internal override async Task<IEnumerable<Tweet>> ProcessResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var rawTweets = JsonConvert.DeserializeObject<List<TweetResponse>>(json);

            var tweets = new List<Tweet>();
            foreach (var rawTweet in rawTweets)
            {
                tweets.Add(Tweet.CreateFromResponse(rawTweet));
            }

            return tweets;
        }

        private HttpRequestMessage CreateRequest()
        {
            Uri tweetsUri = this.GetParameterisedUri();
            return new HttpRequestMessage(HttpMethod.Get, tweetsUri);
        }

        private Uri GetParameterisedUri()
        {
            string fullUriString =
                string.Format("{0}?screen_name={1}&count=20", UserTimelineUri.AbsoluteUri, this.Username);

            return new Uri(fullUriString);
        }

        private async Task AddAuthorizationHeader(HttpRequestMessage request, TwitterClient twitter)
        {
            BearerToken token = await twitter.CurrentBearerToken();
            string authorizationHeaderValue = string.Format("Bearer {0}", token.Value);
            request.Headers.Add("Authorization", authorizationHeaderValue);
        }
    }
}