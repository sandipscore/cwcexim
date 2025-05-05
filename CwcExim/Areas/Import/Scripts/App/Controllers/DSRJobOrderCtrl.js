(function () {
    angular.module('CWCApp').
    controller('DSRJobOrderCtrl', function ($scope, DSRJobOrderService) {
        $scope.InvoiceNo = "";
        document.getElementById("DivFormOneDet").style.display = 'block';
        document.getElementById("DivFormOneDett").style.display = 'none';
        var t2 = 0;
        $scope.ActiveControl = false;
        $scope.WithSummary = false;
       
        $scope.WithShow = false;
        $scope.JODetails = [
            {
                'ID': t2,
                'ChargeId': 0,
                "TrainSummarySerial": 0,
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
                "PolId": 0,
                "PolName": '',

                "ImporterName": '',
                "ImporterId": 0,
                "PayerName": '',
                "PayerId": 0,
                "NewImporterName": '',
                "NewImporterId": 0,
                "CargoType": '2',
                "ContainerLoadType": '',
                "ContainerODCType": 0,
                "TransportForm": '',
                "NoOfPackages": 0,
                "Line_Seal_No": '',
                "Cont_Commodity": '',
                "S_Line": '',
                "Ct_Tare": 0,
                "Cargo_Wt": 0,
                "Gross_Wt": 0,
                "Ct_Status": 0,
                "Ct_ODC":0,
                "Destination": '',
                "Smtp_No": '',
                "Received_Date": '',
                "Genhaz": '',
                "Remarks": '',
                "InvoiceAmt": 0,
                "Roundup": 0,
                "CargoDescription": '',
                "Taxable": 0,
                "IGSTPer": 0,
                "IGSTAmt": 0,
                "CGSTPer": 0,
                "CGSTAmt": 0,
                "SGSTPer": 0,
                "SGSTAmt": 0,
                "InoviceId": '',
                "OperationId": '',
                "ChargeType": '',
                "ChargeName": '',
                "SACCode": '',
                "Clause": '',
                "GST": 0,

            }];


        $scope.JOViewDetails = [
          {
              'ID': t2,
              'ChargeId': 0,
              "TrainSummarySerial": 0,
              "TrainSummaryID": 0,
              "TrainNo": '',
              "TrainDate": '',
              "PortId": 0,
              "Wagon_No": '',
              "Container_No": '',
              "CT_Size": '',
              "CustomSealNo": '',
              "PolId": 0,
              "PolName": '',
              "ShippingLineNo": '',
              "ShippingLineId": 0,
              "ShippingLineName": '',


              "ImporterName": '',
              "ImporterId": 0,
              "PayerName": '',
              "PayerId": 0,
              "NewImporterName": '',
              "NewImporterId": 0,
              "CargoType": '2',
              "ContainerLoadType": '',
              "ContainerODCType": 0,
              "TransportForm": '',
              "NoOfPackages": 0,
              "Line_Seal_No": '',
              "Cont_Commodity": '',
              "S_Line": '',
              "Ct_Tare": 0,
              "Cargo_Wt": 0,
              "Gross_Wt": 0,
              "Ct_Status": '',
              "Ct_ODC": '',
              "Destination": '',
              "Smtp_No": '',
              "Received_Date": '',
              "Genhaz": '',
              "Remarks": '',
              "InvoiceAmt": 0,
              "Total": 0,
              "Roundup": 0,
              "CargoDescription": '',
              "Taxable": 0,
              "IGSTPer": 0,
              "IGSTAmt": 0,
              "CGSTPer": 0,
              "CGSTAmt": 0,
              "SGSTPer": 0,
              "SGSTAmt": 0,
              "InoviceId": '',
              "OperationId": '',
              "ChargeType": '',
              "ChargeName": '',
              "SACCode": '',
              "Clause": '',
              "GST": 0,

          }];



        $scope.ConDetails = [
           {
               'ID': t2,
               'ChargeId': 0,
               "TrainSummarySerial": 0,
               "TrainSummaryID": 0,
               "TrainNo": '',
               "TrainDate": '',
               "PortId": 0,
               "PolId": 0,
               "Wagon_No": '',
               "Container_No": '',
               "CT_Size": '',
               "CustomSealNo": '',

               "ShippingLineNo": '',
               "ShippingLineId": 0,
               "ShippingLineName": '',


               "ImporterName": '',
               "ImporterId": 0,
               "PayerName": '',
               "PayerId": 0,
               "NewImporterName": '',
               "NewImporterId": 0,
               "CargoType": '2',
               "ContainerLoadType": '',
               "ContainerODCType": 0,
               "TransportForm": '',
               "NoOfPackages": 0,
               "Line_Seal_No": '',
               "Cont_Commodity": '',
               "S_Line": '',
               "Ct_Tare": 0,
               "Cargo_Wt": 0,
               "Gross_Wt": 0,
               "Ct_Status": '',
               "Ct_ODC": '',
               "Destination": '',
               "Smtp_No": '',
               "Received_Date": '',
               "Genhaz": '',
               "Remarks": '',

           }];
        $scope.JODetails.length = 0;

        $(function () {
            document.getElementById("DivFormOneDet").style.display = 'block';
            document.getElementById("DivFormOneDett").style.display = 'none';
            GetTrainDetailsOnEditMode();
            DSRJobOrderService.GetPort();


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
        $scope.ContainerODCTypeList = [
              {
                  "id": 0,
                  "ContainerODCType": "Normal",


              },
             {
                 "id": 1,
                 "ContainerODCType": "Single Side ODC",


             },
             {
                 "id": 2,
                 "ContainerODCType": "Double Side ODC",
             },
             {
                 "id": 3,
                 "ContainerODCType": "Three Side ODC",
             }

        ];
        $scope.CargoTypeList = [
             {
                 "id": '2',
                 "CargoType": "NON-HAZ",

             },
            {
                "id": '1',
                "CargoType": "HAZ",
            }

        ];
       
        $scope.SizeList = [
         {
             "id": '20',
             "CT_Size": "20",

         },
        {
            "id": '40',
            "CT_Size": "40",


        }

        ];
        DSRJobOrderService.GetPort().then(function (response) {
            debugger;

            $scope.TransportFromList = [{
                "PortId": 0,
                "PortName": "--Select--"
            }];
            for (var i = 0; i < response.data.length; i++) {
                $scope.TransportFromList.push(response.data[i])
            }

        });

        $scope.ResetArray = function () {
            debugger;
            $scope.JODetails.length = 0;
            $scope.ActiveControl = false;
            $scope.WithOut = false;
        }
        $scope.HiddenArray = function () {           
            $scope.WithOut = true;
        }
        $scope.GetBlankTrainDetails = function () {
            debugger;
            document.getElementById("DivFormOneDet").style.display = 'block';
            document.getElementById("DivFormOneDett").style.display = 'none';

            debugger;
            var joborderln = $scope.JODetails.length;
            $("#hdnlength").val(joborderln);

            $scope.JODetails.push
                ({
                    'ID': t2,
                    'ChargeId': 0,
                    "TrainSummarySerial": 0,
                    "TrainSummaryID": 0,
                    "TrainNo": '',
                    "TrainDate": '',
                    "PortId": 0,
                    "Wagon_No": '',
                    "Container_No": '',
                    "CT_Size": '',
                    "CustomSealNo": '',
                    "PolId": 0,
                    "PolName": '',
                    "ShippingLineNo": '',
                    "ShippingLineId": 0,
                    "ShippingLineName": '',
                    "ImporterName": '',
                    "ImporterId": 0,
                    "PayerName": '',
                    "PayerId": 0,
                    "NewImporterName": '',
                    "NewImporterId": 0,
                    "CargoType": '2',
                    "ContainerLoadType": '',
                    "ContainerODCType": 0,
                    "TransportForm": '',
                    "NoOfPackages": 0,
                    "Line_Seal_No": '',
                    "Cont_Commodity": '',
                    "S_Line": '',
                    "Ct_Tare": 0,
                    "Cargo_Wt": 0,
                    "Gross_Wt": 0,
                    "Ct_Status": '',
                    "Ct_ODC": '',
                    "Destination": '',
                    "Smtp_No": '',
                    "Received_Date": '',
                    "Genhaz": '',
                    "Remarks": '',
                    "InvoiceAmt": 0,
                    "Roundup": 0,
                    "CargoDescription": '',
                    "Taxable": 0,
                    "IGSTPer": 0,
                    "IGSTAmt": 0,
                    "CGSTPer": 0,
                    "CGSTAmt": 0,
                    "SGSTPer": 0,
                    "SGSTAmt": 0,
                    "InoviceId": '',
                    "OperationId": '',
                    "ChargeType": '',
                    "ChargeName": '',
                    "SACCode": '',
                    "Clause": '',
                    "GST": 0,
                });
          
           
        }
        
        $scope.GetTrainDetails = function () {
            debugger;
            document.getElementById("DivFormOneDet").style.display = 'block';
            document.getElementById("DivFormOneDett").style.display = 'none';
            var TSumId = $('#TrainSummaryID').val();
            DSRJobOrderService.GetTrainDetails(TSumId).then(function (response) {
                debugger;
                if ($('#ImpJobOrderId').val()== 0) {
                    $scope.JODetails = response.data;
                    $('#TrainDate').val(response.data[0].TrainDate);
                    angular.forEach($scope.JODetails, function (item) {
                        
                        item.CargoType = '';                       
                    });
                    
                    $('#TrainDate').focus();
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
                                "Wagon_No": response.data[i].Wagon_No,
                                "Container_No": response.data[i].Container_No,
                                "CT_Size": response.data[i].CT_Size,
                                "CustomSealNo": response.data[i].CustomSealNo,
                                "ShippingLineNo": response.data[i].ShippingLineNo,
                                "ShippingLineId": response.data[i].ShippingLineId,
                                "ShippingLineName": response.data[i].ShippingLineName,

                                "ImporterName": response.data[i].ImporterName,
                                "ImporterId": response.data[i].ImporterId,
                                "PayerName": response.data[i].PayerName,
                                "PayerId": response.data[i].PayerId,
                                "NewImporterName": response.data[i].ImporterName,
                                "NewImporterId": response.data[i].ImporterId,

                                "CargoType": '2',
                                "ContainerLoadType": response.data[i].ContainerLoadType,
                                "ContainerODCType": response.data[i].ContainerODCType,
                                "TransportForm": response.data[i].TransportForm,
                                "NoOfPackages": response.data[i].NoOfPackages,
                                "Line_Seal_No": response.data[i].Line_Seal_No,
                                "Cont_Commodity": response.data[i].Cont_Commodity,
                                "S_Line": response.data[i].S_Line,
                                "Ct_Tare": response.data[i].Ct_Tare,
                                "Cargo_Wt": response.data[i].Cargo_Wt,
                                "Gross_Wt": response.data[i].Gross_Wt,
                                "Ct_Status": response.data[i].Ct_Status,
                                "Ct_ODC": response.data[i].Ct_ODC,
                                "Destination": response.data[i].Destination,
                                "Smtp_No": response.data[i].Smtp_No,
                                "Received_Date": response.data[i].Received_Date,
                                "Genhaz": response.data[i].Genhaz,
                                "Remarks": response.data[i].Remarks,
                                "CargoDescription": response.data[i].CargoDescription
                            });
                    });
                }
            });
            //} 
        }
        function GetTrainDetailsOnEditMode() {
            debugger;
            document.getElementById("DivFormOneDet").style.display = 'block';
            document.getElementById("DivFormOneDett").style.display = 'none';
            if ($('#ImpJobOrderId').val()!= 0) {
                DSRJobOrderService.GetTrainDetailsOnEditMode($('#ImpJobOrderId').val()).then(function (response) {
                    debugger;
                    if ($('#TrainSummaryID').val() > 0) {
                        $('#WithsummaryCheck').val('1');
                        $('#rdWithSummary').prop('checked', true);
                        $('#divTrainSearch').css("display", "");
                        $('#divAddNewRow').css("display", "none");
                        $('#divTraderModal').css("display", "none");

                        $('#thSline').css("display", "");
                        $('#thSlineNo').css("display", "");
                        $('#thParty').css("display", "");
                        $('#thPayer').css("display", "");
                        $('#thImporter').css("display", "");
                        $('#thPort').css("display", "");
                        $('#thPol').css("display", "");
                        $('#thRemarks').css("display", "");
                    }
                    else {


                        $('#WithsummaryCheck').val('0');
                        $('#rdWithOutSummary').prop('Without', true);

                        $('#rdWithOutSummary').prop('checked', true);
                        $('#divTrainSearch').css("display", "none");
                        $('#divAddNewRow').css("display", "none");
                        $('#divTraderModal').css("display", "");

                        $('#thSline').css("display", "none");
                        $('#thSlineNo').css("display", "none");
                        $('#thParty').css("display", "none");
                        $('#thPayer').css("display", "none");
                        $('#thImporter').css("display", "none");
                        $('#thPort').css("display", "none");
                        $('#thPol').css("display", "none");
                        $('#thRemarks').css("display", "none");
                    }
                    $scope.ActiveControl = true;
                    $scope.JODetails = response.data;
                });
            }
        }



        var ind = 0;
        $scope.onShippingLineChange = function (index) {
            debugger;
            ind = index;
        }
        $scope.SelectShippingLine = function (ShippingLineId, ShippingLineName, PartyCode) {
            debugger;
            $scope.JODetails[ind].ShippingLineId = ShippingLineId;
            $scope.JODetails[ind].ShippingLineName = ShippingLineName;
            $scope.JODetails[ind].S_Line = PartyCode;
            $scope.JODetails[ind].ShippingLineNo = PartyCode;
            $('#ShpngLinebox').val('');
            //$("#slmodal").modal("hide");
            LoadEximTrader();
            $scope.$applyAsync();
            $("#slmodal").modal("hide");
            $('#S_Line_' + ind).focus();
        }



        $scope.SelectShippingLineKeyPress = function (ShippingLineId, ShippingLineName, PartyCode) {
            debugger;

            var CharCode;
            if (window.event) {
                CharCode = window.event.keyCode;
            }
            else {
                CharCode = evt.which;
            }
            //if (CharCode == 13) {


                $scope.JODetails[ind].ShippingLineId = ShippingLineId;
                $scope.JODetails[ind].ShippingLineName = ShippingLineName;
                $scope.JODetails[ind].S_Line = PartyCode;
                $scope.JODetails[ind].ShippingLineNo = PartyCode;
                $('#ShpngLinebox').val('');
                //$("#slmodal").modal("hide");
                LoadEximTrader();
                $scope.$applyAsync();
                $("#slmodal").modal("hide");
                $('#S_Line_' + ind).focus();
                
            //}
        };

        var ind = 0;
        $scope.onCHAChange = function (index) {
            debugger;
            ind = index;
        }

        $scope.onImporterChange = function (index) {
            debugger;
            ind = index;
        }
        $scope.SelectCHAline = function (ImporterId, ImporterName) {
            debugger;
            $scope.JODetails[ind].ImporterId = ImporterId;
            $scope.JODetails[ind].ImporterName = ImporterName;
            $("#CHAModal").modal("hide");
            $('#CHAbox').val('');
            LoadCHA();
            $scope.$applyAsync();
            $("#CHAModal").modal("hide");
            $('#PayerName_' + ind).focus();
        }



        $scope.SelectCHAlineKey = function (ImporterId, ImporterName) {
            debugger;
            var CharCode;
            if (window.event) {
                CharCode = window.event.keyCode;
            }
            else {
                CharCode = evt.which;
            }
            //if (CharCode == 13) {

                $scope.JODetails[ind].ImporterId = ImporterId;
                $scope.JODetails[ind].ImporterName = ImporterName;
                $("#CHAModal").modal("hide");
                $('#CHAbox').val('');
                LoadCHA();
                $scope.$applyAsync();
                $("#CHAModal").modal("hide");
                $('#PayerName_' + ind).focus(); 

            //}
        }


        $scope.SelectImporterline = function (ImporterId, ImporterName) {
            debugger;

            $scope.JODetails[ind].NewImporterId = ImporterId;
            $scope.JODetails[ind].NewImporterName = ImporterName;
            $("#NewImporterModal").modal("hide");
            $('#NewImporterbox').val('');
            //LoadCHA();
            LoadNewImporter();
            $scope.$applyAsync();
            $("#NewImporterModal").modal("hide");
            $('#CargoType_' + ind).focus();
        }

        $scope.SelectImporterlinekey = function (ImporterId, ImporterName) {
            debugger;
            var CharCode;
            if (window.event) {
                CharCode = window.event.keyCode;
            }
            else {
                CharCode = evt.which;
            }
            //if (CharCode == 13) {
                $scope.JODetails[ind].NewImporterId = ImporterId;
                $scope.JODetails[ind].NewImporterName = ImporterName;
                $("#NewImporterModal").modal("hide");
                $('#NewImporterbox').val('');
            //LoadCHA();
                LoadNewImporter();
                $scope.$applyAsync();
                $("#NewImporterModal").modal("hide");
                $('#CargoType_' + ind).focus();
            //}
        }

        var ind = 0;
        $scope.onPayerChange = function (index) {
            debugger;
            ind = index;
        }
        $scope.SelectPayerline = function (PayerId, PayerName) {
            debugger;
            $scope.JODetails[ind].PayerId = PayerId;
            $scope.JODetails[ind].PayerName = PayerName;
            $('#PayerLinebox').val('');
            $("#PayerModal").modal("hide");
            LoadPayer();
            $scope.$applyAsync();
            $("#PayerModal").modal("hide");
            $('#NewImporterName_' + ind).focus();
            
        }


        $scope.SelectPayerlinekey = function (PayerId, PayerName) {
            debugger;
            var CharCode;
            if (window.event) {
                CharCode = window.event.keyCode;
            }
            else {
                CharCode = evt.which;
            }
            //if (CharCode == 13) {
                $scope.JODetails[ind].PayerId = PayerId;
                $scope.JODetails[ind].PayerName = PayerName;
                $('#PayerLinebox').val('');
                $("#PayerModal").modal("hide");
                LoadPayer();
                $scope.$applyAsync();
                $("#PayerModal").modal("hide");
                $('#NewImporterName_' + ind).focus();
           // }
        }


        $scope.ResetImpJODetails = function () {
            document.getElementById("DivFormOneDet").style.display = 'block';
            document.getElementById("DivFormOneDett").style.display = 'none';
            $('#lstYard,#lstYardWiseLctn').html('');           
            $('#FormOneId,#FormOneDetailId,#ImpJobOrderId').val('');
            $('#PickUpLocation,#Remarks,#TrainNo,#JobOrderNo,#TrainNobox').val('');            
            $scope.ActiveControl = true;
            $scope.JODetails.length = 0;
           
            $('#DivBody').load('/Import/DSR_CWCImport/CreateJobOrder');
        }





        var Obj = {};
        $scope.GenerateInvoice = function () {

            var SEZ = $('#SEZ').val();
            debugger;
            var len = $scope.JODetails.length;
            if (len == 0) {
                alert('At least one record should required');
                return;
            }

            $('#btnPrint').attr("disabled", true);
            $('#btnPrint').prop('disabled', true);
            
            var x = $('#JobOrderDate').val();
            var reg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
            if (x.match(reg)) {
                //return true;
            }
            else {
                alert("Job Order Date should be dd/mm/yyyy");
                return false;
            }

            if ($('#WithsummaryCheck').val() == '1') {
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
            }
            else {
                $('#TrainNo').val('');
                TrainDate: $('#TrainDate').val(null);
                TrainSummarySerial: $('#TrainSummarySerial').val(0);
                TrainSummaryID: $('#TrainSummaryID').val(0);
                angular.forEach($scope.JODetails, function (item) {
                    if (item.ShippingLineName == '') {
                        item.ShippingLineName = $('#ShippingLineName').val();
                        item.ShippingLineId = $('#ShippingLineId').val();
                    }
                    if (item.ImporterName == '') {
                        item.ImporterName = $('#PartyName').val();// Party
                        item.ImporterId = $('#PartyId').val();// Party
                    }
                    if (item.PortId == 0) {
                        item.PortId = $('#PortId').val();
                    }
                    if (item.PolId == 0) {
                        item.PolId = $('#PolId').val();
                    }
                    if (item.NewImporterName == '') {
                        item.NewImporterName = $('#ImporterName').val();
                        item.NewImporterId = $('#ImporterId').val();
                    }
                    if (item.PayerName == '') {
                        item.PayerName = $('#PayerName').val();
                        item.PayerId = $('#PayerId').val();
                    }
                    if (item.Remarks == '') {
                        item.Remarks = $('#Remarks').val();                        
                    }
                });
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
            var flag11 = 0;
            angular.forEach($scope.JODetails, function (item) {




                if (item.ShippingLineName== '') {
                    flag1 = 1;
                }
                else if (item.CargoType== '' || item.CargoType == null) {
                    flag2 = 1;
                }
                else if (item.PortId == 0) {
                    flag3 = 1;
                }
                else if (item.PolId == 0) {
                    flag11 = 1;
                }
                else if (item.Ct_Status == '') {
                    flag4 = 1;
                }
                else if (item.Ct_ODC == '') {
                    flag5 = 1;
                }
                else if (item.ImporterName == '') {
                    flag6 = 1;
                }
                else if (item.PayerName == '') {
                    flag7 = 1;
                }                
                else if (item.CT_Size == '') {
                    flag8 = 1;
                }
                else if (Number(item.Gross_Wt)<=0) {
                    flag9 = 1;
                }
                else if (item.Container_No == '') {
                    flag10 = 1;
                }
            });
            if (flag1 == 1) {
                alert('Please Select All Shipping Line Name');
                return false;
            }
            if (flag6 == 1) {
                alert('Please Select CHA/Importer Name');
                return false;
            }
            if (flag7 == 1) {
                alert('Please Select Payer Name');
                return false;
            }
            
            if (flag3 == 1) {
                alert('Please Select Transport From');
                return false;
            }
            if (flag2 == 1) {
                alert('Please Select All CargoType');
                return false;
            }
            if (flag4 == 1) {
                alert('Please Select Container Load Type');
                return false;
            }

            if (flag5 == 1) {
                alert('Please Select Container ODC Type');
                return false;
            }
            if (flag10 == 1) {
                alert('Please Enter Container No');
                return false;
            }
            
            if (flag8 == 1) {
                alert('Please Select Container Size');
                return false;
            }

            if (flag9 == 1) {
                alert('Gross Weight must be greater than 0');
                return false;
            }
            if (flag11 == 1) {
                alert('Please Select POL ');
                return false;
            }

            debugger;

            debugger;
            var InvoiceType = 'Tax';
            if ($('#InvoiceType').val() != '') {
                InvoiceType = $('#InvoiceType').val();
            }

            DSRJobOrderService.PrintInvoiceService(InvoiceType, JSON.stringify($scope.JODetails),SEZ).then(function (res) {

                debugger;
                document.getElementById("DivFormOneDet").style.display = 'none';
                document.getElementById("DivFormOneDett").style.display = 'block';
                $scope.JODetails.length = 0;
                $scope.JODetails = res.data.lstInv;
                $scope.ConDetails.length = 0;
                $scope.ConDetails = res.data.lstCont;
                $scope.JOViewDetails.length = 0;
                $scope.JOViewDetails = res.data.lstContView;

                document.getElementById('ModalShippingSearch').style.visibility = "hidden";
                document.getElementById('ModalImporterSearch').style.visibility = "hidden";
                document.getElementById('ModalCHASearch').style.visibility = "hidden";
                document.getElementById('ModalPartySearch').style.visibility = "hidden";
                document.getElementById('ModalPayerSearch').style.visibility = "hidden";
                document.getElementById('ModalPortSearch').style.visibility = "hidden";
                $('#rdWithSummary').prop('disabled', true);
                $('#rdWithOutSummary').prop('disabled', true); 
                $('#Tax').prop('disabled', true);
                $('#Bill').prop('disabled', true);
                $('#Remarks').attr('readonly', 'true');
                $('#JobOrderDate').attr('readonly', true);

                $('#hdnSEZ').val($('#SEZ').val());
                $('#SEZ').prop('disabled', true);
              
                var len = $scope.JODetails.length;
                if (len == 0) {
                    $scope.ActiveControl = false;
                    $scope.ResetImpJODetails();
                    alert('No invoice is generated');

                }
                else {
                    $scope.ActiveControl = true;                    
                    $('#btnSave').attr("disabled", false);
                    $('#btnSave').removeAttr('disabled');
                    $('#Invoice').prop('disabled', true);
                    $('#AddJobOrd').attr("disabled", true);
                    $('#AddJobOrd').prop("disabled", true);
                    $scope.WithShow = $scope.JOViewDetails[0].Showhidden;
                }

            });

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


        $scope.CheckContainerNo = function (CN, index) {
            debugger;
            if (index != 0) {
                //var JODetails = $scope.JODetails.filter(function (finaldata) {
                //    return finaldata.Container_No != CN;
                //})
                var JODetails = $scope.JODetails;
                if (JODetails.length > 0) {
                    for (var i = 0; i < JODetails.length; i++) {

                        if (CN.toLowerCase() == JODetails[i].Container_No.toLowerCase() && i != index) {
                            $scope.JODetails[index].Container_No = '';
                            alert('Duplicate Container No.');
                            return false;
                        }
                    }
                }
            }
            else {
                var JODetails = $scope.JODetails;
            }
            //if (JODetails.length > 0 ) {
            //    for (var i = 0; i < JODetails.length; i++) {

            //        if (CN.toLowerCase() == JODetails[i].Container_No.toLowerCase()) {
            //            $scope.JODetails[index].Container_No = '';
            //            alert('Duplicate Container No.');
            //            return false;
            //        }
            //    }
            //}
        }

        var Obj = {};
        $scope.OnJobOrderSave = function () {
            debugger;

            var x = $('#JobOrderDate').val();
            var reg = /(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d/;
            if (x.match(reg)) {
                //return true;
            }
            else {
                alert("Job Order Date should be dd/mm/yyyy");
                return false;
            }

            if ($('#WithsummaryCheck').val() == '1') {
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
            }
            else {
                $('#TrainNo').val('');
                TrainDate: $('#TrainDate').val(null);
                TrainSummarySerial: $('#TrainSummarySerial').val(0);
                TrainSummaryID: $('#TrainSummaryID').val(0);                
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
            angular.forEach($scope.JODetails, function (item) {
                if (item.ShippingLineName == '') {
                    flag1 = 1;
                }
                else if (item.CargoType == '') {
                    flag2 = 1;
                }
                else if (item.PortId == 0) {
                    flag3 = 1;
                }
                else if (item.Ct_Status == '') {
                    flag4 = 1;
                }
                else if (item.Ct_ODC == '') {
                    flag5 = 1;
                }
                else if (item.ImporterName == '') {
                    flag6 = 1;
                }
                else if (item.PayerName == '') {
                    flag7 = 1;
                }
                else if (item.CT_Size == '') {
                    flag8 = 1;
                }
                else if (Number(item.Gross_Wt) <= 0) {
                    flag9 = 1;
                }
                else if (item.Container_No == '') {
                    flag10 = 1;
                }
            });
            if (flag1 == 1) {
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

            if (flag5 == 1) {
                alert('Please Select Container ODC Type');
                return false;
            }
            if (flag6 == 1) {
                alert('Please Select CHA/Importer Name');
                return false;
            }
            if (flag7 == 1) {
                alert('Please Select Payer Name');
                return false;
            }
            if (flag10 == 1) {
                alert('Please Enter Container No');
                return false;
            }
            if (flag8 == 1) {
                alert('Please Select Container Size');
                return false;
            }
            if (flag9 == 1) {
                alert('Gross Weight must be greater than 0');
                return false;
            }
            debugger;
            if (confirm('Are you sure to save Job Order?')) {
                $('#Invoice').attr("disabled", true);
                $('#btnSave').attr("disabled", true);
                $('#Invoice').prop('disabled', true);
                $('#btnSave').prop('disabled', true);
                debugger;
                var InvoiceType = 'Tax';
                if ($('#InvoiceType').val() != '') {
                    InvoiceType = $('#InvoiceType').val();
                }
                Obj = {
                    ImpJobOrderId: $('#ImpJobOrderId').val()== undefined ? 0 : $('#ImpJobOrderId').val(),
                    PickUpLocation: $('#PickUpLocation').val(),
                    JobOrderNo: $('#JobOrderNo').val(),
                    JobOrderDate: $('#JobOrderDate').val(),
                    JobOrderFor: $('#rbLC').val(),
                    TransportBy: $('#Rail').val(),
                    TrainSummaryID: $('#TrainSummaryID').val(),
                    FormOneDetailId: $('#FormOneDetailId').val(),
                    TrainNo: $('#TrainNo').val(),
                    TrainDate: $('#TrainDate').val(),
                    TrainSummarySerial: $('#TrainSummarySerial').val(),
                    
                    ShippingLineId: $('#ShippingLineId').val(),
                    ShippingLineName: $('#ShippingLineName').val(),
                    ImporterId: $('#ImporterId').val(),
                    ImporterName: $('#ImporterName').val(),
                    CHAId: $('#CHAId').val(),
                    CHAName: $('#CHAName').val(),
                    PartyId: $('#PartyId').val(),
                    PartyName: $('#PartyName').val(),
                    PayerId: $('#PayerId').val(),
                    PayerName: $('#PayerName').val(),
                    PortId: $('#PortId').val(),
                    PolId: $('#PolId').val(),
                    PortName: $('#PortName').val(),
                    Remarks: $('#Remarks').val(),
                }
                var SEZ = $('#hdnSEZ').val();

                DSRJobOrderService.OnJobOrderSave(Obj, InvoiceType, JSON.stringify($scope.JODetails), JSON.stringify($scope.ConDetails),SEZ).then(function (res) {
                    debugger;
                    $('#btnPrint').removeAttr('disabled');
                    $('#btnPrint').attr("disabled", false);
                    $('#btnPrint').prop('disabled', false);
                    $('#BtnGenerateIRN').prop('disabled', false);
                    $('#ImpJobOrderId').val(res.data.Data);
                    alert(res.data.Message);
                    //  setTimeout(function () {
                    //     $('#DivBody').load('/Import/DSR_CWCImport/CreateJobOrder');
                    // }, 5000)
                });
            }
        }

    
    });
})();