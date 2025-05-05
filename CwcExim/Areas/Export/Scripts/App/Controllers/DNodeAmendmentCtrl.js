(function () {
    angular.module('CWCApp').
    controller('CHNAmendmentCtrl', function ($scope, CHNAmendmentService) {
        debugger;
        if ($('#hdnListOfSBNoAmendment').val() == '') {
            $('#hdnListOfSBNoAmendment').val(null);
        }
        else {
            $scope.AllSBIList = JSON.parse($('#hdnListOfSBNoAmendment').val());
        }
        if ($('#hdnListOfExporterForAmendment').val() != '') {
            $scope.AllParty = JSON.parse($('#hdnListOfExporterForAmendment').val());
        }
        else {
            $scope.AllParty = [];
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

        Date.prototype.toShortFormat = function () {


            var day = this.getDate();
            var month = this.getMonth() + 1;
            var year = this.getFullYear();

            return "" + day + "/" + month + "/" + year;
        }




        $scope.AmendDate = new Date().toShortFormat();

        var page = 0;
        $scope.commflag = false;
        $scope.searchCommodity = function () {
            $scope.commflag = true;
        }

        $scope.LoadMoreCommodity = function () {
            debugger;

            page = page + 1;

            $scope.partyCode = $scope.CommodityBox;

            CHNAmendmentService.loadMoreCommodity($scope.partyCode, page).then(function (res) {
                debugger;

                $scope.ListOfCommodityForAmendment = res.data.Data;
            });


        }



        var pagetab = 0;
        $scope.commflagtab = false;
        $scope.searchCommoditytab = function () {
            $scope.commflagtab = true;
        }

        $scope.LoadMoreCommoditytab = function () {
            debugger;

            pagetab = pagetab + 1;

            $scope.partyCode = $scope.tabCommodityBox;

            CHNAmendmentService.loadMoreCommodity($scope.partyCode, pagetab).then(function (res) {
                debugger;
                $scope.tabListOfCommodityForAmendment = res.data.Data;
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
        $scope.CCINId = "";
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
            $scope.CCINId = "";
            $scope.OldCommodityID = 0;
            $scope.ExporterId = 0;

            $scope.IsApprove = 0;
            $scope.IsCuttion = 0;


            $('#SBNoModal').modal('hide');
            CHNAmendmentService.GetSBDetails(SbNo, SbDate).then(function (res) {
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
                    $scope.CCINId = $scope.SBDetails[0].CCINId;
                    $scope.OldCommodityID = $scope.SBDetails[0].CommodityId;
                    $scope.OldPartyId = $scope.SBDetails[0].ExporterId;
                    $scope.IsApprove = $scope.SBDetails[0].IsApprove;
                    $scope.IsCuttion = $scope.SBDetails[0].Cutting;
                   
                }
            });

        }






        $scope.PushValueArray = function () {
            if ($scope.CCINId != "") {
                debugger;

                if ($scope.OldData.length > 0) {


                    if ($scope.OldData[0].IsApprove == $scope.IsApprove && $scope.OldData[0].IsCuttion == $scope.IsCuttion && $scope.txtShipBillNo != $scope.txtShipBillNo) {

                        $scope.OldData.push({
                            'OldShipBillNo': $scope.txtShipBillNo,
                            'OldShipBillDate': $scope.ShipBillDate,
                            'OldParty': $scope.Party,
                            'OldWeight': $scope.Weight,
                            'OldPkg': $scope.Pkg,
                            'OldCargo': $scope.Cargo,
                            'OldArea': $scope.Area,
                            'OldFOB': $scope.FOB,
                            'OldCCINId': $scope.CCINId,
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
                        $scope.CCINId = "";
                        $scope.OldCommodityID = 0;
                        $scope.OldPartyId = 0;
                    }
                    else {
                        //alert('Shipbill should be same stage');
                        alert('Shipbill already added');
                    }
                }
                else {
                    $scope.OldData.push({
                        'OldShipBillNo': $scope.txtShipBillNo,
                        'OldShipBillDate': $scope.ShipBillDate,
                        'OldParty': $scope.Party,
                        'OldWeight': $scope.Weight,
                        'OldPkg': $scope.Pkg,
                        'OldCargo': $scope.Cargo,
                        'OldArea': $scope.Area,
                        'OldFOB': $scope.FOB,
                        'OldCCINId': $scope.CCINId,
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
                    $scope.CCINId = "";
                    $scope.OldCommodityID = 0;
                    $scope.OldPartyId = 0;
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
            debugger;
            if ($scope.txtNewPartyName != '' && $scope.txtNewShipBillNo != '' && $scope.txtNewShipBillDate != '' && $scope.txtWeight > 0 && $scope.txtPkg > 0 && $scope.txtFOB >= 0) {
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
                        CHNAmendmentService.saveAmendment($scope.OldData, $scope.NewData, $scope.AmendDate, $scope.AmendmentNO).then(function (res) {
                            console.log(res.data);
                            debugger;
                            if (res.data != '') {
                                debugger;
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

                                $scope.Saveinfo = res.data.Message;
                                $scope.GetAmendDetails('');
                            }
                        });
                    }
                }

            }

        }



        $scope.GetAmendDetails = function (AmendNo) {

            CHNAmendmentService.GetAmendDetails(AmendNo).then(function (res) {
                //console.log(res.data);
                $scope.AllAmendData = res.data;
                debugger;


            });
        }
        $scope.GetAmendDetails('');




        $scope.ViewAmendDetails = function (AmendNo) {
            $scope.OldData = [];
            $scope.NewData = [];
            CHNAmendmentService.GetAmendDetailsByAmendNo(AmendNo).then(function (res) {
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

        $scope.tabGetCBDetailsBySbIdDate = function (SbNo, SbDate) {
            $scope.TabSaveinfo = '';
            $('#SBNoModalTab').modal('hide');
            CHNAmendmentService.GetSBDetails(SbNo, SbDate).then(function (res) {
                debugger;
                $scope.SBDetails = res.data;
                if ($scope.SBDetails.length > 0) {
                    $scope.tabShipBillDate = $scope.SBDetails[0].ShipBillDate;
                    $('#txttabNewShipbillDate').val($scope.SBDetails[0].ShipBillDate);
                    $('#txttabNewShipbillDate').trigger("click");
                    $scope.txtOldShipbilldate = $scope.SBDetails[0].ShipBillDate;
                    $scope.tabtxtShipBillNo = $scope.SBDetails[0].ShipBillNo;
                    $scope.NewShipBillNo = $scope.SBDetails[0].ShipBillNo;
                    $scope.tabtxtExporterName = $scope.SBDetails[0].Exporter;
                    $scope.txttabCargo = $scope.SBDetails[0].Cargo;
                    $scope.txttabWeight = parseFloat($scope.SBDetails[0].Weight);
                    $scope.txttabPkg = parseFloat($scope.SBDetails[0].Pkg);
                    $scope.Area = $scope.SBDetails[0].Area;
                    $scope.txttabFOB = parseFloat($scope.SBDetails[0].FOBValue);
                    $scope.txttabCCINID = $scope.SBDetails[0].CCINId;
                    $scope.txttabCommodityID = $scope.SBDetails[0].CommodityId;
                    $scope.txttabPartyID = $scope.SBDetails[0].ExporterId;
                    $scope.ddlCargotype = $scope.SBDetails[0].CargoType;
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
            $scope.ddlCargotype = 0;


        }

        $scope.tabtxtShipbillDate = new Date().toShortFormat();

        $scope.ValidationTab = function () {
            debugger;
            var retunflag = true;
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

            if ($scope.ddlCargotype == 0 || $scope.ddlCargotype == null || $scope.ddlCargotype == 'undefined' ) {
                //$('#AmendDate').focus();
                $scope.tabtabddlCargotype = true;
                retunflag = false;
            }
            else {

                $scope.tabtabddlCargotype = false;
                //retunflag = true;
            }


            return retunflag;





        }

        $scope.SubmitTabData = function () {
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
                    'CargoType': $scope.ddlCargotype
                };

                if (confirm('Are you sure to save?')) {
                    $('#btnSavee').attr("disabled", true);
                    CHNAmendmentService.savetabAmendment($scope.vmData).then(function (res) {
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
                            $scope.ddlCargotype = 0;
                            // $scope.GetAmendDetails('');

                            $scope.GetAmendList();
                        }
                        else {
                            $scope.TabSaveinfo = res.data.Message;
                        }
                    });
                }
            }



        }


        $scope.GetAmendList = function () {

            CHNAmendmentService.GetAmendLst().then(function (res) {
                debugger;
                $scope.ListAmendLstData = res.data;
            });
        }
        $scope.GetAmendList();

        $scope.ViewAmendDetailsList= function (id)
        {
            debugger;
            CHNAmendmentService.GetAmendView(id).then(function (res) {
                debugger;
                $scope.AmenViewData = res.data;
               
                    
                $scope.txttabWeight = $scope.AmenViewData.Weight;
                $scope.txttabPkg = $scope.AmenViewData.Pkg;
                
                $scope.NewShipBillNo = $scope.AmenViewData.ShipBillNo;
                $scope.tabShipBillDate = $scope.AmenViewData.ShipbillDate;
              
                $scope.txttabFOB =parseInt($scope.AmenViewData.FOB);
                $scope.ddlCargotype = $scope.AmenViewData.CargoType;
                $scope.tabtxtExporterName = $scope.AmenViewData.ExporterName;
                $scope.txttabCargo = $scope.AmenViewData.Cargo;
                $('#btnSavee').attr("disabled", 'disabled');


            });
        }
    });
})();