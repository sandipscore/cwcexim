(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('JobOrderAmendmentService', function ($http) {

        this.OnJobOrderSave = function (Obj, FormOneDetails) {
            return $http({
                    url: "/Import/Dnd_CWCImport/AddEditJobOrderAmendment/",
                method: "POST",
                data: { objImp: Obj, FormOneDetails: FormOneDetails },
            });
        }

        this.GetJobOrderByRoadByOnEditMode = function (ImpJobOrderId) {
            return $http({
                url: "/Import/Dnd_CWCImport/GetJobOrderByRoadByOnEditMode/",
                method: "GET",
                params: { ImpJobOrderId: ImpJobOrderId },
            });
        }
    });
})()