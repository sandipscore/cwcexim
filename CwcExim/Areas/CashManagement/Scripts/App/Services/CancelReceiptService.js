(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('CancelReceiptService', function ($http) {
        this.GetReceiptDetails = function (ReceiptId) {
            return $http.get('/CashManagement/Ppg_CashManagement/GetReceiptDetails?ReceiptId=' + ReceiptId);
        }
        this.SubmitReceiptDetails = function (ReceiptObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/CashManagement/Ppg_CashManagement/UpdateCancelReceipt",
                method: "POST",
                data: JSON.stringify(ReceiptObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()