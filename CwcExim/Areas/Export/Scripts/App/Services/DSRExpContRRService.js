(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('ExpContRRService', function ($http) {
        this.GetInvoiceDetails = function (InvoiceId) {
            return $http.get('/Export/DSR_CWCExport/GetSelectedInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.GetPostInvoiceDetails = function (InvoiceId, ShippingLineId) {
            return $http.get('/Export/DSR_CWCExport/GetExportRRPaymentSheet/?InvoiceId=' + InvoiceId + '&ShippingLineId=' + ShippingLineId);
        }
        

        this.AddEditExportRR = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/DSR_CWCExport/AddEditExportRRCreditDebitModule/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

    });
})()