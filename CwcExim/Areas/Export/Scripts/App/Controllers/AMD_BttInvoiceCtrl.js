(function () {
    angular.module('CWCApp').
    controller('VRN_BttInvoiceCtrl', function ($scope, VRN_BttInvoiceService) {
        //$scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.InvoiceNo = "";
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];
        $scope.SEZ = "";
        // $('#SEZ').prop("disabled", false);
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
            VRN_BttInvoiceService.SelectReqNo($scope.StuffingReqId).then(function (res) {
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


            debugger;
            if (c > 0) {

                //var ExportUnder = "";
                //if ($('#SEZ').prop("checked") == true) {
                // ExportUnder = "SEZ";
                //}
                $scope.SEZ = $('#SEZ').val();
                VRN_BttInvoiceService.ContainerSelect(0, $('#InvoiceDate').val(), $scope.StuffingReqId, TaxType, $scope.conatiners, $scope.PartyId, $scope.OTHour, $scope.SEZ).then(function (res) {
                    debugger;
                    $scope.InvoiceObj = res.data;
                    $('#PaymentSheetModelJson').val(JSON.stringify(res.data));
                    /*********CWC Charge and HT Charges Distinction***************/
                    $scope.CWCChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                        return item.ChargeType == 'CWC';
                    });
                       var CWCCharge = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                            return item.ChargeType == "CWC";
                            // return $scope.HtCharges.indexOf(item.Clause) < 0;
                        });

                        html = '';
                        var TotalCWC = 0;
                        $.each(CWCCharge, function (i, item) {
                            TotalCWC += item.Total;
                            //html += '<tr><td>' + item.Clause + '. ' + item.ChargeName + '</td>'
                            html += '<tr><td>' + item.ChargeName + '</td>'
                                    + '<td><input class="text-right" id="' + item.ChargeId + '" type="text" value="' + item.Amount.toFixed(2) + '" readonly /></td>'
                                    + '<td><input class="text-right" id="IGSTP' + item.ChargeId + '" type="text" value="' + item.IGSTPer + '" readonly /></td>'
                                    + '<td><input class="text-right" id="IGSTA' + item.ChargeId + '" type="text" value="' + item.IGSTAmt.toFixed(2) + '" readonly /></td>'
                                    + '<td><input class="text-right" id="CGSTP' + item.ChargeId + '" type="text" value="' + item.CGSTPer + '" readonly /></td>'
                                    + '<td><input class="text-right" id="CGSTA' + item.ChargeId + '" type="text" value="' + item.CGSTAmt.toFixed(2) + '" readonly /></td>'
                                    + '<td><input class="text-right" id="SGSTP' + item.ChargeId + '" type="text" value="' + item.SGSTPer + '" readonly /></td>'
                                    + '<td><input class="text-right" id="SGSTA' + item.ChargeId + '" type="text" value="' + item.SGSTAmt.toFixed(2) + '" readonly /></td>'
                                    + '<td><input class="text-right" id="TOT' + item.ChargeId + '" type="text" value="' + item.Total.toFixed(2) + '" readonly /></td></tr>';
                        });
                        $('#tblCWCCharges tbody').html(html);
                        $('#TOTCWCChrg').val((TotalCWC).toFixed(2));

                        var AllTot = Math.ceil(TotalCWC);
                        var roundup = AllTot - TotalCWC;


                        $('#RoundUp').val(roundup.toFixed(2));
                        $('#AllTotal').val(TotalCWC.toFixed(2));
                        $('#InvoiceValue').val(AllTot.toFixed(2));
                        $scope.InvoiceAmt = AllTot;
                        $scope.RoundUp = roundup;
                        $scope.TotalAmt = AllTot;
                    //$scope.HTChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                    //    return item.ChargeType == 'HT';
                    //});
                        //  $scope.HTChargeList
                        var HTCharge = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                            return item.ChargeType == "HT";
                            // return $scope.HtCharges.indexOf(item.Clause) > -1;
                        });
                        debugger;
                        var html = '';
                        var html1 = '';
                        //All H&T binding in modal

                        //  var HTCharge = $.grep(data.lstPostPaymentChrg, function (item) { return item.ChargeType == "HT"; });
                        $.each(HTCharge, function (i, item) {

                            //  var result = $.grep($scope.InvoiceObj.ActualApplicable, function (e) { return e == item.Clause; });
                            //if (result.length == 0) {
                            //    html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                            //}
                            //else {
                            //    html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                            //}
                            html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox"  data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                            // html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                            html1 += '<li id="liNHT' + i + '"><div class="boolean-container"><input id="chkNHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkNHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                        });
                        debugger;
                        console.log(html);
                        $('#lstHT').html(html);
                        $('#lstNewHT').html(html1);
                        console.log(html1);
                    /*************************************************************/
                        $('#OTHour').attr("readonly", true);
                        $('#CFSFactory').attr("readonly", true);
                    //$('#ExportUnder').attr("readonly", true);
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

            var ExportUnder = "";
            if ($('#SEZ').prop("checked") == true) {
                ExportUnder = "SEZ";
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
                $scope.InvoiceObj.lstPostPaymentChrg = JSON.parse($('#hdnPaymentSheetModelJson').val());
                 
                //console.log($scope.InvoiceObj);

                //var objfinal = $scope.InvoiceObj;



                VRN_BttInvoiceService.GenerateInvoice($scope.InvoiceObj, $scope.SEZ, $scope.CFSFactory).then(function (res) {
                    debugger;
                    console.log(res);

                    //  $scope.InvoiceNo = res.data.Data.InvoiceNo;
                    var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                    $scope.InvoiceNo = InvSupplyData[0];
                    $scope.SupplyType = InvSupplyData[1];
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    $('#btnPrint').removeAttr("disabled");
                    $('#btnGeneratedIRN').removeAttr('disabled');
                    //$scope.Message = res.data.Message;

                    // $('#btnSave').attr("disabled", true);
                    // $('#btnPrint').removeAttr("disabled");
                });
            }
        }
        $scope.GenerateIRN = function () {


            VRN_BttInvoiceService.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {

                alert(res.data.Message);

            });

        };

    });
})();