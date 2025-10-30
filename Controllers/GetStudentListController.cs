using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentListAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetStudentListController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        public IActionResult GetStudentList()
        {
            return Ok(new { message = "You are authorized!" });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddStudent()
        {
            return Ok(new { message = "You are authorized!" });
        }

    }
}