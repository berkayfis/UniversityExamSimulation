using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityExamSimulation.Core.Models;
using UniversityExamSimulation.Core.Repositories;

namespace UniversityExamSimulation.Data
{
    public class StudentRepository : IStudentRepository
    {
        private UniversityExamDbContext dbContext;

        public StudentRepository(UniversityExamDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Student AddStudent(string name, string surname)
        {
            var student = new Student() { Name = name, Surname = surname };
            dbContext.Students.Add(student);
            dbContext.SaveChanges();
            return student;
        }

        public void DeleteStudents()
        {
            foreach (var student in dbContext.Students)
            {
                student.IsDeleted = true;
                student.UniversityId = null;
                student.University = null;
            }
            dbContext.SaveChanges();
        }

        public Student GetStudentById(int id)
        {
            var student = dbContext.Students.FirstOrDefault(s => s.Id == id);
            return student;
        }

        public List<Student> GetStudents()
        {
            var students = dbContext.Students.ToList();
            return students;
        }

        public List<Student> GetStudentsByUniversity(int universityId)
        {
            var students = dbContext.Students.Where(s => s.UniversityId == universityId).ToList();
            return students;
        }

        public void UpdateStudents(List<Student> students)
        {
            dbContext.UpdateRange(students);
            dbContext.SaveChanges();
        }
    }
}
