(function () {

    var app=angular.module('CWCApp');
    app.controller('KolGateOperationCtrl', function ($scope, KolGateOperationSvc) {
        

        $scope.LWBDate = $("#hdnCurDate").val();
        $scope.GateInDate = $("#hdnCurDate").val();
        $scope.Containers = [];
        $scope.LWBDetails = [];
        $scope.Id = 0;
        $scope.Container = '';
        $scope.CFSCode = '';
        $scope.Size = '';
        $scope.FilterContainer = '';
        $scope.p_Size = 10;
        $scope.p_Step = 1;

        $scope.GetLWBEntrydetails = function () {
            KolGateOperationSvc.GetLWBEntrydetails(0).then(function (r) {
                debugger
                if (r.data.Status == 1) {
                    $scope.LWBDetails = r.data.Data;
                }
                else {
                    
                    alert(r.data.Message);
                }
            })
        }


        $scope.GetContainers = function () {
            KolGateOperationSvc.GetContainerList($scope.GateInDate).then(function (r) {
                debugger
                if (r.data.Status == 1) {
                    $scope.Containers = r.data.Data;
                    $("#Containerdtl").modal();
                }
                else {
                    $scope.Containers = [];
                    alert(r.data.Message +" "+". Please Check GateIn Date.");
                }
            })
        }

        $scope.SelectContainer = function (Container, CFSCode, Size) {
            $scope.Container = Container;
            $scope.CFSCode = CFSCode;
            $scope.Size = Size;
            $("#Containerdtl").modal('hide');
        }
        $scope.clear = function () {
            $scope.Id = 0;
            $scope.Container = "";
            $scope.CFSCode = "";
            $scope.Size = "";
            $scope.LWBDate = $("#hdnCurDate").val();
            $scope.GateInDate = $("#hdnCurDate").val();
        }
        $scope.SetForUpdate = function (d) {
            $scope.Id = d.Id;
            $scope.Container = d.ContainerNo;
            $scope.CFSCode = d.CFSCode;
            $scope.Size = d.Size;
            $scope.LWBDate = d.LWBDate;
            $scope.GateInDate = d.GateInDate;
        }
        $scope.AddEditLWB = function () {
            if (confirm("Are you sure to save this record? Y/N")) {
                var obj = {
                    Id: $scope.Id,
                    LWBDate: $scope.LWBDate,
                    GateInDate: $scope.GateInDate,
                    ContainerNo: $scope.Container,
                    CFSCode: $scope.CFSCode,
                    Size: $scope.Size
                }

                KolGateOperationSvc.AddEditLWB(obj).then(function (r) {
                    debugger
                    if (r.data.Status == 1 || r.data.Status == 2) {
                        alert(r.data.Message);
                        $scope.clear();
                        $scope.GetLWBEntrydetails();
                    }
                    else {
                        alert("Error " + r.data.Message + " " + ". Please Check.");
                    }
                })
            }

            
        }

        $scope.Reset = function () {
            $scope.$destroy();
            $('#DivBody').load('/GateOperation/kol_CWCGateOperation/CreateLWBForWS');
        }
    })

})();