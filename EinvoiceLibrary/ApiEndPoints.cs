using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
   public static class ApiEndPoints
    {
        private static Dictionary<string, string> dicEndpoints = new Dictionary<string, string>();
       static string environment = "";

        static ApiEndPoints()
        {
            environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();
            if (environment == "P")
            {

                //for production
                dicEndpoints.Add("GetToken", "https://api.einvoice1.gst.gov.in/eivital/v1.04/auth");
                dicEndpoints.Add("GenerateEinvoice", "https://api.einvoice1.gst.gov.in/eicore/v1.03/Invoice");
                dicEndpoints.Add("CancelEinvoice", "https://api.einvoice1.gst.gov.in/eicore/v1.03/Invoice/Cancel");
                dicEndpoints.Add("GetEInvoiceByIRN", "https://api.einvoice1.gst.gov.in/eicore/v1.03/Invoice/irn/irn_no");
                dicEndpoints.Add("GetGSTINDetails", "https://api.einvoice1.gst.gov.in/eivital/v1.03/Master/gstin/gstin_no");
                dicEndpoints.Add("GenerateEwaybillByIrn", "https://api.einvoice1.gst.gov.in/einvewb/v1.03/ewaybill");
                dicEndpoints.Add("CancelEwaybill", "https://api.einvoice1.gst.gov.in/ewaybillapi/v1.03/ewayapi");

            }
            else
            {



                // for sandbox
                dicEndpoints.Add("GetToken", "https://einv-apisandbox.nic.in/eivital/v1.04/auth");
                dicEndpoints.Add("GenerateEinvoice", "https://einv-apisandbox.nic.in/eicore/v1.03/Invoice");
                dicEndpoints.Add("CancelEinvoice", "https://einv-apisandbox.nic.in/eicore/v1.03/Invoice/Cancel");
                dicEndpoints.Add("GetEInvoiceByIRN", "https://einv-apisandbox.nic.in/eicore/v1.03/Invoice/irn/irn_no");
                dicEndpoints.Add("GetGSTINDetails", "https://einv-apisandbox.nic.in/eivital/v1.03/Master/gstin/gstin_no");
                dicEndpoints.Add("GenerateEwaybillByIrn", "https://einv-apisandbox.nic.in/eiewb/v1.03/ewaybill");
                dicEndpoints.Add("CancelEwaybill", "https://einv-apisandbox.nic.in/ewaybillapi/v1.03/ewayapi");
            }
        }

        public static string GetEndpoint(string Method)
        {

            return dicEndpoints[Method];
        }

    }
}
