(function () {
    angular.module('CWCApp').
    controller('OBLSpiltCtrl', function ($scope, OBLSpiltService) {

        $scope.SpiltDetails = [];
        $scope.isFCL = 0;
        $scope.SpiltOBLNo = '';
        $scope.NoOfPkg = '';
        $scope.GRWT = '';
        $scope.oblDetailsJsonData = [];
        var TimeInSeconds = 5000;
        $scope.GetOBLList = function () {

            OBLSpiltService.GetOBLList().then(function (response) {
                debugger;
                if (response.data.Status == 1) {
                    $scope.oblJsonData = response.data.Data;

                }
                else {
                    alert(response.data.Message);
                }
            });
        }


        $scope.GetOBLSpiltDetails = function (OBL, Date, IsFCL) {
            $scope.SpiltDetails = [];
            $scope.isFCL = IsFCL;
            OBLSpiltService.GetOBLDetails(OBL, Date, IsFCL).then(function (response) {
                debugger;
                if (response.data.Status == 1) {
                    $scope.oblDetailsJsonData = response.data.Data;
                    $scope.OBLNo = OBL;
                    $('#OBLNoModal').modal('hide');

                }
                else {
                    alert(response.data.Message);
                }
            });

        }


        $scope.Validation = function () {
            debugger;
            var flag = true;

            if ($scope.SpiltOBLNo == '' || $scope.SpiltOBLNo == null) {
                $('span[data-valmsg-for="SpiltOBLNo"]').text('Fill out this field');
                flag = false;
            }
            else {
                $('span[data-valmsg-for="SpiltOBLNo"]').text('');

            }
            if ($('#CommodityId').val() == '' || $('#CommodityId').val() == 0) {

                $('span[data-valmsg-for="CommodityId"]').text('Fill out this field');
                flag = false;
            }
            else {
                $('span[data-valmsg-for="CommodityId"]').text('');

            }
            if ($scope.NoOfPkg == '' || $scope.NoOfPkg == 0) {

                $('span[data-valmsg-for="NoOfPkg"]').text('Fill out this field');
                flag = false;
            }
            else {
                if (Number($scope.NoOfPkg) > 100000000) {
                    $('span[data-valmsg-for="NoOfPkg"]').text('No Of Pkg should be less than 100000000');
                    flag = false;
                }
                else {
                    $('span[data-valmsg-for="NoOfPkg"]').text('');

                }

            }
            if ($scope.GRWT == '' || $scope.GRWT == 0) {

                $('span[data-valmsg-for="GRWT"]').text('Fill out this field');
                flag = false;
            }
            else {
                if (Number($scope.GRWT) > 100000000) {
                    $('span[data-valmsg-for="GRWT"]').text('WT should be less than 100000000');
                    flag = false;
                }
                else {
                    $('span[data-valmsg-for="GRWT"]').text('');

                }
            }
            if ($('#ImporterId').val() == '' || $('#ImporterId').val() == 0) {

                $('span[data-valmsg-for="ImporterId"]').text('Fill out this field');
                flag = false;
            }
            else {
                $('span[data-valmsg-for="ImporterId"]').text('');

            }
            return flag
        }

        $scope.AddOblEntry = function () {
            var lengthfag = 0;
            debugger;
            if ($scope.SpiltDetails.length > 0) {
                var lengthfag = $scope.SpiltDetails.filter(x=>x.SpiltOBL == $scope.SpiltOBLNo).length;

            }

            if ($scope.Validation()) {
                if (lengthfag == 0) {
                    $scope.SpiltDetails.push({
                        'SpiltOBL': $scope.SpiltOBLNo, 'SpiltOBLDate': $('#SpiltOBLDate').val(), 'SpiltCargoDesc': $scope.CargoDesc, 'SpiltCommodityName': $('#CommodityName').val(),
                        'SpiltCommodityId': $('#CommodityId').val(), 'SpiltPkg': $scope.NoOfPkg, 'SpiltWT': $scope.GRWT, 'SpiltImporter': $('#ImporterName').val(), 'SpiltImporterID': $('#ImporterId').val(),
                        'SpiltLineNo': $scope.LineNo, 'SpiltSMPTNo': $scope.SMPTNo, 'SpiltSMPTDate': $('#SMPTDate').val()
                    });
                    $scope.ResetElement();
                }
                else {
                    alert('Duplicate OBL..');
                }
            }

        }


        $scope.ResetElement = function () {
            $scope.SpiltOBLNo = '';
            $('#SpiltOBLDate').datepicker("setDate", new Date());
            $('#SpiltDate').datepicker("setDate", new Date());
            $('#SMPTDate').val('');
            $scope.CargoDesc = '';
            $('#CommodityName').val('');
            $('#CommodityId').val(0);
            $scope.NoOfPkg = '';
            $scope.GRWT = '';
            $('#ImporterName').val('');
            $('#ImporterId').val(0);
            $scope.LineNo = '';
            $scope.SMPTNo = '';

        }

        $scope.ResetOblDet = function () {
            $scope.ResetElement();
            $scope.SpiltDetails = [];
        }


        $scope.OBLSpiltSave = function () {
            $('#btnSave').attr("disabled", true);
            debugger;
            if ($scope.SpiltDetails.length < 2) {
                alert('Split OBL should be more than one ');
                $('#btnSave').attr("disabled", false);

            }
            else {
                if ($scope.oblDetailsJsonData.length == 0) {
                    alert('Please Select OBL');
                    $('#btnSave').attr("disabled", false);
                }
                else {
                    $scope.varSpiltDate = $('#SpiltDate').val();

                    OBLSpiltService.OBLSpiltSave($scope.oblDetailsJsonData, $scope.SpiltDetails, $scope.isFCL, $scope.varSpiltDate).then(function (response) {
                        debugger;
                        if (response.data.Status == 1) {
                            $scope.SpiltNo = response.data.Data;
                            if ($('#DivMsg').hasClass('logErrMsg'))
                                $('#DivMsg').removeClass('logErrMsg').addClass('logSuccMsg');
                            $('#DivMsg').html('<span>' + response.data.Message + '</span>');
                            setTimeout($scope.ResetImpOBLSpilt, TimeInSeconds);
                        }

                        else {
                            if ($('#DivMsg').hasClass('logSuccMsg'))
                                $('#DivMsg').removeClass('logSuccMsg').addClass('logErrMsg');
                            $('#DivMsg').html('<span>' + response.data.Message + '</span>');
                            $('#btnSave').attr("disabled", true);
                        }
                    });

                }
            }

        }

        $scope.ResetImpOBLSpilt = function () {
            $('#DivBody').load('/Import/Loni_CWCImportV2/OBLSplit');
        }


        $scope.Delete = function (index) {

            var r = confirm("Are you sure you want to delete?");
            if (r == true) {
                $scope.SpiltDetails.splice(index, 1);
            }

        }


        $scope.GetAllSpiltList = function () {
            $('#divlist').load('/Import/Loni_CWCImportV2/ListOBLSpilt?OBLNo');
        }






    });
})();
