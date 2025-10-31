using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentListAPI.Model;
using System.Linq;

namespace StudentListAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {

        [Authorize(Roles = "Admin, Student, User")]
        [HttpGet("StudentList")]
        public async Task<IActionResult> GetStudentList()
        {
            var studentList = await StudentListAsync();
            return Ok(studentList);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent(StudentModel request)
        {
            var message = InsertStudent(request);

            var studentList = await StudentListAsync();
            return Ok(studentList);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var message = DeleteStudentData(studentId);

            var studentList = await StudentListAsync();
            return Ok(studentList);
        }

        private async Task<List<StudentModel>?> StudentListAsync()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLServerConnectionString");

            using var connection = new SqlConnection(connectionString);
            var studentList = await connection.QueryAsync<StudentModel>("SELECT [StudentId],[FirstName],[MiddleName],[LastName],[EmailAddress],[PhoneNumber] FROM [students]");

                   
            return studentList.AsList();
        }

        private string InsertStudent(StudentModel request)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLServerConnectionString");

            using var connection = new SqlConnection(connectionString);
            var studentList = connection.Execute("INSERT INTO [STUDENTS] ([FirstName],[MiddleName],[LastName],[EmailAddress],[PhoneNumber]) VALUES (@FirstName,@MiddleName,@LastName,@EmailAddress,@PhoneNumber)"
                    , new { request.FirstName, request.MiddleName, request.LastName, request.EmailAddress, request.PhoneNumber });


            return studentList > 0 ? "Student Added Successfully" : "Issue in adding student record.";
        }

        private string DeleteStudentData(int StudentID)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLServerConnectionString");

            using var connection = new SqlConnection(connectionString);
            var studentList = connection.Execute("DELETE FROM [STUDENTS] WHERE StudentId = @StudentId"
                    , new { StudentID });


            return studentList > 0 ? "Student Deleted Successfully" : "Issue in Deleteing student record.";
        }

    }
}