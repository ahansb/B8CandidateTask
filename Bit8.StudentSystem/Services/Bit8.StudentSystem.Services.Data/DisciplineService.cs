﻿using System.Collections.Generic;

using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;
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

        public int Edit(int id, string professorName)
        {
            var affectedRows = this.repository.Update(id, professorName);
            return affectedRows;
        }

        public int Create(DisciplineCreateModel model)
        {
            var affectedRows = this.repository.Add(model);
            return affectedRows;
        }

        public int Delete(int id)
        {
            return this.repository.Delete(id);
        }
    }
}
