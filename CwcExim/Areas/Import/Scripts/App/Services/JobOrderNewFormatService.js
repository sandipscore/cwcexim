(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('JobOrderNewFormatService', function ($http) {

        this.OnJobOrderSave = function (Obj, FormOneDetails) {
            return $http({
                url: "/Import/Ppg_CWCImport/AddEditJobOrderNew/",
                method: "POST",
                data: { objImp: Obj, FormOneDetails: FormOneDetails },
            });
        }

        this.GetJobOrderByRoadByOnEditMode = function (ImpJobOrderId) {
            debugger;
            return $http({
                url: "/Import/Ppg_CWCImport/GetTrainDetailsOnEditModeNewFormat/",
                method: "GET",
                params: { ImpJobOrderId: ImpJobOrderId },
            });
        }

        this.GetContainerNo = function () {
            return $http({
                url: "/Import/Ppg_CWCImport/GetAllContainerNo/",
                method: "GET",
               
            });
        }

        this.GetContainerNoDetails = function (ContainerNo, TrainSummaryID) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetContainerNoDetails/",
                method: "GET",
                params: { ContainerNo: ContainerNo, TrainSummaryID: TrainSummaryID },
            });
        }

       

    });
})()