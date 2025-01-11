using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers
{
    //https://localhost:portnumber/api/student
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //https://localhost:portnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] students = new string[]
            {
                "Tom", "Jerry", "Donald", "Mickey"
            };

            return Ok(students);
        }
    }
}
