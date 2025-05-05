(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('OBLSpiltService', function ($http) {

        this.GetOBLList = function () {
            return $http({
                url: "/Import/Ppg_CWCImportV2/GetOBLListForSpilt/",
                method: "GET"               
            });
        }

        this.GetOBLDetails = function (varOBL, vaOBLDate, FCL) {
            debugger;
            return $http({
                url: "/Import/Ppg_CWCImportV2/GetOBLDetailsForSpilt/",
                method: "GET",
                params: { OBL: varOBL, OBLDate: vaOBLDate, isFCL: FCL },
            });
        }

        this.OBLSpiltSave = function (Obj, SpiltDetails, FCL, varSpiltDate) {
            return $http({
                url: "/Import/Ppg_CWCImportV2/AddEditOBLSpilt/",
                method: "POST",
                data: { vm: Obj, lstVM: SpiltDetails, IsFCL: FCL, SpiltDate: varSpiltDate },
            });
        }

      
    });
})()