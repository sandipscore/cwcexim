(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('JobOrderService', function ($http) {
        this.GetPort = function () {
            return $http({
                url: "/Import/Ppg_CWCImport/GetPort/",
                method: "GET",
            });
        }
        this.GetTrainDetails = function (TSumId) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetTrainDetl/",
                method: "GET",
                params: { TrainSumId: TSumId },
            });
        }

        this.OnJobOrderSave = function (Obj, FormOneDetails) {
            return $http({
                url: "/Import/Ppg_CWCImport/AddEditJobOrder/",
                method: "POST",
                data: { objImp: Obj, FormOneDetails: FormOneDetails },
            });
        }

        this.GetTrainDetailsOnEditMode = function (ImpJobOrderId) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetTrainDetailsOnEditMode/",
                method: "GET",
                params: { ImpJobOrderId:ImpJobOrderId },
            });
        }
    });
})()