//-----------------------------------------------------------------------
// <copyright file="Url.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient.Responses
{
    using System.Diagnostics.CodeAnalysis;

    internal class Url
    {
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public string url { get; set; }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public string display_url { get; set; }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public string expanded_url { get; set; }
    }
}
