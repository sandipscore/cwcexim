(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgFCLTOLCLInvoiceEditService', function ($http) {
        this.GetEmptyInvoiceDetails = function (InvoiceId) {
            return $http.get('/Import/Ppg_CWCImport/GetFCLLCLInvoiceDetails/?InvoiceId=' + InvoiceId);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, PartyId, PayeeId, ContainerIdClass, CFSCode, Size) {
            debugger;

            return $http({
                url: "/Import/Ppg_CWCImport/CalculateFCLToLCLInvEdit",
                method: "POST",
                params: { Size: Size, PartyPdaId: PartyId, ContainerClassId: ContainerIdClass, CFSCode: CFSCode, InvoiceDate: InvoiceDate, InvoiceId: InvoiceId }
                //  data: JSON.stringify(conatiners)
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
                url: "/Import/Ppg_CWCImport/AddEditFCLToLCLPaymentSheet/",
                method: "POST",
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()