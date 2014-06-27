using System.Web;
using System.Web.Mvc;

namespace IHS.ZlotePrzeboje
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}