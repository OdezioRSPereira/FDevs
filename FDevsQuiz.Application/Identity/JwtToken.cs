using FDevsQuiz.Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace FDevsQuiz.Application.Identity
{
    public static class JwtToken
    {
        public static string GenerateToken(IConfiguration configuration, AppContato contato)
        {
            var secret = configuration.GetValue<string>("secret");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, contato.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new Claim(ClaimTypes.Name, contato.Nome ?? string.Empty),
                new Claim(ClaimTypes.Surname, contato.SobreNome ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(contato.Codigo)),
                new Claim(ClaimTypes.Role, "ENTREVISTADO")
            };

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new GenericIdentity(contato.Email, "Auth"), claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
