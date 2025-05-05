(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgEmptyGateInvoiceEditService', function ($http) {
        this.GetYardInvoiceDetails = function (InvoiceId) {
            return $http.get('/GateOperation/Ppg_GateOperation/GetEmptyGateInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceDate, InvoiceId, FumiType, TaxType, PartyId, Size, conatiners) {
            return $http({
                url: "/GateOperation/Ppg_GateOperation/GetEmptyGateInvPaymentSheetForEdit?InvoiceId=" + InvoiceId,
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
                url: "/GateOperation/Ppg_GateOperation/EditEmptyGateInvPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()