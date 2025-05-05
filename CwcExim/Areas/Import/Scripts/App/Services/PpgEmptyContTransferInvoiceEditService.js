(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgEmptyContTransferInvoiceEditService', function ($http) {
        this.GetEmptyInvoiceDetails = function (InvoiceId) {
            return $http.get('/Import/Ppg_CWCImport/GetEmptyContTransferInvoiceDetails/?InvoiceId=' + InvoiceId);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, PartyId, PayeeId, StuffingReqDate, ContainerNo, CFSCode, Size, DestuffingDate) {
            debugger;

            return $http({
                url: "/Import/Ppg_CWCImport/CalculateEmptyContTransferInvEdit",
                method: "POST",
                params: { InvoiceDate: InvoiceDate, InvoiceType: TaxType, CFSCode: CFSCode, ContainerNo: ContainerNo, Size: Size, EntryDate: StuffingReqDate, EmptyDate: DestuffingDate, RefId: AppraisementId, PartyId: PartyId, PayeeId: PayeeId, InvoiceId: InvoiceId }
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
                url: "/Import/Ppg_CWCImport/AddEditEmtpyTranserPaymentSheet/",
                method: "POST",
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()