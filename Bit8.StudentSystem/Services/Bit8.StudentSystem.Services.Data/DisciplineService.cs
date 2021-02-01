using System;
using System.Collections.Generic;

using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Services.Data.Interfaces;

namespace Bit8.StudentSystem.Services.Data
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineRepository repository;

        public DisciplineService(IDisciplineRepository repository)
        {
            this.repository = repository;
        }

        public ICollection<Discipline> GetAll()
        {
            var result = this.repository.All();
            return result;
        }

        public Discipline GetById(int id)
        {
            var result = this.repository.GetById(id);
            return result;
        }
    }
}
