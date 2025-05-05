(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('BttContService', function ($http) {
        this.CalculateCharges = function (InvoiceDate, PartyId, PayeeId, TaxType, conatiners, CasualLabour,Distance, InvoiceId,ExportUnder) {
            return $http({
                url: "/Export/Hdb_CWCExport/GetContainerPaymentSheet",
                method: "POST",
                params: { InvoiceDate: InvoiceDate, PartyId: PartyId, PayeeId: PayeeId, InvoiceType: TaxType, InvoiceId: InvoiceId, CasualLabour: CasualLabour, Distance: Distance, AppraisementId: 0, ExportUnder: ExportUnder },
                data: JSON.stringify(conatiners)
            });
        }
        this.AddEditBTTContPS = function (InvoiceObj) {
            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/Hdb_CWCExport/AddEditBTTContPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.LoadPartyList = function (page)
        {
            debugger;
            return $http.get('/Export/Hdb_CWCExport/LoadPartyList/?PartyCode=&Page=' + page);
        }
        this.SearchPartyNameByPartyCode = function (PartyCode)
        {
            return $http.get('/Export/Hdb_CWCExport/SearchPartyNameByPartyCode/?PartyCode=' + PartyCode);
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
