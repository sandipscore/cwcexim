(function () {
    angular.module('CWCApp')
    .controller('KolBankDepositCtrl', function ($scope, KolBankDepositSvc) {
        $scope.DepositDate = $('#hdnCurDate').val();
        $scope.Id = 0;
        $scope.Cash = 0;
        $scope.Cheque = 0;
        $scope.NEFT = 0;
        $scope.Details = [];
        $scope.p_Size = 10;
        $scope.p_Step = 1;

        $scope.clear = function () {
            $scope.DepositDate = $('#hdnCurDate').val();
            $scope.Id = 0;
            $scope.Cash = 0;
            $scope.Cheque = 0;
            $scope.NEFT = 0;
            
        }

        $scope.GetBankDepositList = function () {
            $scope.Details = [];
            KolBankDepositSvc.GetBankDepositList().then(function (r) {
                if (r.data.Status == 1) {
                    $scope.Details = r.data.Data;
                }
            });
        }

        $scope.AddEditBankDeposit = function () {
            debugger;
            if (($scope.Cash == 0 || $scope.Cash == null) && ($scope.Cheque == 0 || $scope.Cheque == null) && ($scope.NEFT == 0 || $scope.NEFT == null) && $scope.TemporaryAdvance.length == 0) {
                alert('Please Input Bank Deposit Value Or Temporary Advance Return');
            }
            else {

                if (confirm('Are you sure to Save? Y/N')) {
                    $('#btnSave').prop("disabled", true);
                    var obj = {
                        Id: $scope.Id,
                        DepositDate: $scope.DepositDate,
                        Cash: $scope.Cash,
                        Cheque: $scope.Cheque,
                        NEFT: $scope.NEFT,
                        ExpensesDetails: $scope.TemporaryAdvance
                    }

                    KolBankDepositSvc.AddEditBankDeposit(obj).then(function (r) {
                        if (r.data.Status == 1 || r.data.Status == 2) {
                            $scope.GetBankDepositList();
                            alert(r.data.Message);
                            $scope.clear();
                            //$scope.Reset();
                        }
                        else {
                            $('#btnSave').prop("disabled", false);
                            alert(r.data.Message);
                        }
                    });
                }
            }
        }

        $scope.DeleteBankDeposit = function (Id) {
            if (confirm('Are you sure to Delete? Y/N')) {
                KolBankDepositSvc.DeleteBankDeposit(Id).then(function (r) {
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
            $scope.Id = d.Id;
            $scope.DepositDate = d.DepositDate;
            $scope.Cash = d.Cash;
            $scope.Cheque = d.Cheque;
            $scope.NEFT = d.NEFT;
        }

        $scope.GetNEFTForBankDeposit = function () {
            $scope.NEFT = 0;
            KolBankDepositSvc.GetNEFTForBankDeposit($scope.DepositDate).then(function (r) {
                console.log(r.data);
                if (r.data.Status == 1) {
                    $scope.NEFT = r.data.Data.NEFT;
                }
            });
        }
        $scope.$watch('DepositDate', function (value) {
            $scope.GetNEFTForBankDeposit();
        });
        $scope.Reset = function () {
            $('#DivBody').load('/CashManagement/Kol_CashManagement/BankDeposit');
        }


        $scope.TemporaryAdvance = [];
        $scope.txtReturnAmt = 0;

        $scope.AddNewRow = function () {
            debugger;
            //if ($('#ddlExpense').val() == '' || $('#ddlExpense').val() == 'undefined') {
            //    alert('Please select Expense Head');
            //}
            if ($('#ddlReceiptVoucher').val() == '' || $('#ddlReceiptVoucher').val() == 'undefined') {
                alert('Please select Receipt Voucher');
            }
            else if ($scope.txtReturnAmt == 0 || $scope.txtReturnAmt == "undefined") {
                alert('Please Enter Return Amount');
            }
            else {
                if (Number($scope.txtBalanceInHand) >= Number($scope.txtReturnAmt)) {
                    $scope.TemporaryAdvance.push({ 'HeadId': $scope.HeadID, 'ExpenseName': $scope.HeadName, 'ReceiptId': $scope.ReceiptId, 'VoucherNo': $scope.VoucherNo, 'BalanceInHand': $scope.txtBalanceInHand, 'RefundAmount': $scope.txtReturnAmt });
                    $scope.txtReturnAmt = 0;
                    $scope.HeadName = '';
                    $scope.VoucherNo = '';
                    $scope.HeadID = 0;
                    $scope.ReceiptId = 0;
                    $scope.txtBalanceInHand = 0;
                    $scope.GetReceiptVoucherWithAmount(0,0);
                }
                else {
                    alert('Return Amount Should Be Less Than Or Equal Balance In Hand');
                }

            }



        }
        $scope.DeleteExpense = function (id) {
            debugger;
            $scope.TemporaryAdvance.splice(id, 1);
        }

        //$scope.GetExpenscHeadWithAmount = function () {
        //    debugger;

        //    KolBankDepositSvc.GetExpenseHeadWithBalance().then(function (r) {
        //        console.log(r.data);
        //        if (r.data.Status == 1) {
        //            debugger;
        //            $scope.lstExpenscHeadWithAmoun = r.data.Data;
        //        }
        //    });
        //}


        //$scope.GetExpenscHeadWithAmount();

        $scope.HeadName = '';
        $scope.EHId = 0;

        $scope.GetReceiptVoucherWithAmount = function (Head, EHId) {
            debugger;
            var DSNo = '';
            if (EHId != undefined) {
                $scope.lstReceiptVoucher = '';
                $scope.HeadName = Head;
                if ($scope.DepSlipNo != '') {
                    DSNo = 'DepositSlip';
                }
                KolBankDepositSvc.GetReceiptVoucherBalance(EHId, DSNo).then(function (r) {
                    console.log(r.data);
                    if (r.data.Status == 1) {
                        debugger;
                        $scope.lstReceiptVoucher = r.data.Data;
                    }
                });
            }
            else {
                $scope.lstReceiptVoucher = '';
            }
        }

        $scope.GetReceiptVoucherWithAmount(0,0);
        $scope.ReceiptId = 0;
        $scope.VoucherNo = '';
        $scope.HeadID = 0;
        $scope.populateExpenseAmount = function (Id, ReceiptId, VoucherNo, Amount) {
            debugger;
            $scope.VoucherNo = VoucherNo;
            $scope.ReceiptId = ReceiptId;
            $scope.HeadID = Id;
            //$scope.txtBalanceInHand = Amount;

            if ($scope.TemporaryAdvance.length > 0) {
                var ExRefundAmount = 0;
                for (var i = 0; i < $scope.TemporaryAdvance.length; i++) {

                    if (ReceiptId == $scope.TemporaryAdvance[i].ReceiptId && Id == $scope.TemporaryAdvance[i].HeadId) {
                        ExRefundAmount += Number($scope.TemporaryAdvance[i].RefundAmount);
                    }
                    if (ExRefundAmount > 0) {
                        if (Number(Amount) > 0) {
                            $scope.txtBalanceInHand = Number(Amount) - ExRefundAmount;
                        }
                        else {
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


        $scope.GetAllDataForEdit = function (id) {
            debugger;

            KolBankDepositSvc.GetBankDetailsIdByID(id).then(function (r) {
                console.log(r.data);
                if (r.data.Status == 1) {
                    debugger;
                    $scope.jsondata = r.data.Data;
                    $scope.DepositDate = $scope.jsondata.DepositDate;
                    $scope.Cash = $scope.jsondata.Cash;
                    $scope.Cheque = $scope.jsondata.Cheque;
                    $scope.NEFT = $scope.jsondata.NEFT;
                    $scope.TemporaryAdvance = $scope.jsondata.ExpensesDetails;
                    $scope.Id = $scope.jsondata.Id;
                }
            });
        }

    });
})()