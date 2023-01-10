using System.Collections.Generic;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration config) : base(config) { }
        public List<Category> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, [Name] FROM Category ORDER BY [Name]";
                    var reader = cmd.ExecuteReader();

                    var categories = new List<Category>();

                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                        });
                    }

                    reader.Close();

                    return categories;
                }
            }
        }
        public Category GetById(int id)
        {
            using (SqlConnection conn = Connection) 
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT [Name]
                        FROM Category
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    return new Category { Id = id, Name = (string)cmd.ExecuteScalar() };
                }
            }
        }
        public void Add(Category category)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Category ([Name])
                        VALUES (@name)
                    ";
                    cmd.Parameters.AddWithValue("@name", category.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(Category category)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Category
                        WHERE Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", category.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Category category)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Category
                        SET [Name] = @name
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", category.Id);
                    cmd.Parameters.AddWithValue("@name", category.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
