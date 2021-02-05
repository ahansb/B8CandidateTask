using System.Collections.Generic;

using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Data.Repository.Interfaces
{
    public interface IDisciplineRepository
    {
        ICollection<Discipline> All();
        Discipline GetById(int id);
        int Update(int id, string professorName);
        int Add(DisciplineCreateModel discipline);
        int Delete(int id);
    }
}
