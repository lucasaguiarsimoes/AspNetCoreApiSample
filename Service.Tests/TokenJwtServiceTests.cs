using AspNetCoreApiSample.Service.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Service.Tests
{
    public class TokenJwtServiceTests
    {
        public class CreateToken
        {
            [Fact]
            public void EmptyClaims_ValidTokenExpected()
            {
                // Arrange
                TokenJwtService service = new TokenJwtService();
                List<Claim> claims = new List<Claim>();

                // Act
                string createdToken = service.CreateToken(claims);

                // Assert
                string expectedToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MjE4MjczMjYsImlzcyI6ImFzcG5ldGNvcmVhcGktc2FtcGxlLWFwaSIsImF1ZCI6ImFzcG5ldGNvcmVhcGktc2FtcGxlLWNsaWVudCJ9.l1Vcd8jjlXtmxo4q-sL7UYY5OL-1PWR8pUmd_ZUYB5g";
                AssertTokenJwt(expectedToken, createdToken);
            }

            [Fact]
            public void SomeClaims_ValidTokenExpected()
            {
                // Arrange
                TokenJwtService service = new TokenJwtService();
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "admin"),
                    new Claim(JwtRegisteredClaimNames.Jti, "9461E259-301C-43FC-8356-F39F82D6FF9C"),
                };

                // Act
                string createdToken = service.CreateToken(claims);

                // Assert
                string expectedToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImp0aSI6Ijk0NjFFMjU5LTMwMUMtNDNGQy04MzU2LUYzOUY4MkQ2RkY5QyIsImV4cCI6MTYyMTgyNzMwMCwiaXNzIjoiYXNwbmV0Y29yZWFwaS1zYW1wbGUtYXBpIiwiYXVkIjoiYXNwbmV0Y29yZWFwaS1zYW1wbGUtY2xpZW50In0.YjuCi7Dr6j8R13foyxTsu2-1oMLVOIMzRK3QJcMInv8";
                AssertTokenJwt(expectedToken, createdToken);
            }

            /// <summary>
            /// Valida ao menos o header do token esperado, já que a cada vez que o token é gerado, alguns dados são sempre diferentes
            /// </summary>
            private void AssertTokenJwt(string expectedToken, string createdToken)
            {
                // Quebra as partes do token
                string[] expectedSplittedToken = expectedToken.Split('.');
                string[] createdSplittedToken = createdToken.Split('.');

                // Garante que todos possuem as mesmas partes, que devem ser 3 (Header, Payload, Signature)
                Assert.Equal(expectedSplittedToken.Length, createdSplittedToken.Length);
                Assert.Equal(3, createdSplittedToken.Length);

                // Valida o header [0] do token apenas
                Assert.Equal(expectedSplittedToken[0], createdSplittedToken[0]);
            }
        }
    }
}
