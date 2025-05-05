(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLD_LINEQueryService', function ($http) {

        this.GetOBLDetails = function (OBLNo, SearchBy) {
            return $http({
                url: "/Report/WFLD_ReportCWC/LINENoQuery/",
                method: "GET",
                params: { OBLNo: OBLNo, SearchBy: SearchBy },
            });
        }

        this.GetLINEDetailsForLINEQuery = function (LINENo,OBLNo, SearchBy) {
            return $http({
                url: "/Report/WFLD_ReportCWC/GetLineGateDetail/",
                method: "GET",
                params: { LINENo: LINENo, OBLNo: OBLNo, SearchBy: SearchBy },
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