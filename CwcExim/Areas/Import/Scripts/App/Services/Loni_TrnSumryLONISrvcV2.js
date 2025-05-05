(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('TrainSummaryLONIService', function ($http) {
        this.PopulatePortList = function () {
            return $http.get('/Import/Loni_CWCImportV2/GetPortList');
        }
        this.LoadMoreShippingList = function (Page, PartyCode) {
            return $http.get('/Import/Loni_CWCImportV2/TrainSummryPayeeListLONI?Page=' + Page + '&PartyCode=' + PartyCode);
        }
        this.SaveInvoice = function (InvoiceObj, SEZ) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Import/Loni_CWCImportV2/SaveUploadDataLONI/",
                method: "POST",
                params: { SEZ: SEZ },
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        /*this.LoadPayerList = function (PayerPage) {
            return $http.get('/Export/Hdb_CWCExport/LoadPayerList/?PartyCode=&Page=' + PayerPage);
        }
        this.SearchPayerByPayerCode = function (PartyCode) {
            return $http.get('/Export/Hdb_CWCExport/SearchPayerNameByPayeeCode/?PartyCode=' + PartyCode);
        }*/
        this.ViewTrainSummary = function (TrainSummaryUploadId) {
            return $http.get('/Import/Loni_CWCImportV2/GetTrainSummaryDetailsLONI/?TrainSummaryUploadId=' + TrainSummaryUploadId);
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/Loni_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }

    });
})()