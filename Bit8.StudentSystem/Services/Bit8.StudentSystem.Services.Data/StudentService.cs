using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;
using Bit8.StudentSystem.Services.Data.Interfaces;

namespace Bit8.StudentSystem.Services.Data
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository repository;

        public StudentService(IStudentRepository repository)
        {
            this.repository = repository;
        }

        public ICollection<Student> GetAll()
        {
            var result = this.repository.All();
            return result;
        }

        public Student GetById(int id)
        {
            var result = this.repository.GetById(id);
            return result;
        }

        public int Create(StudentCreateModel model)
        {
            var student = new Student()
            {
                Name = model.Name,
                Surname = model.Surname,
                DOB = model.DOB,
                Semesters = new List<Semester>()
            };

            foreach (var semesterId in model.Semesters)
            {
                student.Semesters.Add(new Semester() { Id = semesterId });
            }

            var affectedRows = this.repository.Add(student);
            return affectedRows;
        }

        public int DeleteStudentSemester(int id, int semesterId)
        {
            var affectedRows = this.repository.DeleteStudentSemester(id, semesterId);
            return affectedRows;
        }

        public int AddStudentSemester(int id, StudentSemesterCreateModel model)
        {
            if (model == null || model.Id <= 0)
            {
                return 0;
            }

            var affectedRows = this.repository.AddStudentSemester(id, model.Id);
            return affectedRows;
        }

        public int AddStudentDisciplineScore(int id, StudentDisciplineScore model)
        {
            if (id <= 0 || model == null || model.DisciplineId <= 0 || model.Score <= 0)
            {
                return 0;
            }

            var affectedRows = this.repository.AddStudentDisciplineScore(id, model.DisciplineId, model.Score);
            return affectedRows;
        }

        public int EditStudentDisciplineScore(int id, StudentDisciplineScore model)
        {
            if (id <= 0 || model == null || model.DisciplineId <= 0 || model.Score <= 0)
            {
                return 0;
            }

            var affectedRows = this.repository.UpdateStudentDisciplineScore(id, model.DisciplineId, model.Score);
            return affectedRows;
        }

        public int DeleteStudentDisciplineScore(int id, DeleteStudentDisciplineScore model)
        {
            if (id <= 0 || model == null || model.DisciplineId <= 0)
            {
                return 0;
            }

            var affectedRows = this.repository.DeleteStudentDisciplineScore(id, model.DisciplineId);
            return affectedRows;
        }
    }
}
