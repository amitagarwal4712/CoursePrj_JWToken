namespace StudentListAPI.Model
{
    //Error model used to return generated exceptions or errors
    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string Error { get; set; }

    }
}
