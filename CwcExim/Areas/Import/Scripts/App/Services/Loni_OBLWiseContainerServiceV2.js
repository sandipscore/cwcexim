(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('OBLWiseContainerService', function ($http) {

        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/Loni_OblEntryV2/GetOBLWiseContainerDetails/",
                method: "GET",
                params: { OBLNo: OBLNo },
            });
        }

        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/Loni_OblEntryV2/AddEditOBLWiseContainerEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

    });
})()