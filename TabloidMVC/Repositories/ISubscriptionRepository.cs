using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ISubscriptionRepository
    {
        Subscription GetSubscriptionByUserIdAndProviderId(int userId, int providerId);
        void AddSubscription(Subscription sub);

    }
}
