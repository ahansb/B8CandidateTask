using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.Repository.Interfaces;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data.Repository
{
    public class SemesterRepository : BaseRepository, ISemesterRepository
    {
        private const string DisciplineTableName = "bit8studentsystem.discipline";
        private const string SemesterTableName = "bit8studentsystem.semester";
        public SemesterRepository(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<Semester> All()
        {
            var statement = $"SELECT * FROM {SemesterTableName};";
            var reader = this.Context.ExecuteQuery(statement);

            ICollection<Semester> semesters = new List<Semester>();
            while (reader.Read())
            {
                var semester = this.MapReaderToSemester(reader);
                semesters.Add(semester);
            }

            foreach (var semester in semesters)
            {
                var disciplineStatement = $"SELECT * FROM {DisciplineTableName} WHERE SemesterId = {semester.Id};";
                this.Context.CloseConnection();
                this.Context.OpenConnection();

                var disciplineReader = this.Context.ExecuteQuery(disciplineStatement);

                ICollection<Discipline> disciplines = new List<Discipline>();
                while (disciplineReader.Read())
                {
                    var discipline = this.MapReaderToDiscipline(disciplineReader);
                    disciplines.Add(discipline);
                }

                semester.Disciplines = disciplines;
            }

            return semesters;
        }

        private Semester MapReaderToSemester(MySqlDataReader reader)
        {
            var semester = new Semester();

            semester.Id = (int) reader[nameof(semester.Id)];
            semester.Name = reader[nameof(semester.Name)].ToString();
            semester.StartDate = DateTime.Parse(reader[nameof(semester.StartDate)].ToString());
            semester.EndDate = DateTime.Parse(reader[nameof(semester.EndDate)].ToString());

            return semester;
        }

        private Discipline MapReaderToDiscipline(MySqlDataReader reader)
        {
            var discipline = new Discipline();

            discipline.Id = (int) reader[nameof(discipline.Id)];
            discipline.DisciplineName = reader[nameof(discipline.DisciplineName)].ToString();
            discipline.ProfessorName = reader[nameof(discipline.ProfessorName)].ToString();
            discipline.SemesterId = (int) reader[nameof(discipline.SemesterId)];

            return discipline;
        }
    }
}
