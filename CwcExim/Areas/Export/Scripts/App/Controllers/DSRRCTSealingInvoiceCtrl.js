(function () {
    angular.module('CWCApp').
    controller('DSRRCTSealingInvoiceCtrl', function ($scope, DSRRCTSealingInvoiceService) {
    
        $scope.InvoiceNo = "";
        $scope.ConatinerDetails = [];
        $scope.ShipbillDetails = [];
        $scope.Message = '';
        
        //$scope.NoOfVehicles = 0;
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.ExportUnder = "";

       
        if ($('#hdnPartyPayee').val() != null && $('#hdnPartyPayee').val() != '') {
            $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
            $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());
        }
       
        $scope.Rights = JSON.parse($("#hdnRights").val());

        // console.log( $scope.PartyList);

        $scope.SelectParty = function (obj) {
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;
            $('#PayeeName').focus();
            $('#PartyModal').modal('hide');
           
        };

        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            $scope.PayeeName = obj.PartyName;
            $('#CHAName').focus();
            $('#PayeeModal').modal('hide');
           
        };

        $scope.SelectCHA = function (obj) {
            $scope.CHAId = obj.PartyId;
            $scope.CHAName = obj.PartyName;
            $('#ExporterName').focus();
            $('#CHAModal').modal('hide');
           
        };

        $scope.SelectExporter = function (obj) {
            $scope.ExporterId = obj.PartyId;
            $scope.ExporterName = obj.PartyName;
            $('#ShippingLineName').focus();
            $('#ExporterModal').modal('hide');
            
        };
        $scope.SelectShippingLine = function (obj) {
            $scope.ShippingLineId = obj.PartyId;
            $scope.ShippingLineName = obj.PartyName;
            $('#ContainerNo').focus();
            $('#ShippingLineModal').modal('hide');

        };

        $scope.tabindex = 12;
        $scope.tabindex = 52;
        $scope.tabindex = 92;

        

        $scope.InvoiceObj = {};       
        $scope.IsContSelected = false;
        $scope.CreateRCTSealingInvoice = function () {
            debugger;
            if ($scope.PartyId == 0 || $scope.PartyId == '' || $scope.PartyId == null) {
                //$scope.Message = "Select Party";
                alert('Select Party');
                return false;
            }

            if ($scope.PayeeId == 0 || $scope.PayeeId == '' || $scope.PayeeId == null) {
                //$scope.Message = "Select Payee";
                alert('Select Payee');
                return false;
            }

            if ($scope.CHAId == 0 || $scope.CHAId == '' || $scope.CHAId == null) {
                //$scope.Message = "Select Payee";
                alert('Select CHA');
                return false;
            }

            if ($scope.ExporterId == 0 || $scope.ExporterId == '' || $scope.ExporterId == null) {
                //$scope.Message = "Select Payee";
                alert('Select Importer');
                return false;
            }
            if ($scope.ShippingLineId == 0 || $scope.ShippingLineId == '' || $scope.ShippingLineId == null) {
                //$scope.Message = "Select Payee";
                alert('Select Shiping Line');
                return false;
            }

            debugger;
            $('#InvoiceDate').parent().find('img').css('display', 'none');
            var c = 0;

            if ($('#ContainerDetailsJson').val() != "")
            {
                $scope.ConatinerDetails = JSON.parse($('#ContainerDetailsJson').val());
            }
            else {
                $scope.ConatinerDetails = null;
            }
            if ($('#ShipbillDetailsJson').val() != "")
            {
                $scope.ShipbillDetails = JSON.parse($('#ShipbillDetailsJson').val());
            }
            else {
                $scope.ShipbillDetails = null;
            }
            
            $scope.ExportUnder = $('#ExportUnder').val();

            
            //for (i = 0; i < $scope.conatiners.length; i++) {

            //    if ($scope.conatiners[i].Selected == true) {
            //        c = c + 1;
            //    }
            //}

            //if (c > 0) {
            DSRRCTSealingInvoiceService.GetRCTSealingInvoice(0, $('#InvoiceDate').val(), TaxType, $scope.ConatinerDetails, $scope.ShipbillDetails, $scope.PartyId, $scope.PayeeId, $scope.ExportUnder).then(function (res) {

                    $scope.InvoiceObj = res.data;
                    debugger;
                    /*********CWC Charge Distinction***************/                    
                    $('#btnGenerateInvoice').attr('disabled', true);
                    $('#btnAddShipbillDtl').attr('disabled', true);
                    $('#btnAddContainerDtl').attr('disabled', true);                    
                    $('#PaymentSheetModelJson').val(JSON.stringify(res.data));                   
                    var CWCCharge = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                        return item.ChargeType == "CWC";                       
                    });

                    html = '';
                    var TotalCWC = 0;
                    $.each(CWCCharge, function (i, item) {
                        TotalCWC += item.Total;
                        //html += '<tr><td>' + item.Clause + '. ' + item.ChargeName + '</td>'
                        html += '<tr><td>' + item.ChargeName + '</td>'
                                + '<td><input class="text-right" id="' + item.ChargeId + '" type="text" value="' + item.Amount.toFixed(2) + '" readonly /></td>'
                                + '<td><input class="text-right" id="IGSTP' + item.ChargeId + '" type="text" value="' + item.IGSTPer + '" readonly /></td>'
                                + '<td><input class="text-right" id="IGSTA' + item.ChargeId + '" type="text" value="' + item.IGSTAmt.toFixed(2) + '" readonly /></td>'
                                + '<td><input class="text-right" id="CGSTP' + item.ChargeId + '" type="text" value="' + item.CGSTPer + '" readonly /></td>'
                                + '<td><input class="text-right" id="CGSTA' + item.ChargeId + '" type="text" value="' + item.CGSTAmt.toFixed(2) + '" readonly /></td>'
                                + '<td><input class="text-right" id="SGSTP' + item.ChargeId + '" type="text" value="' + item.SGSTPer + '" readonly /></td>'
                                + '<td><input class="text-right" id="SGSTA' + item.ChargeId + '" type="text" value="' + item.SGSTAmt.toFixed(2) + '" readonly /></td>'
                                + '<td><input class="text-right" id="TOT' + item.ChargeId + '" type="text" value="' + item.Total.toFixed(2) + '" readonly /></td></tr>';
                    });
                    $('#tblCWCCharges tbody').html(html);
                    $('#TOTCWCChrg').val((TotalCWC).toFixed(2));

                    var AllTot = Math.ceil(TotalCWC);
                    var roundup = AllTot - TotalCWC;

                    $('#RoundUp').val(roundup.toFixed(2));
                    $('#AllTotal').val(TotalCWC.toFixed(2));
                    $('#InvoiceValue').val(AllTot.toFixed(2));
                    $scope.InvoiceAmt = AllTot;
                    $scope.RoundUp = roundup;
                    $scope.TotalAmt = AllTot;

                    /*************************************************************/
                    $scope.IsContSelected = true;
                    //console.log($scope.InvoiceObj);
                    if ($scope.Rights.CanAdd == 1) {
                        $('#btnSave').removeAttr("disabled");
                    }
                    $('#ExportUnder').prop('disabled', true);
                    $('.search').css('display', 'none');
                    $('#InvoiceDate').parent().find('img').css('display', 'none');
                    $('.edit').css('pointer-events', 'none');

                });
            //}
           
        }

        $scope.SubmitInvoice = function () {

            debugger;
            if ($scope.PartyId == 0 || $scope.PartyId == '' || $scope.PartyId == null) {
                $scope.Message = "Select Party";
                return false;
            }
           
            $scope.ExportUnder = $('#ExportUnder').val();

            var c = 0;
            //for (i = 0; i < $scope.conatiners.length; i++) {

            //    if ($scope.conatiners[i].Selected == true) {
            //        c = c + 1;
            //    }
            //}

            //if (c <= 0) {
            //    $scope.Message = "Select Atleast one container";
            //    return false;
            //}
            if ($scope.InvoiceObj.TotalAmt <= 0) {
                $scope.Message = "Can not be saved. Invoice Amount is Zero or Negative.";
                return false;
            }

            if (confirm('Are you sure to Save this Invoice?')) {
                $('#btnSave').attr('disabled', true);
                                               
                $scope.InvoiceObj.InvoiceId = 0;
                $scope.InvoiceObj.InvoiceType = TaxType;
                $scope.InvoiceObj.PartyId = $scope.PartyId;
                $scope.InvoiceObj.PartyName = $scope.PartyName;
                $scope.InvoiceObj.PartyAddress = $scope.hdnAddress;
                $scope.InvoiceObj.PartyGST = $scope.GSTNo;
                $scope.InvoiceObj.PartyState = $scope.hdnState;
                $scope.InvoiceObj.PartyStateCode = $scope.hdnStateCode;
                $scope.InvoiceObj.PayeeId = $scope.PayeeId;
                $scope.InvoiceObj.PayeeName = $scope.PayeeName;
                $scope.InvoiceObj.CHAId = $scope.CHAId;
                $scope.InvoiceObj.CHAName = $scope.CHAName;
                $scope.InvoiceObj.ExporterId = $scope.ExporterId;
                $scope.InvoiceObj.ExporterName = $scope.ExporterName;
                $scope.InvoiceObj.ShippingLineId = $scope.ShippingLineId;
                $scope.InvoiceObj.ShippingLineName = $scope.ShippingLineName;
                $scope.InvoiceObj.Remarks = $scope.Remarks;
                $scope.InvoiceObj.ExportUnder = $scope.ExportUnder;

                DSRRCTSealingInvoiceService.GenerateRCTInvoice($scope.InvoiceObj).then(function (res) {
                    debugger;
                    console.log(res);
                    
                    var InvSupplyData = res.data.Data.InvoiceNo.split('-');
                    $scope.InvoiceNo = InvSupplyData[0];
                    $scope.SupplyType = InvSupplyData[1];

                    //$scope.InvoiceNo = res.data.Data.InvoiceNo;
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    if (res.data.Status == 0) {
                        $('#btnPrint').attr("disabled");
                    }
                    else {
                        $('#btnPrint').removeAttr("disabled");
                        $('#btnGeneratedIRN').removeAttr("disabled");
                    }
                });
            }
        }
        

        $scope.GetParty = function () {
            DSRRCTSealingInvoiceService.GetPartyForRCTSealing().then(function (res) {
                debugger;              
                $('#hdnPartyPayee').val(res.data);
                $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());               
            });
        };

        $scope.GetPayee = function () {
            DSRRCTSealingInvoiceService.GetPartyForRCTSealing().then(function (res) {
                debugger;
                $('#hdnPartyPayee').val(res.data);
                $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());
            });
        };

        $scope.GetCHA = function () {
            debugger;           
            DSRRCTSealingInvoiceService.GetCHAForRCTSealing().then(function (res) {
                debugger;
                $('#hdnCHA').val(res.data);
                $scope.CHAList = JSON.parse($('#hdnCHA').val());
            });
        };

        $scope.GetExporter = function () {           
            DSRRCTSealingInvoiceService.GetExporterForRCTSealing().then(function (res) {
                debugger;
                $('#hdnExporter').val(res.data);
                $scope.ExporterList = JSON.parse($('#hdnExporter').val());
            });
        };

        $scope.GetShippingLine = function () {            
            DSRRCTSealingInvoiceService.GetShippingLineForRCTSealing().then(function (res) {
                debugger;
                $('#hdnShippingLine').val(res.data);
                $scope.ShippingLineList = JSON.parse($('#hdnShippingLine').val());
            });
        };

        $scope.GenerateIRN = function () {


            DSRRCTSealingInvoiceService.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {

                alert(res.data.Message);

            });

        };
    });
})();