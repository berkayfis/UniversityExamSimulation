using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityExamSimulation.Core.Models;
using UniversityExamSimulation.Core.Repositories;

namespace UniversityExamSimulation.Data
{
    public class UniversityRepository : IUniversityRepository
    {
        private UniversityExamDbContext dbContext;
        public UniversityRepository(UniversityExamDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void DeleteUniversities()
        {
            throw new NotImplementedException();
        }

        public List<University> GetUniversities()
        {
            var universities = dbContext.Universities.ToList();
            return universities;
        }
    }
}
