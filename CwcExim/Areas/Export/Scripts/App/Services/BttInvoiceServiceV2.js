(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('BttInvoiceServiceV2', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Export/Ppg_CWCExportV2/GetPaymentSheetShipBillNo/?StuffingReqId=' + ReqNo);
        }

        this.GetStuffingReq = function () {
            return $http.get('/Export/Ppg_CWCExportV2/GetStuffindNoForBTT');
        }

        this.GetPartyForBTT = function () {
            return $http.get('/Export/Ppg_CWCExportV2/GetPartyForBTT');
        }


        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType,PartyId, conatiners,SEZ) {
            return $http({
                url: "/Export/Ppg_CWCExportV2/GetBTTPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType,  PartyId: PartyId,SEZ:SEZ},
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj) {
            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Export/Ppg_CWCExportV2/AddEditBTTPaymentSheet/",
                method: "POST",
                
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }

    });
})()