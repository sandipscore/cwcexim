(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLD_EditYardInvoiceService', function ($http) {
        this.SelectReqNo = function (ReqNo) {
           return $http.get('/Import/Wfld_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + ReqNo);
        }

        //this.GetAppNoForYard = function () {
        //    return $http.get('/Import/Wfld_CWCImport/GetApprForYard');
        //}

        this.SelectInvoiceNo = function (InvoiceId) {          
            return $http.get('/Import/Wfld_CWCImport/GetInvoiceDetails/?InvoiceId=' + InvoiceId);
        }

        this.SelectReqNoTentative = function (ReqNo) {
            return $http.get('/Import/Wfld_CWCImport/GetPaymentSheetTentativeContainer/?AppraisementId=' + ReqNo);
        }

        this.ContainerSelect = function (InvoiceId, InvoiceDate,DeliveryDate, AppraisementId, TaxType, conatiners, OTHours, PartyId, PayeeId, isdirect, NoOfVehicles, Distance, OwnMovement, InsuredParty,YardBond,SEZ) {
           return $http({
               url: "/Import/Wfld_CWCImport/GetEditContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, DeliveryDate: DeliveryDate, AppraisementId: AppraisementId, TaxType: TaxType, OTHours: OTHours, PartyId: PartyId, PayeeId: PayeeId, isdirect: isdirect, NoOfVehicles: NoOfVehicles, Distance: Distance, OwnMovement: OwnMovement, InsuredParty: InsuredParty, YardToBond: YardBond, SEZ:SEZ },
                data: JSON.stringify(conatiners)
            });
        }

        this.GenerateInvoice = function (InvoiceObj, NoOfVehicle, isdirect, isYardtoBond, SEZ, BillToParty) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            
            return $http({
                url: "/Import/Wfld_CWCImport/UpdateYardPaymentSheet/",
                method: "POST",
                params: { Vehicle: NoOfVehicle, IsDirect: isdirect, IsYardtoBond: isYardtoBond, SEZ: SEZ,BillToParty:BillToParty },
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


        this.PrintInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/Wfld_CWCImport/PrintTentativeYardInvoice/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.LoadPartyList = function (Page)
        {
            debugger;
            return $http.get('/Import/Wfld_CWCImport/LoadPartyListFCL/?Page=' + Page);
        }

        this.LoadPPayeeList = function (Page) {
            debugger;
            return $http.get('/Import/Wfld_CWCImport/LoadPayeeListFCL/?Page=' + Page);
        }

        this.SearchPartyList = function (PartyCode)
        {
            return $http.get('/Import/Wfld_CWCImport/SearchByPartyCodeFCL/?PartyCode=' + PartyCode);
        }



        this.SearchPayeeList = function (PartyCode)
        {
            return $http.get('/Import/Wfld_CWCImport/SearchByPayeeCodeFCL/?PartyCode=' + PartyCode);
        }



        this.SearchCHAList = function (PartyCode,Page) {
            return $http.get('/Import/Wfld_CWCImport/GetCHANameForYardPaymentSheet/?PartyCode=' + PartyCode+'&Page='+Page);
        }
        
        //this.BillToPartyDetails = function (PartyId) {
        //    return $http.get('/Import/Wfld_CWCImport/GetBillToPartyDetails/?PartyId=' + PartyId);
        //}

    });
})()