(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('ChnYardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
            return $http.get('/Import/CHN_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }
          
        this.GetAppNoForYard = function () {
            return $http.get('/Import/CHN_CWCImport/GetApprForYard');
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/CHN_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate, DeliveryDate, AppraisementId, TaxType, conatiners, OTHours, PartyId, PayeeId, isdirect,IsSEZ, NoOfVehicles, Distance, OwnMovement, InsuredParty, YardBond, DirectDelivery,Weighment,DiscountPer,Scanning,FactoryDestuffing, DirectDestuffing) {
            return $http({
                url: "/Import/CHN_CWCImport/GetContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, DeliveryDate: DeliveryDate, SEZ: IsSEZ, AppraisementId: AppraisementId, TaxType: TaxType, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId, isdirect: isdirect, NoOfVehicles: NoOfVehicles, Distance: Distance, OwnMovement: OwnMovement, InsuredParty: InsuredParty, YardToBond: YardBond, DirectDelivery: DirectDelivery, Weighment: Weighment, DiscountPer: DiscountPer, Scanning: Scanning, FactoryDestuffing: FactoryDestuffing, DirectDestuffing: DirectDestuffing },
                data: JSON.stringify(conatiners)
            });
        }




        this.GenerateIRNNo = function (InvoiceNo,SupplyType) {
            return $http({
                url: "/Import/CHN_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },
               
            });
        }
        this.GenerateInvoice = function (InvoiceObj, NoOfVehicle, isdirect,SEZ,parkdays,lockdays) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/CHN_CWCImport/AddEditContPaymentSheet/",
                method: "POST",
                params: { Vehicle: NoOfVehicle, IsDirect: isdirect, SEZ: SEZ, ParkDays: parkdays, LockDays: lockdays },
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


        this.PrintInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/CHN_CWCImport/PrintTentativeYardInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.LoadPartyList = function (Page) {
            debugger;
            return $http.get('/Import/CHN_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }
        this.SearchPartyList = function (PartyCode) {
            return $http.get('/Import/CHN_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
        }

        this.SearchCHAList = function (PartyCode, Page) {
            return $http.get('/Import/CHN_CWCImport/GetCHANameForYardPaymentSheet/?PartyCode=' + PartyCode + '&Page=' + Page);
        }


    });
})()