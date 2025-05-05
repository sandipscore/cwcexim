(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
     .service('Ppg_ExpDestuffService', function ($http) {

         this.GetSBDetails = function (CFSCode,OperationType) {
             return $http({
                 url: "/Export/Ppg_CWCExportV2/GetSBDetForExportDestuffing/",
                 method: "GET",
                 params: { CFSCode: CFSCode, OperationType: OperationType },
             });
         }


         this.GetLocation = function (GodownId) {
             return $http({
                 url: "/Export/Ppg_CWCExportV2/GetLocationDetailsByGodownId/",
                 method: "GET",
                 params: { GodownId: GodownId }, 
             });
         }

         this.DestuffingEntrySave = function (Obj, DestuffDetails) {
             return $http({
                 url: "/Export/Ppg_CWCExportV2/AddEditExpDestuffing/",
                 method: "POST",
                 data: { objDestuff: Obj, lstDestuffDetail: DestuffDetails },
             });
         }

     });



})()