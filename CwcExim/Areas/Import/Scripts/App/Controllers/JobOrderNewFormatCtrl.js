(function () {
    angular.module('CWCApp').
    controller('JobOrderNewFormatCtrl', function ($scope, JobOrderNewFormatService) {
        $scope.InvoiceNo = "";
        var t2 = 0;
        var conind = 0;
        $scope.JODetails = [
            {
                'ID': t2,
                "Container_No": '',
                "CT_Size": '',
                "CustomSealNo": '',
                "CargoDescription": '',
                "ShippingLineId": '',
                "ShippingLineName": '',
                "Ct_Tare": '',
                "Cargo_Wt": '',
                "Gross_Wt": '',
                "ContainerLoadType": '',
                "Destination": '',
                "Smtp_No": '',
                "Received_Date": '',

                "TrainNo": '',
                "TrainDate": '',
                "PortId": '',
                "Wagon_No": '',
                "Line_Seal_No": '',
                "S_Line": '',
                "Genhaz": '',
                "TrainSummaryID": '',
                "TrainSummarySerial": '',
                "Cont_Commodity": '',
                "CargoType":''
            }];
       $scope.JODetails.length = 0;
        debugger;
        $scope.isView = false;
        $scope.ishide = true;
        $scope.isSave = true;

        $scope.btnSaveFlag = false;
        $scope.btnflag = true;
        $scope.btnflag = true;



        var now = new Date();
        var datestring = (now.getDate() > 9 ? '' : '0') + now.getDate() + '/' + (now.getMonth() > 8 ? '' : '0') + (now.getMonth() + 1) + '/' + now.getFullYear();
        $('#FormOneDate').val(datestring);
        if ($('#ImpJobOrderId').val() > 0) {
            GetJobOrderByRoadByOnEditMode();
        }
        if ($('#hdnView').val() == "Edit") {

            $scope.ishide = false;
            $scope.isSave = true;
        }
        if ($('#hdnView').val() == "View") {
            $("#CONTCBT").prop("disabled", true);
            $("#TransportBy").prop("disabled", true);
            $("#FormOneDate").prop("disabled", true);
            $("#Remarks").prop("disabled", true);
            $scope.isView = true;
            $scope.ishide = false;
            $scope.isSave = false;
        }

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
        $scope.ContainerSizeList = [
            {
                "id": '20',
                "ContainerSize": "20",


            },
            {
                "id": '40',
                "ContainerSize": "40",

            }
        ];

        $scope.onchangetext = function (i) {
            var flag1 = 0;
            angular.forEach($scope.JODetails, function (item) {
                debugger;
                if ((item.ContainerNo.toLowerCase() == i.ContainerNo.toLowerCase()) && (item.ID != i.ID)) {
                    flag1 = 1;
                }
            });

            if (flag1 == 1) {
                i.ContainerNo = '';
                alert('Can not add duplicate Container/CBT no.');
                //bootbox.alert({
                //    message: 'Duplicate row added in details section',
                //    size: 'small'
                //});
                return false;
            }
        }
        $scope.AddJobOrderByRoad = function () {
            debugger;
            t2 = t2 + 1;
            var len = $scope.JODetails.length;
            if (len > 0) {
                if ($scope.JODetails[len - 1].Container_No == '' || $scope.JODetails[len - 1].Container_No == null) {
                    return false;
                }
                $scope.JODetails.push
                ({
                    'ID': t2,
                    "Container_No": '',
                    "CT_Size": '',
                    "CustomSealNo": '',
                    "CargoDescription": '',
                    "ShippingLineId": '',
                    "ShippingLineName": '',
                    "Ct_Tare": '',
                    "Cargo_Wt": '',
                    "Gross_Wt": '',
                    "ContainerLoadType": '',
                    "Destination": '',
                    "Smtp_No": '',
                    "Received_Date": '',
                    
                     "TrainNo": '',
                     "TrainDate": '',
                     "PortId": '',
                     "Wagon_No": '',
                     "Line_Seal_No": '',
                     "S_Line": '',
                     "Genhaz": '',
                     "TrainSummaryID": '',
                     "TrainSummarySerial": '',
                     "Cont_Commodity": '',
                      "CargoType":''

                });
            }
            else {
                $scope.JODetails.push
                 ({
                     'ID': t2,
                     "Container_No": '',
                     "CT_Size": '',
                     "CustomSealNo": '',
                     "CargoDescription": '',
                     "ShippingLineId": '',
                     "ShippingLineName": '',
                     "Ct_Tare": '',
                     "Cargo_Wt": '',
                     "Gross_Wt": '',
                     "ContainerLoadType": '',
                     "Destination": '',
                     "Smtp_No": '',

                     "TrainNo": '',
                     "TrainDate": '',
                     "PortId": '',
                     "Wagon_No": '',
                     "Line_Seal_No": '',
                     "S_Line": '',
                     "Genhaz": '',
                     "TrainSummaryID": '',
                     "TrainSummarySerial": '',
                     "Cont_Commodity": '',
                      "CargoType":''

                   
                 });
            }
        }

        function GetJobOrderByRoadByOnEditMode() {
            debugger;
            JobOrderNewFormatService.GetJobOrderByRoadByOnEditMode($('#ImpJobOrderId').val()).then(function (response) {
                debugger;
                $scope.JODetails =JSON.parse(response.data.StringifyXML);
            });

        }
      
        $scope.GetJobOrderContainerNo = function (index) {
            conind = index;
            debugger;
            JobOrderNewFormatService.GetContainerNo().then(function (response) {
                debugger;
                $scope.ContainerDetails = response.data;

                if ($scope.ContainerDetails.Status == 1) {
                    var Html = '';
                  
                    $.each($scope.ContainerDetails.Data, function (i, item) {
                        debugger
                        Html += '<li id=' + item.TrainNo + ' onclick="FillContainerBox(&quot;' + item.TrainNo + '&quot;,' + item.TrainSummaryID + ')">' + item.TrainNo + '</li>';
                    });
                    $('#lstContainerNo').html(Html);
                }
            });

        }

        $scope.FillContainerBoxDetails = function (TrainNo, TrainSummaryID) {
            debugger;
            JobOrderNewFormatService.GetContainerNoDetails(TrainNo, TrainSummaryID).then(function (response) {
                debugger;
                $scope.ContainerDetails = response.data;

                if ($scope.ContainerDetails.Status == 1) {

                    // $scope.JODetails[conind].ShippingLineName = $scope.ContainerDetails.ShippingLineName;
                    $scope.JODetails[conind].Container_No = $scope.ContainerDetails.Data.Container_No;
                    $scope.JODetails[conind].CT_Size = $scope.ContainerDetails.Data.CT_Size;
                    $scope.JODetails[conind].CustomSealNo = $scope.ContainerDetails.Data.CustomSealNo;
                    $scope.JODetails[conind].CargoDescription = $scope.ContainerDetails.Data.CargoDescription;
                    $scope.JODetails[conind].ShippingLineName = $scope.ContainerDetails.Data.ShippingLineName;
                    $scope.JODetails[conind].Ct_Tare = $scope.ContainerDetails.Data.Ct_Tare;
                    $scope.JODetails[conind].Cargo_Wt = $scope.ContainerDetails.Data.Cargo_Wt;
                    $scope.JODetails[conind].Gross_Wt = $scope.ContainerDetails.Data.Gross_Wt;
                    $scope.JODetails[conind].ContainerLoadType = $scope.ContainerDetails.Data.ContainerLoadType;
                    $scope.JODetails[conind].Destination = $scope.ContainerDetails.Data.Destination;
                    $scope.JODetails[conind].Smtp_No = $scope.ContainerDetails.Data.Smtp_No;
                    $scope.JODetails[conind].Received_Date = $scope.ContainerDetails.Data.Received_Date;
                    $scope.JODetails[conind].ShippingLineId = $scope.ContainerDetails.Data.ShippingLineId;

                    $scope.JODetails[conind].TrainNo = $scope.ContainerDetails.Data.TrainNo;
                    $scope.JODetails[conind].TrainDate = $scope.ContainerDetails.Data.TrainDate;
                    $scope.JODetails[conind].PortId = $scope.ContainerDetails.Data.PortId;
                    $scope.JODetails[conind].Wagon_No = $scope.ContainerDetails.Data.Wagon_No;
                    $scope.JODetails[conind].Line_Seal_No = $scope.ContainerDetails.Data.Line_Seal_No;
                    $scope.JODetails[conind].S_Line = $scope.ContainerDetails.Data.S_Line;
                    $scope.JODetails[conind].Genhaz = $scope.ContainerDetails.Data.Genhaz;

                    $scope.JODetails[conind].TrainSummaryID = $scope.ContainerDetails.Data.TrainSummaryID;
                    $scope.JODetails[conind].TrainSummarySerial = $scope.ContainerDetails.Data.TrainSummarySerial;
                    $scope.JODetails[conind].Cont_Commodity = $scope.ContainerDetails.Data.Cont_Commodity;
                    $scope.JODetails[conind].CargoType = $scope.ContainerDetails.Data.CargoType;
                  



                  
                }
                $('#blNomodal').modal('hide');
            });
        }




        $scope.onShippingLineChange = function (index) {
            debugger;
            ind = index;
        }
        $scope.SelectShippingLine = function (ShippingLineId, ShippingLineName) {
            debugger;
            $scope.JODetails[ind].ShippingLineName = ShippingLineName;
            $scope.JODetails[ind].ShippingLineId = ShippingLineId;
           // $scope.$apply();
            LoadEximTrader();
            $("#slmodal").modal("hide");
        };

        $scope.ResetImpJODetails = function () {
            //var now = new Date();
            //var datestring = (now.getDate() > 9 ? '' : '0') + now.getDate() + '/' + (now.getMonth() > 8 ? '' : '0') + (now.getMonth() + 1) + '/' + now.getFullYear();
            //$('#FormOneDate').val(datestring);
            //$('#FormOneId').val('');
            //$('#FormOneNo').val('');
            //$('#CONTCBT').val('');
            //$('#TransportBy').val('');
            //$scope.JODetails.length = 0;
            $('#DivBody').load('/Import/Ppg_CWCImport/JobOrderNewFormat/');
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
           
            var flag1 = 0;
            var flag2 = 0;
            var flag3 = 0;
            if ($scope.JODetails.length == 0)
            {
                alert('Please Select  Atleast One Container.');
                return false;
            }
            if ($('#PickUpLocation').val() == '' || $('#PickUpLocation').val() == null) {
                alert('Please Select Pick Up Location');
                return false;
            }



            angular.forEach($scope.JODetails, function (item) {
                if (item.Container_No == '') {
                    flag1 = 1;
                }
               
            });
            if (flag1 == 1) {
                alert('Please Select Containe No.');
                return false;
            }
           
            debugger;
            if (confirm('Are you sure to save Job Order By Road?')) {
                debugger;
                Obj = {
                    ImpJobOrderId: $('#ImpJobOrderId').val() == undefined ? 0 : $('#ImpJobOrderId').val(),
                
                    JobOrderDate: $('#JobOrderDate').val(),
                    JobOrderTime: $('#JobOrderTime').val(),
                    PickUpLocation: $('#PickUpLocation').val(),
                   
                }

                JobOrderNewFormatService.OnJobOrderSave(Obj, JSON.stringify($scope.JODetails)).then(function (res) {
                    //console.log(res);
                    alert(res.data.Message);
                    if (res.data.Status < 4) {
                        debugger;
                        $('#btnSave').attr("disabled", true);
                        var returnData = res.data.Data.split('-');

                        $('#ImpJobOrderId').val(returnData[0]);
                        $('#JobOrderNo').val(returnData[1]);
                        $scope.btnflag = false;
                        $scope.btnSaveFlag = true;
                        //setTimeout(function () {
                        //    $('#DivBody').load('/Import/PpG_CWCImport/JobOrderNewFormat');
                        //}, 2000);
                    }
                });
            }
        }

     
    });
})();