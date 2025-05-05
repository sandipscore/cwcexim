(function () {
    angular.module('CWCApp').
    controller('GatePassCtrl', function ($scope) {
        function Vehicle() {
            this.Id = 0;
            this.GatePassId = 0;
            this.ContainerNo = "";
            this.VehicleNo = "";
            this.Package = 0;
            this.Weight = 0;
        }
       
        $scope.Vehicle = new Vehicle();
        $scope.Vehicles = [];
        $scope.FilterContainer = '';
        $scope.onlyNumbers = function (event) {
        var keys = {
                38: true, 39: true, 40: true, 37: true, 27: true, 8: true, 9: true, 13: true,
                46: true, 48: true, 49: true, 50: true, 51: true, 52: true, 53: true,
                54: true, 55: true, 56: true, 57: true, 190: true, 96: true, 97: true, 98: true, 99: true,
                100: true, 101: true, 102: true, 103: true, 104: true, 105: true, 110:true
            };

            // if the pressed key is not listed, do not perform any action
            if (!keys[event.keyCode]) { event.preventDefault(); }
        }
        if ($('#VehicleXml').val() != '') {
            $scope.Vehicles = JSON.parse($('#VehicleXml').val());
        }

        $scope.Add = function () {
            $scope.Vehicle.ContainerNo = $('#ContainerNo').val();
            $scope.Vehicles.push($scope.Vehicle);
            $scope.Vehicle = new Vehicle();
        }
        $scope.ok = function () {
            debugger;
            $scope.Vehicle.ContainerNo = $('#ContainerNo').val();
            $scope.Vehicles.push($scope.Vehicle);
            //$scope.Vehicle = new Vehicle();
        }
        $scope.Delete = function (obj) {
            var k = 0;
            for (i = 0; i < $scope.Vehicles.length; i++) {
                if ($scope.Vehicles[i].ContainerNo == obj.ContainerNo && $scope.Vehicles[i].VehicleNo == obj.VehicleNo) {
                    k = i;
                }
            }
            $scope.Vehicles.splice(k, 1);
        }
        $scope.Close = function () {
            debugger;

            for (i = 0; i < $scope.Vehicles.length; i++) {
                for (j = i + 1; j < $scope.Vehicles.length; j++) {

                   // if ($scope.Vehicles[i].VehicleNo == $scope.Vehicles[j].VehicleNo) {
                      //  alert('VehicleNo  must be Different');
                       // return false;
                   // }

                }
            }
            if ($scope.Vehicle.VehicleNo != '') {
                alert("Complete Vehicle Add Operation.");
                //$('#VehicleModal').modal('hide');
                return false;
            }
            var v = 0
            var p = 0
            var w = 0;
            for (i = 0; i < $scope.Vehicles.length; i++) {
                if ($scope.Vehicles[i].VehicleNo == '') {
                    v = 1;
                }
                if (parseFloat($scope.Vehicles[i].Package) < 0) {
                    p = 1;
                }
                if (parseFloat($scope.Vehicles[i].Weight )< 0) {
                    w = 1;
                }
            }
            if (v == 1) {
                alert('VehicleNo should be not blank.Please remove row if not required');
                return false;
            }
            if (p == 1 && (Module != 'ecgodn' && Module != "EXPGERD")) {
                alert('Package should not be blank or zero.Please remove row if not required');
                return false;
            }
            if (w == 1 && (Module != 'ecgodn' && Module != "EXPGERD")) {
                alert('Weight should not be blank or zero.Please remove row if not required');
                return false;
            }
            var json = angular.toJson($scope.Vehicles);
            $('#VehicleXml').val(json);

            var totalNo = parseFloat($('#NoOfPackages').val());
            var totalNoWeigh = parseFloat($('#Weight').val());
            var sum = 0;
            var c = 0;
            var totalNoWeighsum = 0;
            var vch = '';
           
            for (i = 0; i < $scope.Vehicles.length; i++) {
                debugger
                if ($scope.FilterContainer == $scope.Vehicles[i].ContainerNo.trim()) {
                    vch = vch + $scope.Vehicles[i].VehicleNo + ' : ' + $scope.Vehicles[i].Package +':'+ $scope.Vehicles[i].Weight+','
                    sum = sum + parseFloat($scope.Vehicles[i].Package);
                    totalNoWeighsum = totalNoWeighsum + parseFloat($scope.Vehicles[i].Weight);
                    c = c + 1;
                }

            }

            if ((c > 0 && sum != totalNo) && (Module != 'ecgodn' && Module != "EXPGERD")) {
                alert('Sum of Package must be same as ' + $('#NoOfPackages').val());
                return false;
            }
            debugger;
            if ((c > 0 && totalNoWeighsum.toFixed(2) != totalNoWeigh.toFixed(2)) && (Module != 'ecgodn' && Module != "EXPGERD")) {
                alert('Sum of Weight must be same as ' + $('#Weight').val());
                return false;
            }

            $('#VehicleNo').val(vch);

            AddContainer();
            $('#VehicleModal').modal('hide');

        }

    });
})();