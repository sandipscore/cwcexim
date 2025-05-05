(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('OBLSpiltService', function ($http) {

        this.GetOBLList = function () {
            return $http({
                url: "/Import/DSR_CWCImport/GetOBLListForSpilt/",
                method: "GET"
            });
        }

        this.GetOBLDetails = function (varOBL, vaOBLDate, FCL) {
            debugger;
            return $http({
                url: "/Import/DSR_CWCImport/GetOBLDetailsForSpilt/",
                method: "GET",
                params: { OBL: varOBL, OBLDate: vaOBLDate, isFCL: FCL },
            });
        }

        this.OBLSpiltSave = function (Obj, SpiltDetails,SpiltCon, FCL, varSpiltDate) {
            return $http({
                url: "/Import/DSR_CWCImport/AddEditOBLSpilt/",
                method: "POST",
                data: { vm: Obj, lstVM: SpiltDetails,LstContainer:SpiltCon, IsFCL: FCL, SpiltDate: varSpiltDate },
            });
        }

    });
})()