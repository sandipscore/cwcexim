(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRBankDepositSvc', function ($http) {
        this.GetBankDepositList = function (SearchValue) {
            return $http.get('/CashManagement/DSR_CashManagement/GetBankDepositList/?SearchValue='+SearchValue);
        }
        this.AddEditBankDeposit = function (obj) {
            debugger;
            if (obj.BankId == undefined || obj.BankId=='')
            {
                alert('Please select bank account no');
                return false;
            }
            if (obj.Cash == 0 && obj.Cheque == 0 && obj.NEFT == 0 && obj.Draft ==0)
            {
                alert('Please enter bank deposit value');
                return false;
            }
            return $http({
                url: "/CashManagement/DSR_CashManagement/AddEditBankDeposit",
                method: "POST",
                data: JSON.stringify(obj)
            });
        }
        this.DeleteBankDeposit = function (id) {
            return $http({
                url: "/CashManagement/DSR_CashManagement/DeleteBankDeposit/",
                method: "POST",
                params:{Id:id}
            });
        }
        this.GetNEFTForBankDeposit = function (dt) {
            return $http.get('/CashManagement/DSR_CashManagement/GetNEFTForBankDeposit/?dt=' + dt);
        }
        this.GetBankAccount = function () {           
            return $http.get('/CashManagement/DSR_CashManagement/GetBankAccount/');
        }
        
    });
})()