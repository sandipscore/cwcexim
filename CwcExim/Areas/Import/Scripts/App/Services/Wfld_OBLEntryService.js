(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('Wfld_OBLEntryService', function ($http) {
        
        this.GetOBLDetails = function (ContainerNo, CFSCode, ContainerSize, OBLEntryId) {
            return $http({
                url: "/Import/Wfld_OblEntry/GetOBLDetails/",
                method: "GET",
                params: { CFSCode: ContainerNo, ContainerNo: ContainerNo, ContainerSize: ContainerSize,  OBLEntryId: OBLEntryId },
            });
        }


        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/Wfld_OblEntry/AddEditOBLEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }

    });
})()