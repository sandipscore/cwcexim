(function () {
    angular.module('CWCApp').
    controller('EditCargoShiftingCtrl', function ($scope, EditCargoShiftingService) {
        $scope.InvoiceId = 0;
        $scope.InvoiceNo = "";
        $scope.shipbills = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.IsCalculate = false;
        $scope.CheckAll = false;

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

        if ($('#hdnCS').val() != null && $('#hdnCS').val() != '') {
            $scope.ShiftingNoList = JSON.parse($('#hdnCS').val());
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
            $('#FrmGodownModal').modal('hide');
        }
        $scope.SelectGodownT = function (obj) {
            $scope.GodownNameT = obj.GodownName;
            $scope.GodownIdT = obj.GodownId;
            $('#ToGodownModal').modal('hide');
        }

        $scope.SelectShippingLineF = function (obj) {
            $scope.ShippingLineNameF = obj.PartyName;
            $scope.ShippingLineIdF = obj.PartyId;

            $scope.PartyName = obj.PartyName;
            $scope.PartyId = obj.PartyId;
            $scope.GSTNo = obj.GSTNo;

            $scope.GetShipBillDetails();
            $('#FromShippingModal').modal('hide');
        }

        $scope.SelectShippingLineT = function (obj) {
            $scope.ShippingLineNameT = obj.PartyName;
            $scope.ShippingLineIdT = obj.PartyId;

            /*$scope.PartyName = obj.PartyName;
            $scope.PartyId = obj.PartyId;
            $scope.GSTNo = obj.GSTNo;*/
            $('#ToShippingModal').modal('hide');
        }

        $scope.SelectParty = function (obj) {
            $scope.PartyName = obj.PartyName;
            $scope.PartyId = obj.PartyId;
            $scope.GSTNo = obj.GSTNo;
            $('#PartyModal').modal('hide');

        }

        $scope.GetShipBillDetails = function () {
            EditCargoShiftingService.GetShipBillDetails($scope.ShippingLineIdF).then(function (res) {
                $scope.shipbills = res.data;
                console.log($scope.shipbills);
            });
        }

        $scope.GetCargoShiftingInvoice = function () {
            EditCargoShiftingService.GetCargoShiftingInvoice($scope.InvoiceId, $scope.ShiftngDate, $scope.ShippingLineIdF, TaxType, $scope.shipbills, $scope.PartyId).then(function (res) {
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

            });
        }
        $scope.AddEditCargoShifting = function () {
            var conf = confirm('Are you sure you want to Save?');
            if (conf == true)
            {
                EditCargoShiftingService.AddEditCargoShifting($scope.InvoiceObj, $scope.shipbills, $scope.GodownIdF, $scope.GodownIdT, $scope.ShippingLineIdT).then(function (res) {
                    $scope.Message = res.data.Message;
                    /*if (res.data.Data != null) {
                        $scope.ShiftingNo = res.data.Data.split(',')[1];
                        $scope.InvoiceNo = res.data.Data.split(',')[0];
                        $('#InvoiceNo').val($scope.InvoiceNo);
                    }*/
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
        $scope.SelectShiftingNo = function (l) {
            $scope.ShiftingNo = l.ShiftingNo;
            $scope.ShiftngDate = l.ShiftingDt;
            $scope.CargoShiftingId = l.CargoShiftingId;
            $scope.InvoiceNo = l.InvoiceNo;
            $('#InvoiceNo').val($scope.InvoiceNo);
            EditCargoShiftingService.GetCargoShiftingDetailsInv($scope.CargoShiftingId).then(function (res) {
                $scope.InvoiceObj = res.data.Data;
                $scope.shipbills = res.data.Data.lstShippingDet;
                $scope.ShiftngDate = $scope.InvoiceObj.ShiftingDt;
                $scope.ShippingLineIdF = $scope.InvoiceObj.FromShippingId;
                $scope.ShippingLineNameF = $scope.InvoiceObj.FromShippingLineName;
                $scope.ShippingLineIdT = $scope.InvoiceObj.ToShippingId;
                $scope.ShippingLineNameT = $scope.InvoiceObj.ToShippingLineName;
                $scope.GodownIdF = $scope.InvoiceObj.FromGodownId;
                $scope.GodownNameF = $scope.InvoiceObj.FromGodownName;
                $scope.GodownIdT = $scope.InvoiceObj.ToGodownId;
                $scope.GodownNameT = $scope.InvoiceObj.ToGodownName;
                $scope.PartyId = $scope.InvoiceObj.PayeeId;
                $scope.PartyName = $scope.InvoiceObj.PayeeName;
                $scope.GSTNo = $scope.InvoiceObj.PartyGST;
                $scope.InvoiceId = $scope.InvoiceObj.InvoiceId;
                $('#CargoShiftingModal').modal('hide');
                //console.log(res.data.Data);
            });
        }
    });
})()