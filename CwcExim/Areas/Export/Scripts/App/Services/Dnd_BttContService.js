(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('BttContService', function ($http) {
        this.CalculateCharges = function (InvoiceDate, PartyId,  TaxType, conatiners,  InvoiceId) {
            return $http({
                url: "/Export/Hdb_CWCExport/GetBTTPaymentSheet",
                method: "POST",
                params: { InvoiceDate: InvoiceDate, PartyId: PartyId, InvoiceType: TaxType, InvoiceId: InvoiceId, AppraisementId: 0, },
                data: JSON.stringify(conatiners)
            });
        }
        this.AddEditBTTContPS = function (InvoiceObj) {
            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/Dnd_CWCExport/AddEditBTTContPaymentSheet",
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
            return $http.get('/Export/Dnd_CWCExport/LoadPartyList/?PartyCode=&Page=' + page);
        }
        this.SearchPartyNameByPartyCode = function (PartyCode)
        {
            return $http.get('/Export/Dnd_CWCExport/SearchPartyNameByPartyCode/?PartyCode=' + PartyCode);
        }

        this.LoadPayerList = function (PayerPage) {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/LoadPayerList/?PartyCode=&Page=' + PayerPage);
        }
        this.SearchPayerByPayerCode = function (PartyCode) {
            return $http.get('/Export/Dnd_CWCExport/SearchPayerNameByPayeeCode/?PartyCode=' + PartyCode);
        }
    });
})()
