(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLDReservationService', function ($http) {

        this.GetReservationInvoices = function (InvoiceObj, m, y, z) {
            return $http({
                url: "/CashManagement/WFLD_CashManagement/GetReservationInvoices/",
                method: "GET",
                data: JSON.stringify(InvoiceObj),
                params: { month: m, year: y, mode: z }
            });
        }

        this.CreateResInvoices = function (m, y, pid, z) {
            return $http({
                url: "/CashManagement/WFLD_CashManagement/CreateReservationInvoices/",
                method: "GET",
                params: { month: m, year: y, pid: pid, mode: z }
            });
        }
        this.GetReservationParties = function (m, y) {
            return $http({
                url: "/CashManagement/WFLD_CashManagement/GetReservationParties/",
                method: "GET",
                params: { month: m, year: y }
            });
        }

        this.SaveGenerateInvoice = function (InvoiceObj, m, y) {
            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/CashManagement/WFLD_CashManagement/AddEditReservationInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                params: { month: m, year: y, type: 'Tax' },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()