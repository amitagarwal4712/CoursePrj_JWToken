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
    public class GetStudentListController : ControllerBase
    {

        [Authorize(Roles = " User")]
        [HttpGet("StudentList")]
        public async Task<IActionResult> GetStudentList()
        {
            var studentList = await StudentListAsync();
            return Ok(studentList);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult AddStudent()
        {
            

            return Ok(new { message = "You are authorized!" });
        }


        private async Task<List<StudentModel>?> StudentListAsync()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQLServerConnectionString");

            using var connection = new SqlConnection(connectionString);
            var studentList = await connection.QueryAsync<StudentModel>("SELECT [FirstName],[MiddleName],[LastName],[EmailAddress],[PhoneNumber] FROM [students]");

                   
            return studentList.AsList();
        }

    }
}