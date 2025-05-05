(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgEmptyContInvoiceEditService', function ($http) {
        this.GetEmptyInvoiceDetails = function (InvoiceId) {
            return $http.get('/Import/Ppg_CWCImport/GetEmptyContInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, forType, conatiners, PartyIDD, PayeeIDD) {
            debugger;
           
            return $http({
                url: "/Import/Ppg_CWCImport/GetEmptyEditContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, InvoiceType: TaxType, AppraisementId: AppraisementId, PartyId: PartyIDD, InvoiceFor: forType },
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
                url: "/Import/Ppg_CWCImport/AddEditECDeliveryEditPaymentSheet/",
                method: "POST",
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()