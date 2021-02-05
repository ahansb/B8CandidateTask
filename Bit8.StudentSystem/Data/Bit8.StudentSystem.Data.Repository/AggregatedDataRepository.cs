using System;
using System.Collections.Generic;
using System.Linq;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data.Repository
{
    public class AggregatedDataRepository : BaseRepository, IAggregatedDataRepository
    {
        public AggregatedDataRepository(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<TopStudent> GetTopTenStudentsScores()
        {
            var students = new List<TopStudent>();
            using (var connection = this.Context.Connection)
            {
                var statement = @"SELECT st.Id as StudentId, st.Name as StudenName, st.Surname as StudentSurname, st.DOB as StudentDOB,
AVG(sc.Score) as Average FROM bit8studentsystem.student st 
LEFT JOIN bit8studentsystem.studentsemester ss ON ss.StudentId = st.Id 
LEFT JOIN bit8studentsystem.semester sem ON sem.Id = ss.SemesterId 
LEFT JOIN bit8studentsystem.discipline d ON d.SemesterId = sem.Id 
LEFT JOIN bit8studentsystem.score sc ON sc.StudentId = st.Id AND sc.DisciplineId = d.Id 
WHERE sc.Score IS NOT NULL
GROUP BY st.Id
ORDER BY Average DESC
LIMIT 10;";
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var readerStudent = new TopStudent()
                        {
                            Id = (int) reader["StudentId"],
                            Name = reader["StudenName"].ToString(),
                            Surname = reader["StudentSurname"].ToString(),
                            DOB = (DateTime) reader["StudentDOB"],
                            AvarageScore = (decimal) reader["Average"]
                        };

                        students.Add(readerStudent);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }
            }

            return students;
        }

        public ICollection<Student> GetNoMarksStudents()
        {
            var students = new List<Student>();
            using (var connection = this.Context.Connection)
            {
                var statement = @"SELECT st.Id as StudentId, st.Name as StudenName, st.Surname as StudentSurname, st.DOB as StudentDOB,
sem.Id as SemesterId, sem.Name as SemesterName, sem.StartDate as SemesterStartDate, sem.EndDate as SemesterEndDate,
d.Id as DisciplineId, d.DisciplineName, d.ProfessorName,
sc.Score FROM bit8studentsystem.student st 
LEFT JOIN bit8studentsystem.studentsemester ss ON ss.StudentId = st.Id 
LEFT JOIN bit8studentsystem.semester sem ON sem.Id = ss.SemesterId 
LEFT JOIN bit8studentsystem.discipline d ON d.SemesterId = sem.Id 
LEFT JOIN bit8studentsystem.score sc ON sc.StudentId = st.Id AND sc.DisciplineId = d.Id 
WHERE sc.Score IS NULL
AND sem.Id IS NOT NULL
AND d.Id IS NOT NULL
ORDER BY st.Name, st.Surname, sem.Name, d.Id;";
                var command = new MySqlCommand(statement, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var studentId = (int) reader["StudentId"];

                        var student = students.Where(s => s.Id == studentId).FirstOrDefault();
                        if (student == null)
                        {
                            var readerStudent = new Student()
                            {
                                Id = studentId,
                                Name = reader["StudenName"].ToString(),
                                Surname = reader["StudentSurname"].ToString(),
                                DOB = (DateTime) reader["StudentDOB"],
                                Semesters = new List<Semester>()
                            };

                            students.Add(readerStudent);
                            student = students.Last();
                        }

                        var semesterId = (int) reader["SemesterId"];
                        var semester = student.Semesters.Where(s => s.Id == semesterId).FirstOrDefault();
                        if (semester == null)
                        {
                            var readerSemester = new Semester()
                            {
                                Id = semesterId,
                                Name = reader["SemesterName"].ToString(),
                                StartDate = (DateTime) reader["SemesterStartDate"],
                                EndDate = (DateTime) reader["SemesterEndDate"],
                                Disciplines = new List<Discipline>()
                            };

                            student.Semesters.Add(readerSemester);
                            semester = student.Semesters.Last();
                        }

                        var disciplineId = (int) reader["DisciplineId"];
                        var discipline = semester.Disciplines.Where(d => d.Id == disciplineId).FirstOrDefault();
                        if (discipline == null)
                        {
                            var readerDiscipline = new Discipline()
                            {
                                Id = disciplineId,
                                DisciplineName = reader["DisciplineName"].ToString(),
                                ProfessorName = reader["ProfessorName"].ToString()
                            };

                            semester.Disciplines.Add(readerDiscipline);
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    this.Log(this.GetExceptionText(ex));
                }
            }

            return students;
        }
    }
}

