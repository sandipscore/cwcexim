(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSROBLWiseContainerService', function ($http) {
        
        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/DSR_OblEntry/GetOBLWiseContainerDetails/",
                method: "GET",
                params: { OBLNo: OBLNo},
            });
        }
        this.GetContainerList = function (Type) {
            return $http({
                url: "/Import/DSR_OblEntry/GetTContainerNoForOBLEntryByType/",
                method: "GET",
                params: { CONTCBT: Type },
            });
        }
        

        this.OBLEntrySave = function (Obj, OBLEntryDetails) {
            return $http({
                url: "/Import/DSR_OblEntry/AddEditOBLWiseContainerEntry/",
                method: "POST",
                data: { objJO: Obj, lstOBLDetail: OBLEntryDetails },
            });
        }        

    });
})()