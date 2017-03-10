//-----------------------------------------------------------------------
// <copyright file="AssertingMessageHandler.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient.Tests.Utilities
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Would prefer to use Decorator pattern with this taking an inner handler but HttpMessageHandler isn't an interface and SendAsync isn't public so have had to use inheritance instead
    /// </summary>
    internal abstract class AssertingMessageHandler : HttpMessageHandler
    {
        private readonly Action<HttpRequestMessage> requestAssertions;

        public AssertingMessageHandler(Action<HttpRequestMessage> requestAssertions)
        {
            this.requestAssertions = requestAssertions;
        }

        internal abstract Task<HttpResponseMessage> CreateResponse(HttpRequestMessage request, CancellationToken cancellationToken);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.requestAssertions(request);

            return this.CreateResponse(request, cancellationToken);
        }
    }
}
