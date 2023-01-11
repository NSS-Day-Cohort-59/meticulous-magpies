using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                            u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.IsActive,
                            ut.[Name] AS UserTypeName
                        FROM UserProfile u
                        LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        ORDER BY u.DisplayName
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<UserProfile> userProfiles = null;


                        if (reader.Read())
                        {
                            userProfiles = new List<UserProfile>();

                            UserProfile newUserProfile = new()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                UserType = new UserType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                },
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                            userProfiles.Add(newUserProfile);
                        }

                        while (reader.Read())
                        {
                            UserProfile newUserProfile = new()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                UserType = new UserType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                },
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                            userProfiles.Add(newUserProfile);
                        }

                        return userProfiles;
                    }
                }
            }
        }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.IsActive,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        public UserProfile GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.IsActive,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE u.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        UserProfile userProfile = null;

                        if (reader.Read())
                        {
                            userProfile = new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                UserType = new UserType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                },
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }

                        reader.Close();

                        return userProfile;
                    }
                }


            }
        }
        public int GetAdminCount()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT COUNT(uP.Id)
                        FROM UserProfile uP
                        LEFT JOIN UserType uT ON uT.Id = uP.UserTypeId
                        WHERE uT.[Name] = 'Admin'
                    ";

                    int amountOfAdmins = (int)cmd.ExecuteScalar(); // Cast type of int
                    
                    return amountOfAdmins;
                }
            }
        }
        public void Add(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand()) 
                {
                    //!~ Note we do not provide IsActive because it has a default value in the DB ~
                    cmd.CommandText = @"
                        INSERT INTO UserProfile (DisplayName, FirstName, LastName, Email, CreateDateTime, ImageLocation, UserTypeId)
                        VALUES (@displayName, @firstName, @lastName, @email, @createDateTime, @imageLocation, @userTypeId)
                    ";

                    cmd.Parameters.AddWithValue("@displayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@firstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@displayName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@displayName", userProfile.Email);
                    cmd.Parameters.AddWithValue("@displayName", userProfile.CreateDateTime);

                    //! Checks if the ImageLocation provided by user is a valid URL
                    Uri uriResult;
                    bool result = Uri.TryCreate(userProfile.ImageLocation, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    cmd.Parameters.AddWithValue("@displayName", result ? userProfile.ImageLocation : Gravatar.GetImageUrl(userProfile.Email)); //! Unless user provides a valid image URL, we default to Gravatar
                    
                }
            }
        }
        public void Deactivate(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                        SET IsActive = 0
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Activate(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                        SET IsActive = 1
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                        SET FirstName = @firstName,
                            LastName = @lastName,
                            DisplayName = @displayName,
                            Email = @email,
                            CreateDateTime = @createDateTime,
                            ImageLocation = @imageLocation,
                            UserTypeId = @userTypeId
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@firstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@displayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@createDateTime", userProfile.CreateDateTime);
                    cmd.Parameters.AddWithValue("@imageLocation", !string.IsNullOrWhiteSpace(userProfile.ImageLocation) ? userProfile.ImageLocation : DBNull.Value);
                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserTypeId);
                    cmd.Parameters.AddWithValue("@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
