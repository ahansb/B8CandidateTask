using System;
using System.Collections.Generic;
using System.IO;

using Bit8.StudentSystem.Data.Interfaces;
using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;

using MySql.Data.MySqlClient;

using Xunit;

namespace Bit8.StudentSystem.Data.Repository.Tests
{
    public class DisciplineRepositoryTests : BaseRepositoryTest
    {
        private readonly IDisciplineRepository repository;
        public DisciplineRepositoryTests()
        {
            this.ApplicationDbContext = new ApplicationDbContext(Configuration["ConnectionStrings:ApplicationDbContext"]);
            this.ApplicationDbContext.Initialize();
            this.repository = new DisciplineRepository(this.ApplicationDbContext);
        }

        [Fact]
        public void All_ShouldReturn_ListOfDisciplines()
        {
            var disciplines = this.repository.All();
            Assert.NotNull(disciplines);
            Assert.IsType<List<Discipline>>(disciplines);
            Assert.Equal(12, disciplines.Count);
        }

        [Fact]
        public void All_ShouldReturn_TheRightDisciplines()
        {
            var disciplines = this.repository.All();
            foreach (var discipline in disciplines)
            {
                Assert.NotNull(discipline);
                Assert.IsType<int>(discipline.Id);
                Assert.IsType<string>(discipline.DisciplineName);
                Assert.IsType<string>(discipline.ProfessorName);
                Assert.IsType<int>(discipline.SemesterId);
                Assert.NotNull(discipline.Semester);
                Assert.IsType<int>(discipline.Semester.Id);
                Assert.Equal(discipline.SemesterId, discipline.Semester.Id);
                Assert.IsType<string>(discipline.Semester.Name);
                Assert.IsType<DateTime>(discipline.Semester.StartDate);
                Assert.IsType<DateTime>(discipline.Semester.EndDate);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(6)]
        [InlineData(7)]
        public void GetById_ShouldReturn_TheRightDiscipline(int id)
        {
            Discipline dbDiscipline = null;
            using (var connection = this.ApplicationDbContext.Connection)
            {
                var statement = $"SELECT  d.*, s.Name, s.StartDate, s.EndDate  FROM {this.DisciplineTableName} d";
                   statement = $"{statement} LEFT JOIN {this.SemesterTableName} s ON s.Id = d.SemesterId WHERE d.Id = {id};";
                var command = new MySqlCommand(statement, connection);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {

                    dbDiscipline = new Discipline();

                    dbDiscipline.Id = (int) reader["Id"];
                    dbDiscipline.DisciplineName = reader["DisciplineName"].ToString();
                    dbDiscipline.ProfessorName = reader["ProfessorName"].ToString();
                    dbDiscipline.SemesterId = (int) reader["SemesterId"];
                    dbDiscipline.Semester = new Semester()
                    {
                        Id = (int) reader["SemesterId"],
                        Name = (string) reader["Name"],
                        StartDate = (DateTime) reader["StartDate"],
                        EndDate = (DateTime) reader["EndDate"]
                    };
                }

                reader.Close();
            }

            var discipline = this.repository.GetById(id);

            Assert.NotNull(discipline);
            Assert.IsType<int>(discipline.Id);
            Assert.IsType<string>(discipline.DisciplineName);
            Assert.IsType<string>(discipline.ProfessorName);
            Assert.IsType<int>(discipline.SemesterId);
            Assert.NotNull(discipline.Semester);
            Assert.IsType<int>(discipline.Semester.Id);
            Assert.Equal(discipline.SemesterId, discipline.Semester.Id);
            Assert.IsType<string>(discipline.Semester.Name);
            Assert.IsType<DateTime>(discipline.Semester.StartDate);
            Assert.IsType<DateTime>(discipline.Semester.EndDate);

            Assert.Equal(dbDiscipline.Id, discipline.Id);
            Assert.Equal(dbDiscipline.DisciplineName, discipline.DisciplineName);
            Assert.Equal(dbDiscipline.ProfessorName, discipline.ProfessorName);
            Assert.Equal(dbDiscipline.SemesterId, discipline.SemesterId);
            Assert.Equal(dbDiscipline.Semester.Id, discipline.Semester.Id);
            Assert.Equal(dbDiscipline.Semester.Name, discipline.Semester.Name);
            Assert.Equal(dbDiscipline.Semester.StartDate, discipline.Semester.StartDate);
            Assert.Equal(dbDiscipline.Semester.EndDate, discipline.Semester.EndDate);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(999)]
        public void GetById_ShouldReturn_Null(int id)
        {
            var discipline = this.repository.GetById(id);
            Assert.Null(discipline);
        }

        [Fact]
        public void Add_ShouldAdd_Discipline()
        {
            var addedDiscipline = new DisciplineCreateModel() { DisciplineName = "NewDiscipline", ProfessorName = "Professore", SemesterId = 1 };
            var affectedRows = this.repository.Add(addedDiscipline);
            Assert.Equal(1, affectedRows);
            long count = 0;
            using (var connection = this.ApplicationDbContext.Connection)
            {
                var statement = $"SELECT  COUNT(*) as Count  FROM {this.DisciplineTableName};";
                var command = new MySqlCommand(statement, connection);

                connection.Open();
                var reader = command.ExecuteReader();
 
                while (reader.Read())
                {
                    count = (long) reader["Count"];
                }

                reader.Close();
            }

            Assert.Equal(13, count);
        }

        [Fact]
        public void Add_ShouldAdd_CorrectDiscipline()
        {
            var addedDiscipline = new DisciplineCreateModel() { DisciplineName = "NewDiscipline", ProfessorName = "Professore", SemesterId = 1 };
            var affectedRows = this.repository.Add(addedDiscipline);
            Assert.Equal(1, affectedRows);

            var dbDiscipline = new Discipline();
            using (var connection = this.ApplicationDbContext.Connection)
            {
                var statement = $"SELECT  * FROM {this.DisciplineTableName} WHERE Id = 13";
                var command = new MySqlCommand(statement, connection);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dbDiscipline.Id = (int) reader["Id"];
                    dbDiscipline.DisciplineName = reader["DisciplineName"].ToString();
                    dbDiscipline.ProfessorName = reader["ProfessorName"].ToString();
                    dbDiscipline.SemesterId = (int) reader["SemesterId"];
                }

                reader.Close();
            }

            Assert.Equal(13, dbDiscipline.Id);
            Assert.Equal(addedDiscipline.DisciplineName, dbDiscipline.DisciplineName);
            Assert.Equal(addedDiscipline.ProfessorName, dbDiscipline.ProfessorName);
            Assert.Equal(addedDiscipline.SemesterId, dbDiscipline.SemesterId);
        }

