using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityExamSimulation.Core.Models;
using UniversityExamSimulation.Core.Repositories;
using UniversityExamSimulation.Core.Services;

namespace UniversityExamSimulation.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly IUniversityRepository universityRepository;
        private readonly IStudentRepository studentRepository;
        public UniversityService(IUniversityRepository universityRepository, IStudentRepository studentRepository)
        {
            this.universityRepository = universityRepository;
            this.studentRepository = studentRepository;
        }

        public void DeleteUniversities()
        {
            universityRepository.DeleteUniversities();
        }

        public List<Student> GetStudentsByUniversityId(int universityId)
        {
            var students = studentRepository.GetStudentsByUniversity(universityId);
            return students;
        }

        public List<University> GetUniversities()
        {
            var universities = universityRepository.GetUniversities();
            return universities;
        }
    }
}
