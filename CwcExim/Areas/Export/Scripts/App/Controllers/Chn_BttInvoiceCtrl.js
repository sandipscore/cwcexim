(function () {
    angular.module('CWCApp').
    controller('BttInvoiceCtrl', function ($scope, BttInvoiceService) {
        //$scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.InvoiceNo = "";
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];
        $scope.SEZ = "";
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }

        $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());
        $scope.Rights = JSON.parse($("#hdnRights").val());

        // console.log( $scope.PartyList);

        $scope.SelectParty = function (obj) {
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName.split('[')[0];
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;
            //$scope.SelectedPartyIndex=i;
            $('#PartyModal').modal('hide');
        };

        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            $scope.PayeeName = obj.PartyName.split('[')[0];
            //$scope.SelectedPayeeIndex=i;
            $('#PayeeModal').modal('hide');
        };


        $scope.SelectReqNo = function (obj) {
            $scope.StuffingReqId = obj.StuffingReqId;
            $scope.StuffingReqNo = obj.StuffingReqNo;
            $scope.StuffingReqDate = obj.StuffingReqDate;

            $scope.PartyId = obj.CHAId;
            $scope.PartyName = obj.CHAName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.CHAGSTNo;

            $scope.PayeeId = obj.CHAId;
            $scope.PayeeName = obj.CHAName;

            console.log($scope.StuffingReqId);
            BttInvoiceService.SelectReqNo($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                console.log(res.data);
            });

            $('#stuffingModal').modal('hide');
        }

        $scope.InvoiceObj = {};
        $scope.IsContSelected = false;
        $scope.ContainerSelect = function () {
            
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            if (c > 0) {

                $scope.SEZ = $('#SEZ').val();
                BttInvoiceService.ContainerSelect(0, $('#InvoiceDate').val(), $scope.StuffingReqId, TaxType,$scope.SEZ, $scope.conatiners).then(function (res) {
                    debugger;
                    $scope.InvoiceObj = res.data;
                    /*********CWC Charge and HT Charges Distinction***************/
                    $scope.CWCChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                        return item.ChargeType == 'CWC';
                    });
                    $scope.HTChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                        return item.ChargeType == 'HT';
                    });
                    /*************************************************************/
                    $scope.IsContSelected = true;
                    console.log($scope.InvoiceObj);
                    if ($scope.Rights.CanAdd == 1) {
                        $('#btnSave').removeAttr("disabled");
                    }
                    $('.search').css('display', 'none');
                    $('#InvoiceDate').parent().find('img').css('display', 'none');
                    $('#SEZ').prop("disabled", true);

                });
            }
            $('#stuffingModal').modal('hide');

        }

        $scope.GenerateIRN = function () {
            debugger;
            //   var InvNo = "44091/20/23";

            $('.modalloader').show();
            BttInvoiceService.GenerateIRNNo($('#InvoiceNo').val(), $scope.SupplyType).then(function (res) {
                debugger;

                $('.modalloader').hide();
                if (res.data.Status == 1) {

                    alert("IRN has been generated successfully")
                    $('#btnPrint').removeAttr("disabled");
                    $('#btnIRN').attr("disabled", true);
                }
                else {
                    alert(res.data.Message);
                    $('#btnPrint').removeAttr("disabled");
                    $('#btnIRN').attr("disabled", true);
                }

            });


        };

        $scope.SubmitInvoice = function () {

            if ($scope.StuffingReqId == 0 || $scope.StuffingReqId == '' || $scope.StuffingReqId == null) {
                $scope.Message = "Select Assessment Id";
                return false;
            }
            if ($scope.PartyId == 0 || $scope.PartyId == '' || $scope.PartyId == null) {
                $scope.Message = "Select Party";
                return false;
            }

            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            if (c <= 0) {
                $scope.Message = "Select Atleast one container";
                return false;
            }
            if ($scope.InvoiceObj.TotalAmt <= 0) {
                $scope.Message = "Can not be saved. Invoice Amount is Zero or Negative.";
                return false;
            }


            if (confirm('Are you sure to Generate this Invoice?')) {
                $scope.InvoiceObj.InvoiceId = 0;
                $scope.InvoiceObj.InvoiceType = TaxType;
                $scope.InvoiceObj.PartyId = $scope.PartyId;
                $scope.InvoiceObj.PartyName = $scope.PartyName;
                $scope.InvoiceObj.PartyAddress = $scope.hdnAddress;
                $scope.InvoiceObj.PartyGST = $scope.GSTNo;
                $scope.InvoiceObj.PartyState = $scope.hdnState;
                $scope.InvoiceObj.PartyStateCode = $scope.hdnStateCode;

                $scope.InvoiceObj.PayeeId = $scope.PayeeId;
                $scope.InvoiceObj.PayeeName = $scope.PayeeName;
                $scope.InvoiceObj.Remarks = $scope.Remarks;
                //console.log($scope.InvoiceObj);

                //var objfinal = $scope.InvoiceObj;
                $scope.SEZ = $('#SEZ').val();

                BttInvoiceService.GenerateInvoice($scope.InvoiceObj,$scope.SEZ).then(function (res) {
                    debugger;
                    console.log(res);
                    var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                    $scope.InvoiceNo = InvSupplyData[0];
                    $scope.SupplyType = InvSupplyData[1];
                   // $scope.InvoiceNo = res.data.Data.InvoiceNo;
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    //   $('#btnPrint').removeAttr("disabled");
                    $('#btnIRN').removeAttr("disabled");
                });
            }
        }


    });
})();