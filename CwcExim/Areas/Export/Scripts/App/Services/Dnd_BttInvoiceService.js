(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('BttInvoiceService', function ($http) {
        debugger;
        this.SelectReqNo = function (ReqId, ReqNo) {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetPaymentSheetShipBillNo/?StuffingReqId=' + ReqId+' &StuffingReqNo='+ReqNo);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType,Escort, conatiners,PartyId) {
            return $http({
                url: "/Export/Dnd_CWCExport/GetBTTPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType, Escort: Escort, PartyId: PartyId },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Export/Dnd_CWCExport/AddEditBTTPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

    });
})()