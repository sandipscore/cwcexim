(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgMiscInvoiceEditService', function ($http) {
        this.GetYardInvoiceDetails = function (InvoiceId) {
            return $http.get('/CashManagement/Ppg_CashManagement/GetMiscInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceDate, InvoiceId, FumiType, TaxType, PartyId, Size, Ctype,Amt) {
            return $http({
                url: "/CashManagement/Ppg_CashManagement/GetMIscInvPaymentSheetForEdit?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, FumigationChargeType: FumiType, InvoiceType: TaxType, PartyId: PartyId, size: Size, Purpose: Ctype, Amount: Amt },
                
            });
        }
        this.GetAppNoForYard = function (Module) {


            return $http({
                url: "/Import/Ppg_CWCImport/GetInvoiceForEdit",
                method: "GET",
                params: { Module: Module }
            });

        }
        this.GenerateInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/CashManagement/Ppg_CashManagement/EditMiscInvPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()