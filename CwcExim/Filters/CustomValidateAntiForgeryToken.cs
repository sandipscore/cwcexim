using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CwcExim.Filters
{
    public class CustomValidateAntiForgeryToken:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext FilterContext)
        {
            var Request = FilterContext.HttpContext.Request;

            //  Only validate POSTs
            if (Request.HttpMethod == WebRequestMethods.Http.Post)
            {
                //  Ajax POSTs and normal form posts have to be treated differently when it comes
                //  to validating the AntiForgeryToken
                if (Request.IsAjaxRequest())
                {
                    var antiForgeryCookie = Request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;
                    try
                    {
                        AntiForgery.Validate(cookieValue, Request.Headers["__RequestVerificationToken"]);
                    }
                    catch (HttpAntiForgeryException ex)
                    {

                        throw ex;
                    }

                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(FilterContext);
                }
            }
        }
    }
}