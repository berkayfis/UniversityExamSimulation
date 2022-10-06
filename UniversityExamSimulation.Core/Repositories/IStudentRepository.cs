using UniversityExamSimulation.Core.Models;

namespace UniversityExamSimulation.Core.Repositories
{
    public interface IStudentRepository
    {
        List<Student> GetStudents();
        Student GetStudentById(int id);
        List<Student> GetStudentsByUniversity(int universityId);

        Student AddStudent(string name, string surname);
        void UpdateStudents(List<Student> students);
        void DeleteStudents();
    }
}
