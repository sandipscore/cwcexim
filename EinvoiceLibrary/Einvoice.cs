using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;


using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using ZXing;
using System.Web;
using CCA.Util;
using System.Net;

namespace EinvoiceLibrary
{
    public class Einvoice
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HeaderParam _headerParam = null;
        private string _invoicepayload = "";
        
        byte[] appKey = null;
        string dcryptedSek = "";
        public Einvoice()
        {
        
        }
        public Einvoice(HeaderParam HeaderParam, string InvoicePayload)
        {

            _headerParam = HeaderParam;
            _invoicepayload = InvoicePayload;
           
        }

        public async Task<IrnResponse> GenerateEinvoice()
        {
            TokenResponse tokenResponse = await GetToken();
            IrnResponse irnr = new IrnResponse();
            if (tokenResponse.Status == 1)
                irnr = await GenerateIrn(tokenResponse);
            else
            {
                irnr.Status = 0;
                irnr.ErrorDetails = tokenResponse.ErrorDetails;
            }
            return irnr;
        }
        private async Task<TokenResponse> GetToken()
        {

            string apiUrl = ApiEndPoints.GetEndpoint("GetToken");
            string tokenPayload = GenerateTokenPayload();
            TokenResponse tr = new TokenResponse();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("client-id", _headerParam.ClientID);
                client.DefaultRequestHeaders.Add("client-secret", _headerParam.ClientSecret);
                client.DefaultRequestHeaders.Add("Gstin", _headerParam.GSTIN);

                var data = new StringContent(tokenPayload, Encoding.UTF8, "application/json");
                //log.Info("client-id ", _headerParam.ClientID);


                log.Info("apiUrl   :" + apiUrl);
                HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                string content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);
                log.Info("after apiUrl   :" + apiUrl);
                var status = joResponse["Status"];
                
