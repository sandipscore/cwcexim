using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public static class QRApiEndPoints
    {
        private static Dictionary<string, string> dicEndpoints = new Dictionary<string, string>();
        static string environment = "";

        static QRApiEndPoints()
        {
            environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();
            if (environment == "P")
            {
                //for production
                dicEndpoints.Add("PQRUrl", "https://upitest.hdfcbank.com/upi/meTranStatusQuery");               
            }
            else
            {                
                // for sandbox
                dicEndpoints.Add("TQRUrl", "https://upitest.hdfcbank.com/upi/meTranStatusQuery");
                
            }
        }

        public static string GetEndpoint(string Method)
        {

            return dicEndpoints[Method];
        }
    }
}
