(function () {
    angular.module('CWCApp')
        .config(['$httpProvider', function ($httpProvider) {
            $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        }])
        .service('OblEntryUpdateService', function ($http) {

            this.GetOBLDetails = function (OBLNo) {
                return $http({
                    url: "/Import/Ppg_OblEntry/OBLEntryUpdateDetail/",
                    method: "GET",
                    params: { OBLNo: OBLNo },

                });
            }

            this.OBLEntrySave = function (Obj) {
                return $http({
                    url: "/Import/Ppg_OblEntry/AddEditOblEntry_Update/",
                    method: "POST",
                    data: { obj: Obj },
                });
            }

        });
})()