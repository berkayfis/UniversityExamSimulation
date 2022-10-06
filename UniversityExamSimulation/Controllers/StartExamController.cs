using Microsoft.AspNetCore.Mvc;
using UniversityExamSimulation.Core.Services;

namespace UniversityExamSimulation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StartExamController : Controller
    {
        private readonly IStartExamService startExamService;
        public StartExamController(IStartExamService startExamService)
        {
            this.startExamService = startExamService;
        }

        [HttpGet]
        public IActionResult StartExam(DateTime examDate)
        {
            if (examDate == DateTime.MinValue) return BadRequest();
            var students = startExamService.GetExamResult(examDate);
            if (students == null) return BadRequest();
            if (students.Count == default(int)) return NoContent();
            return Ok(students);
        }
    }
}
