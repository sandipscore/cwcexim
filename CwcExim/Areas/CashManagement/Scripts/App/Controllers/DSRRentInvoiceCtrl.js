(function () {
    angular.module('CWCApp').
    controller('RentInvoiceCtrl', function ($scope, DSRRentInvoiceService) {

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
        debugger;
        $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        $scope.montharr = $scope.month[0];
        $scope.Yeararr = $scope.Year[0];
        $scope.taxvalue = "0";
        $scope.Amount = 0;
        $scope.GSTPer = 0;
        $scope.SacCode = '';
        $scope.CGST = 0;
        $scope.SGST = 0;
        $scope.IGST = 0;
        $scope.Round_Up = 0;
        $scope.TotalValue = 0;

        $scope.flagCharge = true;

        $scope.SelectParty = function (obj) {
            debugger;
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;          
            $scope.GSTPer = obj.GSTPer;
            $scope.SacCode = obj.SacCode;

            $scope.Amount = 0;
            $scope.CGST = 0;
            $scope.SGST = 0;
            $scope.IGST = 0;

            $('#hdnCGST').val(Number($scope.GSTPer)/2);
            $('#hdnSGST').val(Number($scope.GSTPer) / 2);
            $('#hdnIGST').val(0);
            $scope.Round_Up = 0;
            $scope.TotalValue = 0;
            $('#igst').val('0');
            $('#sgst').val('0');
            $('#cgst').val('0');
            $('#total').val('0');
            $('#round_up').val(0);
            
            $('#PartyModal').modal('hide');
        };

        $scope.PopulateRent = function (flg) {
            debugger;
            $scope.addeditflag = flg;
            var mon = $('#monthvalue').val();
            var yearv = $('#yearvalue').val();
            DSRRentInvoiceService.PopulateMonthYear(mon, yearv, flg).then(
                function (res) {
                    console.log(res.data);
                    debugger
                    if (res.data != "") {
                        $scope.AddRentDetails = res.data.lstRentDetails;
                        $scope.flagCharge = false;
                        $('#PartySearch').css('display', '');
                    }
                    else
                    {
                        alert('No record found');
                    }
                                        
                });
        };


        $scope.AddRent = function () {
            var detail = {};
            debugger;
            if ($('#amount').val() < '0')
            {
                $('#DivTblStuffingErrMsg').html('Negetive value can not be added');
                return false;
            }

            else if ($('#PartyName').val() == "") {
                $('#ErrParty').html('Fill Out This Field');
                return false;
            }
            else if ($('#amount').val() == "") {
                $('#Erramount').html('Fill Out This Field');
                return false;
            }
            else if ($('#total').val() == '0') {
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
                detail.SacCode = $scope.SacCode;
                detail.amount = $('#amount').val();
                detail.cgst = $('#cgst').val();
                detail.sgst = $('#sgst').val();
                detail.igst = $('#igst').val();
                detail.round_up = $('#round_up').val();
                detail.total = $('#total').val();
                detail.InvoiceNo = "0";
                detail.SEZ = $('#SEZ').val();
                var flag = 0;               
                    for (i = 0; i < $scope.AddRentDetails.length; i++) {
                        if ($scope.AddRentDetails[i].PartyId == detail.PartyId) {
                            flag = 1;
                            alert("Entry for this month already exists for party");//break;
                        }
                    }                
                
                if (flag == 0) {
                    $scope.flagCharge = false;
                    $scope.AddRentDetails.push(detail);


                    $scope.PartyName = "";
                    $scope.GSTNo = "";
                    $scope.Amount = 0;
                    $scope.CGST = 0;
                    $scope.SGST = 0;
                    $scope.IGST = 0;
                    $scope.Round_Up = 0;
                    $scope.TotalValue = 0;
                    $('#igst').val('0');
                    $('#sgst').val('0');
                    $('#cgst').val('0');
                    $('#round_up').val('0');
                    $('#total').val('0');
                    
                }
            }
        };




        $scope.ResetRent = function () {
            debugger;
            $scope.PartyId = 0;
            $scope.PartyName = '';
            $scope.hdnState = '';
            $scope.hdnStateCode = '';
            $scope.hdnAddress = '';
            $scope.GSTNo = '';
            $scope.GSTPer = '';
            $scope.SacCode = '';

            $('#amount,#cgst,#sgst,#igst,#total,#round_up').val('');
            $('#btnAddFormOneDet').prop("disabled", false);
        };

        $scope.EditRentDet = function (i) {
            debugger;
            $('#PartyId').val($scope.AddRentDetails[i].PartyId);
            $('#PartyName').val($scope.AddRentDetails[i].PartyName);
            $('#InvoiceDate').val($scope.AddRentDetails[i].Date);
            $('#GSTNo').val($scope.AddRentDetails[i].GSTNo);
            $('#SacCode').val($scope.AddRentDetails[i].SacCode);
            $('#amount').val($scope.AddRentDetails[i].amount);
            $('#cgst').val($scope.AddRentDetails[i].cgst);
            $('#sgst').val($scope.AddRentDetails[i].sgst);
            $('#igst').val($scope.AddRentDetails[i].igst);
            $('#round_up').val($scope.AddRentDetails[i].round_up);
            $('#total').val($scope.AddRentDetails[i].total);
            $('#SEZ').val($scope.AddRentDetails[i].SEZ);
            //$('#hdnCGST').val(Number($('#SacCode').val())/2);
            //$('#hdnSGST').val(Number($('#SacCode').val()) / 2);
            //$('#hdnIGST').val(0);
            $scope.AddRentDetails.splice(i, 1);
            $('#PartyName').prop("disabled", false);
            $('#PartySearch').css('display', '');
            $('#InvoiceDate').parent().find('img').css('display', '');
            $('#amount').prop("disabled", false);
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
            $('#SacCode').val($scope.AddRentDetails[i].SacCode);
            $('#amount').val($scope.AddRentDetails[i].amount);
            $('#cgst').val($scope.AddRentDetails[i].cgst);
            $('#sgst').val($scope.AddRentDetails[i].sgst);
            $('#igst').val($scope.AddRentDetails[i].igst);
            $('#round_up').val($scope.AddRentDetails[i].round_up);
            $('#total').val($scope.AddRentDetails[i].total);
            $('#SEZ').val($scope.AddRentDetails[i].SEZ);

            $('#PartyName').prop("disabled", true);           
            $('#PartySearch').css('display', 'none');
            $('#InvoiceDate').parent().find('img').css('display', 'none');
            $('#amount').prop("disabled", true); 
            $('#btnAddFormOneDet').prop("disabled", true);           
        };

        $scope.AllChargesLst = [];



        //$scope.AddChargeToRent = function () {
        //    debugger;

        //    var detailCharge = {};               
        //    detailCharge.ChargeName = $scope.ddlAddCharge.Text;
        //    detailCharge.ChargeHead = $scope.ddlAddCharge.Value;
        //    detailCharge.Date = $('#InvoiceDate').val();
        //    detailCharge.GSTNo = $scope.AddRentDetails[0].GSTNo;
        //    detailCharge.Address = $scope.AddRentDetails[0].PartyName;
        //    detailCharge.State = $scope.AddRentDetails[0].State;
        //    detailCharge.StateCode = $scope.AddRentDetails[0].StateCode;
        //    detailCharge.amount = $scope.txtAddChargeAmount;
        //    detailCharge.cgst = ($scope.txtAddChargeAmount * 9) / 100;
        //    detailCharge.sgst = ($scope.txtAddChargeAmount * 9) / 100;
        //    detailCharge.igst = 0;
        //    detailCharge.round_up = 0;
        //    detailCharge.total = ($scope.txtAddChargeAmount + detailCharge.cgst + detailCharge.sgst).toFixed(2);


        //    $scope.AllChargesLst.push(detailCharge);
        //    $scope.txtAddChargeAmount = 0;
        //    $scope.hdnlstCharge = JSON.parse($('#hdnlstCharge').val());

        //}

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
        $scope.PartyInv = [];
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

                    $scope.InvoiceObj.InvoiceId = 0;
                    $scope.InvoiceObj.InvoiceNo = "";
                    $scope.InvoiceObj.addeditflg = $scope.addeditflag;
                    
                    var arrayRentDetails = $scope.AddRentDetails.filter(function (finaldata) {
                        return finaldata.InvoiceNo == 0;
                    });
                    $scope.InvoiceObj.lstRentDetails = arrayRentDetails;
                    //$scope.InvoiceObj.SEZ = $('#SEZ').val();
                    //$scope.InvoiceObj.lstRentDetails = $scope.AddRentDetails;
                    $scope.InvoiceObj.lstRentInvoiceCharge = $scope.AllChargesLst;
                    var mon = $('#monthvalue').val();
                    var yearv = $('#yearvalue').val();
                    DSRRentInvoiceService.GenerateInvoice($scope.InvoiceObj, mon, yearv, $scope.AllChargesLst).then(function (res) {
                        console.log(res);
                        debugger;
                                               
                        //$scope.Message = res.data.Message;
                        //$scope.invoice = res.data.Data.InvoiceNo;
                        //$('#btnSave').attr("disabled", true);
                        $scope.CallInvoiceNo = [];
                        if (res.data.Status == 1) {
                            $scope.PartyInv = JSON.parse(res.data.Data.InvoiceNo);
                            debugger;
                            for (i = 0; i < $scope.PartyInv.length; i++) {
                                for (j = 0; j < $scope.AddRentDetails.length ; j++) {
                                    if ($scope.AddRentDetails[j].PartyId == $scope.PartyInv[i].PartyId) {
                                        $scope.AddRentDetails[j].InvoiceNo = $scope.PartyInv[i].InvoiceNo;
                                        $scope.CallInvoiceNo.push({ 'InvoiceNo': $scope.PartyInv[i].InvoiceNo, 'SupplyType': $scope.PartyInv[i].SupplyType });

                                    }
                                }
                            }

                        //     $scope.CallInvoiceNo = [];
                        //$scope.InvoiceNo = res.data.Data.InvoiceNo;
                        //var InvoiceSpilt = $scope.InvoiceNo.split("-");
                      
                        //$scope.SupplyType = InvoiceSpilt[1];
                        //var arr = InvoiceSpilt[0].split(",");
                        //for (i = 0; i < $scope.AddRentDetails.length; i++) {
                        //    $scope.AddRentDetails[i].InvoiceNo = arr[i];
                        //    $scope.CallInvoiceNo.push({ 'InvoiceNo': arr[i] });
                        //}


                            $scope.Message = res.data.Message;
                            $scope.invoice = res.data.Data.InvoiceNo;
                            $('#btnSave').attr("disabled", true)
                            $('#btnPrint').removeAttr("disabled");
                            $('#BtnGenerateIRN').removeAttr("disabled");
                        }
                        else if (res.data.Status == -1) {
                            $scope.Message = res.data.Message;
                            $('#btnPrint').attr("disabled");
                        }
                        else if (res.data.Status == 4 || res.data.Status == 5 || res.data.Status == 3) {
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

            DSRRentInvoiceService.GeneratePrint($scope.AddRentDetails).then(function (res) {
                console.log(res);
               
            });

        };
        $scope.GenerateIRN = function () {
            for (var i = 0; i < $scope.CallInvoiceNo.length; i++) {
                DSRRentInvoiceService.GenerateIRNNo($scope.CallInvoiceNo[i].InvoiceNo, $scope.CallInvoiceNo[i].SupplyType).then(function (res) {

                    alert(res.data.Message);

                });
            }



        };
    });
})()


