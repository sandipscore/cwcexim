(function () {
    angular.module('CWCApp').
    controller('ChnYardInvoiceCtrl', function ($scope, ChnYardInvoiceService) {
        //  $scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.OTHours = 0;
        $scope.InvoiceNo = "";
        $scope.NoOfVehicles = 1;
        $scope.OwnMovement = false;
        $scope.InsuredParty = false;
        $scope.Distance = 0;
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
        $scope.SupplyType = '';
        $scope.SEZ = "";
        $('#Approved').prop("disabled", false);
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }

        $scope.Rights = JSON.parse($("#hdnRights").val());

        /*********Party / Payee List and Search*******************/
        /*********************************************************/
        $scope.LoadPartyList = function () {
            ////debugger
            $scope.PartyPage = 0;
            $scope.SearchPartyText = "";
            ChnYardInvoiceService.LoadPartyList($scope.PartyPage).then(function (res) {
                $scope.PartyList = res.data.lstParty;
                $scope.btnParty = res.data.State;
            });
        }
        $scope.LoadPayeeList = function () {
            $scope.PayeePage = 0;
            $scope.SearchPayeeText = "";
            ChnYardInvoiceService.LoadPartyList($scope.PayeePage).then(function (res) {
                $scope.PayeeList = res.data.lstParty;
                $scope.btnPayee = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.LoadMorePartyList = function () {
            $scope.PartyPage = $scope.PartyPage + 1;
            ChnYardInvoiceService.LoadPartyList($scope.PartyPage).then(function (res) {
                $.each(res.data.lstParty, function (i, elem) {
                    $scope.PartyList.push(elem);
                });
                $scope.btnParty = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.LoadMorePayeeList = function () {
            $scope.PayeePage = $scope.PayeePage + 1;
            ChnYardInvoiceService.LoadPartyList($scope.PayeePage).then(function (res) {
                $.each(res.data.lstParty, function (i, elem) {
                    $scope.PayeeList.push(elem);
                });
                $scope.btnPayee = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.SearchPartyList = function () {
            ChnYardInvoiceService.SearchPartyList($scope.SearchPartyText).then(function (res) {
                $scope.PartyList = res.data.lstParty;
            });
            $scope.PartyPage = 0;
            $scope.btnParty = false;
            //$scope.$digest();
        }
        $scope.SearchPayeeList = function () {
            ChnYardInvoiceService.SearchPartyList($scope.SearchPayeeText).then(function (res) {
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
            ////debugger
            $scope.PartyId = obj.PartyId;
            //var PartyName = obj.PartyName.split('_');
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;

            $scope.OwnMovement = obj.IsTransporter;

            $scope.InsuredParty = obj.IsInsured;


            if (obj.IsInsured) {
                var Dt = $('#DeliveryDate').val();
                var dd = Dt.split(" ");
                var dd1 = dd[0].split("/");
                var dd2 = dd1[1] + "/" + dd1[0] + "/" + dd1[2];
                /*
                var dd = Dt.getDate(); var mm = Dt.getMonth() + 1;
                var yyyy = Dt.getFullYear();
                if (dd < 10) { dd = '0' + dd };
                if (mm < 10) { mm = '0' + mm };
                */
                //   var today = dd + '/' + mm + '/' + yyyy;
                //   var today1 = mm + '/' + dd + '/' + yyyy;
                var dt1 = new Date(dd2);
                //var InsuredFrom = obj.InsuredFrmDate.split("/");
                // var InvWSp1 = InsuredFrom[2].split(" ");

                // var Frm = InsuredFrom[1] + '/' + InsuredFrom[0] + '/' + InvWSp1[0];
                //var InvdateYear =InvWSp[0]  + '-' + Invday[1] + '-' + Invday[0];
                var dtFrom = new Date(obj.InsuredFrmDate);

                // var InsuredTo = obj.InsuredToDate.split("/");
                //  var InvWSp2 = InsuredTo[2].split(" ");

                //  var to = InsuredTo[1] + '/' + InsuredTo[0] + '/' + InvWSp2[0];
                //var InvdateYear =InvWSp[0]  + '-' + Invday[1] + '-' + Invday[0];
                var dtto = new Date(obj.InsuredToDate);
                if (dtFrom < dt1 && dt1 < dtto) {
                    $('#InsuredParty').prop("checked", true);
                    document.getElementById("InsuredParty").checked = true;
                    // $('#IsInsured').prop('checked', true);
                }
                else {
                    document.getElementById("InsuredParty").checked = false;
                    //$('#IsInsured').prop('checked', false);
                    $('#InsuredParty').prop("checked", false);
                }




            }
            else {
                document.getElementById("InsuredParty").checked = false;
                $('#InsuredParty').prop('checked', false);
            }
            if (obj.IsTransporter == "1") {
                document.getElementById("OwnMovement").checked = true;
                $('#OwnMovement').prop('checked', true);

            }
            else {
                document.getElementById("OwnMovement").checked = false;
                $('#OwnMovement').prop('checked', false);
            }



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
            ////debugger
            $scope.StuffingReqId = obj.StuffingReqId;
            $scope.StuffingReqNo = obj.StuffingReqNo;
            $scope.StuffingReqDate = obj.StuffingReqDate;

            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;



            if (obj.IsInsured == "1") {
                var Dt = $('#DeliveryDate').val();
                var dd = Dt.split(" ");
                var dd1 = dd[0].split("/");
                var dd2 = dd1[1] + "/" + dd1[0] + "/" + dd1[2];
                /*
                var dd = Dt.getDate(); var mm = Dt.getMonth() + 1;
                var yyyy = Dt.getFullYear();
                if (dd < 10) { dd = '0' + dd };
                if (mm < 10) { mm = '0' + mm };
                */
                //   var today = dd + '/' + mm + '/' + yyyy;
                //   var today1 = mm + '/' + dd + '/' + yyyy;
                var dt1 = new Date(dd2);

                var InsuredFrom = obj.InsuredFrom;
                //var InvWSp1=InsuredFrom[2].split(" ");

                //  var Frm =InsuredFrom[1]+'/'+InsuredFrom[0]  +'/' + InvWSp1[0];
                //var InvdateYear =InvWSp[0]  + '-' + Invday[1] + '-' + Invday[0];
                var dtFrom = new Date(InsuredFrom);

                var InsuredTo = obj.InsuredTo;
                //var InvWSp2=InsuredTo[2].split(" ");

                // var to =InsuredTo[1]+'/'+InsuredTo[0]  +'/' + InvWSp2[0];
                //var InvdateYear =InvWSp[0]  + '-' + Invday[1] + '-' + Invday[0];
                var dtto = new Date(InsuredTo);
                if (dtFrom < dt1 && dt1 < dtto) {
                    document.getElementById("InsuredParty").checked = true;
                    $('#InsuredParty').prop('checked', true);
                }
                else {
                    document.getElementById("InsuredParty").checked = false;
                    $('#InsuredParty').prop('checked', false);
                }




            }
            else {
                document.getElementById("InsuredParty").checked = false;
                $('#InsuredParty').prop('checked', false);
            }
            if (obj.Transporter == "1") {
                document.getElementById("OwnMovement").checked = true;
                $('#OwnMovement').prop('checked', true);

            }
            else {
                document.getElementById("OwnMovement").checked = false;
                $('#OwnMovement').prop('checked', false);
            }











            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.CHAGSTNo;
            $scope.CHAId = obj.CHAId;
            $scope.CHAName = obj.CHAName;
            if (obj.Billtoparty == "1") {

                $scope.PayeeId = obj.PartyId;
                $scope.PayeeName = obj.PartyName;


            }
            else {
                $scope.PayeeId = obj.CHAId;
                $scope.PayeeName = obj.CHAName;
            }

            /*$scope.OBLNo = obj.OBLNo;
            $scope.ContainerNo = obj.ContainerNo;
            $scope.SealCutDate = obj.SealCutDate;
            $scope.NoOfPkg = obj.NoOfPkg;
            $scope.GrWait = obj.GrWait;*/

            //debugger
            var dStr = obj.StuffingReqDate.split('/');
            var dt = dStr[2] + '-' + dStr[1] + '-' + dStr[0];
            $("#DeliveryDate").datepicker("option", "minDate", new Date(dt));

            //$("#DeliveryDate").datepicker("option", "minDate", obj.StuffingReqDate);

            //$scope.SelectedReqIndex = i;
            /*
            $http.get('/Import/Ppg_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + $scope.ReqNos[i].StuffingReqId).then(function (res) {
                $scope.conatiners =JSON.parse( res.data);
                // console.log(res.data);
            });
            */
            ChnYardInvoiceService.SelectReqNo($scope.StuffingReqId).then(function (res) {
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
            ChnYardInvoiceService.SelectReqNoTentative($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                // console.log(res.data);
            });

            $('#stuffingModal').modal('hide');
        }


        $scope.Print = function () {
            ////debugger
            ChnYardInvoiceService.PrintInvoice($scope.InvoiceObj).then(function (res) {

                ////debugger

                window.open(res.data.Message + "?_t=" + (new Date().getTime()), "_blank");

            });
        }

        $scope.InvoiceObj = {};
        $scope.IsContSelected = false;
        var isdirect = 0;
        $scope.ContainerSelect = function () {
            ////debugger

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

                $('#DeliveryDate').parent().find('img').css('display', 'none');
                $('#InvoiceDate').parent().find('img').css('display', 'none');
                ////debugger

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
                ////debugger
                var c = 0;
                for (i = 0; i < $scope.conatiners.length; i++) {

                    if ($scope.conatiners[i].Selected == true) {
                        c = c + 1;
                    }
                }

                if (c > 0) {


                    $scope.SEZ = $('#SEZ').val();




                    if ($('#Approved').prop("checked") == true) {
                        isdirect = 1;
                    }
                    var OwnMovement = 0;
                    if ($('#OwnMovement').prop("checked") == true) {
                        OwnMovement = 1;
                    }
                    var InsuredParty = 0;
                    if ($('#InsuredParty').prop("checked") == true) {
                        InsuredParty = 1;
                    }
                    var YardBond = 0;
                    if ($('#YardBond').prop("checked") == true) {
                        YardBond = 1;
                    }
                    var DirectDelivery = 0;
                    if ($('#DirectDelivery').prop("checked") == true) {
                        DirectDelivery = 1;
                    }

                    
                    var Weighment = 0;
                    if ($('#Weighment').prop("checked") == true) {
                        Weighment = 1;
                    }

                    var Scanning = 0;
                    if ($('#Scanning').prop("checked") == true) {
                        Scanning = 1;
                    }
                    debugger;
                    var FactoryDestuffing = 0;
                    if ($('#CustApprove').prop("checked") == true) {
                        FactoryDestuffing = 1;
                    }
                    var DirectDestuffing = 0;
                    if ($('#Approved').prop("checked") == true) {
                        DirectDestuffing = 1;
                    }


                    ChnYardInvoiceService.ContainerSelect(0, $('#InvoiceDate').val(), $('#DeliveryDate').val(), $scope.StuffingReqId, TaxType, $scope.conatiners,
                        $scope.OTHours, $scope.PartyId, $scope.PayeeId, isdirect,$scope.SEZ, $scope.NoOfVehicles,
                        $scope.Distance, OwnMovement, InsuredParty, YardBond, DirectDelivery, Weighment, $('#DiscountPer').val(), Scanning, FactoryDestuffing, DirectDestuffing).then(function (res) {
                            ////debugger
                            $scope.InvoiceObj = res.data;
                            $scope.Nday = $scope.InvoiceObj.NDays;


                            $('#IsPartyStateInCompState').val(res.data.IsPartyStateInCompState);
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
                                        + '<td><input class="text-right" id="val' + item.ChargeId + '" type="text" value="' + item.Amount.toFixed(2) + '" readonly /></td>'
                                          + '<td><input class="text-right flagclass" id="dis' + item.ChargeId + '" type="text" value="' + item.Discount.toFixed(2) + '" onblur="DiscountCal(&quot;' + item.ChargeId + '&quot;)" /></td>'
                                           + '<td><input class="text-right" id="tax' + item.ChargeId + '" type="text" value="' + item.Taxable.toFixed(2) + '" readonly /></td>'
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

                            $('#DiscountPer').prop("disabled", true);
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
                            ////debugger
                            var html = '';
                            var html1 = '';
                            //All H&T binding in modal

                            //  var HTCharge = $.grep(data.lstPostPaymentChrg, function (item) { return item.ChargeType == "HT"; });
                            $.each(HTCharge, function (i, item) {

                                var result = $.grep($scope.InvoiceObj.ActualApplicable, function (e) { return e == item.Clause; });
                                if (result.length == 0) {
                                    html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                                }
                                else {
                                    html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                                }
                                // html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                                html1 += '<li id="liNHT' + i + '"><div class="boolean-container"><input id="chkNHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkNHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                            });
                            ////debugger
                            console.log(html);
                            $('#lstHT').html(html);
                            $('#lstNewHT').html(html1);




                            var htmlADCWC = '';
                            var html1ADCWC = '';
                            //All Additional CWC binding in modal
                            ////debugger
                            var ADCWCCharge = $.grep($scope.InvoiceObj.lstADDPostPaymentChrg, function (item) { return item.ChargeType == "CWC" && item.ADDCWC == 1; });
                            $.each(ADCWCCharge, function (i, item) {
                                ////debugger
                                //var result = $.grep(data.ActualApplicable, function (e) { return e == item.Clause; });
                                //if (result.length == 0) {
                                //    html += '<li id="liADCWC' + i + '"><div class="boolean-container"><input id="chkADCWC' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkADCWC' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                                //}
                                //else {
                                //    html += '<li id="liADCWC' + i + '"><div class="boolean-container"><input id="chkADCWC' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkADCWC' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                                //}
                                html1ADCWC += '<li id="liNCWC' + i + '"><div class="boolean-container"><input id="chkNCWC' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkNCWC' + i + '"><i class="square"></i> &nbsp;  <span style="font-size:8pt;">' + item.ChargeName + '</span>' + '</label></div></li>';
                            });
                            //$('#lstHT').html(html);
                            $('#lstNewCWC').html(html1ADCWC);


                            console.log(html1);

                            var discountPer = parseFloat($('#DiscountPer').val() == '' ? 0 : $('#DiscountPer').val());
                            if (discountPer > 0) {
                                $('.flagclass').prop("readonly", true);
                            }
                            else {
                                $('.flagclass').prop("readonly", false);
                            }

                            /*************************************************************/
                            $scope.IsContSelected = true;
                            //console.log($scope.InvoiceObj);
                            if ($scope.Rights.CanAdd == 1) {
                              //  $('#btnSave').removeAttr("disabled");
                            }
                            $('.search').css('display', 'none');
                            $('#InvoiceDate').parent().find('img').css('display', 'none');
                            $('#DeliveryDate').parent().find('img').css('display', 'none');
                            $('#OTHours').prop('readonly', true);
                            $('#Approved').prop("disabled", true);
                            $('#SEZ').prop("disabled", true);
                            $('#SEZ').prop("ExaminationType", true);
                            $('#Weighment').prop('readonly', true);
                            $('#Scanning').prop('readonly', true);
                            $('#btnAddExtraCWC').removeAttr('disabled');
                            var rawJson = JSON.parse($('#PaymentSheetModelJson').val());
                            $('#lstContWiseAmountXML').val(JSON.stringify(rawJson.lstContWiseAmount));
                            $('#lstOperationCFSCodeWiseAmountXML').val(JSON.stringify(rawJson.lstOperationCFSCodeWiseAmount));
                            $('#lstPostPaymentChrgXML').val(JSON.stringify(rawJson.lstPostPaymentChrg));
                            $('#lstPostPaymentChrgBreakupXML').val(JSON.stringify(rawJson.lstPostPaymentChrgBreakup));
                            $('#lstPostPaymentContXML').val(JSON.stringify(rawJson.lstPostPaymentCont));
                            $('#lstPrePaymentContXML').val(JSON.stringify(rawJson.lstPrePaymentCont));
                        });
                }
                $('#stuffingModal').modal('hide');

            }
            else {
                alert("Invoice Date should be less than or equal to Delivery Date or Delivery Date should be greater than or equal to Invoice Date");
            }
        }
     

        //IRN


        $scope.GenerateIRN = function () {
            ////debugger
         //   var InvNo = "44091/20/23";

            $('.modalloader').show();
            ChnYardInvoiceService.GenerateIRNNo($('#InvoiceNo').val(), $scope.SupplyType).then(function (res) {
                ////debugger
                 
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

        //End



        $scope.SubmitInvoice = function () {

            ////debugger
            var parkdays =0;
            var lockdays =0;
           
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
            var Weighment = 0;
            if ($('#Weighment').prop("checked") == true) {
                Weighment = 1;
            }
            var Scanning = 0;
            if ($('#Scanning').prop("checked") == true) {
                Scanning = 1;
            }
            var OwnMovement = 0;
            if ($('#OwnMovement').prop("checked") == true) {
                OwnMovement = 1;
            }
            var FactoryDestuffing = 0;
            if ($('#CustApprove').prop("checked") == true) {
                FactoryDestuffing = 1;
            }
            var DirectDestuffing = 0;
            if ($('#Approved').prop("checked") == true) {
                DirectDestuffing = 1;
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
                    $scope.InvoiceObj.ExaminationType = $('#ExaminationType').val();
                    $scope.InvoiceObj.Weighment = Weighment;
                    $scope.InvoiceObj.Scanning = Scanning;
                    $scope.InvoiceObj.OwnMovement = OwnMovement;
                    $scope.InvoiceObj.FactoryDestuffing = FactoryDestuffing;
                    


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


                    ////debugger
                    $scope.InvoiceObj.lstContWiseAmountXML = $('#lstContWiseAmountXML').val();
                    $scope.InvoiceObj.lstOperationCFSCodeWiseAmountXML = $('#lstOperationCFSCodeWiseAmountXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgXML = $('#lstPostPaymentChrgXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgBreakupXML = $('#lstPostPaymentChrgBreakupXML').val();
                    $scope.InvoiceObj.lstPostPaymentContXML = $('#lstPostPaymentContXML').val();
                    $scope.InvoiceObj.lstPrePaymentContXML = $('#lstPrePaymentContXML').val();



                    console.log($scope.InvoiceObj);




                    ChnYardInvoiceService.GenerateInvoice($scope.InvoiceObj, $scope.NoOfVehicles, isdirect, $scope.SEZ, parkdays,lockdays).then(function (res) {
                        console.log(res);
                        ////debugger
                        var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        $scope.Message = res.data.Message;

                        $('#btnSave').attr("disabled", true);
                        if (res.data.Status == 0) {
                            $('#btnPrint').attr("disabled");
                            $('#btnIRN').attr("disabled");
                            
                        }
                        else {
                            ////debugger
                           $('#btnPrint').removeAttr("disabled");
                          
                           // $('#btnPrint').removeAttr('disabled');
                            $('#btnIRN').removeAttr("disabled");
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
                    ////debugger
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
                    $scope.InvoiceObj.Weighment = Weighment;
                    $scope.InvoiceObj.Scanning = Scanning;
                    $scope.InvoiceObj.OwnMovement = OwnMovement;
                    ////debugger
                    $scope.InvoiceObj.lstContWiseAmountXML = $('#lstContWiseAmountXML').val();
                    $scope.InvoiceObj.lstOperationCFSCodeWiseAmountXML = $('#lstOperationCFSCodeWiseAmountXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgXML = $('#lstPostPaymentChrgXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgBreakupXML = $('#lstPostPaymentChrgBreakupXML').val();
                    $scope.InvoiceObj.lstPostPaymentContXML = $('#lstPostPaymentContXML').val();
                    $scope.InvoiceObj.lstPrePaymentContXML = $('#lstPrePaymentContXML').val();



                    console.log($scope.InvoiceObj);







                    //var objfinal = $scope.InvoiceObj;



                    ChnYardInvoiceService.GenerateInvoice($scope.InvoiceObj, $scope.NoOfVehicles, isdirect, $scope.SEZ, parkdays, lockdays).then(function (res) {
                        console.log(res);
                        ////debugger
                        var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.Message = res.data.Message;

                        $('#btnSave').attr("disabled", true);
                        if (res.data.Status == 0) {
                            $('#btnPrint').attr("disabled");
                            $('#btnIRN').attr("disabled");
                        }
                        else {
                            ////debugger
                           // $('#btnPrint').removeAttr("disabled");
                            $('#btnIRN').removeAttr("disabled");
                           
                        }
                    });
                }
            }
        }

        $scope.GetAppNo = function () {

            ////debugger
            ChnYardInvoiceService.GetAppNoForYard().then(function (res) {
                ////debugger

                $('#hdnStuffingReq').val(res.data);
                $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());


            });

        };

        $scope.CHAPage = 0;
        //Add CHA Name As per UAT Finding
        $scope.LoadCHAList = function () {
            $scope.SearchCHAText = '';
            ChnYardInvoiceService.SearchCHAList('', 0).then(function (res) {
                ////debugger

                if (res.data != '') {
                    $scope.CHAList = JSON.parse(res.data);
                }
                else {
                    $scope.CHAList = [];
                }

            });
        }


        $scope.SelectCHAList = function (item) {
            $scope.CHAId = item.CHAId;
            $scope.CHAName = item.CHAName;

            $('#CHAModal').modal('hide');
        }

        $scope.SearchCHAList = function () {
            ChnYardInvoiceService.SearchCHAList($scope.SearchCHAText, 0).then(function (res) {
                ////debugger

                if (res.data != '') {
                    $scope.CHAList = JSON.parse(res.data);
                }
                else {
                    $scope.CHAList = [];
                }

            });
        }

        $scope.LoadMoreCHAList = function () {
            $scope.CHAPage++;

            ChnYardInvoiceService.SearchCHAList('', $scope.CHAPage).then(function (res) {
                ////debugger

                if (res.data != '') {
                    if ($scope.CHAList.length > 0) {
                        var jsondata = JSON.parse(res.data);
                        $.each(JSON.parse(res.data), function (i, item) {
                            $scope.CHAList.push(jsondata[i]);
                        });

                    }


                }
                else {

                }

            });

        }
    });
})();