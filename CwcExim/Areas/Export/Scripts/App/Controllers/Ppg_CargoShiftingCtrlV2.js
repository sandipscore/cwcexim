(function () {
    angular.module('CWCApp').
    controller('CargoShiftingCtrl', function ($scope, CargoShiftingService) {
        $scope.InvoiceNo = "";
        $scope.shipbills = [];
        $scope.ShippingLine = [];
        $scope.ShippingLineT = [];
        $scope.SHPCode = '';
        $scope.SHPCodeT = '';
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.IsCalculate = false;
        $scope.CheckAll = false;
        $scope.disMove = false;
        $scope.IsShipping = false;
        $scope.IsGodown = false;
        $scope.ShiftingTypeList = [
         {
             "Id": '1',
             "ShiftingType": "Shipping Line To Shipping Line",
         },
          {
              "Id": '2',
              "ShiftingType": "Godown To Godown"
          },
          {
              "Id": '3',
              "ShiftingType": "Both"
          }
        ];
        $scope.onShiftingChange = function () {
            debugger;
            $scope.ShippingLineNameF = '';
            $scope.ShippingLineIdF = 0;
            $scope.ShippingLineNameT = '';
            $scope.ShippingLineIdT = 0;
            $scope.PartyName = '';
            $scope.PartyId = 0;
            $scope.GSTNo = '';
            $scope.GodownNameF = '';
            $scope.GodownIdF = 0;
            $scope.GodownNameT = '';
            $scope.GodownIdT = 0;
            $scope.shipbills = [];
            $scope.InvoiceObj = [];
            $scope.IsCalculate = false;
            $scope.CheckAll = false;
            $scope.disMove = false;
            $scope.Message = '';
            $scope.InvoiceNo = "";
            $scope.ShiftingNo = '';
            if ($scope.ShiftingType == 1) {
                $scope.IsShipping = true;
                $scope.IsGodown = false;
            }
            else if ($scope.ShiftingType == 2) {
                $scope.IsShipping = false;
                $scope.IsGodown = true;
            }
            else if ($scope.ShiftingType == 3) {
                $scope.IsShipping = true;
                $scope.IsGodown = true;
            }
            //else {
            //    $scope.IsShipping = false;
            //    $scope.IsGodown = false;
            //}
        }
        //------------------------------------------------------------------------------------
        if ($('#hdnShippingLine').val() != null && $('#hdnShippingLine').val() != '') {
            $scope.ShippingLines = JSON.parse($('#hdnShippingLine').val());
        }

        if ($('#hdnPartyPayee').val() != null && $('#hdnPartyPayee').val() != '') {
            $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        }

        if ($('#hdnGodownList').val() != null && $('#hdnGodownList').val() != '') {
            $scope.Godowns = JSON.parse($('#hdnGodownList').val());
        }
        if ($('#hdnGodownListF').val() != null && $('#hdnGodownListF').val() != '') {
            $scope.GodownsF = JSON.parse($('#hdnGodownListF').val());
        }
        $scope.ShiftngDate = $('#hdnDate').val()
        //-----------------------------------------------------------------------------------

        $scope.ShippingLineNameF = '';
        $scope.ShippingLineIdF = 0;


        $scope.ShippingLineNameT = '';
        $scope.ShippingLineIdT = 0;

        $scope.PartyName = '';
        $scope.PartyId = 0;
        $scope.GSTNo = '';

        $scope.GodownNameF = '';
        $scope.GodownIdF = 0;

        $scope.GodownNameT = '';
        $scope.GodownIdT = 0;

        $scope.SelectGodownF = function (obj) {
            $scope.GodownNameF = obj.GodownName;
            $scope.GodownIdF = obj.GodownId;
            $scope.GetShipBillDetails();
            $('#FrmGodownModal').modal('hide');
        }
        $scope.SelectGodownT = function (obj) {
            $scope.GodownNameT = obj.GodownName;
            $scope.GodownIdT = obj.GodownId;
            $('#ToGodownModal').modal('hide');
        }

        $scope.SelectShippingLineF = function (obj)
        {
            debugger;
            //$scope.ShippingLineNameF = obj.PartyName;
            //$scope.ShippingLineIdF = obj.PartyId;

            $scope.ShippingLineNameF = obj.EximTraderName;
            $scope.ShippingLineIdF = obj.EximTraderId;

            //$scope.PartyName = obj.PartyName;
            //$scope.PartyId = obj.PartyId;
            //$scope.GSTNo = obj.GSTNo;
            $scope.PartyName = '';
            $scope.PartyId = 0;
            $scope.GSTNo = '';

            $scope.GetShipBillDetails();
            //$('#FromShippingModal').modal('hide');
            $scope.CloseSHP();
        }

        $scope.SelectShippingLineT = function (obj) {
            $scope.ShippingLineNameT = obj.EximTraderName;
            $scope.ShippingLineIdT = obj.EximTraderId;

            /*$scope.PartyName = obj.PartyName;
            $scope.PartyId = obj.PartyId;
            $scope.GSTNo = obj.GSTNo;*/
            //$('#ToShippingModal').modal('hide');
            $scope.CloseSHPT();
        }

        $scope.SelectParty = function (obj) {
            debugger;
            $scope.PartyName = obj.PartyName;
            $scope.PartyId = obj.PartyId;
            $scope.GSTNo = obj.GSTNo;
            $('#PartyModal').modal('hide');

        }

        $scope.GetShipBillDetails = function () {
            debugger;
            if ($scope.ShiftingType == 1 && $scope.ShippingLineIdF > 0) {
                CargoShiftingService.GetShipBillDetails($scope.ShippingLineIdF, $scope.ShiftingType, $scope.GodownIdF).then(function (res) {
                    if (res.data == 0) {
                        $scope.shipbills = [];
                        $scope.InvoiceObj = [];
                        $scope.IsCalculate = false;
                        alert("No Ship Bill Detail Found");
                        return false;
                    }
                    else {
                        $scope.shipbills = res.data;
                    }
                    //console.log($scope.shipbills);
                });
            }
            else if ($scope.ShiftingType == 2 && $scope.GodownIdF > 0) {
                CargoShiftingService.GetShipBillDetails($scope.ShippingLineIdF, $scope.ShiftingType, $scope.GodownIdF).then(function (res) {
                    if (res.data == 0) {
                        $scope.shipbills = [];
                        $scope.InvoiceObj = [];
                        $scope.IsCalculate = false;
                        alert("No Ship Bill Detail Found");
                        return false;
                    }
                    else {
                        $scope.shipbills = res.data;
                    }
                    //console.log($scope.shipbills);
                });
            }
            else if ($scope.ShiftingType == 3 && $scope.ShippingLineIdF > 0 && $scope.GodownIdF > 0) {
                CargoShiftingService.GetShipBillDetails($scope.ShippingLineIdF, $scope.ShiftingType, $scope.GodownIdF).then(function (res) {
                    if (res.data == 0) {
                        $scope.shipbills = [];
                        $scope.InvoiceObj = [];
                        $scope.IsCalculate = false;
                        alert("No Ship Bill Detail Found");
                        return false;
                    }
                    else {
                        $scope.shipbills = res.data;
                    }
                    //console.log($scope.shipbills);
                });
            }
        }

        $scope.GetCargoShiftingInvoice = function () {
            debugger
            $("#ShiftngDate").datepicker("option", "disabled", true);
            var shipbills = $scope.shipbills.filter(x=>x.IsChecked == true);
            CargoShiftingService.GetCargoShiftingInvoice(0, $scope.ShiftngDate, $scope.ShippingLineIdF, TaxType, shipbills, $scope.PartyId).then(function (res) {
                $scope.IsCalculate = true;
                $scope.InvoiceObj = res.data;
                $("#ShiftngDate").datepicker("option", "disabled", true);
                $scope.ShiftngDate = $scope.InvoiceObj.InvoiceDate;
                //$scope.IsContSelected = true;
                //console.log($scope.InvoiceObj);
                //console.log(res.data);
                /*if ($scope.Rights.CanAdd == 1) {
                    $('#btnSave').removeAttr("disabled");
                }
                $('.search').css('display', 'none');
                $('#InvoiceDate').parent().find('img').css('display', 'none');
                */
                if ($scope.InvoiceObj.InvoiceAmt <= 0 || $scope.InvoiceObj.InvoiceAmt == null) {
                    $scope.disMove = true;
                }
            });
        }
        $scope.AddEditCargoShifting = function () {
            debugger;
            if ($scope.ShiftingType == '' || $scope.ShiftingType == undefined) {
                alert("Please Select Shifting Type");
                return false;
            }
            if ($scope.ShiftingType == 1 && $scope.ShippingLineIdF == 0) {
                alert("Please Select From Shipping Line");
                return false;
            }
            else if ($scope.ShiftingType == 1 && $scope.ShippingLineIdT == 0) {
                alert("Please Select To Shipping Line");
                return false;
            }
            else if ($scope.ShiftingType == 2 && $scope.GodownIdF == 0) {
                alert("Please Select To Godown");
                return false;
            }
            else if ($scope.ShiftingType == 2 && $scope.GodownIdT == 0) {
                alert("Please Select To Godown");
                return false;
            }
            else if ($scope.ShiftingType == 3 && ($scope.ShippingLineIdF == 0 || $scope.GodownIdF == 0 || $scope.ShippingLineIdT == 0 || $scope.GodownIdT == 0)) {
                alert("Please Select From & To Shipping Line & From & To Godown");
                return false;
            }
            //if ($scope.PartyId == 0) {
            //    alert("Please Select Party");
            //    return false;
            //}
            if (($scope.ShiftingType == 1 || $scope.ShiftingType == 3) && $scope.ShippingLineIdF == $scope.ShippingLineIdT) {
                alert("From & To Shipping Line should not be same");
                return false;
            }
            if (($scope.ShiftingType == 2 || $scope.ShiftingType == 3) && $scope.GodownIdF == $scope.GodownIdT) {
                alert("From & To Godown should not be same");
                return false;
            }
            if ($scope.shipbills.length == 0) {
                alert("Shipbill details should not empty");
                return false;
            }
            if ($scope.shipbills.filter(x=>x.IsChecked == true).length == 0) {
                alert("Select Shipbill details");
                return false;
            }
            //if ($scope.InvoiceObj == undefined) {
            //    alert("Without invoice details shifting is not possible");
            //    return false;
            //}
            $scope.InvoiceObj.PartyId = $scope.PartyId;
            $scope.InvoiceObj.PartyName = $scope.PartyName;
            $scope.InvoiceObj.PartyGST = $scope.GSTNo;
            var conf = confirm('Are you sure you want to Save?');
            if (conf == true) {
                $('#BtnMOveCargoShp').attr('disabled', true);

                var shipbills = $scope.shipbills.filter(x=>x.IsChecked == true);
                CargoShiftingService.AddEditCargoShifting($scope.InvoiceObj, shipbills, $scope.GodownIdF, $scope.GodownIdT, $scope.ShippingLineIdT, $scope.ShiftingType, $scope.ShippingLineIdF, $scope.ShiftngDate, $scope.Remarks, 0).then(function (res) {
                    $scope.Message = res.data.Message;
                    if (res.data.Data != null) {
                        $scope.ShiftingNo = res.data.Data.split(',')[1];
                        //$scope.InvoiceNo = res.data.Data.split(',')[0];
                        //$('#InvoiceNo').val($scope.InvoiceNo);
                        $scope.disMove = true;
                    }
                    setTimeout(function () { $('#DivBody').load('/Export/Ppg_CWCExportV2/CreateCargoShifting'); }, 5000);
                });
            }
        }
        $scope.CountShipBill = function () {
            return ((($scope.shipbills.filter(x=>x.IsChecked == true)).length) > 0 ? true : false);
        }
        $scope.CheckCheckBox = function () {
            if ($scope.CheckAll == true) {
                $.each($scope.shipbills, function (i, x) { x.IsChecked = true });
            }
            else {
                $.each($scope.shipbills, function (i, x) { x.IsChecked = false });
            }
        }
        $scope.UnCheckBox = function () {
            if ($scope.shipbills.filter(x=>x.IsChecked == true).length == $scope.shipbills.length)
                $scope.CheckAll = true;
            else $scope.CheckAll = false;
        }


        $scope.GetShippingLine = function () {

            debugger;
            CargoShiftingService.GetShippingLineforCargo().then(function (res) {
                debugger;

                $('#hdnShippingLine').val(res.data);
                $scope.ShippingLines = JSON.parse($('#hdnShippingLine').val());


            });

        };

        $scope.GetParty = function () {

            debugger;
            CargoShiftingService.GetPartyforCargo().then(function (res) {
                debugger;

                $('#hdnPartyPayee').val(res.data);
                $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());


            });

        };


        $scope.GetGodown = function () {

            debugger;
            CargoShiftingService.GetGodownforCargo().then(function (res) {
                debugger;

                $('#hdnGodownListF').val(res.data);
                $scope.GodownsF = JSON.parse($('#hdnGodownListF').val());


            });

        };


        $scope.GetGodownTo = function () {

            debugger;
            CargoShiftingService.GetGodownforToCargo().then(function (res) {
                debugger;

                $('#hdnGodownList').val(res.data);
                $scope.Godowns = JSON.parse($('#hdnGodownList').val());


            });

        };


        //--------------------------------------------------------
        //For Shipping line modal
        var SHPPage = -1;
        $scope.CloseSHP = function () {
            SHPPage = -1;
            $scope.ShippingLine = [];
            $scope.SHPCode = '';
            $("#FromShippingModal").modal("hide");
        }

        $scope.LoadMoreSHP = function () {
            debugger;
            SHPPage++;
            CargoShiftingService.GetShippingLineLazyLoad(SHPPage, $scope.SHPCode).then(function (res)
            {
                res = res.data;
                if (res.Status == 1) {
                    if ($scope.ShippingLine.length > 0) {
                        $.each(res.Data.LstParty, function (item, elem) {
                            $scope.ShippingLine.push(elem);
                        });
                    }

                    else $scope.ShippingLine = res.Data.LstParty;

                    if (res.Data.State == true)
                        $('#btnSHP').prop('disabled', false);
                    else $('#btnSHP').prop('disabled', true);
                }
            });
        }
        $scope.SearchSHP = function ()
        {
            SHPPage = -1;
            CargoShiftingService.GetShippingLineLazyLoad(SHPPage, $scope.SHPCode).then(function (res)
            {
                res = res.data;
                if (res.Status == 1)
                {
                    $scope.ShippingLine = res.Data.LstParty;

                    if (res.Data.State == true)
                        $('#btnSHP').prop('disabled', false);
                    else $('#btnSHP').prop('disabled', true);
                }
            });
        }

        var SHPPageT = -1;
        $scope.CloseSHPT = function () {
            SHPPageT = -1;
            $scope.ShippingLineT = [];
            $scope.SHPCodeT = '';
            $("#ToShippingModal").modal("hide");
        }

        $scope.LoadMoreSHPT = function () {
            SHPPageT++;
            CargoShiftingService.GetShippingLineLazyLoad(SHPPageT, $scope.SHPCodeT).then(function (res) {
                res = res.data;
                if (res.Status == 1) {
                    if ($scope.ShippingLineT.length > 0) {
                        $.each(res.Data.LstParty, function (item, elem) {
                            $scope.ShippingLineT.push(elem);
                        });
                    }

                    else $scope.ShippingLineT = res.Data.LstParty;

                    if (res.Data.State == true)
                        $('#btnSHPT').prop('disabled', false);
                    else $('#btnSHPT').prop('disabled', true);
                }
            });
        }
        $scope.SearchSHPT = function () {
            SHPPageT = -1;
            CargoShiftingService.GetShippingLineLazyLoad(SHPPageT, $scope.SHPCodeT).then(function (res) {
                res = res.data;
                if (res.Status == 1) {
                    $scope.ShippingLineT = res.Data.LstParty;

                    if (res.Data.State == true)
                        $('#btnSHPT').prop('disabled', false);
                    else $('#btnSHPT').prop('disabled', true);
                }
            });
        }
    });
})()