using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace EinvoiceLibrary
{
    public static class UPIConfiguration
    {
        private static Dictionary<string, string> dicEndpoints = new Dictionary<string, string>();
        static string environment = "";
        private static string _workingKey = "";
        private static string _strAccessCode = "";

        private static string _workingKeyBqr = "";
        private static string _strAccessCodeBqr = "";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static UPIConfiguration()
        {
            
            environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();
            //int BranchId =  Convert.ToInt32(HttpContext.Current.Session["BranchId"]);
            int BranchId =Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());
            
            log.Info("BranchId :"+ BranchId);

            GetCCAVNWorkingKeyNAccesCode(BranchId);

            /*if (environment == "P")
            {

               
                GetCCAVNWorkingKeyNAccesCode(BranchId);
            }
            else
            {
               
                GetCCAVNWorkingKeyNAccesCode(BranchId);
            }*/
        }

        public static string WorkingKey
        {
            get
            {
                return _workingKey;

            }
        }

        public static string AccessCode
        {
            get
            {
                return _strAccessCode;

            }
        }
        public static string WorkingKeyBQR
        {
            get
            {
                return _workingKeyBqr;

            }
        }

        public static string AccessCodeBQR
        {
            get
            {
                return _strAccessCodeBqr;

            }
        }

        public static void GetCCAVNWorkingKeyNAccesCode(int BranchId)
        {
            var Config = HttpContext.Current.Server.MapPath("~/Content/CwcConfig.xml");
            XDocument doc = XDocument.Load(Config);
            var objBranch = from r in doc.Descendants("Branch")
                            select new
                            {
                                CCAVNWorkingKey = r.Element("CCAVNWorkingKey").Value,
                                CCAVNAccessCode = r.Element("CCAVNAccessCode").Value,
                                CCAVNWorkingKeyBqr = r.Element("CCAVNWorkingKeyBqr").Value,
                                CCAVNAccessCodeBqr = r.Element("CCAVNAccessCodeBqr").Value,
                                BranchId = r.Element("BranchId").Value,
                            };
            foreach (var item in objBranch)
            {
                if (Convert.ToInt32(item.BranchId) == BranchId)
                {
                    _workingKey = item.CCAVNWorkingKey;
                   _strAccessCode = item.CCAVNAccessCode;
                    _workingKeyBqr = item.CCAVNWorkingKeyBqr;
                    _strAccessCodeBqr = item.CCAVNAccessCodeBqr;


                    log.Info("_workingKey :" + _workingKey);

                    log.Info("_strAccessCode :" + _strAccessCode);

                    log.Info("_workingKeyBqr :" + _workingKeyBqr);
                    log.Info("_strAccessCodeBqr :" + _strAccessCodeBqr);
                    break;
                }
            }
        }

    }
}
