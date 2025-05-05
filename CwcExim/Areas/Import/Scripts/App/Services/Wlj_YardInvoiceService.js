(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('Wlj_YardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Import/Wlj_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }

        this.GetAppNoForYard = function () {
            return $http.get('/Import/Wlj_CWCImport/GetApprForYard');
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/Wlj_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType,SEZ, conatiners, OTHours, PartyId, PayeeId, isdirect) {
            return $http({
                url: "/Import/Wlj_CWCImport/GetContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType,SEZ: SEZ, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId, isdirect: isdirect },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj, isdirect, SEZ) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            debugger;
            return $http({
                url: "/Import/Wlj_CWCImport/AddEditContPaymentSheet/",
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
                url: "/Import/Wlj_CWCImport/PrintTentativeYardInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/Wlj_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }



        this.LoadPartyList = function (Page) {
            return $http.get('/Import/Wlj_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }
        this.SearchPartyList = function (PartyCode) {
            return $http.get('/Import/Wlj_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
        }
    });

})()