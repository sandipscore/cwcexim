(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
     .service('Dnd_ExpDestuffService', function ($http) {

         this.GetSBDetails = function (CFSCode) {
             return $http({
                 url: "/Export/Dnd_CWCExport/GetSBDetForExportDestuffing/",
                 method: "GET",
                 params: { CFSCode: CFSCode },
             });
         }


         this.GetLocation = function (GodownId) {
             return $http({
                 url: "/Export/Dnd_CWCExport/GetLocationDetailsByGodownId/",
                 method: "GET",
                 params: { GodownId: GodownId },
             });
         }

         this.DestuffingEntrySave = function (Obj, DestuffDetails) {
             return $http({
                 url: "/Export/Dnd_CWCExport/AddEditExpDestuffing/",
                 method: "POST",
                 data: { objDestuff: Obj, lstDestuffDetail: DestuffDetails },
             });
         }

     });



})()