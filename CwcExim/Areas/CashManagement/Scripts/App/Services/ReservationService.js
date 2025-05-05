(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('ReservationService', function ($http) {

        this.GetReservationInvoices = function (m, y,z) {
            return $http({
                url: "/CashManagement/Ppg_CashManagement/GetReservationInvoices/",
                method: "GET",
                params:{month:m,year:y,mode:z}
            });
        }

        this.GenerateInvoice = function (InvoiceObj,m,y) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/CashManagement/Ppg_CashManagement/AddEditReservationInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                params:{month:m,year:y,type:'Tax'},
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
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