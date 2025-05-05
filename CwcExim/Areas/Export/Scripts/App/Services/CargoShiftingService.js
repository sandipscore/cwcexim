(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('CargoShiftingService', function ($http) {
        this.GetShipBillDetails = function (id, ShiftingType, GodownId) {
            //return $http.get('/Export/Ppg_CWCExport/GetShipBillDetails/?ShippingLineId=' + id);
            return $http({
                url: "/Export/Ppg_CWCExport/GetShipBillDetails/?ShippingLineId=" + id,
                method: "POST",
                params: { ShiftingType: ShiftingType,GodownId: GodownId }
            });
        }

        this.GetShippingLineforCargo = function () {
            return $http.get('/Export/Ppg_CWCExport/GetShippingForCargo');
        }

        this.GetPartyforCargo = function () {
            return $http.get('/Export/Ppg_CWCExport/GetPartyForCargo');
        }

        this.GetGodownforCargo = function () {
            return $http.get('/Export/Ppg_CWCExport/GetFromGodownForCargo');
        }
        
        this.GetGodownforToCargo = function () {
            return $http.get('/Export/Ppg_CWCExport/GetToGodownForCargo');
        }
        this.GetCargoShiftingInvoice = function (InvoiceId, InvoiceDate, ShippingLineId, TaxType, lstShipbills, PartyId) {
            debugger;
            var shipbills = lstShipbills.filter(x=>x.IsChecked == true);
            return $http({
                url: "/Export/Ppg_CWCExport/GetCargoShiftingPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, ShippingLineId: PartyId, TaxType: TaxType, PayeeId: PartyId },
                data: JSON.stringify(shipbills)
            });
        }

        this.AddEditCargoShifting = function (InvoiceObj, shipbills, GodownIdF, GodownIdT, ShippingLineIdT, ShiftingType, ShippingLineIdF)
        {
            var sbFiltered = shipbills.filter(x=>x.IsChecked == true);
            var Indata = { lstShipbills: sbFiltered, objForm: InvoiceObj };
            return $http({
                url: "/Export/Ppg_CWCExport/AddEditCargoShifting",
                method: "POST",
                params: { FromGodownId: GodownIdF, ToGodownId: GodownIdT, ToShippingId: ShippingLineIdT, ShiftingType: ShiftingType, FromShippingLineId:ShippingLineIdF },
                data: Indata
            });
        }

    });
})()