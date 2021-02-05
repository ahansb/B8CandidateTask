using System.Collections.Generic;

using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Services.Data.Interfaces
{
    public interface IDisciplineService
    {
        ICollection<Discipline> GetAll();
        Discipline GetById(int id);
        int Edit(int id, string professorName);
        int Create(DisciplineCreateModel model);
        int Delete(int id);

    }
}
