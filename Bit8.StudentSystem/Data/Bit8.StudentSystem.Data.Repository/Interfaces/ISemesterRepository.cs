using System.Collections.Generic;

using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Data.Repository.Interfaces
{
    public interface ISemesterRepository
    {
        ICollection<Semester> All();
        int Add(Semester semester);
        Semester GetById(int id);
        int Update(int id, Semester semester);
    }
}
