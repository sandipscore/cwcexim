(function () {
    angular.module('CWCApp').
    controller('IrrCtrl', function ($scope, IrrService) {
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.Rights = JSON.parse($("#hdnRights").val());
        $scope.Containers = [];
        /*if ($("#hdnContainersList").val() != '') {
            $scope.Containers = JSON.parse($("#hdnContainersList").val());
        }*/
        $scope.Trains = [];
        $scope.TrainNo = [];
        $scope.GrossWt = 0;
        $scope.TareWt = 0;
        $scope.NetWt = 0;
        $scope.WagonNo = '';
        $scope.Via = '';
        $scope.TrainSummaryId = 0;
        $scope.TrainDate = '';
        if ($("#hdnTrainsList").val() != '') {
            $scope.Trains = JSON.parse($("#hdnTrainsList").val());
        }

        $scope.IrrObj = new IrrInvoiceModel();

        $scope.IrrObj.InvoiceDate = $('#hdnCurDate').val();
        $scope.IrrObj.InvoiceType = TaxType;
        $scope.CargoTypes = [{ Text: '--Select--', Value: 0 }, { Text: 'Haz', Value: 1 }, { Text: 'Non Haz', Value: 2 }];
        $scope.SelectContainer = function (obj) {
            $scope.ContainerNo = obj.ContainerNo;
            $scope.Size = obj.Size;
            $scope.DisplayCfs = obj.DisplayCfs;
            $scope.IrrObj.CFSCode = obj.CFSCode;
            $scope.IrrObj.PayeeId = obj.ShippingLineId;
            $scope.IrrObj.PayeeName = obj.ShippingLine;
            $scope.IrrObj.PartyId = obj.CHAId;
            $scope.IrrObj.PartyName = obj.CHAName;
            $scope.IrrObj.GstNo = obj.GSTNo;
            $scope.IrrObj.Address = obj.Address;
            $scope.IrrObj.StateCode = obj.StateCode;
            $scope.IrrObj.StateName = obj.StateName;
            $scope.IrrObj.CargoType = obj.CargoType;
            $scope.IrrObj.InvoiceType = TaxType;
            $scope.GrossWt = obj.GrossWt;
            $scope.TareWt = obj.TareWt;
            $scope.CargoWt = obj.CargoWt;
            $scope.WagonNo = obj.WagonNo;
            $scope.Via = obj.Via;
            $scope.PortOfOrigin = obj.PortOfOrigin;
  
            $scope.CalculateCharges();

            $('#ContainerNoModal').modal('hide');
        }

        $scope.SelectTrain = function (obj) {
            $scope.TrainSummaryId = obj.TrainSummaryID;
            $scope.TrainNo = obj.TrainNo;
            $scope.TrainDate = obj.TrainDate;
            //$scope.conatiners = [];
            $scope.IrrObj.PartyName = '';
            $scope.IrrObj.PayeeName = '';
            $scope.DisplayCfs = '';
            $scope.Size = '';
            $scope.GrossWt = 0;
            $scope.TareWt = 0;
            $scope.CargoWt = 0;
            $scope.WagonNo = '';
            $scope.Via = '';
            $scope.IrrObj.GstNo = '';
            $scope.ContainerNo = '';
            $scope.IrrObj.OperationDesc = '';
            $scope.IrrObj.OperationSDesc = '';
            $scope.IrrObj.Amount = 0.00;
            $scope.IrrObj.IGSTPer = 0.00;
            $scope.IrrObj.IGST = 0.00;
            $scope.IrrObj.CGSTPer = 0.00;
            $scope.IrrObj.CGST = 0.00;
            $scope.IrrObj.SGSTPer = 0.00;
            $scope.IrrObj.SGST = 0.00;
            $scope.IrrObj.Total = 0.00;
            $scope.PortOfOrigin = '';

            GetContainersForIRR();
            $('#TrainNoModal').modal('hide');
        }

        function GetContainersForIRR() {
            IrrService.GetContainerDetails($scope.TrainSummaryId).then(function (response)
            {
                $scope.Containers = response.data;
            });

        }

        $scope.CalculateCharges=function()
        {
            debugger;
            IrrService.GetIrrCharges($scope.IrrObj.InvoiceDate, $scope.IrrObj.CFSCode, $scope.IrrObj.InvoiceType, $scope.IrrObj.CargoType, 0).then(function (res) {
                console.log(res.data);
                var charges = res.data;

                $scope.IrrObj.Amount = charges.Amount;
                $scope.IrrObj.CGST = charges.CGSTAmt;
                $scope.IrrObj.SGST = charges.SGSTAmt;
                $scope.IrrObj.IGST = charges.IGSTAmt;
                $scope.IrrObj.Total = charges.Total;
                $scope.IrrObj.InvoiceAmt = charges.Total;
                $scope.IrrObj.CGSTPer = charges.CGSTPer;
                $scope.IrrObj.SGSTPer = charges.SGSTPer;
                $scope.IrrObj.IGSTPer = charges.IGSTPer;

                $scope.IrrObj.OperationId = charges.OperationId;
                $scope.IrrObj.OperationDesc = charges.ChargeName;
                $scope.IrrObj.OperationSDesc = charges.Clause;
                $scope.IrrObj.Quantity = charges.Quantity;
                $scope.IrrObj.Rate = charges.Rate;
                $scope.IrrObj.SACCode = charges.SACCode;
            });
        }



        $scope.GenerateIRN = function () {
            $('.modalloader').show();

            IrrService.GenerateIRNNo( $scope.IrrObj.InvoiceNo,  $scope.IrrObj.SupplyType).then(function (res) {
                $('.modalloader').hide();
                alert(res.data.Message);

            });

        };

   


        $scope.Save = function () {

            if ($scope.IrrObj.CFSCode == '') {
                $scope.Message = 'Error: Select Container.';
                return false;
            }
            if ($scope.IrrObj.InvoiceAmt==0) {
                $scope.Message = 'Zero value invoice can not be saved.';
                return false;
            }

            if (confirm('Are you sure to save invoice?')) {
                $('#btnSave').attr('disabled', true);
                IrrService.GenerateInvoice($scope.IrrObj).then(function (res) {
                    if (res.data.Status == 1) {
                        //console.log(JSON.parse(res.data.Data));
                        var out = JSON.parse(res.data.Data);
                        $scope.IrrObj.InvoiceId = out.InvoiceId;
                        $scope.IrrObj.InvoiceNo = out.InvoiceNo;
                        $scope.IrrObj.SupplyType = out.SupplyType;

                    }

                    $scope.Message = res.data.Message;

                    $('#btnPrint').removeAttr("disabled");
                    $('#BtnGenerateIRN').removeAttr("disabled");
                });
            }
        }



        


        $scope.Reset = function () {
            $('#DivBody').load('/Import/Ppg_CwcImport/CreateIRR?type=Tax');
        }

    });
})();