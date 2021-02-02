using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Models;

namespace Bit8.StudentSystem.Data.Repository.Interfaces
{
    public interface ISemesterRepository
    {
        ICollection<Semester> All();
        int Add(Semester semester);
        Semester GetById(int id);
    }
}
