using System;
using System.Collections.Generic;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.Repository.Interfaces;

namespace Bit8.StudentSystem.Data.Repository
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private const string TableName = "bit8studentsystem.discipline";
        private readonly IGenericRepository<Discipline> repository;
        public DisciplineRepository(IApplicationDbContext dbContext)
        {
            this.repository = new GenericRepository<Discipline>(dbContext, TableName);
        }

        public ICollection<Discipline> All()
        {
            var result = this.repository.All();
            return result;
        }

        public void GetById(int id)
        {

        }

        public void Add()
        {

        }

        public void Update()
        {

        }

        public void Delete()
        {

        }
    }
}
