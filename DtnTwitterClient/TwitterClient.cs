//-----------------------------------------------------------------------
// <copyright file="TwitterClient.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient
{
    using System.Collections.Generic;

    /// <summary>
    /// Coordinates all interactions with the Twitter platform.
    /// </summary>
    public class TwitterClient
    {
        /// <summary>
        /// Request all tweets matching the supplied request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>All tweets matching the supplied request, or empty if there are no matches.</returns>
        public IEnumerable<Tweet> RequestTweets(UserTimelineRequest request)
        {
            return new List<Tweet>();
        }
    }
}
