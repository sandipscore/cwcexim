(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgRentInvoiceEditService', function ($http) {
        this.GetYardInvoiceDetails = function (InvoiceId) {
            return $http.get('/CashManagement/Ppg_CashManagement/GetRentInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceDate, InvoiceId, FumiType, TaxType, PartyId, Size, conatiners) {
            return $http({
                url: "/CashManagement/Ppg_CashManagement/GetRentInvPaymentSheetForEdit?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, FumigationChargeType: FumiType, InvoiceType: TaxType, PartyId: PartyId, size: Size },
                data: JSON.stringify(conatiners)
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
                url: "/CashManagement/Ppg_CashManagement/EditRentInvPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()