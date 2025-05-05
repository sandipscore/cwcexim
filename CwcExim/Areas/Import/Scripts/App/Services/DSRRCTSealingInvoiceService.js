(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRRCTSealingInvoiceService', function ($http) {       

   
        this.GetPartyForRCTSealing = function () {
            return $http.get('/Import/DSR_CWCImport/GetPartyForRCTSealing');
        }

        this.GetCHAForRCTSealing = function () {
            debugger;
            return $http.get('/Import/DSR_CWCImport/GetCHAForRCTSealing');
        }
        this.GetExporterForRCTSealing = function () {
            debugger;
            return $http.get('/Import/DSR_CWCImport/GetImporterForRCTSealing');
        }
        this.GetShippingLineForRCTSealing = function () {
            debugger;
            return $http.get('/Import/DSR_CWCImport/GetShippingLineForRCTSealing');
        }
        
       
        this.GetRCTSealingInvoice = function (InvoiceId, InvoiceDate, TaxType, ConatinerDetails, OBLDetails, PartyId, PayeeId,SEZ) {
            debugger;
            if (ConatinerDetails != null)
            {
                var ConatinerDetails = JSON.stringify(ConatinerDetails);
            }
            else {
                var ConatinerDetails = ConatinerDetails;
            }
            
            if (OBLDetails != null) {
                var OBLDetails = JSON.stringify(OBLDetails);
            }
            else {
                var OBLDetails = OBLDetails;
            }

            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Import/DSR_CWCImport/GetRCTSealingInvoice/?InvoiceId=" + InvoiceId,
                method: "POST",                
                data: { InvoiceId: InvoiceId, InvoiceDate: InvoiceDate, TaxType: TaxType, ConatinerDetails: ConatinerDetails, OBLDetails: OBLDetails, PartyId: PartyId, PayeeId: PayeeId, SEZ: SEZ },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.GenerateRCTInvoice = function (InvoiceObj, SEZ) {
            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/DSR_CWCImport/AddRCTSealingPaymentSheet/",
                method: "POST",

                params: { SEZ: SEZ },

                data: JSON.stringify(InvoiceObj),
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