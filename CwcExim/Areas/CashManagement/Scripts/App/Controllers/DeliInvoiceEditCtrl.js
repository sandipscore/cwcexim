(function () {
    angular.module('CWCApp').
    controller('DeliInvoiceEditCtrl', function ($scope, DeliInvoiceEditService) {
        debugger;
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

        $scope.SelectParty = function (obj) {
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
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

        $scope.InvoiceObj = {};
        $scope.SelectInvoice = function (obj) {
            $scope.InvoiceNo = obj.InvoiceNo;
            $scope.InvoiceId = obj.InvoiceId;
            $scope.InvoiceDate = obj.InvoiceDate;
            $('#InvoiceDate').val(obj.InvoiceDate);

            $('#InvoiceModal').modal('hide');

            DeliInvoiceEditService.GetDeliInvoiceDetails($scope.InvoiceId).then(function (res) {
                $scope.conatiners = res.data.Containers;
                $scope.InvoiceObj = res.data.Data;

                $scope.TaxType = $scope.InvoiceObj.InvoiceType;
                $('#InvoiceDate').val($scope.InvoiceObj.DeliveryDate);

                $scope.StuffingReqId = $scope.InvoiceObj.RequestId;
                $scope.StuffingReqNo = $scope.InvoiceObj.RequestNo;
                $scope.StuffingReqDate = $scope.InvoiceObj.RequestDate;

                $scope.PartyId = $scope.InvoiceObj.PartyId;
                $scope.PartyName = $scope.InvoiceObj.PartyName;
                $scope.hdnState = $scope.InvoiceObj.PartyState;
                $scope.hdnStateCode = $scope.InvoiceObj.PartyStateCode;
                $scope.hdnAddress = $scope.InvoiceObj.PartyAddress;
                $scope.GSTNo = $scope.InvoiceObj.PartyGST;
                $scope.OTHours = $scope.InvoiceObj.OTHours;
                $scope.PayeeId = $scope.InvoiceObj.PayeeId;
                $scope.PayeeName = $scope.InvoiceObj.PayeeName;
                $scope.Remarks = $scope.InvoiceObj.Remarks;
                if ($scope.Rights.CanEdit == 1) {
                    $('#btnSave').removeAttr("disabled");
                }

                console.log($scope.InvoiceObj);
            })
        }

        $scope.ContainerSelect = function () {
            debugger;
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            if (c > 0) {
             
                    DeliInvoiceEditService.ContainerSelect($('#InvoiceDate').val(),$scope.TaxType, $scope.StuffingReqId,1,$scope.StuffingReqNo,$scope.StuffingReqDate, $scope.PartyId, $scope.PartyName, $scope.hdnAddress, $scope.hdnState, $scope.hdnStateCode, $scope.GSTNo, $scope.PayeeId, $scope.PayeeName,$scope.conatiners,$scope.OTHours,$scope.InvoiceId).then(function (res) {
                    debugger;
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


        $scope.SubmitInvoice = function () {
            debugger;

            if ($scope.StuffingReqId == 0 || $scope.StuffingReqId == '' || $scope.StuffingReqId == null) {
                $scope.Message = "Select Delivery Id";
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
            if ($scope.InvoiceObj.TotalAmt == 0) {
                $scope.Message = "Can not be saved. Invoice Amount is Zero.";
                return false;
            }


            if (confirm('Are you sure to Update this Invoice?')) {
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
                //console.log($scope.InvoiceObj);

                //var objfinal = $scope.InvoiceObj;
                DeliInvoiceEditService.GenerateInvoice($scope.InvoiceObj).then(function (res) {
                    //$scope.conatiners = JSON.parse(res.data);
                    console.log(res.data);
                    $scope.InvoiceNo = res.data.Data.InvoiceNo;
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    $('#btnPrint').removeAttr("disabled");
                });
            }
        }



    });
})()