(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('VRN_YardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Import/VRN_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }

        this.GetAppNoForYard = function () {
            return $http.get('/Import/VRN_CWCImport/GetApprForYard');
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/VRN_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners, OTHours, PartyId, PayeeId, isdirect, isMovement, SEZ, ReeferHours) {
            return $http({
                url: "/Import/VRN_CWCImport/GetContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId, isdirect: isdirect, Movement: isMovement, SEZ: SEZ, ReeferHours: ReeferHours },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/VRN_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }

        this.GenerateInvoice = function (InvoiceObj, isdirect, Movement, SEZ, ReeferHours) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            debugger;
            return $http({
                url: "/Import/VRN_CWCImport/AddEditContPaymentSheet/",
                method: "POST",
                params: { IsDirect: isdirect, Movement: Movement, SEZ: SEZ, ReeferHours: ReeferHours },
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


        this.PrintInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/VRN_CWCImport/PrintTentativeYardInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.LoadPartyList = function (Page) {
            return $http.get('/Import/VRN_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }
        this.SearchPartyList = function (PartyCode) {
            return $http.get('/Import/VRN_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
        }
    });
})()