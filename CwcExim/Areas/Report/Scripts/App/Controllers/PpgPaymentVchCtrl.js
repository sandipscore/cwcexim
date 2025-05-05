(function () {
    angular.module('CWCApp').
    controller('PpgPaymentVchCtrl', function ($scope, PpgPaymentVchSvc) {
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
            else if($scope.ddlPurpose=='')
            {
                alert("Select Purpose");
            }
            else
            {
                $scope.rol2 = 0;

                $scope.rol6 = 0;

                $scope.rol5 = 0;

                $scope.rol9 = 0;

                $scope.rol10 = 0;

                $scope.rol11 = 0;

                $scope.rol12 = 0;

                $scope.rol13 = 0;

                $scope.rol14 = 0;

                $scope.rol15 = 0;

                $scope.rol16 = 0;
                $scope.rol17 = 0;
                $scope.rol18 = 0;
                $scope.rol19 = 0;
                $scope.rol20 = 0;
                $scope.rol21 = 0;
                $scope.rol22 = 0;
                $scope.rol23 = 0;
                $scope.rol24 = 0;
                $scope.rol25 = 0;
                $scope.rol26 = 0;
                $scope.rol27 = 0;
                $scope.rol28 = 0;
                $scope.rol29 = 0;
                $scope.rol31 = 0;
                PpgPaymentVchSvc.GetPaymentVchReport($scope.FromDate, $scope.ToDate, $scope.ddlPurpose).then(function (r) {
                    if (r.data.Status == 1) {
                        debugger;
                        $scope.RptData = r.data.Data;
                        for(var i=0;i<$scope.RptData.length;i++)
                        {
                            $scope.rol2 = $scope.rol2 +parseFloat($scope.RptData[i+1].Col2);

                           // $scope.rol6 = $scope.rol6 + parseFloat($scope.RptData[i+1].Col6);
                                  
                          //  $scope.rol5 = $scope.rol5 + parseFloat($scope.RptData[i+1].Col5);
                                  
                           // $scope.rol9 = $scope.rol9 + parseFloat($scope.RptData[i+1].Col9);
                                  
                            $scope.rol10 = $scope.rol10 + parseFloat($scope.RptData[i+1].Col10);
                                  
                            $scope.rol11 = $scope.rol11 + parseFloat($scope.RptData[i+1].Col11);
                                 
                            $scope.rol12 = $scope.rol12 + parseFloat($scope.RptData[i+1].Col12);
                                  
                            $scope.rol13 = $scope.rol13 + parseFloat($scope.RptData[i+1].Col13);
                                 
                            $scope.rol14 = $scope.rol14 + parseFloat($scope.RptData[i+1].Col14);
                                  
                            $scope.rol15 = $scope.rol15 + parseFloat($scope.RptData[i+1].Col15);
                                  
                            $scope.rol16 = $scope.rol16 + parseFloat($scope.RptData[i + 1].Col16);
                            $scope.rol17 = $scope.rol17 + parseFloat($scope.RptData[i + 1].Col17);
                            $scope.rol18 = $scope.rol18 + parseFloat($scope.RptData[i + 1].Col18);
                            $scope.rol19 = $scope.rol19 + parseFloat($scope.RptData[i + 1].Col19);
                            $scope.rol20 = $scope.rol20 + parseFloat($scope.RptData[i + 1].Col20);
                            $scope.rol21 = $scope.rol21 + parseFloat($scope.RptData[i + 1].Col21);
                            $scope.rol22 = $scope.rol22 + parseFloat($scope.RptData[i + 1].Col22);
                            $scope.rol23 = $scope.rol23 + parseFloat($scope.RptData[i + 1].Col23);
                            $scope.rol24 = $scope.rol24 + parseFloat($scope.RptData[i + 1].Col24);
                            $scope.rol25 = $scope.rol25 + parseFloat($scope.RptData[i + 1].Col25);
                            $scope.rol26 = $scope.rol26 + parseFloat($scope.RptData[i + 1].Col26);
                            $scope.rol27 = $scope.rol27 + parseFloat($scope.RptData[i + 1].Col27);
                            $scope.rol28 = $scope.rol28 + parseFloat($scope.RptData[i + 1].Col28);
                            $scope.rol29 = $scope.rol29 + parseFloat($scope.RptData[i + 1].Col29);
                            $scope.rol31 = $scope.rol31 + parseFloat($scope.RptData[i + 1].Col31);
                        }

                    }
                    else {
                        $scope.RptData = [];
                    }
                })
            }
        }
    });
})()