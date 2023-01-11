using System.Collections.Generic;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class UserTypeRepository : BaseRepository, IUserTypeRepository
    {
        public UserTypeRepository(IConfiguration config) : base(config) { }
        public List<UserType> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, [Name] FROM UserType";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<UserType> userTypes = new List<UserType>();

                        while (reader.Read())
                        {
                            UserType newType = new()
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Id = reader.GetInt32(reader.GetOrdinal("Id"))
                            };
                            userTypes.Add(newType);
                        }

                        return userTypes;
                    }
                }
            }
        }
    }
}
