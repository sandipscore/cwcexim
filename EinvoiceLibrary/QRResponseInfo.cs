using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public class QRResponseInfo
    {
        public string QRTAckId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public int UpiTranRefNo { get; set; }
        public string TxnAuthDate { get; set; }
        public string TxnType { get; set; }
        public string ApprovalNumber { get; set; }
        public string PayerVPA { get; set; }
        public string ResponseCode { get; set; }
        public string StatusDescription { get; set; }
        public string CustRefNo { get; set; }
        public string MerchantTranxRef { get; set; }
        public string RefID { get; set; }
        public string RefUrl { get; set; }
        public string Status { get; set; }
        public string TxnId { get; set; }
        public string PayerAccountNumber { get; set; }
        public string PayerBankName { get; set; }
        public string PayerBankIFSC { get; set; }
        public string PayerMobileNumber { get; set; }
        public string DynamicPayeeValue { get; set; }
        public string PayeeVPA { get; set; }
        public string ConsentName { get; set; }
        public string AddInfo1 { get; set; }
        public string AddInfo2 { get; set; }
        public string AddInfo3 { get; set; }
        public string AddInfo4 { get; set; }
        public string AddInfo5 { get; set; }
        public string AddInfo6 { get; set; }
        public string AddInfo7 { get; set; }
        public string AddInfo8 { get; set; }
        public string AddInfo9 { get; set; }
        public string AddInfo10 { get; set; }
    }

    public class QRTResponseJson
    {
        public QRTapiResp apiResp { get; set; } = new QRTapiResp();        
        public QRTpayerInfo payerInfo { get; set; } = new QRTpayerInfo();
        public QRTpayeeInfo payeeInfo { get; set; } = new QRTpayeeInfo();
        public string consentName { get; set; }      
        public QRTaddInfo addInfo { get; set; } = new QRTaddInfo();
    }
    public class QRTapiResp
    {
        public string orderNo { get; set; }
        public decimal amount { get; set; }
        public int upiTranRefNo { get; set; }
        public string txnAuthDate { get; set; }
        public string txnType { get; set; }
        public string approvalNumber { get; set; }
        public string payerVPA { get; set; }
        public string responseCode { get; set; }
        public string statusDescription { get; set; }
        public string custRefNo { get; set; }
        public string merchantTranxRef { get; set; }
        public string refID { get; set; }
        public string refUrl { get; set; }
        public string status { get; set; }
        public string txnId { get; set; }
        
    }
    public class QRTpayerInfo
    {
        public string payerAccountNumber { get; set; }
        public string payerBankName { get; set; }
        public string payerBankIFSC { get; set; }
        public string payerMobileNumber { get; set; }
        
    }
    public class QRTpayeeInfo
    {        
        public string dynamicPayeeValue { get; set; }
        public string payeeVPA { get; set; }
                
    }
    public class QRTaddInfo
    {
        public string addInfo1 { get; set; }
        public string addInfo2 { get; set; }
        public string addInfo3 { get; set; }
        public string addInfo4 { get; set; }
        public string addInfo5 { get; set; }
        public string addInfo6 { get; set; }
        public string addInfo7 { get; set; }
        public string addInfo8 { get; set; }
        public string addInfo9 { get; set; }
        public string addInfo10 { get; set; }
    }
}
