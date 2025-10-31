using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentListAPI.Model;
using System.Linq;

namespace StudentListAPI.Controllers
{
    //This controller is used to perform Student related operation like VIEW, INSERT, ADD
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        //GET request to get list of All students based on Roles and JWT token validation
        //IF JWT validation get failed, 401 error returned with error Please log in to access this resource
        //IF ROLE validation get failed, 403 error returned with error Not authorized to perform this action
        [Authorize(Roles = "Admin, Student, User")] //Each operation is authrozed using JWT token and assigned role. User With only mentioned role can get successful response
        [HttpGet("StudentList")]
        public async Task<IActionResult> GetStudentList()
        {
            var studentList = await StudentListAsync();  //calls private method to get list from database
            return Ok(studentList);
        }

        //POST request to add new student and get list of All students based on Roles and JWT token validation
        //IF JWT validation get failed, 401 error returned with error Please log in to access this resource
        //IF ROLE validation get failed, 403 error returned with error Not authorized to perform this action
        [Authorize(Roles = "Admin, User")]  //Each operation is authrozed using JWT token and assigned role. User With only mentioned role can get successful response
        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent(StudentModel request)
        {
            var message = InsertStudent(request);   //calls private method to add new student into database

            var studentList = await StudentListAsync();   //calls private method to get list from database
            return Ok(studentList);
        }

        //POST request to DELETE existing student and get list of All students based on Roles and JWT token validation
        //IF JWT validation get failed, 401 error returned with error Please log in to access this resource
        //IF ROLE validation get failed, 403 error returned with error Not authorized to perform this action
        [Authorize(Roles = "Admin, User")]  //Each operation is authrozed using JWT token and assigned role. User With only mentioned role can get successful response
        [HttpPost("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var message = DeleteStudentData(studentId); //calls private method to delete student from database

            var studentList = await StudentListAsync(); //calls private method to get list from database
            return Ok(studentList);
        }


        //Method called from controller method to fetch student list
        private async Task<List<StudentModel>?> StudentListAsync()
        {
            using var connection = new SqlConnection(Environment.GetEnvironmentVariable("SQLServerConnectionString"));
            var studentList = await connection.QueryAsync<StudentModel>("SELECT [StudentId],[FirstName],[MiddleName],[LastName],[EmailAddress],[PhoneNumber] FROM [students]");
            return studentList.AsList();
        }

        //Method called from controller method to add new student
        private string InsertStudent(StudentModel request)
        {
            //Used parameterized query to avoid SQL injection
            using var connection = new SqlConnection(Environment.GetEnvironmentVariable("SQLServerConnectionString"));
            var studentList = connection.Execute("INSERT INTO [STUDENTS] ([FirstName],[MiddleName],[LastName],[EmailAddress],[PhoneNumber]) VALUES (@FirstName,@MiddleName,@LastName,@EmailAddress,@PhoneNumber)"
                    , new { request.FirstName, request.MiddleName, request.LastName, request.EmailAddress, request.PhoneNumber });
            return studentList > 0 ? "Student Added Successfully" : "Issue in adding student record.";
        }

        //Method called from controller method to delete a student
        private string DeleteStudentData(int StudentID)
        {
            //Used parameterized query to avoid SQL injection
            using var connection = new SqlConnection(Environment.GetEnvironmentVariable("SQLServerConnectionString"));
            var studentList = connection.Execute("DELETE FROM [STUDENTS] WHERE StudentId = @StudentId"
                    , new { StudentID });


            return studentList > 0 ? "Student Deleted Successfully" : "Issue in Deleteing student record.";
        }
    }
}