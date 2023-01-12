
using System.IO;
using System.Linq;
using System;

namespace TabloidMVC.Utils
{
    public class UrlUtil
    {
        public static bool IsValidImage(string url)
        {
            //! Checks if the ImageLocation provided by user is a valid URL
            Uri uriResult;
            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isValidUrl) return false;

            //! Now check if the URL contains a valid image url extension
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            bool isValidExtension = imageExtensions.Contains(Path.GetExtension(url).ToLowerInvariant());

            return isValidExtension;
        }
    }
}
