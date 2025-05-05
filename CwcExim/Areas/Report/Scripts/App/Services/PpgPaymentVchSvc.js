(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgPaymentVchSvc', function ($http) {

        this.GetPaymentVchReport = function (fdt, tdt, purpose) {
            return $http({
                url: "/Report/Ppg_ReportCWC/GetPaymentVoucherReport/",
                method: "POST",
                params: { Fdt: fdt, Tdt: tdt, Purpose: purpose }
            });
        }
    });
})()