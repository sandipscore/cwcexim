using System.Web.Mvc;

namespace CwcExim.Areas.SLA
{
    public class SLAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SLA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SLA_default",
                "SLA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}