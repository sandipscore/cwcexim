(function () {
    angular.module('CWCApp').
        controller('OblEntryUpdateCtrl', function ($scope, OblEntryUpdateService) {
            $scope.InvoiceNo = "";
            $scope.GetOBLDetails = function () {
                debugger;
                var OBLNo = $('#OldOBLNo').val();
                OblEntryUpdateService.GetOBLDetails(OBLNo).then(function (response) {
                    debugger;
                    if (response.data.RetValue == 1) {
                        alert('OBLNo Not Found');
                        return false;
                    }
                    $('#OldOBLNo').val(response.data.OldOBLNo);
                    $('#CargoType').val(response.data.CargoType);
                    $('#OBLDate').val(response.data.OBLDate);

                });
            }

            var Obj = {};
            $scope.OnOBLEntrySave = function () {
                if ($('#OldOBLNo').val() == null || $('#OldOBLNo').val() == '') {
                    alert('Please Enter Old OBL No.');
                    return false;
                }
                if ($('#OBLDate').val() == null || $('#OBLDate').val() == '') {
                    alert('Please Enter New OBL Date.');
                    return false;
                }
                if ($('#CargoType').val() == null || $('#CargoType').val() == '') {
                    alert('Please Enter Cargo Type');
                    return false;
                }
                //if ($('#NewOBLNo').val()== 0 || $('#NewNoOfPkg') == 0 || $('#NewGRWT').val().length == 0) {
                //    alert('Please enter any one field in new obl section');
                //    return false;
                //}
                if (confirm('Are you sure to update OBL No?')) {
                    debugger;
                    Obj = {
                        OldOBLNo: $('#OldOBLNo').val(),
                        OBLDate: $('#OBLDate').val(),
                        CargoType: $('#CargoType').val(),
                    }
                    OblEntryUpdateService.OBLEntrySave(Obj).then(function (res) {
                        debugger;
                        console.log(res);
                        if (res.data.Status == -1) {
                            alert('You can not left NoOfPkg or Gross Wt. blank.');
                            $('#NewNoOfPkg').val(0);
                            $('#NewGRWT').val(0);
                        }
                        else {
                            alert(res.data.Message);
                            $('#btnSave').attr("disabled", true);
                            LoadOblEntry();
                        }

                        //setTimeout(LoadOblEntry, 3000);
                    });
                }
            }
            function LoadOblEntry() {
                $('#OldOBLNo').val('');
                $('#OBLDate').val('');
                $('#CargoType').val('');
                $('#btnSave').attr("disabled", false);
            }
            $scope.ResetJODet = function () {
                debugger;
                LoadOblEntry();

            }
        });
})();