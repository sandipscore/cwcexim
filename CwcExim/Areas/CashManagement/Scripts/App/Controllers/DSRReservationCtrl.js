(function () {
    angular.module('CWCApp').
    controller('DSRReservationCtrl', function ($scope, DSRReservationService) {
        var d = new Date();
        var n = d.getFullYear();
        $scope.MonthArray = ['--Select--', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        $scope.YearArray = [];
        $scope.InvDtls = [];
        $scope.EditIndex = -1;
        $scope.IsAddClicked = false;
        $scope.IsSaveClicked = false;
        $scope.IsSaveDone = false;
        $scope.PartyList = [];

        var resobj1 = new DSRReservationModel();
        $scope.resobj = resobj1;
        $scope.Message = '';


        $scope.YearArray.push('--Select--');
        for (i = n - 10; i <= n + 10 ;i++){
            $scope.YearArray.push(i);
        }

        $scope.Month = $scope.MonthArray[0];
        $scope.Year = $scope.YearArray[0];

        //$scope.GodownList = [{ GodownId: 0, GodownName: "--Select--", Uid: 0 }];

        //--------------------------------------------------------------------------------------------------------
        //$scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        //$scope.resobj.InvoiceDate = $('#hdnCurDate').val();
        //$scope.Rights = JSON.parse($("#hdnRights").val());
        //for (i = 0; i < JSON.parse($('#hdnGodownList').val()).length; i++) {
          //  $scope.GodownList.push(JSON.parse($('#hdnGodownList').val())[i]);
        //}
        //$scope.GodownList=JSON.parse($('#hdnGodownList').val());
        //$scope.resobj.GodownId=0;

        //$scope.SelectParty = function (p) {
        //    $scope.resobj.PartyId = p.PartyId;
        //    $scope.resobj.PartyName = p.PartyName;
        //    $scope.resobj.GstNo = p.GstNo;
        //    $scope.resobj.Address = p.Address;
        //    $scope.resobj.StateCode = p.StateCode;
        //    $scope.resobj.StateName = p.StateName;

        //    $scope.resobj.GF = 0;
        //    $scope.resobj.MF = 0;
        //    $scope.CalcSpace();
        //    $('#PartyModal').modal('hide');

        //}

        //$scope.AddEditInv = function () {
           
        //    $scope.IsAddClicked = true;
        //    if ($scope.resobj.PartyId == 0 || $scope.resobj.PartyId == '' || $scope.resobj.PartyId == null
        //       || $scope.resobj.InvoiceDate == 0 || $scope.resobj.InvoiceDate == '' || $scope.resobj.InvoiceDate == null
        //       || $scope.resobj.Amount == 0 || $scope.resobj.Amount == '' || $scope.resobj.Amount == null
        //       || $scope.resobj.Total == 0 || $scope.resobj.Total == '' || $scope.resobj.Total == null
        //       || $scope.resobj.InvoiceAmt == 0 || $scope.resobj.InvoiceAmt == '' || $scope.resobj.InvoiceAmt == null
        //       || $scope.resobj.GodownId == 0 || $scope.resobj.GodownId == '' || $scope.resobj.GodownId == null
        //       || $scope.resobj.TotalSpace == 0 || $scope.resobj.TotalSpace == '' || $scope.resobj.TotalSpace == null

        //        )
        //    {
        //        return false;
        //    }

            
                
        //        var d = $scope.resobj.InvoiceDate.split('/');
        //        var m = d[1];
        //        var y = d[2].split(' ')[0];
                
        //        if ($scope.InvDtls.length > 0) {
        //            var d2 = $scope.InvDtls[0].InvoiceDate.split('/');
        //            var m2 = d2[1];
        //            var y2 = d2[2].split(' ')[0];

        //            if (m != m2 || y != y2) {
        //                $('#errdiv').html('All Invoice Date must be of same month and year.');
        //                return false;
        //            }

        //        }


        //        for (i = 0; i < $scope.GodownList.length; i++) {
        //            if ($scope.resobj.GodownId == $scope.GodownList[i].GodownId) {
        //                $scope.resobj.GodownName = $scope.GodownList[i].GodownName;
        //            }
        //        }

        //        $scope.InvDtls.push($scope.resobj);
        //    /*}
        //    else {
        //        $scope.InvDtls[$scope.EditIndex] = $scope.resobj;
        //    }*/
        //    $scope.ResetInv();
            
        //}
        var PartyId = 0;
       
        $scope.CreateInvoice = function () {
            debugger;
            if ($scope.Month != null && $scope.Month != '' && $scope.Year != null && $scope.Year != '') {
                if ($scope.PartyId == undefined)
                {
                    $scope.PartyId = PartyId;
                }
                var InvoiceDate = $('#InvoiceDate').val();
                DSRReservationService.CreateResInvoices($scope.Month, $scope.Year, $scope.PartyId, 1, InvoiceDate, $('#SEZ').val()).then(function (res) {
                    //debugger;
                    if (res.data.Status == 1) {
                        $scope.InvDtls = res.data.Data;
                    }
                    else {
                        $scope.InvDtls = [];
                        alert(res.data.Message  + ' Found.');
                    }
                });
            }
        }


        $scope.ResetInv = function () {
            $('#errdiv').html('');
            $scope.resobj = new DSRReservationModel();
            $scope.resobj.InvoiceDate = $('#hdnCurDate').val();
            $scope.EditIndex = -1;
            $scope.IsAddClicked = false;
        }

        $scope.DeleteInv = function (i) {
            if (confirm('Are you sure to delete?')) {
                $scope.InvDtls.splice(i, 1);
            }
        }
        $scope.EditInv = function (i,obj) {
            $scope.EditIndex = i;            
            $scope.resobj = $scope.InvDtls[$scope.EditIndex];
            $scope.InvDtls.splice(i, 1);
        }
        //-----------------------------------------------------------------
        debugger;
        //$scope.Populate = function () {
        //    if ($scope.Month != null && $scope.Month != '' && $scope.Year != null && $scope.Year != '') {
        //        DSRReservationService.GetReservationInvoices($scope.Month, $scope.Year,2).then(function (res) {
        //            if (res.data.Status == 1) {
        //                $scope.InvDtls = res.data.Data;
        //            }
        //            else {
        //                $scope.InvDtls = [];
        //            }
        //        });
        //    }            
        //}

        //$scope.CopyPrev = function () {
        //    if ($scope.Month != null && $scope.Month != '' && $scope.Year != null && $scope.Year != '') {
        //        DSRReservationService.GetReservationInvoices($scope.Month, $scope.Year, 1).then(function (res) {
        //            if (res.data.Status == 1) {
        //                $scope.InvDtls = res.data.Data;
        //            }
        //            else {
        //                $scope.InvDtls = [];
        //            }
        //        });
        //    }
        //}

        $scope.FullReset = function () {
            $('#DivBody').load('/CashManagement/DSR_CashManagement/ReservationInvoice');
        }

        $scope.PartyInv=[];
        $scope.Save = function () {
            $scope.IsSaveClicked = true;
            if ($scope.InvDtls.length <= 0) {
                return false;
            }
            if ($scope.EditIndex >= 0) {
                $scope.Message = "Complete Update Operation Before Save.";
                return false;
            }

            if ($scope.Month == '--Select--' || $scope.Year == '--Select--') {
                $scope.Message = "Select Reservation Month & Year Before Save.";
                return false;
            }
           
            var Remarks = $('#Remarks').val();
            var InvoiceDate = $('#InvoiceDate').val();

            if ($scope.InvDtls.length > 0) {
                debugger;
                for (var i = 0; i < $scope.InvDtls.length; i++) {
                    $scope.InvDtls[i].Remarks = Remarks;
                    $scope.InvDtls[i].InvoiceDate = InvoiceDate;
                }
            }

            if (confirm('Are you sure to save?')) {
                $('#btnSave').attr('disabled', true);
                debugger;
                DSRReservationService.SaveGenerateInvoice($scope.InvDtls, $scope.Month, $scope.Year,$('#SEZ').val()).then(function (res) {
                    console.log(res.data);
                    debugger;
                    $scope.CallInvoiceNo = [];
                    if (res.data.Status == 1) {
                        $scope.IsSaveDone = true;
                        $scope.PartyInv = JSON.parse(res.data.Data);
                        $('#BtnGenerateIRN').removeAttr("disabled");
                        for (i = 0; i < $scope.PartyInv.length; i++) {
                            for (j = 0; j < $scope.InvDtls.length ; j++) {
                                if ($scope.InvDtls[j].PartyId == $scope.PartyInv[i].PartyId && $scope.InvDtls[j].GodownId == $scope.PartyInv[i].GodownId && $scope.InvDtls[j].ReservationId == $scope.PartyInv[i].ReservationId) {
                                    $scope.InvDtls[j].InvoiceNo = $scope.PartyInv[i].InvoiceNo;
                                    $scope.InvDtls[j].InvoiceId = $scope.PartyInv[i].InvoiceId;
                                    $scope.CallInvoiceNo.push({ 'InvoiceNo': $scope.PartyInv[i].InvoiceNo, 'SupplyType': $scope.PartyInv[i].SupplyType });
                                }
                            }
                        }
                        //debugger;
                        //DSRReservationService.GetReservationInvoices($scope.Month, $scope.Year, 2).then(function (res) {
                        //    if (res.data.Status == 1) {
                        //        $scope.InvDtls = res.data.Data;
                        //    }
                        //    else {
                        //        $scope.InvDtls = [];
                        //    }
                        //});

                    }
                    $scope.Message = res.data.Message;
                });
            }
        }
        //$scope.CalcSpace = function () {
        //    if ($('#gf').val() == '') {
        //        $scope.resobj.GF = 0;
        //    }
        //    if ($('#mf').val() == '') {
        //        $scope.resobj.MF = 0;
        //    }
        //    $scope.resobj.TotalSpace = (parseFloat($scope.resobj.GF) + parseFloat($scope.resobj.MF)).toFixed(2);
        //    $scope.resobj.Amount = Math.ceil(((parseFloat($scope.resobj.TotalSpace) * 11250) / 50)).toFixed(2);
        //    $scope.Calc();
        //}
        //$scope.Calc = function () {
        //    if ($('#cgstper').val() == '') {
        //        $scope.resobj.CGSTPer = 9;
        //    }
        //    if ($('#sgstper').val() == '') {
        //        $scope.resobj.SGSTPer = 9;
        //    }
        //    if ($('#igstper').val() == '') {
        //        $scope.resobj.IGSTPer = 0;
        //    }

        //    $scope.resobj.CGST = ((parseFloat($scope.resobj.Amount) *( parseFloat($scope.resobj.CGSTPer) + parseFloat($scope.resobj.SGSTPer))) / 100).toFixed(2);
        //    $scope.resobj.SGST = ((parseFloat($scope.resobj.Amount) * (parseFloat($scope.resobj.CGSTPer) + parseFloat($scope.resobj.SGSTPer))) / 100).toFixed(2);
        //    $scope.resobj.IGST = ((parseFloat($scope.resobj.Amount) * parseFloat($scope.resobj.IGSTPer)) / 100).toFixed(2);

        //    $scope.resobj.CGST = ((Math.ceil(parseFloat($scope.resobj.CGST)))/2).toFixed(2);
        //    $scope.resobj.SGST = ((Math.ceil(parseFloat($scope.resobj.SGST))) / 2).toFixed(2);
        //    $scope.resobj.IGST = Math.ceil(parseFloat($scope.resobj.IGST)).toFixed(2);

        //    $scope.resobj.Total = (parseFloat($scope.resobj.Amount) + parseFloat($scope.resobj.CGST) + parseFloat($scope.resobj.SGST) + parseFloat($scope.resobj.IGST)).toFixed(2);
        //    $scope.resobj.InvoiceAmt = Math.ceil(parseFloat($scope.resobj.Total));
        //    $scope.resobj.RoundOff = ($scope.resobj.InvoiceAmt - $scope.resobj.Total).toFixed(2);
        //    $scope.resobj.InvoiceAmt = $scope.resobj.InvoiceAmt.toFixed(2);
        //}

        $scope.InvoiceNoArray = function () {
            debugger;
            var x = '';
            for (j = 0; j < $scope.InvDtls.length ; j++) {
                
                if (j != $scope.InvDtls.length - 1) {
                    x = x + $scope.InvDtls[j].InvoiceNo+',';
                }
                else {
                    x = x + $scope.InvDtls[j].InvoiceNo;
                }
            }

            $scope.invoice = x;
            
            var m3 = $scope.Month;
            var y3 = $scope.Year;
            

           
            PrintInvoice2(m3, y3, x);
        }

        $scope.IsPrintDisable = function () {
            if ($scope.InvDtls.length <= 0) {
                return true;
            }
            else {
                
                var c = 0;
                for (i = 0; i < $scope.InvDtls.length; i++) {
                    if ($scope.InvDtls[i].InvoiceNo == '') {
                        c = c + 1;
                    }
                }

                if (c == 0) {
                    return false;
                }
                return true;
            }
        }

        $scope.ChangePartyList = function () {
            debugger;
            var Month = $('#drpMonth').val();
            var Year = $('#drpYear').val();

            if (Month != '' && Year > 0) {
                DSRReservationService.GetReservationParties($scope.Month, $scope.Year).then(function (res) {
                    console.log(res.data);
                    $scope.PartyList = res.data.Data;
                    debugger;
                });
            }

            
            
        }

        $scope.SelectParty = function (p) {
            $scope.PartyId = p.PartyId;
            $scope.PartyName = p.PartyName;        
            $('#PartyModal').modal('hide');

        }

        $scope.GenerateIRN = function () {
            for (var i = 0; i < $scope.CallInvoiceNo.length; i++) {
                DSRReservationService.GenerateIRNNo($scope.CallInvoiceNo[i].InvoiceNo, $scope.CallInvoiceNo[i].SupplyType).then(function (res) {

                    alert(res.data.Message);

                });
            }



        };

    });
})()