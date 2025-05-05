(function () {
   // alert('by road ctrl');
    angular.module('CWCApp').
    controller('WFLDJobOrderByRoadCtrl', function ($scope, WFLDJobOrderByRoadService) {
        $scope.InvoiceNo = "";
        var t2 = 0;
        $scope.JODetails = [
            {
                'ID': t2,
                "ContainerNo": '',
                "ContainerSize": '',
                "ShippingLineId":'',
                "ShippingLineName":'',
                "LCLFCL": '',
                "ForeignLiner": '',
                "VesselName": '',
                "VesselNo": '',
                "IsODC": '',

            }];
        $scope.JODetails.length = 0;

        $(function () {
            debugger
            $scope.isView = false;
            $scope.ishide = true;
            $scope.isSave = true;
            $scope.isShow = true;
            var now = new Date();
            var datestring = (now.getDate() > 9 ? '' : '0') + now.getDate() + '/' + (now.getMonth() > 8 ? '' : '0') + (now.getMonth() + 1) + '/' + now.getFullYear();
            $('#FormOneDate').val(datestring);
            if ($('#FormOneId').val() > 0) {
                GetJobOrderByRoadByOnEditMode();
            }
            if ($('#hdnView').val() == "Edit") {
                
                $scope.ishide = false;
                $scope.isSave = true;
                $scope.isShow = false;
            }
            if ($('#hdnView').val() == "View") {
                $("#CONTCBT").prop("disabled", true);
                $("#TransportBy").prop("disabled",true);
                $("#FormOneDate").prop("disabled", true);
                $("#Remarks").prop("disabled", true);
                $scope.isView = true;
                $scope.ishide = false;
                $scope.isSave = false;
            }
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
        $scope.IsODCList = [
            {
                "id": '1',
                "IsODC": "Yes",


            },
            {
                "id": '2',
                "IsODC": "No",

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
                if ($scope.JODetails[len - 1].ContainerNo == '' || $scope.JODetails[len - 1].ContainerNo == null) {
                    return false;
                }
                $scope.JODetails.push
                ({
                    'ID': t2,
                    "ContainerNo": '',
                    "ContainerSize": '',
                    "ShippingLineId": '',
                    "ShippingLineName": '',
                    "LCLFCL": '',
                    "ForeignLiner": '',
                    "VesselName": '',
                    "VesselNo": '',
                    "IsODC": '',
                });
            }
            else {
                $scope.JODetails.push
                 ({
                    'ID': t2,
                    "ContainerNo": '',
                    "ContainerSize": '',
                    "ShippingLineId": '',
                    "ShippingLineName": '',
                    "LCLFCL": '',
                    "ForeignLiner": '',
                    "VesselName": '',
                    "VesselNo": '',
                    "IsODC": '',
                });
            }
        }

        function GetJobOrderByRoadByOnEditMode()
        {
            debugger;
            WFLDJobOrderByRoadService.GetJobOrderByRoadByOnEditMode($('#FormOneId').val()).then(function (response) {
               debugger;
               $scope.JODetails = response.data;
            });
           
        }
        
        $scope.onShippingLineChange = function (index) {
            debugger;
            ind = index;
        }
        //$scope.SelectShippingLine = function (ShippingLineId, ShippingLineName) {
        //    debugger;
        //    $scope.JODetails[ind].ShippingLineName = $(this).text().split('(')[0];
        //    $scope.JODetails[ind].ShippingLineId = $(this).attr('id');
        //    $scope.$apply();
        //    LoadEximTrader();
        //    $("#ShippingLineModal").modal("hide");
        //};

        $scope.ResetImpJODetails = function () {
            //var now = new Date();
            //var datestring = (now.getDate() > 9 ? '' : '0') + now.getDate() + '/' + (now.getMonth() > 8 ? '' : '0') + (now.getMonth() + 1) + '/' + now.getFullYear();
            //$('#FormOneDate').val(datestring);
            //$('#FormOneId').val('');
            //$('#FormOneNo').val('');
            //$('#CONTCBT').val('');
            //$('#TransportBy').val('');
            //$scope.JODetails.length = 0;
            $('#DivBody').load('/Import/DSR_CWCImport/CreateJobOrderByRoad/');
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
            var x = $('#FormOneDate').val();
            var reg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
            if (x.match(reg)) {
                //return true;
            }
            else {
                alert("Job Order Date should be dd/mm/yyyy");
                return false;
            }
            //if ($('#ShippingLineName').val() == "") {
            //    alert('Please Select Shipping Line Name.');
            //    return false;
            //}
            if ($('#CONTCBT').val() == "") {
                alert('Please Select Container/CBT');
                return false;
            }
            var flag1 = 0;
            var flag2 = 0;
            var flag3 = 0;
            angular.forEach($scope.JODetails, function (item) {
                if (item.ContainerNo=='') {
                    flag1 = 1;
                }
                else if (item.ContainerSize == '' && $('#CONTCBT').val() == "CONT") {
                    flag2 = 1;
                }
                else if (item.LCLFCL == '') {
                    flag3 = 1;
                }
            });
            if(flag1 == 1){
                alert('Please Select Containe No.');
                return false;
            }
            if (flag2 == 1) {
                alert('Please Select Container Size');
                return false;
            }
            if (flag3 == 1) {
                alert('Please Select Container Load Type');
                return false;
            }
            if ($('#Remarks').val().length > 250) {
                alert('Remarks should not more than 250 character');
                return false;
            }
            debugger;
            if (confirm('Are you sure to save Job Order By Road?')) {
                debugger;
                Obj = {
                    FormOneId: $('#FormOneId').val() == undefined ? 0 : $('#FormOneId').val(),
                    FormOneNo: $('#FormOneNo').val(),
                    FormOneDate: $('#FormOneDate').val(),
                    TransportBy: $('#TransportBy').val(),
                    ShippingLineId: $('#ShippingLineId').val() == undefined ? 0 : $('#ShippingLineId').val(),
                    CONTCBT: $('#CONTCBT').val(),
                    ForeignLiner: $('#ForeignLiner').val(),
                    VesselName: $('#VesselName').val(),
                    VesselNo: $('#VesselNo').val(),
                    Remarks: $('#Remarks').val(),
                }
                
                WFLDJobOrderByRoadService.OnJobOrderSave(Obj, JSON.stringify($scope.JODetails)).then(function (res) {
                    //console.log(res);
                    alert(res.data.Message);
                    if (res.data.Status < 4)
                    {
                        $('#btnSave').attr("disabled", true);
                        setTimeout(function () {
                            $('#DivBody').load('/Import/DSR_CWCImport/CreateJobOrderByRoad');
                        }, 2000);
                    }
                });
            }
        }
    });
})();