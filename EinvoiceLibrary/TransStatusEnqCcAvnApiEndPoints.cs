using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public static class TransStatusEnqCcAvnApiEndPoints
    {
        private static Dictionary<string, string> dicEndpoints = new Dictionary<string, string>();
        static string environment = "";

        static TransStatusEnqCcAvnApiEndPoints()
        {
            environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();
            if (environment == "P")
            {
                //for production
                dicEndpoints.Add("PCcAvnUrl", "https://api.ccavenue.com/apis/servlet/DoWebTrans");
            }
            else
            {
                // for sandbox
                dicEndpoints.Add("TCcAvnUrl", "https://apitest.ccavenue.com/apis/servlet/DoWebTrans");

            }
        }

        public static string GetEndpoint(string Method)
        {

            return dicEndpoints[Method];
        }

    }
}
