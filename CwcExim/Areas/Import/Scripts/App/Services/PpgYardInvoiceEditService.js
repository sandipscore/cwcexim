(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgYardInvoiceEditService', function ($http) {
        this.GetYardInvoiceDetails = function (InvoiceId) {           
            return $http.get('/Import/Ppg_CWCImport/GetYardInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners,OTHour,PartyIDD,PayeeIDD) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType, OTHours: OTHour, PartyId: PartyIDD, PayeeId: PayeeIDD },
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
                url: "/Import/Ppg_CWCImport/AddEditContPaymentSheet/",
                method: "POST",
                params: { IsDirect: 0 },
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()