(function () {
    angular.module('CWCApp').
    controller('WfldPaymentVchCtrl', function ($scope, WfldPaymentVchSvc) {
        $scope.FromDate = '';
        $scope.ToDate = '';
        $scope.ddlPurpose = 'Imprest';
        $scope.PurposeType = '';
        $scope.RptData = [];


        $scope.getReport = function () {
            $scope.PurposeType = $scope.ddlPurpose;
            $scope.RptData = [];
            if ($scope.FromDate == '') {
                alert("Enter From Date");
            }
            else if ($scope.ToDate == '') {
                alert("Enter To Date");
            }
            else if ($scope.ddlPurpose == '') {
                alert("Select Purpose");
            }
            else {
                WfldPaymentVchSvc.GetPaymentVchReport($scope.FromDate, $scope.ToDate, $scope.ddlPurpose).then(function (r) {
                    if (r.data.Status == 1) {
                        window.open(r.data.Data + "?_t=" + (new Date().getTime()), '_blank', 'fullscreen=yes,modal=yes');                     
                    }
                    else {
                        $('#DivDwnldWavMsg').html('<span style="color:red;">' + r.data.Message + '</span>')
                        $scope.RptData = [];
                    }
                })
            }
        }
    });
})()