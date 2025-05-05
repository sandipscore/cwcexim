(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('ReeferService', function ($http) {
       this.LoadContainer = function (ReqNo) {
           return $http.get('/Export/Hdb_CWCExport/GetReeferPluginRequest/?InvoiceId=' + ReqNo);
        }
 
       this.CalculateCharges = function (InvoiceDate, PartyId, TaxType, conatiners, InvoiceId, PayeeId, PayeeName, ExportUnder, Distance) {
            return $http({
                url: "/Export/Hdb_CWCExport/GetReeferPaymentSheet",
                method: "POST",
                params: {
                    InvoiceDate: InvoiceDate, PartyId: PartyId, InvoiceType: TaxType, InvoiceId: InvoiceId, PayeeId: PayeeId, PayeeName: PayeeName,
                    ExportUnder: ExportUnder , Distance: Distance
                },
                data: JSON.stringify(conatiners)
            });
        }
       this.AddEditReeferInv = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/Hdb_CWCExport/AddEditReeferPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
       }
       this.LoadPayerList = function (PayerPage) {
           debugger;
           return $http.get('/Export/Hdb_CWCExport/LoadPayerList/?PartyCode=&Page=' + PayerPage);
       }
       this.SearchPayerByPayerCode = function (PartyCode) {
           return $http.get('/Export/Hdb_CWCExport/SearchPayerNameByPayeeCode/?PartyCode=' + PartyCode);
       }

    });
})()