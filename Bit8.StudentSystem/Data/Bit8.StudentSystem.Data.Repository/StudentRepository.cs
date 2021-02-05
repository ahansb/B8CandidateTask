using System;
using System.Collections.Generic;
using System.Linq;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data.Repository
{
    public class StudentRepository : BaseRepository, IStudentRepository
    {
        private const string DisciplineTableName = "bit8studentsystem.discipline";
        private const string SemesterTableName = "bit8studentsystem.semester";
        private const string StudentTableName = "bit8studentsystem.student";
        private const string StudentSemesterTableName = "bit8studentsystem.studentsemester";
        private const string ScoreTableName = "bit8studentsystem.score";

        private const string StudentIdText = "StudentId";
        private const string StudentNameText = "StudenName";
        private const string StudentSurnameText = "StudentSurname";
        private const string StudentDOBText = "StudentDOB";

        private const string SemesterIdText = "SemesterId";
        private const string SemesterNameText = "SemesterName";
        private const string SemesterStartDateText = "SemesterStartDate";
        private const string SemesterEndDateText = "SemesterEndDate";

        private const string DisciplineIdText = "DisciplineId";
        private const string DisciplineNameText = "DisciplineName";
        private const string DisciplineProfessorNameText = "DisciplineProfessorName";
        private const string DisciplineSemesterIdText = "DisciplineSemesterId";

        private const string ScoreText = "Score";
        private const int ScoreColumnIndex = 12;


        public StudentRepository(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<Student> All()
        {
            var students = new List<Student>();
            using (var connection = this.Context.Connection)
            {
                var statement = this.GetSelectStatement();
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var studentId = (int) reader[StudentIdText];

                        var student = students.Where(s => s.Id == studentId).FirstOrDefault();
                        if (student == null)
                        {
                            var readerStudent = new Student()
                            {
                                Id = studentId,
                                Name = reader[StudentNameText].ToString(),
                                Surname = reader[StudentSurnameText].ToString(),
                                DOB = (DateTime) reader[StudentDOBText],
                                Semesters = new List<Semester>()
                            };

                            students.Add(readerStudent);
                            student = students.Last();
                        }

                        if (reader[SemesterIdText] != DBNull.Value)
                        {
                            var semesterId = (int) reader[SemesterIdText];
                            var semester = student.Semesters.Where(s => s.Id == semesterId).FirstOrDefault();
                            if (semester == null)
                            {
                                var readerSemester = new Semester()
                                {
                                    Id = semesterId,
                                    Name = reader[SemesterNameText].ToString(),
                                    StartDate = (DateTime) reader[SemesterStartDateText],
                                    EndDate = (DateTime) reader[SemesterEndDateText],
                                    Disciplines = new List<Discipline>()
                                };

                                student.Semesters.Add(readerSemester);
                                semester = student.Semesters.Last();
                            }

                            if (reader[DisciplineIdText] != DBNull.Value)
                            {
                                var disciplineId = (int) reader[DisciplineIdText];
                                var discipline = semester.Disciplines.Where(d => d.Id == disciplineId).FirstOrDefault();
                                if (discipline == null)
                                {
                                    var readerDiscipline = new Discipline()
                                    {
                                        Id = disciplineId,
                                        DisciplineName = reader[DisciplineNameText].ToString(),
                                        ProfessorName = reader[DisciplineProfessorNameText].ToString(),
                                        SemesterId = (int) reader[DisciplineSemesterIdText],
                                        Score = reader[ScoreText] == DBNull.Value ? null : (int?) reader[ScoreText]
                                    };

                                    semester.Disciplines.Add(readerDiscipline);
                                }
                            }
                        }
                    }

                    reader.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return students;
        }

        public Student GetById(int id)
        {
            var student = new Student();
            using (var connection = this.Context.Connection)
            {
                var statement = this.GetSelectStatement(id);
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (student.Id == default(int))
                        {
                            student.Id = (int) reader[StudentIdText];
                            student.Name = reader[StudentNameText].ToString();
                            student.Surname = reader[StudentSurnameText].ToString();
                            student.DOB = (DateTime) reader[StudentDOBText];
                            student.Semesters = new List<Semester>();
                        }

                        if (reader[SemesterIdText] != DBNull.Value)
                        {
                            var semesterId = (int) reader[SemesterIdText];
                            var semester = student.Semesters.Where(s => s.Id == semesterId).FirstOrDefault();
                            if (semester == null)
                            {
                                var readerSemester = new Semester()
                                {
                                    Id = semesterId,
                                    Name = reader[SemesterNameText].ToString(),
                                    StartDate = (DateTime) reader[SemesterStartDateText],
                                    EndDate = (DateTime) reader[SemesterEndDateText],
                                    Disciplines = new List<Discipline>()
                                };

                                student.Semesters.Add(readerSemester);
                                semester = student.Semesters.Last();
                            }

                            if (reader[DisciplineIdText] != DBNull.Value)
                            {
                                var disciplineId = (int) reader[DisciplineIdText];
                                var discipline = semester.Disciplines.Where(d => d.Id == disciplineId).FirstOrDefault();
                                if (discipline == null)
                                {
                                    var readerDiscipline = new Discipline()
                                    {
                                        Id = disciplineId,
                                        DisciplineName = reader[DisciplineNameText].ToString(),
                                        ProfessorName = reader[DisciplineProfessorNameText].ToString(),
                                        SemesterId = (int) reader[DisciplineSemesterIdText],
                                        Score = reader[ScoreText] == DBNull.Value ? null : (int?) reader[ScoreText]
                                    };

                                    semester.Disciplines.Add(readerDiscipline);
                                }
                            }
                        }
                    }

                    reader.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return student;
        }

        public int Add(Student student)
        {
            var affectedRows = 0; 
            int idOfStudent = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"INSERT INTO {StudentTableName}(`Name`,`Surname`,`DOB`)VALUES(@Name,@Surname,@DOB);SELECT Id FROM {StudentTableName} WHERE Id = LAST_INSERT_ID();";
                var command = new MySqlCommand(statement, connection);

                command.Parameters.AddWithValue("Name", student.Name);
                command.Parameters.AddWithValue("Surname", student.Surname);
                command.Parameters.AddWithValue("DOB", student.DOB);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        idOfStudent = (int) reader["Id"];
                    }

                    reader.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            if (student.Semesters.Count > 0)
            {
                using (var connection = this.Context.Connection)
                {
                    var statement = $"INSERT INTO  {StudentSemesterTableName} (`StudentId`,`SemesterId`) VALUES ";
                    var command = new MySqlCommand(statement, connection);

                    for (int i = 0; i < student.Semesters.Count; i++)
                    {
                        var studentSemesterForAdd = $"({idOfStudent}, @SemesterId{i})";
                        command.Parameters.AddWithValue($"SemesterId{i}", student.Semesters[i].Id);
                        statement = $"{statement}{studentSemesterForAdd}";
                        if (i == student.Semesters.Count - 1)
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
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            return affectedRows + 1;
        }

        public int DeleteStudentSemester(int id, int semesterId)
        {
            if (id <= 0 || semesterId <= 0)
            {
                return 0;
            }

            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"DELETE FROM {StudentSemesterTableName} WHERE StudentId={id} AND SemesterId={semesterId};";
                var command = new MySqlCommand(statement, connection);
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

        public int AddStudentSemester(int id, int semesterId)
        {
            if (id <= 0 || semesterId <= 0)
            {
                return 0;
            }

            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"INSERT INTO  {StudentSemesterTableName} (`StudentId`,`SemesterId`) VALUES ({id}, {semesterId});";
                var command = new MySqlCommand(statement, connection);
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

        public int AddStudentDisciplineScore(int id, int disciplineId, int score)
        {
            if (id <= 0 || disciplineId <= 0 || score <= 0)
            {
                return 0;
            }

            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"INSERT INTO  {ScoreTableName} (`StudentId`,`DisciplineId`,`Score`) VALUES ({id}, {disciplineId}, {score});";
                var command = new MySqlCommand(statement, connection);
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

        public int UpdateStudentDisciplineScore(int id, int disciplineId, int score)
        {
            if (id <= 0 || disciplineId <= 0 || score <= 0)
            {
                return 0;
            }

            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"UPDATE {ScoreTableName} SET Score = {score} WHERE StudentId = {id} AND DisciplineId = {disciplineId};";
                var command = new MySqlCommand(statement, connection);
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

        public int DeleteStudentDisciplineScore(int id, int disciplineId)
        {
            if (id <= 0 || disciplineId <= 0)
            {
                return 0;
            }

            var affectedRows = 0;
            using (var connection = this.Context.Connection)
            {
                var statement = $"DELETE FROM {ScoreTableName} WHERE StudentId = {id} AND DisciplineId = {disciplineId};";
                var command = new MySqlCommand(statement, connection);
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

        private string GetSelectStatement(int? studentId = null)
        {
            var statementLineOne = $"SELECT st.Id as {StudentIdText}, st.Name as {StudentNameText}, st.Surname as {StudentSurnameText}, st.DOB as {StudentDOBText},";
            var statementSecondLine = $"sem.Id as {SemesterIdText}, sem.Name as {SemesterNameText}, sem.StartDate as {SemesterStartDateText}, sem.EndDate as {SemesterEndDateText},";
            var statementThirdLine = $"d.Id as {DisciplineIdText}, d.DisciplineName, d.ProfessorName as {DisciplineProfessorNameText}, d.SemesterId as {DisciplineSemesterIdText},";
            var statementFourthLine = $"sc.Score ";
            var statementFifthLine = $"FROM {StudentTableName} st LEFT JOIN {StudentSemesterTableName} ss ON ss.StudentId = st.Id LEFT JOIN {SemesterTableName} sem ON sem.Id = ss.SemesterId ";
            var statementSixthLine = $"LEFT JOIN {DisciplineTableName} d ON d.SemesterId = sem.Id LEFT JOIN {ScoreTableName} sc ON sc.StudentId = st.Id AND sc.DisciplineId = d.Id ";

            var statement = $"{statementLineOne}{statementSecondLine}{statementThirdLine}{statementFourthLine}{statementFifthLine}{statementSixthLine}";
            var statementOrderClause = " ORDER BY st.Id, sem.Id, d.Id;";
            statement = studentId.HasValue ? $"{statement}WHERE st.Id = {studentId} {statementOrderClause}" : $"{statement} {statementOrderClause}";

            return statement;
        }
    }
}
