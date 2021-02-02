using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Models;

namespace Bit8.StudentSystem.Data.Repository.Interfaces
{
    public interface IDisciplineRepository
    {
        ICollection<Discipline> All();
        Discipline GetById(int id);
        int Update(int id, string professorName);
        int Add(Discipline discipline);
        int Delete(int id);
    }
}
