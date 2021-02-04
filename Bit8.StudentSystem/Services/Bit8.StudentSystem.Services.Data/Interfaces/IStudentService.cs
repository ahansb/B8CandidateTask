using System.Collections.Generic;

using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Services.Data.Interfaces
{
    public interface IStudentService
    {
        ICollection<Student> GetAll();
        Student GetById(int id);
        int Create(StudentCreateModel model);
        int DeleteStudentSemester(int id, int semesterId);
        int AddStudentSemester(int id, StudentSemesterCreateModel model);
        int AddStudentDisciplineScore(int id, StudentDisciplineScore model);
        int EditStudentDisciplineScore(int id, StudentDisciplineScore model);
        int DeleteStudentDisciplineScore(int id, DeleteStudentDisciplineScore model);
    }
}
