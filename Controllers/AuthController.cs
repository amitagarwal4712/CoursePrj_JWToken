using Microsoft.AspNetCore.Mvc;
using StudentListAPI.Service;
using StudentListAPI.Model;
using Microsoft.Data.SqlClient;
using Dapper;

namespace StudentListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JWTService _jwtService;

        public AuthController(JWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Model.LoginRequest request)
        {
            var s = Utility.PrepareHashfor128Bit(request.Password);

            // Validating user details
            var user = await ValidateUserAsync(request.UserName, request.Password);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            var token = _jwtService.GenerateToken(request.UserName, request.UserName.Equals("admin") ? "Admin" : "User");
            return Ok(new LoginResponse() { token = token });
        }

        private async Task<UserResponse?> ValidateUserAsync(string username, string password)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLServerConnectionString");

            using var connection = new SqlConnection(connectionString);
            var user = await connection.QuerySingleOrDefaultAsync<UserResponse>("SELECT username, usersecret, case isactive when 'Y' then 'true' else 'false' end as isactive FROM [user] WHERE username = @Username", new { Username = username });

            if (user == null)
                return null;

            // Verifying after hashing user input password with DB stored hashed password
            bool valid = string.Equals(Utility.PrepareHashfor128Bit(password).ToString(), user.usersecret);
            return valid ? user : null;
        }
    }
}
