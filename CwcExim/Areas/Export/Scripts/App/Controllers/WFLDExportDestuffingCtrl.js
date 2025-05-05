(function () {
    angular.module('CWCApp').
    controller('ExportDestuffingCtrl', function ($scope, ExportDestuffingService) {
        $scope.ContArray = [];
        $scope.SelectedContainer = {};

       /*
        public int ContainerStuffingId { get; set; }
        public int ContainerStuffingDtlId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        */

        $scope.Message = '';
        var c = new ContainerDetails();
        $scope.SelectedContainer = c;

        $scope.ContainersList = JSON.parse($('#hdnContainersList').val());
        $scope.ShippingLineList = JSON.parse($('#hdnShippingLineList').val());
        $scope.CHAList = JSON.parse($('#hdnCHAList').val());
        $scope.PartyList = JSON.parse($('#hdnPaymentParty').val());
        $scope.Rights = JSON.parse($("#hdnRights").val());

        $scope.selectContainer = function (obj) {
            $scope.SelectedContainer = obj;
            /*
            public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; }
        public string Size { get; set; }
        public string ArrivalDt { get; set; }
        public string IsHaz { get; set; }
            */
            $scope.ContArray.push({
                CFSCode: $scope.SelectedContainer.CFSCode,
                ContainerNo: $scope.SelectedContainer.ContainerNo,
                Selected: true,
                Size: '',
                ArrivalDt: '',
                IsHaz: ''
            });
            ExportDestuffingService.ContainerSelect(0, $('#InvoiceDate').val(), $scope.SelectedContainer.ContainerStuffingDtlId, TaxType, $scope.ContArray).then(function (res) {

                $scope.InvoiceObj = res.data;
                //$scope.IsContSelected = true;
                console.log($scope.InvoiceObj);



                if ($scope.Rights.CanAdd == 1) {
                    $('#btnSave').removeAttr("disabled");
                }
                $('.search').css('display', 'none');
                $('#InvoiceDate').parent().find('img').css('display', 'none');


            });
            //$scope.getCharges();

            $('#Containerdtl').modal('hide');
        }

        $scope.selectShippingLine = function (obj) {
            $scope.SelectedContainer.ShippingLineId = obj.ShippingLineId;
            $scope.SelectedContainer.ShippingLineName = obj.ShippingLine;
            $('#ShippingLineModal').modal('hide');
        }

        $scope.selectCHA = function (obj) {
            $scope.SelectedContainer.CHAId = obj.CHAEximTraderId;
            $scope.SelectedContainer.CHAName = obj.CHAName;
            $('#CHAModal').modal('hide');
        }

        $scope.selectParty = function (obj) {
            $scope.SelectedContainer.PartyId = obj.PartyId;
            $scope.SelectedContainer.PartyName = obj.PartyName;
            $('#PartyModal').modal('hide');
        }

        $scope.getCharges = function () {
            ExportDestuffingService.getDestufcharges($scope.SelectedContainer.ContainerStuffingId).then(function (res) {
                console.log(res.data);
                $scope.DestuffingObj = res.data;
            });
            console.log($scope.DestuffingObj);
        }
        
        $scope.SubmitInvoice = function () {

            
            /*
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
            */
            if ($scope.InvoiceObj.TotalAmt == 0) {
                $scope.Message = "Can not be saved. Invoice Amount is Zero.";
                return false;
            }


            if (confirm('Are you sure to Generate this Invoice?')) {
                $scope.InvoiceObj.InvoiceId = 0;
                $scope.InvoiceObj.InvoiceType = TaxType;
                /*
                $scope.InvoiceObj.PartyId = $scope.PartyId;
                $scope.InvoiceObj.PartyName = $scope.PartyName;
                $scope.InvoiceObj.PartyAddress = $scope.hdnAddress;
                $scope.InvoiceObj.PartyGST = $scope.GSTNo;
                $scope.InvoiceObj.PartyState = $scope.hdnState;
                $scope.InvoiceObj.PartyStateCode = $scope.hdnStateCode;

                $scope.InvoiceObj.PayeeId = $scope.PayeeId;
                $scope.InvoiceObj.PayeeName = $scope.PayeeName;
                */
                $scope.InvoiceObj.Remarks = $scope.Remarks;
                //console.log($scope.InvoiceObj);

                //var objfinal = $scope.InvoiceObj;



                ExportDestuffingService.GenerateInvoice($scope.InvoiceObj).then(function (res) {
                    console.log(res);
                    $scope.InvoiceNo = res.data.Data.InvoiceNo;
                    $scope.DestufNo = res.data.Data.ExportDestuffingNo;
                    $scope.Message = res.data.Message;

                    $('#btnSave').attr("disabled", true);
                    $('#btnPrint').removeAttr("disabled");
                });
            }
        }

    });
})()