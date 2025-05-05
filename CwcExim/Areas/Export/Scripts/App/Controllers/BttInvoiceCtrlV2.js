(function () {
    angular.module('CWCApp').
    controller('BttInvoiceCtrlV2', function ($scope, BttInvoiceServiceV2) {
        $scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.InvoiceNo = "";
        $scope.SupplyType = "";
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];

        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            debugger;
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }

        if ($('#hdnPartyPayee').val() != null && $('#hdnPartyPayee').val() != '') {
            $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
            $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());
        }
       
        $scope.Rights = JSON.parse($("#hdnRights").val());

        // console.log( $scope.PartyList);

        $scope.SelectParty = function (obj) {
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.PlaceOfSupply = obj.State;

            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;
            //$scope.SelectedPartyIndex=i;
            $('#PartyModal').modal('hide');
        };

        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            $scope.PayeeName = obj.PartyName;
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
            $scope.PlaceOfSupply = obj.State;

            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.CHAGSTNo;

            $scope.PayeeId = obj.CHAId;
            $scope.PayeeName = obj.CHAName;

            console.log($scope.StuffingReqId);
            BttInvoiceServiceV2.SelectReqNo($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                console.log(res.data);
            });

            $('#stuffingModal').modal('hide');
        }

        $scope.InvoiceObj = {};
        $scope.IsContSelected = false;
        $scope.ContainerSelect = function () {
            $('#InvoiceDate').parent().find('img').css('display', 'none');
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            $scope.SEZ=$('#SEZ').val();

            if (c > 0) {
                BttInvoiceServiceV2.ContainerSelect(0, $('#InvoiceDate').val(), $scope.StuffingReqId, TaxType, $scope.PartyId, $scope.conatiners, $scope.SEZ).then(function (res) {

                    $scope.InvoiceObj = res.data;
                    /*********CWC Charge and HT Charges Distinction***************/
                    $scope.CWCChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                        return $scope.HtCharges.indexOf(item.Clause) < 0;
                    });
                    $scope.HTChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                        return $scope.HtCharges.indexOf(item.Clause) > -1;
                    });
                    /*************************************************************/
                    $scope.IsContSelected = true;
                    console.log($scope.InvoiceObj);
                    $('#SEZ').prop('disabled',true)
                    if ($scope.Rights.CanAdd == 1) {
                        $('#btnSave').removeAttr("disabled");
                    }
                    $('.search').css('display', 'none');
                    $('#InvoiceDate').parent().find('img').css('display', 'none');


                });
            }
            $('#stuffingModal').modal('hide');

        }


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
                $('#btnSave').attr('disabled', true);
               
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
                $scope.InvoiceObj.SEZ = $scope.SEZ;
                //console.log($scope.InvoiceObj);

                //var objfinal = $scope.InvoiceObj;



                BttInvoiceServiceV2.GenerateInvoice($scope.InvoiceObj, $scope.SEZ).then(function (res) {
                    debugger;
                    console.log(res);
                    
                   // $scope.InvoiceNo = res.data.Data.InvoiceNo;

                    var InvSupplyData = res.data.Data.InvoiceNo.split(',');
                    $scope.InvoiceNo = InvSupplyData[0];
                    $scope.SupplyType = InvSupplyData[1];

                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    $('#btnPrint').removeAttr("disabled");
                    $('#BtnGenerateIRN').removeAttr("disabled");
                });
            }
        }

        $scope.GetStuffingReqNo = function () {


            BttInvoiceServiceV2.GetStuffingReq().then(function (res) {
                debugger;
                $('#hdnStuffingReq').val(res.data);
                $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
               // $scope.$apply();
               
            });

        };


        $scope.GetParty = function () {


            BttInvoiceServiceV2.GetPartyForBTT().then(function (res) {
                debugger;
              
                $('#hdnPartyPayee').val(res.data);
                $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
               

            });

        };

        $scope.GetPayee = function () {


            BttInvoiceServiceV2.GetPartyForBTT().then(function (res) {
                debugger;

                $('#hdnPartyPayee').val(res.data);
                $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());


            });

        };

        $scope.GenerateIRN = function () {


            BttInvoiceServiceV2.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {

                alert(res.data.Message);

            });

        };
    });
})();