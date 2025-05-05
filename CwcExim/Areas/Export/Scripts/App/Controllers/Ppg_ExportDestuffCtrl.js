(function () {
    angular.module('CWCApp').
    controller('Ppg_ExpDestuffCtrl', function ($scope, Ppg_ExpDestuffService) {

        $scope.SQMDisable = 0;
        var t2 = 0;
        $scope.LocationList = [];
        $scope.DestuffDetails = [
          { 
              'ID': t2,
              "SBNo": '',
              "SBDate": '',
              "EXPId": 0,
              "Exporter": '',
              "cargoDesc": '',
              "CommodityId": 0,
              "Commodity": '',
              "CargoType": 'Non HAZ', 
              "GrWt": 0,
              "CUM": 0,
              "Unit": 0,
              "FOB": 0,
              "ReservedSQM": 0,
              "UnReservedSQM": 0,
              "LocationId": '',
              "Location": ''
          }];
        $scope.DestuffDetails.length = 0;

        $scope.CargoTypeList = [
          {
              "id": '1',
              "CargoType": "HAZ",


          },
          {
              "id": '2',
              "CargoType": "Non-HAZ",

          }
        ];
        var ind = 0;
        $scope.onCommodityChange = function (index) {
            debugger;
            ind = index;
        }

        $scope.SelectCommodity = function (CommodityId, CommodityName) {
            debugger;
            $scope.DestuffDetails[ind].CommodityId = CommodityId;
            $scope.DestuffDetails[ind].Commodity = CommodityName;
            $('#CommodityBox').val('');
            $("#CommodityModal").modal("hide");
            $scope.$applyAsync();
            $("#slmodal").modal("hide");
        }

        $scope.onExporterChange = function (index) {
            debugger;
            ind = index;
        }

        $scope.SelectExporter = function (ExpId, Exporter) {
            debugger;
            $scope.DestuffDetails[ind].EXPId = ExpId;
            $scope.DestuffDetails[ind].Exporter = Exporter;
            $('#ExporterBox').val('');
            $("#ExporterModal").modal("hide");
            $scope.$applyAsync();
            $("#ExporterModal").modal("hide");
        }

        $scope.onLocationSearch = function () {
            debugger;
            var GodownId = $('#GodownId').val();
            Ppg_ExpDestuffService.GetLocation(GodownId).then(function (response) {
                debugger;
                $scope.LocationList = response.data;
                var LocVal = $scope.DestuffDetails[ind].Location;
                var selectedText = "";
                var Locarr;
                if (LocVal != '') {
                    selectedText = LocVal;
                    Locarr = selectedText.split(",");
                }
                var htm = '';
                var flag = -1;
                $.each($scope.LocationList, function (i, data) {
                    //if (data.IsOccupied)
                    //    htm += '<div class="col-md-4"><div class="boolean-container"><input type="checkbox" onclick="CheckBoxChange()" disabled id="' + data.LocationId + '"/><label for="' + data.LocationId + '">' + data.Row + ' ' + data.Column + '<i class="square" style="margin-left:10px;"></i></label></div></div>';
                    //else
                    if (LocVal != '') {

                        for (var j = 0; j < Locarr.length; j++) {
                            if (Locarr[j] == data.LocationName) {
                                flag = i;
                                htm += '<div class="col-md-4"><div class="boolean-container"><input type="checkbox" onclick="CheckBoxChange()" id="Loc' + data.LocationId + '" checked/><label for="Loc' + data.LocationId + '">' + data.LocationName + '<i class="square" style="margin-left:10px;"></i></label></div></div>';

                            }

                        }


                        if (i != flag) {
                            htm += '<div class="col-md-4"><div class="boolean-container"><input type="checkbox" onclick="CheckBoxChange()" id="Loc' + data.LocationId + '"/><label for="Loc' + data.LocationId + '">' + data.LocationName + '<i class="square" style="margin-left:10px;"></i></label></div></div>';

                        }

                    }
                    else {
                        htm += '<div class="col-md-4"><div class="boolean-container"><input type="checkbox" onclick="CheckBoxChange()" id="Loc' + data.LocationId + '"/><label for="Loc' + data.LocationId + '">' + data.LocationName + '<i class="square" style="margin-left:10px;"></i></label></div></div>';
                    }
                });
                $('#DivLocationDet').html(htm);
            });


        }

        $scope.onLocationChange = function (index) {
            debugger;
            ind = index;
        }

        $scope.SelectLoaction = function (obj) {
            debugger;
            var LocationId = obj.LocationId;
            //var PartyName = obj.PartyName.split('_');
            var Location = obj.LocationName;
            $scope.DestuffDetails[ind].LocationId = LocationId;
            $scope.DestuffDetails[ind].Location = Location;
            $('#LocationModal').modal('hide');
        };

        $scope.GetLocation = function (obj) {
            debugger;

            $scope.DestuffDetails[ind].LocationId = obj;
            $scope.DestuffDetails[ind].Location = obj;

        };


        $scope.ClearLoaction = function () {
            debugger;
            angular.forEach($scope.DestuffDetails, function (item, i) {
                $scope.DestuffDetails[i].LocationId = 0;
                $scope.DestuffDetails[i].Location = '';
            });
            $scope.$applyAsync();
        }

        $scope.GetSBDetails = function () {
            debugger;
            $scope.DestuffDetails = [];
            var CFSCodeVal = $('#CFSCode').val();
            var OperationType = $('#OperationType').val();
            Ppg_ExpDestuffService.GetSBDetails(CFSCodeVal, OperationType).then(function (response) {
                debugger;
                //if ($('#ImpJobOrderId').val() == 0) {
                //    $scope.JODetails = response.data;
                //    $('#TrainDate').val(response.data[0].TrainDate);
                //    angular.forEach($scope.JODetails, function (item) {

                //        item.CargoType = "Non HAZ";
                //    });
                //}

                angular.forEach(response.data.Data, function (item, i) {
                    debugger;
                    $scope.DestuffDetails.push
                        ({

                            'ID': t2 + 1,
                            "SBNo": response.data.Data[i].SBNo,
                            "SBDate": response.data.Data[i].SBDate,
                            "EXPId": response.data.Data[i].EXPId,
                            "Exporter": response.data.Data[i].Exporter,
                            "cargoDesc": response.data.Data[i].cargoDesc,
                            "CommodityId": response.data.Data[i].CommodityId,
                            "Commodity": response.data.Data[i].Commodity,
                            "CargoType": response.data.Data[i].CargoType,
                            "GrWt": response.data.Data[i].GrWt,
                            "CUM": response.data.Data[i].CUM,
                            "Unit": response.data.Data[i].Unit,
                            "FOB": response.data.Data[i].FOB,
                            "ReservedSQM": 0,
                            "UnReservedSQM": response.data.Data[i].UnReservedSQM,
                            "LocationId": 0,
                            "Location": '',
                            'CartingRegisterDtlId': response.data.Data[i].CartingRegisterDtlId,
                            'CHAId': response.data.Data[i].CHAId


                        });
                    $('#btnSave').attr('disabled', false);

                });

            });
            //} 
        }

        $scope.SelectSpace = function (Type) {
            debugger;
            if (Type == 'ReservedSpace') {
                $('#UnReservedSQM').val('0');
                $scope.SQMDisable = 1;
            }
            else if (Type == 'GeneralSpace') {
                $('#UnReservedSQM').val('0');
                $scope.SQMDisable = 0;
            }
            $scope.$applyAsync();
            $('#SpaceType').val(Type);
        }

        $scope.DestuffingSave = function () {
            debugger;




            if ($('#ContainerNo').val() == null || $('#ContainerNo').val() == '') {
                alert('Please Select ContainerNo');
                return false;
            }
            if ($('#CFSCode').val() == null || $('#CFSCode').val() == '') {
                alert('Please Select CFSCode');
                return false;
            }
            if ($('#GodownId').val() == '' || $('#GodownId').val() == 0) {
                alert('Please Select Godown.');
                return false;
            }
           

            var From = $('#RefDate').val();
            var To = $('#Destuffingdate').val().toString("dd/mm/yyyy");
            var SF = From.split('/');
            var ST = To.split('/');
            var STyear = ST[2].split(" "); //Split Time and Year From InvoiceDate;
            //var FromDate = new Date(SF[2], (Number(SF[1]) - 1), SF[0]);
            //var ToDate = new Date(ST[2], (Number(ST[1]) - 1), ST[0]);
            var FromDate = SF[1] + '/' + SF[0] + '/' + SF[2]; // Createing Date In DD/MM/YYYY;
            var ToDate = ST[1] + '/' + ST[0] + '/' + STyear[0]; // Createing Date In DD/MM/YYYY;
            var dt1 = new Date(FromDate);
            var dt2 = new Date(ToDate);
            if (dt1 > dt2) {
                alert("Destuffing Date Can't be Less than Stuffing Date or Loaded Container Request Date !!");
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

            var SpaceType = $("input[name='SpaceType']:checked").val();

            angular.forEach($scope.DestuffDetails, function (item) {
                if (item.OBL_No == '') {
                    flag1 = 1;

                }
                else if (item.EXPId == '0') {
                    flag2 = 1;

                }
                else if (item.CargoType == "0" || item.CargoType == "") {
                    flag3 = 1;
                }
                else if (item.GrWt == '' || item.GrWt == 0) {
                    flag4 = 1;

                }
                else if (item.Unit == '' || item.Unit == 0) {
                    flag5 = 1;

                }
                else if (item.FOB == '' || item.FOB == 0) {
                    flag6 = 1;

                }
                else if ((SpaceType == 'ReservedSpace' && item.ReservedSQM == '' && item.ReservedSQM != 0)) {
                    flag7 = 1;

                }
                else if (item.CommodityId == '0') {
                    flag8 = 1;

                }
                else if (item.LocationId == '0') {
                    flag9 = 1;

                }
                else if ((SpaceType == 'GeneralSpace' && item.UnReservedSQM == '') || (SpaceType == 'GeneralSpace' && item.UnReservedSQM == 0)) {
                    flag7 = 1;

                }
            });


            //if (flag1 == 1) {
            //    alert('Please Enter All SB No.');
            //    return false;
            //}
            //if (flag2 == 1) {
            //    alert('Please Select Exporter');
            //    return false;
            //}
            //if (flag3 == 1) {
            //    alert('Please Select CargoType');
            //    return false;
            //}

            //if (flag4 == 1) {
            //    alert('Please Enter Gross Wt.');
            //    return false;
            //}
            //if (flag5 == 1) {
            //    alert('Please Enter No Of Unit.');
            //    return false;
            //}
            //if (flag6 == 1) {
            //    alert('Please Enter FOB');
            //    return false;
            //}
            //if (flag7 == 1 || flag10 == 1) {
            //    alert('Please Enter SQM.');
            //    return false;
            //}
            //if (flag8 == 1) {
            //    alert('Please Select Commodity');
            //    return false;
            //}
            //if (flag9 == 1) {
            //    alert('Please Select Location');
            //    return false;
            //}

            debugger;
            var len = $scope.DestuffDetails.length;
            if (len == 0) {
                alert('At least one record should required for SB details');
                return false;
            }

            if (confirm('Are you sure to save Destuffing Entry?')) {
                debugger;
                $('#btnSave').attr("disabled", true);
                Obj = {
                    DestuffingId: $('#DestuffingId').val() == undefined ? 0 : $('#DestuffingId').val(),
                    CFSCode: $('#CFSCode').val(),
                    ContainerNo: $('#ContainerNo').val(),
                    Size: $('#Size').val() == "" ? 0 : $('#Size').val(),
                    DestuffingDate: $('#Destuffingdate').val(),
                    GodownId: $('#GodownId').val(),
                    GodownName: $('#GodownName').val(),
                    ShippingLineId: $('#ShippingLineId').val(),
                    ShippingLineName: $('#ShippingLineName').val(),
                    Remarks: $('#Remarks').val(),
                    RefNo: $('#RefNo').val(),
                    SpaceType: SpaceType,
                    OperationType: $('#OperationType').val(),
                }
                Ppg_ExpDestuffService.DestuffingEntrySave(Obj, JSON.stringify($scope.DestuffDetails)).then(function (res) {
                    console.log(res);
                    alert(res.data.Message);
                    $('#btnSave').attr("disabled", true);
                    setTimeout(LoadDestuffingEntry, 3000);
                });
            }
        }
        function LoadDestuffingEntry() {
            $('#DivBody').load('/Export/Ppg_CWCExportV2/CreateExportDestuffing');
        }

        $(function () {
            $scope.Action = false;
            GetSBDetailsOnEditMode();
        });

        function GetSBDetailsOnEditMode() {
            debugger;
            if ($('#DestuffingId').val() != 0) {
                $scope.Action = false;

                var SerializedData = $.parseJSON($('#StringifiedText').val());
                //$scope.OblEntryDetails = $.parseJSON(SerializedData);
                angular.forEach(SerializedData, function (item) {
                    debugger;
                    $scope.DestuffDetails.push
                     ({
                         'DestuffingDtlId': item.DestuffingDtlId,
                         "SBNo": item.SBNo,
                         "SBDate": item.SBDate,
                         "EXPId": item.EXPId,
                         "Exporter": item.Exporter,
                         "cargoDesc": item.cargoDesc,
                         "CommodityId": item.CommodityId,
                         "Commodity": item.Commodity,
                         "CargoType": item.CargoType,
                         "GrWt": item.GrWt,
                         "CUM": item.CUM,
                         "Unit": item.Unit,
                         "FOB": item.FOB,
                         "ReservedSQM": item.ReservedSQM,
                         "UnReservedSQM": item.UnReservedSQM,
                         "LocationId": item.LocationId,
                         "Location": item.Location

                     });
                    $('#btnSave').attr('disabled', false);
                });
            }
        }


    });
})();

