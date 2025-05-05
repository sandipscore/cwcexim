(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRReservationService', function ($http) {

        this.GetReservationInvoices = function (InvoiceObj,m, y, z) {
            return $http({
                url: "/CashManagement/DSR_CashManagement/GetReservationInvoices/",
                method: "GET",
                data: JSON.stringify(InvoiceObj),
                params:{month:m,year:y,mode:z}
            });
        }

        this.CreateResInvoices = function (m, y,pid, z,invoicedate,SEZ) {
            return $http({
                url: "/CashManagement/DSR_CashManagement/CreateReservationInvoices/",
                method: "GET",
                params: { month: m, year: y, pid: pid, mode: z,invoicedate:invoicedate,SEZ:SEZ }
            });
        }

        this.GetReservationParties = function (m, y) {
            return $http({
                url: "/CashManagement/DSR_CashManagement/GetReservationParties/",
                method: "GET",
                params: { month: m, year: y }
            });
        }

        this.SaveGenerateInvoice = function (InvoiceObj, m, y,SEZ) {
            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/CashManagement/DSR_CashManagement/AddEditReservationInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                params: { month: m, year: y, type: 'Tax', SEZ: SEZ },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/DSR_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }


    });
})()