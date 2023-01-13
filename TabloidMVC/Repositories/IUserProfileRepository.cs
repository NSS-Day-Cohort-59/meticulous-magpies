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
        int GetRequesterId(int userId, int requestType);
        int RequestsByUserId(int id, int requestType);
        void RequestChange(AdminRequest request);
        void RetireRequest(int userId, int requestType);
        void CancelRequest(int requesterId, int userId, int requestType);
        void Add(UserProfile userProfile);
        void Deactivate (UserProfile userProfile);
        void Activate (UserProfile userProfile);
        void Update(UserProfile userProfile);
    }
}