(function () {
    angular.module('CWCApp')
    .directive("datepicker", function () {
        function link(scope, element, attrs) {
            // CALL THE "datepicker()" METHOD USING THE "element" OBJECT.
            element.datetimepicker({
                showOn: "button",
                    buttonImage: "/Content/images/calendar.png",
                    buttonImageOnly: true,
                    dateFormat: "dd/mm/yy",
                    altField: "#slider_example_4andHalf_alt",
                    altFieldTimeOnly: false,
                    onClose: function () {
                        $(".Date_Img .Error_Msg").text(""); 
                        $('[data-valmsg-for="Data"]').html('<span></span>');
                    }
            });
        }
        return {
            require: 'ngModel',
            link: link
        };
    })
    .controller('RefInvoiceCtrl', function ($scope, ReeferService) {
        $scope.ReeferInvNo = '';
        $scope.SupplyType = '';
        $scope.IsCalculated = 0;
        $scope.Distance = 0;
        $scope.Message = '';
        $scope.InvoiceId = 0;
        $scope.InvoiceNo = '';
        $scope.PartyId = 0;
        $scope.PartyName = '';
        $scope.PayeeId = '';
        $scope.PayeeName = '';
        $scope.StatePayer = '';
        $scope.CHAName = '';
        $scope.ExporterName = '';
        $scope.InvoiceDate = '';
        $scope.GSTNo = '';
        $scope.PayerCode = '';
        $scope.PayerPage = 0;
        $scope.InvoiceList = [];
        $scope.PayeeList = [];
        $scope.InvoiceObj = {};
        $scope.ConatinersList = [];
        $scope.ListOfCont = [];
        $scope.ExportUnder = "";        
        $scope.TaxType = ($('#Tax').is(':checked') == true ? 'Tax' : 'Bill');
        /*******************Invoice No,Payee,Party Bind*****************************/
        if ($('#hdnStuffingReq').val() != null && $('#hdnStuffingReq').val() != '') {
            $scope.InvoiceList = JSON.parse($('#hdnStuffingReq').val());
        }
        if ($('#hdnPayee').val() != null && $('#hdnPayee').val() != '') {
            $scope.PayerList = JSON.parse($('#hdnPayee').val());
            $scope.StatePayer = $('#hdnPayerState').val();
        }
        /*******************Invoice No,Payee,Party Bind*****************************/
        $scope.SelectInvoiceNo = function (item) {
            debugger;
            var InvoiceCont = item.InvoiceNo.split('_');
            $scope.InvoiceId = item.InvoiceId;
            $scope.InvoiceNo = InvoiceCont[0];
            $scope.InvoiceDate = item.Date;
            $scope.CHAName = item.CHA;
            $scope.ExporterName = item.Exporter;
            $scope.PartyId = item.PartyId;
            $scope.PartyName = item.PartyName;
            $scope.PayeeId = item.PayeeId;
            $scope.PayeeName = item.PayeeName;
            $scope.GSTNo = item.GSTNo;
            $('#invoicemodal').modal('hide');
            ReeferService.LoadContainer($scope.InvoiceId).then(function (res) {
                if (res.data.Status == 1)
                    $scope.ConatinersList = res.data.Data;
            });
        }
        $scope.SelectPayer = function (item) {
            debugger;
            $scope.PayeeId = item.PartyId;
            $scope.PayeeName = item.PartyName;
            //$scope.GSTNo = item.GSTNo;
            $('#PayerModal').modal('hide');
            $scope.LoadPayer();
        }
        $scope.CalculateCharges = function () {
            debugger;
            var ContList = ($scope.ConatinersList.filter(x=>x.Selected == true && x.PlugInDatetime != '' && x.PlugOutDatetime != ''));
            $scope.TaxType = ($('#Tax').is(':checked') == true ? 'Tax' : 'Bill');
            if (ContList.length > 0) {
                ReeferService.CalculateCharges($scope.InvoiceDate, $scope.PartyId, $scope.TaxType, ContList, $scope.InvoiceId, $scope.PayeeId, $scope.PayeeName, $scope.ExportUnder, $scope.Distance).then(function (res) {
                    $scope.IsCalculated = 1;
                    $scope.InvoiceObj = res.data;
                    $.each($scope.InvoiceObj.lstPostPaymentCont, function (i, item) {
                        if ($scope.ListOfCont.filter(x=>x.CFSCode == item.CFSCode).length <= 0)
                            $scope.ListOfCont.push(item);
                    });
                    $("#InvoiceDate").datepicker("option", "disabled", true);
                    $scope.InvoiceDate = $scope.InvoiceObj.InvoiceDate;
                    $('#Tax,#Bill').prop('disabled', true);
                });
            }
            $('#ContainerModal').modal('hide');
        }
        $scope.AddEditReeferInv = function () {
            var conf = confirm('Are you sure you want to Save?');
            if (conf == true) {
                $scope.InvoiceObj.ExportUnder = $scope.ExportUnder;
                $scope.InvoiceObj.Distance = $scope.Distance;
                ReeferService.AddEditReeferInv($scope.InvoiceObj).then(function (res) {
                    if (res.data.Status == 1) {
                        $scope.Message = res.data.Message;
                        var InvSupplyData = res.data.Data.split('-');
                        $scope.ReeferInvNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        //$scope.ReeferInvNo = res.data.Data;
                        debugger;
                        $('#hdnSupplyType').val($scope.SupplyType);
                        $('#ReeferInvNo').val($scope.ReeferInvNo);
                        $('#btnSave').prop('disabled', true);
                        $('#btnPrint').prop('disabled', false);
                        $('#btnGeneratedIRN').prop('disabled', false);
                    }
                    else $scope.Message = res.data.Message;
                });
            }
        }
        $scope.ReCalculate = function (obj, i) {
            if ($scope.TaxType == 'Bill') {
                obj.Total = obj.Taxable;
                $scope.InvoiceObj.AllTotal = parseFloat(obj.Taxable);
                $scope.InvoiceObj.InvoiceAmt = Math.ceil(parseFloat(obj.Taxable));
                $scope.InvoiceObj.RoundUp = parseFloat($scope.InvoiceObj.InvoiceAmt) - parseFloat($scope.InvoiceObj.AllTotal);
            }
            else {
                if ($scope.InvoiceObj.CompStateCode = $scope.InvoiceObj.PartyStateCode) {
                    obj.CGSTAmt = ((parseFloat(obj.CGSTPer) * parseFloat(obj.Taxable)) / 100).toFixed(2);
                    obj.SGSTAmt = ((parseFloat(obj.SGSTPer) * parseFloat(obj.Taxable)) / 100).toFixed(2);
                }
                else {
                    obj.IGSTAmt = ((parseFloat(obj.IGSTPer) * parseFloat(obj.Taxable)) / 100).toFixed(2);
                }
                $scope.InvoiceObj.lstPostPaymentChrg[i].CGSTAmt = obj.CGSTAmt;
                $scope.InvoiceObj.lstPostPaymentChrg[i].SGSTAmt = obj.SGSTAmt;
                $scope.InvoiceObj.lstPostPaymentChrg[i].IGSTAmt = obj.IGSTAmt;
                $scope.InvoiceObj.lstPostPaymentChrg[i].Total = (parseFloat(obj.CGSTAmt) + parseFloat(obj.SGSTAmt) + parseFloat(obj.IGSTAmt) + parseFloat(obj.Taxable)).toFixed(2);

                $scope.InvoiceObj.AllTotal = (parseFloat(obj.Taxable) + parseFloat(obj.IGSTAmt) + parseFloat(obj.CGSTAmt) + parseFloat(obj.SGSTAmt)).toFixed(2);
                $scope.InvoiceObj.InvoiceAmt = (Math.ceil(parseFloat($scope.InvoiceObj.AllTotal))).toFixed(2);
                $scope.InvoiceObj.RoundUp = (parseFloat($scope.InvoiceObj.InvoiceAmt) - parseFloat($scope.InvoiceObj.AllTotal)).toFixed(2);
            }
        }

        $scope.LoadPayer = function () {
            $scope.PayerPage = 0;
            ReeferService.LoadPayerList($scope.PayerPage).then(function (data) {
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
            ReeferService.LoadPayerList($scope.PayerPage).then(function (data) {
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
            ReeferService.SearchPayerByPayerCode($scope.PayerCode).then(function (data) {
                if (data.data.Status == 1) {
                    $scope.PayerList = data.data.Data.lstPayer;
                    $scope.StatePayer = data.data.Data.StatePayer;
                }
                else {
                    $scope.Message = data.data.Message;
                }
            });
        }       

        $scope.ClosePayer = function () {
            debugger;
            $scope.LoadPayer();
            $('#PayerModal').modal('hide');
        }
    });
})();