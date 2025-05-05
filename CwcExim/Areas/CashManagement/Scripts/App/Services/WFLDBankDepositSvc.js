(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('WFLDBankDepositSvc', function ($http) {
        this.GetBankDepositList = function () {
            return $http.get('/CashManagement/WFLD_CashManagement/GetBankDepositList/');
        }
        this.AddEditBankDeposit = function (obj) {
            debugger;
            if (obj.BankId == undefined || obj.BankId=='')
            {
                if($('#AccountNoId').val()!='')
                {
                    obj.BankId = $('#AccountNoId').val();
                }
                else
                {
                    alert('Please select bank account no');
                    return false;
                }                
            }
            if (obj.Cash == 0 && obj.Cheque == 0 && obj.NEFT == 0 && obj.Draft ==0 && obj.ExpensesDetails.length==0)
            {
                alert('Please enter bank deposit value or temporary advance refund ');
                return false;
            }
            return $http({
                url: "/CashManagement/WFLD_CashManagement/AddEditBankDeposit",
                method: "POST",
                data: JSON.stringify(obj)
            });
        }
        this.DeleteBankDeposit = function (id) {
            return $http({
                url: "/CashManagement/WFLD_CashManagement/DeleteBankDeposit/",
                method: "POST",
                params:{Id:id}
            });
        }
        this.GetNEFTForBankDeposit = function (dt) {
            return $http.get('/CashManagement/WFLD_CashManagement/GetNEFTForBankDeposit/?dt=' + dt);
        }
        this.GetBankAccount = function () {           
            return $http.get('/CashManagement/WFLD_CashManagement/GetBankAccount/');
        }
        
        this.GetExpenseHeadWithBalance = function () {
            return $http.get('/CashManagement/WFLD_CashManagement/GetExpenseHeadWithBalance/');
        }

        this.GetReceiptVoucherBalance = function (EHId,DSNo) {
            debugger;
            return $http.get('/CashManagement/WFLD_CashManagement/GetReceiptVoucherBalance/?HeadId=' + EHId + '&DSNo=' + DSNo);
        }


        this.GetBankDetailsIdByID = function (id) {
            return $http.get('/CashManagement/WFLD_CashManagement/GetBankDepositListById/?id='+id);
        }
    });
})()