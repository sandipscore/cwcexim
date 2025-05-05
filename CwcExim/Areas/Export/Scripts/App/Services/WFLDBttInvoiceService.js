(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('BttInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Export/WFLD_CWCExport/GetPaymentSheetShipBillNo/?StuffingReqId=' + ReqNo);
        }

        this.GetStuffingReq = function () {
            return $http.get('/Export/WFLD_CWCExport/GetStuffindNoForBTT');
        }

        this.GetPartyForBTT = function () {
            return $http.get('/Export/WFLD_CWCExport/GetPartyForBTT');
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/Wfld_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }
       
        this.ContainerSelect = function (InvoiceId, DeliveryDate, InvoiceDate, AppraisementId, TaxType, conatiners,PartyId,PayeeId,SEZ, NoOfVehicles) {
            return $http({
                url: "/Export/WFLD_CWCExport/GetBTTPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { DeliveryDate: DeliveryDate, InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType,PartyId : PartyId,PayeeId : PayeeId, SEZ: SEZ, NoOfVehicles: NoOfVehicles },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj, Vno, VehicleNumber) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Export/WFLD_CWCExport/AddEditBTTPaymentSheet/",
                method: "POST",
                params: { Vehicle: Vno, VehicleNumber: VehicleNumber },
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

    });
})()