using cultured.server.Infrastructure.Models.Helpers;

namespace cultured.server.Helpers
{
    public class AuthHelpers : AppSettings
    {
        public static bool Authorize(HttpContext context)
        {
            if (ReadBearerToken(context) == GetSecret())
            {
                return true;
            }
            return false;
        }
        public static string? ReadBearerToken(HttpContext context)
        {
            try
            {
                string header = (string)context.Request.Headers["Authorization"];
                if (header != null)
                {
                    return header.Substring(7);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}