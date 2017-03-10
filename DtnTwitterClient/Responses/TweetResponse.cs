//-----------------------------------------------------------------------
// <copyright file="TweetResponse.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient.Responses
{
    using System.Diagnostics.CodeAnalysis;

    internal class TweetResponse
    {
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public string id_str { get; set; }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public string text { get; set; }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public Entities entities { get; set; }
    }
}
