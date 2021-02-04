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

        //TODO: 1 query
        public ICollection<Semester> All()
        {
            ICollection<Semester> semesters = new List<Semester>();

            using (var connection = this.Context.Connection)
            {
                var statement = $"SELECT * FROM {SemesterTableName};";
                var command = new MySqlCommand(statement, connection);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var semester = this.MapReaderToSemester(reader);
                        semesters.Add(semester);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            foreach (var semester in semesters)
            {
                List<Discipline> disciplines = new List<Discipline>();
                using (var connection = this.Context.Connection)
                {
                    var disciplineStatement = $"SELECT * FROM {DisciplineTableName} WHERE SemesterId = {semester.Id};";
                    var command = new MySqlCommand(disciplineStatement, connection);


                    try
                    {
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var discipline = this.MapReaderToDiscipline(reader);
                            disciplines.Add(discipline);
                        }

                        reader.Close();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                semester.Disciplines = disciplines;
            }

            return semesters;
        }

        //TODO: 1 query
        public Semester GetById(int id)
        {
            Semester semester = new Semester();
            using (var connection = this.Context.Connection)
            {
                var statement = $"SELECT * FROM {SemesterTableName} WHERE Id = {id};";
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        semester = this.MapReaderToSemester(reader);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            List<Discipline> disciplines = new List<Discipline>();
            using (var connection = this.Context.Connection)
            {
                var statement = $"SELECT * FROM {DisciplineTableName} WHERE SemesterId = {semester.Id};";
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var discipline = this.MapReaderToDiscipline(reader);
                        disciplines.Add(discipline);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            semester.Disciplines = disciplines;
            return semester;
        }

        public int Add(Semester semester)
        {
            var affectedRows = 0;
            int idOfSemester = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"INSERT INTO {SemesterTableName}(`Name`,`StartDate`,`EndDate`)VALUES(@Name,@StartDate,@EndDate);SELECT Id FROM {SemesterTableName} WHERE Id = LAST_INSERT_ID();";
                var command = new MySqlCommand(statement, connection);

                command.Parameters.AddWithValue("Name", semester.Name);
                command.Parameters.AddWithValue("StartDate", semester.StartDate);
                command.Parameters.AddWithValue("EndDate", semester.EndDate);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        idOfSemester = (int) reader["Id"];
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }

            if (semester.Disciplines.Count > 0)
            {
                using (var connection = this.Context.Connection)
                {
                    var statement = $"INSERT INTO  {DisciplineTableName} (`DisciplineName`,`ProfessorName`,`SemesterId`) VALUES ";
                    var command = new MySqlCommand(statement, connection);

                    for (int i = 0; i < semester.Disciplines.Count; i++)
                    {
                        var disciplineForAdd = $"(@DisciplineName{i}, @ProfessorName{i}, @SemesterId{i})";
                        command.Parameters.AddWithValue($"DisciplineName{i}", semester.Disciplines[i].DisciplineName);
                        command.Parameters.AddWithValue($"ProfessorName{i}", semester.Disciplines[i].ProfessorName);
                        command.Parameters.AddWithValue($"SemesterId{i}", idOfSemester);

                        statement = $"{statement}{disciplineForAdd}";
                        if (i == semester.Disciplines.Count - 1)
                        {
                            statement = $"{statement};";
                        }
                        else
                        {
                            statement = $"{statement},";
                        }
                    }

                    try
                    {
                        connection.Open();
                        affectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            return affectedRows + 1;
        }

        public int Update(int id, Semester semester)
        {
            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"UPDATE {SemesterTableName} SET Name = @Name, StartDate = @StartDate, EndDate = @EndDate WHERE Id = {id}";
                var command = new MySqlCommand(statement, connection);

                command.Parameters.AddWithValue("Name", semester.Name);
                command.Parameters.AddWithValue("StartDate", semester.StartDate);
                command.Parameters.AddWithValue("EndDate", semester.EndDate);

                try
                {
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return affectedRows;
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
