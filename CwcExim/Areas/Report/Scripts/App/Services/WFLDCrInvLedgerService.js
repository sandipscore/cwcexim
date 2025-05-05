(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLDCrInvLedgerService', function ($http) {

        this.GetLedgerReport = function (pid, fdt, tdt) {
            return $http({
                url: "/Report/WFLD_ReportCWC/GetCashReceiptInvoiceLedgerReport/",
                method: "POST",
                params: { partyId: pid, fromdate: fdt, todate: tdt }
            });
        }
    });
})()