using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MiddleTask.Data
{
    public class PeopleRepository
    {
        private string _connectionString =
            @"Server=(LocalDb)\MSSQLLocalDB;Database=MiddleTask.Database;Trusted_Connection=True;";

        public People GetPeople(int id)
        {
            var query = "SELECT * FROM People WHERE Id = @id";

            using IDbConnection db = new SqlConnection(_connectionString);
            {
                return db.QueryFirstOrDefault<People>(query, new {id});
            }
        }

        public void UpdatePeople(People people)
        {
            var query = "UPDATE People SET Id=@Id, FullName=@FullName, City=@City, Email=@Email, PhoneNumber=@PhoneNumber," +
                        " DateChange=@DateChange WHERE Id=@Id";

            using IDbConnection db = new SqlConnection(_connectionString); 
            db.Execute(query, people);
        }

        public void AddPeople(People people)
        {
            var query = "INSERT INTO People (Id, FullName, City, Email, PhoneNumber, DateChange)" +
                        "VALUES(@Id, @FullName, @City, @Email, @PhoneNumber, @DateChange)";

            using IDbConnection db = new SqlConnection(_connectionString);
            db.Execute(query, people);
        }
    }
}
