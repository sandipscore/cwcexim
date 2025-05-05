(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('TrainSummaryLONIService', function ($http) {
               
        this.SaveTrainSummary = function (InvoiceObj) {
            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Import/LNSM_CWCImport/SaveUploadDataLONISAMO/",
                method: "POST",                
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        
        this.ViewTrainSummary = function (TrainSummaryUploadId) {
            return $http.get('/Import/LNSM_CWCImport/GetTrainSummaryDetailsLONISAMO/?TrainSummaryUploadId=' + TrainSummaryUploadId);
        }

    });
})()