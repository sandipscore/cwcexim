(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('BttInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Export/Chn_CWCExport/GetPaymentSheetShipBillNo/?StuffingReqId=' + ReqNo);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType,SEZ, conatiners) {
            return $http({
                url: "/Export/Chn_CWCExport/GetBTTPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType,SEZ:SEZ },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj,SEZ) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Export/Chn_CWCExport/AddEditBTTPaymentSheet/",
                method: "POST",
                param:{SEZ:SEZ},
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/CHN_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }

    });
})()