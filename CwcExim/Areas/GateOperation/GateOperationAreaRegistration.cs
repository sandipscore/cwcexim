using System.Web.Mvc;

namespace CwcExim.Areas.GateOperation
{
    public class GateOperationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GateOperation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GateOperation_default",
                "GateOperation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}