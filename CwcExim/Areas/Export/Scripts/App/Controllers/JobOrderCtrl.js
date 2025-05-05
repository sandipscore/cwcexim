(function () {
    angular.module('CWCApp').
    controller('JobOrderCtrl', function ($scope, JobOrderService) {
        $scope.InvoiceNo = "";
        var t2 = 0;

        $scope.JODetails = [
            {
                'ID': t2,
                "TrainSummarySerial": '',
                "TrainSummaryID": 0,
                "TrainNo": '',
                "TrainDate": '',
                "PortId": 0,
                "Wagon_No": '',
                "Container_No": '',
                "CT_Size": '',
                "CustomSealNo": '',
                "ShippingLineNo": '',
                "ShippingLineId": 0,
                "ShippingLineName": '',
                "CargoType": 'Non HAZ',
                "ContainerLoadType": '',
                "TransportForm": '',
                "NoOfPackages": 0,
                "Line_Seal_No": '',
                "Cont_Commodity": '',
                "S_Line": '',
                "Ct_Tare": '',
                "Cargo_Wt": '',
                "Gross_Wt": '',
                "Ct_Status": '',
                "Destination": '',
                "Smtp_No": '',
                "Received_Date": '',
                "Genhaz": '',
                "Remarks": '',
                "CargoDescription": ''
            }];
        $scope.JODetails.length = 0;

        $(function () {
            GetTrainDetailsOnEditMode();
            JobOrderService.GetPort();
            
        });
        $scope.ContainerLoadTypeList = [

               {
                   "id": 'FCL',
                   "ContainerLoadType": "FCL",


               },
               {
                   "id": 'LCL',
                   "ContainerLoadType": "LCL"
               }
               
        ];

        $scope.CargoTypeList = [
            {
                "id": 'HAZ',
                "CargoType": "HAZ",


            },
            {
                "id": 'Non HAZ',
                "CargoType": "Non HAZ",

            }
        ];
        JobOrderService.GetPort().then(function (response) {
            debugger;
            
                $scope.TransportFromList = [{
                    "PortId": 0,
                    "PortName": "--Select--"
                }];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.TransportFromList.push(response.data[i])
                }
           
        });

        $scope.GetTrainDetails=function() {
            debugger;
            var TSumId = $('#TrainSummaryID').val();
            JobOrderService.GetTrainDetails(TSumId).then(function (response) {
                debugger;
                if ($('#ImpJobOrderId').val() == 0) {
                    $scope.JODetails = response.data;
                    $('#TrainDate').val(response.data[0].TrainDate);
                        angular.forEach($scope.JODetails, function (item) {
                          
                            item.CargoType = "Non HAZ";
                        });
                    }
                    if ($('#ImpJobOrderId').val() > 0) {
                        angular.forEach(response.data, function (item, i) {
                            debugger;
                            
                            $('#TrainDate').val(response.data[i].TrainDate)
                            $scope.JODetails.push
                                ({
                                    'ID': t2 + 1,
                                    "TrainSummarySerial": response.data[i].TrainSummarySerial,
                                    "TrainSummaryID": response.data[i].TrainSummaryID,
                                    "TrainNo": response.data[i].TrainNo,
                                    "TrainDate": response.data[i].TrainDate,
                                    "PortId": response.data[i].PortId,
                                    "Wagon": response.data[i].Wagon,
                                    "ContainerNo": response.data[i].ContainerNo,
                                    "SZ": response.data[i].SZ,
                                    "LineSeal": response.data[i].LineSeal,
                                    "Commodity": response.data[i].Commodity,
                                    "SLine": response.data[i].SLine,
                                    "ShippingLineName": response.data[i].ShippingLineName,
                                    "CargoType": 'Non HAZ',
                                    "ContainerLoadType": response.data[i].ContainerLoadType,
                                    "TransportForm": response.data[i].TransportForm,
                                    "NoOfPackages": response.data[i].NoOfPackages,
                                    "Line_Seal_No": response.data[i].Line_Seal_No,
                                    "Cont_Commodity": response.data[i].Cont_Commodity,
                                    "S_Line": response.data[i].S_Line,
                                    "TW": response.data[i].TW,
                                    "CW": response.data[i].CW,
                                    "GW": response.data[i].GW,
                                    "Ct_Status": response.data[i].Ct_Status,
                                    "PKGS": response.data[i].PKGS,
                                    "CustomSeal": response.data[i].CustomSeal,
                                    "Shipper": response.data[i].Shipper,
                                    "CHA": response.data[i].CHA,
                                    "CRRSBillingParty": response.data[i].CRRSBillingParty,
                                    "BillingParty": response.data[i].BillingParty,
                                    "StuffingMode": response.data[i].StuffingMode,
                                    "SBillNo": response.data[i].SBillNo,
                                    "Date": response.data[i].Date,
                                    "Origin": response.data[i].Origin,
                                    "POL": response.data[i].POL,
                                    "POD": response.data[i].POD,
                                    "DepDate": response.data[i].DepDate,
                                    
                                    "Remarks": response.data[i].Remarks,
                                    "CargoDescription": response.data[i].CargoDescription
                                });
                        });
                    }
                });
            //} 
        }
        function GetTrainDetailsOnEditMode()
        {
            debugger;
            if ($('#ImpJobOrderId').val() != 0) {
                JobOrderService.GetTrainDetailsOnEditMode($('#ImpJobOrderId').val()).then(function (response) {
                    debugger;
                    $scope.JODetails = response.data;
                });
            }
        }
        var ind = 0;
        $scope.onShippingLineChange = function (index) {
            debugger;
            ind = index;
        }
        $scope.SelectShippingLine = function (ShippingLineId, ShippingLineName) {
            debugger;
            $scope.JODetails[ind].ShippingLineId = ShippingLineId;
            $scope.JODetails[ind].ShippingLineName = ShippingLineName;
            $('#ShpngLinebox').val('');
            $("#slmodal").modal("hide");
            LoadEximTrader();
            $scope.$applyAsync();
            $("#slmodal").modal("hide");
        }
        $scope.ResetImpJODetails=function() {
            $('#lstYard,#lstYardWiseLctn').html('');
            //  $('#lstTrainNoNo #' + $('#FormOneId').val()).remove();
            $('#FormOneId,#FormOneDetailId,#ImpJobOrderId').val('');
            $('#PickUpLocation,#Remarks,#TrainNo,#JobOrderNo,#TrainNobox').val('');
            //$('#DivFormOneDet').html('');
            //$('.data-valmsg-summary').html('');
            // $('#lstTrainNoNo').html('');
            //$('#joborderCheck').val(new Date());
            //$('#trainCheck').val(new Date());
            //$('#TrainDate').val(new Date());
            
            $scope.JODetails.length = 0;
            //$scope.GetTrainDetails();
        }
        $scope.Delete = function (val) {
            debugger;
            var len = $scope.JODetails.length;
            if (len == 1) {
                alert('At least one record should required');
            }
            else {
                $scope.JODetails.splice(val, 1);
            }
        }
        var Obj = {};
        $scope.OnJobOrderSave = function () {
            debugger;
            if ($('#PickUpLocation').val() == '') {
                alert('Please Select Pick Up Location');
                return false;
            }
            var x = $('#JobOrderDate').val();
            var reg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
            if (x.match(reg)) {
                //return true;
            }
            else {
                alert("Job Order Date should be dd/mm/yyyy");
                return false;
            }

            if ($('#TrainNo').val() == '') {
                alert('Please Select TrainNo.');
                return false;
            }

            var y = $('#TrainDate').val();
            
            if (y.match(reg)) {
                //return true;
            }
            else {
                alert("Train Date should be dd/mm/yyyy");
                return false;
            }

            var flag1 = 0;
            var flag2 = 0;
            var flag3 = 0;
            var flag4 = 0;
            angular.forEach($scope.JODetails, function (item) {
                if (item.ShippingLineName=='') {
                    flag1 = 1;
                }
                else if (item.CargoType=='') {
                    flag2 = 1;
                }
                else if (item.PortId == 0) {
                    flag3 = 1;
                }
                else if (item.Ct_Status == '') {
                    flag4 = 1;
                }
            });
            if(flag1 == 1){
                alert('Please Select All Shipping Line Name');
                return false;
            }
            if (flag2 == 1) {
                alert('Please Select All CargoType');
                return false;
            }
            if (flag3 == 1) {
                alert('Please Select Transport From');
                return false;
            }
            if (flag4 == 1) {
                alert('Please Select Container Load Type');
                return false;
            }
            debugger;
            if (confirm('Are you sure to save Job Order?')) {
                debugger;
                Obj = {
                    ImpJobOrderId: $('#ImpJobOrderId').val() == undefined ? 0 : $('#ImpJobOrderId').val(),
                    PickUpLocation: $('#PickUpLocation').val(),
                    JobOrderNo: $('#JobOrderNo').val(),
                    JobOrderDate: $('#JobOrderDate').val(),
                    JobOrderFor: $('#rbLC').val(),
                    TransportBy: $('#Rail').val(),
                    TrainSummaryID: $('#TrainSummaryID').val(),
                    FormOneDetailId: $('#FormOneDetailId').val(),
                    TrainNo:  $('#TrainNo').val(),
                    TrainDate: $('#TrainDate').val(),
                    TrainSummarySerial: $('#TrainSummarySerial').val(),
                    TrainSummaryID: $('#TrainSummaryID').val(),
                }
                
                JobOrderService.OnJobOrderSave(Obj,JSON.stringify($scope.JODetails)).then(function (res) {
                    console.log(res);
                    alert(res.data.Message);
                    $('#btnSave').attr("disabled", true);
                    setTimeout(function () {
                        $('#DivBody').load('/Export/Ppg_CWCExport/CreateJobOrder');
                    }, 5000)
                });
            }
        }


    });
})();