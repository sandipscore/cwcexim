(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('CargoShiftingService', function ($http) {
        this.GetShipBillDetails = function (id, ShiftingType, GodownId) {
            //return $http.get('/Export/Ppg_CWCExport/GetShipBillDetails/?ShippingLineId=' + id);
            return $http({
                url: "/Export/Ppg_CWCExportV2/GetShipBillDetails/?ShippingLineId=" + id,
                method: "POST",
                params: { ShiftingType: ShiftingType, GodownId: GodownId }
            });
        }

        this.GetShippingLineforCargo = function () {
            return $http.get('/Export/Ppg_CWCExportV2/GetShippingForCargo');
        }

        this.GetShippingLineLazyLoad = function (Page, PartyCode)
        {
            return $http.get('/Export/Ppg_CWCExportV2/Eximtraderlist?Page=' + Page + '&PartyCode=' + PartyCode + '&ShippingLine=1');
        }

        this.GetPartyforCargo = function () {
            return $http.get('/Export/Ppg_CWCExportV2/GetPartyForCargo');
        }

        this.GetGodownforCargo = function () {
            return $http.get('/Export/Ppg_CWCExportV2/GetFromGodownForCargo');
        }

        this.GetGodownforToCargo = function () {
            return $http.get('/Export/Ppg_CWCExportV2/GetToGodownForCargo');
        }
        this.GetCargoShiftingInvoice = function (InvoiceId, InvoiceDate, ShippingLineId, TaxType, lstShipbills, PartyId) {
            var shipbills = lstShipbills.filter(x=>x.IsChecked == true);
            return $http({
                url: "/Export/Ppg_CWCExportV2/GetCargoShiftingPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, ShippingLineId: ShippingLineId, TaxType: TaxType, PayeeId: PartyId },
                data: JSON.stringify(shipbills)
            });
        }

        this.AddEditCargoShifting = function (InvoiceObj, shipbills, GodownIdF, GodownIdT, ShippingLineIdT, ShiftingType, ShippingLineIdF, ShiftingDate, Remarks, CargoShiftingId) {
            var sbFiltered = shipbills.filter(x=>x.IsChecked == true);
            //var Indata = { lstShipbills: sbFiltered, objForm: InvoiceObj };
            return $http({
                url: "/Export/Ppg_CWCExportV2/AddEditCargoShifting",
                method: "POST",
                params: {
                    FromGodownId: GodownIdF,
                    ToGodownId: GodownIdT,
                    ToShippingId: ShippingLineIdT,
                    ShiftingType: ShiftingType,
                    FromShippingLineId: ShippingLineIdF,
                    ShiftingDate: ShiftingDate,
                    Remarks: Remarks,
                    CargoShiftingId: CargoShiftingId
                },
                data: sbFiltered
            });
        }

    });
})()