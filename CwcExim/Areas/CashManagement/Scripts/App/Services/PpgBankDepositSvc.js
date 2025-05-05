(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgBankDepositSvc', function ($http) {
        this.GetBankDepositList = function () {
            return $http.get('/CashManagement/Ppg_CashManagement/GetBankDepositList/');
        }
        this.AddEditBankDeposit = function (obj) {
            return $http({
                url: "/CashManagement/Ppg_CashManagement/AddEditBankDeposit",
                method: "POST",
                data: JSON.stringify(obj)
            });
        }
        this.DeleteBankDeposit = function (id) {
            return $http({
                url: "/CashManagement/Ppg_CashManagement/DeleteBankDeposit/",
                method: "POST",
                params:{Id:id}
            });
        }
        this.GetNEFTForBankDeposit = function (dt) {
            return $http.get('/CashManagement/Ppg_CashManagement/GetNEFTForBankDeposit/?dt=' + dt);
        }
        /*this.AddEditLWB = function (objLwb) {
            return $http({
                url: "/GateOperation/Ppg_CWCGateOperation/AddEditContainersForLWB",
                method: "POST",
                data: JSON.stringify(objLwb)
            });
        }*/

        this.GetExpenseHeadWithBalance = function () {
            return $http.get('/CashManagement/Ppg_CashManagement/GetExpenseHeadWithBalance/');
        }

        this.GetReceiptVoucherBalance = function (EHId, DSNo) {
            debugger;
            return $http.get('/CashManagement/Ppg_CashManagement/GetReceiptVoucherBalance/?HeadId=' + EHId + '&DSNo=' + DSNo);
        }


        this.GetBankDetailsIdByID = function (id) {
            return $http.get('/CashManagement/Ppg_CashManagement/GetBankDepositListById/?id=' + id);
        }
    });
})()