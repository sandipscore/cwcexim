(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgDeliInvoiceEditService', function ($http) {
        this.GetDeliInvoiceDetails = function (InvoiceId) {
            return $http.get('/Import/Ppg_CWCImport/GetDeliInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceDate, InvoiceType, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName, conatiners, OTHours, InvoiceId) {

            return $http({
                url: "/Import/Ppg_CWCImport/GetDeliveryPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, InvoiceType: InvoiceType, AppraisementId: AppraisementId, DeliveryType: DeliveryType, AppraisementNo: AppraisementNo, AppraisementDate: AppraisementDate, PartyId: PartyId, PartyName: PartyName, PartyAddress: PartyAddress, PartyState: PartyState, PartyStateCode: PartyStateCode, PartyGST: PartyGST, PayeeId: PayeeId, PayeeName: PayeeName, OTHours: OTHours, InvoiceId: InvoiceId },
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
                url: "/Import/Ppg_CWCImport/AddEditDeliPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()