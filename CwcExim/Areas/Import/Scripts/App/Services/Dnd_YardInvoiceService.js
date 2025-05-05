(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('Dnd_YardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
           return $http.get('/Import/Dnd_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }

        this.GetAppNoForYard = function () {
            return $http.get('/Import/Dnd_CWCImport/GetApprForYard');
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/Dnd_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners, OTHours, PartyId, PayeeId, isdirect) {
           return $http({
                url: "/Import/Dnd_CWCImport/GetContainerPaymentSheet/?InvoiceId="+InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId, isdirect: isdirect },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj,isdirect) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            
            return $http({
                url: "/Import/Dnd_CWCImport/AddEditContPaymentSheet/",
                method: "POST",
                params:{IsDirect:isdirect},
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


        this.PrintInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/Dnd_CWCImport/PrintTentativeYardInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.LoadPartyList = function (Page)
        {
            return $http.get('/Import/Dnd_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }
        this.SearchPartyList = function (PartyCode)
        {
            return $http.get('/Import/Dnd_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
        }
    });
})()