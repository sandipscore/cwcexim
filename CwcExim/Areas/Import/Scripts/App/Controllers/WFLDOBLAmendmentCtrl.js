(function () {
    angular.module('CWCApp').
    controller('WFLDOBLAmendmentCtrl', function ($scope, WFLDOBLAmendmentService) {
        $scope.InvoiceNo = "";
        $scope.GetOBLDetails = function () {
            debugger;
            var OBLNo = $('#OldOBLNo').val();
            WFLDOBLAmendmentService.GetOBLDetails(OBLNo).then(function (response) {
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

                $('#NewNoOfPkg').val(response.data.OldNoOfPkg);
                $('#NewGRWT').val(response.data.OldGRWT);
                $('#OldCargoType').val(response.data.OldCargoType);
                $('#NewCargoType').val(response.data.NewCargoType);
                $('#OldTSA').val(response.data.OldTSA);
                $('#NewTSA').val(response.data.NewTSA);
                $('#OldLine').val(response.data.OldLine);
                $('#NewLine').val(response.data.NewLine);
                $('#NewCBM').val(response.data.NewCBM);
                $('#OldCBM').val(response.data.OldCBM);


                $('#ODLImporterName').val(response.data.ODLImporterName);
                $('#ODLImporterID').val(response.data.ODLImporterID);
                $('#ODLShippinglineName').val(response.data.ODLShippinglineName);
                $('#ODLShippingLineId').val(response.data.ODLShippingLineId);
                $('#NewImporterName').val(response.data.NewImporterName);
                $('#NewImporterID').val(response.data.NewImporterID);
                $('#NewShippinglineName').val(response.data.NewShippinglineName);
                $('#NewShippingLineId').val(response.data.NewShippingLineId);
                $('#NewCargoDesc').val(response.data.NewCargoDesc);
                $('#ODLCargoDesc').val(response.data.ODLCargoDesc);
                $('#NewOBLNo').focus();
                
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
                         OldOBLNo: $('#OldOBLNo').val(),
                         OBLDate: $('#OBLDate').val(),
                        OldNoOfPkg:$('#OldNoOfPkg').val(),
                        OldGRWT: $('#OldGRWT').val(),
                        NewOBLNo: $('#NewOBLNo').val(),
                        NewNoOfPkg: $('#NewNoOfPkg').val(),
                        NewGRWT: $('#NewGRWT').val(),
                        OldCargoType: $('#OldCargoType').val(),
                        NewCargoType: $('#NewCargoType').val(),
                        OldTSA: $('#OldTSA').val(),
                        NewTSA: $('#NewTSA').val(),
                        OldLine: $('#OldLine').val(),
                        NewLine: $('#NewLine').val(),
                        NewCBM: $('#NewCBM').val(),
                        OldCBM: $('#OldCBM').val(),

                        ODLImporterID: $('#ODLImporterID').val(),
                        ODLShippingLineId: $('#ODLShippingLineId').val(),
                        NewImporterID: $('#NewImporterID').val(),
                        NewShippingLineId: $('#NewShippingLineId').val(),
                        NewCargoDesc: $('#NewCargoDesc').val(),
                        ODLCargoDesc: $('#ODLCargoDesc').val(),



                }
                debugger;
                WFLDOBLAmendmentService.OBLEntrySave(Obj).then(function (res) {
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
            $('#NewTSA').val('');
            $('#OldTSA').val('');
            $('#NewLine').val('');
            $('#OldLine').val('');
            $('#NewCBM').val('0');
            $('#OldCBM').val('0');


            $('#ODLImporterID').val(''),
            $('#ODLShippingLineId').val(''),
            $('#NewImporterID').val(''),
             $('#NewShippingLineId').val(''),
            $('#NewCargoDesc').val(''),
            $('#ODLCargoDesc').val(''),

             $('#ODLImporterName').val(''),
             $('#ODLShippinglineName').val(''),
             $('#NewImporterName').val(''),
             $('#NewShippinglineName').val(''),
            
            $('#btnSave').attr("disabled", false);
        }
        $scope.ResetJODet = function () {
            debugger;
            LoadOblEntry();
            
        }
    });
})();