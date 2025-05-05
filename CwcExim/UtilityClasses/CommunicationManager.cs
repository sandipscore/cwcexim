using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
namespace CwcExim.UtilityClasses
{
    public static class CommunicationManager
    {
        private static string _From;
        private static string _Password;
        private static string _Host;
        private static int _Port;

        private static string _SmsUrl;
        private static string _SmsUserName;
        private static string _SmsUserPassword;
        private static string _SmsSender;

        private static string _SmsMsgType;
        private static string _SmsPID;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static CommunicationManager()
        {
            /* _From = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"]; 
             _Password = System.Configuration.ConfigurationManager.AppSettings["EmailPwd"];
             _Host = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"];
             _Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]); */
            /*_From = Convert.ToString(HttpContext.Current.Session["EmailFrom"]); 
            _Password = Convert.ToString(HttpContext.Current.Session["EmailPwd"]); 
            _Host = Convert.ToString(HttpContext.Current.Session["SmtpHost"]); 
            _Port = Convert.ToInt32(HttpContext.Current.Session["SmtpPort"]); */

            _SmsUrl = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"];
            _SmsUserName = System.Configuration.ConfigurationManager.AppSettings["SmsUserName"];
            _SmsUserPassword = System.Configuration.ConfigurationManager.AppSettings["SmsUserPassword"];
            _SmsSender = System.Configuration.ConfigurationManager.AppSettings["SmsSender"];
            _SmsMsgType = System.Configuration.ConfigurationManager.AppSettings["SmsMsgType"];
            _SmsPID = System.Configuration.ConfigurationManager.AppSettings["SmsPID"];
        }

        public static string SendMail(EmailDataModel ObjEmail)
        {
            _From = Convert.ToString(HttpContext.Current.Session["EmailFrom"]);
            _Password = Convert.ToString(HttpContext.Current.Session["EmailPwd"]);
            _Host = Convert.ToString(HttpContext.Current.Session["SmtpHost"]);
            _Port = Convert.ToInt32(HttpContext.Current.Session["SmtpPort"]);

            string Status = "";
            string From = _From;
            string To = ObjEmail.ReceiverEmail;
            string Password = _Password;

            string Subject = ObjEmail.Subject;
            string Body = ObjEmail.MailBody;
            MailMessage Msg = new MailMessage(From, To, Subject, string.Format("<p style=\"text-align:justify;\">{0}</p>", Body.Replace("\r\n", "<br/>")));
            Msg.IsBodyHtml = true;
            Msg.Priority = MailPriority.High;
            //Smtp Host is the  name or Ip host of the computer used for sending mail
            SmtpClient smtpobj = new SmtpClient(_Host, _Port);
            //smtpobj.Host = "smtp.rediffmail.com";
            //smtpobj.Port = 25;
            smtpobj.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpobj.EnableSsl = false;
            smtpobj.UseDefaultCredentials = false;
            smtpobj.Credentials = new System.Net.NetworkCredential(From, Password);

            try
            {
                smtpobj.Send(Msg);
                Status = "Success";
                return Status;

            }
            catch (Exception ex)
            {
                Status = "Error";
                return Status;

            }


        }

        public static string SendMail(string emailTitle, string emailBody, string[] toAddress, string[] attachments = null,
            string[] toCc = null, string[] toBcc = null, bool isHtml = true)
        {
            _From = Convert.ToString(HttpContext.Current.Session["EmailFrom"]);
            _Password = Convert.ToString(HttpContext.Current.Session["EmailPwd"]);
            _Host = Convert.ToString(HttpContext.Current.Session["SmtpHost"]);
            _Port = Convert.ToInt32(HttpContext.Current.Session["SmtpPort"]);



            try
            {
                var _email = new MailMessage();
                _email.Subject = emailTitle;
                _email.Body = emailBody;
                _email.IsBodyHtml = isHtml;
                _email.Priority = MailPriority.High;
                _email.From = new MailAddress(_From);
                foreach (var to in toAddress)
                    _email.To.Add(new MailAddress(to));

                if (toCc != null && toCc.Any())
                    foreach (var cc in toCc)
                        _email.CC.Add(new MailAddress(cc));

                if (toBcc != null && toBcc.Any())
                    foreach (var bcc in toBcc)
                        _email.Bcc.Add(new MailAddress(bcc));

                if (attachments != null && attachments.Any())
                    foreach (var attachment in attachments)
                        _email.Attachments.Add(new Attachment(attachment));
                var _smtp = new SmtpClient(_Host, _Port);
                _smtp.UseDefaultCredentials = false;
                _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                _smtp.Credentials = new NetworkCredential(_From, _Password);
                _smtp.EnableSsl = false;
                _smtp.ServicePoint.MaxIdleTime = 1;
                _smtp.Send(_email);
                _email.Dispose();
                _smtp.Dispose();
                return "Success";
            }
            catch (Exception e)
            {
                return e.GetBaseException().Message;
            }
        }

