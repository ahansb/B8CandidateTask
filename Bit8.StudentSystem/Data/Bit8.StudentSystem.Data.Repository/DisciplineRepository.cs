using System;
using System.Collections.Generic;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data.Repository
{
    public class DisciplineRepository : BaseRepository, IDisciplineRepository
    {
        public DisciplineRepository(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<Discipline> All()
        {
            ICollection<Discipline> disciplines = new List<Discipline>();
            using (var connection = this.Context.Connection)
            {
                var statement = $"SELECT  d.*, s.Name, s.StartDate, s.EndDate  FROM {disciplineTableName} d LEFT JOIN {semesterTableName} s ON s.Id = d.SemesterId;";
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
                    this.Log(this.GetExceptionText(ex));
                }
            }

            return disciplines;
        }

        public Discipline GetById(int id)
        {
            Discipline discipline = null;
            using (var connection = this.Context.Connection)
            {
                var statement = $"SELECT  d.*, s.Name, s.StartDate, s.EndDate  FROM {disciplineTableName} d LEFT JOIN {semesterTableName} s ON s.Id = d.SemesterId WHERE d.Id = {id};";
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        discipline = this.MapReaderToDiscipline(reader);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }
            }

            return discipline;
        }

        public int Add(DisciplineCreateModel discipline)
        {
            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"INSERT INTO  {disciplineTableName} (`DisciplineName`,`ProfessorName`,`SemesterId`) VALUES (@DisciplineName, @ProfessorName, @SemesterId);";
                var command = new MySqlCommand(statement, connection);

                command.Parameters.AddWithValue("DisciplineName", discipline.DisciplineName);
                command.Parameters.AddWithValue("ProfessorName", discipline.ProfessorName);
                command.Parameters.AddWithValue("SemesterId", discipline.SemesterId);
                try
                {
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }
            }

            return affectedRows;
        }

        public int Update(int id, string professorName)
        {
            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"UPDATE {disciplineTableName} SET ProfessorName = @ProfessorName WHERE Id = {id}";
                var command = new MySqlCommand(statement, connection);
                command.Parameters.AddWithValue("ProfessorName",professorName);

                try
                {
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }
            }

            return affectedRows;
        }

        public int Delete(int id)
        {
            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"DELETE FROM {disciplineTableName} WHERE Id={id};";
                var command = new MySqlCommand(statement, connection);

                try
                {
                    connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }
            }

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
