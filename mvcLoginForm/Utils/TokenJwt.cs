using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace mvcLoginForm.Utils
{
    public class TokenJwt
    {
        private readonly IConfiguration _config;
        public TokenJwt(IConfiguration config)
        {
            _config = config;
        }
        public string JwtToken(List<Claim> c)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtConfig = new JwtSecurityToken(
                claims: c,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
