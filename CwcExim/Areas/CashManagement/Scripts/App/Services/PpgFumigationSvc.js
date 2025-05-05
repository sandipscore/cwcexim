(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('PpgFumigationSvc', function ($http) {
        this.getConts = function () {
            return $http.get('/CashManagement/Ppg_CashManagement/GetContainersForFumigation');
        }
    })
})()