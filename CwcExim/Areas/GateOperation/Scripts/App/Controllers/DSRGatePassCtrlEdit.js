(function () {
    angular.module('CWCApp').
    controller('GatePassCtrl', function ($scope) {
        function Vehicle() {
            this.Id = 0;
            this.GatePassId = 0;
            this.ContainerNo = "";
            this.VehicleNo = "";
            this.Package = 0;
            this.Location = "";
        }
        $scope.Vehicle = new Vehicle();
        $scope.Vehicles = [];
        $scope.PVehicles = [];
        $scope.FilterContainer = '';
        
        $scope.BindVehicleNo = function () {
            debugger;
            $scope.Vehicles.length = 0;
            if ($('#BaseVehicles').val() != '') {
               
                if ($('#BaseVehicles').val() != '') {
                    $scope.Vehicles = JSON.parse($('#BaseVehicles').val());
                }
                var EditVehicles = $scope.Vehicles.filter(function (item) {
                    return item.Location == $('#Location').val();
                   
                });
                debugger;
                $scope.Vehicles = EditVehicles;
               
            }
        }
        $scope.Add = function () {
            $scope.Vehicle.ContainerNo = $('#ContainerNo').val();
            $scope.Vehicle.Location = $('#Location').val();
            $scope.Vehicle.GatePassId = $('#GatePassId').val();
            
            $scope.Vehicles.push($scope.Vehicle);
            $scope.Vehicle = new Vehicle();            
        }
        $scope.ok = function () {
            debugger;
            $scope.Vehicle.ContainerNo = $('#ContainerNo').val();
            $scope.Vehicle.Location = $('#Location').val();
            $scope.Vehicle.GatePassId = $('#GatePassId').val();
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
            $scope.Vehicles.splice(k,1);
        }
        $scope.Close = function () {
            debugger;
            if ($scope.Vehicle.VehicleNo != '') {
                alert("Complete Vehicle Add Operation.");
                //$('#VehicleModal').modal('hide');
                return false;
            }
            var v = 0
            var p = 0
            for (i = 0; i < $scope.Vehicles.length; i++) {
                if ($scope.Vehicles[i].VehicleNo=='') {
                    v = 1;
                }
                if ($scope.Vehicles[i].Package == '0' && ($('#NoOfPackages').val()) != 0) {
                    p = 1;
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
            if ($('#VehicleXml').val() != '') {
                $scope.PVehicles = JSON.parse($('#VehicleXml').val());
                var EditVehicles = $scope.PVehicles.filter(function (item) {
                    return item.Location != $('#Location').val();
                    // return $scope.HtCharges.indexOf(item.Clause) > -1;
                });
                $scope.PVehicles = EditVehicles;
            }
            if ($scope.PVehicles != '') {
                $scope.PVehicles = $scope.PVehicles.concat($scope.Vehicles);

                var json = angular.toJson($scope.PVehicles);
                $('#VehicleXml').val(json);
            }
            else {
                var json = angular.toJson($scope.Vehicles);
                $('#VehicleXml').val(json);
            }

            var totalNo = parseFloat($('#NoOfPackages').val());
            var sum = 0;
            var c = 0;
            var vch = '';
            for (i = 0; i < $scope.Vehicles.length; i++) {
                if ($scope.FilterContainer == $scope.Vehicles[i].ContainerNo && $('#Location').val() == $scope.Vehicles[i].Location) {
                    vch = vch + $scope.Vehicles[i].VehicleNo + ' : ' + $scope.Vehicles[i].Package + ','
                    sum = sum + parseFloat($scope.Vehicles[i].Package);
                    c = c + 1;
                }
                
            }
            
            if ((c > 0 && sum != totalNo) && (Module != 'ecgodn' && Module != "EXPGERD")) {
                alert('Sum of Package must be same as ' + $('#NoOfPackages').val());
                return false;
            }

            $('#VehicleNo').val(vch);

            AddContainer();
            $('#VehicleModal').modal('hide');
            
        }
        
    });
})();