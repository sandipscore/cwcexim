(function () {
    angular.module('CWCApp').
    controller('ReservationCtrl', function ($scope, ReservationService) {
        var d = new Date();
        var n = d.getFullYear();
        $scope.MonthArray = ['--Select--', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        $scope.YearArray = [];
        $scope.InvDtls = [];
        $scope.EditIndex = -1;
        $scope.IsAddClicked = false;
        $scope.IsSaveClicked = false;
        $scope.IsSaveDone = false;
        $scope.IsLocalGST = 0;
        var resobj1 = new ReservationModel();
        $scope.resobj = resobj1;
        $scope.Message = '';


        $scope.YearArray.push('--Select--');
        for (i = n - 10; i <= n + 10 ;i++){
            $scope.YearArray.push(i);
        }

        $scope.Month = $scope.MonthArray[0];
        $scope.Year = $scope.YearArray[0];

        $scope.GodownList = [{ GodownId: 0, GodownName: "--Select--", Uid: 0 }];


        //--------------------------------------------------------------------------------------------------------
        $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        $scope.resobj.InvoiceDate = $('#hdnCurDate').val();
        $scope.Rights = JSON.parse($("#hdnRights").val());
        for (i = 0; i < JSON.parse($('#hdnGodownList').val()).length; i++) {
            $scope.GodownList.push(JSON.parse($('#hdnGodownList').val())[i]);
        }
        //$scope.GodownList=JSON.parse($('#hdnGodownList').val());
        $scope.resobj.GodownId=0;

        $scope.SelectParty = function (p) {
            debugger;
            $scope.resobj.PartyId = p.PartyId;
            $scope.resobj.PartyName = p.PartyName;
            $scope.resobj.GstNo = p.GstNo;
            $scope.resobj.Address = p.Address;
            $scope.resobj.StateCode = p.StateCode;
            $scope.resobj.StateName = p.StateName;

            //Find GST
            if (p.StateCode == p.ComStateCode)
            {
                $scope.IsLocalGST = 1;
                $scope.resobj.CGSTPer = 9;
                $scope.resobj.SGSTPer = 9;
                $scope.resobj.IGSTPer = 0;


            }
            else
            {
                $scope.IsLocalGST = 0;
                $scope.resobj.CGSTPer = 0;
                $scope.resobj.SGSTPer =0;
                $scope.resobj.IGSTPer = 18;

            }


            if (p.StateCode==null || p.StateCode=='') 
            {
                $scope.IsLocalGST = 1;
                $scope.resobj.CGSTPer = 9;
                $scope.resobj.SGSTPer = 9;
                $scope.resobj.IGSTPer = 0;
            }
           
            if ($('#SEZ').val() == 'SEZWP')
            {
                $scope.IsLocalGST = 0;
                $scope.resobj.CGSTPer = 0;
                $scope.resobj.SGSTPer = 0;
                $scope.resobj.IGSTPer = 18;
            }
            else if ($('#SEZ').val() == 'SEZWOP') {
                $scope.IsLocalGST = 0;
                $scope.resobj.CGSTPer = 0;
                $scope.resobj.SGSTPer = 0;
                $scope.resobj.IGSTPer = 0;
            }


            $scope.resobj.GF = 0;
            $scope.resobj.MF = 0;
            $scope.CalcSpace();
            $('#PartyModal').modal('hide');
            $('#SEZ').prop('disabled',true);

        }


        //$('#SEZ').change(function () {
        //    debugger;
        //    if($('#SEZ').val()=='SEZWP')
        //    {
        //        $scope.resobj.CGSTPer = 0;
        //        $scope.resobj.SGSTPer = 0;
        //        $scope.resobj.IGSTPer = 18;
        //        $scope.Calc();
        //    }
        //    else if($('#SEZ').val()=='SEZWOP')
        //    {
        //        $scope.resobj.CGSTPer = 0;
        //        $scope.resobj.SGSTPer = 0;
        //        $scope.resobj.IGSTPer = 0;
        //        $scope.Calc();
        //    }
        //});

        $scope.AddEditInv = function () {
            debugger;
            $scope.IsAddClicked = true;
            if ($scope.resobj.PartyId == 0 || $scope.resobj.PartyId == '' || $scope.resobj.PartyId == null
               || $scope.resobj.InvoiceDate == 0 || $scope.resobj.InvoiceDate == '' || $scope.resobj.InvoiceDate == null
               || $scope.resobj.Amount == 0 || $scope.resobj.Amount == '' || $scope.resobj.Amount == null
               || $scope.resobj.Total == 0 || $scope.resobj.Total == '' || $scope.resobj.Total == null
               || $scope.resobj.InvoiceAmt == 0 || $scope.resobj.InvoiceAmt == '' || $scope.resobj.InvoiceAmt == null
               || $scope.resobj.GodownId == 0 || $scope.resobj.GodownId == '' || $scope.resobj.GodownId == null
               || $scope.resobj.TotalSpace == 0 || $scope.resobj.TotalSpace == '' || $scope.resobj.TotalSpace == null

                )
            {
                return false;
            }

            //if ($scope.EditIndex < 0) {
                if ($scope.InvDtls.filter(function (elem) { return elem.PartyId == $scope.resobj.PartyId }).length > 0) {
                    $('#errdiv').html('Party Already Exists.');
                    return false;
                }
                
                var d = $scope.resobj.InvoiceDate.split('/');
                var m = d[1];
                var y = d[2].split(' ')[0];
                //commented for the validation
                /*if ($scope.InvDtls.length > 0) {
                    var d2 = $scope.InvDtls[0].InvoiceDate.split('/');
                    var m2 = d2[1];
                    var y2 = d2[2].split(' ')[0];

                    if (m != m2 || y != y2) {
                        $('#errdiv').html('All Invoice Date must be of same month and year.');
                        return false;
                    }

                }*/


                for (i = 0; i < $scope.GodownList.length; i++) {
                    if ($scope.resobj.GodownId == $scope.GodownList[i].GodownId) {
                        $scope.resobj.GodownName = $scope.GodownList[i].GodownName;
                    }
                }
                $scope.resobj.SEZ = $('#SEZ').val();
                $scope.InvDtls.push($scope.resobj);
            /*}
            else {
                $scope.InvDtls[$scope.EditIndex] = $scope.resobj;
            }*/
                $scope.ResetInv();
                $('#SEZ').prop('disabled', false);
            
        }

        $scope.ResetInv = function () {
            $('#errdiv').html('');
            $scope.resobj = new ReservationModel();
            $scope.resobj.InvoiceDate = $('#hdnCurDate').val();
            $scope.EditIndex = -1;
            $scope.IsAddClicked = false;
            $('#SEZ').val('');
        }

        $scope.DeleteInv = function (i) {
            if (confirm('Are you sure to delete?')) {
                $scope.InvDtls.splice(i, 1);
            }
        }
        $scope.EditInv = function (i,obj) {
            $scope.EditIndex = i;            
            $scope.resobj = $scope.InvDtls[$scope.EditIndex];
            $('#SEZ').val($scope.InvDtls[$scope.EditIndex].SEZ);
            $scope.InvDtls.splice(i, 1);
            $('#SEZ').prop('disabled', true);
        }
        //-----------------------------------------------------------------

        $scope.Populate = function () {
            if ($scope.Month != null && $scope.Month != '' && $scope.Year != null && $scope.Year != '') {
                ReservationService.GetReservationInvoices($scope.Month, $scope.Year,2).then(function (res) {
                    if (res.data.Status == 1) {
                        $scope.InvDtls = res.data.Data;
                    }
                    else {
                        $scope.InvDtls = [];
                    }
                });
            }
        }

        $scope.CopyPrev = function () {
            if ($scope.Month != null && $scope.Month != '' && $scope.Year != null && $scope.Year != '') {
                ReservationService.GetReservationInvoices($scope.Month, $scope.Year, 1).then(function (res) {
                    if (res.data.Status == 1) {
                        $scope.InvDtls = res.data.Data;
                    }
                    else {
                        $scope.InvDtls = [];
                    }
                });
            }
        }

        $scope.FullReset = function () {
            $('#DivBody').load('/CashManagement/Ppg_CashManagement/ReservationInvoice');
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

            if (confirm('Are you sure to save?')) {
                $('#btnSave').attr('disabled', true);
                ReservationService.GenerateInvoice($scope.InvDtls, $scope.Month, $scope.Year).then(function (res) {
                    debugger;
                    console.log(res.data);
                    if (res.data.Status == 1) {
                        debugger;
                        $scope.IsSaveDone = true;
                        $scope.PartyInv = JSON.parse(res.data.Data);
                        $('#BtnGenerateIRN').removeAttr("disabled");

                        for (i = 0; i < $scope.PartyInv.length; i++) {
                            for (j = 0; j < $scope.InvDtls.length ; j++) {
                                if ($scope.InvDtls[j].PartyId == $scope.PartyInv[i].PartyId) {
                                    $scope.InvDtls[j].InvoiceNo = $scope.PartyInv[i].InvoiceNo;
                                    $scope.InvDtls[j].InvoiceId = $scope.PartyInv[i].InvoiceId;
                                    $scope.InvDtls[j].InvoiceId = $scope.PartyInv[i].InvoiceId;
                                    $scope.InvDtls[j].SupplyType = $scope.PartyInv[i].SupplyType;
                                }
                            }
                        }


                    }
                    $scope.Message = res.data.Message;
                });
            }
        }
        $scope.CalcSpace = function () {
            if ($('#gf').val() == '') {
                $scope.resobj.GF = 0;
            }
            if ($('#mf').val() == '') {
                $scope.resobj.MF = 0;
            }
            $scope.resobj.TotalSpace = (parseFloat($scope.resobj.GF) + parseFloat($scope.resobj.MF)).toFixed(2);
            $scope.resobj.Amount = Math.ceil(((parseFloat($scope.resobj.TotalSpace) * 11250) / 50)).toFixed(2);
            $scope.Calc();
        }
        $scope.Calc = function () {

            debugger;
            /*
            if ($('#cgstper').val() == '') {
                $scope.resobj.CGSTPer = 9;
            }
            if ($('#sgstper').val() == '') {
                $scope.resobj.SGSTPer = 9;
            }
            if ($('#igstper').val() == '') {
                $scope.resobj.IGSTPer = 0;
            }
            */

            $scope.resobj.CGST = ((parseFloat($scope.resobj.Amount) *( parseFloat($scope.resobj.CGSTPer) + parseFloat($scope.resobj.SGSTPer))) / 100).toFixed(2);
            $scope.resobj.SGST = ((parseFloat($scope.resobj.Amount) * (parseFloat($scope.resobj.CGSTPer) + parseFloat($scope.resobj.SGSTPer))) / 100).toFixed(2);
            $scope.resobj.IGST = ((parseFloat($scope.resobj.Amount) * parseFloat($scope.resobj.IGSTPer)) / 100).toFixed(2);

            $scope.resobj.CGST = ((Math.ceil(parseFloat($scope.resobj.CGST)))/2).toFixed(2);
            $scope.resobj.SGST = ((Math.ceil(parseFloat($scope.resobj.SGST))) / 2).toFixed(2);
            $scope.resobj.IGST = Math.ceil(parseFloat($scope.resobj.IGST)).toFixed(2);

            $scope.resobj.Total = (parseFloat($scope.resobj.Amount) + parseFloat($scope.resobj.CGST) + parseFloat($scope.resobj.SGST) + parseFloat($scope.resobj.IGST)).toFixed(2);
            $scope.resobj.InvoiceAmt = Math.ceil(parseFloat($scope.resobj.Total));
            $scope.resobj.RoundOff = ($scope.resobj.InvoiceAmt - $scope.resobj.Total).toFixed(2);
            $scope.resobj.InvoiceAmt = $scope.resobj.InvoiceAmt.toFixed(2);
           // $("#Total").trigger("click");
        }

        $scope.InvoiceNoArray = function () {
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
            /*
            var d3 = $scope.InvDtls[0].InvoiceDate.split('/');
            var m3 = d3[1];
            var y3 = d3[2].split(' ')[0];
            */
            var m3 = $scope.Month;
            var y3 = $scope.Year;
            

            //PrintInvoice(0);
            PrintInvoice2(m3,y3);
        }


        $scope.GenerateIRN = function () {
            debugger;
            $('.modalloader').show();
            var str = "";
            for (j = 0; j < $scope.InvDtls.length ; j++) {
                ReservationService.GenerateIRNNo($scope.InvDtls[j].InvoiceNo, $scope.InvDtls[j].SupplyType).then(function (res) {
                    
                    str=res.data.Message;
                    $('.modalloader').hide();
                    alert(str);
                });
            }

           
        };

   


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

    });



   


})()