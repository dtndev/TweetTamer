namespace DtnTwitterClient
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public abstract class TwitterRequest<TResult>
    {
        protected internal abstract Task<HttpRequestMessage> CreateHttpRequest(TwitterClient twitter);

        protected internal abstract Task<TResult> ProcessResponse(HttpResponseMessage response);
    }
}
