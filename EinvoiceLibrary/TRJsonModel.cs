using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceLibrary
{
    public class TRJsonModel
    {
        public TRRequestInfo requestInfo { get; set; } = new TRRequestInfo();
        public string referenceID { get; set; }
        public string txnAuthDate { get; set; }       
        public TRAddInfo addInfo { get; set; } = new TRAddInfo();

        //sb.Append("{");
        //                sb.Append("\"requestInfo\": {");
        //                sb.Append("\"pgMerchantID\": \"HDFC000007951897\",");
        //                sb.Append("\"orderNo\": \"INV1001\",");
        //                sb.Append("\"upiTxnID\": \"TXNID5600978\",");
        //                sb.Append("\"txnStatusType\": \"ChkTxn\"");
        //                sb.Append("},");
        //                sb.Append("\"referenceID\": \"\",");
        //                sb.Append("\"txnAuthDate\": \"2021-12-09 02:42:55 PM\",");
        //                sb.Append("\"addInfo\": {");
        //                sb.Append("\"addInfo1\": \"NA\",");
        //                sb.Append("\"addInfo2\": \"NA\",");
        //                sb.Append("\"addInfo3\": \"NA\",");
        //                sb.Append("\"addInfo4\": \"NA\",");
        //                sb.Append("\"addInfo5\": \"NA\",");
        //                sb.Append("\"addInfo6\": \"NA\",");
        //                sb.Append("\"addInfo7\": \"NA\",");
        //                sb.Append("\"addInfo8\": \"NA\",");
        //                sb.Append("\"addInfo9\": \"NA\",");
        //                sb.Append("\"addInfo10\": \"NA\"");

        //                sb.Append("}}");
    }
    public class TRRequestInfo
    {
        public string pgMerchantID { get; set; }
        public string orderNo { get; set; }
        public string upiTxnID { get; set; }
        public string txnStatusType { get; set; }
    }

    public class TRAddInfo
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
