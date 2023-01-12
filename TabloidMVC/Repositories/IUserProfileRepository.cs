using System.Collections.Generic;

using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        List<UserProfile> GetAll();
        UserProfile GetByEmail(string email);
        UserProfile GetById(int id);
        int GetAdminCount();
        int GetRequesterId(int userId, byte requestType);
        byte RequestsByUserId(int id, byte requestType);
        void RequestChange(AdminRequest request);
        void RetireRequest(int userId, byte requestType);
        void CancelRequest(int requesterId, int userId, byte requestType);
        void Add(UserProfile userProfile);
        void Deactivate (UserProfile userProfile);
        void Activate (UserProfile userProfile);
        void Update(UserProfile userProfile);
    }
}