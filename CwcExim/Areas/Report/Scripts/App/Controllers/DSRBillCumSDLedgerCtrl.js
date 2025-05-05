(function () {
    angular.module('CWCApp').
    controller('DSRBillCumSDLedgerCtrl', function ($scope, DSRBillCumSDLedgerSvc) {

        $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        $scope.Rights = JSON.parse($("#hdnRights").val());
        $scope.PartyName = '';
        $scope.PartyCode = '';
        $scope.PartyId = 0;
        $scope.OpenningBal = 0;
        $scope.ClosingBal = 0;
        $scope.FromDate = '';
        $scope.ToDate = '';
        $scope.Details = [];

        $scope.SelectParty = function (obj) {
            $scope.PartyName = obj.PartyName;
            $scope.PartyId = obj.PartyId;
            $('#PartyModal').modal('hide');
        }
        $scope.generate = function () {
            $scope.OpenningBal = 0;
            $scope.ClosingBal = 0;
            $scope.ComName = '';
            $scope.PartyCode = '';
            $scope.Details = [];
            $scope.COMGST = '';
            $scope.COMPAN = '';
            $scope.CurDate = '';

            DSRBillCumSDLedgerSvc.GetLedgerReport($scope.PartyId, $scope.FromDate, $scope.ToDate).then(function (res) {
                
                if (res.data.Status == 1) {
                    $scope.OpenningBal = res.data.Data.OpenningBalance;
                    $scope.ClosingBal = res.data.Data.ClosingBalance;
                    $scope.ComName = res.data.Data.CompanyName;
                    $scope.PartyCode = res.data.Data.EximTraderAlias;
                    //$scope.Address = res.data.Data.CompanyAddress;
                    //$scope.Summary = res.data.Data.lstLedgerSummary;
                    $scope.Details = res.data.Data.lstDetails;

                    //$scope.Address = res.data.Data.Address;
                    //$scope.City = res.data.Data.City;
                    //$scope.State = res.data.Data.State;
                    //$scope.GSTNo = res.data.Data.GSTNo;
                    //$scope.PinCode = res.data.Data.PinCode;
                    $scope.COMGST = res.data.Data.COMGST;
                    $scope.COMPAN = res.data.Data.COMPAN;
                    //$scope.TotalDebit = res.data.Data.TotalDebit;
                    //$scope.TotalCredit = res.data.Data.TotalCredit;
                    $scope.CurDate = res.data.Data.CurDate;

                    //$scope.Details2 = res.data.Data.lstLedgerDetailsFull;
                    console.log($scope.Details);
                }
            });
        }
    });
})()