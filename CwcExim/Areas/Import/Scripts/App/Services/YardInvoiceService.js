(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('YardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
           return $http.get('/Import/Ppg_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }

        this.GetAppNoForYard = function () {
            return $http.get('/Import/Ppg_CWCImport/GetApprForYard');
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/Ppg_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners, OTHours, PartyId, PayeeId, isdirect,SEZ) {
           return $http({
                url: "/Import/Ppg_CWCImport/GetContainerPaymentSheet/?InvoiceId="+InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId, isdirect: isdirect, SEZ: SEZ },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj, isdirect, SEZ) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            
            return $http({
                url: "/Import/Ppg_CWCImport/AddEditContPaymentSheet/",
                method: "POST",
                params: { IsDirect: isdirect, SEZ: SEZ },
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


        this.PrintInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Report/Ppg_ReportCWCV2/GetBulkInvoiceReport/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.LoadPartyList = function (Page)
        {
            return $http.get('/Import/Ppg_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }
        this.SearchPartyList = function (PartyCode)
        {
            return $http.get('/Import/Ppg_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
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