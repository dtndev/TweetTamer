namespace DtnTwitterClient.Tests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class RetrieveUserTimelineTests
    {
        [Fact]
        public void DtndevUserTimelineOn20161126HasTwoKnownTweets()
        {
            DateTime startDate = new DateTime(2016, 11, 26);
            DateTime endDate = new DateTime(2016, 11, 27);
            var request = new UserTimelineRequest.Builder("dtndev")
            {
                StartDate = startDate,
                EndDate = endDate
            }.Build();

            var twitter = new TwitterClient();
            var tweets = twitter.RequestTweets(request);

            tweets.Count().Should().Be(2);
        }
    }
}
