using Microsoft.IdentityModel.Tokens;
using PocketWallet.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PocketWallet.Helpers
{
    public static class TokenHelper
    {
        public static string GetToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.GivenName, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44323",
                audience: "https://localhost:44323",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signinCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
