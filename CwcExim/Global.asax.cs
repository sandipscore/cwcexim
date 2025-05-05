using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CwcExim.UtilityClasses;

namespace CwcExim
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
          
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log.Info("Application started at");
        }
        /*protected void Application_PreSendRequestHeaders()
        {

            Response.Headers.Remove("Server");
            Response.Headers.Add("Server", "*");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "0");
        }*/
        protected void Application_PreSendRequestHeaders()
        {

            Response.Headers.Remove("Server");
            Response.Headers.Add("Server", "*");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Expires", "0");

            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
           
        }



        protected void Application_BeginRequest(object sender, EventArgs e)
        {

       //     MvcHandler.DisableMvcResponseHeader = true;
            /*try
            {
                string SSLEnabled = System.Configuration.ConfigurationManager.AppSettings["SSLEnabled"];

                if (SSLEnabled == "Y")
                    EnsureSslAndDomain.SslAndDomain(HttpContext.Current, false, true);
                else
                    EnsureSslAndDomain.SslAndDomain(HttpContext.Current, false, false);
            }
            catch(Exception ex)
            {
                log.Error(ex);

            }*/

            string UrlHost = "";

            


            //log.InfoFormat("Request.Url.Scheme {0} {1}:", Request.Url.Scheme, HttpContext.Current.Request.RawUrl);
            //log.Info("Request.Url.Scheme :" + Request.Url.Scheme);
            UrlHost = Request.Url.Host;

            if (System.Configuration.ConfigurationManager.AppSettings["Environment"] == "P")
            {
                /*if (!UrlHost.Contains("www."))
                    UrlHost= "www."+ Request.Url.Host;*/

                switch (Request.Url.Scheme)
                {

                    case "https":
                        //log.Info("Request.Url :" + Request.Url);
                        Response.AddHeader("Strict-Transport-Security", "max-age=300");
                        break;
                    case "http":
                        //log.Info("Request.Url :" + Request.Url);
                        var path = "https://" + UrlHost + Request.Url.PathAndQuery;

                        //log.Info("Request.Url.Host :" + Request.Url.Host);
                        //log.Info("Request.Url.PathAndQuery :" + Request.Url.PathAndQuery);
                        //log.Info("path :" + path);
                        Response.Status = "301 Moved Permanently";
                        Response.AddHeader("Location", path);
                        break;
                }

            }





        }
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            log.Error(ex);
            //log the error!
            // _Logger.Error(ex);
            Response.RedirectToRoute(ConfigurationManager.AppSettings["MainDomainUrl"].ToString());
        }
        void Session_Start(object sender, EventArgs e)
        {
            //log.InfoFormat("Session started for session id  :{0}", Session.SessionID);
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            //log.InfoFormat("Session ended for session id  :{0}", Session.SessionID);
        }

    }
}
