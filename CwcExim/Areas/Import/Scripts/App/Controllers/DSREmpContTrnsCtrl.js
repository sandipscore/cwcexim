(function () {
    angular.module('CWCApp').
    controller('EmpContTrnsCtrl', function ($scope, EmpContTrnsService) {
        $scope.ContainersList = [];
        $scope.ShippingLine = [];
        $scope.Message = '';
        $scope.InvoiceDate = $('#InvoiceDate').val();
        $scope.Invoiceobj = {};
        $scope.c = 0;
        $scope.a = 0;
        $scope.Gener = 0;
        if ($('#hdnContainersList').val() != '')
            $scope.ContainersList = JSON.parse($('#hdnContainersList').val());
        else $scope.ContainersList = [];
        if ($('#hdnShippingLine').val() != '')
            $scope.ShippingLine = JSON.parse($('#hdnShippingLine').val());
        else $scope.ShippingLine = [];
        
        $scope.SelectContainer = function (item) {
            debugger;
            $scope.ToShippingLineId = '';
            $scope.ToShippingLineName = '';
            //$('#InvoiceDate').parent().find('img').css('display', 'block');
            EmpContTrnsService.GetEmptyContToShipLineForTransfer(item.ContainerNo).then(function (res) {
                debugger;
                if (res.data.length > 0) {
                    $scope.FromShippingLineId = res.data[0].ToShippingLineId;
                    $scope.FromShippingLineName = res.data[0].ToShippingLineName;
                    $scope.PayeeId = res.data[0].ToShippingLineId;
                    $scope.PayeeName = res.data[0].ToShippingLineName;
                }
                else {
                    $scope.FromShippingLineId = item.ShippingLineId;
                    $scope.FromShippingLineName = item.ShippingLineName;
                    $scope.PayeeId = item.ShippingLineId;
                    $scope.PayeeName = item.ShippingLineName;
                }
               });
            debugger;
            $scope.c = 1;
            $scope.ContainerNo = item.ContainerNo;
            $scope.CFSCode = item.CFSCode;
            $scope.Size = item.Size;
           
            $scope.GstNo = item.GSTNo;
            $scope.RefId = item.RefId;
            $scope.EntryDateTime = item.EntryDate;
            $scope.DestuffDate = item.EmptyDate;
            $scope.Invoiceobj = {};
            $('#ContainerNoModal').modal('hide');
            if ($('#hdnShippingLine').val() != '')
                $scope.ShippingLine = JSON.parse($('#hdnShippingLine').val());
            else $scope.ShippingLine = [];
        }
        $scope.SelectShippingLine = function (item) {
            $scope.ToShippingLineId = item.ShippingLineId;
            $scope.ToShippingLineName = item.ShippingLineName;
            $('#ShippingLineModal').modal('hide');
        }
        $scope.SelectPayee = function (PartyId, PartyName) {
            debugger;
            $scope.PayeeId = PartyId;
            $scope.PayeeName = PartyName;
            $('#Paybox').val('');
            $("#PayeeModal").modal("hide");
            LoadPayee();
            $scope.$applyAsync();
            $("#PayeeModal").modal("hide");
        }
        $scope.CalculateCharges = function () {
            $scope.InvoiceDate = $('#InvoiceDate').val();
            $scope.Gener = 1;
            $('#InvoiceDate').parent().find('img').css('display', 'none');
            if ($scope.c == 1) {
            debugger;
            EmpContTrnsService.GetEmptyContTransferCharges($scope.InvoiceDate, TaxType, $scope.CFSCode, $scope.ContainerNo, $scope.Size, $scope.EntryDateTime,
               $scope.DestuffDate, $scope.RefId, $scope.FromShippingLineId, $scope.PayeeId, 0).then(function (res) {
                   $scope.Invoiceobj = res.data;
                   console.log($scope.Invoiceobj);
                  // $("#InvoiceDate").datepicker("option", "disabled", true);
            });
            
            $('#InvoiceDate').parent().find('img').css('display', 'none');
        }
        }
        $scope.Reset = function () {
            $('#DivBody').load('/Import/DSR_CwcImport/EmptyContainerTransferInv?type=Tax');
        }

        $scope.Save= function () {

           
            if ($scope.Invoiceobj.TotalAmt <= 0) {
                $scope.Message = "Can not be saved. Invoice Amount cannot be Zero or Negative.";
                return false;
            }

            if ($('#ToShippingLineName').val()=='') {
                $scope.Message = "To ShippingLine Name Should be selected ";
                return false;
            }
            if ($scope.Gener == 0)
            {
                $scope.Message = "Please Generate Invoice charges";
                return false;
            }
            if (confirm('Are you sure to Generate this Invoice?')) {
                // $scope.InvoiceObj.InvoiceId = 0;
                $('#btnSave').attr("disabled", true);
                debugger;
                $scope.Gener = 0;
                $scope.Invoiceobj.PayeeId = $scope.PayeeId;
                $scope.Invoiceobj.PayeeName = $scope.PayeeName;
                $scope.Invoiceobj.Remarks = $scope.Remarks;
                $scope.Invoiceobj.LocationId = $scope.ToShippingLineId;
                $scope.Invoiceobj.LocationName = $scope.ToShippingLineName;
                //console.log($scope.InvoiceObj);

                //var objfinal = $scope.InvoiceObj;



                EmpContTrnsService.GenerateInvoice($scope.Invoiceobj).then(function (res) {
                    console.log(res);
                    $scope.InvoiceNo = res.data.Data.InvoiceNo;
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    $('#btnPrint').removeAttr("disabled");
                });
            }
        }
       

    });
})();