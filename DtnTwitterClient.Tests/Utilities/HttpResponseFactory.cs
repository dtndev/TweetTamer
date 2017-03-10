//-----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient.Tests.Utilities
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    internal static class HttpResponseFactory
    {
        internal const string DummyBearerTokenValue = "y88t589yg654";

        private const string JsonMediaType = "application/json";

        internal static Task<HttpResponseMessage> CreateHttpResponseTaskFromJsonString(string json)
        {
            var message = new HttpResponseMessage { Content = new StringContent(json, new UTF8Encoding(), JsonMediaType) };

            return Task.FromResult(message);
        }

        internal static Task<HttpResponseMessage> CreateBearerTokenHttpResponseTask(string tokenType, string tokenValue)
        {
            string json = $@"{{""token_type"":""{tokenType}"",""access_token"":""{tokenValue}""}}";

            return CreateHttpResponseTaskFromJsonString(json);
        }

        internal static Task<HttpResponseMessage> CreateDummyBearerTokenHttpResponseTask()
        {
            return CreateBearerTokenHttpResponseTask("bearer", DummyBearerTokenValue);
        }
    }
}
