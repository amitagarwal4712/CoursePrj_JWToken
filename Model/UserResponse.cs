namespace StudentListAPI.Model
{
    public class UserResponse
    {
        public int userid { get; set; }
        public required string username { get; set; }
        public required string usersecret { get; set; }
        public bool isACtive { get; set; }
    }
}
