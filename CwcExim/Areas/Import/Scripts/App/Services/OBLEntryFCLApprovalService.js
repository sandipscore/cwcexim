(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('OBLWiseContainerService', function ($http) {

        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/Ppg_OblEntryV2/GetOBLWiseContainerDetails/",
                method: "GET",
                params: { OBLNo: OBLNo },
            });
        }

        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/Ppg_OblEntryV2/AddEditOBLWiseContainerEntryFCLApproval/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

        this.OBLEntryDetails = function () {
            return $http({
                url: "/Import/Ppg_OblEntryV2/GetOblEntryDetails/",
                method: "GET",
            });
        }
        this.OBLEntryDetailsByObl = function (oblID) {
            debugger;
            return $http({
                url: "/Import/Ppg_OblEntryV2/GetOblEntryDetailsByOblNo/",
                method: "GET",
                params: { ID: oblID },
            });
        }


    });
})()