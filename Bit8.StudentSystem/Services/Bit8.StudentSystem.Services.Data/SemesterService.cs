using System.Collections.Generic;

using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;
using Bit8.StudentSystem.Services.Data.Interfaces;

namespace Bit8.StudentSystem.Services.Data
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository repository;

        public SemesterService(ISemesterRepository repository)
        {
            this.repository = repository;
        }

        public ICollection<Semester> GetAll()
        {
            var result = this.repository.All();
            return result;
        }

        public Semester GetById(int id)
        {
            var result = this.repository.GetById(id);
            return result;
        }

        public int Create(SemesterCreateModel model)
        {
            var affectedRows = this.repository.Add(model);
            return affectedRows;
        }

        public int Edit(int id, SemesterEditModel model)
        {
            var affectedRows = this.repository.Update(id, model);
            return affectedRows;
        }
    }
}
