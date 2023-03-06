using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Domain;

using Microsoft.IdentityModel.Tokens;


namespace API.TokenService
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService( IConfiguration config)
        {
             _config = config;
        }
        public string CreateToken(Usuario user)
        {
            var jwt = _config.GetSection("Jwt").Get<Jwt>();
                
                var claims = new []
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("id", user.Id),
                    new Claim("userName", user.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddDays(10),
                    signingCredentials: signIn
                );

                var tokenSTRING = new JwtSecurityTokenHandler().WriteToken(token);
                return tokenSTRING;
        }
    }
}