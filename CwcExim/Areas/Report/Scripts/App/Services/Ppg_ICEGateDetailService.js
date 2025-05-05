(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('Ppg_ICEGateDetailService', function ($http) {

        this.GetOBLDetails = function (OBLNo, SearchBy) {
            return $http({
                url: "/Report/Ppg_ReportCWCV2/GetICEGateDetail/",
                method: "GET",
                params: { OBLNo: OBLNo, SearchBy: SearchBy },
            });
        }

        this.GetOBLDetailsForOBLQuery = function (oblnum,obldate, SearchBy) {
            return $http({
                url: "/Report/Ppg_ReportCWCV2/GetICEGateDetail/",
                method: "GET",
                params: { oblnum: oblnum, obldate:obldate,SearchBy: SearchBy },
            });
        }



        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Report/Ppg_ReportCWCV2/AddEditOBLWiseContainerEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

    });
})()