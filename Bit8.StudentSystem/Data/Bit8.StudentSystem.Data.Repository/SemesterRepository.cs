using System;
using System.Collections.Generic;
using System.Linq;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data.Repository
{
    public class SemesterRepository : BaseRepository, ISemesterRepository
    {
        public SemesterRepository(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<Semester> All()
        {
            var semesters = new List<Semester>();
            using (var connection = this.Context.Connection)
            {
                var statement = $"SELECT s.*, d.Id as DisciplineId, d.DisciplineName, d.ProfessorName FROM {this.semesterTableName} s ";
                statement = $"{statement} LEFT JOIN {this.disciplineTableName} d ON d.SemesterId = s.Id;";
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var semesterId = (int) reader["Id"];
                        var semester = semesters.Where(s => s.Id == semesterId).FirstOrDefault();
                        if (semester == null)
                        {
                            var readerSemester = new Semester()
                            {
                                Id = semesterId,
                                Name = reader["Name"].ToString(),
                                StartDate = (DateTime) reader["StartDate"],
                                EndDate = (DateTime) reader["EndDate"],
                                Disciplines = new List<Discipline>()
                            };

                            semesters.Add(readerSemester);
                            semester = semesters.Last();
                        }

                        if (reader["DisciplineId"] != DBNull.Value)
                        {
                            var disciplineId = (int) reader["DisciplineId"];
                            var discipline = semester.Disciplines.Where(d => d.Id == disciplineId).FirstOrDefault();
                            if (discipline == null)
                            {
                                var readerDiscipline = new Discipline()
                                {
                                    Id = disciplineId,
                                    DisciplineName = reader["DisciplineName"].ToString(),
                                    ProfessorName = reader["ProfessorName"].ToString(),
                                    SemesterId = semesterId
                                };

                                semester.Disciplines.Add(readerDiscipline);
                            }
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }
            }

            return semesters;
        }

        public Semester GetById(int id)
        {
            var semester = new Semester();
            using (var connection = this.Context.Connection)
            {
                var statement = $"SELECT s.*, d.Id as DisciplineId, d.DisciplineName, d.ProfessorName FROM {this.semesterTableName} s LEFT JOIN {this.disciplineTableName} d ON d.SemesterId = s.Id WHERE s.Id = {id};";
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (semester.Id == default(int))
                        {
                            semester.Id = (int) reader["Id"];
                            semester.Name = reader["Name"].ToString();
                            semester.StartDate = (DateTime) reader["StartDate"];
                            semester.EndDate = (DateTime) reader["EndDate"];
                            semester.Disciplines = new List<Discipline>();
                        }

                        if (reader["DisciplineId"] != DBNull.Value)
                        {
                            var disciplineId = (int) reader["DisciplineId"];
                            var discipline = semester.Disciplines.Where(d => d.Id == disciplineId).FirstOrDefault();
                            if (discipline == null)
                            {
                                var readerDiscipline = new Discipline()
                                {
                                    Id = disciplineId,
                                    DisciplineName = reader["DisciplineName"].ToString(),
                                    ProfessorName = reader["ProfessorName"].ToString(),
                                    SemesterId = semester.Id
                                };

                                semester.Disciplines.Add(readerDiscipline);
                            }
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }
            }

            return semester;
        }

        public int Add(SemesterCreateModel semester)
        {
            var affectedRows = 0;
            int idOfSemester = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"INSERT INTO {semesterTableName}(`Name`,`StartDate`,`EndDate`)VALUES(@Name,@StartDate,@EndDate);SELECT Id FROM {semesterTableName} WHERE Id = LAST_INSERT_ID();";
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
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }

            }

            if (semester.Disciplines.Count > 0)
            {
                using (var connection = this.Context.Connection)
                {
                    var statement = $"INSERT INTO  {disciplineTableName} (`DisciplineName`,`ProfessorName`,`SemesterId`) VALUES ";
                    var command = new MySqlCommand(statement, connection);

                    for (int i = 0; i < semester.Disciplines.Count; i++)
                    {
                        var disciplineForAdd = $"(@DisciplineName{i}, @ProfessorName{i}, {idOfSemester})";
                        command.Parameters.AddWithValue($"DisciplineName{i}", semester.Disciplines[i].DisciplineName);
                        command.Parameters.AddWithValue($"ProfessorName{i}", semester.Disciplines[i].ProfessorName);

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

                    command.CommandText = statement;
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
            }

            return affectedRows + 1;
        }

        public int Update(int id, SemesterEditModel semester)
        {
            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"UPDATE {semesterTableName} SET Name = @Name, StartDate = @StartDate, EndDate = @EndDate WHERE Id = {id}";
                var command = new MySqlCommand(statement, connection);

                command.Parameters.AddWithValue("Name", semester.Name);
                command.Parameters.AddWithValue("StartDate", semester.StartDate);
                command.Parameters.AddWithValue("EndDate", semester.EndDate);

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
