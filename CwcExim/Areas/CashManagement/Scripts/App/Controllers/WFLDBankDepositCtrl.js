(function () {
    angular.module('CWCApp')
    .controller('WFLDBankDepositCtrl', function ($scope, WFLDBankDepositSvc) {
              
        $scope.BankAccountList =null
        WFLDBankDepositSvc.GetBankAccount().then(function (r) {
            console.log(r.data);
            debugger;
            if (r.data.Data != '') {
                $scope.BankAccountList = r.data.Data;
            }
            console.log($scope.BankAccountList);
        });
        


        $scope.DepositDate = $('#hdnCurDate').val();
        $scope.ReceivedDate = $('#hdnRecDate').val();
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
        

        $scope.clear = function () {
            $scope.DepositDate = $('#hdnCurDate').val();
             $scope.ReceivedDate = $('#hdnRecDate').val();
            $scope.Id = 0;
            $scope.Cash = 0;
            $scope.Cheque = 0;
            $scope.NEFT = 0;
            $scope.Draft = 0;
            $scope.BankId = 0;
            $scope.AccountNo = '';
            $scope.DepSlipNo = '';
            
        }

        
        //$scope.selectedBankAccount = function (value) {
        //    debugger
        //    console.log($scope.AccountNo);

        //};
      

        $scope.GetBankDepositList = function () {
            $scope.Details = [];
            WFLDBankDepositSvc.GetBankDepositList().then(function (r) {
                if (r.data.Status == 1) {
                    $scope.Details = r.data.Data;
                }
            });
        }

        $scope.AddEditBankDeposit = function () {
            debugger;
            if (confirm('Are you sure to Save? Y/N')) {
                var obj = {
                    Id: $scope.Id,
                    DepositDate: $scope.DepositDate,
                    ReceivedDate: $scope.ReceivedDate,
                    Cash: $scope.Cash,
                    Cheque: $scope.Cheque,
                    NEFT: $scope.NEFT,
                    Draft:$scope.Draft, 
                    BankId:$scope.AccountNo,//$scope.BankId,
                    DepSlipNo: $scope.DepSlipNo,
                    ExpensesDetails: $scope.TemporaryAdvance
                }

                WFLDBankDepositSvc.AddEditBankDeposit(obj).then(function (r) {
                    if (r.data.Status == 1 || r.data.Status == 2) {
                        debugger;
                        $scope.GetBankDepositList();
                        alert(r.data.Message);
                        if (r.data.Status == 1) {
                            $scope.DepSlipNo = r.data.Data;
                        }
                       // $scope.$apply();
                        setTimeout(function () {
                            debugger;
                            $scope.Reset();
                        }, 3000);
                    }
                    else {
                        alert(r.data.Message);
                    }
                });
            }
        }

        $scope.DeleteBankDeposit = function (Id) {
            debugger;
            if (confirm('Are you sure to Delete? Y/N')) {
                WFLDBankDepositSvc.DeleteBankDeposit(Id).then(function (r) {
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
           
            $scope.TemporaryAdvance = [];
            
            WFLDBankDepositSvc.GetBankDetailsIdByID(d.Idd).then(function (r) {
                if (r.data.Status == 1) {
                    debugger;                    
                    document.getElementById('AccountNoId').focus();
                    $scope.Id = r.data.Data.Id;
                    $scope.DepositDate = r.data.Data.DepositDate;
                    $scope.ReceivedDate = r.data.Data.ReceivedDate;
                    $scope.Cash = r.data.Data.Cash;
                    $scope.Cheque = r.data.Data.Cheque;
                    $scope.NEFT = r.data.Data.NEFT;
                    $scope.Draft = r.data.Data.Draft;
                    $scope.BankId = r.data.Data.BankId;
                    //$scope.AccountNo = r.data.Data.BankId;
                    $('#AccountNoId').val(r.data.Data.BankId);
                    $scope.DepSlipNo = r.data.Data.DepSlipNo;                   
                    var jsonData = r.data.Data.ExpensesDetails;
                    
                     $("#txtDate").datepicker("option", "disabled", true);
                    
                    for (let i = 0; i < jsonData.length; i++)
                    {
                    
                        $scope.TemporaryAdvance.push({ 'HeadId': jsonData[i].HeadId, 'ExpenseName': jsonData[i].HeadName, 'ReceiptId': jsonData[i].ReceiptId, 'VoucherNo': jsonData[i].VoucherNo, 'BalanceInHand': '', 'RefundAmount': jsonData[i].RefundAmount, 'id': jsonData[i].id });
                    }
                }
                else {
                    alert(r.data.Message);
                }
            });


            
        }

        $scope.GetNEFTForBankDeposit = function () {
            debugger;
            $scope.NEFT = 0;
            WFLDBankDepositSvc.GetNEFTForBankDeposit($scope.ReceivedDate).then(function (r) {
                console.log(r.data);
                if (r.data.Status == 1) {
                    $scope.NEFT = r.data.Data.NEFT;
                }
            });
        }
        $scope.$watch('ReceivedDate', function (value) {
            $scope.GetNEFTForBankDeposit();
        });
        $scope.Reset = function () {
            $('#DivBody').load('/CashManagement/WFLD_CashManagement/BankDeposit');
        }


        $scope.TemporaryAdvance = [];
        $scope.txtReturnAmt = 0;
       
        $scope.AddNewRow=function()
        {
            debugger;
            if ($('#ddlExpense').val() == '' || $('#ddlExpense').val() == 'undefined')
            {
                alert('Please select Expense Head');
            }
            else if ($('#ddlReceiptVoucher').val() == '' || $('#ddlReceiptVoucher').val() == 'undefined') {
                alert('Please select Receipt Voucher');
            }
            else if($scope.txtReturnAmt == 0 || $scope.txtReturnAmt == "undefined")
            {
                alert('Please Enter Return Amount');
            }
           else
            {
                if (Number($scope.txtBalanceInHand) >= Number($scope.txtReturnAmt))
                {
                    $scope.TemporaryAdvance.push({ 'HeadId': $scope.HeadID, 'ExpenseName': $scope.HeadName, 'ReceiptId': $scope.ReceiptId, 'VoucherNo': $scope.VoucherNo, 'BalanceInHand': $scope.txtBalanceInHand, 'RefundAmount': $scope.txtReturnAmt });
                    $scope.txtReturnAmt = 0;
                    $scope.HeadName = '';
                    $scope.VoucherNo = '';
                    $scope.HeadID = 0;
                    $scope.ReceiptId = 0;
                    $scope.txtBalanceInHand = 0;
                    $scope.GetExpenscHeadWithAmount();                    
                }
                else
                {
                    alert('Return Amount Should Be Less Than Or Equal Balance In Hand');
                }
              
            }
            
           

 }

        $scope.DeleteExpense=function(id)
        {
            debugger;
            $scope.TemporaryAdvance.splice( id,1);
        }

        //$scope.GetBankAccount = function () {
        //    debugger;
        //    alert('00');
        //    BankAccountList = [];
        //    WFLDBankDepositSvc.GetBankAccount().then(function (r) {
        //        console.log(r.data);
        //        if (r.data.Status == 1) {
        //            BankAccountList = r.data.Data.lstBankAccount;
        //        }
        //    });
        //}
        

        $scope.GetExpenscHeadWithAmount = function () {
            debugger;
         
            WFLDBankDepositSvc.GetExpenseHeadWithBalance().then(function (r) {
                console.log(r.data);
                if (r.data.Status == 1) {
                    debugger;
                    $scope.lstExpenscHeadWithAmoun= r.data.Data;
                }
            });
        }


        $scope.GetExpenscHeadWithAmount();
        
        $scope.HeadName = '';
        $scope.EHId = 0;
        
        $scope.GetReceiptVoucherWithAmount = function (Head,EHId) {
            debugger;
            var DSNo = '';
            if (EHId != undefined) {
                $scope.lstReceiptVoucher = '';
                $scope.HeadName = Head;
                if ($scope.DepSlipNo != '')
                {
                    DSNo = 'DepositSlip';
                }
                WFLDBankDepositSvc.GetReceiptVoucherBalance(EHId,DSNo).then(function (r) {
                    console.log(r.data);
                    if (r.data.Status == 1) {
                        debugger;
                        $scope.lstReceiptVoucher = r.data.Data;
                    }
                });
            }
            else
            {
                $scope.lstReceiptVoucher = '';
            }
        }

        $scope.ReceiptId = 0;
        $scope.VoucherNo = '';
        $scope.HeadID = 0;
        $scope.populateExpenseAmount = function (Id, ReceiptId,VoucherNo, Amount) {
            debugger;            
            $scope.VoucherNo = VoucherNo;
            $scope.ReceiptId = ReceiptId;
            $scope.HeadID = Id;
            //$scope.txtBalanceInHand = Amount;

            if($scope.TemporaryAdvance.length>0)
            {
                var ExRefundAmount = 0;
                for (var i = 0; i < $scope.TemporaryAdvance.length; i++) {

                    if (ReceiptId == $scope.TemporaryAdvance[i].ReceiptId && Id == $scope.TemporaryAdvance[i].HeadId) {
                        ExRefundAmount += Number($scope.TemporaryAdvance[i].RefundAmount);
                    }
                    if (ExRefundAmount > 0) {
                        if (Number(Amount) > 0)
                        {
                            $scope.txtBalanceInHand = Number(Amount) - ExRefundAmount;
                        }
                        else
                        {
                            $scope.txtBalanceInHand = Amount;
                        }
                        
                    }
                    else {
                        $scope.txtBalanceInHand = Amount;
                    }

                }
            }
            else {
                $scope.txtBalanceInHand = Amount;
            }

        }

    });
})()