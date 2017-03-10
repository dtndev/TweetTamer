//-----------------------------------------------------------------------
// <copyright file="Tweet.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Responses;

    /// <summary>
    /// Represents a single Tweet (status update) from Twitter.
    /// </summary>
    public class Tweet
    {
        private readonly List<Uri> urls = new List<Uri>(); 

        internal Tweet(long id, string text, IEnumerable<Uri> urls)
        {
            this.Id = id;
            this.Text = text;
            this.urls.AddRange(
                from url in urls
                select new Uri(url.ToString()));
        }

        public long Id { get; }

        public string Text { get; }

        public bool ContainsUrls
        {
            get
            {
                return this.urls.Count > 0;
            }
        }

        public IEnumerable<Uri> Urls
        {
            get
            {
                return from url in this.urls
                       select new Uri(url.ToString());
            }
        }

        internal static Tweet CreateFromResponse(TweetResponse rawTweet)
        {
            long id = long.Parse(rawTweet.id_str);
            var urls = ExtractUrls(rawTweet);

            return new Tweet(id, rawTweet.text, urls);
        }

        private static IEnumerable<Uri> ExtractUrls(TweetResponse rawTweet)
        {
            var urls = rawTweet?.entities?.urls;

            if (urls == null)
            {
                throw new ArgumentException($"Tweet with id {rawTweet.id_str} is missing urls element");
            }

            return 
                from url in urls
                select new Uri(url.expanded_url);
        }
    }
}