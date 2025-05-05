(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('ReeferService', function ($http) {
        this.LoadContainer = function (ReqNo) {
            return $http.get('/Export/WFLD_CWCExport/GetReeferPluginRequest/?InvoiceId=' + ReqNo);
        }

        this.CalculateCharges = function (InvoiceDate, PartyId, TaxType, conatiners, InvoiceId, PayeeId, PayeeName, ExportUnder, SEZ) {
            debugger;
            return $http({
                url: "/Export/WFLD_CWCExport/GetReeferPaymentSheet",
                method: "POST",
                params: {
                    InvoiceDate: InvoiceDate, PartyId: PartyId, InvoiceType: TaxType, InvoiceId: InvoiceId, PayeeId: PayeeId, PayeeName: PayeeName,
                    ExportUnder: ExportUnder, SEZ: SEZ
                },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/Wfld_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }


        this.GetReeferContainerDet = function (LoadContainerDetId) {
            debugger;
            return $http({
                url: "/Export/WFLD_CWCExport/GetContainerDetforReefer",
                method: "POST",
                params: {
                    Id: LoadContainerDetId
                },
               
            });
        }


        this.AddEditReeferInv = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/WFLD_CWCExport/AddEditReeferPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.LoadPayerList = function (PartyCode, PayerPage) {
            debugger;
            return $http.get('/Export/WFLD_CWCExport/LoadPayerList/?PartyCode=' + PartyCode + '&Page=' + PayerPage);
        }
        this.SearchPayerByPayerCode = function (PartyCode, PayerPage) {
            return $http.get('/Export/WFLD_CWCExport/LoadPayerList/?PartyCode=' + PartyCode + '&Page=' + PayerPage);
        }


        this.GetPartyList = function (PartyCode,Page) {
            return $http.get('/Export/WFLD_CWCExport/GetPartyList/?PartyName=' + PartyCode + '&Page=' + Page);
        }

        this.GetExporterList = function (PartyCode, Page) {
            return $http.get('/Export/WFLD_CWCExport/GetExporterList/?ExporterName=' + PartyCode + '&Page=' + Page);
        }
        this.GetReeferContainerList = function () {
            return $http.get('/Export/WFLD_CWCExport/GetReeferPluginRequest');
        }
    });
})()