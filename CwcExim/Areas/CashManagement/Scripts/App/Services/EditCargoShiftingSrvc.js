(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('EditCargoShiftingService', function ($http) {
        this.GetShipBillDetails = function (id) {
            return $http.get('/Export/Ppg_CWCExport/GetShipBillDetails/?ShippingLineId=' + id);
        }

        this.GetCargoShiftingInvoice = function (InvoiceId, InvoiceDate, ShippingLineId, TaxType, lstShipbills, PartyId) {
            return $http({
                url: "/Export/Ppg_CWCExport/GetCargoShiftingPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, ShippingLineId: ShippingLineId, TaxType: TaxType, PayeeId: PartyId },
                data: JSON.stringify(lstShipbills)
            });
        }

        this.AddEditCargoShifting = function (InvoiceObj, shipbills, GodownIdF, GodownIdT, ShippingLineIdT) {
            var Indata = { lstShipbills: shipbills, objForm: InvoiceObj };
            return $http({
                url: "/Export/Ppg_CWCExport/AddEditCargoShifting",
                method: "POST",
                params: { FromGodownId: GodownIdF, ToGodownId: GodownIdT, ToShippingId: ShippingLineIdT },
                data: Indata
            });
        }

        this.GetCargoShiftingDetailsInv = function (CargoShiftingId) {
            return $http.get('/CashManagement/Ppg_CashManagement/GetCargoShiftingDetailsInv/?CargoShiftingId=' + CargoShiftingId);
        }

    });
})()