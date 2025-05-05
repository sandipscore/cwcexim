(function () {
    angular.module('CWCApp').
    controller('VLDAHTChargesCtrl', function ($scope, VLDAHTChargesService, $http) {
        var t2 = 0;

        if ($('#HTChargesId').val() == 0 || $('#HTChargesId').val() == undefined) {
            $scope.ChargeList = [
            {
                'ID': t2,
                'WtSlabId': '',
                'FromWtSlabCharge': '',
                'ToWtSlabCharge': '',
                'DisSlabId': '',
                'FromDisSlabCharge': '',
                'ToDisSlabCharge': '',
                'FromCbmSlabCharge': '',
                'ToCbmSlabCharge': '',
                'CwcRate': '',
                'ContractorRate': '',
                'RoundTripRate': '',
                'EmptyRate': '',
                'SlabType': '',
                'WeightSlab': '',
                'DistanceSlab': '',
                'PortId': '',
                'PortName': '',
                'AddlWtCharges': '',
                'AddlDisCharges': '',
                'AddlCbmCharges': ''
                //'PortFromDistance':'',
                //'PortToDistance':''

            }];
            $scope.ChargeList.length = 0;
        }
        debugger;
     

        $scope.EditHTCharges = function (HTChargesId) {
            debugger;
            //var HTChargesId = $('#HTChargesId').val();
            $('#DivBody').load('/Master/VLDAMaster/EditHTChargesForEximTraders?HTChargesId=' + HTChargesId);
            $('#BtnHTSave').removeAttr("disabled");
            $('#BtnHTList').removeAttr("disabled");
        }

        $scope.ViewHTCharges = function (HTChargesId) {
            debugger;
            //var HTChargesId = $('#HTChargesId').val();
            $('#DivBody').load('/Master/VLDAMaster/ViewHTChargesForEximTraders?HTChargesId=' + HTChargesId);
            // $scope.OnChargeChange($scope.ChargesFor);
            $('#BtnHTSave').removeAttr("disabled");
            $('#BtnHTList').removeAttr("disabled");
        }
        var HTChargesId = 0;
        $scope.WithSlab = false;
        $scope.WithoutSlab = true;
        $scope.Slab = 'WithoutSlab';
        $scope.SlabType = 2;
        $('#chargeslab').hide();
        $('#chargeslabFormTo').hide();

        GetSlab();

        function GetSlab() {

            //WFLDHTChargesService.GetSlab($scope.Size, $scope.ChargesFor).then(function (response) {
            debugger;
            //console.log(response.data);
            //$scope.WeightSlabList = response.data.LstWeightSlab;
            //$scope.DistanceSlabList = response.data.LstDistanceSlab;

            HTChargesId = $('#HTChargesId').val();
            if (HTChargesId > 0) {
             
                            
                VLDAHTChargesService.GetSlabChargeDtl(HTChargesId).then(function (response) {
                    debugger;
                    console.log(response.data);
                    $scope.ChargeList = response.data;
                    console.log(response.data[0].SlabType);
                    // alert(ChargeList.SlabType.val + '2');
                    console.log($scope.SlabType);
                    if (response.data[0].SlabType == 1) {
                        $scope.WithoutSlab = false;
                        $scope.WithSlab = true;
                        $scope.SlabType = 1;
                        $scope.Slab = 'WithSlab';
                        $('#chargeslab').show();
                        $('#chargeslabFormTo').show();
                    }
                    else {
                        $scope.WithSlab = false;
                        $scope.WithoutSlab = true;
                        $scope.SlabType = 2;
                        $scope.Slab = 'WithoutSlab';
                        $('#chargeslab').hide();
                        $('#chargeslabFormTo').hide();

                    }
                });
            }
            //});
        }


        //$scope.chargeslabFormTo = false;
        //}
        $scope.TransPortModeFunc = function () {
            debugger;
            if ($scope.Slab == 'WithSlab') {
                $scope.SlabType = 1;
                $scope.isSlab = false;
                $scope.ChargeList.length = 0;
                $('#chargeslab').show();
                $('#chargeslabFormTo').show();
                //alert($scope.SlabType);
            }
            else {
                $scope.SlabType = 2;
                $scope.ChargeList.length = 0;
                $scope.isSlab = false;
                $('#chargeslab').prop('checked', false);
                $('#weight').prop('checked', false);
                $('#distence').prop('checked', false);
                $('#cbm').prop('checked', false);
                $('#chargeslab').hide();
                $('#chargeslabFormTo').hide();

            }
            $scope.ResetDtl();
        }
        //$scope.isdis = true;
        //if ($('#HTChargesId').val() == 0 || $('#HTChargesId').val() == undefined) {
        //    if ($scope.ChargesFor == "" || $scope.ChargesFor == undefined) {
        //        debugger;
        //        //$scope.Slab = 'WithoutSlab';
        //        $scope.WithSlab = false;
        //        $scope.WithoutSlab = true;
        //        $scope.SlabType = 2;
        //        $scope.ChargeList.length = 0;
        //        $scope.isSlab = false;
        //        $('#chargeslab').hide();
        //        $('#chargeslabFormTo').hide();
        //    }
        //}
        $scope.OnChargeChange = function (ChargesFor) {
            debugger;
            if ($scope.ChargesFor == "" || $scope.ChargesFor == undefined) {
                debugger;
                //$scope.Slab = 'WithoutSlab';
                $scope.WithSlab = false;
                $scope.WithoutSlab = true;
                $scope.SlabType = 2;
                //$scope.ChargeList.length = 0;
                $scope.isSlab = false;
                $scope.Size = ''; //$scope.sizelist[0];
                $('#Size').val('');
                //$scope.Size.id = $scope.sizelist[0].id;
                //$scope.Size.name = $scope.sizelist[0].name;
                $('#chargeslab').hide();
                $('#chargeslabFormTo').hide();
            }
            $scope.ResetDtl();
        }
        $scope.OnSizeChange = function () {

            //WFLDHTChargesService.GetSlab($scope.Size, $scope.ChargesFor).then(function (response) {
            //debugger;
            //$scope.WeightSlabList = response.data.LstWeightSlab;
            //$scope.DistanceSlabList = response.data.LstDistanceSlab;
            //});
        }
        VLDAHTChargesService.GetAllHTCharges().then(function (response) {
            debugger;
            // console.log(response.data);
            if (response.data.LstviewList.length > 0) {
                $scope.lists = response.data.LstviewList;
                //console.log($scope.lists);

                /**/
                var ListData = JSON.stringify($scope.lists);
                if (ListData != '' && ListData != undefined && ListData != []) {
                    var TblHTCharges = $('#TblHTCharges').dataTable();
                    $.each(JSON.parse(ListData), function (i, elem) {
                        TblHTCharges.fnAddData([
                        i + 1,
                        [elem.EffectiveDate],
                        [elem.OperationCode],
                        [elem.OperationDesc],
                        [elem.ChargesFor],
                        ['<a href="#" onclick=ViewHTCharges(' + [elem.HTChargesId] + ')><i class="fa fa-search-plus Edit"></i></a>'],
                        ['<a href="#" onclick=EditHTCharges(' + [elem.HTChargesId] + ')><i class="fa fa-pencil-square Edit"></i></a>']
                        ]);
                    });
                }
                /**/

                //$('#TblHTCharges tr:last').remove();
            }
            else {
                $scope.lists = response.data.LstviewList;
            }
        });

        $scope.DeleteHTCharges = function (val) {
            debugger;
            var len = $scope.ChargeList.length;
            $scope.ChargeList.splice(val, 1);
        }
        $scope.WeightSlabSelection = function (position, entities) {
            debugger;
            angular.forEach(entities, function (i, index) {
                debugger;
                if (position != index)
                    i.chkWeightSlab = false;
            });
        }

        $scope.DistanceSlabSelection = function (position, entities) {
            debugger;
            angular.forEach(entities, function (i, index) {
                debugger;
                if (position != index)
                    i.chkDistanceSlab = false;
            });
        }
        var p = 0;
        //$scope.WeightSlab = true;
        $scope.AddHTCharges = function () {
            debugger;
            t2 = t2 + 1;
            var flag = 0;
            if ($scope.Slab == 'WithSlab') {
                

                debugger;
              
                              
                if ($scope.WeightSlab == true || $scope.DistanceSlab == true || $scope.CBMSlab == true) {
                    //alert($scope.SlabType);
                    $scope.ChargeList.push({

                        'ID': t2,
                        'WtSlabId': '0',//WeightSlabId,
                        'FromWtSlabCharge': $('#FromWtSlabCharge').val(),//FromWeightSlab,
                        'ToWtSlabCharge': $('#ToWtSlabCharge').val(),//ToWeightSlab,
                        'DisSlabId': '0',
                        'FromDisSlabCharge': $('#FromDisSlabCharge').val(),//'0',
                        'ToDisSlabCharge': $('#ToDisSlabCharge').val(),//'0',
                        'FromCbmSlabCharge': $('#FromCbmSlabCharge').val(),
                        'ToCbmSlabCharge': $('#ToCbmSlabCharge').val(),
                        'CwcRate': $('#RateCWC').val(),
                        'ContractorRate': $('#ContractorRate').val(),
                        'RoundTripRate': $('#RoundTripRate').val(),
                        'EmptyRate': $('#EmptyRate').val(),
                        'SlabType': $scope.SlabType,//$('#SlabType').val(),
                        'WeightSlab': $scope.WeightSlab == true ? 1 : 0,
                        'DistanceSlab': $scope.DistanceSlab == true ? 1 : 0,
                        'CbmSlab': $scope.DistanceSlab == true ? 1 : 0,
                        'PortId': ($('#PortId').val() == null || $('#PortId').val() == "") ? 0 : $('#PortId').val(),
                        'PortName': $('#PortName').val(),
                        'AddlWtCharges': $('#AddlWtCharges').val(),
                        'AddlDisCharges': $('#AddlDisCharges').val(),//,
                        'AddlCbmCharges': $('#AddlCbmCharges').val()
                        //'PortFromDistance': ($('#PortId').val() == null || $('#PortId').val() == "") ? 0 : 1,
                        //'PortToDistance': $('#PortToDistance').val()
                    });
                }
                else {
                    alert('You HaveTo Select At Least One Checkbox');
                    return false;
                }
             
            }
            else {
                $scope.ChargeList.length = 0;
                $scope.ChargeList.push({
                    'ID': p + 1,
                    'WtSlabId': '0',
                    //'FromWtSlabCharge': '0',
                    //'ToWtSlabCharge': '0',
                    'FromWtSlabCharge': $('#FromWtSlabCharge').val(),//FromWeightSlab,
                    'ToWtSlabCharge': $('#ToWtSlabCharge').val(),//ToWeightSlab,
                    'DisSlabId': '0',
                    'FromDisSlabCharge': $('#FromDisSlabCharge').val(),//'0',
                    'ToDisSlabCharge': $('#ToDisSlabCharge').val(),//'0',
                    //'FromDisSlabCharge': '0',
                    //'ToDisSlabCharge': '0',
                    'FromCbmSlabCharge': $('#FromCbmSlabCharge').val(),
                    'ToCbmSlabCharge': $('#ToCbmSlabCharge').val(),
                    'CwcRate': $('#RateCWC').val(),
                    'ContractorRate': $('#ContractorRate').val(),
                    'RoundTripRate': $('#RoundTripRate').val(),
                    'EmptyRate': $('#EmptyRate').val(),
                    'SlabType': $scope.SlabType,//2,
                    'WeightSlab': 0,
                    'DistanceSlab': 0,
                    'CbmSlab': 0,
                    'PortId': ($('#PortId').val() == null || $('#PortId').val() == "") ? 0 : $('#PortId').val(),
                    'PortName': $('#PortName').val(),
                    'AddlWtCharges': $('#AddlWtCharges').val(),
                    'AddlDisCharges': $('#AddlDisCharges').val(),
                    'AddlCbmCharges': $('#AddlCbmCharges').val()
                });

            }
            $('#EmptyRate, #RoundTripRate, #ContractorRate, #RateCWC').val(0);
            $('#PortName').val('');
            $('#PortId').val(0);
            //$scope.isSlab = true;
            //$scope.WeightSlab = false;
            //$scope.DistanceSlab = false;
            //$scope.CBMSlab = false;
            $('#weight').prop('checked', false);
            $('#distence').prop('checked', false);
            $('#cbm').prop('checked', false);

            $('#FromWtSlabCharge').val('0');
            $('#ToWtSlabCharge').val('0');
            $('#FromDisSlabCharge').val('0');
            $('#ToDisSlabCharge').val('0');
            $('#FromCbmSlabCharge').val('0');
            $('#ToCbmSlabCharge').val('0');
            $('#AddlWtCharges').val('0');
            $('#AddlDisCharges').val('0');
            $('#AddlCbmCharges').val('0');
            //WFLDHTChargesService.GetSlab($scope.Size, $scope.ChargesFor).then(function (response) {
            //    debugger;
            //    //console.log(response.data);
            //    $scope.WeightSlabList = response.data.LstWeightSlab;
            //    $scope.DistanceSlabList = response.data.LstDistanceSlab;
            //});
        }

        $scope.Reset = function () {
            $('#DivBody').load('/Master/VLDAMaster/CreateHTChargesForEximTraders');
        }
        $scope.ResetDtl = function () {
            //$('#DivBody').load('/Master/WFLDMaster/CreateHTCharges');
            //WFLDHTChargesService.GetSlab($scope.Size, $scope.ChargesFor).then(function (response) {
            //debugger;
            //$scope.WeightSlabList = response.data.LstWeightSlab;
            // $scope.DistanceSlabList = response.data.LstDistanceSlab;
            //});
            $('#EmptyRate, #RoundTripRate, #ContractorRate, #RateCWC').val(0);
            $('#PortName').val('');
            $('#PortId').val(0);
            //$scope.WeightSlab = false;
            //$scope.DistanceSlab = false;
            //$scope.CBMSlab = false;
            //$scope.isSlab = false;
            $('#weight').prop('checked', false);
            $('#distence').prop('checked', false);
            $('#cbm').prop('checked', false);

            $('#FromWtSlabCharge').val('0');
            $('#ToWtSlabCharge').val('0');
            $('#FromDisSlabCharge').val('0');
            $('#ToDisSlabCharge').val('0');
            $('#FromCbmSlabCharge').val('0');
            $('#ToCbmSlabCharge').val('0');
            $('#AddlWtCharges').val('0');
            $('#AddlDisCharges').val('0');
            $('#AddlCbmCharges').val('0');
            if (HTChargesId == 0) {
                $scope.ChargeList.length = 0;
            }
        }
        var Obj = {};
        $scope.SaveHTCharges = function () {
            debugger;
            if ($('#EffectiveDate').val() == '') {
                alert('Please Select EffectiveDate');
                return false;
            }
            if ($('#OperationId').val() == '0' || $('#OperationId').val() == null) {
                alert('Please Select OperationType');
                return false;
            }
            //if (($scope.Size == '' || $scope.Size == null) && $scope.ChargesFor == 'CONT') {
            //    alert('Please Select Size');
            //    return false;
            //}
            if (($('#CommodityType').val() == '' || $('#CommodityType').val() == null) && $scope.ChargesFor == 'CONT') {
                alert('Please Select CommodityType');
                return false;
            }
            if ($('#OperationCode').val() == '' || $('#OperationCode').val() == null) {
                alert('Please Select OperationCode');
                return false;
            }

            if ($scope.ChargeList.length != 0) {
                debugger;
                Obj = {
                    HTChargesId: $('#HTChargesId').val() == undefined ? 0 : $('#HTChargesId').val(),
                    EffectiveDate: $('#EffectiveDate').val(),
                    OperationType: $('#OperationType').val(),
                    OperationCode: $('#OperationCode').val(),
                    OperationId: $('#OperationId').val(),
                    ContainerType: $('#ContainerType').val(),
                    Type: $('#Type').val(),
                    //Size: $scope.Size,
                    Size: $('#Size').val(),
                    ChargesFor: $('#ChargesFor').val(),
                    //ChargesFor: $scope.ChargesFor,
                    MaxDistance: $('#MaxDistance').val(),
                    CommodityType: $('#CommodityType').val(),
                    ContainerLoadType: $('#ContainerLoadType').val(),
                    TransportFrom: $('#TransportFrom').val(),
                    EximType: $('#EximType').val(),
                    SlabType: $('#SlabType').val(),
                    // SlabType: $scope.SlabType,
                    WeightSlab: $scope.WeightSlab == true ? 1 : 0,
                    DistanceSlab: $scope.DistanceSlab == true ? 1 : 0,
                    CbmSlab: $scope.CbmSlab == true ? 1 : 0,
                    IsODC: $('#IsODC').val(),
                    // $scope.IsODC //== true ? 1 : 0
                    PartyID: $('#PartyID').val(),
                    PartyName: $('#PartyName').val()
                }

                VLDAHTChargesService.SaveHTCharges(Obj, $scope.ChargeList).then(function (res) {
                    debugger;
                    if (res.data.Status == 1 || res.data.Status == 2) {
                        if ($('#DivMsg').hasClass('logErrMsg'))
                            $('#DivMsg').removeClass('logErrMsg').addClass('logSuccMsg');
                        $('#DivMsg').html(res.data.Message);
                       // $('#DivBody').load('/Master/VLDAMaster/CreateHTChargesForEximTraders');
                        //Reset();
                        //LoadHTListData();
                    }
                    else {
                        if ($('#DivMsg').hasClass('logSuccMsg'))
                            $('#DivMsg').removeClass('logSuccMsg').addClass('logErrMsg');
                        $('#DivMsg').html(res.data.Message);
                        //$('#DivBody').load('/Master/VLDAMaster/CreateHTChargesForEximTraders');
                    }
                    setTimeout($scope.Reset(), 5000);
                });

            }
            else {
                alert("Charge List Should Not Be Empty");
            }
        }

    });
})();