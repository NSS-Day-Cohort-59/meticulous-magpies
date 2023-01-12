using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

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
                    cmd.Parameters.AddWithValue("@lastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@createDateTime", userProfile.CreateDateTime);
                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserTypeId);

                    //! If the user passed a valid imageUrl, USE IT
                    if (!string.IsNullOrWhiteSpace(userProfile.ImageLocation) && UrlUtil.IsValidImage(userProfile.Email))
                    {
                        cmd.Parameters.AddWithValue("@imageLocation", userProfile.ImageLocation);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imageLocation", GravatarUtil.GetImageUrl(userProfile.Email));
                    }

                    cmd.ExecuteNonQuery();
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
                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserTypeId);
                    cmd.Parameters.AddWithValue("@id", userProfile.Id);

                    //! If the user passed a valid imageUrl, USE IT
                    if (!string.IsNullOrWhiteSpace(userProfile.ImageLocation) && UrlUtil.IsValidImage(userProfile.Email))
                    {
                        cmd.Parameters.AddWithValue("@imageLocation", userProfile.ImageLocation);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imageLocation", GravatarUtil.GetImageUrl(userProfile.Email));
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void RequestChange(AdminRequest request)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO dbo.[AdminRequest] (RequesterUserProfileId, TargetUserProfileId, Demote, Deactivate, CreateDateTime)
                        VALUES (@requesterId, @targetId, @demote, @deactivate, @createDateTime)
                    ";

                    cmd.Parameters.AddWithValue("@requesterId", request.RequesterUserProfileId);
                    cmd.Parameters.AddWithValue("@targetId", request.TargetUserProfileId);
                    cmd.Parameters.AddWithValue("@demote", request.Demote ? 1 : 0);
                    cmd.Parameters.AddWithValue("@deactivate", request.Deactivate ? 1 : 0);
                    cmd.Parameters.AddWithValue("@createDateTime", request.CreateDateTime ?? DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public byte RequestsByUserId(int userId, byte requestType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"
                        SELECT CAST(COUNT(Id) AS tinyint) 
                        FROM dbo.AdminRequest
                        WHERE TargetUserProfileId = @userId 
                            AND {(requestType == 0 ? "Demote" : "Deactivate")} = 1
                            AND IsCancelled = 0
                            AND CloseDateTime IS NULL
                    ";

                    cmd.Parameters.AddWithValue("@userId", userId);

                    byte requests = (byte)cmd.ExecuteScalar();
                    return requests;
                }
            }
        }
        public void RetireRequest(int userId, byte requestType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"
                        UPDATE dbo.AdminRequest
                        SET CloseDateTime = @closeDateTime
                        WHERE TargetUserProfileId = @userId 
                            AND {(requestType == 0 ? "Demote" : "Deactivate")} = 1
                            AND IsCancelled = 0
                            AND CloseDateTime IS NULL
                    ";

                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@closeDateTime", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public int GetRequesterId(int targetId, byte requestType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"
                        SELECT RequesterUserProfileId
                        FROM dbo.AdminRequest
                        WHERE {(requestType == 0 ? "Demote" : "Deactivate")} = 1
                            AND TargetUserProfileId = @targetId
                            AND IsCancelled = 0
                            AND CloseDateTime IS NULL
                    ";

                    cmd.Parameters.AddWithValue("@targetId", targetId);

                    int requesterId = (int)cmd.ExecuteScalar();
                    return requesterId;
                }
            }
        }
        public void CancelRequest(int requesterId, int userId, byte requestType) 
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"
                        UPDATE dbo.[AdminRequest]
                        SET [CloseDateTime] = @closeDateTime,
                            [IsCancelled] = 1
                        WHERE [CloseDateTime] IS NULL
                            AND [RequesterUserProfileId] = @requesterId
                            AND [TargetUserProfileId] = @userId
                            AND IsCancelled = 0
                            AND {(requestType == 0 ? "Demote" : "Deactivate")} = 1
                    ";

                    cmd.Parameters.AddWithValue("@closeDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@requesterId", requesterId);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
