(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('EmpContTrnsService', function ($http) {
        this.GetEmptyContTransferCharges = function ( InvoiceDate, InvoiceType, CFSCode, ContainerNo, Size, EntryDate,
               EmptyDate, RefId, PartyId, PayeeId, InvoiceId ) {
          return $http({
              url: "/Import/DSR_CWCImport/CalculateEmptyContTransferInv",
              method: "GET",
              params: {
                  InvoiceDate: InvoiceDate,
                  InvoiceType: InvoiceType,
                  CFSCode: CFSCode,
                  ContainerNo: ContainerNo,
                  Size: Size,
                  EntryDate: EntryDate,
                  EmptyDate: EmptyDate,
                  RefId: RefId,
                  PartyId: PartyId,
                  PayeeId: PayeeId,
                  InvoiceId: InvoiceId
              }
          });
        }

        this.GenerateInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/DSR_CWCImport/AddEditEmtpyTranserPaymentSheet/",
                method: "POST",
                data: { InvoiceObj: JSON.stringify(InvoiceObj) },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.GetEmptyContToShipLineForTransfer = function ( ContainerNo) {
            return $http({
                url: "/Import/DSR_CWCImport/GetEmptyContToShipLineForTransfer",
                method: "GET",
                params: {
                    ContainerNo: ContainerNo,
                }
            });
        }



 /* 
        this.GenerateInvoice = function (InvoiceObj) {
            var Token = $('input[name="__RequestVerificationToken"]').val();

            return $http({
                url: "/Import/DSR_CWCImport/AddEditIRRPaymentSheet/",
                method: "POST",
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }*/

    });
})()