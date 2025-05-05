(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PPG_BOEQueryService', function ($http) {

        this.GetOBLDetails = function (OBLNo, SearchBy) {
            return $http({
                url: "/Report/Ppg_ReportCWCV2/BOENoQuery/",
                method: "GET",
                params: { OBLNo: OBLNo, SearchBy: SearchBy },
            });
        }

        this.GetBOEDetailsForBOEQuery = function (OBLNo, SearchBy) {
            return $http({
                url: "/Report/Ppg_ReportCWCV2/GetBOEGateDetail/",
                method: "GET",
                params: { OBLNo: OBLNo, SearchBy: SearchBy },
            });
        }



        //this.OBLEntrySave = function (Obj, OBLEntryDetails) {
        //    return $http({
        //        url: "/Report/Ppg_ReportCWCV2/AddEditOBLWiseContainerEntry/",
        //        method: "POST",
        //        data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
        //    });
        //}

    });
})()