(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('Dnd_OBLWiseContainerService', function ($http) {

        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/Dnd_OblEntry/GetOBLWiseContainerDetails/",
                method: "GET",
                params: { OBLNo: OBLNo },
            });
        }
        this.GetContCBT = function (ContainerNo, ContainerSize) {
            return $http({
                url: "/Import/Dnd_OblEntry/GetCONTCBT/",
                method: "GET",
                params: { ContainerNo: ContainerNo ,ContainerSize:ContainerSize},
            });
        }
        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/Dnd_OblEntry/AddEditOBLWiseContainerEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

    });
})()