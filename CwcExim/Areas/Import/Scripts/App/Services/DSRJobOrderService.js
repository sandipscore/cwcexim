(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRJobOrderService', function ($http) {
        this.GetPort = function () {
            return $http({
                url: "/Import/DSR_CWCImport/GetPort/",
                method: "GET",
            });
        }

        this.GetTrainDetails = function (TSumId) {
            return $http({
                url: "/Import/DSR_CWCImport/GetTrainDetl/",
                method: "GET",
                params: { TrainSumId: TSumId },
            });
        }

        this.OnJobOrderSave = function (Obj, InvoiceType, FormOneDetails, ContainerOneDetails,SEZ) {
            return $http({
                url: "/Import/DSR_CWCImport/AddEditJobOrder/",
                method: "POST",
                data: { objImp: Obj, InvoiceType: InvoiceType, FormOneDetails: FormOneDetails, ConDetails: ContainerOneDetails,SEZ:SEZ },
            });
        }

        this.PrintInvoiceService = function (InvoiceType, FormOneDetails,SEZ) {
            return $http({
                url: "/Import/DSR_CWCImport/PrintInvoiceService/",
                method: "POST",
                data: { InvoiceType: InvoiceType, FormOneDetails: FormOneDetails,SEZ:SEZ },
            });
        }
        this.GetTrainDetailsOnEditMode = function (ImpJobOrderId) {
            return $http({
                url: "/Import/DSR_CWCImport/GetTrainDetailsOnEditMode/",
                method: "GET",
                params: { ImpJobOrderId: ImpJobOrderId },
            });
        }

    });
})()