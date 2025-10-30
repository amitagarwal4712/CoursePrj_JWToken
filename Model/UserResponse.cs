namespace StudentListAPI.Model
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string UserSecret { get; set; }
        public required string UserRole { get; set; }
        public bool isActive { get; set; }
    }
}
