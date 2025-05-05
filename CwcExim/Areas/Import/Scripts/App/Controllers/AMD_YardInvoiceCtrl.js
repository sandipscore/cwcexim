(function () {
    angular.module('CWCApp').
    controller('AMD_YardInvoiceCtrl', function ($scope, AMD_YardInvoiceService) {
        $scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.OTHours = 0;
        $scope.InvoiceNo = "";
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];
        $scope.Nday = "";
        $scope.PartyList = [];
        $scope.PayeeList = [];
        $scope.SearchPartyText = "";
        $scope.SearchPayeeText = "";
        $scope.PartyPage = 0;
        $scope.PayeePage = 0;
        $scope.btnParty = false;
        $scope.btnPayee = false;
        $scope.SEZ = "";
        $scope.ReeferHours = 0;
        $scope.Distance = 0;
        $scope.CustomExam = "";
        $('#Approved').prop("disabled", false);
        $('#IsFranchise').prop("disabled", false);
        $('#IsOnWheel').prop("disabled", false);
        $('#IsReworking').prop("disabled", false);
        $('#IsCargoShifting').prop("disabled", false);        
        $('#IsLiftOnOff').prop("disabled", false);
        $('#IsSweeping').prop("disabled", false);
        $('#IsHandling').prop("disabled", false);
        
        //$('#SEZ').prop("disabled", false);
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }
        /*$scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());*/
        $scope.Rights = JSON.parse($("#hdnRights").val());

        /*********Party / Payee List and Search*******************/
        /*********************************************************/
       
        $scope.LoadPartyList = function () {
            $scope.PartyPage = 0;
            $scope.SearchPartyText = "";
            AMD_YardInvoiceService.LoadPartyList($scope.PartyPage).then(function (res) {
                $scope.PartyList = res.data.lstParty;
                $scope.btnParty = res.data.State;
            });
        }
        $scope.LoadPayeeList = function () {
            $scope.PayeePage = 0;
            $scope.SearchPayeeText = "";
            AMD_YardInvoiceService.LoadPartyList($scope.PayeePage).then(function (res) {
                $scope.PayeeList = res.data.lstParty;
                $scope.btnPayee = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.LoadPayeeList();
        $scope.LoadPartyList();

        $scope.LoadMorePartyList = function () {
            $scope.PartyPage = $scope.PartyPage + 1;
            AMD_YardInvoiceService.LoadPartyList($scope.PartyPage).then(function (res) {
                $.each(res.data.lstParty, function (i, elem) {
                    $scope.PartyList.push(elem);
                });
                $scope.btnParty = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.LoadMorePayeeList = function () {
            $scope.PayeePage = $scope.PayeePage + 1;
            AMD_YardInvoiceService.LoadPartyList($scope.PayeePage).then(function (res) {
                $.each(res.data.lstParty, function (i, elem) {
                    $scope.PayeeList.push(elem);
                });
                $scope.btnPayee = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.SearchPartyList = function () {
            AMD_YardInvoiceService.SearchPartyList($scope.SearchPartyText).then(function (res) {
                $scope.PartyList = res.data.lstParty;
            });
            $scope.PartyPage = 0;
            $scope.btnParty = false;
            //$scope.$digest();
        }
        $scope.SearchPayeeList = function () {
            AMD_YardInvoiceService.SearchPartyList($scope.SearchPayeeText).then(function (res) {
                $scope.PayeeList = res.data.lstParty;
            });
            $scope.PayeePage = 0;
            $scope.btnPayee = false;
            //$scope.$digest();
        }
        $scope.SearchOnEnterPartyList = function (e) {
            if (e.keyCode == 13) {
                $scope.SearchPartyList();
            }
        }
        $scope.SearchOnEnterPayeeList = function (e) {
            if (e.keyCode == 13) {
                $scope.SearchPayeeList();
            }
        }


        //  $scope.LoadPartyList();
        //  $scope.LoadPayeeList();
        /*********************************************************/

        $scope.SelectParty = function (obj) {
            $scope.PartyId = obj.PartyId;
            //var PartyName = obj.PartyName.split('_');
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
            // var PartyName = obj.PartyName.split('_');
            $scope.PayeeName = obj.PartyName;
            //$scope.PayeeName = obj.PartyName;
            //$scope.SelectedPayeeIndex=i;
            $('#PayeeModal').modal('hide');
        };


        $scope.SelectReqNo = function (obj) {
            //debugger;
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
            /*$scope.OBLNo = obj.OBLNo;
            $scope.ContainerNo = obj.ContainerNo;
            $scope.SealCutDate = obj.SealCutDate;
            $scope.NoOfPkg = obj.NoOfPkg;
            $scope.GrWait = obj.GrWait;*/

            //debugger;
            var dStr = obj.StuffingReqDate.split('/');
            var dt = dStr[2] + '-' + dStr[1] + '-' + dStr[0];
            $("#InvoiceDate").datepicker("option", "minDate", new Date(dt));            

            //$("#InvoiceDate").datepicker("option", "minDate", obj.StuffingReqDate);

            //$scope.SelectedReqIndex = i;
            /*
            $http.get('/Import/Ppg_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + $scope.ReqNos[i].StuffingReqId).then(function (res) {
                $scope.conatiners =JSON.parse( res.data);
                // console.log(res.data);
            });
            */
            AMD_YardInvoiceService.SelectReqNo($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                console.log($scope.conatiners);
            });

            $('#stuffingModal').modal('hide');
        }



        $scope.SelectReqNoTentative = function (obj) {
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

            $("#InvoiceDate").datepicker("option", "minDate", obj.StuffingReqDate);
            //$scope.SelectedReqIndex = i;
            /*
            $http.get('/Import/Ppg_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + $scope.ReqNos[i].StuffingReqId).then(function (res) {
                $scope.conatiners =JSON.parse( res.data);
                // console.log(res.data);
            });
            */
            AMD_YardInvoiceService.SelectReqNoTentative($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                // console.log(res.data);
            });

            $('#stuffingModal').modal('hide');
        }


        $scope.Print = function () {
            //debugger;
            AMD_YardInvoiceService.PrintInvoice($scope.InvoiceObj).then(function (res) {

                //debugger;

                window.open(res.data.Message + "?_t=" + (new Date().getTime()), "_blank");

            });
        }

        $scope.InvoiceObj = {};
        $scope.IsContSelected = false;

        $scope.MovementType = '';

        $scope.ContainerSelect = function () {
            debugger;
            $('#InvoiceDate').parent().find('img').css('display', 'none');
            //debugger;

            //console.log($scope.conatiners);
            /*
            $http({
                url: "/Import/Ppg_CWCImport/GetContainerPaymentSheet/?InvoiceId=0",
                method: "POST",
                params: { InvoiceDate: $('#InvoiceDate').val(), AppraisementId: $scope.StuffingReqId },
                data: JSON.stringify($scope.conatiners)
                
            }).then(function (res) {
                console.log(res.data);
                $scope.InvoiceObj =res.data;
            });
            */
            //debugger;
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }
            
            if (c > 0) {

                $scope.SEZ = $('#SEZ').val();
                $scope.CustomExam = $('#CustomExam').val();
                var isdirect = 0;
                if ($('#Approved').prop("checked") == true) {
                    isdirect = 1;
                }
                
                var IsFranchise = 0;
                if ($('#IsFranchise').prop("checked") == true) {
                    IsFranchise = 1;
                }
                var IsOnWheel = 0;
                if ($('#IsOnWheel').prop("checked") == true) {
                    IsOnWheel = 1;
                }
                var IsReworking = 0;
                if ($('#IsReworking').prop("checked") == true) {
                    IsReworking = 1;
                }
                var IsCargoShifting = 0;
                if ($('#IsCargoShifting').prop("checked") == true) {
                    IsCargoShifting = 1;
                }                
                var IsLiftOnOff = 0;
                if ($('#IsLiftOnOff').prop("checked") == true) {
                    IsLiftOnOff = 1;
                }
                var IsSweeping = 0;
                if ($('#IsSweeping').prop("checked") == true) {
                    IsSweeping = 1;
                }
                var IsHandling = 0;
                if ($('#IsHandling').prop("checked") == true) {
                    IsHandling = 1;
                }
                              

                AMD_YardInvoiceService.ContainerSelect(0, $('#InvoiceDate').val(), $scope.StuffingReqId, TaxType, $scope.conatiners, $scope.OTHours, $scope.PartyId, $scope.PayeeId, isdirect, $scope.Movement, $scope.SEZ, $scope.CustomExam, $scope.ReeferHours, $scope.Distance, IsFranchise, IsOnWheel,IsReworking, IsCargoShifting, IsLiftOnOff, IsSweeping, IsHandling).then(function (res) {
                    debugger;
                    $scope.InvoiceObj = res.data;
                    $scope.Nday = $scope.InvoiceObj.NDays;
                    $('#IsLocalGST').val($scope.InvoiceObj.IsLocalGST);
                    /*********CWC Charge and HT Charges Distinction***************/
                    //$scope.CWCChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {

                    //    return $scope.HtCharges.indexOf(item.Clause) < 0;
                    //});
                    //$scope.HTChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                    //    return $scope.HtCharges.indexOf(item.Clause) > -1;
                    //});

                    $('#PaymentSheetModelJson').val(JSON.stringify(res.data));
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
                    //var AllTot = TotalCWC;
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
                    //debugger;
                    var html = '';
                    var html1 = '';
                    //All H&T binding in modal

                    //  var HTCharge = $.grep(data.lstPostPaymentChrg, function (item) { return item.ChargeType == "HT"; });
                    $.each(HTCharge, function (i, item) {

                        //var result = $.grep($scope.InvoiceObj.ActualApplicable, function (e) { return e == item.Clause; });
                        //var result = 0;
                        //if (result.length == 0) {
                            html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                        //}
                        //else {
                        //    html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                        //}
                        // html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                        html1 += '<li id="liNHT' + i + '"><div class="boolean-container"><input id="chkNHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkNHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                    });
                    //debugger;
                    console.log(html);
                    $('#lstHT').html(html);
                    $('#lstNewHT').html(html1);

                    /*************************************************************/
                    $scope.IsContSelected = true;
                    //console.log($scope.InvoiceObj);
                    if ($scope.Rights.CanAdd == 1) {
                        $('#btnSave').removeAttr("disabled");
                    }
                    //$('#Movement').attr
                    $scope.MovementType = $("#Movement").val();
                    $("#Movement").attr("disabled", "disabled");
                    $('.search').css('display', 'none');
                    $('#InvoiceDate').parent().find('img').css('display', 'none');
                    $('#OTHours').prop('readonly', true);
                    $('#ReeferHours').prop('readonly', true);
                    $('#Approved').prop("disabled", true);
                    $('#SEZ').prop("disabled", true);
                    $('#Distance').prop('readonly', true);
                    $('#CustomExam').prop("disabled", true);
                    $('#IsFranchise').prop("disabled", true);
                    $('#IsOnWheel').prop("disabled", true);
                    $('#IsReworking').prop("disabled", true);
                    $('#IsCargoShifting').prop("disabled", true);                    
                    $('#IsLiftOnOff').prop("disabled", true);
                    $('#IsSweeping').prop("disabled", true);
                    $('#IsHandling').prop("disabled", true);
                    
                });
            }
            $('#stuffingModal').modal('hide');

        }

        //IRN 


        $scope.GenerateIRN = function () {
            //debugger;
            //   var InvNo = "44091/20/23";

            $('.modalloader').show();
          AMD_YardInvoiceService.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {
                //debugger;

                $('.modalloader').hide();
                if (res.data.Status == 1) {

                    alert("IRN has been generated sucessfully")
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
            var isdirect = 0;
            if ($('#Approved').prop("checked") == true) {
                isdirect = 1;
            }
            var IsFranchise = 0;
            if ($('#IsFranchise').prop("checked") == true) {
                IsFranchise = 1;
            }
            var IsOnWheel = 0;
            if ($('#IsOnWheel').prop("checked") == true) {
                IsOnWheel = 1;
            }
            var IsReworking = 0;
            if ($('#IsReworking').prop("checked") == true) {
                IsReworking = 1;
            }
            var IsCargoShifting = 0;
            if ($('#IsCargoShifting').prop("checked") == true) {
                IsCargoShifting = 1;
            }
            var IsLiftOnOff = 0;
            if ($('#IsLiftOnOff').prop("checked") == true) {
                IsLiftOnOff = 1;
            }
            var IsSweeping = 0;
            if ($('#IsSweeping').prop("checked") == true) {
                IsSweeping = 1;
            }
            var IsHandling = 0;
            if ($('#IsHandling').prop("checked") == true) {
                IsHandling = 1;
            }
            //if ($('#SEZ').prop("checked") == true) {
              //  ExportUnder = "SEZ";
            //}
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
                $scope.Message = "Can not be saved. Invoice Amount cannot be Zero or Negative.";
                return false;
            }
            if ($scope.InvoiceObj.PaymentMode == "CASH") {



                if (confirm('Are you sure to Generate Cash Invoice?')) {
                    $('#btnSave').attr("disabled", true);
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

                    var rawJson = JSON.parse($('#PaymentSheetModelJson').val());

                    $scope.InvoiceObj.CWCTDS = rawJson.cwcTDS;
                    $scope.InvoiceObj.HTTDS = rawJson.htTDS;
                    $scope.InvoiceObj.TDS = rawJson.grandTDS;
                    $scope.InvoiceObj.RoundUp = rawJson.RoundUp;
                    $scope.InvoiceObj.InvoiceAmt = rawJson.InvoiceAmt;

                    $scope.InvoiceObj.AllTotal = rawJson.AllTotal;

                    $scope.InvoiceObj.TotalAmt = rawJson.TotalAmt;
                    $scope.InvoiceObj.TotalCGST = rawJson.TotalCGST;
                    $scope.InvoiceObj.TotalSGST = rawJson.TotalSGST;
                    $scope.InvoiceObj.TotalIGST = rawJson.TotalIGST;


                    $scope.InvoiceObj.CWCTotal = rawJson.CWCTotal;
                    $scope.InvoiceObj.HTTotal = rawJson.HTTotal;
                    $scope.InvoiceObj.HTAmtTotal = rawJson.HTAmtTotal;


                    //debugger;
                    $scope.InvoiceObj.lstContWiseAmountXML = $('#lstContWiseAmountXML').val();
                    $scope.InvoiceObj.lstOperationCFSCodeWiseAmountXML = $('#lstOperationCFSCodeWiseAmountXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgXML = $('#lstPostPaymentChrgXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgBreakupXML = $('#lstPostPaymentChrgBreakupXML').val();
                    $scope.InvoiceObj.lstPostPaymentContXML = $('#lstPostPaymentContXML').val();
                    $scope.InvoiceObj.lstPrePaymentContXML = $('#lstPrePaymentContXML').val();


                    //console.log($scope.InvoiceObj);

                    //var objfinal = $scope.InvoiceObj;



                    AMD_YardInvoiceService.GenerateInvoice($scope.InvoiceObj, isdirect, $scope.MovementType, $scope.SEZ, $scope.CustomExam, $scope.ReeferHours, $scope.Distance, IsFranchise, IsOnWheel, IsReworking, IsCargoShifting, IsLiftOnOff, IsSweeping, IsHandling).then(function (res) {
                        //debugger;
                        console.log(res);
                        //debugger;
                        var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        $scope.Message = res.data.Message;

                        //var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                        //$scope.InvoiceNo = InvSupplyData[0];
                        //$scope.SupplyType = InvSupplyData[1];
                       // $scope.InvoiceNo = InvSupplyData[0];
                       // $scope.InvoiceNo = res.data.Data.InvoiceNo;
                        //$scope.Message = res.data.Message;

                        $('#btnSave').attr("disabled", true);
                        if (res.data.Status == 0) {
                            $('#btnPrint').attr("disabled");
                        }
                        else {
                            $('#btnPrint').removeAttr("disabled");
                            $('#btnGeneratedIRN').removeAttr("disabled");
                        }
                    });
                }



            }
            else {

                if (confirm('Are you sure to Generate this Invoice?')) {
                    $('#btnSave').attr("disabled", true);
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

                    var rawJson = JSON.parse($('#PaymentSheetModelJson').val());

                    $scope.InvoiceObj.CWCTDS = rawJson.cwcTDS;
                    $scope.InvoiceObj.HTTDS = rawJson.htTDS;
                    $scope.InvoiceObj.TDS = rawJson.grandTDS;
                    $scope.InvoiceObj.RoundUp = rawJson.RoundUp;
                    $scope.InvoiceObj.InvoiceAmt = rawJson.InvoiceAmt;

                    $scope.InvoiceObj.AllTotal = rawJson.AllTotal;

                    $scope.InvoiceObj.TotalAmt = rawJson.TotalAmt;
                    $scope.InvoiceObj.TotalCGST = rawJson.TotalCGST;
                    $scope.InvoiceObj.TotalSGST = rawJson.TotalSGST;
                    $scope.InvoiceObj.TotalIGST = rawJson.TotalIGST;


                    $scope.InvoiceObj.CWCTotal = rawJson.CWCTotal;
                    $scope.InvoiceObj.HTTotal = rawJson.HTTotal;
                    $scope.InvoiceObj.HTAmtTotal = rawJson.HTAmtTotal;
                    
                    //debugger;
                    $scope.InvoiceObj.lstContWiseAmountXML = $('#lstContWiseAmountXML').val();
                    $scope.InvoiceObj.lstOperationCFSCodeWiseAmountXML = $('#lstOperationCFSCodeWiseAmountXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgXML = $('#lstPostPaymentChrgXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgBreakupXML = $('#lstPostPaymentChrgBreakupXML').val();
                    $scope.InvoiceObj.lstPostPaymentContXML = $('#lstPostPaymentContXML').val();
                    $scope.InvoiceObj.lstPrePaymentContXML = $('#lstPrePaymentContXML').val();
                    //console.log($scope.InvoiceObj);

                    //var objfinal = $scope.InvoiceObj;



                    AMD_YardInvoiceService.GenerateInvoice($scope.InvoiceObj, isdirect, $scope.MovementType, $scope.SEZ, $scope.CustomExam, $scope.ReeferHours, $scope.Distance, IsFranchise, IsOnWheel, IsReworking, IsCargoShifting, IsLiftOnOff, IsSweeping, IsHandling).then(function (res) {
                        console.log(res);
                        var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        $scope.Message = res.data.Message;

                        $('#btnSave').attr("disabled", true);
                        if (res.data.Status == 0) {
                            $('#btnPrint').attr("disabled");
                        }
                        else {
                            $('#btnPrint').removeAttr("disabled");
                             $('#btnGeneratedIRN').removeAttr("disabled");
                        }
                    });
                }
            }
        }

        $scope.GetAppNo = function () {

            //debugger;
            AMD_YardInvoiceService.GetAppNoForYard().then(function (res) {
                //debugger;

                $('#hdnStuffingReq').val(res.data);
                $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());


            });

        };
    });
})();