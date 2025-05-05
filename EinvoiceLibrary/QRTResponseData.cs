using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public class QRTResponseData
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static object BindQRTResponseData(QRTResponseJson RMgs)
       // public static object BindQRTResponseData(string RMgs)
        {
            var result = RMgs;
            //var result = JsonConvert.DeserializeObject<QRTResponseJson>(RMgs);           
            QRResponseInfo ObjQRT = new QRResponseInfo();

            ObjQRT.InvoiceNo = result.apiResp.orderNo;
            ObjQRT.Amount = result.apiResp.amount;
            ObjQRT.UpiTranRefNo = result.apiResp.upiTranRefNo;
            ObjQRT.TxnAuthDate = result.apiResp.txnAuthDate;
            ObjQRT.TxnType = result.apiResp.txnType;
            ObjQRT.ApprovalNumber = result.apiResp.approvalNumber;
            ObjQRT.PayerVPA = result.apiResp.payerVPA;
            ObjQRT.ResponseCode = result.apiResp.responseCode;
            ObjQRT.StatusDescription = result.apiResp.statusDescription;
            ObjQRT.CustRefNo = result.apiResp.custRefNo;
            ObjQRT.MerchantTranxRef = result.apiResp.merchantTranxRef;
            ObjQRT.RefID = result.apiResp.refID;
            ObjQRT.RefUrl = result.apiResp.refUrl;
            ObjQRT.Status = result.apiResp.status;
            ObjQRT.TxnId = result.apiResp.txnId;
            ObjQRT.PayerAccountNumber = result.payerInfo.payerAccountNumber;
            ObjQRT.PayerBankName = result.payerInfo.payerBankName;
            ObjQRT.PayerBankIFSC = result.payerInfo.payerBankIFSC;
            ObjQRT.PayerMobileNumber = result.payerInfo.payerMobileNumber;
            ObjQRT.DynamicPayeeValue = result.payeeInfo.dynamicPayeeValue;
            ObjQRT.PayeeVPA = result.payeeInfo.payeeVPA;
            ObjQRT.ConsentName = result.consentName;
            ObjQRT.AddInfo1 = result.addInfo.addInfo1;
            ObjQRT.AddInfo2 = result.addInfo.addInfo2;
            ObjQRT.AddInfo3 = result.addInfo.addInfo3;
            ObjQRT.AddInfo4 = result.addInfo.addInfo4;
            ObjQRT.AddInfo5 = result.addInfo.addInfo5;
            ObjQRT.AddInfo6 = result.addInfo.addInfo6;
            ObjQRT.AddInfo7 = result.addInfo.addInfo7;
            ObjQRT.AddInfo8 = result.addInfo.addInfo8;
            ObjQRT.AddInfo9 = result.addInfo.addInfo9;
            ObjQRT.AddInfo10 = result.addInfo.addInfo10;
            
            return ObjQRT;
        }
        
    }
}
