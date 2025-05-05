(function () {
    var angularapp = angular.module('CWCApp');
    /*  angularapp.config(['$httpProvider', function ($httpProvider) {
          $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
      }]);*/
    angularapp.controller('DSRFumigationCtrl', function ($scope, DSRFumigationSvc) {
        $scope.Containers = [];
        //$scope.CFSCode = '';
        $scope.getContainerList = function () {
            $('#ContainerModal').modal();
            DSRFumigationSvc.getConts().then(function (res) {
                if (res.data.Status == 1) {
                    $scope.Containers = res.data.Data;
                } 
                else {
                    $scope.Containers = [];
                }
            });
        }

        $scope.selectContainer = function (obj) {
            $('#Container').val(obj.ContainerNo);
            $('#CFSCode').val(obj.CFSCode);
            //$scope.CFSCode = obj.CFSCode;
            $('#Size').val(obj.Size);
            $('#ContainerModal').modal('hide');
        }
    });
})()