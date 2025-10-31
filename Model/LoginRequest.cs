using System.ComponentModel.DataAnnotations;

namespace StudentListAPI.Model
{
    //Used to login request when user made Login for auth controller
    public class LoginRequest
    {
        public required string UserName { get; set; }
        
        public required string Password { get; set; }
    }
}
