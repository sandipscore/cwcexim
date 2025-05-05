(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('Wlj_OBLEntryService', function ($http) {
        
        this.GetOBLDetails = function (ContainerNo, CFSCode, ContainerSize, IGM_No, IGM_Date, OBLEntryId) {
            return $http({
                url: "/Import/Wlj_OblEntry/GetOBLDetails/",
                method: "GET",
                params: { CFSCode: ContainerNo, ContainerNo: ContainerNo, ContainerSize: ContainerSize, IGM_No: IGM_No, IGM_Date: IGM_Date, OBLEntryId: OBLEntryId },
            });
        }


        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/Wlj_OblEntry/AddEditOBLEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

    });
})()