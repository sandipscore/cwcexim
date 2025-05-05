(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgAuctionInvoiceEditService', function ($http) {
        this.GetDeliInvoiceDetails = function (InvoiceId) {
            return $http.get('/Auction/Ppg_AuctionInvoice/GetAucInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (BidId, InvoiceId, InvoiceDate, FreeUpTo, HSNCode, CustomDuty, OT, ValuersCharge, AuctionCharge, MISC) {

            return $http({
                url: "/Auction/Ppg_AuctionInvoice/GetAuctionChargesforEdit",
                method: "POST",
                params: { BidId: BidId, InvoiceId: InvoiceId, InvoiceDate: InvoiceDate, FreeUpTo: FreeUpTo, HSNCode: HSNCode, CustomDuty: CustomDuty, OT: OT, ValuersCharge: ValuersCharge, AuctionCharge: AuctionCharge, MISC: MISC },
               
            });
        }
         this.GetAppNoForYard = function (Module) {


            return $http({
                url: "/Import/Ppg_CWCImport/GetInvoiceForEdit",
                method: "GET",
                params: { Module: Module }
            });
           
        }
        this.GenerateInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Auction/Ppg_AuctionInvoice/AddEditInvoiceforEdit/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()