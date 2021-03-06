﻿//-----------------------------------------------------------------------
// <copyright file="Urls.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient.Responses
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal class Urls
    {
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Must match Twitter response for deserialization")]
        public List<Url> urls { get; set; }
    }
}
