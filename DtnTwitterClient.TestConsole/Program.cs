namespace DtnTwitterClient.TestConsole
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            string consumerKey = null; // BLANK OUT FOR CHECK IN!!!
            string consumerSecret = null; // BLANK OUT FOR CHECK IN!!!

            var credentials = new ApplicationCredentials(consumerKey, consumerSecret);
            var twitter = new TwitterClient(credentials);

            DisplayRecentTweetsBy("dtndev", twitter);
            DisplayRecentTweetsBy("johnrentoul", twitter);

            Console.ReadLine();
        }

        private static void DisplayRecentTweetsBy(string username, TwitterClient twitter)
        {
            Console.WriteLine("User: " + username);
            Console.WriteLine();
            var userTimelineRequest = new UserTimelineRequest(username);
            var tweets = twitter.SendAsync(userTimelineRequest).Result;

            foreach (Tweet tweet in tweets)
            {
                Console.WriteLine("Tweet ID: " + tweet.Id);
                Console.WriteLine(tweet.Text);
                if (tweet.ContainsUrls)
                {
                    Console.WriteLine("Urls");
                    foreach (var url in tweet.Urls)
                    {
                        Console.WriteLine(url);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("--------------------------------------");
            Console.WriteLine();
        }
    }
}
