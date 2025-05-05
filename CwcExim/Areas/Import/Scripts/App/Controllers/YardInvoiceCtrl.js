(function () {
    angular.module('CWCApp').
    controller('YardInvoiceCtrl', function ($scope, YardInvoiceService) {
        $scope.HtCharges = ['LOL', 'LOE', 'SH', 'BTT', 'TPT', 'DTF', 'HND', 'MF'];
        $scope.OTHours = 0;


        $scope.PlaceOfSupply = "";
        $scope.InvoiceNo = "";
        $scope.conatiners = [];
        $scope.Message = '';
        $scope.IsSubmitClicked = false;
        $scope.CWCChargeList = [];
        $scope.HTChargeList = [];
        $scope.Nday = "";
        $scope.PartyList = [];
        $scope.PayeeList = [];
        $scope.SearchPartyText = "";
        $scope.SearchPayeeText = "";
        $scope.PartyPage = 0;
        $scope.PayeePage = 0;
        $scope.btnParty = false;
        $scope.btnPayee = false;

        $('#Approved').prop("disabled", false);
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());
        }
        /*$scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        $scope.PayeeList = JSON.parse($('#hdnPartyPayee').val());*/
        $scope.Rights = JSON.parse($("#hdnRights").val());

        /*********Party / Payee List and Search*******************/
        /*********************************************************/
        $scope.LoadPartyList = function () {
            $scope.PartyPage = 0;
            $scope.SearchPartyText = "";
            YardInvoiceService.LoadPartyList($scope.PartyPage).then(function (res) {
                $scope.PartyList = res.data.lstParty;
                $scope.btnParty = res.data.State;
            });
        }
        $scope.LoadPayeeList = function () {
            $scope.PayeePage = 0;
            $scope.SearchPayeeText = "";
            YardInvoiceService.LoadPartyList($scope.PayeePage).then(function (res) {
                $scope.PayeeList = res.data.lstParty;
                $scope.btnPayee = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.LoadMorePartyList = function () {
            $scope.PartyPage = $scope.PartyPage + 1;
            YardInvoiceService.LoadPartyList($scope.PartyPage).then(function (res) {
                $.each(res.data.lstParty, function (i, elem) {
                    $scope.PartyList.push(elem);
                });
                $scope.btnParty = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.LoadMorePayeeList = function () {
            $scope.PayeePage = $scope.PayeePage + 1;
            YardInvoiceService.LoadPartyList($scope.PayeePage).then(function (res) {
                $.each(res.data.lstParty, function (i, elem) {
                    $scope.PayeeList.push(elem);
                });
                $scope.btnPayee = res.data.State;
            });
            //$scope.$digest();
        }
        $scope.SearchPartyList = function () {
            YardInvoiceService.SearchPartyList($scope.SearchPartyText).then(function (res) {
                $scope.PartyList = res.data.lstParty;
            });
            $scope.PartyPage = 0;
            $scope.btnParty = false;
            //$scope.$digest();
        }
        $scope.SearchPayeeList = function () {
            YardInvoiceService.SearchPartyList($scope.SearchPayeeText).then(function (res) {
                $scope.PayeeList = res.data.lstParty;
            });
            $scope.PayeePage = 0;
            $scope.btnPayee = false;
            //$scope.$digest();
        }
        $scope.SearchOnEnterPartyList = function (e) {
            if (e.keyCode == 13) {
                $scope.SearchPartyList();
            }
        }
        $scope.SearchOnEnterPayeeList = function (e) {
            if (e.keyCode == 13) {
                $scope.SearchPayeeList();
            }
        }


      //  $scope.LoadPartyList();
      //  $scope.LoadPayeeList();
        /*********************************************************/

        $scope.SelectParty = function (obj) {
            $scope.PartyId = obj.PartyId;
            //var PartyName = obj.PartyName.split('_');
            $scope.PartyName = obj.PartyName;
            $scope.hdnState = obj.State;
            $scope.PlaceOfSupply = obj.State;

            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.GSTNo;
            //$scope.SelectedPartyIndex=i;
            $('#PartyModal').modal('hide');
        };

        $scope.SelectPayee = function (obj) {
            $scope.PayeeId = obj.PartyId;
            // var PartyName = obj.PartyName.split('_');
            $scope.PayeeName = obj.PartyName;
            //$scope.PayeeName = obj.PartyName;
            //$scope.SelectedPayeeIndex=i;
            $('#PayeeModal').modal('hide');
        };


        $scope.SelectReqNo = function (obj) {           
            $scope.StuffingReqId = obj.StuffingReqId;
            $scope.StuffingReqNo = obj.StuffingReqNo;
            $scope.StuffingReqDate = obj.StuffingReqDate;

            $scope.PartyId = obj.CHAId;
            $scope.PartyName = obj.CHAName;
            $scope.hdnState = obj.State;
            $scope.PlaceOfSupply = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.CHAGSTNo;

            $scope.PayeeId = obj.CHAId;
            $scope.PayeeName = obj.CHAName;
            /*$scope.OBLNo = obj.OBLNo;
            $scope.ContainerNo = obj.ContainerNo;
            $scope.SealCutDate = obj.SealCutDate;
            $scope.NoOfPkg = obj.NoOfPkg;
            $scope.GrWait = obj.GrWait;*/
            var dStr = obj.StuffingReqDate.split('/');            
            var dt=dStr[2] + '-' + dStr[1] + '-' + dStr[0];
            $("#InvoiceDate").datepicker("option", "minDate", new Date(dt));
            //$("#InvoiceDate").datepicker("option", "minDate", obj.StuffingReqDate);
            //$scope.SelectedReqIndex = i;
            /*
            $http.get('/Import/Ppg_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + $scope.ReqNos[i].StuffingReqId).then(function (res) {
                $scope.conatiners =JSON.parse( res.data);
                // console.log(res.data);
            });
            */
            YardInvoiceService.SelectReqNo($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                console.log($scope.conatiners);
            });

            $('#stuffingModal').modal('hide');
        }



        $scope.SelectReqNoTentative = function (obj) {
            $scope.StuffingReqId = obj.StuffingReqId;
            $scope.StuffingReqNo = obj.StuffingReqNo;
            $scope.StuffingReqDate = obj.StuffingReqDate;

            $scope.PartyId = obj.CHAId;
            $scope.PartyName = obj.CHAName;
            $scope.hdnState = obj.State;
            $scope.hdnStateCode = obj.StateCode;
            $scope.hdnAddress = obj.Address;
            $scope.GSTNo = obj.CHAGSTNo;

            $scope.PayeeId = obj.CHAId;
            $scope.PayeeName = obj.CHAName;

            var dStr = obj.StuffingReqDate.split('/');
            var dt = dStr[2] + '-' + dStr[1] + '-' + dStr[0];
            $("#InvoiceDate").datepicker("option", "minDate", new Date(dt));

            //$("#InvoiceDate").datepicker("option", "minDate", obj.StuffingReqDate);
            //$scope.SelectedReqIndex = i;
            /*
            $http.get('/Import/Ppg_CWCImport/GetPaymentSheetContainer/?AppraisementId=' + $scope.ReqNos[i].StuffingReqId).then(function (res) {
                $scope.conatiners =JSON.parse( res.data);
                // console.log(res.data);
            });
            */
            YardInvoiceService.SelectReqNoTentative($scope.StuffingReqId).then(function (res) {
                $scope.conatiners = JSON.parse(res.data);
                // console.log(res.data);
            });

            $('#stuffingModal').modal('hide');
        }


        $scope.Print = function () {
            //debugger;
            YardInvoiceService.PrintInvoice($scope.InvoiceObj).then(function (res) {

                //debugger;

                window.open(res.data.Message + "?_t=" + (new Date().getTime()), "_blank");

            });
        }

        $scope.InvoiceObj = {};
        $scope.IsContSelected = false;
        $scope.ContainerSelect = function () {
            $('#InvoiceDate').parent().find('img').css('display', 'none');
            
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            if (c > 0) {

                var isdirect = 0;
                if ($('#Approved').prop("checked") == true) {
                    isdirect = 1;
                }

                $scope.SEZ = $('#SEZ').val();


                YardInvoiceService.ContainerSelect(0, $('#InvoiceDate').val(), $scope.StuffingReqId, TaxType, $scope.conatiners, $scope.OTHours, $scope.PartyId, $scope.PayeeId, isdirect, $scope.SEZ).then(function (res) {
                    //debugger;
                    $scope.InvoiceObj = res.data;
                    $scope.Nday = $scope.InvoiceObj.NDays;
                    /*********CWC Charge and HT Charges Distinction***************/
                    $scope.CWCChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {

                        return $scope.HtCharges.indexOf(item.Clause) < 0;
                    });
                    $scope.HTChargeList = $scope.InvoiceObj.lstPostPaymentChrg.filter(function (item) {
                        return $scope.HtCharges.indexOf(item.Clause) > -1;
                    });
                    /*************************************************************/
                    $scope.IsContSelected = true;
                    //console.log($scope.InvoiceObj);
                    if ($scope.Rights.CanAdd == 1) {
                        $('#btnSave').removeAttr("disabled");
                    }
                    $('.search').css('display', 'none');
                    $('#InvoiceDate').parent().find('img').css('display', 'none');
                    $('#OTHours').prop('readonly', true);
                    $('#Approved').prop("disabled", true);
                    $('#SEZ').prop("disabled", true);
                });
            }
            $('#stuffingModal').modal('hide');

        }


        $scope.SubmitInvoice = function () {

            if ($scope.StuffingReqId == 0 || $scope.StuffingReqId == '' || $scope.StuffingReqId == null) {
                $scope.Message = "Select Assessment Id";
                return false;
            }
            if ($scope.PartyId == 0 || $scope.PartyId == '' || $scope.PartyId == null) {
                $scope.Message = "Select Party";
                return false;
            }
            var isdirect = 0;
            if ($('#Approved').prop("checked") == true) {
                isdirect = 1;
            }
            var c = 0;
            for (i = 0; i < $scope.conatiners.length; i++) {

                if ($scope.conatiners[i].Selected == true) {
                    c = c + 1;
                }
            }

            if (c <= 0) {
                $scope.Message = "Select Atleast one container";
                return false;
            }
            if ($scope.InvoiceObj.TotalAmt <= 0) {
                $scope.Message = "Can not be saved. Invoice Amount cannot be Zero or Negative.";
                return false;
            }
            if ($scope.InvoiceObj.PaymentMode == "CASH") {



                if (confirm('Are you sure to Generate Cash Invoice?')) {
                    $('#btnSave').attr("disabled", true);
                    $scope.InvoiceObj.InvoiceId = 0;
                    $scope.InvoiceObj.InvoiceType = TaxType;
                    $scope.InvoiceObj.PartyId = $scope.PartyId;
                    $scope.InvoiceObj.PartyName = $scope.PartyName;
                    $scope.InvoiceObj.PartyAddress = $scope.hdnAddress;
                    $scope.InvoiceObj.PartyGST = $scope.GSTNo;
                    $scope.InvoiceObj.PartyState = $scope.hdnState;
                    $scope.InvoiceObj.PartyStateCode = $scope.hdnStateCode;
                    $scope.InvoiceObj.PlaceOfSupply = $scope.PlaceOfSupply;
                    $scope.InvoiceObj.PayeeId = $scope.PayeeId;
                    $scope.InvoiceObj.PayeeName = $scope.PayeeName;
                    $scope.InvoiceObj.Remarks = $scope.Remarks;
                    //console.log($scope.InvoiceObj);

                    //var objfinal = $scope.InvoiceObj;



                    YardInvoiceService.GenerateInvoice($scope.InvoiceObj, isdirect, $scope.SEZ).then(function (res) {
                        console.log(res);
                        var InvSupplyData = res.data.Data.InvoiceNo.split(',');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        $scope.Message = res.data.Message;
                        $scope.Message = res.data.Message;

                        $('#btnSave').attr("disabled", true);
                        if (res.data.Status == 0) {
                            $('#btnPrint').attr("disabled");
                        }
                        else {
                            $('#btnPrint').removeAttr("disabled");
                            $('#BtnGenerateIRN').removeAttr("disabled");
                        }
                    });
                }



            }
            else {

                if (confirm('Are you sure to Generate this Invoice?')) {
                    $('#btnSave').attr("disabled", true);
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
                    $scope.InvoiceObj.Remarks = $scope.Remarks;
                    //console.log($scope.InvoiceObj);

                    //var objfinal = $scope.InvoiceObj;



                    YardInvoiceService.GenerateInvoice($scope.InvoiceObj, isdirect,$scope.SEZ).then(function (res) {
                        console.log(res);
                        //debugger;
                        var InvSupplyData = res.data.Data.InvoiceNo.split(',');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        $scope.Message = res.data.Message;

                        $('#btnSave').attr("disabled", true);
                        if (res.data.Status == 0) {
                            $('#btnPrint').attr("disabled");
                        }
                        else {
                            $('#btnPrint').removeAttr("disabled");
                            $('#BtnGenerateIRN').removeAttr("disabled");
                        }
                    });
                }
            }
        }

        $scope.GetAppNo = function () {

            //debugger;
            YardInvoiceService.GetAppNoForYard().then(function (res) {
                //debugger;

                $('#hdnStuffingReq').val(res.data);
                $scope.ReqNos = JSON.parse($('#hdnStuffingReq').val());


            });

        };
        $scope.GenerateIRN = function () {


            YardInvoiceService.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {

                alert(res.data.Message);

            });

        };

    });
})();