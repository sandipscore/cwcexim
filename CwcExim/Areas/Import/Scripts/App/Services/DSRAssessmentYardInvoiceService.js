(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRYardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Import/DSR_CWCImport/GetReassessmentPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }

        this.BindAmendment = function (ReqId) {
            return $http.get('/Import/DSR_CWCImport/GetCheckOBLAmendment/?AppraisementId=' + ReqId);
        }

        this.GetAppNoForYard = function () {
            return $http.get('/Import/DSR_CWCImport/GetApprForYard');
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/DSR_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners, OTHours, PartyId, PayeeId, isdirect,
           NoOfVehicles, Distance, PrivateMovement, InsuredParty, CWCMovement, Amendment, PortId, MovementType, ExamType,SEZ) {
            debugger;
            return $http({
                url: "/Import/DSR_CWCImport/GetContainerReassessmentPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: {
                    InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType,MovementType:MovementType,ExamType:ExamType, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId,
                    isdirect: isdirect, NoOfVehicles: NoOfVehicles, Distance: Distance, PrivateMovement: PrivateMovement,
                    InsuredParty: InsuredParty, CWCMovement: CWCMovement, Amendment: Amendment, ReturnPort: PortId, SEZ: SEZ
                },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj, isdirect, MovementType, conatiners, EmptyPort, ExamType, SEZ, Amendment, InsuredParty) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            debugger;
            return $http({
                url: "/Import/DSR_CWCImport/AddEditYardReassessmentPaymentSheet/",
                method: "POST",
                params: { IsDirect: isdirect, MovementType: MovementType, EmptyPort: EmptyPort, ExamType: ExamType, SEZ: SEZ, Amendment: Amendment, InsuredParty: InsuredParty },
              
                data: { InvoiceObj: JSON.stringify(InvoiceObj), lstPaySheetContainer:conatiners },
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

        this.PrintInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/DSR_CWCImport/PrintTentativeYardInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.LoadCHAList = function (Page) {
            debugger;
            return $http.get('/Import/DSR_CWCImport/LoadYardCHAList/?Page=' + Page);
        }
        this.SearchCHAList = function (PartyCode) {
            return $http.get('/Import/DSR_CWCImport/SearchByPartyCodeYardCHA/?PartyCode=' + PartyCode);
        }
        this.LoadPartyList = function (Page)
        {
            debugger;
            return $http.get('/Import/DSR_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }
        this.SearchPartyList = function (PartyCode)
        {
            return $http.get('/Import/DSR_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
        }
    });
})()