using Microsoft.AspNetCore.Mvc;
using UniversityExamSimulation.Core.Services;

namespace UniversityExamSimulation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : Controller
    {
        private readonly IStudentService studentService;

        public StudentsController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = studentService.GetStudents();
            if (students.Count == default(int)) return NoContent();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = studentService.GetStudentById(id);
            if (student == null) return NoContent();
            return Ok(student);
        }

        [HttpDelete]
        public IActionResult DeleteStudents()
        {
            studentService.DeleteStudents();
            return Ok("Deleted");
        }

        [HttpPost]
        public IActionResult AddStudent([FromForm] string name, [FromForm]string surname)
        {
            var student = studentService.AddStudent(name, surname);
            return StatusCode(StatusCodes.Status201Created,student);
        }
    }
}
