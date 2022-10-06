using UniversityExamSimulation.Core.Models;

namespace UniversityExamSimulation.Core.Repositories
{
    public interface IUniversityRepository
    {
        List<University> GetUniversities();
        void DeleteUniversities();
    }
}
