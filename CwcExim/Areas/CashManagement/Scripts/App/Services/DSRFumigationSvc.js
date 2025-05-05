(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRFumigationSvc', function ($http) {
        this.getConts = function () {
            return $http.get('/CashManagement/DSR_CashManagement/GetContainersForFumigation');
        }
    })
})()