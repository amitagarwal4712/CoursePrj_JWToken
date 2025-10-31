namespace StudentListAPI.Model
{
    //used to perform get student list for add/delete/view student list
    public class StudentModel
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }


    }
}
