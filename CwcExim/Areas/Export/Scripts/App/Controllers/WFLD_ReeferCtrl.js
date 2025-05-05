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
                    changeMonth: true,
                    changeYear: true,
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
      
        document.getElementById("BtnAdd").style.display = 'none';
        debugger;
        $scope.ReeferInvNo = '';
        $scope.IsCalculated = 0;
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
      
        $scope.GSTNo = '';
        $scope.PartyState = '';
        $scope.PayerCode = '';
        $scope.PayerPage = 0;
        $scope.InvoiceList = [];
        $scope.PayeeList = [];
        $scope.InvoiceObj = {};
        $scope.ConatinersList = [];
        $scope.ListOfCont = [];
        $scope.ExportUnder = "";
        $scope.SEZ = ($('#SEZ').is(':checked') == true ? 1 : 0);
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
            $scope.PartyState = item.StateName;
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
            $scope.NOOfContainer = $scope.AllContainerList.length;

            if ($scope.ValidationCharge()) {


                // var ContList = ($scope.ConatinersList.filter(x=>x.Selected == true && x.PlugInDatetime != '' && x.PlugOutDatetime != ''));
                $scope.TaxType = ($('#Tax').is(':checked') == true ? 'Tax' : 'Bill');
                $scope.SEZ = ($('#SEZ').val());
                if ($scope.AllContainerList.length > 0) {
                    ReeferService.CalculateCharges($("#InvoiceDate").val(), $scope.PartyId, $scope.TaxType, $scope.AllContainerList, $scope.InvoiceId, $scope.PayeeId, $scope.PayeeName, $scope.ExportUnder,$scope.SEZ).then(function (res) {
                        debugger;
                        $scope.IsCalculated = 1;
                        $scope.InvoiceObj = res.data;
                        $.each($scope.InvoiceObj.lstPostPaymentCont, function (i, item) {
                            if ($scope.ListOfCont.filter(x=>x.CFSCode == item.CFSCode).length <= 0)
                                $scope.ListOfCont.push(item);
                        });
                        $("#InvoiceDate").datepicker("option", "disabled", true);
                        $("#PlugInDatetime").datepicker("option", "disabled", true);
                        $("#txtPlugOUTDatetime").datepicker("option", "disabled", true);
                        
                        $('#DivReefer span > i').remove()
                       
                        $('.edit').css('pointer-events', 'none');
                        $('.delete').css('pointer-events', 'none');
                        //$("#InvoiceDate").val($scope.InvoiceObj.InvoiceDate);
                        $('#SEZ').prop("disabled", true);
                        $('#Tax,#Bill').prop('disabled', true);
                    });
                }
                //$('#ContainerModal').modal('hide');
            }
            
        }
        $scope.AddEditReeferInv = function () {
            var conf = confirm('Are you sure you want to Save?');
            if (conf == true) {
             $('#btnSave').attr('disabled', true);
                $scope.InvoiceObj.ExportUnder = $scope.ExportUnder;
                $scope.InvoiceObj.SEZ = ($('#SEZ').val());
                ReeferService.AddEditReeferInv($scope.InvoiceObj).then(function (res) {
                    if (res.data.Status == 1) {
                        debugger;
                        $scope.Message = res.data.Message;
                        var InvSupplyData = res.data.Data.split(',');
                        $scope.InvoiceNo = InvSupplyData[0];
                        $scope.SupplyType = InvSupplyData[1];
                        $scope.ReeferInvNo = $scope.InvoiceNo;
                        $('#ReeferInvNo').val($scope.ReeferInvNo);
                        $('#btnSave').prop('disabled', true);
                        $('#btnPrint').prop('disabled', false);
                        $('#BtnGenerateIRN').removeAttr('disabled');
                    }
                    else $scope.Message = res.data.Message;
                });
            }
        }


        $scope.GenerateIRN = function () {


            ReeferService.GenerateIRNNo($scope.InvoiceNo, $scope.SupplyType).then(function (res) {

                alert(res.data.Message);

            });

        };




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
                $scope.InvoiceObj.InvoiceAmt = ((parseFloat($scope.InvoiceObj.AllTotal))).toFixed(2);
                $scope.InvoiceObj.RoundUp = 0;// (parseFloat($scope.InvoiceObj.InvoiceAmt) - parseFloat($scope.InvoiceObj.AllTotal)).toFixed(2);
            }
        }

        //$scope.GetContainerList = function () {
        //    debugger;
        //    $scope.PayerPage = 1;
        //    ReeferService.LoadContainerList('').then(function (data) {
        //        debugger
        //        $scope.PayerList = data.data.Data.lstPayer;
        //        $scope.StatePayer = data.data.Data.StatePayer;
        //    });
        //}


        $scope.StatePayer = false;

        $scope.GetPayeerList=function()
        {
            debugger;
            $scope.PayerPage = 1;
            ReeferService.LoadPayerList('', $scope.PayerPage).then(function (data) {
                debugger
                $scope.PayerList = data.data.Data.lstPayer;
                $scope.StatePayer = data.data.Data.StatePayer;
            });
        }



        $scope.LoadPayer = function () {
            $scope.PayerPage = 1;
            ReeferService.LoadPayerList('', $scope.PayerPage).then(function (data) {
                //console.log(data);                
                    $scope.PayerList = data.data.lstPayer;
                    $scope.StatePayer = data.data.StatePayer;                
            });
        }

        $scope.LoadMorePayer = function () {
            $scope.PayerPage = $scope.PayerPage + 1;
            ReeferService.LoadPayerList('', $scope.PayerPage).then(function (data) {
                debugger;
                //console.log(data);
                if (data.data != '') {
                    $.each(data.data.Data.lstPayer, function (item, elem) {
                        $scope.PayerList.push(elem);
                    });
                    $scope.StatePayer = data.data.StatePayer;
                }
                else {
                    // $scope.Message = data.data.Message;
                }
            });
        }

        $scope.SearchPayerByPayerCode = function () {
            debugger;
            $scope.PayerPage = 1;
            ReeferService.LoadPayerList($scope.PayerCode, $scope.PayerPage).then(function (data) {
            
                if (data.data != '') {
                    $scope.PayerList = data.data.Data.lstPayer;
                    $scope.StatePayer = data.data.Data.StatePayer;
                }
               
            });
        }       

        $scope.ClosePayer = function () {
            debugger;
            //$scope.LoadPayer();
            $('#PayerModal').modal('hide');
        }

        $scope.StateParty = false;
       
        $scope.GetPartyList= function ()
        {
            $scope.PartyFlag = 1;
            ReeferService.GetPartyList('', $scope.PartyFlag).then(function (data) {
                $scope.GetPartyListarr = data.data.lstParty;
                $scope.StateParty = data.data.State;
            });
            
        }
        

        $scope.LoadMoreParty=function()
        {
            $scope.PartyFlag++;
            ReeferService.GetPartyList('', $scope.PartyFlag).then(function (data) {
                if (data.data != '') {
                    $.each(data.data.lstParty, function (item, elem) {
                        $scope.GetPartyListarr.push(elem);
                    });

                    $scope.StateParty = data.data.State;
                }
            });
        }

        $scope.SearchParty = function () {
            debugger;
            ReeferService.GetPartyList($scope.PartyCode, 1).then(function (data) {
                if (data.data != '') {
                    $.each(data.data.lstParty, function (item, elem) {
                        $scope.GetPartyListarr.push(elem);
                    });
                    //$scope.GetPartyListarr = data.data.lstParty;
                    $scope.StateParty = data.data.State;

               
                }
            });
        }

        $scope.SelectParty=function(data)
        {
            $scope.PartyId = data.PartyId;
            $scope.PartyName = data.PartyName;
            $scope.PartyState = data.State;
            $scope.GSTNo = data.GSTNo;
            $('#PartyModal').modal('hide');
            debugger;
        }

        $scope.CloseParty=function()
        {
            
            $('#PartyModal').modal('hide');
        }

       

        //Exporter Name

        $scope.GetExporterList = function () {
            $scope.exportFlag = 1;
            ReeferService.GetExporterList('', $scope.exportFlag).then(function (data) {
                $scope.GetExporterListarr = data.data.lstPayer;
                $scope.StateParty = data.data.StatePayer;
            });

        }

        $scope.exportFlag = 1;
        $scope.LoadMoreExporter = function () {
            $scope.PartyFlag++;
            ReeferService.GetExporterList('', $scope.exportFlag).then(function (data) {
                if (data.data != '') {
                    $.each(data.data.lstPayer, function (item, elem) {
                        $scope.GetExporterListarr.push(elem);
                    });

                    $scope.StateParty = data.data.StatePayer;
                }
            });
        }

        $scope.SearchExporter = function () {

            ReeferService.GetExporterList($scope.ExporterCode, 1).then(function (data) {
                if (data.data != '') {
                    $scope.GetExporterListarr = data.data.lstPayer;
                    $scope.StateParty = data.data.StatePayer;


                }
            });
        }

        $scope.SelectExporter = function (data) {
            $scope.ExporterId = data.PartyId;
            $scope.ExporterName = data.PartyName;          
            $('#ExporterModal').modal('hide');
            debugger;
        }

        $scope.CloseExporter = function () {

            $('#ExporterModal').modal('hide');
        }


        $scope.GetAllContainerNo=function()
        {
            ReeferService.GetReeferContainerList().then(function (data) {
                debugger;
                if (data.data.Status = 1)
                {

                    debugger;
                    $scope.GetAllContainerCFS = data.data.Data
                }
                
            });
        }

        debugger;
        $scope.AllContainerList = [];
        var flagAdd = 0;
        if ($('#BtnAdd').css('display') == 'none') {
            debugger;
            flagAdd=1;
        }
        var flag = 0;
        $scope.GetConatinerNo = function (item) {
            debugger;
            var flagAdd = 0;
            var flag = 0;
            if ($('#BtnAdd').css('display') == 'none') {
                debugger;
                flagAdd = 1;
            }
           
            debugger;
            if (flagAdd == 1) {

                if (flag == 0) {

                    if ($scope.AllContainerList.filter(x=>x.ContainerNo == item.ContainerNo).length <= 0) {
                        $scope.ContainerNo = item.ContainerNo;
                    }
                    else {
                        flag = 1;
                        //$scope.ContainerNo = '';
                        alert('Container No already Added');                        
                        return false;
                    }
                                       
                    $('#ContainerModal').modal('hide');

                    ReeferService.GetReeferContainerDet(item.LoadContainerId).then(function (data) {
                        debugger;
                        if (data.Status = 1) {
                            debugger
                            $scope.ExporterId = data.data.Data.Exporter
                            $scope.ExporterName = data.data.Data.ExporterName;
                            $scope.CHAId = data.data.Data.CHAId;
                            $scope.CHAName = data.data.Data.CHAName;
                            $scope.PartyId = data.data.Data.CHAId;
                            $scope.PartyName = data.data.Data.CHAName;
                            $scope.PayeeId = data.data.Data.CHAId;
                            $scope.PayeeName = data.data.Data.CHAName;
                            $scope.PartyState = data.data.Data.StateName;
                            $scope.GSTNo = data.data.Data.GST;
                            $scope.PlugInDatetime = data.data.Data.FromDate;
                            $scope.PlugOUTDatetime = data.data.Data.ToDate
                            $scope.calcHour();
                            if (flag == 0) {
                                debugger;
                                $scope.AllContainerList.push({ 'ContainerNo': item.ContainerNo, 'Size': item.Size, 'CFSCode': item.CFSCode, 'PlugInDatetime': $scope.PlugInDatetime, 'PlugOutDatetime': $scope.PlugOUTDatetime, 'LoadContainerId': item.LoadContainerId, 'Hours': $scope.Hours });
                                $scope.NOOfContainer = $scope.AllContainerList.length;
                            }
                        }

                    });

                }
                else {
                    //alert('Container No already Added');
                    //return false;
                    alert("You are in Edit Mode so you unable to add new container");
                    document.getElementById("BtnAdd").style.display = 'block';

                }


            }
        }


        $scope.calcHour = function () {
            debugger;
            var plugInDatetime = $scope.PlugInDatetime;
            var PlugOUTDatetime = $scope.PlugOUTDatetime;
            var varplugInDatetime = new Date(plugInDatetime.substring(6, 10), parseInt(plugInDatetime.substring(3, 5)) - 1, plugInDatetime.substring(0, 2), plugInDatetime.substring(11, 13), plugInDatetime.substring(14, 16));
                var varPlugOUTDatetime = new Date(PlugOUTDatetime.substring(6, 10), parseInt(PlugOUTDatetime.substring(3, 5))-1, PlugOUTDatetime.substring(0, 2), PlugOUTDatetime.substring(11, 13), PlugOUTDatetime.substring(14, 16));
                if (varPlugOUTDatetime.getTime() <= varplugInDatetime.getTime()) {
                    alert('Period To DateTime must be greater than Period From DateTime');
                    $('#txtHours').val('');
                    return false;
                }
                else {
                    var timeDiff = Math.abs(varPlugOUTDatetime.getTime() - varplugInDatetime.getTime())
                    var HoursDays = Math.ceil(timeDiff / (1000 * 3600));
                  //  $('#txtHours').val(HoursDays);
                    $scope.Hours = HoursDays;
             
                }

            

        };

        $scope.DeleteContainerNo = function (id) {
            debugger;
            $scope.AllContainerList.splice(id, 1);
            $scope.NOOfContainer = $scope.AllContainerList.length;
        }
        $scope.EditContainerNo = function (id) {
            debugger;
            if ($('#BtnAdd').css('display') == 'block') {                
                alert('Please Add Above Container First');
                return false;
            }

            flag = 2;
            $scope.ContainerNo = $scope.AllContainerList[id].ContainerNo;
            $scope.CFSCode = $scope.AllContainerList[id].CFSCode;
            $scope.Size = $scope.AllContainerList[id].Size;
            $scope.PlugInDatetime = $scope.AllContainerList[id].PlugInDatetime;
            $scope.PlugOUTDatetime = $scope.AllContainerList[id].PlugOutDatetime;
            $scope.Hours = $scope.AllContainerList[id].Hours;
            $('#txtHours').val($scope.Hours);
            $scope.LoadContainerId = $scope.AllContainerList[id].LoadContainerId;
            $scope.AllContainerList.splice(id, 1);
            document.getElementById("BtnAdd").style.display = 'block';
            //   $scope.NOOfContainer = $scope.AllContainerList.length;           
        }
        

        $scope.AddContainer = function () {       
            var hours = $('#txtHours').val();
            if (hours == '')
            {
                alert('Period To DateTime must be greater than Period From DateTime');
                return false;
            }

                flag = 0;
                $scope.AllContainerList.push({ 'ContainerNo': $scope.ContainerNo, 'Size': $scope.Size, 'CFSCode': $scope.CFSCode, 'PlugInDatetime': $scope.PlugInDatetime, 'PlugOutDatetime': $scope.PlugOUTDatetime, 'LoadContainerId': $scope.LoadContainerId, 'Hours': hours });

                document.getElementById("BtnAdd").style.display = 'none';
            //   $scope.NOOfContainer = $scope.AllContainerList.length;                
        }


        $scope.ValidationCharge=function()
        {
            debugger;
            var flag = true;
            if ($("#InvoiceDate").val() == '' || $("#InvoiceDate").val() == null || $("#InvoiceDate").val() == 'undefined')
            {
                flag = false;
                $('#spInvoiceDate').text('Fill this field');               
            }
            else
            {
                flag = true;
               
                $('#spInvoiceDate').text('');
            }
            if ($scope.ExporterName == '' || $scope.ExporterName == null || $scope.ExporterName == 'undefined') {
                flag = false;
               
                $('#spExporterName').text('Fill this field');
            }
            else {
                flag = true;
               
                $('#spExporterName').text('');
            }
            if ($scope.PartyName == '' || $scope.PartyName == null || $scope.PartyName == 'undefined') {
                flag = false;
               
                $('#spPartyName').text('Fill this field');
            }
            else {
                
                $('#spPartyName').text('');
                flag = true;
            }
            if ($scope.PlugInDatetime == '' || $scope.PlugInDatetime == null || $scope.PlugInDatetime == 'undefined') {
                flag = false;
             
                $('#spPlugInDatetime').text('Fill this field');
            }
            else {
                flag = true;
                
                $('#spPlugInDatetime').text('');
            }

            if ($scope.PlugOUTDatetime == '' || $scope.PlugOUTDatetime == null || $scope.PlugOUTDatetime == 'undefined') {
                flag = false;
              
                $('#spPlugOUTDatetime').text('Fill this field');
            }
            else {
                flag = true;
               
                $('#spPlugOUTDatetime').text('');
            }

            if ($scope.AllContainerList.length == 0)
            {
                flag = false;
                alert('Select Atleast One Container')
               
            }
            else {
                flag = true;
            }

            if ($('#BtnAdd').css('display') == 'block') {
                flag = false;
                alert('Please Add Container')
            }
            else {
                flag = true;
            }


            return flag;
        }


        $scope.hideModal=function()
        {
            $('#Containerbox').val('');
            $('#ContainerModal').modal('hide');
        }
    });
})();