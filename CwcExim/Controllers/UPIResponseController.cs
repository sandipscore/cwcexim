using CCA.Util;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using EinvoiceLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CwcExim.Controllers
{
    //[RoutePrefix("UPIResponse")]
    public class UPIResponseController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        

        public UPIResponseController()
        {
            //workingKey = UPIConfiguration.WorkingKey;//  "F52847EDA0C715911416D9054623BB3E";           

        }
        // GET: UPIResponse
        [HttpPost]
       
        [Route("~/UPIResponse/GetResponseBqr")]
        public ActionResult GetResponseBqr(FormCollection resMsg)
        {
            PaymentGatewayResponse objPGR = new PaymentGatewayResponse();
            try
            {
                CCACrypto ccaCrypto = new CCACrypto();
            string workingKey = "";
            workingKey = UPIConfiguration.WorkingKeyBQR;
            log.Info("Payment Response GetResponseBQR Method Start");

            string encResp = "";
            string decResp = "";
            string orderNo = "";
            string ccaRequest = "";
            string crossSellUrl = "";
                /*foreach (string name in Request.Form)
                {
                    if (name != null)
                    {
                        if (!name.StartsWith("_"))
                        {
                            ccaRequest = ccaRequest + name + "=" + Request.Form[name] + "&";

                        }
                    }
                }*/

                  encResp = Request.Form["encResp"];

                     decResp = ccaCrypto.Decrypt(encResp, workingKey);
                
                
                         
                log.Info("Payment Response Decrypt response data" + decResp);

                CcavenueRepository ObjIR = new CcavenueRepository();
              

                string[] respAttribute = decResp.Replace("null", "").Split('&');

                foreach (var item in respAttribute)
                {
                    string[] value = item.Split('=');
                    if (value.Length > 1)
                    {
                        if (item.Contains("order_id"))
                        {
                            objPGR.order_id = Convert.ToInt64(value[1]);
                        }
                        else if (item.Contains("tracking_id"))
                        {
                            objPGR.tracking_id = Convert.ToDecimal(value[1]);
                        }
                        else if (item.Contains("bank_ref_no"))
                        {
                            objPGR.bank_ref_no = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("order_status"))
                        {
                            objPGR.order_status = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("failure_message"))
                        {
                            objPGR.failure_message = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("payment_mode"))
                        {
                            objPGR.payment_mode = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("status_code"))
                        {
                            objPGR.status_code = ((short)(string.IsNullOrEmpty(value[1]) ? 0 : Convert.ToInt16(value[1])));
                        }
                        else if (item.Contains("status_message"))
                        {
                            objPGR.status_message = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("currency"))
                        {
                            objPGR.currency = Convert.ToString(value[1]);

                        }
                        else if (item.Contains("amount"))
                        {
                            objPGR.amount = String.IsNullOrEmpty(value[1]) == true ? 0 : Convert.ToDecimal(value[1]);

                        }
                        else if (item.Contains("billing_name"))
                        {
                            objPGR.billing_name = Convert.ToString(value[1]);

                        }
                        else if (item.Contains("billing_address"))
                        {
                            objPGR.billing_address = Convert.ToString(value[1]);

                        }
                        else if (item.Contains("billing_city"))
                        {
                            objPGR.billing_city = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("billing_state"))
                        {
                            objPGR.billing_state = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("billing_zip"))
                        {
                            objPGR.billing_zip = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("billing_country"))
                        {
                            objPGR.billing_country = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("billing_tel"))
                        {
                            objPGR.billing_tel =String.IsNullOrEmpty(value[1])==true?0: Convert.ToDecimal(value[1]);
                        }
                        else if (item.Contains("billing_email"))
                        {
                            objPGR.billing_email = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("delivery_name"))
                        {
                            objPGR.delivery_name = Convert.ToString(value);
                        }
                        else if (item.Contains("delivery_address"))
                        {
                            objPGR.delivery_address = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("delivery_city"))
                        {
                            objPGR.delivery_city = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("delivery_state"))
                        {
                            objPGR.delivery_state = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("delivery_zip"))
                        {
                            objPGR.delivery_zip = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("delivery_country"))
                        {
                            objPGR.delivery_country = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("delivery_tel"))
                        {
                            objPGR.delivery_tel = String.IsNullOrEmpty(value[1]) == true ? 0 : Convert.ToDecimal(value[1]);
                        }
                        else if (item.Contains("vault"))
                        {
                            objPGR.vault = (string.IsNullOrEmpty(value[1]) ? char.MinValue : Convert.ToChar(value[1]));
                        }
                        else if (item.Contains("offer_type"))
                        {
                            objPGR.offer_type = string.IsNullOrEmpty(value[1]) ? string.Empty : Convert.ToString(value[1]);
                        }
                        else if (item.Contains("offer_code"))
                        {
                            objPGR.offer_code = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("discount_value"))
                        {
                            objPGR.discount_value = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                        }
                        else if (item.Contains("mer_amount"))
                        {
                            objPGR.mer_amount = String.IsNullOrEmpty(value[1]) == true ? 0 : Convert.ToDecimal(value[1]);
                        }
                        else if (item.Contains("eci_value"))
                        {
                            objPGR.eci_value = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                        }
                        else if (item.Contains("retry"))
                        {
                            objPGR.retry = string.IsNullOrEmpty(value[1]) ? char.MinValue : Convert.ToChar(value[1]);
                        }
                        else if (item.Contains("response_code"))
                        {
                            objPGR.response_code = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                        }
                        else if (item.Contains("billing_notes"))
                        {
                            objPGR.billing_notes = Convert.ToString(value[1]);
                        }
                        else if (item.Contains("trans_date"))
                        {
                            objPGR.trans_date = string.IsNullOrEmpty(value[1]) ? DateTime.Now : Convert.ToDateTime(value[1]);
                        }
                        else if (item.Contains("merchant_param1"))
                        {
                            objPGR.merchant_param1 = String.IsNullOrEmpty(value[1]) == true ? 0 : Convert.ToInt32(value[1]); // Convert.ToInt32(value[1]);  // Branch id store
                        }
                        else if (item.Contains("bin_country"))
                        {
                            objPGR.bin_country = Convert.ToString(value[1]);
                        }
                    }
                }
                objPGR.merchant_param1=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BranchId"]);
                log.Info("Payment Response initlize connection string after session out Start");

                //To initlize connection string after session out
                SaveSessionData(objPGR.merchant_param1);
                log.Info("Payment Response initlize connection string after session out End");

                log.Info("Payment Response DB Save Start");
                CcavenueRepository ObjCash = new CcavenueRepository();
                log.Info("Payment Response Checking Before ");
                ObjCash.CheckResponceOrderIdAndAmountBqr(objPGR.order_id.ToString(), objPGR.amount);

                if (ObjCash.DBResponse.Data.Equals(1))
                {
                    log.Info("Payment Response Checking Complete ");
                    ObjIR.AddPaymentGatewayResponse(objPGR, 1);
                    log.Info("Payment Response DB Save End");

                }
                else
                {
                    log.Info("Payment Response temparing start ");
                    CcavenueRepository ObjIRR = new CcavenueRepository();
                    ObjIR.AddPaymentGatewayResponseTemparing(objPGR);
                    objPGR.order_status = "Error";
                }
                /*if (ObjIR.DBResponse.Status.Equals(1))
                {
                    objPGR.PageFor = "Error";
                }
                orderNo = Request.Form["orderNo"];
                crossSellUrl = Request.Form["crossSellUrl"];

                if (objPGR.order_status == "Success")
                {
                    objPGR.PageFor = "Success";
                }
                else
                {
                    objPGR.PageFor = "Failure";
                }*/
                log.Info("Payment Response GetResponse Method End");
            }
            catch(Exception ex)
            {

            }
            return PartialView(objPGR);





        }
        // GET: UPIResponse
        [HttpPost]
        [Route("~/UPIResponse/GetResponse")]
        public ActionResult GetResponse(FormCollection resMsg)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            string workingKey = "";
            workingKey = UPIConfiguration.WorkingKey;
            log.Info("Payment Response GetResponse Method Start");

            string encResp = "";
            string decResp = "";
            string orderNo = "";
            string ccaRequest = "";
            string crossSellUrl = "";
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

            encResp = Request.Form["encResp"];
            decResp = ccaCrypto.Decrypt(encResp, workingKey);

            log.Info("Payment Response Decrypt response data"+ decResp);

            CcavenueRepository ObjIR = new CcavenueRepository();
            PaymentGatewayResponse objPGR = new PaymentGatewayResponse();
      
            string[] respAttribute = decResp.Replace("null", "").Split('&');

            foreach (var item in respAttribute)
            {
                string[] value = item.Split('=');
                if (value.Length > 1)
                {
                    if (item.Contains("order_id"))
                    {
                        objPGR.order_id = Convert.ToInt64(value[1]);
                    }
                    else if (item.Contains("tracking_id"))
                    {
                        objPGR.tracking_id = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("bank_ref_no"))
                    {
                        objPGR.bank_ref_no = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("order_status"))
                    {
                        objPGR.order_status = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("failure_message"))
                    {
                        objPGR.failure_message = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("payment_mode"))
                    {
                        objPGR.payment_mode = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("status_code"))
                    {
                        objPGR.status_code = ((short)(string.IsNullOrEmpty(value[1]) ? 0 : Convert.ToInt16(value[1])));
                    }
                    else if (item.Contains("status_message"))
                    {
                        objPGR.status_message = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("currency"))
                    {
                        objPGR.currency = Convert.ToString(value[1]);

                    }
                    else if (item.Contains("amount"))
                    {
                        objPGR.amount = Convert.ToDecimal(value[1]);

                    }
                    else if (item.Contains("billing_name"))
                    {
                        objPGR.billing_name = Convert.ToString(value[1]);

                    }
                    else if (item.Contains("billing_address"))
                    {
                        objPGR.billing_address = Convert.ToString(value[1]);

                    }
                    else if (item.Contains("billing_city"))
                    {
                        objPGR.billing_city = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_state"))
                    {
                        objPGR.billing_state = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_zip"))
                    {
                        objPGR.billing_zip = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_country"))
                    {
                        objPGR.billing_country = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_tel"))
                    {
                        objPGR.billing_tel = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("billing_email"))
                    {
                        objPGR.billing_email = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_name"))
                    {
                        objPGR.delivery_name = Convert.ToString(value);
                    }
                    else if (item.Contains("delivery_address"))
                    {
                        objPGR.delivery_address = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_city"))
                    {
                        objPGR.delivery_city = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_state"))
                    {
                        objPGR.delivery_state = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_zip"))
                    {
                        objPGR.delivery_zip = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_country"))
                    {
                        objPGR.delivery_country = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_tel"))
                    {
                        objPGR.delivery_tel = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("vault"))
                    {
                        objPGR.vault = (string.IsNullOrEmpty(value[1]) ? char.MinValue : Convert.ToChar(value[1]));
                    }
                    else if (item.Contains("offer_type"))
                    {
                        objPGR.offer_type = string.IsNullOrEmpty(value[1]) ? string.Empty : Convert.ToString(value[1]);
                    }
                    else if (item.Contains("offer_code"))
                    {
                        objPGR.offer_code = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("discount_value"))
                    {
                        objPGR.discount_value = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("mer_amount"))
                    {
                        objPGR.mer_amount = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("eci_value"))
                    {
                        objPGR.eci_value = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("retry"))
                    {
                        objPGR.retry = string.IsNullOrEmpty(value[1]) ? char.MinValue : Convert.ToChar(value[1]);
                    }
                    else if (item.Contains("response_code"))
                    {
                        objPGR.response_code = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("billing_notes"))
                    {
                        objPGR.billing_notes = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("trans_date"))
                    {
                        objPGR.trans_date = string.IsNullOrEmpty(value[1]) ? DateTime.Now : Convert.ToDateTime(value[1]);
                    }
                    else if (item.Contains("merchant_param1"))
                    {
                        objPGR.merchant_param1 = Convert.ToInt32(value[1]);  // Branch id store
                    }
                    else if (item.Contains("bin_country"))
                    {
                        objPGR.bin_country = Convert.ToString(value[1]);  
                    }
                }
            }

            log.Info("Payment Response initlize connection string after session out Start");
            objPGR.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BranchId"]);
            //To initlize connection string after session out
            SaveSessionData(objPGR.merchant_param1);
            log.Info("Payment Response initlize connection string after session out End");


            CcavenueRepository ObjCash = new CcavenueRepository();
            ObjCash.CheckResponceOrderIdAndAmount(objPGR.order_id.ToString(), objPGR.amount);
            if (ObjCash.DBResponse.Data.Equals(1))
            {
                log.Info("Payment Response DB Save Start");
                ObjIR.AddPaymentGatewayResponse(objPGR);
                if (ObjIR.DBResponse.Status.Equals(1))
                {
                    objPGR.PageFor = "Error";
                }
                log.Info("Payment Response DB Save End");

            }
            else
            {
                objPGR.order_status = "Error";
            }

            orderNo = Request.Form["orderNo"];
            crossSellUrl = Request.Form["crossSellUrl"];

            if (objPGR.order_status == "Success")
            {
                objPGR.PageFor = "Success";
            }
            else
            {
                objPGR.PageFor = "Failure";
            }
            log.Info("Payment Response GetResponse Method End");

            //Set Session from DB
            CcavenueRepository obj = new CcavenueRepository();
            obj.GetSessionDetailsFromDB(objPGR.order_id);
            if(obj.DBResponse.Status==1)
            {
                Login user = (Login)obj.DBResponse.Data;
                HttpContext.Session["LoginUser"] = user;
                System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
                manager.RemoveSessionID(System.Web.HttpContext.Current);
                var SessionId = manager.CreateSessionID(System.Web.HttpContext.Current);
                Session["SessionId"] = SessionId;

            }


            //end 


            return PartialView(objPGR);



            // return Json("");
        }

        [HttpPost]
        [Route("~/UPIResponse/CancelResponse")]
        public ActionResult CancelResponse(FormCollection request)
        {
            log.Info("Payment Response Cancel Method Start");
            string workingKey = "";
            workingKey = UPIConfiguration.WorkingKey;
            CCACrypto ccaCrypto = new CCACrypto();
            string encResp = "";
            string decResp = "";
            string orderNo = "";
            string ccaRequest = "";
            string crossSellUrl = "";
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
            //long OrderId = Convert.ToInt64(Session["OrderId"]);
            encResp = Request.Form["encResp"];
            decResp = ccaCrypto.Decrypt(encResp, workingKey);
            log.Info("Payment Response Decrypt response data" + decResp);

            orderNo = Request.Form["orderNo"];
            crossSellUrl = Request.Form["crossSellUrl"];

            string[] respAttribute = decResp.Replace("null", "").Split('&');
            PaymentGatewayResponse objPGR = new PaymentGatewayResponse();
            CcavenueRepository ObjIR = new CcavenueRepository();
            foreach (var item in respAttribute)
            {
                string[] value = item.Split('=');
                if (value.Length > 1)
                {
                    if (item.Contains("order_id"))
                    {
                        objPGR.order_id = Convert.ToInt64(value[1]);
                    }
                    else if (item.Contains("tracking_id"))
                    {
                        objPGR.tracking_id = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("bank_ref_no"))
                    {
                        objPGR.bank_ref_no = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("order_status"))
                    {
                        objPGR.order_status = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("failure_message"))
                    {
                        objPGR.failure_message = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("payment_mode"))
                    {
                        objPGR.payment_mode = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("status_message"))
                    {
                        objPGR.status_message = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("amount"))
                    {
                        objPGR.amount = Convert.ToDecimal(value[1]);

                    }
                    else if (item.Contains("billing_name"))
                    {
                        objPGR.billing_name = Convert.ToString(value[1]);

                    }
                    else if (item.Contains("billing_address"))
                    {
                        objPGR.billing_address = Convert.ToString(value[1]);

                    }
                    else if (item.Contains("billing_city"))
                    {
                        objPGR.billing_city = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_state"))
                    {
                        objPGR.billing_state = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_zip"))
                    {
                        objPGR.billing_zip = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_country"))
                    {
                        objPGR.billing_country = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_tel"))
                    {
                        objPGR.billing_tel = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("billing_email"))
                    {
                        objPGR.billing_email = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_name"))
                    {
                        objPGR.delivery_name = Convert.ToString(value);
                    }
                    else if (item.Contains("delivery_address"))
                    {
                        objPGR.delivery_address = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_city"))
                    {
                        objPGR.delivery_city = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_state"))
                    {
                        objPGR.delivery_state = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_zip"))
                    {
                        objPGR.delivery_zip = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_country"))
                    {
                        objPGR.delivery_country = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_tel"))
                    {
                        objPGR.delivery_tel = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("vault"))
                    {
                        objPGR.vault = (string.IsNullOrEmpty(value[1]) ? char.MinValue : Convert.ToChar(value[1]));
                    }
                    else if (item.Contains("offer_type"))
                    {
                        objPGR.offer_type = string.IsNullOrEmpty(value[1]) ? string.Empty : Convert.ToString(value[1]);
                    }
                    else if (item.Contains("offer_code"))
                    {
                        objPGR.offer_code = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("discount_value"))
                    {
                        objPGR.discount_value = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("mer_amount"))
                    {
                        objPGR.mer_amount = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("eci_value"))
                    {
                        objPGR.eci_value = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("retry"))
                    {
                        objPGR.retry = string.IsNullOrEmpty(value[1]) ? char.MinValue : Convert.ToChar(value[1]);
                    }
                    else if (item.Contains("response_code"))
                    {
                        objPGR.response_code = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("billing_notes"))
                    {
                        objPGR.billing_notes = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("trans_date"))
                    {
                        objPGR.trans_date = string.IsNullOrEmpty(value[1]) ? DateTime.Now : Convert.ToDateTime(value[1]);
                    }
                    else if (item.Contains("merchant_param1"))
                    {
                        objPGR.merchant_param1 = Convert.ToInt32(value[1]);  // Branch id store
                    }
                    else if (item.Contains("bin_country"))
                    {
                        objPGR.bin_country = Convert.ToString(value[1]);  
                    }
                }
            }
            objPGR.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BranchId"]);
            log.Info("Payment Response initlize connection string after session out Start");
            //To initlize connection string after session out
            SaveSessionData(objPGR.merchant_param1);
            log.Info("Payment Response initlize connection string after session out End");

            log.Info("Payment Response DB Save Start");
            ObjIR.AddPaymentGatewayResponse(objPGR);
            log.Info("Payment Response DB Save End");

            objPGR.PageFor = "Cancel";
            log.Info("Payment Response GetResponse Method End");
            //Set Session from DB
            CcavenueRepository obj = new CcavenueRepository();
            obj.GetSessionDetailsFromDB(objPGR.order_id);
            if (obj.DBResponse.Status == 1)
            {
                Login user = (Login)obj.DBResponse.Data;
                HttpContext.Session["LoginUser"] = user;
                System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
                manager.RemoveSessionID(System.Web.HttpContext.Current);
                var SessionId = manager.CreateSessionID(System.Web.HttpContext.Current);
                Session["SessionId"] = SessionId;

            }

            return PartialView("GetResponse", objPGR);

        }

        

        [HttpPost]
        [Route("~/UPIResponse/CancelResponse")]
        public ActionResult CancelResponseBqr(FormCollection request)
        {
            log.Info("Payment Response Cancel Method Start");
            string workingKey = "";
            workingKey = UPIConfiguration.WorkingKey;
            CCACrypto ccaCrypto = new CCACrypto();
            string encResp = "";
            string decResp = "";
            string orderNo = "";
            string ccaRequest = "";
            string crossSellUrl = "";
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
            //long OrderId = Convert.ToInt64(Session["OrderId"]);
            encResp = Request.Form["encResp"];
            decResp = ccaCrypto.Decrypt(encResp, workingKey);
            log.Info("Payment Response Decrypt response data" + decResp);

            orderNo = Request.Form["orderNo"];
            crossSellUrl = Request.Form["crossSellUrl"];

            string[] respAttribute = decResp.Replace("null", "").Split('&');
            PaymentGatewayResponse objPGR = new PaymentGatewayResponse();
            CcavenueRepository ObjIR = new CcavenueRepository();
            foreach (var item in respAttribute)
            {
                string[] value = item.Split('=');
                if (value.Length > 1)
                {
                    if (item.Contains("order_id"))
                    {
                        objPGR.order_id = Convert.ToInt64(value[1]);
                    }
                    else if (item.Contains("tracking_id"))
                    {
                        objPGR.tracking_id = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("bank_ref_no"))
                    {
                        objPGR.bank_ref_no = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("order_status"))
                    {
                        objPGR.order_status = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("failure_message"))
                    {
                        objPGR.failure_message = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("payment_mode"))
                    {
                        objPGR.payment_mode = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("status_message"))
                    {
                        objPGR.status_message = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("amount"))
                    {
                        objPGR.amount = Convert.ToDecimal(value[1]);

                    }
                    else if (item.Contains("billing_name"))
                    {
                        objPGR.billing_name = Convert.ToString(value[1]);

                    }
                    else if (item.Contains("billing_address"))
                    {
                        objPGR.billing_address = Convert.ToString(value[1]);

                    }
                    else if (item.Contains("billing_city"))
                    {
                        objPGR.billing_city = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_state"))
                    {
                        objPGR.billing_state = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_zip"))
                    {
                        objPGR.billing_zip = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_country"))
                    {
                        objPGR.billing_country = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("billing_tel"))
                    {
                        objPGR.billing_tel = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("billing_email"))
                    {
                        objPGR.billing_email = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_name"))
                    {
                        objPGR.delivery_name = Convert.ToString(value);
                    }
                    else if (item.Contains("delivery_address"))
                    {
                        objPGR.delivery_address = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_city"))
                    {
                        objPGR.delivery_city = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_state"))
                    {
                        objPGR.delivery_state = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_zip"))
                    {
                        objPGR.delivery_zip = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_country"))
                    {
                        objPGR.delivery_country = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("delivery_tel"))
                    {
                        objPGR.delivery_tel = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("vault"))
                    {
                        objPGR.vault = (string.IsNullOrEmpty(value[1]) ? char.MinValue : Convert.ToChar(value[1]));
                    }
                    else if (item.Contains("offer_type"))
                    {
                        objPGR.offer_type = string.IsNullOrEmpty(value[1]) ? string.Empty : Convert.ToString(value[1]);
                    }
                    else if (item.Contains("offer_code"))
                    {
                        objPGR.offer_code = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("discount_value"))
                    {
                        objPGR.discount_value = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("mer_amount"))
                    {
                        objPGR.mer_amount = Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("eci_value"))
                    {
                        objPGR.eci_value = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("retry"))
                    {
                        objPGR.retry = string.IsNullOrEmpty(value[1]) ? char.MinValue : Convert.ToChar(value[1]);
                    }
                    else if (item.Contains("response_code"))
                    {
                        objPGR.response_code = string.IsNullOrEmpty(value[1]) ? decimal.Zero : Convert.ToDecimal(value[1]);
                    }
                    else if (item.Contains("billing_notes"))
                    {
                        objPGR.billing_notes = Convert.ToString(value[1]);
                    }
                    else if (item.Contains("trans_date"))
                    {
                        objPGR.trans_date = string.IsNullOrEmpty(value[1]) ? DateTime.Now : Convert.ToDateTime(value[1]);
                    }
                    else if (item.Contains("merchant_param1"))
                    {
                        objPGR.merchant_param1 = Convert.ToInt32(value[1]);  // Branch id store
                    }
                    else if (item.Contains("bin_country"))
                    {
                        objPGR.bin_country = Convert.ToString(value[1]);
                    }
                }
            }
            objPGR.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BranchId"]);
            log.Info("Payment Response initlize connection string after session out Start");
            //To initlize connection string after session out
            SaveSessionData(objPGR.merchant_param1);
            log.Info("Payment Response initlize connection string after session out End");

            log.Info("Payment Response DB Save Start");
            ObjIR.AddPaymentGatewayResponse(objPGR);
            log.Info("Payment Response DB Save End");

            objPGR.PageFor = "Cancel";
            log.Info("Payment Response GetResponse Method End");

            return PartialView("GetResponse", objPGR);

        }







        [NonAction]
        private void SaveSessionData(int BranchId)
        {
            log.Error("SaveSessionData :" + BranchId);
            var Config = Server.MapPath("~/Content/CwcConfig.xml");
            XDocument doc = XDocument.Load(Config);
            var objBranch = from r in doc.Descendants("Branch")
                            select new
                            {
                                BranchId = r.Element("BranchId").Value,
                                BranchType = r.Element("BranchType").Value,
                                Name = r.Element("Name").Value,
                                ConnectionString = r.Element("ConnectionString").Value,
                                EmailFrom = r.Element("EmailFrom").Value,
                                EmailPwd = r.Element("EmailPwd").Value,
                                SmtpHost = r.Element("SmtpHost").Value,
                                SmtpPort = r.Element("SmtpPort").Value,
                                GateEntryMailSent = r.Element("GateEntryMailSent").Value,
                                GateExitMailSent = r.Element("GateExitMailSent").Value,
                                FtpServerIP = r.Element("ResourceUrl").Value,
                                FtpUid = r.Element("FtpUid").Value,
                                FtpPwd = r.Element("FtpPwd").Value,
                                PortNo = r.Element("SFTPPortNo").Value
                            };
           
            foreach (var item in objBranch)
            {
                log.Error("Inside forloop :" + BranchId);
                if (Convert.ToInt32(item.BranchId) == BranchId)
                {
                    log.Error("inside for loop  BranchId :" + item.BranchId);
                    log.Error("inside for loop  BranchType :" + item.BranchType);
                    log.Error("inside for loop : Name" + item.Name);
                    log.Error("inside for loop : ConnectionString " + item.ConnectionString);
                    log.Error("inside for loop : EmailFrom " + item.EmailFrom);
                    log.Error("inside for loop : EmailPwd " + item.EmailPwd);
                    log.Error("inside for loop : SmtpHost " + item.SmtpHost);
                    log.Error("inside for loop : SmtpPort " + item.SmtpPort);
                    log.Error("inside for loop : GateEntryMailSent " + item.GateEntryMailSent);
                    log.Error("inside for loop : GateExitMailSent " + item.GateExitMailSent);
                    log.Error("inside for loop : FtpServerIP " + item.FtpServerIP);
                    log.Error("inside for loop : FtpUid " + item.FtpUid);
                    log.Error("inside for loop : FtpPwd " + item.FtpPwd);
                    log.Error("inside for loop : PortNo " + item.PortNo);
                  

                    Session["BranchId"] = item.BranchId;
                    Session["BranchType"] = item.BranchType;
                    Session["Name"] = item.Name;
                    Session["ConnectionString"] = item.ConnectionString;
                    Session["EmailFrom"] = item.EmailFrom;
                    Session["EmailPwd"] = item.EmailPwd;
                    Session["SmtpHost"] = item.SmtpHost;
                    Session["SmtpPort"] = item.SmtpPort;
                    Session["GateEntryMailSent"] = item.GateEntryMailSent;
                    Session["GateExitMailSent"] = item.GateExitMailSent;
                    Session["FtpServerIP"] = item.FtpServerIP;
                    Session["FtpUid"] = item.FtpUid;
                    Session["FtpPwd"] = item.FtpPwd;
                    Session["PortNo"] = item.PortNo;
                    break;
                }
            }
        }


        [HttpPost, ValidateInput(false)]
        // [CustomValidateAntiForgeryToken]
        [Route("~/UPIResponse/GenerateDeStuffingReportPDF")]
        public JsonResult GenerateDeStuffingReportPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                string OrderId = fc["OrderId"].ToString();
                var Pages = new string[1];
                var FileName = OrderId+"ReceiptOrderId.pdf";
                Pages[0] = fc["Page"].ToString();
              
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + OrderId + "/Report/Receipt/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = "";
                    ObjPdf.HOAddress = "";
                    ObjPdf.ZonalOffice = "";
                    ObjPdf.ZOAddress = "";
                    ObjPdf.GeneratePDFWithoutFooter(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + OrderId + "/Report/Receipt/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }
    }
}