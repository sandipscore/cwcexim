(function () {
    angular.module('CWCApp').
    controller('PpgContMoveInvoiceEditCtrl', function ($scope, PpgContMoveInvoiceEditService) {
        $scope.InvoiceNo = "";
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;


        if ($('#hdnInvoice').val() != null && $('#hdnInvoice').val() != '') {
            $scope.InvoiceList = JSON.parse($('#hdnInvoice').val());
        }
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }

        $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());
        $scope.Rights = JSON.parse($("#hdnRights").val());
        $scope.LoadPartyList = function () {

            $scope.searchPayee = "";
            $scope.searchParty = "";
        }
        $scope.SelectParty = function (obj) {
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;
            $scope.PlaceOfSupply = obj.State;
           
            //$scope.SelectedPartyIndex=i;
         //   $scope.GetInvoiceCharges();
            $('#PartyModal').modal('hide');
        };


        $scope.GetInvNo = function () {

            debugger;
            PpgContMoveInvoiceEditService.GetAppNoForYard("EXPMovement").then(function (res) {
                debugger;

                $('#hdnInvoice').val(res.data);
                $scope.InvoiceList = JSON.parse($('#hdnInvoice').val());


            });

        };

        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            $scope.PayeeName = obj.PartyName;
            //$scope.SelectedPayeeIndex=i;
            $('#PayeeModal').modal('hide');
        };

        $scope.InvoiceObj = {};
        $scope.SelectInvoice = function (obj) {
            var InvoiceNumber = obj.InvoiceNo.split('-');
            $scope.InvoiceNo = InvoiceNumber[0];
            //  $scope.InvoiceNo = obj.InvoiceNo;
            $scope.InvoiceId = obj.InvoiceId;
            $scope.InvoiceDate = obj.InvoiceDate;
            $('#InvoiceDate').val(obj.InvoiceDate);

            $('#InvoiceModal').modal('hide');
            $('.modalloader').show();
            PpgContMoveInvoiceEditService.GetYardInvoiceDetails($scope.InvoiceId).then(function (res) {
                $('.modalloader').hide();
                $scope.conatiners = res.data.Containers;
                $scope.InvoiceObj = res.data.Data;

                $scope.TaxType = $scope.InvoiceObj.InvoiceType;

                $scope.InvoiceId = $scope.InvoiceObj.InvoiceId;
                $('#InvoiceDate').val($scope.InvoiceObj.DeliveryDate);

                $scope.StuffingReqId = $scope.InvoiceObj.RequestId;
                $scope.StuffingReqNo = $scope.InvoiceObj.RequestNo;
                $scope.StuffingReqDate = $scope.InvoiceObj.RequestDate;
                $scope.GatewayPortId = $scope.InvoiceObj.GatewayPortId
                $scope.PartyId = $scope.InvoiceObj.PartyId;
                $scope.PartyName = $scope.InvoiceObj.PartyName;
                $scope.PayeeId = $scope.InvoiceObj.PayeeId;
                $scope.PayeeName = $scope.InvoiceObj.PayeeName;
                $scope.hdnState = $scope.InvoiceObj.PartyState;
                $scope.hdnStateCode = $scope.InvoiceObj.PartyStateCode;
                $scope.hdnAddress = $scope.InvoiceObj.PartyAddress;
                $scope.PlaceOfSupply = $scope.InvoiceObj.PartyState;
                $scope.PaymentMode = $scope.InvoiceObj.PaymentMode;
                
                $scope.GSTNo = $scope.InvoiceObj.PartyGST;
                $scope.CFSCode = $scope.InvoiceObj.CFSCode;
                $scope.ContainerNo = $scope.InvoiceObj.ContainerNo;
                $scope.MoveTo = $scope.InvoiceObj.MoveTo;
                $scope.MoveToId = $scope.InvoiceObj.MoveToId;
                $scope.ContainerStuffingId = $scope.InvoiceObj.ContainerStuffingId;
                $scope.TransportMode = $scope.InvoiceObj.TransportMode;
                $scope.TareWeight = $scope.InvoiceObj.TareWeight;
                $scope.CargoWeight = $scope.InvoiceObj.CargoWeight;

                $scope.PayeeId = $scope.InvoiceObj.PayeeId;
                $scope.ShippingLineId = $scope.InvoiceObj.ShippingId;
                $scope.ShippingLineName = $scope.InvoiceObj.ShippingLineName;
                $scope.Remarks = $scope.InvoiceObj.Remarks;
                if ($scope.Rights.CanEdit == 1) {
                  //  $('#btnSave').removeAttr("disabled");
                }

                console.log($scope.InvoiceObj);
            })
        }

        $scope.ContainerSelect = function () {
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            if (c > 0) {
                PpgContMoveInvoiceEditService.ContainerSelect($scope.InvoiceId, $('#InvoiceDate').val(), $scope.StuffingReqId, $scope.TaxType, $scope.conatiners).then(function (res) {

                    $scope.InvoiceObj = res.data;
                    $scope.IsContSelected = true;
                    console.log($scope.InvoiceObj);
                    if ($scope.Rights.CanEdit == 1) {
                        $('#btnSave').removeAttr("disabled");
                    }
                    $('.search').css('display', 'none');
                    $('#InvoiceDate').parent().find('img').css('display', 'none');


                });
            }
            $('#stuffingModal').modal('hide');

        }
        $scope.GetInvoiceCharges = function () {
            debugger


            if ($('#InvoiceNo').val() == '') {
                alert("Please select Invoice No.");
                return false;
            }

            else {

                console.log('GetInvoiceCharges');
                //ContainerStuffingDtlId, ContainerNo, MovementDate, InvoiceType, DestLocationIdiceId, Partyid, chargetype, portvalue, InvoiceId
                $('.modalloader').show();
                PpgContMoveInvoiceEditService.GetInvoiceCharges($scope.InvoiceObj.RequestId, $scope.ContainerStuffingId, $scope.ContainerNo, $scope.StuffingReqDate, $scope.TaxType, $scope.MoveToId, $scope.PartyId, 'F', $scope.GatewayPortId, $scope.TareWeight,

                    $scope.InvoiceObj.CargoType,
                    $scope.PayeeId,
                    $scope.InvoiceId
                    ).then(function (res) {
                        debugger;
                        $('.modalloader').hide();
                        $scope.InvoiceObj = res.data;
                        $scope.InvoiceObj.RequestNo = $scope.StuffingReqNo;
                        $scope.IsContSelected = true;
                        console.log($scope.InvoiceObj);
                        if ($scope.Rights.CanEdit == 1) {
                            $('#btnSave').removeAttr("disabled");
                        }
                        //$('.search').css('display', 'none');
                        //$('#InvoiceDate').parent().find('img').css('display', 'none');

                        $('.search').css('display', 'none');
                        $('#InvoiceDate').parent().find('img').css('display', 'none');

                    });
            }
        }


        $scope.SubmitInvoice = function () {
            debugger;
            if ($scope.StuffingReqId == 0 || $scope.StuffingReqId == '' || $scope.StuffingReqId == null) {
                $scope.Message = "Select  Container Movement Request Id";
                return false;
            }
            if ($scope.PartyId == 0 || $scope.PartyId == '' || $scope.PartyId == null) {
                $scope.Message = "Select Party";
                return false;
            }

           

            // if (c <= 0) {
            //    $scope.Message = "Select Atleast one container";
            //    return false;
            // }
            if ($scope.InvoiceObj.TotalAmt <= 0) {
                $scope.Message = "Can not be saved. Invoice Amount is Zero.";
                return false;
            }


            if (confirm('Are you sure to Update this Invoice?')) {
                debugger;
                $scope.InvoiceObj.InvoiceId = $scope.InvoiceId;
                $scope.InvoiceObj.InvoiceType = $scope.TaxType;
                $scope.InvoiceObj.InvoiceNo = $scope.InvoiceNo;
                $scope.InvoiceObj.InvoiceDate = $('#InvoiceDate').val();
                $scope.InvoiceObj.PartyId = $scope.PartyId;
                $scope.InvoiceObj.PartyName = $scope.PartyName;
                $scope.InvoiceObj.PartyAddress = $scope.hdnAddress;
                $scope.InvoiceObj.PartyGST = $scope.GSTNo;
                $scope.InvoiceObj.PartyState = $scope.hdnState;
                $scope.InvoiceObj.PartyStateCode = $scope.hdnStateCode;

                $scope.InvoiceObj.PayeeId = $scope.PayeeId;
                $scope.InvoiceObj.PayeeName = $scope.PayeeName;
                $scope.InvoiceObj.Remarks = $scope.Remarks;

                
                $scope.InvoiceObj.TareWeight = $scope.TareWeight;
                $scope.InvoiceObj.CargoWeight = $scope.CargoWeight;
                //console.log($scope.InvoiceObj);

                //var objfinal = $scope.InvoiceObj;
                PpgContMoveInvoiceEditService.GenerateInvoice($scope.InvoiceObj).then(function (res) {
                    //$scope.conatiners = JSON.parse(res.data);
                    console.log(res.data);
                    //$scope.InvoiceNo = res.data.Data.InvoiceNo;
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    if (res.data.Status > 0) {
                        $('#btnPrint').removeAttr("disabled");
                    }
                });
            }
        }



    });
})()