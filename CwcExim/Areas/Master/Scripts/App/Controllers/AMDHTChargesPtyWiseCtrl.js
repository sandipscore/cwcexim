(function () {
    angular.module('CWCApp').
    controller('AMDHTChargesPtyWiseCtrl', function ($scope, AMDHTChargesPtyWiseService, $http) {
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
                'CustomExam': ''
                //'PortFromDistance':'',
                //'PortToDistance':''

            }];
            $scope.ChargeList.length = 0;
        }
        debugger;
        $scope.sizelist = [

            {
                "id": 20,
                "name": "20"

            },
            {
                "id": 40,
                "name": "40"
            },
            
            {
                "id": 45,
                "name": "45"
            }
        ];
        //$scope.Size = $scope.sizelist[0];
        $scope.Size = '';
        //$scope.ChargesFor = '';

        $scope.EditHTChargesPtyWise = function (HTChargesId) {
            debugger;
            //var HTChargesId = $('#HTChargesId').val();
            $('#DivBody').load('/Master/AMDMaster/EditHTChargesPtyWise?HTChargesId=' + HTChargesId);
            $('#BtnHTSave').removeAttr("disabled");
            $('#BtnHTList').removeAttr("disabled");
        }

        $scope.ViewHTChargesPtyWise = function (HTChargesId) {
            debugger;
            
            $('#DivBody').load('/Master/AMDMaster/ViewHTChargesPtyWise?HTChargesId=' + HTChargesId);
            
            $('#BtnHTSave').removeAttr("disabled");
            $('#BtnHTList').removeAttr("disabled");
        }
        GetSlab();
        var HTChargesId = 0;
        function GetSlab() {

            AMDHTChargesPtyWiseService.GetSlab($scope.Size, $scope.ChargesFor).then(function (response) {
                debugger;
                console.log(response.data);
                $scope.WeightSlabList = response.data.LstWeightSlab;
                $scope.DistanceSlabList = response.data.LstDistanceSlab;

                HTChargesId = $('#HTChargesId').val();
                if (HTChargesId > 0) {
                    AMDHTChargesPtyWiseService.EditHTChargesOther(HTChargesId).then(function (res) {
                        debugger;

                        console.log(res.data);
                        $scope.Size = res.data.Size == "0" ? "" : res.data.Size;
                        //$scope.Size.id = res.data.Size == "0" ? "" : res.data.Size;
                        //$scope.Size.name = res.data.Size == "0" ? "--Select--" : res.data.Size;
                        $scope.ChargesFor = res.data.ChargesFor;
                        $scope.OnChargeChange(res.data.ChargesFor);

                        if (res.data.SlabType == 1) {
                            $scope.WithoutSlab = false;
                            $scope.WithSlab = true;
                            $scope.isSlab = true;
                            $scope.WeightSlab = res.data.WeightSlab == 1 ? true : false,
                            $scope.DistanceSlab = res.data.DistanceSlab == 1 ? true : false
                            $scope.IsODC = res.data.IsODC == 1 ? true : false
                            $('#chargeslab').show();
                        }
                        else {

                            $scope.Slab = 'WithoutSlab';
                            $scope.WithSlab = false;
                            $scope.WithoutSlab = true;
                            $scope.IsODC = res.data.IsODC == 1 ? true : false;
                            $('#chargeslab').prop('checked', false);
                            $('#weight').prop('checked', false);
                            $('#distence').prop('checked', false);
                            $('#chargeslab').hide();
                        }
                    });
                    AMDHTChargesPtyWiseService.GetSlabChargeDtl(HTChargesId).then(function (response) {
                        debugger;
                        $scope.ChargeList = response.data;
                        
                    });
                }
            });
        }
                        
        $scope.WithSlab = false;
        $scope.WithoutSlab = true;
        $scope.Slab = 'WithoutSlab';
        $scope.SlabType = 1;
        
        $scope.TransPortModeFunc = function () {
            debugger;
            if ($scope.Slab == 'WithSlab') {
                $scope.SlabType = 1;
                $scope.isSlab = false;
                $scope.ChargeList.length = 0;
                $('#chargeslab').show();
            }
            else {
                $scope.SlabType = 2;
                $scope.ChargeList.length = 0;
                $scope.isSlab = false;
                $('#chargeslab').prop('checked', false);
                $('#weight').prop('checked', false);
                $('#distence').prop('checked', false);
                $('#chargeslab').hide();

            }
            $scope.ResetDtl();
        }
        
        if ($('#HTChargesId').val() == 0 || $('#HTChargesId').val() == undefined) {
            if ($scope.ChargesFor == "" || $scope.ChargesFor == undefined) {
                debugger;
                $scope.Slab = 'WithoutSlab';
                $scope.WithSlab = false;
                $scope.WithoutSlab = true;
                $scope.SlabType = 2;
                $scope.ChargeList.length = 0;
                $scope.isSlab = false;
                $('#chargeslab').hide();
            }
        }
        $scope.OnChargeChange = function (ChargesFor) {
            debugger;
            if ($scope.ChargesFor == "" || $scope.ChargesFor == undefined) {
                debugger;
                $scope.Slab = 'WithoutSlab';
                $scope.WithSlab = false;
                $scope.WithoutSlab = true;
                $scope.SlabType = 2;
                
                $scope.isSlab = false;
                $scope.Size = ''; 
                
                $('#chargeslab').hide();
            }
            $scope.ResetDtl();
        }
        $scope.OnSizeChange = function () {
            
            AMDHTChargesPtyWiseService.GetSlab($scope.Size, $scope.ChargesFor).then(function (response) {
                debugger;
                $scope.WeightSlabList = response.data.LstWeightSlab;
                $scope.DistanceSlabList = response.data.LstDistanceSlab;
            });
        }
        AMDHTChargesPtyWiseService.GetAllHTCharges().then(function (response) {
            debugger;
            
            if (response.data.LstviewList.length > 0) {
                $scope.lists = response.data.LstviewList;
                
                var ListData = JSON.stringify($scope.lists);
                if (ListData != '' && ListData != undefined && ListData != []) {
                    $('#TblHTCharges tbody').html('');
                    var TblHTCharges = $('#TblHTCharges').dataTable();
                    $.each(JSON.parse(ListData), function (i, elem) {
                        TblHTCharges.fnAddData([
                        i + 1,
                        [elem.EffectiveDate],
                        [elem.OperationCode],
                        [elem.OperationDesc],
                        [elem.ChargesFor],
                        ['<a href="#" onclick=ViewHTChargesPtyWise(' + [elem.HTChargesId] + ')><i class="fa fa-search-plus Edit"></i></a>'],
                        ['<a href="#" onclick=EditHTChargesPtyWise(' + [elem.HTChargesId] + ')><i class="fa fa-pencil-square Edit"></i></a>']
                        ]);
                    });
                }                
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
       
        $scope.AddHTCharges = function () {
            debugger;
            t2 = t2 + 1;
            var flag = 0;
            if ($scope.Slab == 'WithSlab') {
                var weight = 0;
                for (i = 0; i < $scope.WeightSlabList.length; i++) {
                    if ($scope.WeightSlabList[i].chkWeightSlab == true && $scope.WeightSlab == true) {
                        weight = 1;
                    }
                }
                if (weight == 0 && $scope.WeightSlab == true) {
                    alert('Please Select Atleast One Check Box From Weight Slab');
                    return false
                }
                var Dis = 0;
                for (i = 0; i < $scope.DistanceSlabList.length; i++) {
                    if ($scope.DistanceSlabList[i].chkDistanceSlab == true && $scope.DistanceSlab == true) {
                        Dis = 1;
                    }
                }
                if (Dis == 0 && $scope.DistanceSlab == true) {
                    alert('Please Select Atleast One Check Box From Distance Slab');
                    return false
                }

                debugger;
                //**************************************************************
                if ($scope.WeightSlab == true) {
                    var WeightSlabId = $.grep($scope.WeightSlabList, function (item) {
                        return item.chkWeightSlab == true;
                    })[0].WeightSlabId;
                    var FromWeightSlab = $.grep($scope.WeightSlabList, function (item) {
                        return item.chkWeightSlab == true;
                    })[0].FromWeightSlab;
                    var ToWeightSlab = $.grep($scope.WeightSlabList, function (item) {
                        return item.chkWeightSlab == true;
                    })[0].ToWeightSlab;

                    var chkWeightSlab = $.grep($scope.WeightSlabList, function (item) {
                        return item.chkWeightSlab == true;
                    })[0].chkWeightSlab;
                }
                if ($scope.DistanceSlab == true) {
                    var DistanceSlabId = $.grep($scope.DistanceSlabList, function (item) {
                        return item.chkDistanceSlab == true;
                    })[0].DistanceSlabId;
                    var FromDistanceSlab = $.grep($scope.DistanceSlabList, function (item) {
                        return item.chkDistanceSlab == true;
                    })[0].FromDistanceSlab;
                    var ToDistanceSlab = $.grep($scope.DistanceSlabList, function (item) {
                        return item.chkDistanceSlab == true;
                    })[0].ToDistanceSlab;
                    var chkDistanceSlab = $.grep($scope.DistanceSlabList, function (item) {
                        return item.chkDistanceSlab == true;
                    })[0].chkDistanceSlab;
                }

                if ($scope.ChargeList.length > 0) {
                    angular.forEach($scope.ChargeList, function (item) {
                        if ((item.WtSlabId + ':' + item.DisSlabId == WeightSlabId + ':' + DistanceSlabId) && ($('#PortId').val() == item.PortId) && ($scope.WeightSlab == true && $scope.DistanceSlab == true)) {
                            flag = 1;
                        }
                        else if ((item.WtSlabId == WeightSlabId) && ($scope.WeightSlab == true && ($scope.DistanceSlab == false || $scope.DistanceSlab == undefined)) && ($('#PortId').val() == item.PortId)) {
                            flag = 1;
                        }
                        else if ((item.DisSlabId == DistanceSlabId) && (($scope.WeightSlab == false || $scope.WeightSlab == undefined) && $scope.DistanceSlab == true) && ($('#PortId').val() == item.PortId)) {
                            flag = 1;
                        }
                    });
                    if (flag == 1) {
                        alert('You Have Already Added This Combination');
                        return false;
                    }
                }

                if ($scope.WeightSlab == true && $scope.DistanceSlab == true) {
                    $scope.ChargeList.push({
                        'ID': t2,
                        'WtSlabId': WeightSlabId,
                        'FromWtSlabCharge': FromWeightSlab,
                        'ToWtSlabCharge': ToWeightSlab,
                        'DisSlabId': DistanceSlabId,
                        'FromDisSlabCharge': FromDistanceSlab,
                        'ToDisSlabCharge': ToDistanceSlab,
                        'CwcRate': $('#RateCWC').val(),
                        'ContractorRate': $('#ContractorRate').val(),
                        'RoundTripRate': $('#RoundTripRate').val(),
                        'EmptyRate': $('#EmptyRate').val(),
                        'SlabType': $('#SlabType').val(),
                        'WeightSlab': $scope.WeightSlab == true ? 1 : 0,
                        'DistanceSlab': $scope.DistanceSlab == true ? 1 : 0,
                        'PortId': ($('#PortId').val() == null || $('#PortId').val() == "") ? 0 : $('#PortId').val(),
                        'PortName': $('#PortName').val(),
                        'AddlWtCharges': $('#AddlWtCharges').val(),
                        'AddlDisCharges': $('#AddlDisCharges').val(),
                        'CustomExam': $('#CustomExam').val()
                        
                    });
                }
                else if ($scope.WeightSlab == true && ($scope.DistanceSlab == false || $scope.DistanceSlab == undefined)) {
                    $scope.ChargeList.push({
                        'ID': t2,
                        'WtSlabId': WeightSlabId,
                        'FromWtSlabCharge': FromWeightSlab,
                        'ToWtSlabCharge': ToWeightSlab,
                        'DisSlabId': '0',
                        'FromDisSlabCharge': '0',
                        'ToDisSlabCharge': '0',
                        'CwcRate': $('#RateCWC').val(),
                        'ContractorRate': $('#ContractorRate').val(),
                        'RoundTripRate': $('#RoundTripRate').val(),
                        'EmptyRate': $('#EmptyRate').val(),
                        'SlabType': $('#SlabType').val(),
                        'WeightSlab': $scope.WeightSlab == true ? 1 : 0,
                        'DistanceSlab': $scope.DistanceSlab == true ? 1 : 0,
                        'PortId': ($('#PortId').val() == null || $('#PortId').val() == "") ? 0 : $('#PortId').val(),
                        'PortName': $('#PortName').val(),
                        'AddlWtCharges': $('#AddlWtCharges').val(),
                        'AddlDisCharges': $('#AddlDisCharges').val(),
                        'CustomExam': $('#CustomExam').val()
                    });
                }
                else if (($scope.WeightSlab == false || $scope.WeightSlab == undefined) && $scope.DistanceSlab == true) {
                    $scope.ChargeList.push({
                        'ID': t2,
                        'WtSlabId': '0',
                        'FromWtSlabCharge': '0',
                        'ToWtSlabCharge': '0',
                        'DisSlabId': DistanceSlabId,
                        'FromDisSlabCharge': FromDistanceSlab,
                        'ToDisSlabCharge': ToDistanceSlab,
                        'CwcRate': $('#RateCWC').val(),
                        'ContractorRate': $('#ContractorRate').val(),
                        'RoundTripRate': $('#RoundTripRate').val(),
                        'EmptyRate': $('#EmptyRate').val(),
                        'SlabType': $('#SlabType').val(),
                        'WeightSlab': $scope.WeightSlab == true ? 1 : 0,
                        'DistanceSlab': $scope.DistanceSlab == true ? 1 : 0,
                        'PortId': ($('#PortId').val() == null || $('#PortId').val() == "") ? 0 : $('#PortId').val(),
                        'PortName': $('#PortName').val(),
                        'AddlWtCharges': $('#AddlWtCharges').val(),
                        'AddlDisCharges': $('#AddlDisCharges').val(),
                        'CustomExam': $('#CustomExam').val()
                    });
                }
            }
            else {
                $scope.ChargeList.length = 0;
                $scope.ChargeList.push({
                    'ID': p + 1,
                    'WtSlabId': '0',
                    'FromWtSlabCharge': '0',
                    'ToWtSlabCharge': '0',
                    'DisSlabId': '0',
                    'FromDisSlabCharge': '0',
                    'ToDisSlabCharge': '0',
                    'CwcRate': $('#RateCWC').val(),
                    'ContractorRate': $('#ContractorRate').val(),
                    'RoundTripRate': $('#RoundTripRate').val(),
                    'EmptyRate': $('#EmptyRate').val(),
                    'SlabType': 2,
                    'WeightSlab': 0,
                    'DistanceSlab': 0,
                    'PortId': ($('#PortId').val() == null || $('#PortId').val() == "") ? 0 : $('#PortId').val(),
                    'PortName': $('#PortName').val(),
                    'AddlWtCharges': $('#AddlWtCharges').val(),
                    'AddlDisCharges': $('#AddlDisCharges').val(),
                    'CustomExam': $('#CustomExam').val()
                });

            }
            $('#EmptyRate, #RoundTripRate, #ContractorRate, #RateCWC').val(0);
            $('#PortName').val('');
            $('#PortId').val(0);
            $scope.isSlab = true;
            
            AMDHTChargesPtyWiseService.GetSlab($scope.Size, $scope.ChargesFor).then(function (response) {
                debugger;
                
                $scope.WeightSlabList = response.data.LstWeightSlab;
                $scope.DistanceSlabList = response.data.LstDistanceSlab;
            });
        }

        $scope.Reset = function () {
            $('#DivBody').load('/Master/AMDMaster/CreateHTChargesPtyWise');
        }
        $scope.ResetDtl = function () {
            
            AMDHTChargesPtyWiseService.GetSlab($scope.Size, $scope.ChargesFor).then(function (response) {
                debugger;
                $scope.WeightSlabList = response.data.LstWeightSlab;
                $scope.DistanceSlabList = response.data.LstDistanceSlab;
            });
            $('#EmptyRate, #RoundTripRate, #ContractorRate, #RateCWC').val(0);
            $('#PortName').val('');
            $('#PortId').val(0);
            $scope.WeightSlab = false;
            $scope.DistanceSlab = false;
            $scope.isSlab = false;
            
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
            
            if (($('#CommodityType').val() == '' || $('#CommodityType').val() == null) && $scope.ChargesFor == 'CONT') {
                alert('Please Select CommodityType');
                return false;
            }
            if ($('#OperationCode').val() == '' || $('#OperationCode').val() == null) {
                alert('Please Select OperationCode');
                return false;
            }
            if ($('#PartyName').val() == '' || $('#PartyName').val() == null) {
                alert('Please Select Party Name');
                return false;
            }

            if ($scope.ChargeList.length != 0) {
                Obj = {
                    HTChargesId: $('#HTChargesId').val() == undefined ? 0 : $('#HTChargesId').val(),
                    EffectiveDate: $('#EffectiveDate').val(),
                    OperationType: $('#OperationType').val(),
                    OperationCode: $('#OperationCode').val(),
                    OperationId: $('#OperationId').val(),
                    ContainerType: $('#ContainerType').val(),
                    Type: $('#Type').val(),
                    Size: $scope.Size,
                    ChargesFor: $scope.ChargesFor,
                    MaxDistance: $('#MaxDistance').val(),
                    CommodityType: $('#CommodityType').val(),
                    ContainerLoadType: $('#ContainerLoadType').val(),
                    TransportFrom: $('#TransportFrom').val(),
                    EximType: $('#EximType').val(),
                    SlabType: $scope.SlabType,
                    WeightSlab: $scope.WeightSlab == true ? 1 : 0,
                    DistanceSlab: $scope.DistanceSlab == true ? 1 : 0,
                    IsODC: $scope.IsODC == true ? 1 : 0,
                    PartyId: $('#PartyId').val(),
                    PartyName: $('#PartyName').val()
                }

                AMDHTChargesPtyWiseService.SaveHTCharges(Obj, $scope.ChargeList).then(function (res) {
                    debugger;
                    if (res.data.Status == 1 || res.data.Status == 2) {
                        if ($('#DivMsg').hasClass('logErrMsg'))
                            $('#DivMsg').removeClass('logErrMsg').addClass('logSuccMsg');
                        $('#DivMsg').html(res.data.Message);
                        
                    }
                    else {
                        if ($('#DivMsg').hasClass('logSuccMsg'))
                            $('#DivMsg').removeClass('logSuccMsg').addClass('logErrMsg');
                        $('#DivMsg').html(res.data.Message);
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