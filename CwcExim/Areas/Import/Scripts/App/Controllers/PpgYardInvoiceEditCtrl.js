(function () {
    angular.module('CWCApp').
    controller('PpgYardInvoiceEditCtrl', function ($scope, PpgYardInvoiceEditService) {
        $scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.InvoiceNo = "";
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];
        $scope.Nday = "";
        $scope.searchPayee = "";
        $scope.searchParty = "";
        var InvType = "";
        var dstuff = "";
        if ($('#hdnInvoice').val() != null && $('#hdnInvoice').val() != '') {
            $scope.InvoiceList = JSON.parse($('#hdnInvoice').val());
        }
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }
        $scope.LoadPartyList = function () {
         
            $scope.searchPayee = "";
            $scope.searchParty = "";
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
            PpgYardInvoiceEditService.GetAppNoForYard("ImpYard").then(function (res) {
                debugger;

                $('#hdnInvoice').val(res.data);
                $scope.InvoiceList = JSON.parse($('#hdnInvoice').val());


            });

        };



        $scope.InvoiceObj = {};
        $scope.SelectInvoice = function (obj) {
            var InvoiceNumber = obj.InvoiceNo.split('-');
            $scope.InvoiceNo = InvoiceNumber[0];
            $scope.InvoiceId = obj.InvoiceId;
            $scope.InvoiceDate = obj.InvoiceDate;
            $('#InvoiceDate').val(obj.InvoiceDate);

            $('#InvoiceModal').modal('hide');
            $('.modalloader').show();
            PpgYardInvoiceEditService.GetYardInvoiceDetails($scope.InvoiceId).then(function (res) {
                $('.modalloader').hide();
                $scope.conatiners = res.data.Containers;
                $scope.InvoiceObj = res.data.Data;

                $scope.Nday = $scope.InvoiceObj.NDays;


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
                $scope.PartyId = $scope.InvoiceObj.PartyId;
                $scope.InvoiceType = $scope.InvoiceObj.InvoiceType;
                InvType = $scope.InvoiceObj.InvoiceType;
                $scope.PaymentMode = $scope.InvoiceObj.PaymentMode;
                $scope.PartyName = $scope.InvoiceObj.PartyName;
                $scope.hdnState = $scope.InvoiceObj.PartyState;
                $scope.hdnStateCode = $scope.InvoiceObj.PartyStateCode;
                $scope.hdnAddress = $scope.InvoiceObj.PartyAddress;
                $scope.GSTNo = $scope.InvoiceObj.PartyGST;
                $scope.OTHours = $scope.InvoiceObj.OTHours;
                $scope.PayeeId = $scope.InvoiceObj.PayeeId;
                $scope.PlaceOfSupply = $scope.InvoiceObj.PartyState;
                $scope.DirectDestuffing = $scope.InvoiceObj.DirectDestuffing;
                dstuff = $scope.InvoiceObj.DirectDestuffing;
                
                $scope.PayeeName = $scope.InvoiceObj.PayeeName;
                $scope.Remarks = $scope.InvoiceObj.Remarks;
                if ($scope.Rights.CanEdit == 1) {
              //      $('#btnSave').removeAttr("disabled");
                }

                console.log($scope.InvoiceObj);
            })
        }

        $scope.ContainerSelect = function () {


            if ($('#InvoiceNo').val() == '') {
                alert("Please select Invoice No.");
                return false;
            }

            else {

                var c = 0;
                for (i = 0; i < $scope.conatiners.length; i++) {

                    if ($scope.conatiners[i].Selected == true) {
                        c = c + 1;
                    }
                }
                $('.modalloader').show();
                if (c > 0) {
                    PpgYardInvoiceEditService.ContainerSelect($scope.InvoiceId, $('#InvoiceDate').val(), $scope.StuffingReqId, $scope.TaxType, $scope.conatiners, $scope.OTHours, $scope.PartyId, $scope.PayeeId).then(function (res) {
                        $('.modalloader').hide();
                        $scope.InvoiceObj = res.data;
                        $scope.IsContSelected = true;
                        console.log($scope.InvoiceObj);
                        $scope.Nday = $scope.InvoiceObj.NDays;
                        $scope.DirectDestuffing = dstuff;

                        $scope.InvoiceType = InvType;
                        $scope.CWCChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {

                            return $scope.HtCharges.indexOf(item.Clause) < 0;
                        });
                        $scope.HTChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                            return $scope.HtCharges.indexOf(item.Clause) > -1;
                        });


                        if ($scope.Rights.CanEdit == 1) {
                            $('#btnSave').removeAttr("disabled");
                        }
                        $('.search').css('display', 'none');
                        $('#InvoiceDate').parent().find('img').css('display', 'none');


                    });
                }
            }
                $('#stuffingModal').modal('hide');
            
        }


        $scope.SubmitInvoice = function () {

            debugger;
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
            if ($scope.InvoiceObj.TotalAmt == 0) {
                $scope.Message = "Can not be saved. Invoice Amount is Zero.";
                return false;
            }


            if (confirm('Are you sure to Update this Invoice?')) {
                $('#btnSave').attr("disabled", true);
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
                PpgYardInvoiceEditService.GenerateInvoice($scope.InvoiceObj).then(function (res) {
                    //$scope.conatiners = JSON.parse(res.data);
                    console.log(res.data);
                    $scope.InvoiceNo = res.data.Data.InvoiceNo;
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);


                    if (res.data.Status> 0)
                    {
                        $('#btnPrint').removeAttr("disabled");
                    }
                    
                });
            }
        }



    });
})()