using System.Collections.Generic;
using TabloidMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Utils;
using Microsoft.AspNetCore.SignalR;

namespace TabloidMVC.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IConfiguration configuration) : base(configuration) { }

        public void AddSubscription(Subscription sub)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            Insert Into Subscription (SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime)
                            Output Inserted.Id
                            Values (@subscriberUserProfileId, @providerUserProfileId, @beginDateTime, @endDateTime)";

                    cmd.Parameters.AddWithValue("@subscriberUserProfileId", sub.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@providerUserProfileId", sub.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginDateTime", sub.BeginDateTime);
                    cmd.Parameters.AddWithValue("@endDateTime", sub.EndDateTime);

                    sub.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public Subscription GetSubscriptionByUserIdAndProviderId(int currentUserId, int providerId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            Select * From Subscription s Where s.ProviderUserProfileId = @providerId AND s.SubscriberUserProfileId = @currentUserId"
                    ;

                    cmd.Parameters.AddWithValue("@providerId", providerId);
                    cmd.Parameters.AddWithValue("@currentUserId", currentUserId);

                   var reader = cmd.ExecuteReader();
                    Subscription sub = null;
                    
                        if (reader.Read())
                        {
                            sub = new Subscription()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId")),
                                SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId")),
                                BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime")),
                                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime")),

                            };
                            reader.Close();
                            return sub;
                        }

                    return sub;
                }
            }
        }
    


    }
}
