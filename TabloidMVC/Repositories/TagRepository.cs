using System.Collections.Generic;
using TabloidMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration configuration) : base(configuration) { }
      



        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
Select t.Id, t.Name
From Tag t
Order By t.Name
";
                    var reader = cmd.ExecuteReader();
                    var tags = new List<Tag>();
                    while (reader.Read())
                    {
                        tags.Add(new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                           Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }
                    reader.Close();
                    return tags;
                }
            }
        }
    public void AddTag(Tag tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            Insert Into Tag (Name)
                            Output Inserted.Id
                            Values (@name)";

                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    tag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    public Tag GetTagById(int id)
        {
            using (SqlConnection connection= Connection)
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        Select [Name]
                        From Tag
                        Where Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    return new Tag { Id = id, Name = (string)cmd.ExecuteScalar() };
                }
            }
        }
        public List<int> GetTagsByPostId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Select *
                        From Post p 
                        Join PostTag pt on p.Id = pt.PostId
                        Join Tag t on t.Id = pt.TagId";

                    
                    var reader = cmd.ExecuteReader();
                    var tags = new List<int>();
                    while (reader.Read())
                    {
                        tags.Add(reader.GetInt32(reader.GetOrdinal("TagId")));
                    }
                    reader.Close();
                    return tags;
                }
            }
        }
    public void DeleteTag(Tag tag)
        {
            using (SqlConnection connection= Connection)
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Tag
                            WHERE Id = @id
                            ";

                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    public void UpdateTag(Tag tag)
        {
            using (SqlConnection connection= Connection)
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Tag
                        SET [Name] = @name
                        WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
