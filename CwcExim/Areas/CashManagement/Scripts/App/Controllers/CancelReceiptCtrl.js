(function () {
    var angularapp = angular.module('CWCApp');
  /*  angularapp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }]);*/
    angularapp.controller('CancelReceiptController', function ($scope, CancelReceiptService, $timeout) {
        $scope.Message = '';
        $scope.Receiptlist =JSON.parse($('#hdnReceiptlist').val());
        $scope.SelectReceiptNo = function (obj) {
            $scope.SelectedReceipt = obj;
            $scope.ReceiptNo = obj.ReceiptNo;
            $scope.ReceiptId = obj.ReceiptId;
            $('#ReceiptModal').modal('hide');
            CancelReceiptService.GetReceiptDetails($scope.ReceiptId).then(function (data) {
                $scope.ReceiptDate = '';
                $scope.PartyId = 0;
                $scope.PartyName = '';
                $scope.Amount = '';
                $scope.Mode = '';
                $scope.No = '';
                if (data.data.Data != null||data.data.Data!=undefined)
                {
                    $scope.ReceiptDate = data.data.Data.ReceiptDate;
                    $scope.PartyId = data.data.Data.PartyId;
                    $scope.PartyName = data.data.Data.PartyName;
                    $scope.Amount = data.data.Data.Total;
                    $scope.Mode = data.data.Data.Mode;
                    $scope.No = data.data.Data.No;
                }
                
            });
        }
        $scope.SubmitReceipt = function ()
        {
            var conf = confirm("Are you sure you want to Save");
            if(conf==true)
            {
                var PostDate={
                    ReceiptId:$scope.ReceiptId,
                    CancelledReason:$scope.CancelledReason
                };
                var Token = $('input[name="__RequestVerificationToken"]').val();
                CancelReceiptService.SubmitReceiptDetails(PostDate).then(function (res) {
                    if (res.data.Status == 1)
                    {
                        $scope.Message = res.data.Message;
                        $scope.ReceiptId = '';
                        $scope.ReceiptNo = '';
                        $scope.ReceiptDate = '';
                        $scope.PartyId = '';
                        $scope.PartyName = '';
                        $scope.Amount = '';
                        $scope.Mode = '';
                        $scope.No = '';
                        $scope.CancelledReason = '';
                        $scope.Receiptlist.splice($scope.Receiptlist.indexOf($scope.SelectedReceipt), 1);
                        $scope.SelectedReceipt = {};
                        $('#DivListCancel').load('/CashManagement/Ppg_CashManagement/ListOfCancelledReceipt');
                        $timeout(function () { $scope.Message = ''; }, 5000);
                    }
                });
            }
        }
        $scope.ResetField=function()
        {
            $scope.Message = '';
            $scope.ReceiptId = '';
            $scope.ReceiptNo = '';
            $scope.ReceiptDate = '';
            $scope.PartyId = '';
            $scope.PartyName = '';
            $scope.Amount = '';
            $scope.Mode = '';
            $scope.No = '';
            $scope.CancelledReason = '';
            $scope.SelectedReceipt = {};
        }
    });
})();