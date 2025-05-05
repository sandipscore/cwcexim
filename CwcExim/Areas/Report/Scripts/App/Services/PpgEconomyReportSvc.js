(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgEconomyReportSvc', function ($http) {

        this.GetEconomyReport = function (m,y) {
            return $http({
                url: "/Report/Ppg_ReportCWC/GetEconomyReport/",
                method: "POST",
                params: { month: m, year: y }
            });
        }
         
        this.SaveEconomyReport = function (m, y, rptData) {
            debugger;
            return $http({
                url: "/Report/Ppg_ReportCWC/SaveEconomyReport/",
                method: "POST",
                params: { month: m, year: y },
                data: rptData
            });
        }


    });
})()