        [Theory]
        [InlineData(null,0)]
        [InlineData("Normal name", 1)]
        [InlineData("Very Very Long Name Very Very Long Name Very Very Long Name ", 0)]
        public void Update_ShouldReturn_CorrectValue(string professorName, int expectedReturn)
        {
            int id = 3;
            var affectedRows = this.repository.Update(id, professorName);
            Assert.Equal(expectedReturn, affectedRows);
        }

        //TODO: Check rest of the properties
        [Fact]
        public void Update_ShouldChange_Discipline()
        {
            int id = 3;
            var professorName = "New Name Of Professor";
            var affectedRows = this.repository.Update(id, professorName);

            var dbDiscipline = new Discipline();
            using (var connection = this.ApplicationDbContext.Connection)
            {
                var statement = $"SELECT  * FROM {this.DisciplineTableName} WHERE Id = {id}";
                var command = new MySqlCommand(statement, connection);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dbDiscipline.Id = (int) reader["Id"];
                    dbDiscipline.DisciplineName = reader["DisciplineName"].ToString();
                    dbDiscipline.ProfessorName = reader["ProfessorName"].ToString();
                    dbDiscipline.SemesterId = (int) reader["SemesterId"];
                }

                reader.Close();
            }

            Assert.Equal(1, affectedRows);
            Assert.Equal(id, dbDiscipline.Id);
            Assert.Equal(professorName, dbDiscipline.ProfessorName);
        }

        [Fact]
        public void Delete_ShouldRemove_Discpiline()
        {
            int id = 0;

            using (var connection = this.ApplicationDbContext.Connection)
            {
                var statement = $"INSERT INTO  {this.DisciplineTableName} (`DisciplineName`,`ProfessorName`,`SemesterId`) VALUES ('NewDisciplineName', 'NewProfessorName', 1);";
                statement = $"{statement} SELECT Id FROM {this.DisciplineTableName} WHERE Id = LAST_INSERT_ID();";
                var command = new MySqlCommand(statement, connection);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    id = (int) reader["Id"];
                }

                reader.Close();
            }

            Assert.Equal(13, id);

            var affectedRows = this.repository.Delete(id);

            var hasDiscipline = false;
            using (var connection = this.ApplicationDbContext.Connection)
            {
                var statement = $"SELECT  * FROM {this.DisciplineTableName} WHERE Id = {id}";
                var command = new MySqlCommand(statement, connection);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    hasDiscipline = true;
                }

                reader.Close();
            }

            long count = 0;
            using (var connection = this.ApplicationDbContext.Connection)
            {
                var statement = $"SELECT  COUNT(*) as Count FROM {this.DisciplineTableName};";
                var command = new MySqlCommand(statement, connection);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count = (long)reader["Count"];
                }

                reader.Close();
            }

            Assert.Equal(1, affectedRows);
            Assert.False(hasDiscipline);
            Assert.Equal(12, count);
        }
    }
}
