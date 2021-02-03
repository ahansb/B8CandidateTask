﻿using System;
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
    }
}
