(function () {
    angular.module('CWCApp').
    controller('OBLEntryCtrl', function ($scope, OBLEntryService) {
        $scope.InvoiceNo = "";
        var t2 = 0;
        $scope.OblEntryDetails = [
            {
                'AddID': t2,
                'ID': t2,
                'OBLEntryId': 0,
                'OBL_No': '',
                'OBL_Date': '',
                'LineNo': '',
                'SMTPNo': '',
                'SMTP_Date': '',
                'NoOfPkg': '',
                'CargoType': '',
                'CommodityId': 0,
                'CargoDescription': '',
                'PkgType': '',
                'GR_WT': '',
                'ImporterId': 0,
                'ImporterName': '',
                'IGM_IMPORTER': '',
                'IcesData': 0
            }];
        $scope.OblEntryDetails.length = 0;
        $scope.onchangetext = function (i) {
            var flag1 = 0;
            angular.forEach($scope.OblEntryDetails, function (item) {
                debugger;
                if ((item.OBL_No.toLowerCase() == i.OBL_No.toLowerCase()) && (item.AddID != i.AddID)) {
                    flag1 = 1;
                }
            });

            if (flag1 == 1) {
                i.OBL_No = '';
                alert('Can not add duplicate OBL No.');
                //bootbox.alert({
                //    message: 'Duplicate row added in details section',
                //    size: 'small'
                //});
                return false;
            }
        }
        $(function () {
            $scope.Action = false;
            GetOBLDetailsOnEditMode();
        });
        var ind = 0;
        $scope.onImporterChange = function (index) {
            debugger;
            ind = index;
        }
        ////$('#lstImporter > li').on("click", function () {
        ////    debugger;
        ////    $scope.OblEntryDetails[ind].ImporterName = $(this).text().split('(')[0];
        ////    $scope.OblEntryDetails[ind].ImporterId = $(this).attr('id');
        ////    $scope.$apply();
        ////    $("#ImporterModal").modal("hide");
        ////});


        var inComo = 0;
        $scope.onCommodityChange = function (index) {
            debugger;
            inComo = index;
        }

        $scope.SelectCommodity = function (CommodityId, Commodity, CommodityType) {
            debugger;
            $scope.OblEntryDetails[inComo].CommodityId = CommodityId;
            $scope.OblEntryDetails[inComo].Commodity = Commodity;
            $scope.OblEntryDetails[inComo].CargoType = CommodityType;
            $('#CommodityBox').val('');
            $("#CommodityModal").modal("hide");
            LoadCommodity();
            $scope.$applyAsync();
            $("#CommodityModal").modal("hide");
        }

        var inImp = 0;
        $scope.onImporterChange = function (index) {
            debugger;
            inImp = index;
        }

        $scope.SelectImporter = function (ImporterId, ImporterName) {
            debugger;
            $scope.OblEntryDetails[inImp].ImporterId = ImporterId;
            $scope.OblEntryDetails[inImp].ImporterName = ImporterName;
            $('#Impbox').val('');
            $("#ImporterModal").modal("hide");
            LoadImporter();
            $scope.$applyAsync();
            $("#ImporterModal").modal("hide");
        }

        //$('#lstCommodity > li').on("click", function () {
        //    $scope.OblEntryDetails[inComo].Commodity = $(this).text().split('(')[0];
        //    $scope.OblEntryDetails[inComo].CommodityId = $(this).attr('id');
        //    $scope.$apply();
        //    $("#CommodityModal").modal("hide");
        //});

        $scope.CargoTypeList = [
            {
                "id": '1',
                "CargoType": "HAZ",


            },
            {
                "id": '2',
                "CargoType": "NON-HAZ",

            }
        ];
        $scope.AddOblEntry = function () {
            debugger;
            t2 = t2 + 1;
            var len = $scope.OblEntryDetails.length;
            if (len > 0) {
                if ($scope.OblEntryDetails[len - 1].OBL_No == '' || $scope.OblEntryDetails[len - 1].OBL_No == null) {
                    alert("Please fillup the row");
                    return false;
                }
                if ($scope.OblEntryDetails[len - 1].NoOfPkg == '' || $scope.OblEntryDetails[len - 1].NoOfPkg == null) {
                    alert("Please fillup the row");
                    return false;
                }
                if ($scope.OblEntryDetails[len - 1].GR_WT == '' || $scope.OblEntryDetails[len - 1].GR_WT == null) {
                    alert("Please fillup the row");
                    return false;
                }
                if ($scope.OblEntryDetails[len - 1].ImporterId == 0 || $scope.OblEntryDetails[len - 1].ImporterId == null) {
                    alert("Please fillup the row");
                    return false;
                }
                $scope.OblEntryDetails.push
                   ({
                       'AddID': t2,
                       'ID': -1,
                       'OBLEntryId': 0,
                       'OBL_No': '',
                       'OBL_Date': '',
                       'LineNo': '',
                       'SMTPNo': '',
                       'SMTP_Date': '',
                       'NoOfPkg': '',
                       'CargoType': "2",
                       'CommodityId': 0,
                       'CargoDescription': '',
                       'PkgType': '',
                       'GR_WT': '',
                       'ImporterId': 0,
                       'ImporterName': '',
                       'IGM_IMPORTER': '',
                       'IcesData': 0
                   });
            }
            else {
                $scope.OblEntryDetails.push
                   ({
                       'AddID': t2,
                       'ID': -1,
                       'OBLEntryId': 0,
                       'OBL_No': '',
                       'OBL_Date': '',
                       'LineNo': '',
                       'SMTPNo': '',
                       'SMTP_Date': '',
                       'NoOfPkg': '',
                       'CargoType': "2",
                       'CommodityId': 0,
                       'CargoDescription': '',
                       'PkgType': '',
                       'GR_WT': '',
                       'ImporterId': 0,
                       'ImporterName': '',
                       'IGM_IMPORTER': '',
                       'IcesData': 0
                   });
            }
        }
        $scope.GetOBLDetails = function () {
            debugger;
            var ContainerNo = $('#ContainerNo').val();
            var CFSCode = $('#CFSCode').val();
            var ContainerSize = $('#ContainerSize').val();
            var IGM_No = $('#IGM_No').val();
            var IGM_Date = $('#IGM_Date').val();
            var OBLEntryId = $('#Id').val();

            OBLEntryService.GetOBLDetails(ContainerNo, CFSCode, ContainerSize, IGM_No, IGM_Date, OBLEntryId).then(function (response) {
                debugger;
                $scope.OblEntryDetails = [];
                if (response.data.OblEntryDetailsList.length == 0) {
                    alert('No record found for given IGM No.');
                    return false;
                }
                $('#ContainerNoSerach').hide();
                $('#ContainerSize').val(response.data.ContainerSize);
                $('#ddlContainerSize').val(response.data.ContainerSize);
                $('#IGM_No').val(response.data.IGM_No);
                $('#IGM_Date').val(response.data.IGM_Date);
                $('#TPNo').val(response.data.TPNo);
                $('#TPDate').val(response.data.TPDate);
                $('#MovementType,#ddlMovementType').val(response.data.MovementType);
                $('#ContIcesData').val('1');
                $('#ddlContainerSize,#ddlMovementType').prop('disabled', true);
                $('#IGM_No,#IGM_Date,#TPNo,#TPDate,#ContainerNo').prop('readonly', true);
                $('#IGM_Date,#TPDate').parent().find('img').css('display', 'none');
                var j = 0;
                angular.forEach(response.data.OblEntryDetailsList, function (item) {
                    /*angular.forEach($scope.OblEntryDetails, function (item1) {
                          if (item1.OBL_No == item.OBL_No) {
                              j = 1;
                          }
                          item1.CargoType = "2";
                      });*/
                    debugger;
                    //if (j == 0) {
                    $scope.OblEntryDetails.push
                     ({
                         'ID': t2 + 1,
                         'OBLEntryId': 0,
                         'icesContId': item.icesContId,
                         'OBL_No': item.OBL_No,
                         'OBL_Date': item.OBL_Date,
                         'LineNo': item.LineNo,
                         'SMTPNo': item.SMTPNo,
                         'SMTP_Date': item.SMTP_Date,
                         'NoOfPkg': item.NoOfPkg,
                         'CargoType': "2",
                         'CommodityId': 0,
                         'CargoDescription': item.CargoDescription,
                         'PkgType': item.PkgType,
                         'GR_WT': item.GR_WT,
                         'ImporterId': item.ImporterId,
                         'ImporterName': item.ImporterName,
                         'IGM_IMPORTER': item.IGM_IMPORTER,
                         'IcesData': 1,
                     });
                    //}
                });
            });
        }
        function GetOBLDetailsOnEditMode() {
            debugger;
            if ($('#Id').val() != 0) {
                $scope.Action = false;
                if ($("#CONTCBT").val() == 'CBT') {
                    $("#ContainerSize,#ddlContainerSize").prop('disabled', true);
                    $("#CONTCBT").prop('disabled', true);
                }
                else {
                    $("#ContainerSize,#ddlContainerSize").prop('disabled', false);
                    $("#CONTCBT").prop('disabled', false);
                }
                $('#ddlContainerSize').val($('#ContainerSize').val());
                $('#ddlMovementType').val($('#MovementType').val());
                if ($('#ContIcesData').val() == '1')
                {
                    $('#ddlContainerSize,#ddlMovementType').prop('disabled', true);
                    $('#IGM_No,#IGM_Date,#TPNo,#TPDate,#ContainerNo').prop('readonly', true);
                    $('#TPDate').parent().find('img').css('display', 'none');
                    $('#IGM_Date').parent().find('img').css('display', 'none');
                    //$('#IGM_Date,#TPDate').parent().removeClass('Date_img');
                }

                var SerializedData = $.parseJSON($('#StringifiedText').val());
                //$scope.OblEntryDetails = $.parseJSON(SerializedData);
                angular.forEach(SerializedData, function (item) {
                    debugger;
                    $scope.OblEntryDetails.push
                     ({
                         'ID': 0,
                         'OBLEntryId': item.OBLEntryId,
                         'OBL_No': item.OBL_No,
                         'OBL_Date': item.OBL_Date,
                         'LineNo': item.LineNo,
                         'SMTPNo': item.SMTPNo,
                         'SMTP_Date': item.SMTP_Date,
                         'NoOfPkg': item.NoOfPkg,
                         'CargoType': item.CargoType == 0 ? "0" : item.CargoType == 1 ? "1" : "2",
                         'CommodityId': item.CommodityId,
                         'Commodity': item.Commodity,
                         'CargoDescription': item.CargoDescription,
                         'PkgType': item.PkgType,
                         'GR_WT': item.GR_WT,
                         'ImporterId': item.ImporterId,
                         'ImporterName': item.ImporterName,
                         'IsProcessed': item.IsProcessed,
                         'IGM_IMPORTER': item.IGM_IMPORTER,
                         'IcesData': item.IcesData,
                     });

                });
            }
        }

        $scope.ResetImpJODetails = function () {
            $scope.OblEntryDetails.length = 0;
        }
        $scope.Delete = function (val, j) {
            debugger;
            if (j.IsProcessed == 1) {
                alert('Can not delete as this ICE GATE OBL is already Processed');
                return false;
            }
            var len = $scope.OblEntryDetails.length;
            if (len == 1) {
                alert('At least one record should required');
            }
            else {
                $scope.OblEntryDetails.splice(val, 1);
            }
        }
        var Obj = {};
        $scope.OnOBLEntrySave = function () {
            debugger;
            if ($('#CONTCBT').val() == null || $('#CONTCBT').val() == '') {
                alert('Please Select Container/CBT');
                return false;
            }
            if ($('#ContainerNo').val() == null || $('#ContainerNo').val() == '') {
                alert('Please Select ContainerNo');
                return false;
            }
            if (($('#ContainerSize').val() == null || $('#ContainerSize').val() == '') && $('#CONTCBT').val() == "CONT") {
                alert('Please Enter Container Size');
                return false;
            }
            if ($('#IGM_No').val() == null || $('#IGM_No').val() == '') {
                alert('Please Enter IGM No.');
                return false;
            }
            if ($('#IGM_Date').val() == null || $('#IGM_Date').val() == '') {
                alert('Please Select IGM Date');
                return false;
            }
            if ($('#TPNo').val() == null || $('#TPNo').val() == '') {
                alert('Please Enter TP No.');
                return false;
            }
            if ($('#TPDate').val() == null || $('#TPDate').val() == '') {
                alert('Please Select TP Date');
                return false;
            }
            if ($('#MovementType').val() == '') {
                alert('Please Select Movement Type');
                return false;
            }
            if ($('#PortId').val() == '' || $('#PortId').val() == 0) {
                alert('Please Select Port.');
                return false;
            }
            if ($('#CountryId').val() == '' || $('#CountryId').val() == 0) {
                alert('Please Select Country.');
                return false;
            }
            if ($('#ShippingLineId').val() == '' || $('#ShippingLineId').val() == 0) {
                alert('Please Select Shipping Line.');
                return false;
            }
            var flag1 = 0;
            var flag2 = 0;
            var flag3 = 0;
            var flag4 = 0;
            var flag5 = 0;
            var flag6 = 0;
            var flag7 = 0;
            var flag8 = 0;
            var flag9 = 0;
            var flag10 = 0;
            angular.forEach($scope.OblEntryDetails, function (item) {
                if (item.OBL_No == '') {
                    flag1 = 1;
                }
                if (item.SMTPNo == '') {
                    flag3 = 1;
                }
                if (item.CargoDescription == '') {
                    flag5 = 1;
                }
                else if (item.CommodityId == '0') {
                    flag6 = 1;
                }
                else if (item.NoOfPkg == '' || item.NoOfPkg == 0) {
                    flag7 = 1;
                }
              
                else if (item.PkgType == '') {
                    flag8 = 1;
                }
                else if (item.GR_WT == '' || item.GR_WT == 0) {
                    flag9 = 1;
                }
                else if (item.ImporterId == 0) {
                    flag10 = 1;
                }
                var x = item.OBL_Date;
                var reg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
                var y = item.SMTP_Date;
                var re = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
                if (x.match(reg) && y.match(re)) {
                    return true;
               
                }
               
                else {
                    alert("OBL Date and SMTP Date should be dd/mm/yyyy");
                   // alert("SMTP Date should be dd/mm/yyyy");
                    return false;
                  
               

                }
           
            });
            if (flag1 == 1) {
                alert('Please Enter All OBL No.');
                return false;
            }
            if (flag2 == 1) {
                alert('Please Select All OBL Date');
                return false;
            }
            if (flag3 == 1) {
                alert('Please Enter All SMTP No.');
                return false;
            }
            if (flag4 == 1) {
                alert('Please Select All SMTP Date');
                return false;
            }
            if (flag5 == 1) {
                alert('Please Enter CargoDescription');
                return false;
            }
            if (flag6 == 1) {
                alert('Please Select Commodity');
                return false;
            }
            if (flag7 == 1) {
                alert('Please Enter No.Of PKG');
                return false;
            }
            if (flag8== 1) {
                alert('Please Enter Pkg. Type');
                return false;
            }
            if (flag9 == 1) {
                alert('Please Enter Gross Wt');
                return false;
            }
            if (flag10 == 1) {
                alert('Please Select Importer Name');
                return false;
            }
            debugger;
            var len = $scope.OblEntryDetails.length;
            if (len == 0) {
                alert('At least one record should required for obl details');
                return false;
            }

            if (confirm('Are you sure to save OBL Entry?')) {
                debugger;
                Obj = {
                    Id: $('#Id').val() == undefined ? 0 : $('#Id').val(),
                    CFSCode: $('#CFSCode').val(),
                    ContainerNo: $('#ContainerNo').val(),
                    ContainerSize: $('#ContainerSize').val(),
                    IGM_No: $('#IGM_No').val(),
                    IGM_Date: $('#IGM_Date').val(),
                    TPNo: $('#TPNo').val(),
                    TPDate: $('#TPDate').val(),
                    MovementType: $('#MovementType').val(),
                    PortId: $('#PortId').val(),
                    CountryId: $('#CountryId').val(),
                    ShippingLineId: $('#ShippingLineId').val(),
                    ShippingLineName: $('#ShippingLineName').val(),
                    CONTCBT: $('#CONTCBT').val(),
                    'ContIcesData': $('#ContIcesData').val(),
                }
                OBLEntryService.OBLEntrySave(Obj, JSON.stringify($scope.OblEntryDetails)).then(function (res) {
                    console.log(res);
                    alert(res.data.Message);
                    $('#btnSave').attr("disabled", true);
                    setTimeout(LoadOblEntry, 3000);
                });
            }
        }
        function LoadOblEntry() {
            $('#DivBody').load('/Import/Ppg_OblEntryV2/OBLEntry');
        }
        $scope.ResetJODet = function () {

            debugger;
            //$('#OBL_No,#OBL_Date,#LineNo,#SMTPNo,#NoOfPkg,#CargoDescription,#PkgType,#GR_WT,#ImporterName').val('');
            $scope.OblEntryDetails = [];
            //$('#CargoType').val(0);
            $('#btnAddJO').attr('disabled', false);
            $('#btnResetJO').attr('disabled', false);
        }
    });
})();

//app.directive('focusMe', ['$timeout', '$parse', function ($timeout, $parse) {
//    return {
//        //scope: true,   // optionally create a child scope
//        link: function (scope, element, attrs) {
//            var model = $parse(attrs.focusMe);
//            scope.$watch(model, function (value) {
//                console.log('value=', value);
//                if (value === true) {
//                    $timeout(function () {
//                        element[0].focus();
//                    });
//                }
//            });
//            // to address @blesh's comment, set attribute value to 'false'
//            // on blur event:
//            element.bind('blur', function () {
//                console.log('blur');
//                scope.$apply(model.assign(scope, false));
//            });
//        }
//    };
//}]);