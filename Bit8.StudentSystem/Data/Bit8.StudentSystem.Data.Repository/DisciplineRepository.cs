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
     
        public DisciplineRepository(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public ICollection<Discipline> All()
        {
            var statement = $"SELECT * FROM {DisciplineTableName};";
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

        public void Add()
        {

        }

        public void Update()
        {

        }

        public void Delete()
        {

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
