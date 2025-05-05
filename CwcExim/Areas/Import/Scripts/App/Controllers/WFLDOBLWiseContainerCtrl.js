(function () {  
    angular.module('CWCApp').
    controller('WFLDOBLWiseContainerEntryCtrl', function ($scope, WFLDOBLWiseContainerService) {
        $scope.InvoiceNo = "";
        var t2 = 0;
        $scope.OblEntryDetails = [
            {
                'AddID': t2,
                'ID': t2,
                'impobldtlId': 0,
                'DetailsID': 0,
                'CONTCBT':'',
                'ContainerNo': '',
                'ContainerSize': '',
                'NoOfPkg': '',
                'GR_WT': '',
                'ShippingLineId': 0,
                'ShippingLineName': '',
                
            }];
        $scope.OblEntryDetails.length = 0;
       // $scope.OblEntryDetails = [];
        $scope.onchangetext = function (i) {
            var flag1 = 0;

            debugger;
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

        $scope.Action = false;
        GetOBLDetailsOnEditMode();
        $(function () {
          
        });
        


        


        $scope.ChangeCONTCBT = function (index) {
            debugger;
            ind = index;
            
            WFLDOBLWiseContainerService.GetContCBT(index.CONTCBT).then(function (response) {
                debugger;
                $("#ContType").val((index.CONTCBT));
                var html = '';
                var tabindex = 32;
                $.each(response.data, function (item, elem) {
                    debugger;
                    html += '<tr tabindex=' + tabindex + ' onkeypress="SelectContainer(&quot;' + elem.ContainerNo + '&quot;,&quot;' + elem.ShippingLineId + '&quot;,&quot;' + elem.ShippingLine + '&quot;,&quot;' + elem.NoOfPackages + '&quot;,&quot;' + elem.GrossWeight + '&quot;,&quot;' + elem.Size + '&quot;)"  onclick="SelectContainer(&quot;' + elem.ContainerNo + '&quot;,&quot;' + elem.ShippingLineId + '&quot;,&quot;' + elem.ShippingLine + '&quot;,&quot;' + elem.NoOfPackages + '&quot;,&quot;' + elem.GrossWeight + '&quot;,&quot;' + elem.Size + '&quot;)"><td>' + elem.ContainerNo + '</td></tr>';
                    tabindex++;
                });

                $('#tblCont tbody').html(html);

            });
           
        }
        $scope.onShippingLineChange = function (index) {
            debugger;
            ind = index;
        }
        $scope.SelectShippingLine = function (ShippingLineId, ShippingLineName) {
            debugger;
            $scope.OblEntryDetails[ind].ShippingLineName = ShippingLineName;
            $scope.OblEntryDetails[ind].ShippingLineId = ShippingLineId;
            $scope.$apply();
            $("#ShippingLineModal").modal("hide");
        };

        $scope.SelectContainer = function (Cont,Sid,Sname,NoOfPack,GrossWt,Size) {
            debugger;
          
            var flag1 = 0;
          
            $("#ContainerCbtNoModal").modal("hide");
            angular.forEach($scope.OblEntryDetails, function (item) {
                debugger;
                if ((item.ContainerNo.toLowerCase() == Cont.toLowerCase())) {
                    flag1 = 1;
                }
            });


            ind.ContainerNo = Cont;
            ind.ContainerSize = Size;
            ind.NoOfPkg = NoOfPack;
            ind.GR_WT = GrossWt;
            ind.ShippingLineId = Sid;
            ind.ShippingLineName = Sname;

            if (flag1 == 1) {
                
                alert('Can not add duplicate Container No.');

                ind.ContainerNo = "";
            }

            $scope.$apply();
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


     
    

    $scope.CallShippingLineModal= function (index)
    {
        ind = index;
        var CharCode;
        if (window.event) {
            CharCode = window.event.keyCode;
        }
        else {
            CharCode = evt.which;
        }

        if (CharCode == 32) {
            debugger;
            //LoadCountryModal();
            $("#ShippingLineModal").modal('show');
            $('#ShippingLineModal').on('shown.bs.modal', function () {
                $('#ShpngLinebox').focus();
                $('#ShpngLinebox').val('');
            })
            var tabindex = 102;
            debugger;
            //$('#tbleSline  tr').each(function () {
            //    if (this.type != "hidden") {
            //        var $input = $(this);
            //        $input.attr("tabindex", tabindex);
            //        tabindex++;
            //    }
            //});
        }
    }



        //$scope.ChangeCONTCBT = function (i) {
        //    var flag1 = 0;
            
        //        debugger;
        //        if ( i.CONTCBT=='CBT') {
        //            flag1 = 1;
        //        }
           

        //    if (flag1 == 1) {
        //        //alert('jj');
        //        $('#ContainerSize').attr('readonly', true);
        //    }
        //        else
        //    {
        //        $('#ContainerSize').attr('readonly', false);
        //    }
        //}

        
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
                
                if ($scope.OblEntryDetails[len - 1].GR_WT == '' || $scope.OblEntryDetails[len - 1].GR_WT == null) {
                    alert("Please fillup the row");
                    return false;
                }
                if ($scope.OblEntryDetails[len - 1].NoOfPkg == '' || $scope.OblEntryDetails[len - 1].NoOfPkg == null) {
                    alert("Please fillup the row");
                    return false;
                }
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
                       'CONTCBT': '',
                       'ContainerNo': '',
                       'ContainerSize': '',
                       'NoOfPkg': '',
                       'GR_WT': '',
                       'ShippingLineId': 0,
                       'ShippingLineName': '',
                   });
            }
            else {
                $scope.OblEntryDetails.push
                   ({
                       'AddID': t2,
                       'ID': -1,
                       'impobldtlId': 0,
                       'DetailsID': 0,
                       'CONTCBT': '',
                       'ContainerNo': '',
                       'ContainerSize': '',
                       'NoOfPkg': '',
                       'GR_WT': '',
                       'ShippingLineId': 0,
                       'ShippingLineName': '',
                   });
            }
        }
        $scope.GetOBLDetails = function () {
            debugger;
            var OBLNo = $('#OBL_No').val();
            WFLDOBLWiseContainerService.GetOBLDetails(OBLNo).then(function (response) {
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
                $('#AreaCBM').val(response.data.AreaCBM);
                $('#ShippingLineId').val(response.data.ShippingLineId);
                $('#ShippingLineName').val(response.data.ShippingLineName);
                $('#MovementType').val(response.data.MovementType);


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
                              'ShippingLineId': item.ShippingLineId,
                              'ShippingLineName': item.ShippingLineName,
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
                             'ShippingLineId': item.ShippingLineId,
                             'ShippingLineName': item.ShippingLineName,
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
            if (j.DetailsID >0) {
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
            if ($('#MovementType').val() == '') {
                alert('Please Select Movement Type');
                return false;
            }
            if ($('#PortId').val() == '') {
                alert('Please Select Port.');
                return false;
            }
            if ($('#CountryId').val() == '') {
                alert('Please Select Country.');
                return false;
            }
            if ($('#CommodityId').val() == 0) {
                alert('Please Select Commodity');
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
                else if (item.GR_WT == '') {
                    flag5 = 1;
                }
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
                    NoOfPkg:$('#NoOfPkg').val(),
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
                    AreaCBM: $('#AreaCBM').val(),
                    ShippingLineId: $('#ShippingLineId').val(),
                    ShippingLineName: $('#ShippingLineName').val(),
                    MovementType: $('#MovementType').val(),
                    PortId: $('#PortId').val(),
                    CountryId: $('#CountryId').val(),
                    CommodityId: $('#CommodityId').val(),
                }
               
                WFLDOBLWiseContainerService.OBLEntrySave(Obj, JSON.stringify($scope.OblEntryDetails)).then(function (res) {
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
            $('#DivBody').load('/Import/Wfld_OblEntry/OBLWiseContainerEntry');
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