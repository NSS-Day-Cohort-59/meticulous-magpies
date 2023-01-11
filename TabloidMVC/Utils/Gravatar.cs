namespace TabloidMVC.Utils
{
    public class Gravatar
    {
        private const string _defaultImageUrl = "https://static.vecteezy.com/system/resources/thumbnails/010/260/479/small/default-avatar-profile-icon-of-social-media-user-in-clipart-style-vector.jpg";
        public static string GetImageUrl(string email)
        {
            string emailHash = HashUtil.MD5(email.ToLower().Trim());

            return $"https://www.gravatar.com/avatar/{emailHash}?s=200&d={_defaultImageUrl}".ToLower();
        }
    }
}
