(function () {
    angular.module('CWCApp').
    controller('DSR_OBLEntryCtrl', function ($scope, DSR_OBLEntryService) {
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
                'TSANO': '',
                'TSA_Date': '',
                'SMTPNo': '',
                'SMTP_Date': '',
                'NoOfPkg': '',
                'CargoType': '',
                'CommodityId': 0,
                'CommodityName': '',
                'CargoDescription': '',
                'PkgType': '',
                'GR_WT': '',
                'ImporterId': 0,
                'ImporterName': '',
                'ShippingLineId': 0,
                'ShippingLineName': '',
                'IGM_IMPORTER': '',
                'AreaCBM': 0,
                'CIFValue': 0,
                'CHAId': 0,
                'CHAName':''

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
            if (CommodityType != 0)
            {
                $('#Cargo_Type').val(CommodityType); 
            }
            $('#Cargo_Type').focus();
            
            $('#Commodity_Id').val(CommodityId);
            $('#Commodity_Name').val(Commodity);
            $('#Commodity_Type').val(CommodityType);

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
            
            $('#Importer_Id').val(ImporterId);
            $('#Importer_Name').val(ImporterName);
            $('#Impbox').val('');
            $("#ImporterModal").modal("hide");
            LoadImporter();
            $scope.$applyAsync();
            $("#ImporterModal").modal("hide");
            $('#ShippingLine_Name').focus();
        }

        var inShipp = 0;
        $scope.onShippingLineChange = function (index) {
            debugger;
            inShipp = index;
        }

        $scope.EditData = function (j) {
            debugger;

            $('#Rno').val(j);
            $('#OBLHBL_No').val($scope.OblEntryDetails[j].OBL_No);
            $('#OBLHBL_Date').val($scope.OblEntryDetails[j].OBL_Date);

            $('#TSA_No').val($scope.OblEntryDetails[j].TSANO);
            $('#TSA_Date').val($scope.OblEntryDetails[j].TSA_Date);

            $('#SMTP_No').val($scope.OblEntryDetails[j].SMTPNo);
            $('#SMTP_Date').val($scope.OblEntryDetails[j].SMTP_Date);

        

            $('#ShippingLine_Id').val($scope.OblEntryDetails[j].ShippingLineId);
            $('#ShippingLine_Name').val($scope.OblEntryDetails[j].ShippingLineName);

            $('#NO_PKG').val($scope.OblEntryDetails[j].NoOfPkg);
            $('#Cargo_Type').val($scope.OblEntryDetails[j].CargoType);

            $('#Commodity_Id').val($scope.OblEntryDetails[j].CommodityId);
            $('#Commodity_Name').val($scope.OblEntryDetails[j].Commodity);

            $('#Cargo_Des').val($scope.OblEntryDetails[j].CargoDescription);
            $('#PKG_Type').val($scope.OblEntryDetails[j].PkgType);

            $('#GR_WT').val($scope.OblEntryDetails[j].GR_WT);
            $('#Importer_Id').val($scope.OblEntryDetails[j].ImporterId);

            $('#Importer_Name').val($scope.OblEntryDetails[j].ImporterName);
            $('#Area_CBM').val($scope.OblEntryDetails[j].AreaCBM);

            $('#CIF_Value').val($scope.OblEntryDetails[j].CIFValue);
            $('#CHAId').val($scope.OblEntryDetails[j].CHAId);
            $('#CHAName').val($scope.OblEntryDetails[j].CHAName);
        }
        $scope.OBLSelectShippingLine = function (ShippingLineId, ShippingLineName, PartyCode) {

            debugger;
            //  $scope.OblEntryDetails[inShipp].ShippingLineId = ShippingLineId;
            //   $scope.OblEntryDetails[inShipp].ShippingLineName = ShippingLineName;

            $('#ShippingLine_Name').val(ShippingLineName);
            $('#ShippingLine_Id').val(ShippingLineId);
            $('#ShippingLine').val(PartyCode);
            $('#OBLShpngLinebox').val('');
            $("#OBLShippingLineModal").modal("hide");
            LoadShippingLine();
            $scope.$applyAsync();
            $("#OBLShippingLineModal").modal("hide");
            $('#CIF_Value').focus();
        }

        $scope.ViewData = function (j) {
            debugger;

            $('#Rno').val(j);
            $('#OBLHBL_No').val($scope.OblEntryDetails[j].OBL_No);
            $('#OBLHBL_Date').val($scope.OblEntryDetails[j].OBL_Date);

            $('#TSA_No').val($scope.OblEntryDetails[j].TSANO);
            $('#TSA_Date').val($scope.OblEntryDetails[j].TSA_Date);

            $('#SMTP_No').val($scope.OblEntryDetails[j].SMTPNo);
            $('#SMTP_Date').val($scope.OblEntryDetails[j].SMTP_Date);


            $('#ShippingLine_Id').val($scope.OblEntryDetails[j].ShippingLineId);
            $('#ShippingLine_Name').val($scope.OblEntryDetails[j].ShippingLineName);

            $('#NO_PKG').val($scope.OblEntryDetails[j].NoOfPkg);
            $('#Cargo_Type').val($scope.OblEntryDetails[j].CargoType);

            $('#Commodity_Id').val($scope.OblEntryDetails[j].CommodityId);
            $('#Commodity_Name').val($scope.OblEntryDetails[j].Commodity);

            $('#Cargo_Des').val($scope.OblEntryDetails[j].CargoDescription);
            $('#PKG_Type').val($scope.OblEntryDetails[j].PkgType);

            $('#GR_WT').val($scope.OblEntryDetails[j].GR_WT);
            $('#Importer_Id').val($scope.OblEntryDetails[j].ImporterId);

            $('#Importer_Name').val($scope.OblEntryDetails[j].ImporterName);
            $('#Area_CBM').val($scope.OblEntryDetails[j].AreaCBM);

            $('#CIF_Value').val($scope.OblEntryDetails[j].CIFValue);
            $('#CHAId').val($scope.OblEntryDetails[j].CHAId);
            $('#CHAName').val($scope.OblEntryDetails[j].CHAName);
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

        $scope.PkgTypeList = [
            {
                "id": 'Pallet',
                "PkgType": "Pallet",


            },
            {
                "id": 'Baggage',
                "PkgType": "Baggage",

            },
            {
                "id": 'Boxes',
                "PkgType": "Boxes",

            },
            {
                "id": 'Jumbo Bag',
                "PkgType": "Jumbo Bag",

            },
            {
                "id": 'Unit',
                "PkgType": "Unit",

            },
            {
                "id": 'Pieces',
                "PkgType": "Pieces",

            },
            {
                "id": 'Coils',
                "PkgType": "Coils",

            },
            {
                "id": 'Sheets',
                "PkgType": "Sheets",

            },
            {
                "id": 'Drums',
                "PkgType": "Drums",

            },
            {
                "id": 'Loose',
                "PkgType": "Loose",

            },
            {
                "id": 'Others',
                "PkgType": "Others",

            }
        ];

        $scope.AddOblEntry = function () {
            debugger;
            var j = $('#Rno').val();
           
            if (j == '') {


                if ($('#OBLHBL_No').val() == '' || $('#OBLHBL_No').val() == null) {
                    alert("Please enter OBL No");
                    return false;
                }
                if ($('#OBLHBL_Date').val() == '' || $('#OBLHBL_Date').val() == null) {
                    alert("Please enter OBL Date");
                    return false;
                }
                if ($('#Commodity_Id').val() == 0 || $('#Commodity_Id').val() == null) {
                    alert("Please select Commodity");
                    return false;
                }
                if ($('#Cargo_Type').val() == '' || $('#Cargo_Type').val() == null) {
                    alert("Please enter Cargo Type");
                    return false;
                }
                if (Number($('#NO_PKG').val())<=0) {
                    alert("No of package must be greater than 0");
                    return false;
                }

                
                if ($('#PKG_Type').val() == '' || $('#PKG_Type').val() == null) {
                    alert("Please select Package type");
                    return false;
                }
                if (Number($('#GR_WT').val())<=0) {
                    alert("Gross Weight must be greater than 0");
                    return false;
                }
                //if (Number($('#Area_CBM').val()) <= 0) {
                //    alert("CBM Value must be greater than 0");
                //    return false;
                //}
                //if ($('#Area_CBM').val() == '' || $('#Area_CBM').val() == null) {
                //    alert("Please fillup Area CBM");
                //    return false;
                //}
                if ($('#Importer_Id').val() == 0 || $('#Importer_Id').val() == null) {
                    alert("Please select Importer Name");
                    return false;
                }
                if ($('#ShippingLine_Id').val() == 0 || $('#ShippingLine_Id').val() == null) {
                    alert("Please  select Shipping Line");
                    return false;
                }
                
                
                if (Number($('#CIF_Value').val())<=0) {
                    alert("CIF Value must be greater than 0");
                    return false;
                }


                t2 = t2 + 1;
                var len = $scope.OblEntryDetails.length;
                if (len > 0) {
                    if ($scope.OblEntryDetails[len - 1].OBL_No == '' || $scope.OblEntryDetails[len - 1].OBL_No == null) {
                        alert("Please fillup the row");
                        return false;
                    }
                    if (Number($scope.OblEntryDetails[len - 1].NoOfPkg)<=0) {
                        alert("No of Package must be greater than 0");
                        return false;
                    }
                    if (Number($scope.OblEntryDetails[len - 1].GR_WT)<=0) {
                        alert("Gross Weight must be greater than 0");
                        return false;
                    }
                    //if (Number($scope.OblEntryDetails[len - 1].Area_CBM) <= 0) {
                    //    alert("CBM Value must be greater than 0");
                    //    return false;
                    //}
                    if ($scope.OblEntryDetails[len - 1].ImporterId == 0 || $scope.OblEntryDetails[len - 1].ImporterId == null) {
                        alert("Please fillup the row");
                        return false;
                    }
                    if ($scope.OblEntryDetails[len - 1].ShippingLineId == 0 || $scope.OblEntryDetails[len - 1].ShippingLineId == null) {
                        alert("Please fillup the row");
                        return false;
                    }
                    if (Number($scope.OblEntryDetails[len - 1].CIFValue)<=0) {
                        alert("CIF Value must be greater than 0");
                        return false;
                    }
                    var k = 0;
                    angular.forEach($scope.OblEntryDetails, function (item1) {
                        if (item1.OBL_No == $('#OBLHBL_No').val()) {
                            k = 1;
                        }
                       
                    });
                   
                    if (k == 1)
                    {
                        alert("OBL No already exists");
                        return false;
                    }
                    $scope.OblEntryDetails.push
                       ({
                           'AddID': t2,
                           'ID': -1,
                           'OBLEntryId': 0,
                           'OBL_No': $('#OBLHBL_No').val(),
                           'OBL_Date': $('#OBLHBL_Date').val(),
                           'LineNo': $('#ShippingLine').val(),
                           'TSANO': $('#TSA_No').val(),
                           'TSA_Date': $('#TSA_Date').val(),
                           'SMTPNo': $('#SMTP_No').val(),
                           'SMTP_Date': $('#SMTP_Date').val(),
                           'NoOfPkg': $('#NO_PKG').val(),
                           'CargoType': $('#Cargo_Type').val(),
                           'CommodityId': $('#Commodity_Id').val(),
                           'CommodityName': $('#Commodity_Name').val(),
                           'CargoDescription': $('#Cargo_Des').val(),
                           'PkgType': $('#PKG_Type').val(),
                           'GR_WT': $('#GR_WT').val(),
                           'ImporterId': $('#Importer_Id').val(),
                           'ImporterName': $('#Importer_Name').val(),
                           'ShippingLineId': $('#ShippingLine_Id').val(),
                           'ShippingLineName': $('#ShippingLine_Name').val(),
                           'IGM_IMPORTER': '',
                           'AreaCBM': $('#Area_CBM').val() == "" ? 0 : $('#Area_CBM').val(),
                           'CIFValue': $('#CIF_Value').val(),
                           'IsProcessed': 0,
                           'CHAId': $('#CHAId').val(),
                           'CHAName': $('#CHAName').val(),

                       });
                    $('#OBLHBL_No').val('');
                    $('#OBLHBL_Date').val('');
                    $('#NO_PKG').val('');
                    $('#Commodity_Id').val(0);
                    $('#PKG_Type').val('');
                    $('#GR_WT').val('');
                    $('#Area_CBM').val(0);
                    $('#ShippingLine_Id').val(0);
                    $('#ShippingLine').val('');
                    $('#Importer_Id').val(0);
                    $('#CIF_Value').val('');
                    $('#TSA_No').val('');
                    $('#TSA_Date').val('');
                    $('#SMTP_No').val('');
                    $('#SMTP_Date').val('');
                    $('#Commodity_Name').val('');
                    $('#Importer_Name').val('');
                    $('#ShippingLine_Name').val('');
                    $('#Cargo_Des').val('');
                    $('#Cargo_Type').val(2);
                    $('#CHAId').val(0);
                    $('#CHAName').val('');

                }
                else {
                    $scope.OblEntryDetails.push
                       ({
                           'AddID': t2,
                           'ID': -1,
                           'OBLEntryId': 0,
                           'OBL_No': $('#OBLHBL_No').val(),
                           'OBL_Date': $('#OBLHBL_Date').val(),
                           'LineNo': $('#ShippingLine').val(),
                           'TSANO': $('#TSA_No').val(),
                           'TSA_Date': $('#TSA_Date').val(),
                           'SMTPNo': $('#SMTP_No').val(),
                           'SMTP_Date': $('#SMTP_Date').val(),
                           'NoOfPkg': $('#NO_PKG').val(),
                           'CargoType': $('#Cargo_Type').val(),
                           'CommodityId': $('#Commodity_Id').val(),
                           'CommodityName': $('#Commodity_Name').val(),
                           'CargoDescription': $('#Cargo_Des').val(),
                           'PkgType': $('#PKG_Type').val(),
                           'GR_WT': $('#GR_WT').val(),
                           'ImporterId': $('#Importer_Id').val(),
                           'ImporterName': $('#Importer_Name').val(),
                           'ShippingLineId': $('#ShippingLine_Id').val(),
                           'ShippingLineName': $('#ShippingLine_Name').val(),
                           'IGM_IMPORTER': '',
                           // 'AreaCBM': $('#Area_CBM').val(),
                           'AreaCBM': $('#Area_CBM').val() == "" ? 0 : $('#Area_CBM').val(),
                           'CIFValue': $('#CIF_Value').val(),
                           'IsProcessed': 0,
                           'CHAId': $('#CHAId').val(),
                           'CHAName': $('#CHAName').val(),
                       });
                    $('#OBLHBL_No').val('');
                    $('#OBLHBL_Date').val('');
                    $('#NO_PKG').val('');
                    $('#Commodity_Id').val(0);
                    $('#PKG_Type').val('');
                    $('#GR_WT').val('');
                    $('#Area_CBM').val(0);
                    $('#ShippingLine_Id').val(0);
                    $('#ShippingLine').val('');
                    $('#Importer_Id').val(0);
                    $('#CIF_Value').val('');
                    $('#TSA_No').val('');
                    $('#TSA_Date').val('');
                    $('#SMTP_No').val('');
                    $('#SMTP_Date').val('');
                    $('#Commodity_Name').val('');
                    $('#Importer_Name').val('');
                    $('#ShippingLine_Name').val('');
                    $('#Cargo_Des').val('');
                    $('#Cargo_Type').val(2);
                    $('#CHAId').val(0);
                    $('#CHAName').val('');
                }
            }
            else {
                $('#Rno').val('');

                $scope.OblEntryDetails[j].OBL_No = $('#OBLHBL_No').val();
                $scope.OblEntryDetails[j].OBL_Date = $('#OBLHBL_Date').val();

                $scope.OblEntryDetails[j].TSANO = $('#TSA_No').val();
                $scope.OblEntryDetails[j].TSA_Date = $('#TSA_Date').val();


                $scope.OblEntryDetails[j].SMTPNo = $('#SMTP_No').val();
                $scope.OblEntryDetails[j].SMTP_Date = $('#SMTP_Date').val();

                $scope.OblEntryDetails[j].ShippingLineId = $('#ShippingLine_Id').val();
                $scope.OblEntryDetails[j].ShippingLineName = $('#ShippingLine_Name').val();

                $scope.OblEntryDetails[j].NoOfPkg = $('#NO_PKG').val();
                $scope.OblEntryDetails[j].CargoType = $('#Cargo_Type').val();

                $scope.OblEntryDetails[j].CommodityId = $('#Commodity_Id').val();
                $scope.OblEntryDetails[j].CommodityName = $('#Commodity_Name').val();

                $scope.OblEntryDetails[j].CargoDescription = $('#Cargo_Des').val();
                $scope.OblEntryDetails[j].PkgType = $('#PKG_Type').val();

                $scope.OblEntryDetails[j].GR_WT = $('#GR_WT').val();
                $scope.OblEntryDetails[j].ImporterId = $('#Importer_Id').val();

                $scope.OblEntryDetails[j].ImporterName = $('#Importer_Name').val();
                $scope.OblEntryDetails[j].AreaCBM = $('#Area_CBM').val();

                $scope.OblEntryDetails[j].CIFValue = $('#CIF_Value').val();
                $scope.OblEntryDetails[j].CHAId = $('#CHAId').val();
                $scope.OblEntryDetails[j].CHAName = $('#CHAName').val();


                $scope.$applyAsync();
                $('#OBLHBL_No').val('');
                $('#OBLHBL_Date').val('');
                $('#NO_PKG').val('');
                $('#Commodity_Id').val(0);
                $('#PKG_Type').val('');
                $('#GR_WT').val('');
                $('#Area_CBM').val(0);
                $('#ShippingLine_Id').val(0);
                $('#ShippingLine').val('');
                $('#Importer_Id').val(0);
                $('#CIF_Value').val('');
                $('#TSA_No').val('');
                $('#TSA_Date').val('');
                $('#SMTP_No').val('');
                $('#SMTP_Date').val('');
                $('#Commodity_Name').val('');
                $('#Importer_Name').val('');
                $('#ShippingLine_Name').val('');
                $('#Cargo_Des').val('');
                $('#Cargo_Type').val(2);
                $('#CHAId').val(0);
                $('#CHAName').val('');
                //$("#OBLShippingLineModal").modal("hide");
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

            var NoOfPackage = $('#NoOfPackage').val();
            var GrossWeight = $('#GrossWeight').val();
            var Vessel = $('#Vessel').val();
            var Voyage = $('#Voyage').val();
            var ForeignLine = $('#ForeignLine').val();
            var Rotation = $('#Rotation').val();

            DSR_OBLEntryService.GetOBLDetails(ContainerNo, CFSCode, ContainerSize, IGM_No, IGM_Date, OBLEntryId).then(function (response) {
                debugger;
                $scope.OblEntryDetails = [];
                if (response.data.OblEntryDetailsList.length == 0) {
                    alert('No record found for given IGM No.');
                    return false;
                }
                $('#ContainerSize').val(response.data.ContainerSize);
                $('#IGM_No').val(response.data.IGM_No);
                $('#IGM_Date').val(response.data.IGM_Date);
                $('#TPNo').val(response.data.TPNo);
                $('#TPDate').val(response.data.TPDate);
                $('#MovementType').val(response.data.MovementType);
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
                         'TSANO': item.TSANo,
                         'TSA_Date': item.TSA_Date,
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
                         'ShippingLineId': item.ShippingLineId,
                         'ShippingLineName': item.ShippingLineName,
                         'IGM_IMPORTER': item.IGM_IMPORTER,
                         'AreaCBM': item.AreaCBM,
                         'CIFValue': item.CIFValue,
                         'CHAId': item.CHAId,
                         'CHAName': item.CHAName,

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
                    $("#ContainerSize").prop('disabled', true);
                    $("#CONTCBT").prop('disabled', true);
                }
                else {
                    $("#ContainerSize").prop('disabled', false);
                    $("#CONTCBT").prop('disabled', false);
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
                         'TSANO': item.TSANo,
                         'TSA_Date': item.TSA_Date,
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
                         'ShippingLineId': item.ShippingLineId,
                         'ShippingLineName': item.ShippingLineName,
                         'IsProcessed': item.IsProcessed,
                         'IGM_IMPORTER': item.IGM_IMPORTER,
                         'AreaCBM': item.AreaCBM,//$('#Area_CBM').val() == "" ? 0 : $('#Area_CBM').val(),
                         'CIFValue': item.CIFValue,
                         'CHAId': item.CHAId,
                         'CHAName': item.CHAName,
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
                if (j.ID == -1) {
                    $scope.OblEntryDetails.splice(val, 1);
                }
                else {
                    var isConfirmed = confirm("Are you sure to delete this record ?");
                    if (isConfirmed) {
                        DSR_OBLEntryService.OBLEntryDelete(j.OBL_No).then(function (res) {
                            alert(res.data.Message);
                        });
                        $scope.OblEntryDetails.splice(val, 1);
                    } else {
                        return false;
                    }

                }



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
            //if($('#IGM_No').val() == null || $('#IGM_No').val() == ''){
            //    alert('Please Enter IGM No.');
            //    return false;
            //}
            //if($('#IGM_Date').val() == null || $('#IGM_Date').val() == ''){
            //    alert('Please Select IGM Date');
            //    return false;
            //}
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
            //if ($('#ShippingLineId').val() == '' || $('#ShippingLineId').val() == 0) {
            //    alert('Please Select Shipping Line.');
            //    return false;
            //}
            var flag1 = 0;
            var flag2 = 0;
            var flag3 = 0;
            var flag4 = 0;
            var flag5 = 0;
            var flag6 = 0;
            angular.forEach($scope.OblEntryDetails, function (item) {
                if (item.OBL_No == '') {
                    flag1 = 1;
                }
                else if (item.Cargo_Type == "0" || item.Cargo_Type == "") {
                    flag3 = 1;
                }
                else if (item.PkgType == '') {
                    flag4 = 1;
                }
                else if (item.GR_WT == '') {
                    flag5 = 1;
                }
                else if (item.CommodityId == '0') {
                    flag6 = 1;
                }
                var x = item.OBL_Date;
                var reg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
                if (x.match(reg)) {
                    return true;
                }
                else {
                    alert("OBL Date should be dd/mm/yyyy");
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
                alert('Please Select CargoType');
                return false;
            }
            if (flag6 == 1) {
                alert('Please Select Commodity');
                return false;
            }
            if (flag4 == 1) {
                alert('Please Entry Pkg. Type');
                return false;
            }
            if (flag5 == 1) {
                alert('Please Enter Gross Wt.');
                return false;
            }
            
            debugger;
            var len = $scope.OblEntryDetails.length;
            if (len == 0) {
                alert('At least one record should required for obl details');
                return false;
            }

            if (confirm('Are you sure to save OBL Entry?')) {
                $('#btnSave').attr("disabled", true);
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
                    Vessel: $('#Vessel').val(),
                    CargoType: $('#CargoType').val(),
                    Voyage: $('#Voyage').val(),
                    ForeignLine: $('#ForeignLine').val(),
                    NoOfPackage: $('#NoOfPackage').val(),
                    GrossWeight: $('#GrossWeight').val(),
                    Rotation: $('#Rotation').val(),
                    
                }
                DSR_OBLEntryService.OBLEntrySave(Obj, JSON.stringify($scope.OblEntryDetails)).then(function (res) {
                    console.log(res);
                    alert(res.data.Message);
                    $('#btnSave').attr("disabled", true);
                    setTimeout(LoadOblEntry, 3000);
                });
            }
        }
        function LoadOblEntry() {
            $('#DivBody').load('/Import/DSR_OblEntry/OBLEntry');
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