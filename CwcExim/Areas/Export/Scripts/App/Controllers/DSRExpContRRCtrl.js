(function () {
    angular.module('CWCApp').
    controller('ExpContRRCtrl', function ($scope, ExpContRRService) {
        $scope.StuffingReqList = [];
        $scope.ShippingLine = [];
        if ($('#StuffingReqList').val() != null && $('#StuffingReqList').val() != undefined && $('#StuffingReqList').val() !='')
            $scope.StuffingReqList = JSON.parse($('#StuffingReqList').val());
        if ($('#hdnShippingLine').val() != null && $('#hdnShippingLine').val() != undefined && $('#hdnShippingLine').val() != '')
            $scope.ShippingLine = JSON.parse($('#hdnShippingLine').val());
        $scope.InvoiceObj = {};
        $scope.PostInvoiceObj = {};
        $scope.InvoiceSelected = 0;
        $scope.IsSaved = 0;
        $scope.SelectInvoice = function (obj) {
            $scope.InvoiceId = obj.InvoiceId;
            $scope.InvoiceNo = obj.InvoiceNo;
            $scope.InvoiceDate = obj.InvoiceDate;

            ExpContRRService.GetInvoiceDetails($scope.InvoiceId).then(function (res) {
                if (res.data.Status == 1)
                {
                    $scope.InvoiceObj = res.data.Data;
                    $scope.InvoiceSelected = 1;
                    //console.log($scope.InvoiceObj);
                }
            });
            $('#InvoiceModal').modal('hide');
        }
        $scope.SelectShippingLine = function (obj) {
            $scope.ShippingLineName = obj.ShippingLineName;
            $scope.ShippingLineId = obj.ShippingLineId;
            ExpContRRService.GetPostInvoiceDetails($scope.InvoiceId, $scope.ShippingLineId).then(function (res) {
                if (res.data.Status == 1) {
                    $scope.PostInvoiceObj = res.data.Data;
                    $scope.PostInvoiceObj.InvoiceIdCRNote = $scope.InvoiceId;
                }
            });
            $('#ShippingLineModal').modal('hide');
        }
        $scope.AddEditExportRR = function()
        {
            var conf = confirm("Are you sure you want to Save?");
            if(conf==true)
            {
                ExpContRRService.AddEditExportRR($scope.PostInvoiceObj).then(function (res) {
                    if (res.data.Status == 1 || res.data.Status == 2) {
                        $scope.IsSaved = 1;
                        //console.log(res.data.Data);
                        var arr = JSON.parse(res.data.Data);
                        $scope.DebitInvoiceNo=arr.InvoiceNo;
                        $scope.CreditNoteNo = arr.CRNoteNo;
                        $scope.Message = res.data.Message;
                    }
                    else
                    {
                        $scope.Message = res.data.Message;
                    }
                });
            }
        }
    })
})()