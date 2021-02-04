﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.Repository.Interfaces;

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
            var statement = this.GetSelectStatement();
            var reader = this.Context.ExecuteQuery(statement);

            ICollection<Student> students = new List<Student>();
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

            return students;
        }

        public Student GetById(int id)
        {
            var statement = this.GetSelectStatement(id);
            var reader = this.Context.ExecuteQuery(statement);

            var student = new Student();
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

            return student;
        }

        public int Add(Student student)
        {
            var statement = $"INSERT INTO {StudentTableName}(`Name`,`Surname`,`DOB`)VALUES(@Name,@Surname,@DOB);SELECT Id FROM {StudentTableName} WHERE Id = LAST_INSERT_ID();";
            var parameters = new List<MySqlParameter>()
            {
                new MySqlParameter("Name",student.Name),
                new MySqlParameter("Surname",student.Surname),
                new MySqlParameter("DOB",student.DOB)
            };

            var reader = this.Context.ExecuteQuery(statement, parameters);
            var affectedRows = 0;

            if (student.Semesters.Count > 0)
            {
                int idOfStudent = 0;
                while (reader.Read())
                {
                    idOfStudent = (int) reader["Id"];
                }

                var studentSemesterStatement = $"INSERT INTO  {StudentSemesterTableName} (`StudentId`,`SemesterId`) VALUES ";
                var studentSemesterParameters = new List<MySqlParameter>();
                for (int i = 0; i < student.Semesters.Count; i++)
                {
                    var studentSemesterForAdd = $"({idOfStudent}, @SemesterId{i})";
                    studentSemesterParameters.Add(new MySqlParameter($"SemesterId{i}", student.Semesters[i].Id));

                    studentSemesterStatement = $"{studentSemesterStatement}{studentSemesterForAdd}";
                    if (i == student.Semesters.Count - 1)
                    {
                        studentSemesterStatement = $"{studentSemesterStatement};";
                    }
                    else
                    {
                        studentSemesterStatement = $"{studentSemesterStatement},";
                    }
                }

                this.Context.CloseConnection();
                this.Context.OpenConnection();
                affectedRows = this.Context.ExecuteNonQuery(studentSemesterStatement, studentSemesterParameters);
            }

            return affectedRows + 1;
        }

        public int DeleteStudentSemester(int id, int semesterId)
        {
            if (id <=0 || semesterId<=0)
            {
                return 0;
            }

            var statement = $"DELETE FROM {StudentSemesterTableName} WHERE StudentId={id} AND SemesterId={semesterId};";
            var affectedRows = this.Context.ExecuteNonQuery(statement);
            return affectedRows;
        }

        public int AddStudentSemester(int id, int semesterId)
        {
            if (id <= 0 || semesterId <= 0)
            {
                return 0;
            }

            var statement = $"INSERT INTO  {StudentSemesterTableName} (`StudentId`,`SemesterId`) VALUES ({id}, {semesterId});";
            var affectedRows = this.Context.ExecuteNonQuery(statement);
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
