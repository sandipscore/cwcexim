(function () {
    angular.module('CWCApp', ['ngFileUpload'])
    .controller('TrainSummaryLONICtrl', function ($scope, TrainSummaryLONIService, Upload, $timeout) {
        $scope.TrainNo = '';
        $scope.TrainDate = '';        
        $scope.TrainSummaryList = [];        
        $scope.SelectedFiles = [];       
        $scope.IsUploaded = 0;
        $scope.State = false;
        $scope.Page = 0;        
        /**************************************/
        
        $scope.UploadFiles = function (files) {
            $('.modalloader').show();
            $scope.SelectedFiles = files;
            $('.modalloader').hide();
        }
        $scope.UploadFinal = function () {
            debugger;
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
                    url: '/Import/LNSM_CWCImport/CheckUploadLONISAMO/',
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
        
        $scope.SaveTrainSummary = function () {
            debugger;
            $scope.TrainDate = $('#txtDate').val();
            if ($scope.TrainNo == '') {
                alert("Enter train no.");
                return false;
            }
            else if ($scope.TrainDate == '') {
                alert("Select train date");
                return false;
            }            
            if (confirm('Are you sure you want to Save?') == false)
                return false;

            $('.modalloader').show();
            TrainSummaryLONIService.SaveTrainSummary($scope.TrainSummaryList).then(function (response) {
                $('.modalloader').hide();
                if (response.data.Status = 1) {
                    $('#btnSave').attr("disabled", true);
                    alert("Train summary uploaded successfully");
                    return true;
                }
            });
                 
        }
        $scope.ViewTrainSummary = function (TrainSummaryUploadId, TrainNo, TrainDate) {
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
                }
            });
        }       
        
    });
})();