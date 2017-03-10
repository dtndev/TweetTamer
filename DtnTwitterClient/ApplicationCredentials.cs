//-----------------------------------------------------------------------
// <copyright file="ApplicationCredentials.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient
{
    using System;
    using System.Web;

    public class ApplicationCredentials
    {
        private readonly string consumerKey;
        private readonly string consumerSecret;

        public ApplicationCredentials(string consumerKey, string consumerSecret)
        {
            if (consumerKey == null)
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }

            if (consumerSecret == null)
            {
                throw new ArgumentNullException(nameof(consumerSecret));
            }

            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        public string EncodedValue
        {
            get
            {
                string bearerTokenCredential = string.Format(
                    "{0}:{1}", 
                    Rfc1738Encode(this.consumerKey), 
                    Rfc1738Encode(this.consumerSecret));

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(bearerTokenCredential);
                string encodedBearerTokenCredential = Convert.ToBase64String(bytes);
                return encodedBearerTokenCredential;
            }
        }

        private static string Rfc1738Encode(string key)
        {
            return HttpUtility.UrlEncode(key);
        }
    }
}
