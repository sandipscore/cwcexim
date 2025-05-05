(function () {
    angular.module('CWCApp').
    controller('DSRYardInvoiceCtrl', function ($scope, DSRYardInvoiceService) {
      //  $scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.OTHours = 0;
        $scope.InvoiceNo = "";
        $scope.NoOfVehicles = 1;
        $scope.PrivateMovement = false;
        $scope.CWCMovement = false;
        $scope.Amendment = false;
        $scope.InsuredParty = false;
        $scope.Distance = 0;
      
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];
        $scope.Nday = "";
        $scope.IsPartyStateInCompState = "";
        $scope.PartyList = [];
        $scope.PayeeList = [];
        $scope.SearchPartyText = "";
        $scope.SearchPayeeText = "";
      
        $scope.PartyPage = 0;
        $scope.PayeePage = 0;
        $scope.btnParty = false;
        $scope.btnPayee = false;
        $scope.PaymentMode = '';       
        $scope.SEZ = "";

        $('#Approved').prop("disabled", false);
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }
        
        $scope.Rights = JSON.parse($("#hdnRights").val());

        /********* CHA List and Search Start *******************/
        $scope.LoadCHAList = function () {
            //debugger;
            $scope.CHAPage = 0;
            $scope.SearchPartyText = "";
            DSRYardInvoiceService.LoadCHAList($scope.CHAPage).then(function (res) {
                $scope.CHAList = res.data.lstCHA;
                $scope.btnCHA = res.data.State;
              
            });
            $("#CHAModal").modal('show');
            $('#CHAModal').on('shown.bs.modal', function () {
                $('#SearchCHAText').focus();
            })


        }
        $scope.LoadMoreCHAList = function () {
            $scope.PartyPage = $scope.CHAPage + 1;
            DSRYardInvoiceService.LoadCHAList($scope.CHAPage).then(function (res) {
                $.each(res.data.lstCHA, function (i, elem) {
                    $scope.CHAList.push(elem);
                });
                $scope.btnCHA = res.data.State;
            });
            
        }

        $scope.SelectCHA = function (obj) {
            ////debugger;
            $scope.CHAId = obj.CHAId;
            $scope.CHAName = obj.CHAName;
            $('#CHAModal').modal('hide');
            $('#PartyName').focus();
        };

        $scope.SearchCHAList = function () {
            DSRYardInvoiceService.SearchCHAList($scope.SearchCHAText).then(function (res) {
                $scope.CHAList = res.data.lstCHA;
            });
            $scope.CHAPage = 0;
            $scope.btnCHA = false;           
        }
        /********* CHA List and Search End *******************/

        /*********Party / Payee List and Search*******************/
        /*********************************************************/
        $scope.LoadPartyList = function () {
            //debugger;
            $scope.PartyPage = 0;
            $scope.SearchPartyText = "";
            DSRYardInvoiceService.LoadPartyList($scope.PartyPage).then(function (res) {
                $scope.PartyList = res.data.lstParty;
                $scope.btnParty = res.data.State;
            });

            $("#PartyModal").modal('show');
            $('#PartyModal').on('shown.bs.modal', function () {
                $('#SearchPartyText').focus();
            })
        }
        $scope.LoadPayeeList = function () {
            $scope.PayeePage = 0;
            $scope.SearchPayeeText = "";
            DSRYardInvoiceService.LoadPartyList($scope.PayeePage).then(function (res) {
                $scope.PayeeList = res.data.lstParty;
                $scope.btnPayee = res.data.State;
            });
            //$scope.$digest();
            $("#PayeeModal").modal('show');
            $('#PayeeModal').on('shown.bs.modal', function () {
                $('#SearchPayeeText').focus();
            })
        }
        $scope.LoadMorePartyList = function () {
            $scope.PartyPage = $scope.PartyPage + 1;
            DSRYardInvoiceService.LoadPartyList($scope.PartyPage).then(function (res) {
                $.each(res.data.lstParty, function (i, elem) {
                    $scope.PartyList.push(elem);
                });
                $scope.btnParty = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.LoadMorePayeeList = function () {
            $scope.PayeePage = $scope.PayeePage + 1;
            DSRYardInvoiceService.LoadPartyList($scope.PayeePage).then(function (res) {
                $.each(res.data.lstParty, function (i, elem) {
                    $scope.PayeeList.push(elem);
                });
                $scope.btnPayee = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.SearchPartyList = function () {
            DSRYardInvoiceService.SearchPartyList($scope.SearchPartyText).then(function (res) {
                $scope.PartyList = res.data.lstParty;
            });
            $scope.PartyPage = 0;
            $scope.btnParty = false;
            //$scope.$digest();
        }
        $scope.SearchPayeeList = function () {
            DSRYardInvoiceService.SearchPartyList($scope.SearchPayeeText).then(function (res) {
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
            //debugger;
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
                var Dt = new Date();
                var dd = Dt.getDate(); var mm = Dt.getMonth() + 1;
                var yyyy = Dt.getFullYear();
                if (dd < 10) { dd = '0' + dd };
                if (mm < 10) { mm = '0' + mm };
                var today = dd + '/' + mm + '/' + yyyy;
                var today1 = mm + '/' + dd + '/' + yyyy;
                var dt1 = new Date(today1);
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
                if (dtFrom <= dt1 && dt1 < dtto) {
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
            //if (obj.IsTransporter == "1") {
            //    document.getElementById("PrivateMovement").checked = true;
            //    $('#PrivateMovement').prop('checked', true);

            //}
            //else {
            //    document.getElementById("PrivateMovement").checked = false;
            //    $('#PrivateMovement').prop('checked', false);
            //}



            //$scope.SelectedPartyIndex=i;
            $('#PartyModal').modal('hide');

            $('#PayeeName').focus();
        };

        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            // var PartyName = obj.PartyName.split('_');
            $scope.PayeeName = obj.PartyName;
            //$scope.PayeeName = obj.PartyName;
            //$scope.SelectedPayeeIndex=i;
            $('#PayeeModal').modal('hide');
            $('#Distance').focus();
        };


        $scope.SelectReqNo = function (obj) {
            //debugger;
            $scope.StuffingReqId = obj.StuffingReqId;
            var AppNo = obj.StuffingReqNo.split('-');
            $scope.StuffingReqNo = AppNo[0];
            $scope.StuffingReqDate = obj.StuffingReqDate;

            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;            
            var rms = obj.RMS;
            $('#RMSValue').val(rms);
            if (obj.IsInsured == "1") {
                var Dt = new Date();
                var dd = Dt.getDate(); var mm = Dt.getMonth() + 1;
                var yyyy = Dt.getFullYear();
                if (dd < 10) { dd = '0' + dd };
                if (mm < 10) { mm = '0' + mm };
               
                var today1 = mm + '/' + dd + '/' + yyyy;
                var dt1 = new Date(today1);
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
                if (dtFrom <= dt1 && dt1 < dtto) {
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

            // For Uat Point Block This Line
            //if (obj.Transporter == "1") {
            //    document.getElementById("PrivateMovement").checked = true;
            //    $('#PrivateMovement').prop('checked', true);
            //    document.getElementById("CWCMovement").checked = false;
            //    $('#CWCMovement').prop('checked', false);

            //}
            //else {
            //    document.getElementById("CWCMovement").checked = true;
            //    $('#CWCMovement').prop('checked', true);
            //    document.getElementById("PrivateMovement").checked = false;
            //    $('#PrivateMovement').prop('checked', false);
            //}

            DSRYardInvoiceService.BindAmendment($scope.StuffingReqId).then(function (res) {                
                console.log(res.data);
                if (res.data.IsAmendment == "1") {
                    document.getElementById("Amendment").checked = true;
                    $('#Amendment').prop('checked', true);                
                    }
                else {
                        document.getElementById("Amendment").checked = false;
                        $('#Amendment').prop('checked', false);                
                }
            });

            


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
            else
            {
                $scope.PayeeId = obj.CHAId;
                $scope.PayeeName = obj.CHAName;
            }
                      

            //debugger;
            var dStr = obj.StuffingReqDate.split('/');
            var dt = dStr[2] + '-' + dStr[1] + '-' + dStr[0];
            $("#InvoiceDate").datepicker("option", "minDate", new Date(dt));
            $("#DeliveryDate").datepicker("option", "minDate", new Date(dt));

       //$("#InvoiceDate").datepicker("option", "minDate", obj.StuffingReqDate);
       //$("#DeliveryDate").datepicker("option", "minDate", obj.StuffingReqDate);
           
            
            DSRYardInvoiceService.SelectReqNo($scope.StuffingReqId).then(function (res) {
                ////debugger;
                $scope.conatiners = JSON.parse(res.data);
                console.log($scope.conatiners);
            });
            $('#CHAName').focus();
            $('#stuffingModal').modal('hide');
        }



        $scope.SelectReqNoTentative = function (obj) {
            $scope.StuffingReqId = obj.StuffingReqId;
            var AppNo = obj.StuffingReqNo.split('-');
            $scope.StuffingReqNo = AppNo[0];
            $scope.StuffingReqDate = obj.StuffingReqDate;

            $scope.PartyId = obj.CHAId;
            $scope.PartyName = obj.CHAName;

            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.CHAGSTNo;

            $scope.PayeeId = obj.CHAId;
            $scope.PayeeName = obj.CHAName;

            var dStr = obj.StuffingReqDate.split('/');
            var dt = dStr[2] + '-' + dStr[1] + '-' + dStr[0];
            $("#InvoiceDate").datepicker("option", "minDate", new Date(dt));
            $("#DeliveryDate").datepicker("option", "minDate", new Date(dt));

            //$("#InvoiceDate").datepicker("option", "minDate", obj.StuffingReqDate);
            //$("#DeliveryDate").datepicker("option", "minDate", obj.StuffingReqDate);
            
            DSRYardInvoiceService.SelectReqNoTentative($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                // console.log(res.data);
            });

            $('#stuffingModal').modal('hide');
        }


        $scope.Print = function () {
            ////debugger;
            DSRYardInvoiceService.PrintInvoice($scope.InvoiceObj).then(function (res) {

                ////debugger;

                window.open(res.data.Message + "?_t=" + (new Date().getTime()), "_blank");

            });
        }
        
        $scope.InvoiceObj = {};
        $scope.IsContSelected = false;
        $scope.ContainerSelect = function () {

            if ($('#InvoiceDate').val() == '' || $('#DeliveryDate').val() == '') {
                alert('Please Fill InvoiceDate and DeliveryDate');
            }
            else {

                $('#InvoiceDate').parent().find('img').css('display', 'none');
                $('#DeliveryDate').parent().find('img').css('display', 'none');
                //debugger;

                $('input[type="text"],input[type="checkbox"],input[type="radio"], select').attr("disabled", true);
               

                //debugger;
                var c = 0;
                for (i = 0; i < $scope.conatiners.length; i++) {

                    if ($scope.conatiners[i].Selected == true) {
                        c = c + 1;
                    }
                }

                if (c > 0) {

                    var isdirect = 0;
                    if ($('#Direct').prop("checked") == true) {
                        isdirect = 1;
                    }
                    var PrivateMovement = 0;
                    if ($('#PrivateMovement').prop("checked") == true) {
                        PrivateMovement = 1;
                    }
                    var CWCMovement = 0;
                    if ($('#CWCMovement').prop("checked") == true) {
                        CWCMovement = 1;
                    }
                    var Amendment = 0;
                    if ($('#Amendment').prop("checked") == true) {
                        Amendment = 1;
                    }


                    var InsuredParty = 0;
                    if ($('#InsuredParty').prop("checked") == true) {
                        InsuredParty = 1;
                    }

                    $scope.SEZ = $('#SEZ').val();


                    DSRYardInvoiceService.ContainerSelect(0, $('#DeliveryDate').val(), $scope.StuffingReqId, TaxType, $scope.conatiners,
                        $scope.OTHours, $scope.PartyId, $scope.PayeeId, isdirect, $scope.NoOfVehicles,
                        $scope.Distance, PrivateMovement, InsuredParty, CWCMovement, Amendment, $('#PortId').val(), $('#MovementType').val(), $('#RMSValue').val(),$scope.SEZ).then(function (res) {
                            ////debugger;
                            $scope.InvoiceObj = res.data;
                            $scope.Nday = $scope.InvoiceObj.NDays;


                            $('#IsPartyStateInCompState').val($scope.InvoiceObj.IsPartyStateInCompState);
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
                            ////debugger;
                            var html = '';
                            var html1 = '';
                            //All H&T binding in modal

                            //  var HTCharge = $.grep(data.lstPostPaymentChrg, function (item) { return item.ChargeType == "HT"; });
                            $.each(HTCharge, function (i, item) {
                                //debugger;
                                var result = $.grep($scope.InvoiceObj.ActualApplicable, function (e) { return e.ClauseId == item.Clause && e.ClauseName == item.ChargeName; });

                                if (result.length == 0) {
                                    //debugger;
                                    html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                                }
                                else {
                                    html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                                }
                                // html += '<li id="liAHT' + i + '"><div class="boolean-container"><input id="chkAHT' + i + '" type="checkbox" checked data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkAHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                                if (item.Clause != 'Haz')
                                html1 += '<li id="liNHT' + i + '"><div class="boolean-container"><input id="chkNHT' + i + '" type="checkbox" data-val="' + btoa(JSON.stringify(item)) + '"/><label for="chkNHT' + i + '"><i class="square"></i> &nbsp; &nbsp; &nbsp;' + item.Clause + ' (<span style="font-size:8pt;">' + item.ChargeName + '</span>)' + '</label></div></li>';
                            });
                            //debugger;
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
                            $('#DeliveryDate').parent().find('img').css('display', 'none');
                            $('#OTHours').prop('readonly', true);
                            $('#Approved').prop("disabled", true);
                            $('#SEZ').prop("disabled", true);
                            BindHnTCharges();
                        });
                }
                $('#stuffingModal').modal('hide');
            }
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
            //var isdirect = 0;
            //if ($('#Approved').prop("checked") == true) {
            //    isdirect = 1;
            //}
            $scope.SEZ = $('#SEZ').val();

            var isdirect = 0;
            if ($('#Direct').prop("checked") == true) {
                isdirect = 1;
            }
            var Amendment = 0;
            if ($('#Amendment').prop("checked") == true) {
                Amendment = 1;
            }

            var InsuredParty = 0;
            if ($('#InsuredParty').prop("checked") == true) {
                InsuredParty = 1;
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
            if (Number($('#InvoiceValue').val()) <= 0) {
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
                   

                    $scope.InvoiceObj.InvoiceDate = $('#InvoiceDate').val();
                    $scope.InvoiceObj.DeliveryDate = $('#DeliveryDate').val();

                    $scope.InvoiceObj.CWCTotal = rawJson.CWCTotal;
                    $scope.InvoiceObj.HTTotal = rawJson.HTTotal;
                    $scope.InvoiceObj.HTAmtTotal = rawJson.HTAmtTotal;

                    $scope.InvoiceObj.Distance = $scope.Distance;               
                    $scope.InvoiceObj.SEZ = $scope.SEZ;
                    //debugger;
                    $scope.InvoiceObj.lstContWiseAmountXML = $('#lstContWiseAmountXML').val();
                    $scope.InvoiceObj.lstOperationCFSCodeWiseAmountXML = $('#lstOperationCFSCodeWiseAmountXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgXML = $('#lstPostPaymentChrgXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgBreakupXML = $('#lstPostPaymentChrgBreakupXML').val();
                    $scope.InvoiceObj.lstPostPaymentContXML = $('#lstPostPaymentContXML').val();
                    $scope.InvoiceObj.lstPrePaymentContXML = $('#lstPrePaymentContXML').val();



                    console.log($scope.InvoiceObj);


                   
                    MovementType = $('#MovementType').val();
                    if (MovementType == 'CWCMovement')
                    {
                        MovementType = 'CWC';
                    }
                    
                   
                    var PortName = $('#PortName').val();
                    var ExamType = $('#RMSValue').val();


                    DSRYardInvoiceService.GenerateInvoice($scope.InvoiceObj, isdirect, MovementType, $scope.conatiners, PortName, ExamType, $scope.SEZ, Amendment,
                    InsuredParty).then(function (res) {
                        console.log(res);
                        //debugger;
                        var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];

                        //$scope.InvoiceNo = res.data.Data.InvoiceNo;
                        $scope.Message = res.data.Message;


                        $('#btnSave').attr("disabled", true);
                        if (res.data.Status == 0) {
                            $('#btnPrint').attr("disabled");
                        }
                        else {
                            $('#btnPrint').removeAttr("disabled");
                            $('#BtnGenerateIRN').removeAttr("disabled");
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
                    //debugger;
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


                    $scope.InvoiceObj.InvoiceDate = $('#InvoiceDate').val();
                    $scope.InvoiceObj.DeliveryDate = $('#DeliveryDate').val();

                    $scope.InvoiceObj.CWCTotal = rawJson.CWCTotal;
                    $scope.InvoiceObj.HTTotal = rawJson.HTTotal;
                    $scope.InvoiceObj.HTAmtTotal = rawJson.HTAmtTotal;

                    $scope.InvoiceObj.Distance = $scope.Distance;
                    $scope.InvoiceObj.SEZ = $scope.SEZ;
                    //debugger;
                    $scope.InvoiceObj.lstContWiseAmountXML = $('#lstContWiseAmountXML').val();
                    $scope.InvoiceObj.lstOperationCFSCodeWiseAmountXML = $('#lstOperationCFSCodeWiseAmountXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgXML = $('#lstPostPaymentChrgXML').val();
                    $scope.InvoiceObj.lstPostPaymentChrgBreakupXML = $('#lstPostPaymentChrgBreakupXML').val();
                    $scope.InvoiceObj.lstPostPaymentContXML = $('#lstPostPaymentContXML').val();
                    $scope.InvoiceObj.lstPrePaymentContXML = $('#lstPrePaymentContXML').val();



                    console.log($scope.InvoiceObj);


                        

                    MovementType = $('#MovementType').val();
                    if (MovementType == 'CWCMovement') {
                        MovementType = 'CWC';
                    }

                    var PortName = $('#PortName').val();
                    var ExamType = $('#RMSValue').val();

                    //var objfinal = $scope.InvoiceObj;



                    DSRYardInvoiceService.GenerateInvoice($scope.InvoiceObj, isdirect, MovementType, $scope.conatiners, PortName, ExamType, $scope.SEZ, Amendment,
                    InsuredParty).then(function (res) {
                        console.log(res);

                        var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];

                        //$scope.InvoiceNo = res.data.Data.InvoiceNo;
                        $scope.Message = res.data.Message;

                        $('#btnSave').attr("disabled", true);
                        if (res.data.Status == 0) {
                            $('#btnPrint').attr("disabled");
                        }
                        else {
                            $('#btnPrint').removeAttr("disabled");
                            $('#BtnGenerateIRN').removeAttr("disabled");
                        }
                    });
                }
            }
        }

        $scope.GetAppNo = function () {

            //debugger;
            DSRYardInvoiceService.GetAppNoForYard().then(function (res) {
                //debugger;

                $('#hdnStuffingReq').val(res.data);
                $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());


            });

        };

        $scope.GenerateIRN = function () {


            DSRYardInvoiceService.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {

                alert(res.data.Message);

            });

        };

    });
})();