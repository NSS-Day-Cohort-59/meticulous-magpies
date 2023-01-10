using System.Collections.Generic;

using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        List<UserProfile> GetAll();
        UserProfile GetByEmail(string email);
        UserProfile GetById(int id);
        void Deactivate (UserProfile profile);
        void Activate (UserProfile profile);
        void Update(UserProfile profile);
    }
}