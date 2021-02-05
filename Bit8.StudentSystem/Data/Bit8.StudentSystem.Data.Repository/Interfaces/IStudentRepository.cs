using System.Collections.Generic;

using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Data.Repository.Interfaces
{
    public interface IStudentRepository
    {
        ICollection<Student> All();
        Student GetById(int id);
        int Add(StudentCreateModel student);
        int DeleteStudentSemester(int id, int semesterId);
        int AddStudentSemester(int id, int semesterId);
        int AddStudentDisciplineScore(int id, int disciplineId, int score);
        int UpdateStudentDisciplineScore(int id, int disciplineId, int score);
        int DeleteStudentDisciplineScore(int id, int disciplineId);
    }
}
