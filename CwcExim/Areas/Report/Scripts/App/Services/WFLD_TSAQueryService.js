(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLD_TSAQueryService', function ($http) {

        this.GetOBLDetails = function (OBLNo, SearchBy) {
            return $http({
                url: "/Report/WFLD_ReportCWC/TSAQuery/",
                method: "GET",
                params: { OBLNo: OBLNo, SearchBy: SearchBy },
            });
        }

        this.GetTSADetailsForTSAQuery = function (TSANo,OBLNo, SearchBy) {
            return $http({
                url: "/Report/WFLD_ReportCWC/GetTSAGateDetail/",
                method: "GET",
                params: { TSANo: TSANo, OBLNo: OBLNo, SearchBy: SearchBy },
            });
        }



        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Report/WFLD_ReportCWC/AddEditOBLWiseContainerEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

    });
})()