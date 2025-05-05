(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('IrrService', function ($http) {
        this.GetIrrCharges = function (invoicedt, cfscode, taxtype, CargoType, invoiceid) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetIRRPaymentSheet/",
                method: "POST",
                params: { InvoiceDate: invoicedt, CFSCode: cfscode, TaxType: taxtype,CargoType:CargoType, InvoiceId: invoiceid }
            });
        }

        this.GetContainerDetails = function (trainSummaryId) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetContainersForIRR/",
                method: "GET",
                params: { TrainSummaryId: trainSummaryId }
            });
        }


        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }
    

        this.GenerateInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/Ppg_CWCImport/AddEditIRRPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

    });
})()