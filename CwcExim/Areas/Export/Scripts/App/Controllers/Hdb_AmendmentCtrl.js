(function () {
    angular.module('CWCApp').
    controller('Hdb_AmendmentCtrl', function ($scope, Hdb_AmendmentService) {
        $scope.InvoiceNo = '';
        $scope.InvoiceId = 0;
        $scope.InvoiceDate = '';
        $scope.tabInvoiceNo = '';
        $scope.tabInvoiceId = 0;
        $scope.tabInvoiceDate = '';
        $scope.tabtxtShipBillNo = '';
        $scope.tabtxtSBType = "0";
        $scope.SBType = [{
            id: '1',
            name: 'Baggage'
        }, {
            id: '2',
            name: 'Duty Free Goods'
        }, {
            id: '3',
            name: 'Cargo in Drawback'
        }];
        debugger;
        //if ($('#hdnListOfSBNoAmendment').val() == '') {
        //    $('#hdnListOfSBNoAmendment').val(null);
        //}
        //else {
        //    $scope.AllSBIList = JSON.parse($('#hdnListOfSBNoAmendment').val());
        //}
        if ($('#hdnListOfExporterForAmendment').val() != '') {
            $scope.AllParty = JSON.parse($('#hdnListOfExporterForAmendment').val());
        }
        else {
            $scope.AllParty = [];
        }

        if ($('#hdnListOShippingForAmendment').val() != '') {
            debugger;
            $scope.AllShippingLine = JSON.parse($('#hdnListOShippingForAmendment').val());
        }
        else {
            $scope.AllShippingLine = [];
        }



        if ($('#hdnListOPortLoadingForAmendment').val() != '') {
            debugger;
            $scope.AllPortLoading = JSON.parse($('#hdnListOPortLoadingForAmendment').val());
        }
        else {
            $scope.AllPortLoading = [];
        }



        if ($('#hdnListGodownListForAmendment').val() != '') {
            debugger;
            $scope.AllGodownName = JSON.parse($('#hdnListGodownListForAmendment').val());
        }
        else {
            $scope.AllGodownName = [];
        }

        if ($('#hdnListPackUQCListForAmendment').val() != '') {
            debugger;
            $scope.AllPackUQC = JSON.parse($('#hdnListPackUQCListForAmendment').val());
        }
        else {
            $scope.AllPackUQC = [];
        }



        debugger;
        if ($('#hdnListOfCommodityForAmendment').val() != '') {
            $scope.ListOfCommodityForAmendment = JSON.parse($('#hdnListOfCommodityForAmendment').val());
        }
        else {
            $scope.ListOfCommodityForAmendment = [];
        }

        if ($('#hdnListOfCommodityForAmendment').val() != '') {
            $scope.tabListOfCommodityForAmendment = JSON.parse($('#hdnListOfCommodityForAmendment').val());
        }
        else {
            $scope.tabListOfCommodityForAmendment = [];
        }
        if ($('#hdnListInvoice').val() != '') {
            $scope.lstInvoice = JSON.parse($('#hdnListInvoice').val());
        }
        else {
            $scope.lstInvoice = [];
        }

        Date.prototype.toShortFormat = function () {


            var day = this.getDate();
            var month = this.getMonth() + 1;
            var year = this.getFullYear();

            return "" + day + "/" + month + "/" + year;
        }


        $scope.FlagMerger = 'Merger';

        $scope.OnClickSpace = function (flag) {
            $scope.FlagMerger = flag;
        }

        $scope.AmendDate = new Date().toShortFormat();



        $scope.GetAllSbNoListForAmendmentSearch = function () {
           
            Hdb_AmendmentService.GetAmendLstSearch($scope.searchSBNo).then(function (res) {
                debugger;
               // if (res.data.Status == 1) {

                    $scope.AllSBIList = res.data;
                    $('#SBNoModalTab').modal('show');
               // }



            });
        }


        $scope.GetAllSbNoListForAmendment = function () {

            Hdb_AmendmentService.GetAllSbNoListForAmendment().then(function (res) {
                debugger;
                // if (res.data.Status == 1) {

                $scope.AllSBIList = res.data;
                $('#SBNoModalTab').modal('show');
                // }



            });
        }

        var page = 0;
        $scope.commflag = true;
        $scope.searchCommodity = function () {
            $scope.partyCode = $scope.CommodityBox;
            Hdb_AmendmentService.loadMoreCommodity($scope.partyCode, 0).then(function (res) {
                debugger;
                if (res.data.Status == 1) {

                    $scope.ListOfCommodityForAmendment = res.data.Data;

                }



            });
        }
        $scope.CommodityBox = '';
        $scope.LoadMoreCommodity = function () {
            debugger;

            page = page + 1;

            $scope.partyCode = $scope.CommodityBox;

            Hdb_AmendmentService.loadMoreCommodity($scope.partyCode, page).then(function (res) {
                debugger;
                if (res.data.Status == 1) {
                    if ($scope.ListOfCommodityForAmendment.LstCommodity.length > 0) {
                        $.merge($scope.ListOfCommodityForAmendment.LstCommodity, res.data.Data.LstCommodity);
                    }
                    else {
                        $scope.ListOfCommodityForAmendment = res.data.Data;
                    }
                }



            });


        }



        var pagetab = 0;
        $scope.tabPageFlag = 0;
        $scope.commflagtab = false;
        $scope.tabCommodityBox = '';
        //$scope.searchCommoditytab = function () {
        //    $scope.commflagtab = true;
        //}


        $scope.searchCommoditytab = function () {
            $scope.partyCode = $scope.tabCommodityBox;
            Hdb_AmendmentService.loadMoreCommodity($scope.partyCode, 0).then(function (res) {
                debugger;
                if (res.data.Status == 1) {
                    $scope.tabListOfCommodityForAmendment = res.data.Data;
                }
            });
        }


        $scope.LoadMoreCommoditytab = function () {
            debugger;

            //pagetab = pagetab + 1;
            $scope.tabPageFlag = $scope.tabPageFlag + 1;
            //$scope.partyCode = $scope.tabCommodityBox;

            Hdb_AmendmentService.loadMoreCommodity($scope.tabCommodityBox, $scope.tabPageFlag).then(function (res) {
                debugger;
                $.merge($scope.tabListOfCommodityForAmendment.LstCommodity, res.data.Data.LstCommodity);
                // $scope.tabListOfCommodityForAmendment = res.data.Data;
            });


        }

        $scope.CloseCommodity = function () {
            $('#CommodityModal').modal('hide');
        }

        $('#btnSave').removeClass('hidden');
        $('#btnReset').removeClass('hidden');
        $scope.Flag = false;

        $scope.OldData = [];
        $scope.NewData = [];
        $scope.CartingAppId = "";
        $scope.NewPartyName = "";
        $scope.NewPartyId = "";
        $scope.txtNewPartyName = '';
        $scope.txtNewShipBillNo = '';
        $scope.txtNewShipBillDate = '';
        $scope.txtNewShipBillDate = new Date().toShortFormat();
        $scope.AmendmentNO = '';



        $scope.SelectCommodity = function (commodityID, commodityName) {
            debugger;
            $scope.commodityID = commodityID;
            $scope.txtCommodity = commodityName;
            $('#CommodityModal').modal('hide');
        }



        $scope.GetCBDetailsBySbIdDate = function (SbNo, SbDate) {
            $scope.ShipBillDate = "";
            $scope.txtShipBillNo = "";
            $scope.Party = "";
            $scope.Cargo = "";
            $scope.Weight = "";
            $scope.Pkg = "";
            $scope.Area = "";
            $scope.FOB = "";
            $scope.CartingAppId = "";
            $scope.OldCommodityID = 0;
            $scope.ExporterId = 0;
            $scope.CartingAppNo = "";
            $scope.IsApprove = 0;
            $scope.IsCuttion = 0;


            $('#SBNoModal').modal('hide');
            Hdb_AmendmentService.GetSBDetails(SbNo, SbDate).then(function (res) {
                debugger;
                $scope.SBDetails = res.data;
                if ($scope.SBDetails.length > 0) {
                    $scope.ShipBillDate = $scope.SBDetails[0].ShipBillDate;
                    $scope.txtShipBillNo = $scope.SBDetails[0].ShipBillNo;
                    $scope.Party = $scope.SBDetails[0].Exporter;
                    $scope.Cargo = $scope.SBDetails[0].Cargo;
                    $scope.Weight = $scope.SBDetails[0].Weight;
                    $scope.Pkg = $scope.SBDetails[0].Pkg;
                    $scope.Area = $scope.SBDetails[0].Area;
                    $scope.FOB = $scope.SBDetails[0].FOBValue;
                    $scope.CartingAppId = $scope.SBDetails[0].CartingAppId;
                    $scope.OldCommodityID = $scope.SBDetails[0].CommodityId;
                    $scope.OldPartyId = $scope.SBDetails[0].ExporterId;
                    $scope.IsApprove = $scope.SBDetails[0].IsApprove;
                    $scope.IsCuttion = $scope.SBDetails[0].Cutting;
                    $scope.ShortCargo = $scope.SBDetails[0].ShortCargo;

                }
            });

        }






        $scope.PushValueArray = function () {
            debugger;
            if ($scope.CartingAppId != "") {
                debugger;
                if ($scope.ShortCargo == 1) {
                    var conf = confirm("Partial carting already done.Do you want to proceed?");
                    if (conf == false) {
                        return false;
                    }
                }
                if ($scope.OldData.length > 0) {
                    if ($scope.OldData.filter(x=>x.OldShipBillNo == $scope.txtShipBillNo).length > 0) {
                        alert('Shipbill already added');
                        return false;
                    }





                    if ($scope.OldData.filter(x=>x.IsCuttion != $scope.IsCuttion).length > 0) {
                        alert('Shipbill should be same stage');
                        return false;
                    }

                    //if ($scope.OldData[0].IsApprove == $scope.IsApprove && $scope.OldData[0].IsCuttion == $scope.IsCuttion && $scope.txtShipBillNo != $scope.txtShipBillNo) {

                    $scope.OldData.push({
                        'OldShipBillNo': $scope.txtShipBillNo,
                        'OldShipBillDate': $scope.ShipBillDate,
                        'OldParty': $scope.Party,
                        'OldWeight': $scope.Weight,
                        'OldPkg': $scope.Pkg,
                        'OldCargo': $scope.Cargo,
                        'OldArea': $scope.Area,
                        'OldFOB': $scope.FOB,
                        'OldCartingAppId': $scope.CartingAppId,
                        'OldCommodityID': $scope.OldCommodityID,
                        'OldPartyId': $scope.OldPartyId,
                        'IsApprove': $scope.IsApprove,
                        'IsCuttion': $scope.IsCuttion
                    });
                    $scope.ShipBillDate = "";
                    $scope.txtShipBillNo = "";
                    $scope.Party = "";
                    $scope.Cargo = "";
                    $scope.Weight = "";
                    $scope.Pkg = "";
                    $scope.Area = "";
                    $scope.FOB = "";
                    $scope.CartingAppId = "";
                    $scope.OldCommodityID = 0;
                    $scope.OldPartyId = 0;
                    $scope.ShortCargo = 0;
                    $scope.CartingAppNo = "";
                    /*}
                    else {
                        //alert('Shipbill should be same stage');
                        alert('Shipbill already added');
                    }*/
                }
                else {
                    if ($scope.OldData.filter(x=>x.OldShipBillNo == $scope.txtShipBillNo).length > 0) {
                        alert('Shipbill already added');
                        return false;
                    }
                    $scope.OldData.push({
                        'OldShipBillNo': $scope.txtShipBillNo,
                        'OldShipBillDate': $scope.ShipBillDate,
                        'OldParty': $scope.Party,
                        'OldWeight': $scope.Weight,
                        'OldPkg': $scope.Pkg,
                        'OldCargo': $scope.Cargo,
                        'OldArea': $scope.Area,
                        'OldFOB': $scope.FOB,
                        'OldCartingAppId': $scope.CartingAppId,
                        'OldCommodityID': $scope.OldCommodityID,
                        'OldPartyId': $scope.OldPartyId,
                        'IsApprove': $scope.IsApprove,
                        'IsCuttion': $scope.IsCuttion
                    });
                    $scope.ShipBillDate = "";
                    $scope.txtShipBillNo = "";
                    $scope.Party = "";
                    $scope.Cargo = "";
                    $scope.Weight = "";
                    $scope.Pkg = "";
                    $scope.Area = "";
                    $scope.FOB = "";
                    $scope.CartingAppId = "";
                    $scope.OldCommodityID = 0;
                    $scope.OldPartyId = 0;
                    $scope.ShortCargo = 0;
                }
            }
            else {
                alert('Please Select Ship Bill No');
            }

        }
        $scope.DeleteOldShip = function (i) {
            if (confirm('Are you sure to delete?')) {
                $scope.OldData.splice(i, 1);
            }
        }

        $scope.ClickParty = function (id, Name) {
            debugger;

            $scope.txtNewPartyName = Name;
            $scope.NewPartyId = id;
            $('#PartyModal').modal('hide');
        }

        $scope.PushValueArrayNewInfo = function () {
            if ($scope.txtNewPartyName != '' && $scope.txtNewShipBillNo != '' && $scope.txtNewShipBillDate != '' && $scope.txtWeight > 0 && $scope.txtPkg > 0 && $scope.txtFOB > 0 && $scope.txtArea >= 0) {
                if ($scope.NewData.length > 0) {
                    if ($scope.NewData.filter(x=>x.NewInfoSBNo == $scope.txtNewShipBillNo).length > 0) {
                        alert("Shipping bill number already exists");
                        return false;
                    }
                }

                $scope.NewData.push(
             {
                 'NewInfoPartyName': $scope.txtNewPartyName,
                 'NewInfoPartyId': $scope.NewPartyId,
                 'NewInfoSBNo': $scope.txtNewShipBillNo,
                 'NewInfoSBDate': $scope.txtNewShipBillDate,
                 'NewInfoCommodityID': $scope.commodityID,
                 'NewCommodityName': $scope.txtCommodity,
                 'NewInfoWeight': $scope.txtWeight,
                 'NewInfoPkg': $scope.txtPkg,
                 'NewInfoArea': $scope.txtArea,
                 'NewInfoFOB': $scope.txtFOB,
             }
             );
                $scope.txtNewPartyName = '';
                $scope.NewPartyId = '';
                $scope.txtNewShipBillNo = '';
                $scope.txtNewShipBillDate = new Date().toShortFormat();
                $scope.commodityID = '';
                $scope.txtCommodity = '';
                $scope.txtWeight = '';
                $scope.txtPkg = '';
                $scope.txtArea = '';
                $scope.txtFOB = '';

            }
            else {

                alert('Fill Out New Ship Bill Information Properly .')
            }


        }

        $scope.DeleteNewInfoShip = function (i) {
            if (confirm('Are you sure to delete?')) {
                $scope.NewData.splice(i, 1);
            }
        }
        $scope.Saveinfo = '';

        $scope.saveAmendment = function () {
            //if ($scope.InvoiceNo == '') {
            //    alert('Select Invoice No.');
            //    return false;
            //}
            debugger;
            if ($scope.AmendDate == '' || $scope.AmendDate == null || $scope.AmendDate == 'undefined') {
                $('#AmendDate').focus();
                $scope.Flag = true;
            }
            else {
                $scope.Flag = false;
                if ($scope.OldData.length == 0) {
                    alert('Insert at least 1 Old Ship Bill Info...')
                }
                else if ($scope.NewData.length == 0) {
                    alert('Insert at least 1 New Ship Bill Info...')
                }
                else {

                    /// $('#btnReset').addClass('hidden');
                    if (confirm('Are you sure to save?')) {
                        $('#btnSave').attr("disabled", true);
                        Hdb_AmendmentService.saveAmendment($scope.OldData, $scope.NewData, $scope.AmendDate, $scope.AmendmentNO, $scope.FlagMerger).then(function (res) {
                            console.log(res.data);
                            debugger;
                            if (res.data != '') {
                                debugger;


                                $scope.Saveinfo = res.data.Message;
                                if (res.data.Status != 0) {
                                    $scope.AmendmentNO = res.data.Data;
                                    $scope.OldData = [];
                                    $scope.NewData = [];
                                    $scope.CCINId = "";
                                    $scope.NewPartyName = "";
                                    $scope.NewPartyId = "";
                                    $scope.txtNewPartyName = '';
                                    $scope.txtNewShipBillNo = '';
                                    $scope.txtNewShipBillDate = '';
                                    $scope.txtNewShipBillDate = new Date().toShortFormat();
                                    $scope.AmendmentNO = '';
                                    $scope.InvoiceId = 0;
                                    $scope.InvoiceNo = '';
                                    $scope.InvoiceDate = '';
                                    setTimeout(function () { $('#DivBody').load('/Export/Hdb_CWCExport/Amendment'); }, 5000);
                                }

                            }
                        });
                    }
                }

            }

        }



        /*$scope.GetAmendDetails = function (AmendNo) {

            Ppg_AmendmentService.GetAmendDetails(AmendNo).then(function (res) {
                //console.log(res.data);
                $scope.AllAmendData = res.data;
                debugger;


            });
        }*/
        //$scope.GetAmendDetails('');




        $scope.ViewAmendDetails = function (AmendNo) {
            $scope.OldData = [];
            $scope.NewData = [];
            Hdb_AmendmentService.GetAmendDetailsByAmendNo(AmendNo).then(function (res) {
                //console.log(res.data);
                $scope.AllAmendDataByNo = res.data;
                debugger;
                if ($scope.AllAmendDataByNo.length > 0) {

                    $scope.AmendmentNO = $scope.AllAmendDataByNo[0].AmendmentNo;
                    $scope.AmendDate = $scope.AllAmendDataByNo[0].AmendmentDate;
                    for (var i = 0; i < $scope.AllAmendDataByNo.length; i++) {
                        if ($scope.AllAmendDataByNo[i].Type == 'OLD') {
                            $scope.OldData.push({
                                'OldShipBillNo': $scope.AllAmendDataByNo[i].ShipBillNo,
                                'OldShipBillDate': $scope.AllAmendDataByNo[i].ShipBillDate,
                                'OldParty': $scope.AllAmendDataByNo[i].Exporter,
                                'OldWeight': $scope.AllAmendDataByNo[i].Weight,
                                'OldPkg': $scope.AllAmendDataByNo[i].Pkg,
                                'OldCargo': $scope.AllAmendDataByNo[i].Cargo,
                                'OldArea': '',
                                'OldFOB': $scope.AllAmendDataByNo[i].FOBValue,
                                'OldCCINId': '',
                                'OldCommodityID': '',
                                'OldPartyId': ''
                            })
                        }
                        else if ($scope.AllAmendDataByNo[i].Type == 'NEW') {
                            $scope.NewData.push({
                                'NewInfoPartyName': $scope.AllAmendDataByNo[i].Exporter,
                                'NewInfoPartyId': '',
                                'NewInfoSBNo': $scope.AllAmendDataByNo[i].ShipBillNo,
                                'NewInfoSBDate': $scope.AllAmendDataByNo[i].ShipBillDate,
                                'NewInfoCommodityID': '',
                                'NewCommodityName': $scope.AllAmendDataByNo[i].Cargo,
                                'NewInfoWeight': $scope.AllAmendDataByNo[i].Weight,
                                'NewInfoPkg': $scope.AllAmendDataByNo[i].Pkg,
                                'NewInfoFOB': $scope.AllAmendDataByNo[i].FOBValue
                            });
                        }
                    }
                }

            });

            $('#btnSave').attr("disabled", true);
            //$('#btnReset').addClass('hidden');
        }



        /// For Ship Bill Amendment 


        $scope.tabCloseCommodity = function () {
            $('#tabCommodityModal').modal('hide');
        }

        $scope.tabSelectCommodity = function (id, name) {

            $scope.txttabCommodityID = id;
            $scope.txttabCargo = name;
            $('#tabCommodityModal').modal('hide');
        }

        $scope.tabClickParty = function (id, name) {
            $scope.tabtxtExporterName = name;
            $scope.txttabPartyID = id;
            $('#tabPartyModal').modal('hide');
        }

        $scope.tabClickPortOfLoading = function (id, name) {
            $scope.tabtxtPortOfLoading = name;
            $scope.txttabPortLoadingID = id;
            $('#tabPortOfLoadingModal').modal('hide');
        }



        $scope.tabClickPackUQC = function (PackUQCCode, PackUQCDescription) {
            $scope.tabtxtPackUQCCode = PackUQCCode;
            $scope.tabtxtPackUQCDescription = PackUQCDescription;
            $('#tabPackUQCModal').modal('hide');
        }
        $scope.tabClickGodownName = function (id, name) {
            $scope.tabtxtGodownName = name;
            $scope.txttabGodownId = id;
            $('#tabGodownNameModal').modal('hide');
        }
        $scope.tabClickPortOfDest = function (id, name) {
            $scope.tabtxtPortOfDest = name;
            $scope.txttabPortDestID = id;
            $('#tabPortOfDestModal').modal('hide');
        }

        $scope.tabClickShipping = function (id, name) {
            $scope.tabtxtShippingLineName = name;
            $scope.txttabShippingID = id;
            $('#tabShippingModal').modal('hide');
        }
        $scope.tabGetCBDetailsBySbIdDate = function (SbNo, SbDate) {
            $scope.TabSaveinfo = '';
            $('#SBNoModalTab').modal('hide');
            Hdb_AmendmentService.GetSBDetails(SbNo, SbDate).then(function (res) {
                debugger;
                $scope.SBDetails = res.data;
                if ($scope.SBDetails.length > 0) {
                    $scope.tabShipBillDate = $scope.SBDetails[0].ShipBillDate;
                    $('#txttabNewShipbillDate').val($scope.SBDetails[0].ShipBillDate);
                    $('#txttabNewShipbillDate').trigger("click");
                    $scope.txtOldShipbilldate = $scope.SBDetails[0].ShipBillDate;
                    $scope.tabtxtShipBillNo = $scope.SBDetails[0].ShipBillNo;
                    $scope.NewShipBillNo = $scope.SBDetails[0].ShipBillNo;
                    $scope.tabtxtCCINNo = $scope.SBDetails[0].CartingNo;

                    $scope.txttabShippingID = $scope.SBDetails[0].ShippingLineId;
                    $scope.txttabPortLoadingID = $scope.SBDetails[0].PortOfLoadingId;
                    $scope.txttabPortDestID = $scope.SBDetails[0].PortofDestId
                    document.getElementById("NewSBType").selectedIndex = $scope.SBDetails[0].SBType;
                    // $('#NewSBType').val($scope.SBDetails[0].SBType);
                    //    $scope.tabtxtSBType = 1;
                    $scope.tabtxtShippingLineName = $scope.SBDetails[0].ShippingLineName;
                    $scope.tabtxtPortOfLoading = $scope.SBDetails[0].PortOfLoading;
                    $scope.tabtxtPortOfDest = $scope.SBDetails[0].PostOfDest;

                    $scope.tabtxtGodownName = $scope.SBDetails[0].GodownName;
                    $scope.txttabGodownId = $scope.SBDetails[0].GodownId;


                    $scope.tabtxtExporterName = $scope.SBDetails[0].Exporter;
                    $scope.txttabCargo = $scope.SBDetails[0].Cargo;
                    $scope.txttabWeight = parseFloat($scope.SBDetails[0].Weight);
                    $scope.txttabPkg = parseFloat($scope.SBDetails[0].Pkg);
                    $scope.Area = $scope.SBDetails[0].Area;
                    $scope.txttabFOB = parseFloat($scope.SBDetails[0].FOBValue);
                    $scope.txttabCCINID = $scope.SBDetails[0].CartingAppId;
                    $scope.txttabCommodityID = $scope.SBDetails[0].CommodityId;
                    $scope.txttabPartyID = $scope.SBDetails[0].ExporterId;
                    

                    $scope.tabtxtPackUQCCode = $scope.SBDetails[0].PackUQCCode;
                    $scope.tabtxtPackUQCDescription = $scope.SBDetails[0].PackUQCDescription;
                }
            });

        }

        // $scope.vmData = [];

        $scope.tabDateFlag = false;
        $scope.tabExporterFlag = false;
        $scope.tabCargoFlag = false;
        $scope.tabWeightFlag = false;
        $scope.tabPkgFlag = false;
        $scope.tabNewShipBillNoFlag = false;
        $scope.tabShipbillDateFlag = false;
        $scope.tabtabFOBFlag = false;
        $scope.TabSaveinfo = '';



        $scope.ResetAlltab = function () {
            $scope.tabDateFlag = false;
            $scope.tabExporterFlag = false;
            $scope.tabCargoFlag = false;
            $scope.tabWeightFlag = false;
            $scope.tabPkgFlag = false;
            $scope.tabNewShipBillNoFlag = false;
            $scope.tabShipbillDateFlag = false;
            $scope.tabtabFOBFlag = false;
            $scope.TabSaveinfo = '';
            $scope.tabGodownNameFlag = false;
            $scope.tabShipBillDate;
            $('#txttabNewShipbillDate').val('');
            $('#txttabNewShipbillDate').trigger("click");
            $scope.txtOldShipbilldate = '';
            $scope.tabtxtSBType = "0";
            $scope.tabtxtShipBillNo = '';
            $scope.NewShipBillNo = '';
            $scope.tabtxtExporterName = '';
            $scope.txttabCargo = '';
            $scope.txttabWeight = 0;
            $scope.txttabPkg = 0;
            $scope.Area = '';
            $scope.txttabFOB = '';
            $scope.txttabCCINID = '';
            $scope.txttabCommodityID = 0;
            $scope.txttabPartyID = 0;

            $('#DivBody').load('/Export/Hdb_CWCExport/Amendment');
        }

        $scope.tabtxtShipbillDate = new Date().toShortFormat();

        $scope.ValidationTab = function () {
            debugger;
            var retunflag = true;
            if ($scope.tabtxtShipBillNo == '') {
                $scope.tabtxtShipBillNoFlag = true;
                retunflag = false;
            }
            else {
                $scope.tabtxtShipBillNoFlag = false;
            }
            if ($scope.tabtxtShipbillDate == '' || $scope.tabtxtShipbillDate == null || $scope.tabtxtShipbillDate == 'undefined') {
                //$('#AmendDate').focus();
                $scope.tabDateFlag = true;
                retunflag = false;
            }
            else {
                $scope.tabDateFlag = false;
                //retunflag = true;
            }

            if ($scope.txttabPartyID == '0' || $scope.txttabPartyID == null || $scope.txttabPartyID == 'undefined') {
                //$('#AmendDate').focus();
                $scope.tabExporterFlag = true;
                retunflag = false;
            }
            else {
                $scope.tabExporterFlag = false;
                //retunflag = true;


            }

            if ($scope.txttabCommodityID == '0' || $scope.txttabCommodityID == null || $scope.txttabCommodityID == 'undefined') {
                //$('#AmendDate').focus();
                $scope.tabCargoFlag = true;
                retunflag = false;
            }
            else {
                $scope.tabCargoFlag = false;
                //retunflag = true;
            }
            if ($scope.txttabWeight == '' || $scope.txttabWeight == null || $scope.txttabWeight == 'undefined' || $scope.txttabWeight < 0) {
                //$('#AmendDate').focus();
                $scope.tabWeightFlag = true;
                retunflag = false;
            }
            else {
                $scope.tabWeightFlag = false;
                //retunflag = true;
            }


            if ($scope.txttabPkg == '' || $scope.txttabPkg == null || $scope.txttabPkg == 'undefined' || $scope.txttabPkg < 0) {
                //$('#AmendDate').focus();
                $scope.tabPkgFlag = true;
                retunflag = false;
            }
            else {
                $scope.tabPkgFlag = false;
                //retunflag = true;
            }


            if ($scope.NewShipBillNo == '' || $scope.NewShipBillNo == null || $scope.NewShipBillNo == 'undefined') {
                //$('#AmendDate').focus();
                $scope.tabNewShipBillNoFlag = true;
                retunflag = false;
            }
            else {
                $scope.tabNewShipBillNoFlag = false;
                //retunflag = true;
            }

            if ($scope.tabShipBillDate == '' || $scope.tabShipBillDate == null || $scope.tabShipBillDate == 'undefined') {
                //$('#AmendDate').focus();
                $scope.tabShipbillDateFlag = true;
                retunflag = false;
            }
            else {
                $scope.tabShipbillDateFlag = false;
                //retunflag = true;
            }


            if ($scope.txttabFOB == '' || $scope.txttabFOB == null || $scope.txttabFOB == 'undefined' || $scope.txttabFOB < 0) {
                //$('#AmendDate').focus();
                $scope.tabtabFOBFlag = true;
                retunflag = false;
            }
            else {

                $scope.tabtabFOBFlag = false;
                //retunflag = true;
            }


            return retunflag;





        }

        $scope.SubmitTabData = function () {

            debugger;
            //if ($scope.tabInvoiceNo == '') {
            //    alert('Select Invoice No.');
            //    return false;
            //}
            //if ($scope.tabtxtShipBillNo == '') {
            //    alert('Select ShipBillNo.');
            //    return false;
            //}
            debugger;
            if ($scope.ValidationTab()) {
                $scope.vmData =
                {
                    'ID': $scope.txttabCCINID,
                    'Date': $scope.tabtxtShipbillDate,
                    'ExporterID': $scope.txttabPartyID,
                    'CargoID': $scope.txttabCommodityID,
                    'Weight': $scope.txttabWeight,
                    'Pkg': $scope.txttabPkg,
                    'ShipBillNo': $scope.tabtxtShipBillNo,
                    'NewShipBillNo': $scope.NewShipBillNo,
                    'ShipbillDate': $scope.tabShipBillDate,
                    'OldShipBillDate': $scope.txtOldShipbilldate,
                    'FOB': $scope.txttabFOB,
                  //  'InvoiceId': $scope.tabInvoiceId,
                //    'InvoiceNo': $scope.tabInvoiceNo,
                  //  'InvoiceDate': $scope.tabInvoiceDate,
                    'SBType': document.getElementById("NewSBType").value,
                    'ShippingLineId': $scope.txttabShippingID,
                    'ShippingLineName': $scope.tabtxtShippingLineName,

                    'PortOfLoadingId': $scope.txttabPortLoadingID,
                    'GodownId': $scope.txttabGodownId,
                    'GodownName': $scope.tabtxtGodownName,
                    'PortOfDestId': $scope.txttabPortDestID,
                    'PackUQCCode': $scope.tabtxtPackUQCCode,
                    'PackUQCDescription': $scope.tabtxtPackUQCDescription,

                };

                if (confirm('Are you sure to save?')) {
                    $('#btnSavee').attr("disabled", true);
                    Hdb_AmendmentService.savetabAmendment($scope.vmData).then(function (res) {
                        //console.log(res.data);
                        debugger;
                        if (res.data.Status == '1') {
                            debugger;
                            $('#btnSavee').attr("disabled", true);
                            $scope.tabDateFlag = false;
                            $scope.tabExporterFlag = false;
                            $scope.tabCargoFlag = false;
                            $scope.tabWeightFlag = false;
                            $scope.tabPkgFlag = false;
                            $scope.tabNewShipBillNoFlag = false;
                            $scope.tabShipbillDateFlag = false;
                            $scope.tabtabFOBFlag = false;
                            $scope.TabSaveinfo = '';

                            $scope.tabShipBillDate;
                            $('#txttabNewShipbillDate').val('');
                            $('#txttabNewShipbillDate').trigger("click");
                            $scope.txtOldShipbilldate = '';
                            $scope.tabtxtShipBillNo = '';
                            $scope.NewShipBillNo = '';
                            $scope.tabtxtExporterName = '';
                            $scope.txttabCargo = '';
                            $scope.txttabWeight = 0;
                            $scope.txttabPkg = 0;
                            $scope.Area = '';
                            $scope.txttabFOB = '';
                            $scope.txttabCCINID = '';
                            $scope.txttabCommodityID = 0;
                            $scope.txttabPartyID = 0;
                            $scope.TabSaveinfo = res.data.Message;
                            $scope.tabInvoiceId = 0;
                            $scope.tabInvoiceNo = '';
                            $scope.tabInvoiceDate = '';
                            // $scope.GetAmendDetails('');

                            //$scope.GetAmendList();
                            setTimeout(function () { $('#DivBody').load('/Export/Hdb_CWCExport/Amendment'); }, 500);
                        }
                        else {
                            $scope.TabSaveinfo = res.data.Message;
                        }
                    });
                }
            }



        }

        $scope.PageFlag = 0;
        $scope.GetAllCommodity = function () {
            debugger;
            Hdb_AmendmentService.GetAllCommodityName('', $scope.PageFlag).then(function (res) {
                debugger;
                if (res.data != '') {
                    $scope.ListOfCommodityForAmendment = res.data.Data;
                }

            });
        }

        // For shipp bill amendment tab
        //$scope.tabPageFlag = 0;
        $scope.tabGetAllCommodity = function () {
            debugger;
            Hdb_AmendmentService.GetAllCommodityName('', $scope.tabPageFlag).then(function (res) {
                debugger;
                if (res.data != '') {
                    $scope.tabListOfCommodityForAmendment = res.data.Data;
                }

            });
        }

        $scope.GetAllCommodity();
        $scope.tabGetAllCommodity();

        $scope.GetAmendList = function () {

            Hdb_AmendmentService.GetAmendLst().then(function (res) {
                debugger;
                $scope.ListAmendLst = res.data;
            });
        }
        //$scope.GetAmendList();
        $scope.SelectInvoice = function (obj) {
            $scope.InvoiceNo = obj.InvoiceNo;
            $scope.InvoiceId = obj.InvoiceId;
            $scope.InvoiceDate = obj.InvoiceDate;
            $('#InvoiceModal').modal('hide');
        }
        $scope.tabSelectInvoice = function (obj) {
            $scope.tabInvoiceNo = obj.InvoiceNo;
            $scope.tabInvoiceId = obj.InvoiceId;
            $scope.tabInvoiceDate = obj.InvoiceDate;
            $('#tabInvoiceModal').modal('hide');
        }



       
       
      

    });
})();