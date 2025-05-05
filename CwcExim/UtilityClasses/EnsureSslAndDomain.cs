using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.UtilityClasses
{
    public static class EnsureSslAndDomain
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void SslAndDomain(HttpContext context, bool redirectWww, bool redirectSsl)
        {
            if (context == null)
                return;

            var redirect = false;
            var uri = context.Request.Url;
            var scheme = uri.GetComponents(UriComponents.Scheme, UriFormat.Unescaped);
            var host = uri.GetComponents(UriComponents.Host, UriFormat.Unescaped);
            var pathAndQuery = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);

            log.InfoFormat("uri :", uri);
            log.InfoFormat("scheme :", scheme);
            log.InfoFormat("host :", host);
            log.InfoFormat("pathAndQuery :", pathAndQuery);
            //&& !scheme.Equals("https")
            if (redirectSsl )
            {
                scheme = "https";
                //redirect = true;
            }
            else
            {
                scheme = "http";
                //redirect = true;

            }
            redirect = true;
            /*if (redirectWww && !host.StartsWith("www", StringComparison.OrdinalIgnoreCase))
            {
                host = "www." + host;
                redirect = true;
            }*/

            if (redirect)
            {
                log.InfoFormat("AddHeader :", "Location", scheme + "://" + host + pathAndQuery);
                context.Response.Status = "301 Moved Permanently";
                context.Response.StatusCode = 301;
                context.Response.AddHeader("Location", scheme + "://" + host + pathAndQuery);
            }
        }

    }
}