(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
     .service('Dnd_ExpDestuffServiceSpl', function ($http) {

         this.GetSBDetails = function (CFSCode) {
             return $http({
                 url: "/Export/Dnd_CWCExport/GetSBDetForExportDestuffingSpl/",
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
                 url: "/Export/Dnd_CWCExport/AddEditExpDestuffingSpl/",
                 method: "POST",
                 data: { objDestuff: Obj, lstDestuffDetail: DestuffDetails },
             });
         }

     });



})()