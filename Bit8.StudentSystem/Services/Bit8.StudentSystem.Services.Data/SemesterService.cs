using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.Repository.Interfaces;
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
    }
}
