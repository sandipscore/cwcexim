(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSR_OBLEntryService', function ($http) {
        
        this.GetOBLDetails = function (ContainerNo, CFSCode, ContainerSize, IGM_No, IGM_Date, OBLEntryId) {
            return $http({
                url: "/Import/DSR_OblEntry/GetOBLDetails/",
                method: "GET",
                params: { CFSCode: ContainerNo, ContainerNo: ContainerNo, ContainerSize: ContainerSize, IGM_No: IGM_No, IGM_Date: IGM_Date, OBLEntryId: OBLEntryId },
            });
        }


        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/DSR_OblEntry/AddEditOBLEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }
        this.OBLEntryDelete = function (OblNo) {
            return $http({
                url: "/Import/DSR_OblEntry/DeleteOBLEntryByObl/",
                method: "POST",
                data: { OblNo: OblNo},
            });
        }
        
    });
})()