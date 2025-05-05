(function () {  
    angular.module('CWCApp').
    controller('DSROBLWiseContainerEntryCtrl', function ($scope, DSROBLWiseContainerService) {
        $scope.InvoiceNo = "";
        var t2 = 0;
        $scope.FinalOBLDetails = [];
        $scope.OblEntryDetails = [
            {
                'AddID': t2,
                'ID': t2,
                'impobldtlId': 0,
                'DetailsID': 0,
                'CONTCBT': 'CONT',
                'ContainerNo': '',
                'ContainerSize': '',
                'NoOfPkg': '0',
                'GR_WT': '0',
                'Vessel': '',
                'Voyage': '',
                'ForeignLine': '',
                'ShippingLineId': 0,
                'ShippingLineName': '',
                'IsProcessed': 0,
                'Rotation': '',
                //'AreaCBM' : 0,
                
            }];
        $scope.ContainerList = [
           {
               'CFSCode': '',
               'ContainerNo': '',
               'Size': 0,
               'MovementType': '',
              

           }];
      




        $scope.OblEntryDetails.length = 0;
        $scope.onchangetext = function (i) {
            debugger;
            var flag1 = 0;
            angular.forEach($scope.OblEntryDetails, function (item) {
                debugger;
                if ((item.ContainerNo.toLowerCase() == i.ContainerNo.toLowerCase()) && (item.AddID != i.AddID)) {
                    flag1 = 1;
                }
            });

            if (flag1 == 1) {
                i.OBL_No = '';
                alert('Can not add duplicate Container No.');
                
                return false;
            }
        }
        $(function () {
            $scope.Action = false;
            GetOBLDetailsOnEditMode();
        });
        
        $scope.onShippingLineChangeModal = function (index) {
            debugger;
            ind = index;
            var CharCode;
            if (window.event) {
                CharCode = window.event.keyCode;
            }
            else {
                CharCode = evt.which;
            }

            if (CharCode == 32 || CharCode == 13) {
                $("#ShippingLineModal").modal("show");
            }
        }

        $scope.onShippingLineChange = function (index) {
            debugger;
            ind = index;
        }
        $scope.onContainerNoChange = function (index) {
            debugger;
            ind = index;
        }
        $scope.SelectShippingLine = function (ShippingLineId, ShippingLineName) {
            debugger;
            $scope.OblEntryDetails[ind].ShippingLineName = ShippingLineName;
            $scope.OblEntryDetails[ind].ShippingLineId = ShippingLineId;
            $scope.$apply();
            $("#ShippingLineModal").modal("hide");
            //$('#ShippingLineName_' + ind).focus();
            $('#ShpngLinebox').val('');
            $('#btnAddJO').focus();
        };

        $scope.onContainerNoChange = function (index) {
            debugger;
            ind = index;
        }
        //$scope.SelectShippingLine = function (ShippingLineId, ShippingLineName) {
        //    $scope.OblEntryDetails[ind].ShippingLineName = ShippingLineName;
        //    $scope.OblEntryDetails[ind].ShippingLineId = ShippingLineId;
        //    $scope.$apply();
        //    $("#ShippingLineModal").modal("hide");
        //};
        $scope.SetContainer = function (index, ContainerNo, Size, NoOfPkg, ShippingLineId, ShippingLineName, GR_WT, Vessel, Voyage, ForeignLine,Rotation) {
            var flag1 = 0;
            angular.forEach($scope.OblEntryDetails, function (item) {
                debugger;
                if ((item.ContainerNo.toLowerCase() == ContainerNo.toLowerCase())) {
                    flag1 = 1;
                }
            });

            if (flag1 == 1) {
                $('#ContainerNo_' + index).val('');
                $('#ContainerSize_' + index).val('');
                $('#NoOfPkg_' + index).val('');
                $('#ShippingLineId_' + index).val('');
                $('#ShippingLineName_' + index).val('');
                $('#GR_WT_' + index).val('');
                $('#Vessel_' + index).val('');
                $('#Voyage_' + index).val('');
                $('#ForeignLine_' + index).val('');
                $('#Rotation_' + index).val('');
                alert('Can not add duplicate Container No.');
                return false;
            }
            else

            {
                $scope.OblEntryDetails[index].ContainerNo = ContainerNo;
                $scope.OblEntryDetails[index].ContainerSize = Size;
                $scope.OblEntryDetails[index].NoOfPkg = NoOfPkg;
                $scope.OblEntryDetails[index].ShippingLineId = ShippingLineId;
                $scope.OblEntryDetails[index].ShippingLineName = ShippingLineName;
                $scope.OblEntryDetails[index].GR_WT = GR_WT;
                $scope.OblEntryDetails[index].Vessel = Vessel;
                $scope.OblEntryDetails[index].Voyage = Voyage;
                $scope.OblEntryDetails[index].ForeignLine = ForeignLine;
                $scope.OblEntryDetails[index].Rotation = Rotation;
                $scope.$apply();
            }
        
           
           
        };
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
        $scope.CONTCBTList = [
            {
                "id": 'CBT',
                "CONTCBT": "CBT",


            },
            {
                "id": 'CONT',
                "CONTCBT": "Container",

            }
        ];
        
        $scope.OnChangeCONTCBT = function (k)
        {
            debugger;
            $('#ContainerNo_' + k).val('');
            $('#ContainerSize_' + k).val('');
            $('#NoOfPkg_' + k).val('');
            $('#ShippingLineId_' + k).val('');
            $('#ShippingLineName_' + k).val('');
            $('#GR_WT_' + k).val('');
            $('#Vessel_' + k).val('');
            $('#Voyage_' + k).val('');
            $('#ForeginLine_' + k).val('');
            $('#Rotation_' + k).val('');
            $scope.OblEntryDetails[k].ContainerNo = '';
            $scope.OblEntryDetails[k].ContainerSize = '';
            $scope.OblEntryDetails[k].NoOfPkg = '';
            $scope.OblEntryDetails[k].ShippingLineId = '';
            $scope.OblEntryDetails[k].ShippingLineName = '';
            $scope.OblEntryDetails[k].GR_WT = '';
            $scope.OblEntryDetails[k].Vessel = '';
            $scope.OblEntryDetails[k].Voyage = '';
            $scope.OblEntryDetails[k].ForeignLine = '';
            $scope.OblEntryDetails[k].Rotation = '';


            $scope.$apply();
        }
        $scope.ChangeCONTCBT = function (i,k) {
            var flag1 = 0;
            //$('#ContainerNo_' + k).val('');
                debugger;
                if ( i.CONTCBT=='CBT') {
                    flag1 = 1;
                }
           

            if (flag1 == 1) {
                //alert('jj');
                $('#ContainerSize').attr('readonly', true);
            }
                else
            {
                $('#ContainerSize').attr('readonly', false);
            }
            if (i.CONTCBT != '') {
                var Html = '';
                $('#lstContainerNo').html('');
                $('#ContainerNobox').val('');               
                DSROBLWiseContainerService.GetContainerList(i.CONTCBT).then(function (response) {
                    //debugger;                  
                    if (response.data.Data.length > 0)
                    {
                        $scope.ArrayOBLDetails = [];
                        
                        if ($scope.OblEntryDetails.length > 0) {
                            //debugger;
                            $scope.ArrayOBLDetails = $scope.OblEntryDetails.filter(function (finaldata) {
                                return finaldata.ContainerNo !="";
                            })
                            
                            $scope.FinalOBLDetails = response.data.Data;

                            if ($scope.ArrayOBLDetails.length > 0)
                            {                                
                                $scope.FinalOBLDetails.filter($scope.myFunc);
                                //debugger;
                                //console.log($scope.FinalOBLDetails);
                                var tab = 401;
                                $.each($scope.FinalOBLDetails, function (j, item) {
                                    //debugger
                                    Html += '<li id=' + item.ContainerNo + ' tabindex=' + tab + '_' + j + ' onclick="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;,&quot;' + item.Vessel + '&quot;,&quot;' + item.Voyage + '&quot;,&quot;' + item.ForeignLine + '&quot;,&quot;' + item.Rotation + '&quot;)" onkeypress="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;,&quot;' + item.Vessel + '&quot;,&quot;' + item.Voyage + '&quot;,&quot;' + item.ForeignLine + '&quot;,&quot;' + item.Rotation + '&quot;)">' + item.ContainerNo + '</li>';
                                });
                                $('#lstContainerNo').html(Html);
                            }
                            else {
                                //debugger;
                                var tab = 401;
                                $.each(response.data.Data, function (j, item) {
                                    //debugger
                                    Html += '<li id=' + item.ContainerNo + ' tabindex=' + tab + '_' + j + ' onclick="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;,&quot;' + item.Vessel + '&quot;,&quot;' + item.Voyage + '&quot;,&quot;' + item.ForeignLine + '&quot;,&quot;' + item.Rotation + '&quot;)" onkeypress="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;,&quot;' + item.Vessel + '&quot;,&quot;' + item.Voyage + '&quot;,&quot;' + item.ForeignLine + '&quot;,&quot;' + item.Rotation + '&quot;)">' + item.ContainerNo + '</li>';
                                });
                                $('#lstContainerNo').html(Html);
                            }                            
                        }                        
                        
                        //$.each(response.data.Data, function (j, item) {
                        //    debugger
                        //    Html += '<li id=' + item.ContainerNo + ' onclick="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;)">' + item.ContainerNo + '</li>';
                        //});
                        //$('#lstContainerNo').html(Html);

                    }
                    else
                    {
                        $('#lstContainerNo').html('');
                    }                 
                  
                });
            }
            else {
                $('#lstContainerNo').html('');
            }
        }

        $scope.PopupActivateCont = function (Id, i, k) {
            debugger;
            var CharCode;
            if (window.event) {
                CharCode = window.event.keyCode;
            }
            else {
                CharCode = evt.which;
            }

            if (CharCode == 32 || CharCode == 13) {
                $('#' + Id).modal('show');                                         
                var flag1 = 0;               
                debugger;
                if (i.CONTCBT == 'CBT') {
                    flag1 = 1;
                }

                if (flag1 == 1) {
                    //alert('jj');
                    $('#ContainerSize').attr('readonly', true);
                }
                else {
                    $('#ContainerSize').attr('readonly', false);
                }
                if (i.CONTCBT != '') {
                    var Html = '';
                    $('#lstContainerNo').html('');
                    $('#ContainerNobox').val('');
                    DSROBLWiseContainerService.GetContainerList(i.CONTCBT).then(function (response) {
                        //debugger;                  
                        if (response.data.Data.length > 0) {
                            $scope.ArrayOBLDetails = [];

                            if ($scope.OblEntryDetails.length > 0) {
                                //debugger;
                                $scope.ArrayOBLDetails = $scope.OblEntryDetails.filter(function (finaldata) {
                                    return finaldata.ContainerNo != "";
                                })

                                $scope.FinalOBLDetails = response.data.Data;

                                if ($scope.ArrayOBLDetails.length > 0) {
                                    $scope.FinalOBLDetails.filter($scope.myFunc);
                                    //debugger;                                    
                                    var tab = 401;
                                    $.each($scope.FinalOBLDetails, function (j, item) {
                                        //debugger
                                        Html += '<li id=' + item.ContainerNo + ' tabindex=' + tab + '_' + j + ' onclick="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;,&quot;' + item.Vessel + '&quot;,&quot;' + item.Voyage + '&quot;,&quot;' + item.ForeignLine + '&quot;,&quot;' + item.Rotation + '&quot;)" onkeypress="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;,&quot;' + item.Vessel + '&quot;,&quot;' + item.Voyage + '&quot;,&quot;' + item.ForeignLine + '&quot;,&quot;' + item.Rotation + '&quot;)">' + item.ContainerNo + '</li>';
                                    });
                                    $('#lstContainerNo').html(Html);
                                }
                                else {
                                    //debugger;
                                    var tab = 401;
                                    $.each(response.data.Data, function (j, item) {
                                        //debugger
                                        Html += '<li id=' + item.ContainerNo + ' tabindex=' + tab + '_' + j + ' onclick="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;,&quot;' + item.Vessel + '&quot;,&quot;' + item.Voyage + '&quot;,&quot;' + item.ForeignLine + '&quot;,&quot;' + item.Rotation + '&quot;)" onkeypress="FillContainerBox(&quot;' + k + '&quot;,&quot;' + item.ContainerNo + '&quot;,&quot;' + item.Size + '&quot;,&quot;' + item.CFSCode + '&quot;,&quot;' + item.MovementType + '&quot;,&quot;' + item.ShippingLineId + '&quot;,&quot;' + item.ShippingLine + '&quot;,&quot;' + item.GrossWeight + '&quot;,&quot;' + item.NoOfPKG + '&quot;,&quot;' + item.Vessel + '&quot;,&quot;' + item.Voyage + '&quot;,&quot;' + item.ForeignLine + '&quot;,&quot;' + item.Rotation + '&quot;)">' + item.ContainerNo + '</li>';
                                    });
                                    $('#lstContainerNo').html(Html);
                                }
                            }

                        }
                        else {
                            $('#lstContainerNo').html('');
                        }

                    });
                }
                else {
                    $('#lstContainerNo').html('');
                }

                $('#ContainerNobox').focus();
            }
        }
        //$scope.finished = function ()
        //{
        //    debugger;
        //    alert('aa');
        //    //document.getElementById('ContainerNo_' + len).focus();
        //    //$('#ContainerNo_' + len).focus();          
        //}
       

        $scope.myFunc = function () {          
            //debugger;
            for (var i = 0; i < $scope.ArrayOBLDetails.length; ++i) {
                // to remove every occurrence of the matched value
                for (var j = $scope.FinalOBLDetails.length; j--;) {
                    if ($scope.FinalOBLDetails[j].ContainerNo === $scope.ArrayOBLDetails[i].ContainerNo) {
                        // remove the element
                        $scope.FinalOBLDetails.splice(j, 1);
                    }
                }
            }
        }
        
        $scope.AddOblEntry = function () {
            debugger;
            t2 = t2 + 1;            
            var len = $scope.OblEntryDetails.length;
            if (len > 0) {
                if ($scope.OblEntryDetails[len - 1].CONTCBT == '' || $scope.OblEntryDetails[len - 1].CONTCBT == null) {
                    alert("Please fillup the row");
                    return false;
                }
                if ($scope.OblEntryDetails[len - 1].ContainerNo == '' || $scope.OblEntryDetails[len - 1].ContainerNo == null) {
                    alert("Please fillup the row");
                    return false;
                }
                //if ($scope.OblEntryDetails[len - 1].CONTCBT = 'Container')
                //{
                //    if ($scope.OblEntryDetails[len - 1].ContainerSize == '' || $scope.OblEntryDetails[len - 1].ContainerSize == null) {
                //        alert("Please fillup the row");
                //        return false;
                //    }
                //}
                $scope.OblEntryDetails[len - 1].GR_WT = 0;
                $scope.OblEntryDetails[len - 1].NoOfPkg = 0;
                /*if ($scope.OblEntryDetails[len - 1].GR_WT != '' || $scope.OblEntryDetails[len - 1].GR_WT == null) {
                    alert("Please fillup the row");
                    return false;
                }
                if ($scope.OblEntryDetails[len - 1].NoOfPkg == '' || $scope.OblEntryDetails[len - 1].NoOfPkg == null) {
                    alert("Please fillup the row");
                    return false;
                }*/
                if ($scope.OblEntryDetails[len - 1].ShippingLineId == 0 || $scope.OblEntryDetails[len - 1].ShippingLineId == null) {
                    alert("Please fillup the row");
                    return false;
                }
                $scope.OblEntryDetails.push
                   ({
                       'AddID': t2,
                       'ID': -1,
                       'impobldtlId': 0,
                       'DetailsID': 0,
                       'CONTCBT': 'CONT',
                       'ContainerNo': '',
                       'ContainerSize': '',
                       'NoOfPkg': '',
                       'GR_WT': '',
                       'Vessel': '',
                       'Voyage': '',
                       'ForeignLine': '',
                       'ShippingLineId': 0,
                       'ShippingLineName': '',
                       'IsProcessed': 0,
                       'Rotation':'',
                   });
            }
            else {
                $scope.OblEntryDetails.push
                   ({
                       'AddID': t2,
                       'ID': -1,
                       'impobldtlId': 0,
                       'DetailsID': 0,
                       'CONTCBT': 'CONT',
                       'ContainerNo': '',
                       'ContainerSize': '',
                       'NoOfPkg': '',
                       'GR_WT': '',
                       'Vessel': '',
                       'Voyage': '',
                       'ForeignLine': '',
                       'ShippingLineId': 0,
                       'ShippingLineName': '',
                       'IsProcessed': 0,
                       'Rotation':'',
                   });
               
            }
            debugger;           
            setTimeout(function () {
                //DOM has finished rendering                             
                $('#ContainerNo_'+len).focus();
            },300);
           // finished(len);
        }
        $scope.GetOBLDetails = function () {
            debugger;
            var OBLNo = $('#OBL_No').val();
            DSROBLWiseContainerService.GetOBLDetails(OBLNo).then(function (response) {
                debugger;
                $scope.OblEntryDetails = [];
                if (response.data.OblEntryDetailsList.length == 0) {
                    alert('No record found for given IGM No.');
                    return false;
                }
                $('#OBL_No').val(response.data.OBL_No);
                $('#OBL_Date').val(response.data.OBL_Date);
                $('#LineNo').val(response.data.LINE_NO);
                $('#SMTPNo').val(response.data.SMTPNo);
                $('#SMTP_Date').val(response.data.SMTP_Date);
                $('#NoOfPkg').val(response.data.NoOfPkg);
                $('#CargoType').val(response.data.CargoType);
                $('#CargoDescription').val(response.data.CargoDescription);
                $('#PkgType').val(response.data.PkgType);
                $('#NoOfPkg').val(response.data.NoOfPkg);
                $('#GR_WT').val(response.data.GR_WT);
                $('#ImporterId').val(response.data.ImporterId);
                $('#ImporterName').val(response.data.ImporterName);
                $('#IGM_No').val(response.data.IGM_No);
                $('#IGM_Date').val(response.data.IGM_Date);
                $('#TPNo').val(response.data.TPNo);
                $('#TPDate').val(response.data.TPDate);
                $('#TSANo').val(response.data.TSANo);
                $('#TSA_Date').val(response.data.TSA_Date);
                $('#CIFValue').val(response.data.CIFValue);
                $('#AreaCBM').val(response.data.AreaCBM==""?0:response.data.AreaCBM);
                $('#ShippingLineId').val(response.data.ShippingLineId);
                $('#ShippingLineName').val(response.data.ShippingLineName);
                $('#MovementType').val(response.data.MovementType);
                $('#CHAId').val(response.data.CHAId);
                $('#CHAName').val(response.data.CHAName);


                var j = 0;
                angular.forEach(response.data.OblEntryDetailsList, function (item) {
                    angular.forEach($scope.OblEntryDetails, function (item1) {
                        if (item1.ContainerNo == item.ContainerNo) {
                              j = 1;
                          }
                          item1.ContainerSize = "";
                      });
                      debugger;
                      if (j == 0) {
                          $scope.OblEntryDetails.push
                           ({
                              'ID': t2 + 1,
                              'DetailsID': 0,
                              'impobldtlId': item.Id,
                              'CONTCBT': item.CONTCBT,
                              'ContainerNo': item.ContainerNo,
                              'ContainerSize': item.ContainerSize,
                              'NoOfPkg': item.NoOfPkg,
                              'GR_WT': item.GR_WT,
                              'Vessel': item.Vessel,
                              'Voyage': item.Voyage,
                              'ForeignLine': item.ForeignLine,
                              'ShippingLineId': item.ShippingLineId,
                              'ShippingLineName': item.ShippingLineName,
                              'IsProcessed': 0,
                              'Rotation':item.Rotation,
                               
                         });
                      }
                    });
             });
        }
        function GetOBLDetailsOnEditMode()
        {
            debugger;
            if ($('#impobldtlId').val() != 0) {
                $scope.Action = true;
                var SerializedData = $.parseJSON($('#StringifiedText').val());
                //$scope.OblEntryDetails = $.parseJSON(SerializedData);
                angular.forEach(SerializedData, function (item) {
                    debugger;
                        $scope.OblEntryDetails.push
                         ({
                             'ID': 0,
                             'DetailsID':item.DetailsID,
                             'impobldtlId': item.impobldtlId,
                             'CONTCBT': item.CONTCBT,
                             'ContainerNo': item.ContainerNo,
                             'ContainerSize': item.ContainerSize,
                             'NoOfPkg': item.NoOfPkg,
                             'PkgType': item.PkgType,
                             'GR_WT': item.GR_WT,
                             'Vessel': item.Vessel,
                             'Voyage': item.Voyage,
                             'ForeignLine': item.ForeignLine,
                             'ShippingLineId': item.ShippingLineId,
                             'ShippingLineName': item.ShippingLineName,
                             'IsProcessed': item.IsProcessed,
                             'Rotation': item.Rotation,
                         });

                });
                //$('#btnSave').attr("disabled", true);
            }
        }

        $scope.ResetImpJODetails=function() {
            $scope.OblEntryDetails.length = 0;
        }
        $scope.Delete = function (val,j) {
            debugger;
            if (j.IsProcessed > 0) {
                alert('Can not delete as this Container is already Processed');
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
            if ($('#OBL_No').val() == null || $('#OBL_No').val() == '') {
                alert('Please Select OBL NO');
                 return false;
             }
            if ($('#OBL_Date').val() == null || $('#OBL_Date').val() == '') {
                alert('Please Enter OBL DATE');
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
            if ($('#CommodityId').val() == 0) {
                alert('Please Select Commodity');
                return false;
            }
            if ($('#CargoType').val() == '') {
                alert('Please Select Package Type');
                return false;
            }

            if (Number($('#NoOfPkg').val()) <= 0) {
                alert('No of Package must be greater than 0');
                return false;
            }
            
            if ($('#PkgType').val() == '') {
                alert('Please Select Package Type');
                return false;
            }
            if (Number($('#GR_WT').val()) <= 0) {
                alert('Gross weight must be greater than 0');
                return false;
            }
            if ($('#ImporterName').val() == '') {
                alert('Please Select Importer.');
                return false;
            }            
            
            if ($('#MovementType').val() == '') {
                alert('Please Select Movement Type');
                return false;
            }
            if (Number($('#CIFValue').val()) <= 0) {
                alert('CIF Value must be greater than 0');
                return false;
            }
            //if (Number($('#AreaCBM').val()) <= 0) {
            //    alert('CBM Value must be greater than 0');
            //    return false;
            //}

            if ($('#CountryName').val() == '') {
                alert('Please Select Country.');
                return false;
            }

            if ($('#PortName').val() == '') {
                alert('Please Select Port.');
                return false;
            }
            
            if ($('#ShippingLineName').val() == '') {
                alert('Please Select Shipping Line.');
                return false;
            }
            
            var flag1 = 0;
            var flag2 = 0;
            var flag3 = 0;
            var flag4 = 0;
            var flag5 = 0;
            var flag6 = 0;
            angular.forEach($scope.OblEntryDetails, function (item) {
                debugger;
                if (item.CONTCBT == '') {
                    flag6 = 1;
                }
                else if (item.ContainerNo == '') {
                    flag1 = 1;
                }
                else if (item.ContainerSize == "0" || item.ContainerSize == "") {
                    if (item.CONTCBT = 'CBT')
                    {
                        flag3 = 0;
                    }
                    else
                    {
                        flag3 = 1;
                    }
                    
                }
                //else if (item.GR_WT == '') {
                //    flag5 = 1;
                //}
                else if (item.ShippingLineId == 0) {
                    flag2 = 1;
                }
            });
            if (flag6 == 1) {
                alert('Please Enter All Container or CBT.');
                return false;
            }
            if(flag1 == 1){
                alert('Please Enter All Container No.');
                return false;
            }
            if (flag3 == 1) {
                alert('Please Select All Container size');
                return false;
            }
            if (flag2 == 1) {
                alert('Please Select All Shipping Line');
                return false;
            }
            if (flag5 == 1) {
                alert('Please Enter Gross Wt.');
                return false;
            }
            debugger;
            var len = $scope.OblEntryDetails.length;
            if (len == 0) {
                alert('At least one record should required for obl entry Fcl');
                return false;
            }
            
            if (confirm('Are you sure to save OBL Entry?')) {
                debugger;
                Obj = {
                    impobldtlId:$('#impobldtlId').val() == undefined ? 0 : $('#impobldtlId').val(),
                    OBL_No:$('#OBL_No').val(),
                    OBL_Date:$('#OBL_Date').val(),
                    LINE_NO:$('#LineNo').val(),
                    SMTPNo: $('#SMTPNo').val(),
                    SMTP_Date: $('#SMTP_Date').val(),
                    NoOfPkg:$('#NoOfPkg').val(),
                    CargoType:$('#CargoType').val(),
                    CargoDescription:$('#CargoDescription').val(),
                    PkgType:$('#PkgType').val(),
                    //NoOfPkg:$('#NoOfPkg').val(),
                    GR_WT:$('#GR_WT').val(),
                    ImporterId:$('#ImporterId').val(),
                    ImporterName:$('#ImporterName').val(),
                    IGM_No:$('#IGM_No').val(),
                    IGM_Date:$('#IGM_Date').val(),
                    TPNo:$('#TPNo').val(),
                    TPDate: $('#TPDate').val(),
                    TSANo: $('#TSANo').val(),
                    TSA_Date: $('#TSA_Date').val(),
                    CIFValue: $('#CIFValue').val(),
                    //AreaCBM: $('#AreaCBM').val(),
                    'AreaCBM': $('#AreaCBM').val() == "" ? 0 : $('#AreaCBM').val(),
                    ShippingLineId: $('#ShippingLineId').val(),
                    ShippingLineName: $('#ShippingLineName').val(),
                    MovementType: $('#MovementType').val(),
                    PortId: $('#PortId').val(),
                    CountryId: $('#CountryId').val(),
                    CommodityId: $('#CommodityId').val(),
                    CHAId: $('#CHAId').val(),
                    CHAName: $('#CHAName').val(),
                }
               
                DSROBLWiseContainerService.OBLEntrySave(Obj, JSON.stringify($scope.OblEntryDetails)).then(function (res) {
                    console.log(res);
                    //alert(res.data.Message);
                    if (res.data.Status == 3 || res.data.Status == 6)
                    {
                        $('#btnSave').attr("disabled", false);
                        alert(res.data.Message);                        
                        return false;
                    }
                    else
                    {
                        alert(res.data.Message);
                        $('#btnSave').attr("disabled", true);
                        setTimeout(LoadOblEntry, 3000);
                    }
                    
                });
            }
        }
        function LoadOblEntry() {
            $('#DivBody').load('/Import/DSR_OblEntry/OBLWiseContainerEntry');
        }
        $scope.ResetJODet = function () {

            debugger;
            //$('#OBL_No,#OBL_Date,#LineNo,#SMTPNo,#NoOfPkg,#CargoDescription,#PkgType,#GR_WT,#ImporterName').val('');
            $scope.OblEntryDetails=[];
            //$('#CargoType').val(0);
            $('#btnAddJO').attr('disabled', false);
            $('#btnResetJO').attr('disabled', false);
        }
    });
})();