(function () {
    if (!window.customDirDeclared) {

        angular.module('CWCApp')
        .directive('custPagination', function () {
            return {
                transclude: true,
                scope: {
                    pItems: "=",
                    pBoundarylinks: "=",
                    pSize: "=",
                    pStep: "="
                },
                controller: function ($scope) {
                    $scope.pFirstStep = 1;
                    $scope.pTotalStep = 1;

                    $scope.setTotalStep = function () {

                        var val = 9;
                        if (!isNaN($scope.pSize)) {
                            var _val = 1;
                            if ($scope.pSize > 0) {
                                _val = $scope.pSize;
                            }
                            val = $scope.pItems.length / _val;
                            val = Math.ceil(val);

                        }
                        $scope.pTotalStep = val;
                    }

                    $scope.handleStep = function (step) {
                        $scope.pStep = step;
                    };

                    $scope.$watchGroup(['pItems', 'pSize'], function (n, o) {
                        $scope.setTotalStep();
                    });


                    $scope.setTotalStep();
                },
                template: '<div><ul class="pagination" ng-hide="pTotalStep == 1 || pSize < 1"><li ng-if="pBoundarylinks" ng-class="{ disabled : pStep == 1 }"><a href="" ng-click="handleStep(1)">&laquo;</a></li><li ng-repeat="step in [] | range:(pTotalStep)" ng-class="{ active : pStep == step }"><a href="" ng-click="handleStep(step)">{{ step }}</a></li><li ng-if="pBoundarylinks" ng-class="{ disabled : pStep == pTotalStep }"><a href="" ng-click="handleStep(pTotalStep)">&raquo;</a></li></ul></div>'
            };
        });

        window.customDirDeclared = true;
    }
})();