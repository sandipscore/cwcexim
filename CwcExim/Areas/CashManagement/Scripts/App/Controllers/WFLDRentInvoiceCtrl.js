(function () {
    angular.module('CWCApp').
    controller('RentInvoiceCtrl', function ($scope, RentInvoiceService) {

        debugger;
        $scope.InvoiceNo = "";
        $scope.month = [];
        $scope.addeditflag = 0;
        $scope.AddRentDetails = [];
        $scope.month.push("--Select--");
        $scope.month.push("January");
        $scope.month.push("February");
        $scope.month.push("March");
        $scope.month.push("April");
        $scope.month.push("May");
        $scope.month.push("June");
        $scope.month.push("July");
        $scope.month.push("August");
        $scope.month.push("September");
        $scope.month.push("October");
        $scope.month.push("November");
        $scope.month.push("December");
        var d = new Date();
        var Yr = d.getFullYear();
        var pryr = Yr - 10;
        var nextyr = Yr + 10;
        $scope.Year = [];
        $('#btnAddFormOneDet').prop("disabled", false);
        $scope.Year.push("--Select--");
        for (i = 0; i < 20; i++) {
            $scope.Year.push(pryr);
            pryr++;
        }
        $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        $scope.montharr = $scope.month[0];
        $scope.Yeararr = $scope.Year[0];
        $scope.taxvalue = "0";
        $scope.Amount = 0;
        $scope.CGST = 0;
        $scope.SGST = 0;
        $scope.IGST = 0;
        $scope.Round_Up = 0;
        $scope.TotalValue = 0;
        $scope.Remarks = "";
        $scope.Invoice = "0";
        $scope.flagCharge = true;

        $scope.SelectParty = function (obj) {
            debugger;
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;
            $scope.logic1 = obj.logic1;
            $scope.logic2 = obj.logic2;
            $scope.logic3 = obj.logic3;
            $scope.Amount = 0;
            $scope.CGST = 0;
            $scope.SGST = 0;
            $scope.IGST = 0;
            $scope.Round_Up = 0;
            $scope.TotalValue = 0;
            $('#ErrParty').html('');
            $('#Erramount').html('');
            $('#DivTblStuffingErrMsg').html('');
            $('#cgst').val('0');
            $('#sgst').val('0');
            $('#igst').val('0');
            $('#amount').val('0');           
            $('#total').val('0');
            if ($scope.logic3 == 1) {
                if ($scope.logic1 == 1) {
                    $scope.taxvalue = "1";
                }
                else {
                    $scope.taxvalue = "1";

                }
            }
            else {
                if ($scope.logic2 == 1) {
                    $scope.taxvalue = "1";
                }
                else {
                    $scope.taxvalue = "1";

                }
            }

            //$scope.SelectedPartyIndex=i;
            $('#PartyModal').modal('hide');
        };

        $scope.PopulateRent = function (flg) {
            debugger;
            $scope.addeditflag = flg;
            var mon = $('#monthvalue').val();
            var yearv = $('#yearvalue').val();
            var Pid = 0;
            if ($scope.PartyId == null)
            {
                Pid = 0;
            }
            else
            {
                Pid = $scope.PartyId;
            }
            RentInvoiceService.PopulateMonthYear(mon, yearv, flg, Pid).then(
                function (res) {
                    console.log(res);
                    debugger
                    $scope.AddRentDetails = res.data.lstPrePaymentCont
                    ;
                    $scope.flagCharge = false;

                });

        };


        $scope.AddRent = function () {
            var detail = {};
            debugger;

            if ($('#PartyName').val() == "") {
                $('#ErrParty').html('Fill Out This Field');
                return false;
            }
            else if (Number($('#amount').val()) <=0) {
                $('#Erramount').html('Fill Out This Field');
                return false;
            }
            else if (Number($('#total').val()) <=0) {
                $('#DivTblStuffingErrMsg').html('Zero Amount can not be added');
                return false;
            }


            else {
                detail.PartyId = $('#PartyId').val();
                detail.PartyName = $('#PartyName').val();
                detail.Date = $('#InvoiceDate').val();
                detail.GSTNo = $('#GSTNo').val();
                detail.Address = $('#hdnAddress').val();
                detail.State = $('#hdnState').val();
                detail.StateCode = $('#hdnStateCode').val();
                detail.amount = $('#amount').val();
                detail.cgst = $('#cgst').val();
                detail.sgst = $('#sgst').val();
                detail.igst = $('#igst').val();
                detail.round_up = $('#round_up').val();
                detail.total = $('#total').val();
                detail.InvoiceNo = $('#Invoice').val() == '' ? "0" : $('#Invoice').val();
                detail.Remarks = $('#Remarks').val();
                detail.SEZ = $('#SEZ').val();
                
                var flag = 0;
                for (i = 0; i < $scope.AddRentDetails.length; i++) {
                    if ($scope.AddRentDetails[i].PartyId == detail.PartyId) {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 0) {
                    $scope.flagCharge = false;
                    $scope.AddRentDetails.push(detail);

                    $('#ErrParty').html('');
                    $('#Erramount').html('');
                    $('#DivTblStuffingErrMsg').html('');
                    $scope.PartyName = "";
                    $scope.GSTNo = "";
                    $scope.Amount = 0;
                    $scope.CGST = 0;
                    $scope.SGST = 0;
                    $scope.IGST = 0;
                    $scope.Round_Up = 0;
                    $scope.TotalValue = 0;                    
                    $scope.Remarks = "";
                    $scope.Invoice = "0";
                    $('#PartyId').val('0');
                    $('#PartyName').val('');
                    $('#amount').val('0');
                    $('#cgst').val('0');
                    $('#sgst').val('0');
                    $('#igst').val('0');
                    $('#total').val('0');
                    $('#GSTNo').val('');
                    $('#Remarks').val('');
                    $('#SEZ').prop("disabled", false);
                    $('#SEZ').prop("checked", false);
                }
                else
                {
                    $('#DivTblStuffingErrMsg').html('This Party already exist');
                    return false;
                }
            }
        };




        $scope.ResetRent = function () {
            debugger;
            $('#PartyId').val('0');
            $('#PartyName').val('');
            $('#amount').val('0');
            $('#cgst').val('0');
            $('#sgst').val('0');
            $('#igst').val('0');
            $('#total').val('0');
            $('#GSTNo').val('');
            $('#Remarks').val('');
            $('#SEZ').prop("disabled", false);
            $('#SEZ').prop("checked", false);
            var toDay = new Date();
            var mm = (toDay.getMonth()+1);
            if (mm <= 9)
            {
                var fmm = '0' + mm.toString();
            }
            var date = toDay.getDate() + '/' + fmm + '/' + toDay.getFullYear();
            $('#InvoiceDate').val(date);

            //$('#PartyName, #amount,#cgst,#sgst,#igst,#total,#round_up,#GSTNo', '#Remarks').val('');
            $('#Invoice').val('0');
            $('#btnAddFormOneDet').prop("disabled", false);
        };

        $scope.EditRentDet = function (i) {
            debugger;
            $('#PartyId').val($scope.AddRentDetails[i].PartyId);
            $('#PartyName').val($scope.AddRentDetails[i].PartyName);
            $('#InvoiceDate').val($scope.AddRentDetails[i].Date);
            $('#GSTNo').val($scope.AddRentDetails[i].GSTNo)
            // detail.Address = $('#hdnAddress').val();
            // detail.State = $('#hdnState').val();
            // detail.StateCode = $('#hdnStateCode').val();
            $('#Invoice').val($scope.AddRentDetails[i].InvoiceNo)
            $('#amount').val($scope.AddRentDetails[i].amount);
            $('#cgst').val($scope.AddRentDetails[i].cgst);
            $('#sgst').val($scope.AddRentDetails[i].sgst);
            $('#igst').val($scope.AddRentDetails[i].igst);
            $('#round_up').val($scope.AddRentDetails[i].round_up);
            $('#total').val($scope.AddRentDetails[i].total);
            $('#Remarks').val($scope.AddRentDetails[i].Remarks);
            $('#SEZ').val($scope.AddRentDetails[i].SEZ);
            //if ($scope.AddRentDetails[i].SEZ == "1") {
            //    document.getElementById("SEZ").checked = true;
            //    $('#SEZ').prop("disabled", true);
            //}
            //else {
            //    document.getElementById("SEZ").checked = false;
            //    $('#SEZ').prop("disabled", true);
            //}

            $scope.AddRentDetails.splice(i, 1);
            $('#btnAddFormOneDet').prop("disabled", false);

        };
        $scope.DeleteRentDet = function (i) {
            debugger;

            var conf = confirm("Are you sure to delete?");
            if (conf) {
                $scope.AddRentDetails.splice(i, 1);
                $('#btnAddFormOneDet').prop("disabled", false);
            }

        };

        $scope.ViewRentDet = function (i) {
            debugger;
            $('#PartyId').val($scope.AddRentDetails[i].PartyId);
            $('#PartyName').val($scope.AddRentDetails[i].PartyName);
            $('#InvoiceDate').val($scope.AddRentDetails[i].Date);
            $('#GSTNo').val($scope.AddRentDetails[i].GSTNo)
            // detail.Address = $('#hdnAddress').val();
            // detail.State = $('#hdnState').val();
            // detail.StateCode = $('#hdnStateCode').val();
            $('#Invoice').val($scope.AddRentDetails[i].InvoiceNo)
            $('#amount').val($scope.AddRentDetails[i].amount);
            $('#cgst').val($scope.AddRentDetails[i].cgst);
            $('#sgst').val($scope.AddRentDetails[i].sgst);
            $('#igst').val($scope.AddRentDetails[i].igst);
            $('#round_up').val($scope.AddRentDetails[i].round_up);
            $('#total').val($scope.AddRentDetails[i].total);
            $('#Remarks').val($scope.AddRentDetails[i].Remarks);
            $('#SEZ').val($scope.AddRentDetails[i].SEZ);


            //if ($scope.AddRentDetails[i].SEZ == "1") {                                              
            //    document.getElementById("SEZ").checked = true;
            //    $('#SEZ').prop("disabled", true);
            //}
            //else {               
            //    document.getElementById("SEZ").checked = false;
            //    $('#SEZ').prop("disabled", true);
            //}
            $('#btnAddFormOneDet').prop("disabled", true);
            // $scope.AddRentDetails.splice(i, 1);
        };

        $scope.AllChargesLst = [];



        $scope.AddChargeToRent = function () {
            debugger;

            var detailCharge = {};
            // $scope.AddRentDetails.push(detail);           
            detailCharge.ChargeName = $scope.ddlAddCharge.Text;
            detailCharge.ChargeHead = $scope.ddlAddCharge.Value;
            detailCharge.Date = $('#InvoiceDate').val();
            detailCharge.GSTNo = $scope.AddRentDetails[0].GSTNo;
            detailCharge.Address = $scope.AddRentDetails[0].PartyName;
            detailCharge.State = $scope.AddRentDetails[0].State;
            detailCharge.StateCode = $scope.AddRentDetails[0].StateCode;
            detailCharge.amount = $scope.txtAddChargeAmount;
            detailCharge.cgst = ($scope.txtAddChargeAmount * 9) / 100;
            detailCharge.sgst = ($scope.txtAddChargeAmount * 9) / 100;
            detailCharge.igst = 0;
            detailCharge.round_up = 0;
            detailCharge.total = ($scope.txtAddChargeAmount + detailCharge.cgst + detailCharge.sgst).toFixed(2);


            $scope.AllChargesLst.push(detailCharge);
            $scope.txtAddChargeAmount = 0;
            $scope.hdnlstCharge = JSON.parse($('#hdnlstCharge').val());

        }

        $scope.DeleteRentCharge = function (i) {
            var conf = confirm("Are you sure to delete?");
            if (conf) {
                $scope.AllChargesLst.splice(i, 1);

            }
        }


        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            $scope.PayeeName = obj.PartyName;
            //$scope.SelectedPayeeIndex=i;
            $('#PayeeModal').modal('hide');
        };

        if ($('#hdnlstCharge').val()!="")
            $scope.hdnlstCharge = JSON.parse($('#hdnlstCharge').val());

        $scope.InvoiceObj = {};
        $scope.SelectInvoice = function (obj) {
            $scope.InvoiceNo = obj.InvoiceNo;
            $scope.InvoiceId = obj.InvoiceId;
            $scope.InvoiceDate = obj.InvoiceDate;
            $('#InvoiceDate').val(obj.InvoiceDate);

            $('#InvoiceModal').modal('hide');

            YardInvoiceEditService.GetYardInvoiceDetails($scope.InvoiceId).then(function (res) {
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
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            if (c > 0) {
                YardInvoiceEditService.ContainerSelect($scope.InvoiceId, $('#InvoiceDate').val(), $scope.StuffingReqId, $scope.TaxType, $scope.conatiners).then(function (res) {

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

        $scope.InvoiceObj = {};
        $scope.SubmitInvoice = function () {
            debugger;
            if ($scope.AddRentDetails.length == 0) {
                $('#Errbtn').html('Add atleast one record');
                return false;
            }
            else {

                var conf = confirm("Are you sure you want to Save");
                if (conf == true) {
                    $('#btnSave').attr('disabled', true);
                    debugger;
                    $scope.InvoiceObj.InvoiceId = 0;
                    $scope.InvoiceObj.InvoiceNo = "";
                    $scope.InvoiceObj.addeditflg = $scope.addeditflag;
                    $scope.InvoiceObj.lstPrePaymentCont = $scope.AddRentDetails;
                    $scope.InvoiceObj.lstPpgRentInvoiceCharge = $scope.AllChargesLst;
                    var mon = $('#monthvalue').val();
                    var yearv = $('#yearvalue').val();
                  //  $scope.InvoiceObj.InvoiceHtml = InvoiceHtml($scope.InvoiceObj);



                    RentInvoiceService.GenerateInvoice($scope.InvoiceObj, mon, yearv, $scope.AllChargesLst).then(function (res) {
                        console.log(res);
                        debugger;
                        $scope.CallInvoiceNo = [];
                        $scope.InvoiceNo = res.data.Data.InvoiceNo;
                        var InvoiceSpilt = $scope.InvoiceNo.split("-");
                      
                        $scope.SupplyType = InvoiceSpilt[1];
                        var arr = InvoiceSpilt[0].split(",");
                        for (i = 0; i < $scope.AddRentDetails.length; i++) {
                            $scope.AddRentDetails[i].InvoiceNo = arr[i];
                            $scope.CallInvoiceNo.push({ 'InvoiceNo': arr[i] });
                        }
                          
                        $scope.Message = res.data.Message;
                       
                        $scope.Invoice = InvoiceSpilt ;
                        $('#btnSave').attr("disabled", true);
                        $('#btnGeneratedIRN').removeAttr("disabled");
                        if (res.data.Status == 4 || res.data.Status == 5 || res.data.Status == 3) {
                            $('#btnPrint').attr("disabled");
                        }
                        else {
                            $('#btnPrint').removeAttr("disabled");
                            $('#BtnGenerateIRN').removeAttr("disabled");
                        }
                    });
                }
            }




        };

        $scope.PrintInvoice = function () {

            RentInvoiceService.GeneratePrint($scope.AddRentDetails).then(function (res) {
                console.log(res);

                // $('#btnSave').attr("disabled", true);
                // $('#btnPrint').removeAttr("disabled");
            });






        };


        $scope.GenerateIRN = function () {
            for (var i = 0; i < $scope.CallInvoiceNo.length; i++)
            {
                RentInvoiceService.GenerateIRNNo($scope.CallInvoiceNo[i].InvoiceNo, $scope.SupplyType).then(function (res) {

                    alert(res.data.Message);

                });
            }

           

        };




        function InvoiceHtml(actualJson) {
            debugger;
        var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft' /></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)<br/><span style='font-size:7pt;'>Yard Invoice</span></th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>" + actualJson.ROAddress + "</td><td style='font-weight:600;'>" + actualJson.CompanyName + "<br />" + actualJson.CompanyAddress + "<br />" + actualJson.EmailAddress + "</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>" + (actualJson.InvoiceType == "Tax" ? "TAX INVOICE" : "BILL OF SUPPLY") + "</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.CompGST + "</span><br />PAN NO.- <span>" + actualJson.CompPAN + "</span><br />STATE CODE : <span>" + actualJson.CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
        if ($('#hdnBOL').val() != '') {
            BOL = (($('#hdnBOL').val().split(':')[0] == null || $('#hdnBOL').val().split(':')[0] == undefined) ? "" : $('#hdnBOL').val().split(':')[0]);
            BOLDt = (($('#hdnBOL').val().split(':')[1] == null || $('#hdnBOL').val().split(':')[1] == undefined) ? "" : $('#hdnBOL').val().split(':')[1]);
        }
        //var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft' /></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)</th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>" + actualJson.ROAddress + "</td><td style='font-weight:600;'>" + actualJson.CompanyName + "<br />" + actualJson.CompanyAddress + "<br />" + actualJson.EmailAddress + "</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>" + (actualJson.InvoiceType == "Tax" ? "TAX INVOICE" : "BILL OF SUPPLY") + "</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.CompGST + "</span><br />PAN NO.- <span>" + actualJson.CompPAN + "</span><br />STATE CODE : <span>" + actualJson.CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
        //var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft'/></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)</th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>REGIONAL OFFICE, KOLKATA<br />CMC BUILDING, PHASE-1, 6TH FLOOR, NEW MARKET COMPLEX,<br />15 N, NELLIE SENGUPTA SARANI, KOLKATA - 700087</td><td style='font-weight:600;'>CONTAINER FREIGHT STATION<br />18, COAL DOCK ROAD,<br />KOLKATA - 700043<br />E-mail ID : manager_cfs@yahoo.in</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>" + (actualJson.InvoiceType == "Tax" ? "TAX INVOICE" : "BILL OF SUPPLY") + "</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.CompGST + "</span><br />PAN NO.- <span>" + actualJson.CompPAN + "</span><br />STATE CODE : <span>" + actualJson.CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
        html1 += "<td><span>" + actualJson.PartyName + "</span></td></tr><tr><td>Address</td><td><span>" + actualJson.PartyAddress + "</span></td></tr><tr><td>State</td><td><span>" + actualJson.PartyState + "</span></td></tr><tr><td>State Code</td><td><span>" + actualJson.PartyStateCode + "</span></td></tr><tr><td>GSTIN/ Unique ID</td><td><span>" + actualJson.PartyGST + "</span></td></tr></tbody></table>";
        html1 += "</td><td></td></tr><tr><td colspan='2'><b>Invoice No. <span>" + actualJson.InvoiceNo + "</span></b></td><td style='vertical-align:top;text-align: center;'><b>Date: <span>" + (new Date()).getDate() + "/" + ((new Date()).getMonth() + 1) + "/" + (new Date()).getFullYear() + "</span></b></td>";
        html1 += "</tr><tr><td colspan='2'><table><tr><td style='vertical-align:top;width:50%;'><table style='width:100%;float: left;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;'><thead><tr><th style='border:1px solid #000;'>CONTAINER NO.</th><th style='border:1px solid #000;'>SIZE</th><th style='border:1px solid #000;'>ARRIVAL DT.</th><th style='border:1px solid #000;'>CARGO TYPE</th></tr></thead><tbody>";
        $.each(actualJson.lstPostPaymentCont, function (i, item) {
            html1 += "<tr><td style='border:1px solid #000;text-align:center;'><span>" + item.ContainerNo + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.Size + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.ArrivalDate + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + (item.CargoType == 1 ? "Haz" : "Non-Haz") + "</span></td></tr>";
        });
        html1 += "</tbody></table></td><td style='vertical-align:top;width:50%;'><table style='width:100%;float:right;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;border-left-style: none;'><tbody><tr><td>";
        html1 += "Shipping Line. <span>" + actualJson.ShippingLineName + "</span><br/>CFS Code No. <span>" + actualJson.CFSCode + "</span><br/>Date & Time of Arrival (FN/AN): <span>" + actualJson.ArrivalDate + "</span><br/>Date & Time of Destuffing (FN/AN)/ Delivery. <span>" + actualJson.DestuffingDate + "</span><br/>Name of Importer. <span>" + actualJson.ImporterExporter + "</span><br/>BOE No./Date <span>" + actualJson.BOENo + "/" + actualJson.BOEDate + "</span><br/>BOL No./Date <span>" + BOL + "/" + BOLDt + "</span><br/>Name of CHA. <span>" + actualJson.CHAName + "</span><br/>No of Packages. <span>" + actualJson.TotalNoOfPackages + "</span><br/>Total Gross Weight. <span>" + actualJson.TotalGrossWt + "</span><br/>Gross Weight per Package. <span>" + actualJson.TotalWtPerUnit + "</span><br/>Storage space occupied(" + actualJson.TotalSpaceOccupiedUnit + ") <span>" + actualJson.TotalSpaceOccupied + "</span><br/>Chargeable period for Storage space.";
        html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Destuffing(FN/AN)</td></tr><tr><td>(b) Date of Delivery" + actualJson.InvoiceDate + "</td></tr><tr><td>(c) Customs Examination Date " + actualJson.ApproveOn + "</td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
        html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span>" + actualJson.TotalValueOfCargo + "</span></td></tr></tbody></table>";

        var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + (new Date()).getDate() + "/" + ((new Date()).getMonth() + 1) + "/" + (new Date()).getFullYear() + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;width:20%;'>Particulars</th><th style='border:1px solid #000;'>SAC</th><th style='border:1px solid #000;'>Value</th><th style='border:1px solid #000;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
        var cwcCharges = $.grep(actualJson.lstPostPaymentChrg, function (item) { return item.ChargeType == 'CWC' && item.Total>0; });
        $.each(cwcCharges, function (i, item) {
            html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.Amount.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.Discount.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTAmt.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTAmt.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTAmt.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.Total.toFixed(2) + "</span></td></tr>";
            //html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;'><span>" + item.Amount + "</span></td><td style='border:1px solid #000;'><span></span></td><td style='border:1px solid #000;'><span>" + item.Discount + "</span></td><td style='border:1px solid #000;'><span>" + item.CGSTPer + "</span></td><td style='border:1px solid #000;'><span>" + item.CGSTAmt + "</span></td><td style='border:1px solid #000;'><span>" + item.SGSTPer + "</span></td><td style='border:1px solid #000;'><span>" + item.SGSTAmt + "</span></td><td style='border:1px solid #000;'><span>" + item.IGSTPer + "</span></td><td style='border:1px solid #000;'><span>" + item.IGSTAmt + "</span></td><td style='border:1px solid #000;'><span>" + item.Total + "</span></td></tr>";
        });
        html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>CWC TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + actualJson.CWCTDS.toFixed(2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
        var htCharges = $.grep(actualJson.lstPostPaymentChrg, function (item) { return item.ChargeType == 'HT' && item.Total > 0; });
        //html2 += "<tr><td style='border:1px solid #000;'><b><br />B. H&T Charges :-</b></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
        $.each(htCharges, function (i, item) {
            html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.Amount.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.Discount.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTAmt.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTAmt.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTAmt.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.Total.toFixed(2) + "</span></td></tr>";
            //html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;'><span>" + item.Amount + "</span></td><td style='border:1px solid #000;'><span></span></td><td style='border:1px solid #000;'><span>" + item.Discount + "</span></td><td style='border:1px solid #000;'><span>" + item.CGSTPer + "</span></td><td style='border:1px solid #000;'><span>" + item.CGSTAmt + "</span></td><td style='border:1px solid #000;'><span>" + item.SGSTPer + "</span></td><td style='border:1px solid #000;'><span>" + item.SGSTAmt + "</span></td><td style='border:1px solid #000;'><span>" + item.IGSTPer + "</span></td><td style='border:1px solid #000;'><span>" + item.IGSTAmt + "</span></td><td style='border:1px solid #000;'><span>" + item.Total + "</span></td></tr>";
        });
        html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>H&T TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + actualJson.HTTDS.toFixed(2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
        //html2 += "<tr><td>H&T TDS</td><td></td><td></td><td>" + actualJson.HTTDS + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";

        html2 += "<tr><td colspan='12'>&nbsp;</td></tr><tr><td>Grand Total</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.TotalAmt.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + /*actualJson.TDS.toFixed(2) +*/ "</span></td><td style='border:1px solid #000;text-align:right;'>" + actualJson.TotalDiscount.toFixed(2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.TotalCGST.toFixed(2) + "</span></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.TotalSGST.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.TotalIGST.toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.AllTotal.toFixed(2) + "</span></td></tr>";
        //html2 += "<tr><td colspan='12'>&nbsp;</td></tr><tr><td>Grand Total</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.TDS + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + ((actualJson.CWCTotal * 10 / 100) + (actualJson.HTTotal * 2 / 100)).toFixed(2) + "</span></td><td style='border:1px solid #000;text-align:right;'>" + actualJson.TotalDiscount + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.TotalCGST + "</span></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.TotalSGST + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.TotalIGST + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.AllTotal + "</span></td></tr>";
        html2 += "<tr><td>Round Up</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.RoundUp.toFixed(2) + "</span></td></tr>";
        html2 += "<tr><td>Grand Total(Rounded)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + actualJson.InvoiceAmt.toFixed(2) + "</span></td></tr>";
        html2 += "<tr><td colspan='12'>TDS Deduction : " + actualJson.TDS.toFixed(2) + "<br/>TDS Collection : " + actualJson.TDSCol.toFixed(2) + "<br/><br/>FIGURE IN WORDS: <span>" + CurrentcyToWord(actualJson.InvoiceAmt.toFixed(2)).toUpperCase() + "</span></td></tr><tr><td colspan='12' style='padding:50pt 0pt;'><b>REMARKS:<span>" + actualJson.Remarks + "</span></b></td></tr>";

        html2 += "</tbody><tfoot><tr><td colspan='4'>WAI/TA/JTA/JS/SUPTD</td><td colspan='4'>AM (A/cs)</td><td colspan='4' style='text-align:center;'>Manager (CFS)</td></tr></tfoot></table>";

        return btoa(html1 + '<>' + html2);
    }







    });
})()


