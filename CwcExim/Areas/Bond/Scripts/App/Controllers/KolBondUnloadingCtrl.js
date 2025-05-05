(function () {
    angular.module('CWCApp').
    controller('KolBondUnloadingCtrl', function ($scope) {
        $scope.containers = [];
        if ($('#hdnCfsCodes').val() != null && $('#hdnCfsCodes').val() != '') {
            debugger
            $scope.containers = JSON.parse($('#hdnCfsCodes').val());
        }
        $scope.SelectedCont = [];
        $scope.CloseContainerModal = function () {
            $scope.SelectedCont = $scope.containers.filter(x=>x.Selected == true);
            $('#CfsCodes').val(JSON.stringify($scope.SelectedCont));
            $('#ContainerModal').modal('hide');
        }
    })
    .controller('KolBondUnloadingCtrlEdit', function ($scope) {
        $scope.containers = [];
        $scope.SelectedCont = [];
        if ($('#hdnCfsCodes').val() != null && $('#hdnCfsCodes').val() != '') {
            debugger
            $scope.containers = JSON.parse($('#hdnCfsCodes').val());
        }
        if ($('#CfsCodes').val() != null && $('#CfsCodes').val() != ''){
            $scope.SelectedCont = JSON.parse($('#CfsCodes').val());

            $.each($scope.containers, function (i, item) {
                if ($scope.SelectedCont.map(x=>x.CFSCode).indexOf(item.CFSCode)>-1) {
                    item.Selected = true;
                }
            });

        }

        $scope.CloseContainerModal = function () {
            $scope.SelectedCont = $scope.containers.filter(x=>x.Selected == true);
            $('#CfsCodes').val(JSON.stringify($scope.SelectedCont));
            $('#ContainerModal').modal('hide');
        }
    })
    ;
})()