(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgLoadedContInvoiceEditService', function ($http) {
        this.GetYardInvoiceDetails = function (InvoiceId) {
            return $http.get('/Export/Ppg_CWCExport/GetLoadedContainerInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType,PartyId,PayeeId, conatiners) {
            return $http({
                url: "/Export/Ppg_CWCExport/GetLoadedContainerPaymentSheet" ,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, InvoiceType: TaxType, StuffingReqId: AppraisementId, PayeeId: PayeeId, PartyId: PartyId, InvoiceId: InvoiceId },
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
                url: "/Export/Ppg_CWCExport/AddEditLoadedContPaymentSheetForEdit/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()