                if (status.Value<string>() == "0")
                {
                    log.Error("error   :" + joResponse["ErrorDetails"].ToString());
                    tr.Status = 0;
                    tr.ErrorDetails =GenerateErrorDetails( (JArray)joResponse["ErrorDetails"]);

                }
                if (status.Value<string>() == "1")
                {

                    tr.Status = 1;
                    tr.Data = (JObject)joResponse["Data"];
                    tr.AuthToken = ((JObject)joResponse["Data"]).GetValue("AuthToken").Value<string>();
                    tr.Sek = ((JObject)joResponse["Data"]).GetValue("Sek").Value<string>();


                }


            }


            return tr;


        }

        private async Task<IrnResponse> GenerateIrn(TokenResponse tokenResponse)
        {

            string apiUrl = ApiEndPoints.GetEndpoint("GenerateEinvoice");
            dcryptedSek = EinvoiceCryptography.DecryptBySymmetricKey(tokenResponse.Sek, appKey);

            string invoicePayload = GenerateInvoicePayload(dcryptedSek);
            IrnResponse irnResponse = new IrnResponse();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("client-id", _headerParam.ClientID);
                client.DefaultRequestHeaders.Add("client-secret", _headerParam.ClientSecret);
                client.DefaultRequestHeaders.Add("Gstin", _headerParam.GSTIN);
                client.DefaultRequestHeaders.Add("user_name", _headerParam.UserName);
                client.DefaultRequestHeaders.Add("AuthToken", tokenResponse.AuthToken);





                var data = new StringContent(invoicePayload, Encoding.UTF8, "application/json");

              HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                string content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);

                var status = joResponse["Status"];
                log.Info(status);
               
                string payloadBase64 = "";
                
                if (status.Value<string>() == "0")
                {
                    log.Info(joResponse["ErrorDetails"]);
                    irnResponse.Status = 0;
                    irnResponse.ErrorDetails =GenerateErrorDetails((JArray)joResponse["ErrorDetails"]);
                    log.Info("Duplicate Error Code:" + irnResponse.ErrorDetails.ErrorCode);
                    /*if (irnResponse.ErrorDetails.ErrorCode == "2150")
                    {
                        log.Info("start GetIrnFromDuplicate ");
                        string irn = GetIrnFromDuplicate((JArray)joResponse["InfoDtls"]);
                        log.Info("end GetIrnFromDuplicate ");
                        log.Info("start GetEinvoiceByIrn ");
                        irnResponse = await this.GetEinvoiceByIrn(irn);
                        log.Info("end GetEinvoiceByIrn ");
                    }*/
                }
               else if (status.Value<string>() == "1")
                {
                
                    payloadBase64 = EinvoiceCryptography.DecryptBySymmetricKey(joResponse["Data"].ToString(), Convert.FromBase64String(dcryptedSek));
                    irnResponse = GenerateIrnResponse(payloadBase64);
                    //string irr = EinvoiceCryptography.DecryptBySymmetricKey(irnResponse.irn, Convert.FromBase64String(dcryptedSek));
                    irnResponse.QRCodeImageBase64 = GenerateQCCode(irnResponse.SignedQRCode);

                }


            }


            return irnResponse;


        }
        private string GetIrnFromDuplicate(JArray JerrorDetails)
        {
            string irn = "";
            JToken dtls = null;
            // string errCd = myElementValue.ToString();

            foreach (JObject item in JerrorDetails)
            {
                dtls = item.GetValue("Desc");


            }

            irn = dtls["Irn"].ToString();

            return irn;

        }
        public async Task<IrnResponse> GetEinvoiceByIrn(string Irn)
        {
            log.Info("Inside GetEinvoiceByIrn :" + Irn);
            TokenResponse tokenResponse = await GetToken();
            IrnResponse cirnr = new IrnResponse();
            log.Info("Inside GetEinvoiceByIrn Status :" + tokenResponse.Status);
            if (tokenResponse.Status == 1)
            {
                log.Info("start GetIrn ");
                cirnr = await GetIrn(tokenResponse, Irn);
                log.Info("end GetIrn ");
            }
            else
            {
                cirnr.Status = 0;
                cirnr.ErrorDetails = tokenResponse.ErrorDetails;
            }
            return cirnr;
        }
        private async Task<IrnResponse> GetIrn(TokenResponse tokenResponse, string Irn)
        {
            log.Info("inside GetIrn ");
            IrnResponse cirnResponse = new IrnResponse();
            try
            {


                string apiUrl = ApiEndPoints.GetEndpoint("GetEInvoiceByIRN");
                dcryptedSek = EinvoiceCryptography.DecryptBySymmetricKey(tokenResponse.Sek, appKey);
                log.Info("inside GetIrn ");
                log.Info("API " + apiUrl);
                // GenerateCancelInvoicePayload(dcryptedSek, Irn, CancelReason, CancelRemark);
              
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Add("client-id", _headerParam.ClientID);
                    client.DefaultRequestHeaders.Add("client-secret", _headerParam.ClientSecret);
                    client.DefaultRequestHeaders.Add("Gstin", _headerParam.GSTIN);
                    client.DefaultRequestHeaders.Add("user_name", _headerParam.UserName);
                    client.DefaultRequestHeaders.Add("AuthToken", tokenResponse.AuthToken);





                    //var data = new StringContent(invoicePayload, Encoding.UTF8, "application/json");

                    //HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                    HttpResponseMessage response = await client.GetAsync(apiUrl + "/" + Irn);
                    log.Info("Before ReadAsStringAsync ");
                    string content = await response.Content.ReadAsStringAsync();
                    log.Info("response : " + response);
                    log.Info("response Content: " +  response.Content);
                    log.Info("After ReadAsStringAsync ");
                    log.Info(content);
                    JObject joResponse = JObject.Parse(content);

                    var status = joResponse["Status"];
                    log.Info("inside GetIrn status: " + status);
                    string payloadBase64 = "";

                    if (status.Value<string>() == "0")
                    {

                        cirnResponse.Status = 0;
                        cirnResponse.ErrorDetails = GenerateErrorDetails((JArray)joResponse["ErrorDetails"]);

                    }
                    else if (status.Value<string>() == "1")
                    {

                        payloadBase64 = EinvoiceCryptography.DecryptBySymmetricKey(joResponse["Data"].ToString(), Convert.FromBase64String(dcryptedSek));
                        cirnResponse = GenerateIrnResponse(payloadBase64);
                        cirnResponse.QRCodeImageBase64 = GenerateQCCode(cirnResponse.SignedQRCode);


                    }


                }
                return cirnResponse;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return cirnResponse;
            }
            log.Info("end GetIrn ");
          


        }
        public string GenerateB2cIrn(IrnModel irn)
        {

            string gst = irn.SupplierGstNo;
            string docno = irn.DocumentNo.Replace("-", "").Replace("/", "");
            string docType = irn.DocumentType;
            string docDate = irn.DocumentDate;
            string finYrStart = Convert.ToDateTime(docDate).Year.ToString();
            string finYrEnd = (Convert.ToDateTime(docDate).Year + 1).ToString().Substring(2, 2);
            string plainIrnText = gst + finYrStart + "-" + finYrEnd + docType + docno;

            string hashedData = EinvoiceCryptography.ComputeSha256Hash(plainIrnText);
            return "";// hashedData;
        }

      
        public async Task<CancelIrnResponse> CancelEinvoice(string Irn, string CancelReason, string CancelRemark)
        {
            TokenResponse tokenResponse = await GetToken();
            CancelIrnResponse cirnr = new CancelIrnResponse();
            if (tokenResponse.Status == 1)
                cirnr = await CancelIrn(tokenResponse, Irn, CancelReason, CancelRemark);
            else
            {
                cirnr.Status = 0;
                cirnr.ErrorDetails = tokenResponse.ErrorDetails;
            }
            return cirnr;
        }
        private async Task<CancelIrnResponse> CancelIrn(TokenResponse tokenResponse, string Irn, string CancelReason, string CancelRemark)
        {

            string apiUrl = ApiEndPoints.GetEndpoint("CancelEinvoice");
            dcryptedSek = EinvoiceCryptography.DecryptBySymmetricKey(tokenResponse.Sek, appKey);

            string invoicePayload = GenerateCancelInvoicePayload(dcryptedSek, Irn, CancelReason, CancelRemark);
            CancelIrnResponse cirnResponse = new CancelIrnResponse();
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("client-id", _headerParam.ClientID);
                client.DefaultRequestHeaders.Add("client-secret", _headerParam.ClientSecret);
                client.DefaultRequestHeaders.Add("Gstin", _headerParam.GSTIN);
                client.DefaultRequestHeaders.Add("user_name", _headerParam.UserName);
                client.DefaultRequestHeaders.Add("AuthToken", tokenResponse.AuthToken);





                var data = new StringContent(invoicePayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                string content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);

                var status = joResponse["Status"];

                string payloadBase64 = "";

                if (status.Value<string>() == "0")
                {

                    cirnResponse.Status = 0;
                    cirnResponse.ErrorDetails = GenerateErrorDetails((JArray)joResponse["ErrorDetails"]);

                }
                else if (status.Value<string>() == "1")
                {

                    payloadBase64 = EinvoiceCryptography.DecryptBySymmetricKey(joResponse["Data"].ToString(), Convert.FromBase64String(dcryptedSek));
                    cirnResponse = GenerateCancelIrnResponse(payloadBase64);
                    //string irr = EinvoiceCryptography.DecryptBySymmetricKey(irnResponse.irn, Convert.FromBase64String(dcryptedSek));
                    // cirnResponse.QRCodeImageBase64 = GenerateQRCode(irnResponse.SignedQRCode);

                }


            }


            return cirnResponse;


        }
        private CancelIrnResponse GenerateCancelIrnResponse(string payload)
        {

            CancelIrnResponse cir = new CancelIrnResponse();
            string decryptedPayload = "";
            byte[] payloadDataByte = null;
            payloadDataByte = Convert.FromBase64String(payload);
            decryptedPayload = Encoding.UTF8.GetString(payloadDataByte);

            JObject joPayload = JObject.Parse(decryptedPayload);
            cir.Status = 1;
            cir.Irn = joPayload.GetValue("Irn").Value<string>();
            cir.CancelDate = joPayload.GetValue("CancelDate").Value<string>();



            return cir;
        }
        private string GenerateCancelInvoicePayload(string paramsek, string Irn, string CancelReason, string CancelRemark)
        {
            string payload = "";
            string sek = paramsek;//  Convert.ToBase64String(paramsek, 0, paramsek.Length); ;
            StringBuilder sbirn = new StringBuilder();
            sbirn.Append("{");
            sbirn.Append("\"Irn\":" + "\"" + Irn + "\"," + "\"CnlRsn\":" + "\"" + CancelReason + "\"," + "\"CnlRem\":" + "\"" + CancelRemark + "\"");
            sbirn.Append("}");

            string cancelinvoicepayload = EinvoiceCryptography.Base64Encode(sbirn.ToString());


            string encryptedInvoicePayload = EinvoiceCryptography.EncryptBySymmetricKey(cancelinvoicepayload, sek);


            StringBuilder sbPayload = new StringBuilder();
            // sbPayload.Append("{\"data\":"+"\""+ encryptedInvoicePayload+"\""+"}");
            sbPayload.Append("{\"Data\":" + "\"" + encryptedInvoicePayload + "\"" + "}");

            payload = sbPayload.ToString();



            return payload;
        }
        public string GenerateQCCode(string QCText)
        {
            if (QCText != "")
            {
                var QCwriter = new BarcodeWriter();
                QCwriter.Format = BarcodeFormat.QR_CODE;
                var result = QCwriter.Write(QCText);


                var barcodeBitmap = new Bitmap(result);
                byte[] byteImage = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    barcodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byteImage = ms.ToArray();

                }

                return Convert.ToBase64String(byteImage);
            }
            else
            {
                return "";
            }

        }
        public B2cQRCodeResponse GenerateB2cQRCode(QrCodeInfo Qinfo)
        {

            B2cQRCodeResponse qcr = new B2cQRCodeResponse();
            string QCText = "";
            QCText = JsonConvert.SerializeObject(Qinfo);
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(QCText);


            var barcodeBitmap = new Bitmap(result);
            byte[] byteImage = null;
            using (MemoryStream ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byteImage = ms.ToArray();

            }

            qcr.QrCodeJson = QCText;
            qcr.QrCodeBase64 = Convert.ToBase64String(byteImage);
            return qcr;
        }

        public B2cQRCodeResponse GenerateB2cQRCode(Ppg_QrCodeInfo Qinfo)
        {

            B2cQRCodeResponse qcr = new B2cQRCodeResponse();
            string QCText = "";
            // QCText = JsonConvert.SerializeObject(Qinfo);
            QCText = Qinfo.Data.SupplierGstNo + System.Environment.NewLine + Qinfo.Data.SupplierUPIID + System.Environment.NewLine + Qinfo.Data.BankAccountNo + System.Environment.NewLine + Qinfo.Data.IFSC + System.Environment.NewLine + Qinfo.Data.InvoiceNo + System.Environment.NewLine + Qinfo.Data.InvoiceDate +  System.Environment.NewLine + Qinfo.Data.InvoiceValue + System.Environment.NewLine + Qinfo.Data.CGST + System.Environment.NewLine + Qinfo.Data.SGST + System.Environment.NewLine + Qinfo.Data.IGST + System.Environment.NewLine + Qinfo.Data.CESS + System.Environment.NewLine;
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(QCText);


            var barcodeBitmap = new Bitmap(result);
            byte[] byteImage = null;
            using (MemoryStream ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byteImage = ms.ToArray();

            }

            qcr.QrCodeJson = QCText;
            qcr.QrCodeBase64 = Convert.ToBase64String(byteImage);
            return qcr;
        }

        public B2cQRCodeResponse GenerateB2cQRCode(UpiQRCodeInfo Qinfo)
        {

            B2cQRCodeResponse qcr = new B2cQRCodeResponse();
            string QCText = "";
            //QCText = JsonConvert.SerializeObject(Qinfo);
            QCText = "upi://pay?ver=01&mode="+ Qinfo.mode + "&tr=" + Qinfo.tr + "&tn=" + System.Uri.EscapeDataString(Qinfo.tn) + "&pa=" + Qinfo.pa + "&pn=" + System.Uri.EscapeDataString(Qinfo.pn) + "&mc=" + Qinfo.mc + "&am=" + Qinfo.am +"&mid=" + Qinfo.mid + "&msid=" + Qinfo.mtid + "&mtid=" + Qinfo.mtid + "&gstBrkUp=CGST:" + Qinfo.CGST + "|SGST:"+ Qinfo.SGST + "|IGST:"+ Qinfo.IGST + "&qrMedium=06&invoiceNo=" + Qinfo.invoiceNo + "&InvoiceDate=" + Qinfo.InvoiceDate + "&InvoiceName=" + System.Uri.EscapeDataString(Qinfo.InvoiceName) + "&QRexpire=" + Qinfo.QRexpire + "&pinCode=" + Qinfo.pinCode + "&tier=TIER1&gstIn=" + Qinfo.gstIn + "&sign=";
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(QCText);

           
            var barcodeBitmap = new Bitmap(result);
            byte[] byteImage = null;
            using (MemoryStream ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byteImage = ms.ToArray();

            }

            qcr.QrCodeJson = QCText;
            qcr.QrCodeBase64 = Convert.ToBase64String(byteImage);
            return qcr;
        }

        public B2cQRCodeResponse GenerateB2cQRCode_Wfld(UpiQRCodeInfo Qinfo)
        {
            try
            {
                string environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();



                CCACrypto ccaCrypto = new CCACrypto();
                B2cQRCodeResponse qcr = new B2cQRCodeResponse();

                if (environment == "T")
                {
                    qcr= GenerateB2cQRCode(Qinfo);

                    return qcr;
                }



                    log.Error(Qinfo.merchant_id);
                string ccaRequest = "merchant_id=" + Qinfo.merchant_id + "&order_id=" + Qinfo.order_id + "&amount=" + Qinfo.am + "&currency=INR&redirect_url=" + Qinfo.redirect_url + "&cancel_url=" + Qinfo.cancel_url + "&language=EN&billing_name=" + Qinfo.billing_name + "&billing_address=" + Qinfo.billing_address + "&billing_city=&billing_state=&billing_zip=" + Qinfo.billing_zip + "&billing_country=&billing_tel=" + Qinfo.billing_tel + "&billing_email=" + Qinfo.billing_email + "&delivery_tel=0&merchant_param1=" + Qinfo.merchant_param1 + "&merchant_param2=additional Info.&merchant_param3=additional Info.&merchant_param4=additional Info.&merchant_param5=additional Info.&payment_option=OPTBQR&card_type=BQR&card_name=Bharat QR";
                // string ccaRequest = "merchant_id=827355&order_id=120122&amount=7239.35&currency=INR&redirect_url=https://cwc-cfs.com:88/UPIResponse/GetResponseBqr&cancel_url=https://cwc-cfs.com:88/UPIResponse/CancelResponseBqr&language=EN&billing_name=LIEBHERR MACHINE TOOLS INDIA PVT LTD&billing_address=Karnataka, India&billing_city=&billing_state=&billing_zip=560058&billing_country=&billing_tel=9896226054&billing_email=test@gmail.com&delivery_tel=0&merchant_param1=7&merchant_param2=additional Info.&merchant_param3=additional Info.&merchant_param4=additional Info.&merchant_param5=additional Info.&payment_option=OPTBQR&card_type=BQR&card_name=Bharat QR";

                log.Error("String" + ccaRequest);
                string strEncRequest = ccaCrypto.Encrypt(ccaRequest, UPIConfiguration.WorkingKeyBQR);

                //string ccaRequest = "merchant_id=827355&order_id=737814748487747906&amount=900.00&currency=INR&redirect_url=http://103.249.97.100:81/UPIResponse/GetResponse&cancel_url=http://103.249.97.100:81/UPIResponse/CancelResponse&language=EN&billing_name=QUADRAGEN VETHEALTH PRIVATE LIMITED SEZ unit&billing_address=test&billing_city=&billing_state=&billing_zip=560094&billing_country=&billing_tel=9896226054&billing_email=test@gmail.com&delivery_tel=0&merchant_param1=7&merchant_param2=additional Info.&merchant_param3=additional Info.&merchant_param4=additional Info.&merchant_param5=additional Info.&payment_option=OPTBQR&card_type=BQR&card_name=Bharat QR";
                //string strEncRequest = ccaCrypto.Encrypt(ccaRequest, "F0EF644052DE330EA7144B00409894D0");

                
                string Callapi = "";
                log.Error("Calling String:" + strEncRequest);
                log.Error("Encryption String:" + strEncRequest);
                if (environment == "P")
                {


                    Callapi = "https://secure.ccavenue.com/transaction.do?command=initiatePayloadTransaction&encRequest=";
                    
                }
                else
                {
                    

                    Callapi = "https://test.ccavenue.com/transaction/transaction.do?command=initiatePayloadTransaction&encRequest=";
                }

                log.Error("Calling Api" + Callapi);

                HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create("" + Callapi + "" + strEncRequest + "&access_code=" + UPIConfiguration.AccessCodeBQR);
                // WebRequest.Create("" + Callapi + "" + strEncRequest + "&access_code=AVKX70JB71BE06XKEB");
                request.Method = "POST";
               request.ContentLength = 0;
                WebResponse response = request.GetResponse();
                string responseFromServer = "";
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    // Console.WriteLine(responseFromServer);
                }
                response.Close();
                log.Error("Calling Api End");
                log.Error("Start Responce");
                log.Error(responseFromServer);
                log.Error("End Responce");
                qcr.QrCodeBase64 = responseFromServer;
                return qcr;
            }
            catch( Exception ex)
            {
                log.Error("Error"+ex.Message.ToString());
                B2cQRCodeResponse qcr = new B2cQRCodeResponse();
                return qcr;
            }

          
        }
        public B2cQRCodeResponse GenerateB2cQRCode(string Qinfo)
        {

            B2cQRCodeResponse qcr = new B2cQRCodeResponse();
            string QCText = "";
            // QCText = JsonConvert.SerializeObject(Qinfo);
            //QCText = Qinfo.Data.SupplierGstNo + System.Environment.NewLine + Qinfo.Data.SupplierUPIID + System.Environment.NewLine + Qinfo.Data.BankAccountNo + System.Environment.NewLine + Qinfo.Data.IFSC + System.Environment.NewLine + Qinfo.Data.InvoiceNo + System.Environment.NewLine + Qinfo.Data.InvoiceDate + System.Environment.NewLine + Qinfo.Data.InvoiceValue + System.Environment.NewLine + Qinfo.Data.CGST + System.Environment.NewLine + Qinfo.Data.SGST + System.Environment.NewLine + Qinfo.Data.IGST + System.Environment.NewLine + Qinfo.Data.CESS + System.Environment.NewLine;
            QCText = Qinfo;
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(QCText);


            var barcodeBitmap = new Bitmap(result);
            byte[] byteImage = null;
            using (MemoryStream ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byteImage = ms.ToArray();

            }

            qcr.QrCodeJson = QCText;
            qcr.QrCodeBase64 = Convert.ToBase64String(byteImage);
            return qcr;
        }
        private ErrorDetails GenerateErrorDetails(JArray JerrorDetails)
        {
            string errCd="" ;
            string errMsg= "";
            // string errCd = myElementValue.ToString();

            foreach (JObject item in JerrorDetails)
            {
                 errCd = item.GetValue("ErrorCode").ToString();
                 errMsg = item.GetValue("ErrorMessage").ToString();
               
            }

            

            return new ErrorDetails { ErrorCode = errCd, ErrorMessage = errMsg };

        }
        private IrnResponse GenerateIrnResponse(string payload)
        {

            IrnResponse ir = new IrnResponse();
            string decryptedPayload = "";
            byte[] payloadDataByte = null;
            payloadDataByte = Convert.FromBase64String(payload);
            decryptedPayload = Encoding.UTF8.GetString(payloadDataByte);

            JObject joPayload = JObject.Parse(decryptedPayload);
            ir.Status = 1;
            ir.AckNo = joPayload.GetValue("AckNo").Value<string>();
            ir.AckDt = joPayload.GetValue("AckDt").Value<string>();
            ir.irn = joPayload.GetValue("Irn").Value<string>();
            ir.SignedInvoice = joPayload.GetValue("SignedInvoice").Value<string>();//Decode(joPayload.GetValue("SignedInvoice").Value<string>());
            ir.SignedQRCode = joPayload.GetValue("SignedQRCode").Value<string>();// Decode(joPayload.GetValue("SignedQRCode").Value<string>()); 
            ir.IrnStatus = joPayload.GetValue("Status").Value<string>(); 
            ir.EwbNo= joPayload.GetValue("EwbNo").Value<string>(); 
            ir.EwbDt= joPayload.GetValue("EwbDt").Value<string>(); 
            ir.EwbValidTill= joPayload.GetValue("EwbValidTill").Value<string>(); 


            return ir;
        }
        private string GenerateTokenPayload()
        {
            string payload = "";
            string userName = _headerParam.UserName;
            string password = _headerParam.Password;
            string publicKey = "";
            string environment = "";
            appKey = GenerateAppKey();

            string b64appkey = Convert.ToBase64String(appKey);
            string strAppkey = Encoding.UTF8.GetString(appKey, 0, appKey.Length);

            environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();

            if (environment == "P")
                publicKey = EinvoiceCryptography.GetPublicKey("Production");
            else
                publicKey = EinvoiceCryptography.GetPublicKey("Sandbox");

            string encryptedAppKey = EinvoiceCryptography.Encrypt(appKey, publicKey);
            string encryptedPassword = EinvoiceCryptography.EncryptAsymmetric(password, publicKey);

           

            StringBuilder sbCredential = new StringBuilder();
            sbCredential.Append("{ \"UserName\": " + "\"" + userName + "\"" + ",");
            sbCredential.Append(" \"Password\": " + "\"" + password + "\"" + ",");
            sbCredential.Append(" \"AppKey\": " + "\"" + b64appkey + "\"" + ",");
            sbCredential.Append(" \"ForceRefreshAccessToken\": false}");

            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sbCredential.ToString());
            string b64 = Convert.ToBase64String(toEncodeAsBytes);



            string encryptedCredential = EinvoiceCryptography.EncryptAsymmetric(b64, publicKey);

            // string encryptedCredential = Convert.ToBase64String(sbCredential.ToString());

            StringBuilder sbPayload = new StringBuilder();
            sbPayload.Append("{\"Data\":\"");
            sbPayload.Append(encryptedCredential);
            sbPayload.Append("\"");
            sbPayload.Append("}");
            payload = sbPayload.ToString();



            return payload;
        }
        private string GenerateTokenPayload_1()
        {
            string payload = "";
            string userName = _headerParam.UserName;
            string password = _headerParam.Password;
            string publicKey = "";
            string environment = "";
            appKey = GenerateAppKey();
            //string publicKey = EinvoiceCryptography.GetPublicKey("Sandbox");
            //string publicKey = EinvoiceCryptography.GetPublicKey("Production");

            environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();

            if (environment == "P")
                publicKey = EinvoiceCryptography.GetPublicKey("Production");
            else
                publicKey = EinvoiceCryptography.GetPublicKey("Sandbox");

            string encryptedAppKey = EinvoiceCryptography.Encrypt(appKey, publicKey);
            string encryptedPassword = EinvoiceCryptography.EncryptAsymmetric(password, publicKey);

            StringBuilder sbPayload = new StringBuilder();
            sbPayload.Append("{\"data\":{");
            sbPayload.Append("\"UserName\":" + "\"" + userName + "\"" + ",");
            sbPayload.Append("\"Password\":" + "\"" + encryptedPassword + "\"" + ",");
            sbPayload.Append("\"AppKey\":" + "\"" + encryptedAppKey + "\"" + ",");
            sbPayload.Append("\"ForceRefreshAccessToken\": false}}");

            payload = sbPayload.ToString();



            return payload;
        }
        private string GenerateInvoicePayload(string paramsek)
        {
            string payload = "";
            string sek = paramsek;//  Convert.ToBase64String(paramsek, 0, paramsek.Length); ;

            string invoicepayload = EinvoiceCryptography.Base64Encode(_invoicepayload);


            string encryptedInvoicePayload = EinvoiceCryptography.EncryptBySymmetricKey(invoicepayload, sek);


            StringBuilder sbPayload = new StringBuilder();
            // sbPayload.Append("{\"data\":"+"\""+ encryptedInvoicePayload+"\""+"}");
            sbPayload.Append("{\"Data\":" + "\"" + encryptedInvoicePayload + "\"" + "}");

            payload = sbPayload.ToString();



            return payload;
        }
        private byte[] GenerateAppKey()
        {
            Aes KEYGEN = Aes.Create();
            byte[] secretKey = KEYGEN.Key;
            return secretKey;
        }
        private string Decode(string token)
        {

            try
            {
                var parts = token.Split('.');

                var header = parts[0];
                var payload = parts[1];

                var signature = parts[2];

                int mod4 = payload.Length % 4;
                if (mod4 > 0)
                {
                    payload += new string('=', 4 - mod4);
                }


                byte[] data = Convert.FromBase64String(payload);


                string decodedString = Encoding.UTF8.GetString(data);



                return decodedString.ToString();
            }
            catch(Exception ex)
            {
                return "";
            }
         
        }      

    }
}
