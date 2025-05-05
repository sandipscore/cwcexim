(function () {
    angular.module('CWCApp')
        .directive('numbersOnly', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attr, ngModelCtrl) {
                    function fromUser(text) {
                        if (text) {
                            var transformedInput = text.replace(/[^0-9.]/g, '');

                            if (transformedInput !== text) {
                                ngModelCtrl.$setViewValue(transformedInput);
                                ngModelCtrl.$render();
                            }
                            return transformedInput;
                            /*var transformedInput = text; 
                            if (text.match(/^\d+(\.\d{1,2})?$/)) {
                                
                            }
                            else {
                                transformedInput = text.substring(0, text.length() - 2);
                            }
                            return transformedInput;*/
                        }
                        return undefined;
                    }
                    ngModelCtrl.$parsers.push(fromUser);
                }
            };
        })
        .controller('PpgCoreDataReportCtrl', function ($scope, PpgCoreDataReportSvc) {
            $scope.Msg = '';
            $scope.BEAvgOpeCY = 0;
            $scope.ProrataAvgOpeCY = 0;
            $scope.BEAvgActMonCY = 0;
            $scope.BEAvgActMonComuCY = 0;
            $scope.BEAvgActMonPY = 0;
            $scope.BEAvgActMonCommuPY = 0;
            $scope.BEAvgUtiCY = 0;
            $scope.ProrataAvgUtiCY = 0;
            $scope.BEAvgUtiActMonCY = 0;
            $scope.BEAvgUtiActMonComuCY;
            $scope.BEAvgUtiActMonPY = 0;
            $scope.BEAvgUtiActMonCommuPY = 0;
            $scope.BEAvgUtiCYPer = 0;
            $scope.ProrataAvgUtiCYPer = 0;
            $scope.BEAvgUtiActMonCYPer = 0;
            $scope.BEAvgUtiActMonComuCYPer = 0;
            $scope.BEAvgUtiActMonPYPer = 0;
            $scope.txtdis = 1;
            $scope.BEAvgUtiActMonCommuPY = 0;
            $scope.months = [
                { "Value": "0", "Text": "--Select--" },
                { "Value": "1", "Text": "Jan" },
                { "Value": "2", "Text": "Feb" },
                { "Value": "3", "Text": "Mar" },
                { "Value": "4", "Text": "Apr" },
                { "Value": "5", "Text": "May" },
                { "Value": "6", "Text": "Jun" },
                { "Value": "7", "Text": "Jul" },
                { "Value": "8", "Text": "Aug" },
                { "Value": "9", "Text": "Sep" },
                { "Value": "10", "Text": "Oct" },
                { "Value": "11", "Text": "Nov" },
                { "Value": "12", "Text": "Dec" }
            ];

            var d = new Date();
            var n = d.getFullYear();
            $scope.years = [];
            $scope.years.push({ "Text": '--Select--', "Value": "0" });
            for (i = n - 10; i <= n; i++) {
                $scope.years.push({ "Text": i.toString(), "Value": i.toString() });
            }
            $scope.Month = $scope.months[0].Value;
            $scope.Year = $scope.years[0].Value;

            $scope.EcoRptData = [];
            $scope.Import = [];
            $scope.Export = [];

            $scope.GetCoreDataReport = function () {
                $('.modalloader').show();
                PpgCoreDataReportSvc.GetCoreDataReport($scope.Month, $scope.Year).then(function (res) {
                    //console.log(res.data.Data);
                    if (res.data.Status == 1) {
                        $('.modalloader').hide();
                        debugger;
                        $scope.EcoRptData = res.data.Data;
                        $scope.ICDCurrMon = $scope.EcoRptData.ICDCurrMon;
                        $scope.ICDCommuMon = $scope.EcoRptData.ICDCommuMon;
                        $scope.ICDPreCurrMon = $scope.EcoRptData.ICDPreCurrMon;
                        $scope.ICDPreCommuCurrMon = $scope.EcoRptData.ICDPreCommuCurrMon;
                        $scope.MFCurrMon = $scope.EcoRptData.MFCurrMon;
                        $scope.MFCommuMon = $scope.EcoRptData.MFCommuMon;
                        $scope.MFPreCurrMon = $scope.EcoRptData.MFPreCurrMon;
                        $scope.MFPreCommuCurrMon = $scope.EcoRptData.MFPreCommuCurrMon;
                        $scope.CRTCurrMon = $scope.EcoRptData.CRTCurrMon;
                        $scope.CRTCommuMon = $scope.EcoRptData.CRTCommuMon;
                        $scope.CRTPreCurrMon = $scope.EcoRptData.CRTPreCurrMon;
                        $scope.CRTPreCommuMon = $scope.EcoRptData.CRTPreCommuMon;
                        $scope.PESTCurrMon = $scope.EcoRptData.PESTCurrMon;
                        $scope.PESTCommuMon = $scope.EcoRptData.PESTCommuMon;
                        $scope.PESTPreCurrMon = $scope.EcoRptData.PESTPreCurrMon;
                        $scope.PESTPreCommuCurrMon = $scope.EcoRptData.PESTPreCommuCurrMon;
                        $scope.OtherCurrMon = $scope.EcoRptData.OtherCurrMon;
                        $scope.OtherCommuMon = $scope.EcoRptData.OtherCommuMon;
                        $scope.OtherPreCurrMon = $scope.EcoRptData.OtherPreCurrMon;
                        $scope.OtherPreCommuMon = $scope.EcoRptData.OtherPreCommuMon;
                        $scope.TotActMon = $scope.EcoRptData.TotActMon;
                        $scope.TotCommuMon = $scope.EcoRptData.TotCommuMon;
                        $scope.TotPreActMon = $scope.EcoRptData.TotPreActMon;
                        $scope.TotPreCommMon = $scope.EcoRptData.TotPreCommMon;
                        $scope.txtdis = 0;
                        //$scope.Import = $scope.EcoRptData.filter(x=>x.ItemType=="I");
                        //$scope.Export = ItemType;
                    }
                    else {

                    }

                });
            }
            $scope.indexOfIncomeExpHeadId = function (HeadId) {
                var indexOfField = -1;
                $scope.EcoRptData.filter(function (x, i) {
                    if (x.IncomeExpHeadId == HeadId) {
                        indexOfField = i;
                    }
                });
                return indexOfField;
            };



            $scope.PrintReportData = function () {

                if($scope.BEAvgUtiCYPer==0 || $scope.ProrataAvgUtiCYPer==0||$scope.BEAvgUtiActMonCYPer==0 )
                {
                    alert("Please Input manual in Physical Performance Data");
                }
                else
                {
                    PpgCoreDataReportSvc.PrintCoreReport($scope.months[$scope.Month].Text, $scope.Year,$scope.BEAvgOpeCY,$scope.BEAvgUtiCY,$scope.BEAvgUtiCYPer,$scope.ProrataAvgOpeCY,$scope.ProrataAvgUtiCY,$scope.ProrataAvgUtiCYPer,$scope.BEAvgActMonCY,$scope.BEAvgUtiActMonCY,$scope.BEAvgUtiActMonCYPer, $scope.EcoRptData).then(function (res) {
                        debugger;
                        if (res.data.Status == 1) {
                            debugger;
                            window.open(res.data.Data + "?_t=" + (new Date().getTime()), "_blank");
                        }
                        else
                            alert(res.data.Message);
                    })
                    }

            };
            $scope.CalculateSum = function () {
                debugger;
                var mon=0;
                var remmon=0;
                if($scope.Month<=3)
                {
                    mon=parseInt($scope.Month)+12;
                    remmon=(mon-4)+1;

                }
                else
                {
                    remmon=(parseInt($scope.Month)-4)+1;
                }
                $scope.ProrataAvgOpeCY=((parseFloat($scope.BEAvgOpeCY)*remmon)/12).toFixed(2);
           

       

                


               
        };



            $scope.CalculateSumuti = function () {
                debugger;
                var mon = 0;
                var remmon = 0;
                if ($scope.Month <= 3) {
                    mon = parseInt($scope.Month) + 12;
                    remmon = (mon - 4) + 1;

                }
                else {
                    remmon = (parseInt($scope.Month) - 4) + 1;
                }
                $scope.ProrataAvgUtiCY = ((parseFloat($scope.BEAvgUtiCY) * remmon) / 12).toFixed(2);

                $scope.BEAvgUtiCYPer = ($scope.BEAvgUtiCY / $scope.BEAvgOpeCY).toFixed(2);
                $scope.ProrataAvgUtiCYPer = ($scope.ProrataAvgUtiCY / $scope.ProrataAvgOpeCY).toFixed(2);






            };
            $scope.CalculateSumact = function () {
                $scope.BEAvgUtiActMonCYPer = ($scope.BEAvgUtiActMonCY / $scope.BEAvgActMonCY).toFixed(2);
            };
            $scope.SaveEcoRpt = function () {

                if (confirm('Are You Sure To Save?')) {
                    debugger;
                    PpgCoreDataReportSvc.SaveEconomyReport($scope.Month, $scope.Year, $scope.EcoRptData).then(function (res) {
                        console.log(res.data.Data);
                        if (res.data.Status == 1) {
                            alert(res.data.Message);
                            $scope.Msg = res.data.Message;
                        }
                        else {
                            $scope.Msg = res.data.Message;
                        }

                    },
                    function (err) {
                        console.log(err);
                        //alert(err.data);
                    });
                }
            }

        });
})()