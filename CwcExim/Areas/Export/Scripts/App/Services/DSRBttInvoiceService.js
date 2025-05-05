(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('BttInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Export/DSR_CWCExport/GetPaymentSheetShipBillNo/?StuffingReqId=' + ReqNo);
        }

        this.GetStuffingReq = function () {
            return $http.get('/Export/DSR_CWCExport/GetStuffindNoForBTT');
        }

        this.GetPartyForBTT = function () {
            return $http.get('/Export/DSR_CWCExport/GetPartyForBTT');
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/DSR_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId,PartyId, TaxType, conatiners, NoOfVehicles,ExportUnder) {
            return $http({
                url: "/Export/DSR_CWCExport/GetBTTPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, PartyId: PartyId, TaxType: TaxType, NoOfVehicles: NoOfVehicles, ExportUnder: ExportUnder },
                data: JSON.stringify(conatiners)
            });
        }       

        this.GenerateInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Export/DSR_CWCExport/AddEditBTTPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


    });
})()