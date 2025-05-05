(function () {
    angular.module('CWCApp', ['ngFileUpload'])
    .controller('TrainSummaryLONICtrl', function ($scope, TrainSummaryLONIService, Upload, $timeout) {
        $scope.TrainNo = '';
        $scope.TrainDate = '';
        $scope.PortName = '';
        $scope.PortId = 0;
        $scope.TrainSummaryList = [];
        $scope.PortList = [];
        $scope.SelectedFiles = [];
        $scope.PayeeIndex = -1;
        $scope.lstShipping = [];
        $scope.IsUploaded = 0;
        $scope.State = false;
        $scope.Page = 0;
        $scope.PartyCode = '';
        /**************************************/
        if ($('#hdnShipping').val() != '' && $('#hdnShipping').val() != null)
            $scope.lstShipping = JSON.parse($('#hdnShipping').val());
        if ($('#hdnState').val() != '' && $('#hdnState').val() != null)
            $scope.State = $('#hdnState').val();
        /**************************************/
        $scope.PopulatePortList = function () {
            $('.modalloader').show();
            TrainSummaryLONIService.PopulatePortList().then(function (res) {
                $('.modalloader').hide();
                $scope.PortList = [];
                if (res.data != null) {
                    $scope.PortList = res.data;
                }
            });
        }
        $scope.SelectPort = function (obj) {
            $scope.PortName = obj.PortName;
            $scope.PortId = obj.PortId;
            $('#PortModal').modal('hide');
        }

        $scope.UploadFiles = function (files) {
            $('.modalloader').show();
            $scope.SelectedFiles = files;
            $('.modalloader').hide();
        }
        $scope.UploadFinal = function () {
            $scope.TrainDate = $('#txtDate').val();
            if ($scope.TrainNo == '') {
                alert('Enter Train No.');
                return false;
            }
            else if ($scope.TrainDate == '') {
                alert('Enter Train Date');
                return false;
            }
            else if ($scope.PortId == 0) {
                alert("Select Port");
                return false;
            }
            else if ($scope.SelectedFiles == false) {
                alert("Select an excel file");
                return false;
            }
            if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
                $('.modalloader').show();
                $('#TrainDate').datepicker("option", "disabled", true);
                Upload.upload({
                    url: '/Import/Loni_CWCImportV2/CheckUploadLONI/',
                    data: {
                        files: $scope.SelectedFiles,
                        "TrainNo": $scope.TrainNo,
                        "TrainDate": $scope.TrainDate,
                    }
                }).then(function (response) {
                    $scope.IsUploaded = 1;
                    $('.modalloader').hide();
                    $scope.TrainSummaryList = [];
                    if (response.data.Status == 0 || response.data.Status == 1) {
                        $scope.TrainSummaryList = response.data.Data;
                    }
                    else if (response.data.Status == -1) {
                        alert("No data in Excel File.");
                    }
                    else if (response.data.Status == -2) {
                        $scope.IsUploaded = 0;
                        $('#TrainDate').datepicker("option", "disabled", false);
                        alert("Uploaded Excel file is wrong.");
                    }
                    else if (response.data.Status == -3) {
                        $scope.IsUploaded = 0;
                        $('#TrainDate').datepicker("option", "disabled", false);
                        alert('Please select only Excel file.');
                    }
                    else if (response.data.Status == -4) {
                        $scope.IsUploaded = 0;
                        $('#TrainDate').datepicker("option", "disabled", false);
                        alert('Please select an Excel file.');
                    }
                    else if (response.data.Status == -5) {
                        alert('Please enter Container / CBT No for all the row in the Excel file.');
                    }
                    else {
                        alert("Error in Uploading File.");
                    }
                });
            }

        }
        $scope.SelectIndex = function (i) {
            $scope.PayeeIndex = i;
        }

        $scope.InvoiceSupplyType = [];

        $scope.SelectShippingLine = function (obj) {
            $('.modalloader').show();
            $scope.TrainSummaryList[$scope.PayeeIndex].PayeeName = obj.EximTraderName;
            $scope.TrainSummaryList[$scope.PayeeIndex].PayeeId = obj.EximTraderId;
            $('#PayeeModal').modal('hide');
            $('.modalloader').hide();
        }
        $scope.SaveInvoice = function (i) {
            $scope.TrainDate = $('#txtDate').val();
            if ($scope.TrainNo == '') {
                alert("Enter train no.");
                return false;
            }
            else if ($scope.TrainDate == '') {
                alert("Select train date");
                return false;
            }
            else if ($scope.PortId == 0) {
                alert("Select Port");
                return false;
            }
            else if ($scope.TrainSummaryList[i].PayeeId == 0) {
                alert("Select Payee Name");
                return false;
            }
            if (confirm('Are you sure you want to Save?') == false)
                return false;

            $('.modalloader').show();
            $scope.TrainSummaryList[i].TrainNo = $scope.TrainNo;
            $scope.TrainSummaryList[i].TrainDate = $scope.TrainDate;
            $scope.TrainSummaryList[i].PortId = $scope.PortId;
            $scope.TrainSummaryList[i].PortName = $scope.PortName;

            TrainSummaryLONIService.SaveInvoice($scope.TrainSummaryList[i], $('#SEZ').val()).then(function (response) {
                debugger;
                $('.modalloader').hide();
                if (response.data.Status == 1) {
                    $scope.TrainSummaryList[i].InvoiceNo = response.data.Data.split(',')[0];
                    $scope.TrainSummaryList[i].InvoiceAmt = response.data.Data.split(',')[1];
                    $('#BtnGenerateIRN').removeAttr("disabled");
                    $scope.InvoiceNo = response.data.Data.split(',')[0];
                    $scope.SupplyType = response.data.Data.split(',')[2];
                    // $scope.InvoiceSupplyType.push({'InvoiceNo':response.data.Data.split(',')[0], 'SupplyType':response.data.Data.split(',')[3]});
                    alert(response.data.Message);
                }
                else {
                    alert(response.data.Message);
                }
            });
            //$('.modalloader').hide();
        }
        $scope.ViewTrainSummary = function (TrainSummaryUploadId, TrainNo, TrainDate, PortName) {
            $('.modalloader').show();
            $scope.IsUploaded = 1;
            $('#TrainDate').datepicker("option", "disabled", true);
            TrainSummaryLONIService.ViewTrainSummary(TrainSummaryUploadId).then(function (response) {
                $('.modalloader').hide();
                if (response.data != null) {
                    $scope.TrainSummaryList = response.data;
                    $scope.TrainNo = TrainNo;
                    $scope.TrainDate = TrainDate;
                    $('#txtDate').val(TrainDate);
                    $scope.PortName = PortName;
                }
            });
        }
        $scope.LoadMoreShippingList = function () {
            $('.modalloader').show();
            $scope.Page = $scope.Page + 1;
            TrainSummaryLONIService.LoadMoreShippingList($scope.Page, $scope.PartyCode).then(function (response) {
                $('.modalloader').hide();
                if (response.data.lstShiping.length > 0) {
                    $.each(response.data.lstShiping, function (i, item) {
                        $scope.lstShipping.push(item);
                    });
                    $scope.State = response.data.State;
                }
            });
        }
        $scope.LoadShippingList = function () {
            $('.modalloader').show();
            $scope.Page = 0;
            $scope.PartyCode = '';
            TrainSummaryLONIService.LoadMoreShippingList($scope.Page, $scope.PartyCode).then(function (response) {
                $('.modalloader').hide();
                if (response.data.lstShiping.length > 0) {
                    $scope.lstShipping = response.data.lstShiping;
                    $scope.State = response.data.State;
                }
            });
        }
        $scope.SearchPartyList = function () {
            $('.modalloader').show();
            $scope.Page = 0;
            TrainSummaryLONIService.LoadMoreShippingList($scope.Page, $scope.PartyCode).then(function (response) {
                $('.modalloader').hide();
                if (response.data.lstShiping.length > 0) {
                    $scope.lstShipping = response.data.lstShiping;
                    $scope.State = response.data.State;
                }
                else {
                    $scope.lstShipping = [];
                    $scope.State = false;
                }
            });
        }

        $scope.GenerateIRN = function () {


            TrainSummaryLONIService.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {

                alert(res.data.Message);

            });



        };
    });
})();