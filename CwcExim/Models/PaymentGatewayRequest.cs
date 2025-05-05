using CwcExim.Areas.CashManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class PaymentGatewayRequest
    {
        public int PaymentGateWayRequestId { get; set; }
        public decimal tid { get; set; }
        public decimal merchant_id { get; set; }
        public long order_id { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string redirect_url { get; set; }
        public string cancel_url { get; set; }
        public string language { get; set; }
        public string billing_name { get; set; }
        public string billing_address { get; set; }
        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_zip { get; set; }
        public string billing_country { get; set; }
        public decimal billing_tel { get; set; }
        public string billing_email { get; set; }
        public string delivery_name { get; set; }
        public string delivery_address { get; set; }
        public string delivery_city { get; set; }
        public string delivery_state { get; set; }
        public string delivery_zip { get; set; }
        public string delivery_country { get; set; }
        public decimal delivery_tel { get; set; }
        public string issuing_bank { get; set; }
        public decimal mobile_number { get; set; }
        public string created_on { get; set; }

        public int merchant_param1 { get; set; }
        WFLD_DirectOnlinePayment objDirectOnlinePayment { get; set; } = new WFLD_DirectOnlinePayment();
    }

    public class PaymentGatewayResponse
    {

        public int PaymentGateWayResponseId { get; set; }
        public long order_id { get; set; }
        public decimal tracking_id { get; set; }
        public string bank_ref_no { get; set; }
        public string order_status { get; set; }
        public string failure_message { get; set; }
        public string payment_mode { get; set; }
        public short status_code { get; set; }
        public string status_message { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string billing_name { get; set; }
        public string billing_address { get; set; }
        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_zip { get; set; }
        public string billing_country { get; set; }
        public decimal billing_tel { get; set; }
        public string billing_email { get; set; }
        public string delivery_name { get; set; }
        public string delivery_address { get; set; }
        public string delivery_city { get; set; }
        public string delivery_state { get; set; }
        public string delivery_zip { get; set; }
        public string delivery_country { get; set; }
        public decimal delivery_tel { get; set; }
        public char vault { get; set; }
        public string offer_type { get; set; }
        public string offer_code { get; set; }
        public decimal discount_value { get; set; }
        public decimal mer_amount { get; set; }
        public decimal eci_value { get; set; }
        public char retry { get; set; } = char.MinValue;
        public decimal response_code { get; set; }
        public string billing_notes { get; set; }
        public DateTime? trans_date { get; set; }
        public string bin_country { get; set; }
        public string created_on { get; set; }
        public int merchant_param1 { get; set; }

        public string PageFor { get; set; }
    }

}