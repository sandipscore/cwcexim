(function () {
    angular.module('CWCApp')
    .controller('BttContCtrl', function ($scope, BttContService) {
        $scope.InvoiceDate = $('#hdndate').val();
        $scope.ContList = [];
        $scope.PayeeList = [];
        $scope.InvoiceId = 0;
        $scope.PartyId = 0;
        $scope.PartyName = '';
        $scope.State = '';
        $scope.StatePayer = '';
        $scope.PayeeId = 0;
        $scope.PayeeName = ''; 
        $scope.GSTNo = '';
        $scope.CasualLabour = 0;
        $scope.Distance = 0;
        $scope.IsCalculated = 0;
        $scope.IsShowHT = 0;
        $scope.TaxType = $('#Tax').is(':checked') ? 'Tax' : 'Bill';
        $scope.CWCCharge = [];
        $scope.HTCharge = [];
        $scope.NewHTList = [];
        $scope.HTList = [];
        $scope.ContainerPopulate = [];
        $scope.Page = 0;
        $scope.PayerPage = 0;
        $scope.PartyCode = '';
        $scope.PayerCode = '';
        $scope.ExportUnder = "";
        /*******************Container No,Payee,Party Bind*****************************/
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ContList = JSON.parse($('#hdnStuffingReq').val());
        }
        if ($('#hdnPartyPayee').val() != null && $('#hdnPartyPayee').val() != '') {
            debugger;
            $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
            $scope.State = $('#hdnPartyState').val();
            console.log($scope.State);
        }
        if ($('#hdnPayee').val() != null && $('#hdnPayee').val() != '') {
            $scope.PayerList = JSON.parse($('#hdnPayee').val());
            $scope.StatePayer = $('#hdnPayerState').val();
        }
        /*******************Container No,Payee,Party Bind*****************************/
        $scope.SelectPayer = function (Payeeobj) {
            $scope.PayeeId = Payeeobj.PartyId;
            $scope.PayeeName = Payeeobj.PartyName;
            $('#PayerModal').modal('hide');
            $scope.LoadPayer();
            var ContList = $scope.ContList.filter(x=>x.Selected == 1);
            if (ContList.length > 0 && $scope.PartyId > 0) {
                $scope.CalculateCharges();
            }
        }
        $scope.SelectParty = function (obj) {
            $scope.GSTNo = obj.GSTNo;
            $scope.PartyId = obj.PartyId;
            $scope.PartyName = obj.PartyName;
            if ($scope.PayeeId == 0) {
                $scope.PayeeId = obj.PartyId;
                $scope.PayeeName = obj.PartyName;
            }
            $('#PartyModal').modal('hide');
            $scope.LoadParty();
            var ContList = $scope.ContList.filter(x=>x.Selected == 1);
            if (ContList.length > 0) {
                $scope.CalculateCharges();
            }
        }
        $scope.CalculateCharges = function () {
            var ContList = $scope.ContList.filter(x=>x.Selected == 1);
            if (ContList.length > 0 && $scope.IsCalculated == 0) {
                if ($scope.PartyId == 0) {
                    alert("Select Party");
                }
                else {
                    $scope.TaxType = $('#Tax').is(':checked') ? 'Tax' : 'Bill';
                    BttContService.CalculateCharges($scope.InvoiceDate, $scope.PartyId, $scope.PayeeId, $scope.TaxType, ContList, $scope.CasualLabour,$scope.Distance, $scope.InvoiceId,$scope.ExportUnder).then(function (data) {
                        $scope.InvoiceObj = data.data;
                        console.log($scope.InvoiceObj);
                        //$scope.ContainerPopulate = data.data.lstConatiner;
                        $.each(data.data.lstConatiner, function (i, item) {

                            if ($scope.ContainerPopulate.filter(x=>x.CFSCode == item.CFSCode).length == 0)
                                $scope.ContainerPopulate.push(item);
                        });

                        $scope.CWCCharge = $scope.InvoiceObj.lstContCharges.filter(x=>x.ChargeType == "CWC");
                        var source = $scope.InvoiceObj.lstContCharges.filter(x=>x.ChargeType == "HT");
                        var source1 = $scope.InvoiceObj.lstContHTCharges;
                        $.each(source, function (i, item) {
                            $scope.HTList.push(item);
                        });
                        $.each(source1, function (i, item) {
                            $scope.NewHTList.push(item);
                        });
                        $scope.IsCalculated = 1;
                        $('#InvoiceDate').parent().find('img').css('display', 'none');
                        $scope.InvoiceObj.PayeeId = $scope.PayeeId;
                        $scope.InvoiceObj.PayeeName = $scope.PayeeName;
                    });
                }
            }
            $('#ContainerModal').modal('hide');
        }
        $scope.ShowHT = function () {
            if ($scope.IsCalculated == 1)
            {
                $scope.IsShowHT = 1;
                $scope.HTCharge = $scope.HTList.filter(x=>x.Selected == true);
                /*$.each($scope.HTCharge, function (i, item) {
                    $scope.InvoiceObj.HTTotal += item.Total;
                });*/
                $scope.CalcTax();
            }

            
            $('#HnTModal').modal('hide');
        }
        $scope.AddextraHT = function () {
            $.each($scope.NewHTList, function (i, item) {
                if (item.Selected == true)
                    $scope.HTCharge.push(item);
            });
            $scope.CalcTax();
            $('#NewHnTModal').modal('hide');
        }
        $scope.DeleteHT = function (i) {
            $scope.HTCharge.splice(i, 1);
            $scope.CalcTax();
        }
        $scope.CalcTax = function () {
            /***************************Initializing Zero**********************/
            /******************************************************************/
            $scope.InvoiceObj.TotalAmt = 0;
            $scope.InvoiceObj.TotalDiscount = 0;
            $scope.InvoiceObj.TotalTaxable = 0;
            $scope.InvoiceObj.TotalCGST = 0;
            $scope.InvoiceObj.TotalSGST = 0;
            $scope.InvoiceObj.TotalIGST = 0;
            $scope.InvoiceObj.CWCTotal = 0;
            $scope.InvoiceObj.HTTotal = 0;
            $scope.InvoiceObj.CWCTDS = 0;
            $scope.InvoiceObj.HTTDS = 0;
            $scope.InvoiceObj.CWCTDSPer = 0;
            $scope.InvoiceObj.HTTDSPer = 0;
            $scope.InvoiceObj.TDS = 0;
            $scope.InvoiceObj.TDSCol = 0;
            $scope.InvoiceObj.AllTotal = 0;
            $scope.InvoiceObj.InvoiceAmt = 0;
            $scope.InvoiceObj.RoundUp = 0;
            /******************************************************************/
            $.each($scope.CWCCharge, function (i, item) {
                if ($scope.TaxType == 'Tax') {
                    if (item.ChargeName == "Insurance") {
                        if ($scope.InvoiceObj.IsLocalGST == 1) {
                            /*CGST / SGST*/
                            item.CGSTAmt = Math.round(parseFloat(((parseFloat(item.Taxable) * item.CGSTPer) / 100)));
                            item.SGSTAmt = Math.round(parseFloat(((parseFloat(item.Taxable) * item.SGSTPer) / 100)));
                            item.IGSTAmt = 0;
                        }
                        else {
                            /*IGST*/
                            item.CGSTAmt = 0;
                            item.SGSTAmt = 0;
                            item.IGSTAmt = Math.round(parseFloat(((parseFloat(item.Taxable) * item.IGSTPer) / 100)));
                        }
                    }
                    else {
                        if ($scope.InvoiceObj.IsLocalGST == 1) {
                            /*CGST / SGST*/
                            item.CGSTAmt = parseFloat(((parseFloat(item.Taxable) * item.CGSTPer) / 100).toFixedDown(2));
                            item.SGSTAmt = parseFloat(((parseFloat(item.Taxable) * item.SGSTPer) / 100).toFixedDown(2));
                            item.IGSTAmt = 0;
                        }
                        else {
                            /*IGST*/
                            item.CGSTAmt = 0;
                            item.SGSTAmt = 0;
                            item.IGSTAmt = parseFloat(((parseFloat(item.Taxable) * item.IGSTPer) / 100).toFixedDown(2));
                        }
                    }
                }
                else {
                    item.CGSTAmt = 0;
                    item.SGSTAmt = 0;
                    item.IGSTAmt = 0;
                }

                item.Total = parseFloat((parseFloat(item.CGSTAmt) + parseFloat(item.SGSTAmt) + parseFloat(item.IGSTAmt) + parseFloat(item.Taxable)).toFixedDown(2));
                $scope.InvoiceObj.TotalAmt += parseFloat(parseFloat(item.Taxable).toFixedDown(2));
                $scope.InvoiceObj.TotalTaxable += parseFloat(parseFloat(item.Taxable).toFixedDown(2));
                $scope.InvoiceObj.TotalCGST += parseFloat(item.CGSTAmt.toFixedDown(2));
                $scope.InvoiceObj.TotalSGST += parseFloat(item.SGSTAmt.toFixedDown(2));
                $scope.InvoiceObj.TotalIGST += parseFloat(item.IGSTAmt.toFixedDown(2));
                $scope.InvoiceObj.CWCTotal += parseFloat(item.Total.toFixedDown(2));

            });
            $.each($scope.HTCharge, function (i, item) {
                if ($scope.TaxType == 'Tax')
                {
                    if ($scope.InvoiceObj.IsLocalGST == 1) {
                        /*CGST / SGST*/
                        item.CGSTAmt = parseFloat(((item.Taxable * item.CGSTPer) / 100).toFixedDown(2));
                        item.SGSTAmt = parseFloat(((item.Taxable * item.SGSTPer) / 100).toFixedDown(2));
                        item.IGSTAmt = 0;
                    }
                    else {
                        /*IGST*/
                        item.CGSTAmt = 0;
                        item.SGSTAmt = 0;
                        item.IGSTAmt = parseFloat(((item.Taxable * item.IGSTPer) / 100).toFixedDown(2));
                    }
                }
                else
                {
                    item.CGSTAmt = 0;
                    item.SGSTAmt = 0;
                    item.IGSTAmt = 0;
                }
                item.Total = parseFloat((parseFloat(item.CGSTAmt) + parseFloat(item.SGSTAmt) + parseFloat(item.IGSTAmt) + parseFloat(item.Taxable)).toFixedDown(2));
                $scope.InvoiceObj.TotalAmt += parseFloat(parseFloat(item.Taxable).toFixedDown(2));
                $scope.InvoiceObj.TotalTaxable += parseFloat(parseFloat(item.Taxable).toFixedDown(2));
                $scope.InvoiceObj.TotalCGST += parseFloat(item.CGSTAmt.toFixedDown(2));
                $scope.InvoiceObj.TotalSGST += parseFloat(item.SGSTAmt.toFixedDown(2));
                $scope.InvoiceObj.TotalIGST += parseFloat(item.IGSTAmt.toFixedDown(2));
                $scope.InvoiceObj.HTTotal += parseFloat(item.Total.toFixedDown(2));

            });
            $scope.InvoiceObj.CWCTotal = parseFloat(parseFloat($scope.InvoiceObj.CWCTotal).toFixedDown(2));
            $scope.InvoiceObj.HTTotal = parseFloat(parseFloat($scope.InvoiceObj.HTTotal).toFixedDown(2));

            $scope.InvoiceObj.AllTotal = parseFloat((parseFloat($scope.InvoiceObj.CWCTotal.toFixedDown(2)) + parseFloat($scope.InvoiceObj.HTTotal.toFixedDown(2))).toFixedDown(2));
            $scope.InvoiceObj.InvoiceAmt = Math.ceil($scope.InvoiceObj.CWCTotal + $scope.InvoiceObj.HTTotal);
            $scope.InvoiceObj.RoundUp = parseFloat((parseFloat($scope.InvoiceObj.InvoiceAmt.toFixedDown(2)) - parseFloat($scope.InvoiceObj.AllTotal.toFixedDown(2))).toFixedDown(2));
        }
        $scope.AddEditBTTContPS = function () {
            debugger;
            var Conf = confirm("Are you sure you want to Save?");
            if (Conf == true) {
                $scope.InvoiceObj.ExportUnder = $scope.ExportUnder;
                $scope.InvoiceObj.Distance = $scope.Distance;
                $.each($scope.CWCCharge, function (i, item) {
                    $scope.InvoiceObj.lstPostContCharges.push(item);
                });
                $.each($scope.HTCharge, function (i, item) {
                    $scope.InvoiceObj.lstPostContCharges.push(item);
                });
                BttContService.AddEditBTTContPS($scope.InvoiceObj).then(function (data) {
                    if (data.data.Status == 1) {
                        $scope.Message = data.data.Message;
                        var InvSupplyData = data.data.Data.split('-');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        //$scope.InvoiceNo = data.data.Data;
                        $('#InvoiceNo').val($scope.InvoiceNo);
                        $('#btnGeneratedIRN').removeAttr('disabled');
                        
                        $('#hdnSupplyType').val($scope.SupplyType);
                    }
                    else {
                        $scope.Message = data.data.Message;
                    }
                });
            }
        }
        Number.prototype.toFixedDown = function (digits) {
            var re = new RegExp("(\\d+\\.\\d{" + digits + "})(\\d)"),
                m = this.toString().match(re);
            return m ? parseFloat(m[1]) : this.valueOf();
        };

        $scope.LoadParty = function () {
            $scope.Page = 0;
            BttContService.LoadPartyList($scope.Page).then(function (data) {               
                if (data.data.Status == 1) {
                    $scope.PartyList = data.data.Data.lstParty;
                    $scope.State = data.data.Data.State;
                }
                else {
                    //$scope.Message = data.data.Message;
                }
            });  
        }
            
        $scope.LoadMoreParty = function () {
            $scope.Page = $scope.Page + 1;
            BttContService.LoadPartyList($scope.Page).then(function (data) {               
                if (data.data.Status == 1) {
                    $.each(data.data.Data.lstParty, function (item, elem) {
                        $scope.PartyList.push(elem);
                    });
                    $scope.State = data.data.Data.State;
                }
                else {
                   // $scope.Message = data.data.Message;
                }
            });
        }
                  
        $scope.SearchPartyByPartyCode = function () {
            debugger;
            $scope.Page = 0;
            BttContService.SearchPartyNameByPartyCode($scope.PartyCode).then(function (data) {
                if (data.data.Status == 1) {
                    $scope.PartyList = data.data.Data.lstParty;                    
                    $scope.State = data.data.Data.State;
                }
                else {
                    $scope.Message = data.data.Message;
                }
            });          
        }

        $scope.LoadPayer = function () {
            $scope.PayerPage = 0;
            BttContService.LoadPayerList($scope.PayerPage).then(function (data) {
                console.log(data);
                if (data.data.Status == 1) {
                    $scope.PayerList = data.data.Data.lstPayer;
                    $scope.StatePayer = data.data.Data.StatePayer;
                }
                else {
                    //$scope.Message = data.data.Message;
                }
            });
        }

        $scope.LoadMorePayer = function () {
            $scope.PayerPage = $scope.PayerPage + 1;
            BttContService.LoadPayerList($scope.PayerPage).then(function (data) {
                debugger;
                console.log(data);
                if (data.data.Status == 1) {
                    $.each(data.data.Data.lstPayer, function (item, elem) {
                        $scope.PayerList.push(elem);
                    });
                    $scope.StatePayer = data.data.Data.StatePayer;
                }
                else {
                    // $scope.Message = data.data.Message;
                }
            });
        }

        $scope.SearchPayerByPayerCode = function () {
            debugger;
            $scope.PayerPage = 0;
            BttContService.SearchPayerByPayerCode($scope.PayerCode).then(function (data) {
                if (data.data.Status == 1) {
                    $scope.PayerList = data.data.Data.lstPayer;
                    $scope.StatePayer = data.data.Data.StatePayer;
                }
                else {
                    $scope.Message = data.data.Message;
                }              
            });
        }
        $scope.CloseParty = function () {
            debugger;
            $('#PartyModal').modal('hide');
            $scope.LoadParty();
        }

        $scope.ClosePayer = function () {
            debugger;
            $scope.LoadPayer();
            $('#PayerModal').modal('hide');            
        }

    });
    })();