using UniversityExamSimulation.Core.Models;

namespace UniversityExamSimulation.Core.Services
{
    public interface IUniversityService
    {
        List<University> GetUniversities();
        void DeleteUniversities();

        List<Student> GetStudentsByUniversityId(int universityId);
    }
}
