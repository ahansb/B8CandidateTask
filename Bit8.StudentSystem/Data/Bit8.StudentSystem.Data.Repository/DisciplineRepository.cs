using System;
using System.Collections.Generic;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.Repository.Interfaces;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data.Repository
{
    public class DisciplineRepository : BaseRepository, IDisciplineRepository
    {
        private const string DisciplineTableName = "bit8studentsystem.discipline";
        private const string SemesterTableName = "bit8studentsystem.semester";

        public DisciplineRepository(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<Discipline> All()
        {
            var statement = $"SELECT  d.*, s.Name, s.StartDate, s.EndDate  FROM {DisciplineTableName} d LEFT JOIN {SemesterTableName} s ON s.Id = d.SemesterId; ";
            var reader = this.Context.ExecuteQuery(statement);

            ICollection<Discipline> disciplines = new List<Discipline>();
            while (reader.Read())
            {
                var discipline = this.MapReaderToDiscipline(reader);

                disciplines.Add(discipline);
            }

            return disciplines;
        }

        public Discipline GetById(int id)
        {
            var statement = $"SELECT * FROM {DisciplineTableName} WHERE Id = {id};";
            var reader = this.Context.ExecuteQuery(statement);

            Discipline discipline = null;
            while (reader.Read())
            {
                discipline = this.MapReaderToDiscipline(reader);
            }

            return discipline;
        }

        public int Add(Discipline discipline)
        {
            var statement = $"INSERT INTO  {DisciplineTableName} (`DisciplineName`,`ProfessorName`,`SemesterId`) VALUES (@DisciplineName, @ProfessorName, @SemesterId);";
            var parameters = new List<MySqlParameter>()
            {
                new MySqlParameter("DisciplineName",discipline.DisciplineName),
                new MySqlParameter("ProfessorName",discipline.ProfessorName),
                new MySqlParameter("SemesterId",discipline.SemesterId)
            };

            var affectedRows = this.Context.ExecuteNonQuery(statement, parameters);

            return affectedRows;
        }

        public int Update(int id, string professorName)
        {
            var statement = $"UPDATE {DisciplineTableName} SET ProfessorName = @ProfessorName WHERE Id = {id}";
            var parameters = new List<MySqlParameter>()
            {
                new MySqlParameter("ProfessorName",professorName)
            };

            var affectedRows = this.Context.ExecuteNonQuery(statement, parameters);

            return affectedRows;
        }

        public int Delete(int id)
        {
            var statement = $"DELETE FROM {DisciplineTableName} WHERE Id={id};";
            var affectedRows = this.Context.ExecuteNonQuery(statement);

            return affectedRows;
        }

        private Discipline MapReaderToDiscipline(MySqlDataReader reader)
        {
            var discipline = new Discipline();

            discipline.Id = (int) reader[nameof(discipline.Id)];
            discipline.DisciplineName = reader[nameof(discipline.DisciplineName)].ToString();
            discipline.ProfessorName = reader[nameof(discipline.ProfessorName)].ToString();
            discipline.SemesterId = (int) reader[nameof(discipline.SemesterId)];
            discipline.Semester = new Semester()
            {
                Id = (int) reader[nameof(discipline.SemesterId)],
                Name = (string) reader["Name"],
                StartDate = (DateTime) reader["StartDate"],
                EndDate = (DateTime) reader["EndDate"]
            };

            return discipline;
        }
    }
}
