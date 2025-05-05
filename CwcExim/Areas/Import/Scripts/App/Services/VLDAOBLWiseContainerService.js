(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('VLDAOBLWiseContainerService', function ($http) {

        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/VLDA_OblEntry/GetOBLWiseContainerDetails/",
                method: "GET",
                params: { OBLNo: OBLNo },
            });
        }


        this.GetContCBT = function (ContType) {
            return $http({
                url: "/Import/VLDA_OblEntry/GetCONTCBT/",
                method: "GET",
                params: { ContTypeData: ContType },
            });
        }


        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/VLDA_OblEntry/AddEditOBLWiseContainerEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

    });
})()