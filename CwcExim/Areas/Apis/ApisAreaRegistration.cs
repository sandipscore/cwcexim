using System.Web.Mvc;

namespace CwcExim.Areas.Apis
{
    public class ApisAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Apis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Apis_default",
                "Apis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}