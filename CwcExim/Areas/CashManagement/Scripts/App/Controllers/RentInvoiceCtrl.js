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
        for (i = 0; i < 20; i++)
        {
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
            $('#total').val('0');
            if ($scope.logic3 == 1)
            {
                if($scope.logic1==1)
                {
                    $scope.taxvalue = "1";
                }
                else
                {
                    $scope.taxvalue = "1";

                }
            }
            else
            {
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
            RentInvoiceService.PopulateMonthYear(mon, yearv, flg, $scope.PartyId).then(
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
            else if($('#amount').val() == "")
            {
                $('#Erramount').html('Fill Out This Field');
                return false;
            }
            else if ($('#total').val() == '0')
            {
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
                detail.InvoiceNo = "0";
                var flag = 0;
                for (i = 0; i < $scope.AddRentDetails.length; i++)
                {
                    if ($scope.AddRentDetails[i].PartyId == detail.PartyId)
                    {
                        flag = 1;
                        break;
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
                    $('#total').val('0');
                }
            }
        };




        $scope.ResetRent = function () {

            $('#PartyName, #amount,#cgst,#sgst,#igst,#total,#round_up,#GSTNo').val('');
            $('#btnAddFormOneDet').prop("disabled", false);
        };

        $scope.EditRentDet = function (i) {
            debugger;
            $('#PartyId').val($scope.AddRentDetails[i].PartyId);
            $('#PartyName').val( $scope.AddRentDetails[i].PartyName) ;
            $('#InvoiceDate').val($scope.AddRentDetails[i].Date) ;
            $('#GSTNo').val($scope.AddRentDetails[i].GSTNo)
           // detail.Address = $('#hdnAddress').val();
           // detail.State = $('#hdnState').val();
           // detail.StateCode = $('#hdnStateCode').val();
            $('#amount').val($scope.AddRentDetails[i].amount);
            $('#cgst').val($scope.AddRentDetails[i].cgst)  ;
            $('#sgst').val($scope.AddRentDetails[i].sgst)  ;
            $('#igst').val($scope.AddRentDetails[i].igst);
            $('#round_up').val($scope.AddRentDetails[i].round_up) ;
            $('#total').val($scope.AddRentDetails[i].total) ;
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
            $('#amount').val($scope.AddRentDetails[i].amount);
            $('#cgst').val($scope.AddRentDetails[i].cgst);
            $('#sgst').val($scope.AddRentDetails[i].sgst);
            $('#igst').val($scope.AddRentDetails[i].igst);
            $('#round_up').val($scope.AddRentDetails[i].round_up);
            $('#total').val($scope.AddRentDetails[i].total);
            $('#btnAddFormOneDet').prop("disabled", true);
           // $scope.AddRentDetails.splice(i, 1);
        };

        $scope.AllChargesLst = [];

       

        $scope.AddChargeToRent=function()
        {
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
            if ($('#SEZ').val() == 'SEZWP')
            {
                detailCharge.cgst = 0;
                detailCharge.sgst = 0;
            }
            else if ($('#SEZ').val() == 'SEZWOP')
            {
                detailCharge.cgst = 0;
                detailCharge.sgst = 0;
            }
            else
            {
                detailCharge.cgst = ($scope.txtAddChargeAmount * 9) / 100;
                detailCharge.sgst = ($scope.txtAddChargeAmount * 9) / 100;
            }
            

            detailCharge.igst = 0;
            detailCharge.round_up =0;
            detailCharge.total = ($scope.txtAddChargeAmount + detailCharge.cgst + detailCharge.sgst).toFixed(2);
           

            $scope.AllChargesLst.push(detailCharge);
            $scope.txtAddChargeAmount = 0;
            $scope.hdnlstCharge = JSON.parse($('#hdnlstCharge').val());
          
        }

        $scope.DeleteRentCharge = function (i)
        {
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

        $scope.hdnlstCharge =JSON.parse($('#hdnlstCharge').val());
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
                    
                    $scope.InvoiceObj.InvoiceId = 0;
                    $scope.InvoiceObj.InvoiceNo = "";
                    $scope.InvoiceObj.addeditflg = $scope.addeditflag;
                    $scope.InvoiceObj.lstPrePaymentCont = $scope.AddRentDetails;
                    $scope.InvoiceObj.lstPpgRentInvoiceCharge = $scope.AllChargesLst;
                    $scope.InvoiceObj.SEZ = $('#SEZ').val();
                    var mon = $('#monthvalue').val();
                    var yearv = $('#yearvalue').val();
                    RentInvoiceService.GenerateInvoice($scope.InvoiceObj, mon, yearv, $scope.AllChargesLst).then(function (res) {
                        console.log(res);
                        if (res.data.Status == 1)
                        {
                            $scope.InvoiceNo = res.data.Data.InvoiceNo;
                            var arr = $scope.InvoiceNo.split(",");
                            for (i = 0; i < $scope.AddRentDetails.length; i++) {
                                $scope.AddRentDetails[i].InvoiceNo = arr[i].split('-')[0];
                            }
                        }
                       
                        $scope.Message = res.data.Message;
                        $scope.invoice = res.data.Data.InvoiceNo;
                        $('#btnSave').attr("disabled", true);
                        //$('#SEZ').prop('disabled', false)
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
       

        $scope.GenerateIRN = function () {
            debugger;
            $('.modalloader').show();
            var str = "";

           
            var arr = $scope.InvoiceNo.split(",");
            for (j = 0; j < arr.length ; j++) {
                var arrsup = arr[j].split('-');
                RentInvoiceService.GenerateIRNNo(arrsup[0], arrsup[1]).then(function (res) {

                    str = res.data.Message;
                    $('.modalloader').hide();
                    alert(str);
                });
            }


        };


        $scope.PrintInvoice = function () {

            RentInvoiceService.GeneratePrint($scope.AddRentDetails).then(function (res) {
                console.log(res);
               
               // $('#btnSave').attr("disabled", true);
               // $('#btnPrint').removeAttr("disabled");
            });


           
     


        };


    });
})()


