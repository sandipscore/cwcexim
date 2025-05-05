(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('VLDAYardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Import/VLDA_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }

        this.GetAppNoForYard = function () {
            return $http.get('/Import/VLDA_CWCImport/GetApprForYard');
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/VLDA_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, DeliveryDate, AppraisementId, TaxType, conatiners, OTHours, PartyId, PayeeId, isdirect, IsOpenYard, Isdestuffed, NoOfVehicles, Distance, OwnMovement, InsuredParty, YardBond, SEZ) {
            return $http({
                url: "/Import/VLDA_CWCImport/GetContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, DeliveryDate: DeliveryDate, AppraisementId: AppraisementId, TaxType: TaxType, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId, isdirect: isdirect, IsOpenYard:IsOpenYard, Isdestuffed:Isdestuffed, NoOfVehicles: NoOfVehicles, Distance: Distance, OwnMovement: OwnMovement, InsuredParty: InsuredParty, YardToBond: YardBond, SEZ: SEZ },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj, NoOfVehicle, isdirect, isYardtoBond, SEZ, BillToParty, Distance, OTHours, IsTransporter, IsInsured, IsOpenYard, Isdestuffed) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            debugger;
            return $http({
                url: "/Import/VLDA_CWCImport/AddEditContPaymentSheet/",
                method: "POST",
                params: { Vehicle: NoOfVehicle, IsDirect: isdirect, IsYardtoBond: isYardtoBond, SEZ: SEZ, BillToParty: BillToParty, Distance: Distance, OTHours: OTHours, IsTransporter: IsTransporter, IsInsured: IsInsured, IsOpenYard: IsOpenYard, Isdestuffed: Isdestuffed },
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


        this.PrintInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/VLDA_CWCImport/PrintTentativeYardInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.LoadPartyList = function (Page) {
            debugger;
            return $http.get('/Import/VLDA_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }

        this.LoadPPayeeList = function (Page) {
            debugger;
            return $http.get('/Import/VLDA_CWCImport/LoadPayeeListFCL/?Page=' + Page);
        }

        this.SearchPartyList = function (PartyCode) {
            return $http.get('/Import/VLDA_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
        }



        this.SearchPayeeList = function (PartyCode) {
            return $http.get('/Import/VLDA_CWCImport/SearchByPayeeCodeFCL/?PartyCode=' + PartyCode);
        }



        this.SearchCHAList = function (PartyCode, Page) {
            return $http.get('/Import/VLDA_CWCImport/GetCHANameForYardPaymentSheet/?PartyCode=' + PartyCode + '&Page=' + Page);
        }


        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/VLDA_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }
        //this.BillToPartyDetails = function (PartyId) {
        //    return $http.get('/Import/VLDA_CWCImport/GetBillToPartyDetails/?PartyId=' + PartyId);
        //}

    });
})()