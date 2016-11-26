//-----------------------------------------------------------------------
// <copyright file="UserTimelineRequest.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient
{
    using System;

    /// <summary>
    /// A request for tweets created by the specified user.
    /// </summary>
    public class UserTimelineRequest
    {
        private readonly string username;

        private readonly DateTime startDate;

        private readonly DateTime endDate;

        private UserTimelineRequest(string username, DateTime startDate, DateTime endDate)
        {
            this.username = username;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public class Builder
        {
            public Builder(string username)
            {
                this.Username = username;
            }

            public string Username { get; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public UserTimelineRequest Build()
            {
                return new UserTimelineRequest(this.Username, this.StartDate, this.EndDate);
            }
        }
    }
}