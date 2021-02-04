using System.Collections.Generic;

using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;
using Bit8.StudentSystem.Services.Data.Interfaces;

namespace Bit8.StudentSystem.Services.Data
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository repository;

        public SemesterService(ISemesterRepository repository)
        {
            this.repository = repository;
        }

        public ICollection<Semester> GetAll()
        {
            var result = this.repository.All();
            return result;
        }

        public Semester GetById(int id)
        {
            var result = this.repository.GetById(id);
            return result;
        }

        public int Create(SemesterCreateModel model)
        {
            var semester = new Semester()
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Disciplines = new List<Discipline>()
            };

            foreach (var disciplineModel in model.Disciplines)
            {
                var discipline = new Discipline()
                {
                    DisciplineName = disciplineModel.DisciplineName,
                    ProfessorName = disciplineModel.ProfessorName
                };

                semester.Disciplines.Add(discipline);
            }

            var affectedRows = this.repository.Add(semester);

            return affectedRows;
        }

        public int Edit(int id, SemesterEditModel model)
        {
            var semester = new Semester()
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            var affectedRows = this.repository.Update(id, semester);
            return affectedRows;
        }
    }
}
