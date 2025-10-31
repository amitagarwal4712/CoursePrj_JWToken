using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentListAPI.Service
{
    //This service is used to generate JWT token
    public class JWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");   //Getting JWT configuration from appsetting.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));  //Getting private KEY from configuration and then creating symmetricsecurity key for JWT token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//signing the credential for JWT with the key

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),       //adding Claim Subject, which is user name
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) //JTI-> JWT ID, adding into claim
            };

            //Adding all roles for the user (roles are fetched from data base for logged in user, and adding those roles into claim,
            //so that when yser pass JWT for futhre processing, role could be taken out from JWT token instead from DB
            foreach (var rl in role.Split(","))
            {
                claims.Add(new Claim(ClaimTypes.Role, rl));
            }

            //generating final token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
