(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('ChequeDepositSvc', function ($http) {
        this.SaveEntry=function(obj){
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/CashManagement/Ppg_CashManagement/SaveChequeDeposit/",
                method: "POST",
                data: JSON.stringify(obj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    })
})()