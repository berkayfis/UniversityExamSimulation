using Microsoft.AspNetCore.Mvc;
using UniversityExamSimulation.Core.Services;

namespace UniversityExamSimulation.Controllers
{
    [ApiController]
    [Route("universities")]
    public class UniversityController : Controller
    {
        private IUniversityService universityService;

        public UniversityController(IUniversityService universityService)
        {
            this.universityService = universityService;
        }
        [HttpGet]
        public IActionResult GetUniversities()
        {
            var universities = universityService.GetUniversities();
            if (universities.Count == default(int)) return NoContent();
            return Ok(universities);
        }

        [HttpDelete]
        public IActionResult DeleteUniversities()
        {
            universityService.DeleteUniversities();
            return Ok("Deleted");
        }
    }
}
