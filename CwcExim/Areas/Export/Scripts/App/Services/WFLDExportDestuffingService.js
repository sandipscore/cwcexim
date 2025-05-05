(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('ExportDestuffingService', function ($http) {
        this.getDestufcharges = function (stuffingid) {
            return $http.get('/Export/WFLD_CWCExport/GetChargesExportDestuffing/?ContainerStuffingId=' + stuffingid);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners) {
            return $http({
                url: "/Export/WFLD_CWCExport/GetExportDestuffingPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType },
                data: JSON.stringify(conatiners)
            });
        }
        this.GenerateInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Export/WFLD_CWCExport/AddEditExportDestuffing/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()