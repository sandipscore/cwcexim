(function () {
    angular.module('CWCApp').
    controller('DSRPaymentVchCtrl', function ($scope, DSRPaymentVchSvc) {
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
                DSRPaymentVchSvc.GetPaymentVchReport($scope.FromDate, $scope.ToDate, $scope.ddlPurpose).then(function (r) {

                    debugger;
                    if (r.data.Status == 1) {
                        debugger;                       
                        $scope.RptData = JSON.parse(r.data.Data);
                        
                    }
                    else {
                        $scope.RptData = [];
                    }
                })
            }
    }

    

 

    });
})()