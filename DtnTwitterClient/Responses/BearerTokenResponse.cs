//-----------------------------------------------------------------------
// <copyright file="BearerTokenResponse.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient.Responses
{
    using System.Diagnostics.CodeAnalysis;

    internal class BearerTokenResponse
    {
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public string token_type { get; set; }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public string access_token { get; set; }
    }
}
