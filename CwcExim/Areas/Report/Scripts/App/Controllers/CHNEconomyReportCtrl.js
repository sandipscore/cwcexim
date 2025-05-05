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
        .controller('CHNEconomyReportCtrl', function ($scope, CHNEconomyReportSvc) {
        $scope.Msg = '';
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
        $scope.years.push({"Text":'--Select--',"Value":"0"});
        for (i = n - 10; i <= n; i++) {
            $scope.years.push({ "Text": i.toString(), "Value": i.toString() });
        }
        $scope.Month = $scope.months[0].Value;
        $scope.Year = $scope.years[0].Value;

        $scope.EcoRptData = [];
        $scope.Import = [];
        $scope.Export = [];

        $scope.GetEconomyReport = function () {
            CHNEconomyReportSvc.GetEconomyReport($scope.Month, $scope.Year).then(function (res) {
                //console.log(res.data.Data);
                if(res.data.Status==1){
                    $scope.EcoRptData = res.data.Data;
                    //$scope.Import = $scope.EcoRptData.filter(x=>x.ItemType=="I");
                   //$scope.Export = ItemType;
                }
                else{

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

        $scope.CalculateSum = function () {
            var inputs = $scope.EcoRptData.filter(x=>x.IsTextBox == 1);
            $.each(inputs, function (i, item) {
                if (item.Amount==undefined || item.Amount==null || item.Amount == '') {
                    item.Amount = "0";
                }                
            });



            //Income

            //Set @Sum1=(Select SUM(Amount) from tmpEco Where IncomeExpHeadId in (14,25,34,45,51,60,66,68,150));
            var impcargo = parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(14)].Amount);
            var impgr = parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(25)].Amount);

            var expcargo = parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(34)].Amount);
            var expgr = parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(45)].Amount);

            var ht =parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(51)].Amount);
            var htless =parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(57)].Amount);
            $scope.EcoRptData[$scope.indexOfIncomeExpHeadId(59)].Amount = ht - htless;

            var auc = parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(60)].Amount);
            var misc = parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(66)].Amount);
            var adv = parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(68)].Amount);
            var rail = parseFloat($scope.EcoRptData[$scope.indexOfIncomeExpHeadId(150)].Amount);

            var isum = impcargo + impgr + expcargo + expgr + ht + auc + misc + adv+rail;

            console.log(isum.toString());
            $scope.EcoRptData[$scope.indexOfIncomeExpHeadId(67)].Amount = isum.toString();
            


            //Expense
            var arr = $scope.EcoRptData.filter(x=>x.ItemType == "E" && x.IsTextBox == 1).map(y=>parseFloat(y.Amount));
            var s = 0;
            $.each(arr, function (i, item) {
                s = s + item;
            });
            var totalincome =parseFloat($scope.EcoRptData.filter(x=>x.IncomeExpHeadId == 67)[0].Amount); //Total Income

            $scope.EcoRptData[$scope.indexOfIncomeExpHeadId(148)].Amount = s.toString(); //Total Expense
            $scope.EcoRptData[$scope.indexOfIncomeExpHeadId(69)].Amount = (totalincome - s).toString(); //Total Income-Total Expense
        }    

        $scope.SaveEcoRpt = function () {

            if (confirm('Are You Sure To Save?')) {
                debugger;
                CHNEconomyReportSvc.SaveEconomyReport($scope.Month, $scope.Year, $scope.EcoRptData).then(function (res) {
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