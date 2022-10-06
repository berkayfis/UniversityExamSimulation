using UniversityExamSimulation.Core.Models;

namespace UniversityExamSimulation.Core.Services
{
    public interface IStudentService
    {
        List<Student> GetStudents();
        Student GetStudentById(int id);
        void DeleteStudents();
        Student AddStudent(string name, string surname);
    }
}
