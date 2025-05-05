(function () {
    angular.module('CWCApp')
    .controller('DSRBankDepositCtrl', function ($scope, DSRBankDepositSvc) {
       
        
        $scope.BankAccountList =null
        DSRBankDepositSvc.GetBankAccount().then(function (r) {
            //console.log(r.data);
            debugger;
            if (r.data.Data != '') {
                $scope.BankAccountList = r.data.Data;
            }
            console.log($scope.BankAccountList);
        });
        


        $scope.DepositDate = $('#hdnCurDate').val();
        $scope.Id = 0;
        $scope.Cash = 0;
        $scope.Cheque = 0;
        $scope.NEFT = 0;
        $scope.Draft = 0;
        $scope.BankId = 0;
        $scope.DepSlipNo = '';
        $scope.Details = [];
        $scope.p_Size = 10;
        $scope.p_Step = 1;
        //$scope.AccountNo = '';
        

        $scope.clear = function () {
            $scope.DepositDate = $('#hdnCurDate').val();
            $scope.Id = 0;
            $scope.Cash = 0;
            $scope.Cheque = 0;
            $scope.NEFT = 0;
            $scope.Draft = 0;
            $scope.BankId = 0;
            //$scope.AccountNo = '';
            $scope.DepSlipNo = '';
            
        }

        
        $scope.selectedBankAccount = function (value) {
            debugger
            console.log($scope.AccountNo);

        };
      

        $scope.GetBankDepositList = function () {
            $scope.Details = [];
            $scope.txtFilter = '';
            var SearchValue = '';
            DSRBankDepositSvc.GetBankDepositList(SearchValue).then(function (r) {
                debugger;
                if (r.data.Status == 1) {
                    $scope.Details = r.data.Data;
                }
                
            });
        }

        $scope.SearchBankDepositList = function () {
            debugger;
            $scope.Details = [];           
            var SearchValue = $scope.txtFilter;
            DSRBankDepositSvc.GetBankDepositList(SearchValue).then(function (r) {
                debugger;
                if (r.data.Status == 1) {
                    $scope.Details = r.data.Data;
                }
                
            });
        }

        $scope.AddEditBankDeposit = function () {
            debugger;
            var element = document.getElementById('drpAccount');
           // element.value = d.BankId;
            if (confirm('Are you sure to Save? Y/N')) {
                var obj = {
                    Id: $scope.Id,
                    DepositDate: $scope.DepositDate,
                    Cash: $scope.Cash,
                    Cheque: $scope.Cheque,
                    NEFT: $scope.NEFT,
                    Draft:$scope.Draft, 
                    BankId:element.value,
                    DepSlipNo:$scope.DepSlipNo 
                }

                DSRBankDepositSvc.AddEditBankDeposit(obj).then(function (r) {
                   
                    if (r.data.Status == 1 || r.data.Status == 2) {
                        $scope.GetBankDepositList();
                        $scope.Reset();
                        alert(r.data.Message);
                        $scope.clear();                       
                    }
                    else {
                        alert(r.data.Message);                      
                    }
                });
            }
        }

        $scope.DeleteBankDeposit = function (Id) {
            if (confirm('Are you sure to Delete? Y/N')) {
                DSRBankDepositSvc.DeleteBankDeposit(Id).then(function (r) {
                    if (r.data.Status == 1) {
                        $scope.GetBankDepositList();
                        alert(r.data.Message);
                        $scope.clear();
                       
                    }
                    else {
                        alert(r.data.Message);
                    }
                });
            }
        }

        $scope.SetForUpdate = function (d) {
            debugger;
            $scope.Id = d.Id;
            $scope.DepositDate = d.DepositDate;
            $scope.Cash = d.Cash;
            $scope.Cheque = d.Cheque;
            $scope.NEFT = d.NEFT;
            $scope.Draft = d.Draft;
            $scope.BankId = d.BankId;
            $scope.DepSlipNo = d.DepSlipNo;        
           // $scope.AccountNo = d.BankId;
            var element = document.getElementById('drpAccount');
            element.value = d.BankId;
           
        }

        $scope.GetNEFTForBankDeposit = function () {
            $scope.NEFT = 0;
            DSRBankDepositSvc.GetNEFTForBankDeposit($scope.DepositDate).then(function (r) {
                //console.log(r.data);
                if (r.data.Status == 1) {
                    $scope.NEFT = r.data.Data.NEFT;
                }
            });
        }
        $scope.$watch('DepositDate', function (value) {
            //alert('ff');
            $scope.GetNEFTForBankDeposit();
        });
        $scope.Reset = function () {
            $('#DivBody').load('/CashManagement/DSR_CashManagement/BankDeposit');
        }

        //$scope.GetBankAccount = function () {
        //    debugger;
        //    alert('00');
        //    BankAccountList = [];
        //    DSRBankDepositSvc.GetBankAccount().then(function (r) {
        //        console.log(r.data);
        //        if (r.data.Status == 1) {
        //            BankAccountList = r.data.Data.lstBankAccount;
        //        }
        //    });
        //}

    });
})()

