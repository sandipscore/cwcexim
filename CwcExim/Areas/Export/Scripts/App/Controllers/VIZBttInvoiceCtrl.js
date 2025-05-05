(function () {
    angular.module('CWCApp').
    controller('BttInvoiceCtrl', function ($scope, BttInvoiceService) {
       // $scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.InvoiceNo = "";
        $scope.conatiners = [];
        $scope.Message = '';
        
        $scope.NoOfVehicles = 0;
        $scope.SEZ = 0;
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];
        $scope.SEZ = $('#SEZ').val();

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
            debugger;
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;
            //$scope.SelectedPartyIndex=i;
            $('#BillToParty').val(0);
            $('#PartyModal').modal('hide');
        };

        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            $scope.PayeeName = obj.PartyName;
            //$scope.SelectedPayeeIndex=i;
            $('#PayeeModal').modal('hide');
        };


        $scope.SelectReqNo = function (obj) {
            debugger;
            $scope.StuffingReqId = obj.StuffingReqId;
            $scope.StuffingReqNo = obj.StuffingReqNo;
            $scope.StuffingReqDate = obj.StuffingReqDate;

            
            $scope.PayeeId = obj.CHAId;
            $('#PayeeId').val(obj.CHAId);
            $scope.PayeeName = obj.CHAName;

            if (obj.BillToParty == 1 && obj.ExpCount == 1) {

                $('#BillToParty').val(1);
                $scope.PartyId = obj.PartyId;
                $('#PartyId').val(obj.PartyId)
                $scope.PartyName = obj.PartyName;
                $scope.hdnState = obj.State;
                $scope.hdnStateCode = obj.StateCode;
                $scope.hdnAddress = obj.Address;
                $scope.GSTNo = obj.CHAGSTNo;
            }
            else {
                $('#BillToParty').val(0);
                $scope.PartyId = obj.CHAId;
                $('#PartyId').val(obj.CHAId)
                $scope.PartyName = obj.CHAName;
                $scope.hdnState = obj.State;
                $scope.hdnStateCode = obj.StateCode;
                $scope.hdnAddress = obj.Address;
                $scope.GSTNo = obj.CHAGSTNo;
            }

            console.log($scope.StuffingReqId);
            BttInvoiceService.SelectReqNo($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                console.log(res.data);
            });

            $('#stuffingModal').modal('hide');
        }
        debugger;
        $scope.InvoiceObj = {};
        $scope.IsContSelected = false;
        //$scope.SEZ = ($('#SEZ').is(':checked') == true ? 1 : 0);
        $scope.ContainerSelect = function () {
            $('#InvoiceDate').parent().find('img').css('display', 'none');
            $('#DeliveryDate').parent().find('img').css('display', 'none');
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            if (c > 0) {
                var Invoicedt = $('#InvoiceDate').val();
                var Deliverydt = $('#DeliveryDate').val();
                var dtobj1 = Invoicedt.split("/");
                var dtobj3 = dtobj1[2].split(" ");
                var dtobj5 = dtobj1[1] + "/" + dtobj1[0] + "/" + dtobj3[0];
                var dtobj2 = Deliverydt.split("/");
                var dtobj4 = dtobj2[2].split(" ");
                var dtobj6 = dtobj2[1] + "/" + dtobj2[0] + "/" + dtobj4[0];

                var idt = new Date(dtobj5);

                var ddt = new Date(dtobj6);



                if (idt <= ddt && ddt >= idt) {
                    debugger;
                    $scope.SEZ = $('#SEZ').val();
                    BttInvoiceService.ContainerSelect(0, $('#DeliveryDate').val(), $('#InvoiceDate').val(), $scope.StuffingReqId, TaxType, $scope.conatiners,$('#PartyId').val(),$('#PayeeId').val(), $scope.SEZ, $scope.NoOfVehicles).then(function (res) {

                        $scope.InvoiceObj = res.data;
                        debugger;
                        /*********CWC Charge and HT Charges Distinction***************/




                        $('#PaymentSheetModelJson').val(JSON.stringify(res.data));
                        // var HTCharge = $.grep(data.lstPostPaymentChrg, function (item) { return item.ChargeType == "HT"; });
                        // $scope.CWCChargeList
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
                            html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                            // html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                            html1 += '<li id="liNHT' + i + '"><div class="boolean-container"><input id="chkNHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkNHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                        });
                        debugger;
                        console.log(html);
                        $('#lstHT').html(html);
                        $('#lstNewHT').html(html1);
                        console.log(html1);



                        /*************************************************************/
                        $scope.IsContSelected = true;
                        //console.log($scope.InvoiceObj);
                        if ($scope.Rights.CanAdd == 1) {
                            $('#btnSave').removeAttr("disabled");
                        }
                        $('.search').css('display', 'none');
                        $('#InvoiceDate').parent().find('img').css('display', 'none');
                        $('#SEZ').prop("disabled", true);
                        $('#NoOfVehicles').prop('disabled', true);













                    });
                }
                else
                {
                    alert("Invoice Date should be less than or equal to Delivery Date or Delivery Date should be greater than or equal to Invoice Date");
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
            if ($('#PartyId').val() == 0 || $('#PartyId').val() == '' || $('#PartyId').val() == null) {
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
                $scope.InvoiceObj.PartyId =$('#PartyId').val(); 
                $scope.InvoiceObj.PartyName =$('#PartyName').val();
                $scope.InvoiceObj.PartyAddress =$('#hdnAddress').val();
                $scope.InvoiceObj.PartyGST =$('#GSTNo').val();
                $scope.InvoiceObj.PartyState =$('#hdnState').val();
                $scope.InvoiceObj.PartyStateCode =$('#hdnStateCode').val();

                $scope.InvoiceObj.PayeeId = $('#PayeeId').val();
                $scope.InvoiceObj.PayeeName =$('#PayeeName').val();
                $scope.InvoiceObj.Remarks = $scope.Remarks;
                $scope.InvoiceObj.SEZ = $('#SEZ').val();

                $scope.InvoiceObj.BillToParty = $('#BillToParty').val();

                //console.log($scope.InvoiceObj);

                //var objfinal = $scope.InvoiceObj;



                BttInvoiceService.GenerateInvoice($scope.InvoiceObj, $scope.NoOfVehicles, $scope.VehicleNumber).then(function (res) {
                    debugger;
                    console.log(res);
                    


                  //  $scope.InvoiceNo = res.data.Data.InvoiceNo;
                    var InvSupplyData = res.data.Data.InvoiceNo.split(',');
                    $scope.InvoiceNo = InvSupplyData[0];
                    $scope.SupplyType = InvSupplyData[1];
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    $('#btnPrint').removeAttr("disabled");
                    $('#BtnGenerateIRN').removeAttr('disabled');
                });
            }
        }

        $scope.GetStuffingReqNo = function () {


            BttInvoiceService.GetStuffingReq().then(function (res) {
                debugger;
                $('#hdnStuffingReq').val(res.data);
                $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
               // $scope.$apply();
               
            });

        };


        $scope.GetParty = function () {


            BttInvoiceService.GetPartyForBTT().then(function (res) {
                debugger;
              
                $('#hdnPartyPayee').val(res.data);
                $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
               

            });

        };

        $scope.GetPayee = function () {


            BttInvoiceService.GetPartyForBTT().then(function (res) {
                debugger;

                $('#hdnPartyPayee').val(res.data);
                $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());


            });

        };

        $scope.GenerateIRN = function () {


            BttInvoiceService.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {

                alert(res.data.Message);

            });

        };


    });
})();