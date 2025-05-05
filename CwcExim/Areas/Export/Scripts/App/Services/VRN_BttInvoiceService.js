(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('VRN_BttInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Export/VRN_CWCExport/GetPaymentSheetShipBillNo/?StuffingReqId=' + ReqNo);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners, PartyId, OTHour, SEZ) {
            debugger;
            return $http({
                url: "/Export/VRN_CWCExport/GetBTTPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType, PartyId: PartyId, OTHour:OTHour,ExportUnder:SEZ },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/VRN_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }
        this.GenerateInvoice = function (InvoiceObj,SEZ) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Export/VRN_CWCExport/AddEditBTTPaymentSheet/",
                method: "POST",
                params: { ExportUnder1: SEZ },
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

    });
})()