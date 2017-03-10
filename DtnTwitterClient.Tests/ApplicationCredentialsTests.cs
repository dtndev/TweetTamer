//-----------------------------------------------------------------------
// <copyright file="ApplicationCredentialsTests.cs" company="dtndev">
//     Copyright Ian Dixon
// </copyright>
//-----------------------------------------------------------------------
namespace DtnTwitterClient.Tests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class ApplicationCredentialsTests
    {
        [Fact]
        public void EncodedValue()
        {
            string consumerKey = "h87h87hHJJ8ksi8HY7jJIJ9Kd";
            string consumerSecret = "dj7dH7Hhhjj76HtG6H8uhT68HHHD67uH6J9jjmmkJJ97VD34Bq";

            var credentials = new ApplicationCredentials(consumerKey, consumerSecret);
            credentials.EncodedValue.Should().Be("aDg3aDg3aEhKSjhrc2k4SFk3akpJSjlLZDpkajdkSDdIaGhqajc2SHRHNkg4dWhUNjhISEhENjd1SDZKOWpqbW1rSko5N1ZEMzRCcQ==");
        }

        [Fact]
        public void NullKeyThrowsArgumentNullException()
        {
            Action constructor = () => new ApplicationCredentials(null, "secret");

            constructor.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void NullSecretThrowsArgumentNullException()
        {
            Action constructor = () => new ApplicationCredentials("key", null);

            constructor.ShouldThrow<ArgumentNullException>();
        }
    }
}
