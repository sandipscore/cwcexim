(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('VrnBankDepositSvc', function ($http) {
        this.GetBankDepositList = function () {
            return $http.get('/CashManagement/VRN_CashManagement/GetBankDepositList/');
        }
        this.AddEditBankDeposit = function (obj) {
            return $http({
                url: "/CashManagement/VRN_CashManagement/AddEditBankDeposit",
                method: "POST",
                data: JSON.stringify(obj)
            });
        }
        this.DeleteBankDeposit = function (id) {
            return $http({
                url: "/CashManagement/VRN_CashManagement/DeleteBankDeposit/",
                method: "POST",
                params: { Id: id }
            });
        }
        this.GetNEFTForBankDeposit = function (dt) {
            return $http.get('/CashManagement/VRN_CashManagement/GetNEFTForBankDeposit/?dt=' + dt);
        }
        /*this.AddEditLWB = function (objLwb) {
            return $http({
                url: "/GateOperation/kol_CWCGateOperation/AddEditContainersForLWB",
                method: "POST",
                data: JSON.stringify(objLwb)
            });
        }*/

        this.GetExpenseHeadWithBalance = function () {
            return $http.get('/CashManagement/VRN_CashManagement/GetExpenseHeadWithBalance/');
        }

        this.GetReceiptVoucherBalance = function (EHId, DSNo) {
            debugger;
            return $http.get('/CashManagement/VRN_CashManagement/GetReceiptVoucherBalance/?HeadId=' + EHId + '&DSNo=' + DSNo);
        }


        this.GetBankDetailsIdByID = function (id) {
            return $http.get('/CashManagement/VRN_CashManagement/GetBankDepositListById/?id=' + id);
        }
    });
})()