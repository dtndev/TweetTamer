//-----------------------------------------------------------------------
// <copyright file="BearerToken.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient
{
    internal class BearerToken
    {
        internal BearerToken(string value)
        {
            this.Value = value;
        }

        internal string Value
        {
            get;
        }
    }
}
