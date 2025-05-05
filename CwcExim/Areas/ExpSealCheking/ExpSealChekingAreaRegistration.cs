using System.Web.Mvc;

namespace CwcExim.Areas.ExpSealCheking
{
    public class ExpSealChekingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ExpSealCheking";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ExpSealCheking_default",
                "ExpSealCheking/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}