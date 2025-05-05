(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgContMoveInvoiceEditService', function ($http) {
        this.GetYardInvoiceDetails = function (InvoiceId) {
            return $http.get('/Export/Ppg_CWCExportV2/GetContainerMovementInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners) {
            return $http({
                url: "/Export/Ppg_CWCExportV2/GetContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType },
                data: JSON.stringify(conatiners)
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

            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/Ppg_CWCExportV2/EditContMovementPaymentSheet",
                method: "POST",
                data:  JSON.stringify(InvoiceObj) ,
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        /*
        url: '/Export/Ppg_CWCExport/GetInternalPaymentSheet',
            type: 'POST',
            data: {
                ContainerStuffingDtlId: $('#ContainerStuffingDtlId').val(),
                ContainerStuffingId: $('#ContainerStuffingId').val(),
                ContainerNo: $('#Container').val(),
                MovementDate: $('#MovementDate').val(),
                InvoiceType: 'Tax',
                DestLocationIdiceId: $('#LocationId').val(),
                Partyid: pid,
                ctype: chargetype,
                portvalue: $('#port').val() == "" ? 0 : $('#port').val(),
                InvoiceId: 0

        */

        this.GetInvoiceCharges = function (ContainerStuffingDtlId, ContainerStuffingId, ContainerNo, MovementDate, InvoiceType, DestLocationIdiceId, Partyid, chargetype, portvalue, TareWeight,CargoType,PayeeId,InvoiceId) {
            return $http({
                url: "/Export/Ppg_CWCExportV2/GetInternalPaymentSheet",
                method: "POST",
                params: {
                    ContainerStuffingDtlId: ContainerStuffingDtlId,
                    ContainerStuffingId: ContainerStuffingId,
                    ContainerNo: ContainerNo,
                    MovementDate: MovementDate,
                    InvoiceType: InvoiceType,
                    DestLocationIdiceId: DestLocationIdiceId,
                    Partyid: Partyid,
                    ctype: chargetype,
                    portvalue: portvalue,
                    tareweight: TareWeight,
                    cargotype: CargoType,
                    PayeeId:PayeeId,
                    InvoiceId: InvoiceId
                }
            });
        }


    });
})()