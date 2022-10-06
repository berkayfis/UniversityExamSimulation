using UniversityExamSimulation.Core.Models;

namespace UniversityExamSimulation.Core.Services
{
    public interface IStartExamService
    {
        List<Student> GetExamResult(DateTime examDate);
    }
}
