(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLD_BOEQueryService', function ($http) {

        this.GetOBLDetails = function (OBLNo, SearchBy) {
            return $http({
                url: "/Report/WFLD_ReportCWC/BOENoQuery/",
                method: "GET",
                params: { OBLNo: OBLNo, SearchBy: SearchBy },
            });
        }

        this.GetBOEDetailsForBOEQuery = function (OBLNo, SearchBy) {
            return $http({
                url: "/Report/WFLD_ReportCWC/GetBOEGateDetail/",
                method: "GET",
                params: { OBLNo: OBLNo, SearchBy: SearchBy },
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