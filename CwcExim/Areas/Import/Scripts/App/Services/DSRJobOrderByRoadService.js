(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLDJobOrderByRoadService', function ($http) {

        this.OnJobOrderSave = function (Obj, FormOneDetails) {
            return $http({
                url: "/Import/DSR_CWCImport/AddEditJobOrderByRoad/",
                method: "POST",
                data: { objImp: Obj, FormOneDetails: FormOneDetails },
            });
        }


        this.GetJobOrderByRoadByOnEditMode = function (ImpJobOrderId) {
            return $http({
                url: "/Import/DSR_CWCImport/GetJobOrderByRoadByOnEditMode/",
                method: "GET",
                params: { ImpJobOrderId:ImpJobOrderId },
            });
        }
    });
})()