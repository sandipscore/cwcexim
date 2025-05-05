using CCA.Util;
using CwcExim.Models;
using CwcExim.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EinvoiceLibrary;
namespace CwcExim.Controllers
{
    public class PaymentGatewayController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        CCACrypto ccaCrypto = new CCACrypto();
        string workingKey = "";// "F52847EDA0C715911416D9054623BB3E";//put in the 32bit alpha numeric key in the quotes provided here 	
        string ccaRequest = "";
        public string strEncRequest = "";
        public string strAccessCode = "";// "AVXH61IL68AO51HXOA";// put the access key in the quotes provided here.

        public PaymentGatewayController()
        {
            workingKey = UPIConfiguration.WorkingKey;//  "F52847EDA0C715911416D9054623BB3E";
            strAccessCode = UPIConfiguration.AccessCode;//  "AVXH61IL68AO51HXOA";




        }

        // GET: PaymentGateway
        public ActionResult Index()
        {
            CcavenueRepository ObjIR = new CcavenueRepository();
            long OrderId = Convert.ToInt64(Session["OrderId"]);
            var Uid = ((Login)Session["LoginUser"]).Uid;
            ObjIR.GetOnlinePayACK(OrderId, Uid);
                   

            PaymentGatewayRequest objPGR = (PaymentGatewayRequest)ObjIR.DBResponse.Data;

            //Save Session Data in Database
            Login logindata = ((Login)Session["LoginUser"]);

            CcavenueRepository obj = new CcavenueRepository();
            obj.AddEditSessionForpayment(logindata, OrderId);

            //END

            objPGR.merchant_param1 = Convert.ToInt32(Session["BranchId"]);
            return PartialView(objPGR);
        }

        [HttpPost]
        public async Task<ActionResult> DoPayment(FormCollection data)
        {
            CcavenueRepository ObjIR = new CcavenueRepository();
            log.Info("Payment Request DoPayment POST Start");
            log.Info("Payment Request ccaRequest string creation Start");
            ccaRequest = "";
            foreach (string name in Request.Form)
            {
                if (name != null)
                {
                    if (!name.StartsWith("_"))
                    {
                        ccaRequest = ccaRequest + name + "=" + Request.Form[name] + "&";
                        /* Response.Write(name + "=" + Request.Form[name]);
                          Response.Write("</br>");*/
                    }
                }
            }
            log.Info("Payment Request ccaRequest string creation End");
            PaymentGatewayRequest objPGR = new PaymentGatewayRequest();
            objPGR.tid = Convert.ToDecimal(Request.Form["tid"]);
            objPGR.merchant_id = Convert.ToDecimal(Request.Form["merchant_id"]);
            objPGR.order_id = Convert.ToInt64(Request.Form["order_id"]);
            objPGR.amount = Convert.ToDecimal(Request.Form["amount"]);
            objPGR.currency = Convert.ToString(Request.Form["currency"]);
            objPGR.redirect_url = Convert.ToString(Request.Form["redirect_url"]);
            objPGR.cancel_url = Convert.ToString(Request.Form["cancel_url"]);
            objPGR.language = Convert.ToString(Request.Form["language"]);
            objPGR.billing_name = Convert.ToString(Request.Form["billing_name"]);
            objPGR.billing_address = Convert.ToString(Request.Form["billing_address"]);
            objPGR.billing_city = Convert.ToString(Request.Form["billing_city"]);
            objPGR.billing_state = Convert.ToString(Request.Form["billing_state"]);
            objPGR.billing_zip = Convert.ToString(Request.Form["billing_zip"]);
            objPGR.billing_country = Convert.ToString(Request.Form["billing_country"]);
            objPGR.billing_tel = Convert.ToDecimal(Request.Form["billing_tel"]);
            objPGR.billing_email = Convert.ToString(Request.Form["billing_email"]);
            objPGR.delivery_name = Convert.ToString(Request.Form["delivery_name"]);
            objPGR.delivery_address = Convert.ToString(Request.Form["delivery_address"]);
            objPGR.delivery_city = Convert.ToString(Request.Form["delivery_city"]);
            objPGR.delivery_state = Convert.ToString(Request.Form["delivery_state"]);
            objPGR.delivery_zip = Convert.ToString(Request.Form["delivery_zip"]);
            objPGR.delivery_country = Convert.ToString(Request.Form["delivery_country"]);
            objPGR.delivery_tel = Convert.ToDecimal(Request.Form["delivery_tel"]);
            objPGR.issuing_bank = Convert.ToString(Request.Form["issuing_bank"]);
            objPGR.mobile_number = Convert.ToDecimal(Request.Form["mobile_number"]);

            log.Info("Payment Request AddPaymentGatewayRequest call start");
            ObjIR.AddPaymentGatewayRequest(objPGR);
            log.Info("Payment Request AddPaymentGatewayRequest call end");

            log.Info("Payment Request ccaRequest string Encryption Start");

            log.Info("workingKey :"+ workingKey);
            log.Info("strAccessCode :" + strAccessCode);
            log.Info("ccaRequest :" + ccaRequest);


            strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);
            log.Info("strEncRequest :" + strEncRequest);

            log.Info("Payment Request ccaRequest string Encryption End");
            //var strdata = new StringContent("{\"encRequest\":\"" + strEncRequest + "\",\"access_code\":\"AVXH61IL68AO51HXOA\"}", Encoding.UTF8, "application/json");
            //var strdata = new StringContent("encRequest=" + strEncRequest + "&access_code=" + strAccessCode, Encoding.UTF8, "application/json");
            // strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);
            //HttpClient vClient = new HttpClient();
            //var vResponse =await vClient.PostAsync("https://test.ccavenue.com/transaction/transaction.do?command=initiatePayloadTransaction",strdata);
            //var vResponse = "";
            //Response.Redirect("https://test.ccavenue.com/transaction/transaction.do?command=initiatePayloadTransaction"+ "&encRequest=" + strEncRequest + "&access_code=" + strAccessCode);
            //out.print(vResponse);//writes payload on browser to open bank pag
            ViewBag.EncRequest = strEncRequest;
            ViewBag.AccessCode = strAccessCode;
            log.Info("Payment Request DoPayment POST End");
            return View("~\\Views\\PaymentGateway\\DoPayment.cshtml");
        }
    }
}