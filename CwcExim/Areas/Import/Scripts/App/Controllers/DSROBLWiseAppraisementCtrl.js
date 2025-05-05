(function () {
    angular.module('CWCApp').
    controller('DSROBLWiseAppraisementCtrl', function ($scope, DSROBLWiseAppraisementService) {
        document.getElementById("DivContainerDtl").style.display = 'block';

        $scope.ActiveControl = false;
        $scope.FinalOblContainerDetails = [];
        $scope.OblContainerDetails = [
        {
            "CustomAppraisementDtlId": 0,
            "ContainerNo": '',
            "CFSCode": '',
            "Size": '',
            "NoOfPkg": 0,
            "Gr_Wt": 0,
            "CValue": 0,
            "CDuty": 0,
            "ManualWT": 0,
            "MechanicalWT": 0,
            "RMSValue": '1',
            "InterShifting": 0,
            "LiftOn": 0,
            "LiftOff": 0,
            "Reworking": 0,
            "Weighment": 0,
            "ODCType": 0,
            "CargoType": '',
            "LCLFCL": '',
            "ContainerType": 0

        }];

        $scope.OblContainerDetails.length = 0;
        $scope.RMSValueList = [
           {
               "id": 1,
               "RMSValue": "Upto 25%",
           },
           {
               "id": 2,
               "RMSValue": "Beyond 25%",
           },
           {
               "id": 3,
               "RMSValue": "RMS Non Exim",
           }
        ];

        $(function () {
            if ($('#CustomAppraisementId').val() != 0) {
                GetOBLWiseAppraisementOnEditMode();
            }
        });
        $scope.GetContainerDetails = function () {
            debugger;
            document.getElementById("DivContainerDtl").style.display = 'block';
            //document.getElementById("DivFormOneDett").style.display = '';
            var OBLNo = $('#OBLNo').val();
            var OBLDate = $('#OBLDate').val();
            if (OBLNo != '' && OBLDate != '')
            {
                DSROBLWiseAppraisementService.GetContainerDetails(OBLNo, OBLDate).then(function (response) {

                    debugger;

                    $scope.OblContainerDetails.length = 0;
                    var Pkt = 0, OValue = 0, OGWt = 0, ODuty = 0, PktRemainder = 0, CValueRemainder = 0, CGWtRemainder = 0, CDutyRemainder = 0;
                    var NPkg = 0, NValue = 0, NGWt = 0, NDuty = 0;
                    var Cont = response.data.length;
                    if (Cont > 0) {
                        Pkt = $('#NoOfPackages').val();
                        OValue = $('#CIFValue').val();
                        OGWt = $('#GrossWeight').val();
                        ODuty = $('#Duty').val();

                        var FPkt = Math.floor(Number(Pkt) / Number(Cont));
                        PktRemainder = Number(Pkt) - (FPkt * Cont);

                    }
                    if (response.data.length > 0) {
                        if (response.data.filter(x=>x.Vessel != '')[0])
                            $('#Vessel').val(response.data.filter(x=>x.Vessel != '')[0].Vessel);
                        else
                            $('#Vessel').val('');
                        if (response.data.filter(x=>x.Voyage != '')[0])
                            $('#Voyage').val(response.data.filter(x=>x.Voyage != '')[0].Voyage);
                        else
                            $('#Voyage').val('');
                        if (response.data.filter(x=>x.Rotation != '')[0])
                            $('#Rotation').val(response.data.filter(x=>x.Rotation != '')[0].Rotation);
                        else
                            $('#Rotation').val('');
                        if (response.data.filter(x=>x.ForeignLine != '')[0])
                            $('#ForeignLine').val(response.data.filter(x=>x.ForeignLine != '')[0].ForeignLine);
                        else
                            $('#ForeignLine').val('');
                    }

                    angular.forEach(response.data, function (item, i) {
                        //debugger;
                        if (PktRemainder > 0) {
                            if (i == 0) {
                                NPkg = (FPkt + PktRemainder);
                            }
                            else {
                                NPkg = FPkt
                            }
                        }
                        else {
                            NPkg = FPkt
                        }

                        var FCValue = (((Number(OValue) / Number(Pkt))) * NPkg).toFixed(2);
                        var FGWt = (((Number(OGWt) / Number(Pkt))) * NPkg).toFixed(2);
                        var FDuty = (((Number(ODuty) / Number(Pkt))) * NPkg).toFixed(2);

                        if (response.data[i].InterShifting == 1) {
                            var InterShifting = true;
                        }
                        else {
                            var InterShifting = false;
                        }
                        if (response.data[i].LiftOn == 1) {
                            var LiftOn = true;
                        }
                        else {
                            var LiftOn = false;
                        }

                        if (response.data[i].LiftOff == 1) {
                            var LiftOff = true;
                        }
                        else {
                            var LiftOff = false;
                        }

                        if (response.data[i].Reworking == 1) {
                            var Reworking = true;
                        }
                        else {
                            var Reworking = false;
                        }

                        if (response.data[i].Weighment == 1) {
                            var Weighment = true;
                        }
                        else {
                            var Weighment = false;
                        }
                        //console.log(response.data);
                        $scope.OblContainerDetails.push
                            ({
                                "CustomAppraisementDtlId": 0,
                                "ContainerNo": response.data[i].ContainerNo,
                                "CFSCode": response.data[i].CFSCode,
                                "Size": response.data[i].Size,
                                "NoOfPkg": NPkg,
                                "Gr_Wt": FGWt,
                                "CValue": FCValue,
                                "CDuty": FDuty,
                                "ManualWT": response.data[i].ManualWT,
                                "MechanicalWT": response.data[i].MechanicalWT,
                                "RMSValue": $('#RMSValue').val(),
                                "InterShifting": InterShifting,//response.data[i].InterShifting,
                                "LiftOn": LiftOn,//response.data[i].LiftOn,
                                "LiftOff": LiftOff,//response.data[i].LiftOff,
                                "Reworking": Reworking,//response.data[i].Reworking,
                                "Weighment": Weighment,//response.data[i].Weighment,
                                "ODCType": response.data[i].ODCType,
                                "CargoType": response.data[i].CargoType,
                                "LCLFCL": response.data[i].LCLFCL,
                                "ContainerType": response.data[i].ContainerType,
                                "WithoutDOSealNo": response.data[i].WithoutDOSealNo,
                                "ShippingLineSealNo": response.data[i].ShippingLineSealNo,
                            });
                    });
                    // }
                });
            }
            
            //} 
        }
        //--------------------------------
        var Obj = {};
        $scope.OnAppraisementEntrySave = function () {

            var IsDO = 0;
            if ($("#WithoutDO").prop("checked") == true) {
                IsDO = 2;
            }
            else {
                IsDO = 1;
            }
            if (confirm('Are you sure to save Appraisement Entry?')) {
                //debugger;
                Obj = {
                    CustomAppraisementId: $('#CustomAppraisementId').val() == undefined ? 0 : $('#CustomAppraisementId').val(),
                    ApplicationForApp: 0,
                    AppraisementDate: $('#AppraisementDate').val(),
                    CHAId: $('#CHAId').val(),
                    Vessel: $('#Vessel').val(),
                    operationType: 1,
                    Voyage: $('#Voyage').val(),
                    Rotation: $('#Rotation').val(),
                    Fob: $('#CIFValue').val(),
                    GrossDuty: $('#Duty').val(),
                    DeliveryType: 0,
                    IsDO: IsDO,
                    LineNo: $('#LineNo').val(),
                    ShippingLineId: $('#ShippingLineId').val(),
                    OBLNo: $('#OBLNo').val(),
                    OBLDate: $('#OBLDate').val(),
                    BOENo: $('#BOENo').val(),
                    BOEDate: $('#BOEDate').val(),
                    NoOfPackages: $('#NoOfPackages').val(),
                    GrossWeight: $('#GrossWeight').val(),
                    ImporterId: $('#ImporterId').val(),
                    ForeignLine: $('#ForeignLine').val()
                }
                $scope.FinalOblContainerDetails = [];
                if ($scope.OblContainerDetails.length > 0) {

                    angular.forEach($scope.OblContainerDetails, function (item, i) {
                        if ($scope.OblContainerDetails[i].InterShifting == true) {
                            var InterShifting = 1;
                        }
                        else {
                            var InterShifting = 0;
                        }
                        if ($scope.OblContainerDetails[i].LiftOn == true) {
                            var LiftOn = 1;
                        }
                        else {
                            var LiftOn = 0;
                        }

                        if ($scope.OblContainerDetails[i].LiftOff == true) {
                            var LiftOff = 1;
                        }
                        else {
                            var LiftOff = 0;
                        }

                        if ($scope.OblContainerDetails[i].Reworking == true) {
                            var Reworking = 1;
                        }
                        else {
                            var Reworking = 0;
                        }

                        if ($scope.OblContainerDetails[i].Weighment == true) {
                            var Weighment = 1;
                        }
                        else {
                            var Weighment = 0;
                        }

                        $scope.FinalOblContainerDetails.push
                        ({
                            "CustomAppraisementDtlId": $scope.OblContainerDetails[i].CustomAppraisementDtlId,
                            "ContainerNo": $scope.OblContainerDetails[i].ContainerNo,
                            "CFSCode": $scope.OblContainerDetails[i].CFSCode,
                            "Size": $scope.OblContainerDetails[i].Size,
                            "NoOfPackages": $scope.OblContainerDetails[i].NoOfPkg,
                            "GrossWeight": $scope.OblContainerDetails[i].Gr_Wt,
                            "CIFValue": $scope.OblContainerDetails[i].CValue,
                            "Duty": $scope.OblContainerDetails[i].CDuty,
                            "RMSValue": $('#RMSValue').val(),//$scope.OblContainerDetails[i].RMSValue,
                            "ManualWT": $scope.OblContainerDetails[i].ManualWT,
                            "MechanicalWT": $scope.OblContainerDetails[i].MechanicalWT,
                            "InterShifting": InterShifting,
                            "LiftOn": LiftOn,
                            "LiftOff": LiftOff,
                            "Reworking": Reworking,
                            "Weighment": Weighment,
                            "BOENo": $('#BOENo').val(),
                            "BOEDate": $('#BOEDate').val(),
                            "OBLNo": $('#OBLNo').val(),
                            "OBLDate": $('#OBLDate').val(),
                            "CHANameId": $('#CHAId').val(),
                            "ImporterId": $('#ImporterId').val(),
                            "CargoDescription": $('#hdnCargoDescription').val(),
                            "WithoutDOSealNo": $scope.OblContainerDetails[i].WithoutDOSealNo,
                            "ShippingLineSealNo": $scope.OblContainerDetails[i].ShippingLineSealNo,
                            "ContainerType": $scope.OblContainerDetails[i].ContainerType,
                            "CargoType": $scope.OblContainerDetails[i].CargoType,
                            "ODCType": $scope.OblContainerDetails[i].ODCType,
                            "LCLFCL": $scope.OblContainerDetails[i].LCLFCL,
                            "LineNo": $('#LineNo').val(),
                            "CargoDeliveryType": ''
                        });
                    });
                }


                DSROBLWiseAppraisementService.AddEditOBLCustomAppraisement(Obj, JSON.stringify($scope.FinalOblContainerDetails), $('#CAOrdDtlXml').val()).then(function (res) {
                    //console.log(res);
                    //alert(res.data.Message);
                    //debugger;
                    if (res.data.Status == 3 || res.data.Status == 6) {
                        $('#btnSave').attr("disabled", false);
                        alert(res.data.Message);
                        return false;
                    }
                    else {
                        alert(res.data.Message);
                        $('#btnSave').attr("disabled", true);
                        ResetAllFields();
                        setTimeout(LoadOblWiseAppraisement, 3000);
                        $scope.FinalOblContainerDetails.length = 0;
                        $scope.OblContainerDetails.length = 0;

                    }

                });
            }
        }


        function LoadOblWiseAppraisement() {
            DSROBLWiseAppraisementService.LoadOBLCustomAppraisement(0).then(function (res) {
            });
        }


        //-------------------------------

        //---------------------------------------
        function GetOBLWiseAppraisementOnEditMode() {
            //debugger;
            var action = $('#Action').val();
            if ($('#CustomAppraisementId').val() != 0) {
                DSROBLWiseAppraisementService.GetContainerDetailsOnEditMode($('#CustomAppraisementId').val()).then(function (response) {
                    //debugger;
                    console.log(response.data);

                    $('#AppraisementDate').val(),
                    $('#CHAId').val(),
                    $('#Vessel').val(),
                    $('operationType').val(),
                    $('#Voyage').val(),
                    $('#Rotation').val(),
                    $('#CIFValue').val(response.data.Fob),
                    $('#Duty').val(response.data.GrossDuty),
                    $('#RMSValue').val(response.data.RMSValue),
                    $('#ODCType').val(response.data.ODCType),
                    $('#LineNo').val(response.data.LineNo),
                    $('#ShippingLineId').val(),
                    $('#OBLNo').val(response.data.OBLNo),
                    $('#OBLDate').val(response.data.OBLDate),
                    $('#BOENo').val(response.data.BOENo),
                    $('#BOEDate').val(response.data.BOEDate),
                    $('#NoOfPackages').val(response.data.NoOfPackages),
                    $('#GrossWeight').val(response.data.GrossWeight),
                    $('#ImporterId').val(response.data.ImporterId),
                    $('#Importer').val(response.data.Importer)
                    //$('#IsDO').val(response.data.IsDO)
                    //$('#CustomAppraisementXML').val();
                    //$('#CAOrdDtlXml').val(response.data.LstCustomAppraisementOrdDtl);
                    if (response.data.IsDO == 1) {
                        document.getElementById("DivDeliveryOrderDetails").style.display = "";
                    }

                    var SerializedData = response.data.LstAppraisementWFLD;
                    angular.forEach(SerializedData, function (item) {
                        //debugger;
                        if (item.InterShifting == 1) {
                            var InterShifting = true;
                        }
                        else {
                            var InterShifting = false;
                        }
                        if (item.LiftOn == 1) {
                            var LiftOn = true;
                        }
                        else {
                            var LiftOn = false;
                        }

                        if (item.LiftOff == 1) {
                            var LiftOff = true;
                        }
                        else {
                            var LiftOff = false;
                        }

                        if (item.Reworking == 1) {
                            var Reworking = true;
                        }
                        else {
                            var Reworking = false;
                        }

                        if (item.Weighment == 1) {
                            var Weighment = true;
                        }
                        else {
                            var Weighment = false;
                        }
                        $scope.OblContainerDetails.push
                         ({
                             "CustomAppraisementDtlId": item.CustomAppraisementDtlId,
                             "ContainerNo": item.ContainerNo,
                             "CFSCode": item.CFSCode,
                             "Size": item.Size,
                             "NoOfPkg": item.NoOfPackages,
                             "Gr_Wt": item.GrossWeight,
                             "CValue": item.CIFValue,
                             "CDuty": item.Duty,
                             "RMSValue": item.RMSValue,//$scope.OblContainerDetails[i].RMSValue,
                             "ManualWT": item.ManualWT,
                             "MechanicalWT": item.MechanicalWT,
                             "InterShifting": InterShifting,
                             "LiftOn": LiftOn,
                             "LiftOff": LiftOff,
                             "Reworking": Reworking,
                             "Weighment": Weighment,
                             "BOENo": item.BOENo,
                             "BOEDate": item.BOEDate,
                             "OBLNo": item.OBLNo,
                             "OBLDate": item.OBLDate,
                             "CHANameId": item.CHAId,
                             "ImporterId": item.ImporterId,
                             "CargoDescription": item.CargoDescription,
                             "WithoutDOSealNo": item.WithoutDOSealNo,
                             "ContainerType": item.ContainerType,
                             "CargoType": item.CargoType,
                             "ODCType": item.ODCType,
                             "LCLFCL": item.LCLFCL,
                             "LineNo": item.LineNo,
                             "CargoDeliveryType": item.CargoDeliveryType,
                             "ShippingLineSealNo": item.ShippingLineSealNo,
                         });

                    });
                });

            }
        }
        //--------------------------------------
    });
})();