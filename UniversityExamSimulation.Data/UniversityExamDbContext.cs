using Microsoft.EntityFrameworkCore;
using UniversityExamSimulation.Core.Models;

namespace UniversityExamSimulation.Data
{
    public class UniversityExamDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<University> Universities { get; set; }
        public UniversityExamDbContext(DbContextOptions<UniversityExamDbContext> options) : base(options)
        {
        }

        protected UniversityExamDbContext()
        {
        }
    }
}
