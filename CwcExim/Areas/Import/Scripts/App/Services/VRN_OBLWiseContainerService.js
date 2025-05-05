(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('VRN_OBLWiseContainerService', function ($http) {
        
        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/VRN_OblEntry/GetOBLWiseContainerDetails/",
                method: "GET",
                params: { OBLNo: OBLNo},
            });
        }

        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/VRN_OblEntry/AddEditOBLWiseContainerEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

    });
})()