        public static string SendMailWithAttachment(EmailDataModel ObjEmail, string[] LstAttachment = null)
        {
            _From = Convert.ToString(HttpContext.Current.Session["EmailFrom"]);
            _Password = Convert.ToString(HttpContext.Current.Session["EmailPwd"]);
            _Host = Convert.ToString(HttpContext.Current.Session["SmtpHost"]);
            _Port = Convert.ToInt32(HttpContext.Current.Session["SmtpPort"]);



            string Status = "";
            string From = _From;
            string To = ObjEmail.ReceiverEmail;
            string Password = _Password;
            string Subject = ObjEmail.Subject;
            string Body = ObjEmail.MailBody;
            MailMessage Msg = new MailMessage(From, To, Subject, string.Format("<p style=\"text-align:justify;\">{0}</p>", Body.Replace("\r\n", "<br/>")));
            Msg.IsBodyHtml = true;
            if (LstAttachment != null && LstAttachment.Length > 0)
            {
                foreach (var fileName in LstAttachment)
                {
                    Msg.Attachments.Add(new Attachment(fileName));
                }
            }
            //Smtp Host is the  name or Ip host of the computer used for sending mail
            SmtpClient smtpobj = new SmtpClient(_Host, _Port);
            //smtpobj.Host = "smtp.rediffmail.com";
            //smtpobj.Port = 25;
            smtpobj.EnableSsl = false;
            smtpobj.UseDefaultCredentials = true;
            smtpobj.Credentials = new System.Net.NetworkCredential(From, Password);

            try
            {
                smtpobj.Send(Msg);
                Status = "Success";
                return Status;

            }
            catch (Exception ex)
            {
                Status = "Error";
                return Status;

            }


        }

        public static string SendSMS(SmsDataModel ObjSms)
        {
            log.Info("SendSMS Method started");
            string Status = "";
            string guid = Guid.NewGuid().ToString();
            WebClient Wc = new WebClient();
            try
            {

                //https://gateway.leewaysoftech.com/xml-transconnect-api.php?username=Central_ware&password=cen!ware&mobile=xxxxx&message=xxxxxxx&senderid=CWCMSG&peid=1701159238917761851&contentid=XXXXXX
                /*Wc.QueryString.Add("username", _SmsUserName);
                Wc.QueryString.Add("password", _SmsUserPassword);
                
                Wc.QueryString.Add("mobile", ObjSms.MsgRecepient);
                Wc.QueryString.Add("message", ObjSms.MsgText);
                Wc.QueryString.Add("senderid", _SmsSender);
               // Wc.QueryString.Add("msgtype", _SmsMsgType);
                Wc.QueryString.Add("peid", _SmsPID);
                Wc.QueryString.Add("contentid", guid);*/
                Wc.QueryString.Add("userid", _SmsUserName);
                Wc.QueryString.Add("pwd", _SmsUserPassword);

                Wc.QueryString.Add("mobile", ObjSms.MsgRecepient);
                Wc.QueryString.Add("sender", _SmsSender);
                Wc.QueryString.Add("msg", ObjSms.MsgText);
                 Wc.QueryString.Add("msgtype", _SmsMsgType);
                Wc.QueryString.Add("peid", _SmsPID);
                





                log.Info("SmsUserName:"+ _SmsUserName);

                log.Info("mobile:" + ObjSms.MsgRecepient);

                log.Info("message:" + ObjSms.MsgText);


                //Wc.QueryString.Add("Enterprise", "11");
                Stream ResponseStream = Wc.OpenRead(_SmsUrl);
                StreamReader Sr = new StreamReader(ResponseStream);
                string ResponseString = Sr.ReadToEnd();

                log.Info("SMS response:" + ResponseString);
                //SMSResponse Response = new SMSResponse();
                if (ResponseString == "Null")
                    Status = "Error";
                else
                    Status = "Success";
                //Response.response = ResponseString;
                Sr.Close();
                ResponseStream.Close();


                log.Info("SendSMS Method ended");
                return Status;
            }
            catch (Exception ex)
            {
                log.Error("SMS sending error :" + ex.Message);
                Status = "Error";
                return Status;
            }






            /*string Status = "";
            using (var web = new System.Net.WebClient())
            {
                try
                {
                    string UserName = _SmsUserName;
                    string UserPassword = _SmsUserPassword;
                    string MsgRecepient = ObjSms.MsgRecepient;
                    string MsgText = ObjSms.MsgText;
                    string Url = _SmsUrl +
                        "src=" + MsgRecepient +
                        "&dst=" + MsgRecepient +
                        "&msg=" + System.Web.HttpUtility.UrlEncode(MsgText, System.Text.Encoding.GetEncoding("ISO-8859-1")) +
                        "&username=" + System.Web.HttpUtility.UrlEncode(UserName) +
                        "&password=" + UserPassword;
                    string result = web.DownloadString(Url);
                    if (result.Contains("OK"))
                    {
                        Status = "Success";
                        //MessageBox.Show("Sms sent successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Status = "Error";
                        //MessageBox.Show("Some issue delivering", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return Status;
                }
                catch (Exception ex)
                {
                    Status = "Error";
                    return Status;
                }
                

            }*/
        }
    }
}