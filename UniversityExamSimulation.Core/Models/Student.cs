namespace UniversityExamSimulation.Core.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? Point { get; set; }
        public int? UniversityId { get; set; }

        public bool? IsDeleted{ get; set; }

        public University? University { get; set; }
    }
}
