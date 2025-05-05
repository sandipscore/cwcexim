(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLDOBLAmendmentService', function ($http) {
        
        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/Wfld_OblEntry/OBLAmendmentDetail/",
                method: "GET",
                params: { OBLNo: OBLNo},
            });
        }


        this.OBLEntrySave = function (Obj) {
            return $http({
                url: "/Import/Wfld_OblEntry/AddEditOBLAmendment/",
                method: "POST",
                data: { obj: Obj },
            });
        }

    });
})()