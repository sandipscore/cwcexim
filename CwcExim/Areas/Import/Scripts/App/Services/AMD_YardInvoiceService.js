(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('AMD_YardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Import/AMD_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }

        this.GetAppNoForYard = function () {
            return $http.get('/Import/AMD_CWCImport/GetApprForYard');
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/AMD_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners, OTHours, PartyId, PayeeId, isdirect, isMovement, SEZ, CustomExam, ReeferHours, Distance, IsFranchise, IsOnWheel, IsReworking, IsCargoShifting, IsLiftOnOff, IsSweeping, IsHandling) {
            return $http({
                url: "/Import/AMD_CWCImport/GetContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId, isdirect: isdirect, Movement: isMovement, SEZ: SEZ, CustomExam: CustomExam, ReeferHours: ReeferHours, Distance: Distance, IsFranchise: IsFranchise, IsOnWheel: IsOnWheel, IsReworking: IsReworking, IsCargoShifting: IsCargoShifting, IsLiftOnOff: IsLiftOnOff, IsSweeping: IsSweeping, IsHandling: IsHandling },
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

        this.GenerateInvoice = function (InvoiceObj, isdirect, Movement, SEZ, CustomExam, ReeferHours, Distance, IsFranchise, IsOnWheel, IsReworking, IsCargoShifting, IsLiftOnOff, IsSweeping, IsHandling) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            debugger;
            return $http({
                url: "/Import/AMD_CWCImport/AddEditContPaymentSheet/",
                method: "POST",
                params: { IsDirect: isdirect, Movement: Movement, SEZ: SEZ, CustomExam: CustomExam, ReeferHours: ReeferHours, Distance: Distance, IsFranchise: IsFranchise, IsOnWheel: IsOnWheel, IsReworking: IsReworking, IsCargoShifting: IsCargoShifting, IsLiftOnOff: IsLiftOnOff, IsSweeping: IsSweeping, IsHandling: IsHandling },
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
            return $http.get('/Import/AMD_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }
        this.SearchPartyList = function (PartyCode) {
            return $http.get('/Import/AMD_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
        }
    });
})()