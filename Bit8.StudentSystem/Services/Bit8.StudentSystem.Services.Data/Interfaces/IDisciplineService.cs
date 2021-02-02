using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Services.Data.Interfaces
{
    public interface IDisciplineService
    {
        ICollection<Discipline> GetAll();
        Discipline GetById(int id);
        int Edit(int id, string professorName);
        int Create(DisciplineCreateModel model);
        bool Delete(int id);

    }
}
