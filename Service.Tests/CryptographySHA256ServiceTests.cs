using AspNetCoreApiSample.Service.Services;
using System;
using Xunit;

namespace Service.Tests
{
    public class CryptographySHA256ServiceTests
    {
        public class Encrypt
        {
            [Theory]
            [InlineData("value exemplo", "2b306108faf03992583ef113ddc34f5c3828975faca813d88753c9fe14ce6bb0")]
            [InlineData("texto com uma quantidade maior de caracteres", "3cb6dd486077f33e822c3206829357980ef3546ddfd9502b4f22b8e66e142f23")]
            [InlineData("single", "bb63ddc7b43e271025b7ab9a1037678fafd31663d68d02bc707a44513ed6e964")]
            public void InputValue_ExpectedHash(string value, string encryptedExpected)
            {
                // Arrange
                CryptographySHA256Service service = new CryptographySHA256Service();

                // Act
                string encryptedValue = service.Encrypt(value);

                // Assert
                Assert.Equal(encryptedExpected, encryptedValue);
            }
        }
    }
}
