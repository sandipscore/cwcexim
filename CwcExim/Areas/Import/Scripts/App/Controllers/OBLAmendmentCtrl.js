(function () {
    angular.module('CWCApp').
    controller('OBLAmendmentCtrl', function ($scope, OBLAmendmentService) {
        $scope.InvoiceNo = "";
        $scope.GetOBLDetails = function () {
            debugger;
            var OBLNo = $('#OldOBLNo').val();
            OBLAmendmentService.GetOBLDetails(OBLNo).then(function (response) {
                debugger;
                if (response.data.RetValue == 1) {
                    alert('OBLNo Not Found');
                    return false;
                }
                $('#OldOBLNo').val(response.data.OldOBLNo);
                $('#NewOBLNo').val(response.data.OldOBLNo);
                $('#OBLDate').val(response.data.OBLDate);
                $('#OldNoOfPkg').val(response.data.OldNoOfPkg);
                $('#OldGRWT').val(response.data.OldGRWT);
                $('#ContainerNo').val(response.data.ContainerNo);
                $('#CFSCode').val(response.data.CFSCode);
                $('#IGMNo').val(response.data.IGMNo);
                $('#IGMDate').val(response.data.IGMDate);
             });
        }
        
        var Obj = {};
        $scope.OnOBLEntrySave = function () {
            if ($('#OldOBLNo').val() == null || $('#OldOBLNo').val() == '') {
                alert('Please Enter Old OBL No.');
                 return false;
            }
            if ($('#NewOBLNo').val() == null || $('#NewOBLNo').val() == '') {
                alert('Please Enter New OBL No.');
                return false;
            }
            //if ($('#NewOBLNo').val()== 0 || $('#NewNoOfPkg') == 0 || $('#NewGRWT').val().length == 0) {
            //    alert('Please enter any one field in new obl section');
            //    return false;
            //}
            if (confirm('Are you sure to update OBL No?')) {
                debugger;
                Obj = {
                        OldOBLNo:$('#OldOBLNo').val(),
                        OldNoOfPkg:$('#OldNoOfPkg').val(),
                        OldGRWT: $('#OldGRWT').val(),
                        NewOBLNo: $('#NewOBLNo').val(),
                        NewNoOfPkg: $('#NewNoOfPkg').val(),
                        NewGRWT: $('#NewGRWT').val(),
                }
                OBLAmendmentService.OBLEntrySave(Obj).then(function (res) {
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
            $('#OldNoOfPkg').val('');
            $('#OldGRWT').val('');
            $('#NewOBLNo').val('');
            $('#NewNoOfPkg').val(0);
            $('#NewGRWT').val(0);
            $('#ContainerNo').val('');
            $('#CFSCode').val('');
            $('#IGMNo').val('');
            $('#IGMDate').val('');
            $('#btnSave').attr("disabled", false);
        }
        $scope.ResetJODet = function () {
            debugger;
            LoadOblEntry();
            
        }
    });
})();