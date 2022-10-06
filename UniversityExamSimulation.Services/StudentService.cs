using UniversityExamSimulation.Core.Models;
using UniversityExamSimulation.Core.Repositories;
using UniversityExamSimulation.Core.Services;

namespace UniversityExamSimulation.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public Student AddStudent(string name, string surname)
        {
            var student = studentRepository.AddStudent(name, surname);
            return student;
        }

        public void DeleteStudents()
        {
            studentRepository.DeleteStudents();
        }

        public Student GetStudentById(int id)
        {
            var student = studentRepository.GetStudentById(id);
            return student;
        }

        public List<Student> GetStudents()
        {
            var students = studentRepository.GetStudents();
            return students;
        }
    }
}
