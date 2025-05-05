(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('VRN_BillCumSDLedgerSvc', function ($http) {

        this.GetLedgerReport = function (pid, fdt, tdt) {
            return $http({
                url: "/Report/Ppg_ReportCWC/GetBillCumSDLedgerReport/",
                method: "POST",
                params: { partyId: pid, fromdate: fdt, todate: tdt }
            });
        }
    });
})()