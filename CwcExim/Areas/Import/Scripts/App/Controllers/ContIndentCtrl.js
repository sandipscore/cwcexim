(function () {
    angular.module('CWCApp').
    controller('ContIndentCtrl', function ($scope, ContIndentService) {
        $scope.FormOneId = '';
        $scope.FormOneNo = '';
        $scope.ContainerDetails = [];
        $scope.lstForm1 = [];
        $scope.IndentDate = $('#hdnDt').val();
        $scope.ICDIn = '';
        $scope.ICDOut = '';
        $scope.TrailerNo = '';

        if ($('#hdnList').val() != '')
            $scope.lstForm1 =JSON.parse($('#hdnList').val());

        $scope.SelectForm1 = function (item) {
            $scope.FormOneId = item.FormOneId;
            $scope.FormOneNo = item.FormOneNo;
            $('#Form1Modal').modal('hide');
            ContIndentService.SelectForm1(item.FormOneId).then(function (data) {
                $scope.ContainerDetails = data.data.Data;
            });
        }
        $scope.SaveData = function () {
            var conf = confirm("Are you sure you want to Save?");
            if (conf == true)
            {
                ContIndentService.AddContainerIndent($scope.FormOneId, $scope.IndentDate, $scope.TrailerNo, $scope.ICDIn, $scope.ICDOut, $scope.Remarks).then(function (data) {
                    if (data.data.Status == 1) {
                        $scope.Message = data.data.Message;
                        setTimeout(function () { $('#DivBody').load('/Import/Hdb_CWCImport/CreateContainerIndent'); }, 5000);
                    }
                });
            }
        }
    });
})();