using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public class TransactionJsonFormat
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string BindQRTransactionJson(DataSet ds,string InvoiceNo)
        {          
            TRJsonModel obj = new TRJsonModel();
            try
            {
                #region Data Binding 
                
                if(ds.Tables[0].Rows.Count>0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {                        
                        obj.requestInfo.pgMerchantID = Convert.ToString(dr["MerchantID"]);
                        obj.requestInfo.orderNo = InvoiceNo;
                        obj.requestInfo.upiTxnID = Convert.ToString(dr["upiTxnID"]);  
                        obj.requestInfo.txnStatusType= Convert.ToString(dr["txnStatusType"]);
                    }
                }
                obj.referenceID = "";
                obj.txnAuthDate = ds.Tables[0].Rows[0]["txnAuthDate"].ToString();

                if (ds.Tables[0].Rows.Count > 0)
                {                   
                    obj.addInfo.addInfo1 = "NA";
                    obj.addInfo.addInfo2 = "NA";
                    obj.addInfo.addInfo3 = "NA";
                    obj.addInfo.addInfo4 = "NA";
                    obj.addInfo.addInfo5 = "NA";
                    obj.addInfo.addInfo6 = "NA";
                    obj.addInfo.addInfo7 = "NA";
                    obj.addInfo.addInfo8 = "NA";
                    obj.addInfo.addInfo9 = "NA";
                    obj.addInfo.addInfo10 = "NA";
                }
                

                #endregion

                string json = JsonConvert.SerializeObject(obj);


                return json;
            }
            catch (Exception ex)
            {
                log.Error("Error Message Transaction Json Model:" + ex.Message);
                return "";

            }
        }


        public static string BindCcavnTransactionJson(int InvoiceId,string OrderId)
        {

            //reference_no=tracking_id    AND order_no=order_id

            //string orderStatusQueryJson = "{ \"reference_no\":\"311007894457\", \"order_no\":\"637774256280319861\" }"; //Ex. { "reference_no":"CCAvenue_Reference_No" , "order_no":"123456"} 


            CcAvnJsonModel obj = new CcAvnJsonModel();
            try
            {
                #region Data Binding 
                obj.order_no = OrderId;
                obj.reference_no= null;

                #endregion

                string json = JsonConvert.SerializeObject(obj);


                return json;
            }
            catch (Exception ex)
            {
                log.Error("Error Message Transaction Json Model:" + ex.Message);
                return "";

            }
        }






    }
}


