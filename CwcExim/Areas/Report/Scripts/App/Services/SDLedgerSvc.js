(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('BillCumSDLedgerSvc', function ($http) {

        this.GetLedgerReport = function (fdt, tdt) {
            debugger;
            return $http({
                url: "/Report/Ppg_ReportCWCV2/GetSDDetails/",
                method: "POST",
                params: { fromdate: fdt, todate: tdt }
            });
        }
    });
})()