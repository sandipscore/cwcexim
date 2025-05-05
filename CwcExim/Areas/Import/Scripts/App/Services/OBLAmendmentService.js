(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('OBLAmendmentService', function ($http) {
        
        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/Ppg_OblEntry/OBLAmendmentDetail/",
                method: "GET",
                params: { OBLNo: OBLNo},
            });
        }

        this.OBLEntrySave = function (Obj) {
            return $http({
                url: "/Import/Ppg_OblEntry/AddEditOBLAmendment/",
                method: "POST",
                data: { obj: Obj },
            });
        }

    });
})()