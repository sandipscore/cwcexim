(function () {
    angular.module('CWCApp').
    controller('Ppg_CargoShiftingApprCtrl', function ($scope, Ppg_CargoShiftingApprSrvc,$timeout) {
        $scope.ShiftingTypeList = [{ 'Id': '2', 'ShiftingType': 'Godown To Godown' }, { 'Id': '3', 'ShiftingType': 'Both' }];
        $scope.lstShiftingNo = [];
        $scope.tblShippingDetails = [];
        $scope.IsApproved = 0;
        $scope.IsSelected = false;
        $scope.IsSaved = false;
        $scope.ShowListData = [];
        //$('#tblList tbody tr').html('');

        $scope.ShiftingNo = '';
        $scope.ShiftngDate = '';
        $scope.InvoiceId = 0;
        $scope.InvoiceNo = '';
        $scope.GSTNo = '';
        $scope.CargoShiftingId = 0;
        $scope.ShippingLineIdF = 0;
        $scope.ShippingLineNameF = "";
        $scope.ShippingLineIdT = 0;
        $scope.ShippingLineNameT = "";
        $scope.GodownIdF = 0;
        $scope.GodownNameF = ""
        $scope.GodownIdT = 0;
        $scope.GodownNameT = "";
        $scope.PartyId = 0;
        $scope.PartyName = "";
        $scope.ShiftingType = '0';
        $scope.LoadShiftingNo = function ()
        {
            Ppg_CargoShiftingApprSrvc.CargoShiftingNoForApproval().then(function (res)
            {
                $scope.lstShiftingNo = [];
                if(res.data.Status==1)
                {
                    $scope.lstShiftingNo = res.data.Data;
                }
            });
        }
        $scope.LoadShiftingDet = function (CargoShiftingId)
        {
            Ppg_CargoShiftingApprSrvc.CargoShiftingDetForApproval(CargoShiftingId).then(function (res)
            {
                if(res.data.Status==1)
                {
                    $scope.IsSelected = true;
                    $scope.ShiftingNo = res.data.Data.ShiftingNo;
                    $scope.ShiftngDate = res.data.Data.ShiftingDt;
                    $scope.InvoiceId = res.data.Data.InvoiceId;
                    $scope.InvoiceNo = res.data.Data.InvoiceNo;
                    $scope.GSTNo = res.data.Data.GSTNo;;
                    $scope.CargoShiftingId = res.data.Data.CargoShiftingId;
                    $scope.ShippingLineIdF = res.data.Data.FromShippingId;
                    $scope.ShippingLineNameF = res.data.Data.FromShippingName;
                    $scope.ShippingLineIdT = res.data.Data.ToShippingId;
                    $scope.ShippingLineNameT = res.data.Data.ToShippingName;
                    $scope.GodownIdF = res.data.Data.FromGodownId;
                    $scope.GodownNameF = res.data.Data.FromGodownName;
                    $scope.GodownIdT = res.data.Data.ToGodownId;
                    $scope.GodownNameT = res.data.Data.ToGodownName;
                    $scope.PartyId = res.data.Data.PayeeId;
                    $scope.PartyName = res.data.Data.PayeeName;
                    $scope.ShiftingType = res.data.Data.ShiftingType;
                    $scope.shipbills = res.data.Data.lstCartingRegisterDtl;
                }
                else
                {
                    $scope.ShiftingNo = '';
                    $scope.ShiftngDate = '';
                    $scope.InvoiceId = 0;
                    $scope.InvoiceNo = '';
                    $scope.GSTNo = '';
                    $scope.CargoShiftingId = 0;
                    $scope.ShippingLineIdF = 0;
                    $scope.ShippingLineNameF = "";
                    $scope.ShippingLineIdT = 0;
                    $scope.ShippingLineNameT = "";
                    $scope.GodownIdF = 0;
                    $scope.GodownNameF = ""
                    $scope.GodownIdT = 0;
                    $scope.GodownNameT = "";
                    $scope.PartyId = 0;
                    $scope.PartyName = "";
                    $scope.ShiftingType = 0;
                }
                $('#ShiftingNoModal').modal('hide');
            });
        }
        $scope.AddEditCargoShiftingApproval = function ()
        {
            if($scope.IsApproved==1)
            {
                var conf = confirm("Are you sure you want to Save??");
                if (conf)
                {
                    $scope.IsSaved = true;
                    Ppg_CargoShiftingApprSrvc.AddEditCargoShiftingApproval($scope.CargoShiftingId, $scope.IsApproved, $scope.ShiftngDate).then(function (res) {
                        if (res.data.Status == 1) {
                            $scope.Message = res.data.Message;
                            $timeout(function () { $('#DivBody').load('/Export/Ppg_CWCExport/CargoShiftingApproval'); }, 500);
                        }
                    });
                }
            }
            else
            {
                $scope.Message = 'Please Select Approve checkbox';
                $timeout(function () { $scope.Message = ''; }, 1000);
            }
        }
        $scope.ShowList = function ()
        {
            Ppg_CargoShiftingApprSrvc.ShowList().then(function (res)
            {
                $scope.ShowListData = [];
                if (res.data.Status == 1)
                {
                    $scope.ShowListData = res.data.Data;
                }
            });
        }
    });
})();