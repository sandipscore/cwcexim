(function () {
    angular.module('CWCApp').
    controller('KolPaymentVchCtrl', function ($scope, KolPaymentVchSvc) {
        $scope.FromDate = '';
        $scope.ToDate = '';
        $scope.ddlPurpose = 'Imprest';
        $scope.PurposeType = '';
        $scope.RptData = [];
        $scope.c2 = 0;
        $scope.c5 = 0;
        $scope.c10 = 0;
        $scope.c11 = 0;
        $scope.c12 = 0;
        $scope.c13 = 0;
        $scope.c14 = 0;
        $scope.c15 = 0;
        $scope.c16 = 0;
        $scope.c17 = 0;
        $scope.c18 = 0;
        $scope.c19 = 0;
        $scope.c20 = 0;
        $scope.c21 = 0;
        $scope.c22 = 0;
        $scope.c23 = 0;
        $scope.c24 = 0;
        $scope.c25 = 0;
        $scope.c26 = 0;
        $scope.c27 = 0;
        $scope.c29 = 0;
        $scope.c30 = 0;

        var nn2 = 0;
        var nn10 = 0;                
        var nn11 = 0;
        var nn12 = 0;
        var nn13 = 0;
        var nn14 = 0;
        var nn15 = 0;
        var nn16 = 0;
        var nn17 = 0;
        var nn18 = 0;
        var nn19 = 0;
        var nn20 = 0;
        var nn21 = 0;
        var nn22 = 0;
        var nn23 = 0;
        var nn24 = 0;
        var nn25 = 0;
        var nn26 = 0;
        var nn27 = 0;
        var nn28 = 0;
        var nn = 0;
 
        
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
                KolPaymentVchSvc.GetPaymentVchReport($scope.FromDate, $scope.ToDate, $scope.ddlPurpose).then(function (r) {
                    debugger;
                    if (r.data.Status == 1) {
                        $scope.RptData = r.data.Data;
                        var t2 = 0;
                        var t10 = 0;
                        var t11 = 0;
                        var t12 = 0;
                        var t13 = 0;
                        var t14 = 0;
                        var t15 = 0;
                        var t16 = 0;
                        var t17 = 0;
                        var t18 = 0;
                        var t19 = 0;
                        var t20 = 0;
                        var t21 = 0;
                        var t22 = 0;
                        var t23 = 0;
                        var t24 = 0;
                        var t25 = 0;
                        var t26 = 0;
                        var t27 = 0;
                        var t28 = 0;
                        var t = 0;
                        for (var i = 1; i < r.data.Data.length; i++) {
                             
                            nn = r.data.Data[i].Col30;
                            t = Number(t) + Number(nn);
                            $scope.c30 = t;
                        
                       
                            nn2 = r.data.Data[i].Col2;
                            t2 = Number(t2) + Number(nn2);
                            $scope.c2 = t2;
                           
                                $scope.c5 = r.data.Data.length - 1;
                            
                            
                                nn10 = r.data.Data[i].Col10;
                                t10 = Number(t10) + Number(nn10);
                                $scope.c10 = t10;
                            

                            
                                nn11 = r.data.Data[i].Col11;
                                t11 = Number(t11) + Number(nn11);
                                $scope.c11 = t11;
                            
                            
                                nn12 = r.data.Data[i].Col12;
                                t12 = Number(t12) + Number(nn12);
                                $scope.c12 = t12;
                            
                           
                                nn13 = r.data.Data[i].Col13;
                                t13 = Number(t13) + Number(nn13);
                                $scope.c13 = t13;
                            
                            
                                nn14 = r.data.Data[i].Col14;
                                t14 = Number(t14) + Number(nn14);
                                $scope.c14 = t14;
                            
                          
                                nn15 = r.data.Data[i].Col15;
                                t15 = Number(t15) + Number(nn15);
                                $scope.c15 = t15;
                            
                          

                                nn16 = r.data.Data[i].Col16;
                                t16 = Number(t16) + Number(nn16);
                                $scope.c16 = t16;
                            
                            
                                nn17 = r.data.Data[i].Col17;
                                t17 = Number(t17) + Number(nn17);
                                $scope.c17 = t17;
                            
                           
                                nn18 = r.data.Data[i].Col18;
                                t18 = Number(t18) + Number(nn18);
                                $scope.c18 = t18;
                            
                            
                                nn19 = r.data.Data[i].Col19;
                                t19 = Number(t19) + Number(nn19);
                                $scope.c19 = t19;
                            
                            
                                nn20 = r.data.Data[i].Col20;
                                t20 = Number(t20) + Number(nn20);
                                $scope.c20 = t20;
                            
                            
                                nn21 = r.data.Data[i].Col21;
                                t21 = Number(t21) + Number(nn21);
                                $scope.c21 = t21;
                            
                           
                                nn22 = r.data.Data[i].Col22;
                                t22 = Number(t22) + Number(nn22);
                                $scope.c22 = t22;
                            
                            
                                nn23 = r.data.Data[i].Col23;
                                t23 = Number(t23) + Number(nn23);
                                $scope.c23 = t23;
                            
                            
                                nn24 = r.data.Data[i].Col24;
                                t24 = Number(t24) + Number(nn24);
                                $scope.c24 = t24;
                            
                            
                                nn25 = r.data.Data[i].Col25;
                                t25 = Number(t25) + Number(nn25);
                                $scope.c25 = t25;
                            
                            
                                nn26 = r.data.Data[i].Col26;
                                t26 = Number(t26) + Number(nn26);
                                $scope.c26 = t26;
                            
                           
                                nn27 = r.data.Data[i].Col27;
                                t27 = Number(t27) + Number(nn27);
                                $scope.c27 = t27;

                                nn28 = r.data.Data[i].Col28;
                                t28 = Number(t28) + Number(nn28);
                                $scope.c28 = t28;
                                
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