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

        private readonly IApplicationDbContext applicationDbContext;
        private readonly IDisciplineRepository repository;
        public DisciplineRepositoryTests()
        {
            this.applicationDbContext = new ApplicationDbContext(Configuration["ConnectionStrings:ApplicationDbContext"]);
            this.applicationDbContext.Initialize();
            this.repository = new DisciplineRepository(this.applicationDbContext);

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
            using (var connection = this.applicationDbContext.Connection)
            {
                var statement = $"SELECT  d.*, s.Name, s.StartDate, s.EndDate  FROM {applicationDbContext.GetDatabaseName()}.discipline d";
                   statement += $"{statement} LEFT JOIN {applicationDbContext.GetDatabaseName()}.semester s ON s.Id = d.SemesterId WHERE d.Id = {id};";
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
            using (var connection = this.applicationDbContext.Connection)
            {
                var statement = $"SELECT  COUNT(*) as Count  FROM {applicationDbContext.GetDatabaseName()}.discipline;";
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
            long count = 0;
            using (var connection = this.applicationDbContext.Connection)
            {
                var statement = $"SELECT  COUNT(*) as Count  FROM {applicationDbContext.GetDatabaseName()}.discipline;";
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
    }
}
