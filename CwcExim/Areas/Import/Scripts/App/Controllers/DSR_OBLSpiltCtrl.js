(function () {
    angular.module('CWCApp').
    controller('OBLSpiltCtrl', function ($scope, OBLSpiltService) {
        var allDataStore = "";
        $scope.SpiltOBLDetails = [];
        $scope.SpiltContainerDetails = [];
        $scope.AllUnSelectContainer = [];
        $scope.isFCL = 0;
        $scope.CountTotalcon = 0;
        $scope.SpiltOBLNo = '';
        $scope.SpiltOBLDate = '';
        $scope.NoOfPkg = '';
        $scope.GRWT = '';

        $scope.Value = '';
        $scope.Duty = '';

        $scope.oblDetailsJsonData = [];
        $scope.lstContainerDetails = [];
       
        $scope.flagContriner = 0;
        var TimeInSeconds = 5000;
        $scope.GetOBLList = function () {

            OBLSpiltService.GetOBLList().then(function (response) {
                debugger;
                if (response.data.Status == 1) {
                    $scope.oblJsonData = response.data.Data;

                }
                else {
                    alert(response.data.Message);
                }
            });
        }

        $scope.PopupActivate = function () {
            debugger;
            var CharCode;
            if (window.event) {
                CharCode = window.event.keyCode;
            }
            else {
                CharCode = evt.which;
            }

            if (CharCode == 32) {
                $scope.GetOBLList();
                $('#OBLNoModal').modal('show');

            }
        }

        $scope.GetOBLSpiltDetails = function (OBL, Date, IsFCL) {

            $scope.isFCL = IsFCL;
            OBLSpiltService.GetOBLDetails(OBL, Date, IsFCL).then(function (response) {
                debugger;
                if (response.data.Status == 1) {
                    $scope.oblDetailsJsonData = response.data.Data;
                    allDataStore = $scope.oblDetailsJsonData.lstSpiltContainerDetails;
                    $scope.lstContainerDetails = response.data.Data.lstSpiltContainerDetails;                  
                    $scope.CountTotalcon = response.data.Data.lstSpiltContainerDetails.length;
                    $scope.OBLNo = OBL;
                    $('#OBLNoModal').modal('hide');

                }
                else {
                    alert(response.data.Message);
                }
            });

            $('#SpiltOBLNo').focus();


        }


        $scope.Validation = function () {
            debugger;
            var flag = true;

            if ($scope.SpiltOBLNo == '' || $scope.SpiltOBLNo == null) {
                $('span[data-valmsg-for="SpiltOBLNo"]').text('Fill out this field');
                flag = false;
            }
            else {
                $('span[data-valmsg-for="SpiltOBLNo"]').text('');

            }

            if ($scope.SpiltOBLDate == '' || $scope.SpiltOBLDate == null) {
                $('span[data-valmsg-for="SpiltOBLDate"]').text('Fill out this field');
                flag = false;
            }
            else {
                $('span[data-valmsg-for="SpiltOBLNo"]').text('');

            }

           
            if ($scope.NoOfPkg == '' || $scope.NoOfPkg == 0) {

                $('span[data-valmsg-for="NoOfPkg"]').text('Fill out this field');
                flag = false;
            }
            else {
                if (Number($scope.NoOfPkg) > 100000000) {
                    $('span[data-valmsg-for="NoOfPkg"]').text('No Of Pkg should be less than 100000000');
                    flag = false;
                }
                else {
                    $('span[data-valmsg-for="NoOfPkg"]').text('');

                }

            }
            if ($scope.GRWT == '' || $scope.GRWT == 0) {

                $('span[data-valmsg-for="GRWT"]').text('Fill out this field');
                flag = false;
            }
            else {
                if (Number($scope.GRWT) > 100000000) {
                    $('span[data-valmsg-for="GRWT"]').text('WT should be less than 100000000');
                    flag = false;
                }
                else {
                    $('span[data-valmsg-for="GRWT"]').text('');

                }
            }
            if ($scope.Value == '' || $scope.Value == 0) {

                $('span[data-valmsg-for="Value"]').text('Fill out this field');
                flag = false;
            }
            else {
                if (Number($scope.Value) > 100000000) {
                    $('span[data-valmsg-for="Value"]').text('Value should be less than 100000000');
                    flag = false;
                }
                else {
                    $('span[data-valmsg-for="Value"]').text('');

                }
            }

            //if ($scope.Duty == '' || $scope.Duty == 0) {

            //    $('span[data-valmsg-for="Duty"]').text('Fill out this field');
            //    flag = false;
            //}
            //else {
            //    if (Number($scope.Duty) > 100000000) {
            //        $('span[data-valmsg-for="Duty"]').text('Value should be less than 100000000');
            //        flag = false;
            //    }
            //    else {
            //        $('span[data-valmsg-for="Duty"]').text('');

            //    }
            //}
            var SelectFlag = 0;
            SelectFlag = $scope.lstContainerDetails.filter(x=>x.Selected == true);
            if (SelectFlag == 0)
            {
                flag = false;
                alert('Please Select Container');
            }
            return flag
        }

        $scope.AddOblEntry = function () {
            var lengthfag = 0;
           
            if ($scope.SpiltOBLDetails.length > 0)
            {
                var lengthfag = $scope.SpiltOBLDetails.filter(x=>x.SpiltOBL == $scope.SpiltOBLNo).length;

            }
           
            if ($scope.Validation()) {
                if (lengthfag == 0) {


                    $scope.SpiltOBLDetails.push({
                        'SpiltOBL': $scope.SpiltOBLNo, 'SpiltOBLDate': $('#SpiltOBLDate').val(), 
                        'SpiltPkg': $scope.NoOfPkg, 'SpiltWT': $scope.GRWT,
                        'SpiltValue': $scope.Value, 'SpiltDuty': $scope.Duty
                    });


                    for (var i = 0; i < $scope.lstContainerDetails.length; i++)
                    {
                        if($scope.lstContainerDetails[i].Selected==true)
                        {
                            debugger;
                            $scope.SpiltContainerDetails.push({
                                'SpiltOBLNO': $scope.SpiltOBLNo,
                                'SpiltContainerNo': $scope.lstContainerDetails[i].SpiltContainerNo,
                                'SpiltHeaderID': $scope.lstContainerDetails[i].SpiltHeaderID,
                                'SpiltDetailsId': $scope.lstContainerDetails[i].SpiltDetailsId,
                                'SpiltCFSCode':$scope.lstContainerDetails[i].SpiltCFSCode,
                                'SpiltSize': $scope.lstContainerDetails[i].SpiltSize,
                                'Selected':false
                            });
                        }
                        else
                        {
                            $scope.AllUnSelectContainer.push($scope.lstContainerDetails[i]);
                        }
                    }

                 


                   


                    if ($scope.lstContainerDetails.length > 1)
                    {
                        $scope.lstContainerDetails = $scope.AllUnSelectContainer;
                    }
                
                    $scope.AllUnSelectContainer = [];
                    $scope.ResetElement();
                }
                else
                {
                    alert('Duplicate OBL..');
                }
            }

        }


        $scope.ResetElement = function () {
            $scope.SpiltOBLNo = '';
            //$('#SpiltOBLDate').datepicker("setDate", new Date());
            $('#SpiltDate').datepicker("setDate", new Date());
            $('#SpiltOBLDate').val('');
            $scope.CargoDesc = '';          
            $scope.NoOfPkg = '';
            $scope.GRWT = '';            
            $scope.Value = '';
            $scope.Duty = '';
           
          

        }

        $scope.ResetOblDet = function () {
            debugger;
            $scope.SpiltOBLDetails = [];
            $scope.ResetElement();
            $scope.lstContainerDetails = [];
            $.each(allDataStore, function (i, j) {
                allDataStore[i].Selected = false;
            });
            $scope.lstContainerDetails = allDataStore;
           
            
        }


        $scope.OBLSpiltSave = function () {

            debugger;
            if ($scope.SpiltOBLDetails.length < 2) {
                alert('Spilt OBL should be more than one ');

            }
            else {
                if ($scope.oblDetailsJsonData.length == 0) {
                    alert('Please Select OBL');
                }
                else {
                    if ($scope.CountTotalcon <= $scope.SpiltContainerDetails.length) {
                        $scope.varSpiltDate = $('#SpiltDate').val();

                        OBLSpiltService.OBLSpiltSave($scope.oblDetailsJsonData, $scope.SpiltOBLDetails, $scope.SpiltContainerDetails, $scope.isFCL, $scope.varSpiltDate).then(function (response) {
                            debugger;
                            if (response.data.Status == 1) {
                                $scope.SpiltNo = response.data.Data;
                                if ($('#DivMsg').hasClass('logErrMsg'))
                                    $('#DivMsg').removeClass('logErrMsg').addClass('logSuccMsg');
                                $('#DivMsg').html('<span>' + response.data.Message + '</span>');
                                setTimeout($scope.ResetImpOBLSpilt, TimeInSeconds);
                            }

                            else {
                                if ($('#DivMsg').hasClass('logSuccMsg'))
                                    $('#DivMsg').removeClass('logSuccMsg').addClass('logErrMsg');
                                $('#DivMsg').html('<span>' + response.data.Message + '</span>');
                            }
                        });
                    }
                    else
                    {
                        alert('Select All Container For Split');
                    }
                }
            }

        }

        $scope.ResetImpOBLSpilt = function () {
            $('#DivBody').load('/Import/DSR_CWCImport/OBLSplit');
        }




        $scope.Delete = function (index) {

            var r = confirm('Are you sure you want to delete?');

            if (r) {
                debugger;
                var arrIndex = [];
                var OBLNO = $scope.SpiltOBLDetails[index].SpiltOBL;
                if ($scope.CountTotalcon > 1) {
                    debugger;
                    $.each($scope.SpiltContainerDetails, function (key, value) {
                        debugger;
                        if (value.SpiltOBLNO == OBLNO) {
                            $scope.lstContainerDetails.push($scope.SpiltContainerDetails[key]);

                            arrIndex.push({ 'OBL': value.SpiltOBLNO });
                        }

                    });




                    $.each(arrIndex, function (I, J) {
                        debugger;
                        var returnedData = $.grep($scope.SpiltContainerDetails, function (element, index) {
                            if (element.SpiltOBLNO == J.OBL) {
                                return index;
                            }
                        });



                        $scope.SpiltContainerDetails.splice(returnedData, 1);



                    });



                    //  $scope.SpiltContainerDetails.splice(arrIndex, 1);


                }

                $scope.SpiltOBLDetails.splice(index, 1);
            }

        }


        $scope.GetAllSpiltList = function () {
            $('#divlist').load('/Import/DSR_CWCImport/ListOBLSpilt');
        }
    });
})();
