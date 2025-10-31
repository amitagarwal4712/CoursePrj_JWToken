using Microsoft.AspNetCore.Mvc;
using StudentListAPI.Service;
using StudentListAPI.Model;
using Microsoft.Data.SqlClient;
using Dapper;

namespace StudentListAPI.Controllers
{
    //Controller used to authnticate the user and to return JWT token to authenticatred user
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JWTService _jwtService;

        //In construcror, creating object of JWTservice to generate JWT token if user ger authenticated  
        public AuthController(JWTService jwtService)
        {
            _jwtService = jwtService;
        }

        //User called this Login method to get autrhenitcated and get JWT token in response
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Model.LoginRequest request)
        {
            // Validating user details
            var user = await ValidateUserAsync(request.UserName, request.Password);

            if (user == null)   //IF user details are not in Database or password not matched
                return Ok(new ErrorResponse() { ErrorCode = 401 , Error = "Invalid username or password."});

            //Generating JWT Token
            var token = _jwtService.GenerateToken(request.UserName, user.UserRole);
            return Ok(new LoginResponse() { token = token });
        }

        private async Task<UserResponse?> ValidateUserAsync(string username, string password)
        {
            //Fetching user entry from DB
            using var connection = new SqlConnection(Environment.GetEnvironmentVariable("SQLServerConnectionString"));
            var user = await connection.QuerySingleOrDefaultAsync<UserResponse>("SELECT UserName, UserSecret,UserRole, case isactive when 'Y' then 'true' else 'false' end as isactive FROM [users] WHERE username = @Username", new { Username = username });

            if (user == null)
                return null;

            //Hash on user provided password and comaring with DB stored hashed password
            //created new hashed method - GenerateHashfor128Bit without using anylibrary to hash any value
            bool valid = string.Equals(Utility.GenerateHashfor128Bit(password).ToString(), user.UserSecret);
            return valid ? user : null;
        }
    }
}
