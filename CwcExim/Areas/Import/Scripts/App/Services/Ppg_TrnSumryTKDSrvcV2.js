(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('TrainSummaryTKDService', function ($http) {
        this.PopulatePortList = function () {
            return $http.get('/Import/Ppg_CWCImportV2/GetPortList');
        }
        this.LoadMoreShippingList = function (Page, PartyCode) {
            return $http.get('/Import/Ppg_CWCImportV2/TrainSummryPayeeList?Page=' + Page + '&PartyCode=' + PartyCode);
        }
        this.SaveInvoice = function (InvoiceObj,SEZ)
        {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Import/Ppg_CWCImportV2/SaveUploadData/",
                method: "POST",
                params:{SEZ:SEZ},
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
        this.ViewTrainSummary = function (TrainSummaryUploadId)
        {
            return $http.get('/Import/Ppg_CWCImportV2/GetTrainSummaryDetailsTKD/?TrainSummaryUploadId=' + TrainSummaryUploadId);
        }

         this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/Ppg_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }

    });
})()