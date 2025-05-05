(function () {
    angular.module('CWCApp').
    controller('PpgAuctionInvoiceEditCtrl', function ($scope, PpgAuctionInvoiceEditService) {
        debugger;
        $scope.InvoiceNo = "";
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];

        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];
        if ($('#hdnInvoice').val() != null && $('#hdnInvoice').val() != '') {
            $scope.InvoiceList = JSON.parse($('#hdnInvoice').val());
        }
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }

     //   $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
      //  $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());
        $scope.Rights = JSON.parse($("#hdnRights").val());

        $scope.SelectParty = function (obj) {
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;
            $scope.PlaceOfSupply = obj.State;
            //$scope.SelectedPartyIndex=i;
            $('#PartyModal').modal('hide');
        };

        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            $scope.PayeeName = obj.PartyName;
            //$scope.SelectedPayeeIndex=i;
            $('#PayeeModal').modal('hide');
        };


        $scope.GetInvNo = function () {

            debugger;
            PpgAuctionInvoiceEditService.GetAppNoForYard("AUC").then(function (res) {
                debugger;

                $('#hdnInvoice').val(res.data);
                $scope.InvoiceList = JSON.parse($('#hdnInvoice').val());


            });

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
            PpgAuctionInvoiceEditService.GetDeliInvoiceDetails($scope.InvoiceId).then(function (res) {
                $('.modalloader').hide();
                debugger;
                $scope.conatiners = res.data.Containers;
                $scope.InvoiceObj = res.data.Data;

                $scope.CWCChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {

                    return $scope.HtCharges.indexOf(item.Clause) < 0;
                });
                $scope.HTChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                    return $scope.HtCharges.indexOf(item.Clause) > -1;
                });

                $scope.TaxType = $scope.InvoiceObj.InvoiceType;
                $('#InvoiceDate').val($scope.InvoiceObj.DeliveryDate);

                $scope.StuffingReqId = $scope.InvoiceObj.RequestId;
                $scope.StuffingReqNo = $scope.InvoiceObj.RequestNo;
                $scope.StuffingReqDate = $scope.InvoiceObj.RequestDate;
                $scope.PlaceOfSupply = $scope.InvoiceObj.PartyState;
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
                $scope.PaymentMode = $scope.InvoiceObj.PaymentMode;
                $scope.BidId = $scope.InvoiceObj.BidId;
                $scope.AssesmentType = $scope.InvoiceObj.AssesmentType;
                $scope.HSNCode = $scope.InvoiceObj.HSNCode;
                $scope.GSTPer = $scope.InvoiceObj.GSTPer;
               
                $scope.AssesmentDate = $scope.InvoiceObj.AssesmentDate;
                $scope.FreeUpto = $scope.InvoiceObj.FreeUpto;
                $scope.StuffingReqDate = $scope.InvoiceObj.AssesmentDate;
                if ($scope.Rights.CanEdit == 1) {
                    // $('#btnSave').removeAttr("disabled");
                }

                console.log($scope.InvoiceObj);
            })
        }

        $scope.ContainerSelect = function () {
            debugger;
            var c = 0;
          
            if ($('#InvoiceNo').val() == '') {
                alert("Please select Invoice No.");
                return false;
            }

            else {


                $('.modalloader').show();


                PpgAuctionInvoiceEditService.ContainerSelect($scope.BidId, $scope.InvoiceId, $('#InvoiceDate').val(), $scope.FreeUpto, $scope.HSNCode, "0", $scope.OTHours, "0", "0", "0").then(function (res) {
                    debugger;
                    $('.modalloader').hide();
                    $scope.InvoiceObj = res.data;

                    $('#TotalCGST').val($scope.InvoiceObj.TotalCGST);
                    $('#TotalSGST').val($scope.InvoiceObj.TotalSGST);
                    $('#TotalIGST').val($scope.InvoiceObj.TotalIGST);

                    $('#PaymentSheetModelJson').val($scope.InvoiceObj.PaymentSheetModelJson);
                    var rawJson = JSON.parse($('#PaymentSheetModelJson').val());
                    var TotalCGST = 0;
                    var TotalSGST = 0;
                    var TotalIGST = 0;
                    var AllTotal = 0;
                    var TotalAmt = 0;

                    $.each(rawJson, function (i, item) {
                        TotalCGST += item.CGSTAmt;
                        TotalSGST += item.SGSTAmt;
                        TotalIGST += item.IGSTAmt;
                        AllTotal += Math.ceil(item.Total);
                        TotalAmt += item.Amount;
                    });

                    $('#TotalCGST').val(TotalCGST);
                    $('#TotalSGST').val(TotalSGST);
                    $('#TotalIGST').val(TotalIGST);
                    $('#TotalAmt').val(TotalAmt);
                    // $('#AllTotal').val(parseFloat(AllTotal).toFixed(2));      // decimal value
                    $('#AllTotal').val(Math.ceil(AllTotal));                   // ceilling value
                    $('#InvoiceValue').val(Math.ceil(AllTotal));
                    var round = Math.ceil(AllTotal) - AllTotal;
                    // $('#RoundUp').val(round.toFixed(2));      // decimal value
                    $('#RoundUp').val(0);
                    $scope.CWCChargeList = JSON.parse($scope.InvoiceObj.PaymentSheetModelJson);
                    $scope.InvoiceObj.AllTotal = Math.ceil(AllTotal);
                    $scope.InvoiceObj.RoundUp = Math.ceil(AllTotal) - AllTotal;
                    $scope.InvoiceObj.InvoiceAmt = Math.ceil(AllTotal);
                    $scope.InvoiceObj.TotalAmt = Math.ceil(AllTotal);
                    $scope.InvoiceObj.InvoiceValue = Math.ceil(AllTotal);
                    $scope.InvoiceObj.TotalCGST = TotalCGST;
                    $scope.InvoiceObj.TotalSGST = TotalSGST;
                    $scope.InvoiceObj.TotalIGST = TotalIGST;
                    $scope.InvoiceObj.RoundUp = Math.ceil(AllTotal) - AllTotal;

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
                PpgAuctionInvoiceEditService.GenerateInvoice($scope.InvoiceObj).then(function (res) {
                    //$scope.conatiners = JSON.parse(res.data);
                    console.log(res.data);
                    $scope.InvoiceNo = res.data.Data;
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