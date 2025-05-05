(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRRCTSealingInvoiceService', function ($http) {       

   
        this.GetPartyForRCTSealing = function () {
            return $http.get('/Export/DSR_CWCExport/GetPartyForRCTSealing');
        }

        this.GetCHAForRCTSealing = function () {
            debugger;
            return $http.get('/Export/DSR_CWCExport/GetCHAForRCTSealing');
        }
        this.GetExporterForRCTSealing = function () {
            debugger;
            return $http.get('/Export/DSR_CWCExport/GetExporterForRCTSealing');
        }
        this.GetShippingLineForRCTSealing = function () {
            debugger;
            return $http.get('/Export/DSR_CWCExport/GetShippingLineForRCTSealing');
        }
        
        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/DSR_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }       

        this.GetRCTSealingInvoice = function (InvoiceId, InvoiceDate, TaxType, ConatinerDetails, ShipbillDetails, PartyId, PayeeId, ExportUnder) {
            debugger;
            if (ConatinerDetails != null)
            {
                var ConatinerDetails = JSON.stringify(ConatinerDetails);
            }
            else {
                var ConatinerDetails = ConatinerDetails;
            }
            
            if (ShipbillDetails != null) {
                var ShipbillDetails = JSON.stringify(ShipbillDetails);
            }
            else {
                var ShipbillDetails = ShipbillDetails;
            }

            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/DSR_CWCExport/GetRCTSealingInvoice/?InvoiceId=" + InvoiceId,
                method: "POST",                
                data: { InvoiceId: InvoiceId, InvoiceDate: InvoiceDate, TaxType: TaxType, ConatinerDetails: ConatinerDetails, ShipbillDetails: ShipbillDetails, PartyId: PartyId, PayeeId: PayeeId, ExportUnder: ExportUnder },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.GenerateRCTInvoice = function (InvoiceObj) {
            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Export/DSR_CWCExport/AddRCTSealingPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

    });
})()