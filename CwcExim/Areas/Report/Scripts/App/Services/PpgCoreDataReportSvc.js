(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgCoreDataReportSvc', function ($http) {

        this.GetCoreDataReport = function (m, y) {
            return $http({
                url: "/Report/Ppg_ReportCWC/GetRptCoreDataReport/",
                method: "POST",
                params: { month: m, year: y }
            });
        }

        this.PrintCoreReport = function (m, y,BE1,BE2,BE3,PRO1,PRO2,PRO3,ACT1,ACT2,ACT3, rptData) {
            debugger;
            return $http({
                url: "/Report/Ppg_ReportCWC/GeneratingPDFforCoreDataRpt/",
                method: "POST",
                params: { month: m, year: y,BE1:BE1,BE2:BE2,BE3:BE3,PRO1:PRO1,PRO2:PRO2,PRO3:PRO3,ACT1:ACT1,ACT2:ACT2,ACT3:ACT3 },
                data: rptData
            });
        }



    });
})()