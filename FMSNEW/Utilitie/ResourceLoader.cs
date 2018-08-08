using System.Web;

namespace Utilities
{
    public static class ResourceLoader
    {
        public static void SetCurrentThreadCulture(HttpSessionStateBase session)
        {
            if (session != null && session["Culture"] != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = (System.Globalization.CultureInfo)session["Culture"];
                System.Threading.Thread.CurrentThread.CurrentUICulture = (System.Globalization.CultureInfo)session["Culture"];
            }
            else
            {

            }
        }
    }

}
