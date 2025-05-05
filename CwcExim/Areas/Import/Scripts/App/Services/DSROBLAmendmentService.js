(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSROBLAmendmentService', function ($http) {
        
        this.GetOBLDetails = function (OBLNo) {
            return $http({
                url: "/Import/DSR_OblEntry/OBLAmendmentDetail/",
                method: "GET",
                params: { OBLNo: OBLNo},
            });
        }


        this.OBLEntrySave = function (Obj) {
            return $http({
                url: "/Import/DSR_OblEntry/AddEditOBLAmendment/",
                method: "POST",
                data: { obj: Obj },
            });
        }

        //this.GetOBLAmendmentList = function (SearchOBLNo) {
        //    return $http({
        //        url: "/Import/DSR_OblEntry/GetOBLAmendmentList/",
        //        method: "GET",
        //        params: { SearchOBLNo: SearchOBLNo },
        //    });
        //}

    });
})()