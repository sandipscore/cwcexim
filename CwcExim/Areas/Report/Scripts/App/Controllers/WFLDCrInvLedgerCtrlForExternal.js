(function () {
    angular.module('CWCApp').
    controller('WFLDCrInvLedgerCtrl', function ($scope, WFLDCrInvLedgerService) {

      
        $scope.PartyName = '';
        $scope.PartyId = 0;
        $scope.OpenningBal = 0;
        $scope.ClosingBal = 0;
        $scope.FromDate = '';
        $scope.ToDate = '';
        $scope.Summary = [];
        $scope.Details = [];
        $scope.Details2 = [];

        $scope.PartyName = $('#PartyName').val();
        $scope.PartyId = $('#PartyId').val();


        $scope.generate = function () {

            $scope.OpenningBal = 0;
            $scope.ClosingBal = 0;

            $scope.ComName = '';
            $scope.Address = '';
            $scope.City = '';
            $scope.State = '';
            $scope.GSTNo = '';
            $scope.PinCode = '';
            $scope.COMGST = '';
            $scope.COMPAN = '';
            $scope.TotalDebit = '';
            $scope.TotalCredit = '';
            $scope.CurDate = '';

            $scope.Summary = [];
            $scope.Details = [];
            $scope.Details2 = [];


            WFLDCrInvLedgerService.GetLedgerReport($scope.PartyId, $scope.FromDate, $scope.ToDate).then(function (res) {
                console.log(res.data);
                if (res.data.Status == 1) {
                    $scope.OpenningBal = res.data.Data.OpenningBalance;
                    $scope.ClosingBal = res.data.Data.ClosingBalance;
                    $scope.ComName = res.data.Data.CompanyName;
                    $scope.Address = res.data.Data.CompanyAddress;
                    $scope.Summary = res.data.Data.lstLedgerSummary;
                    $scope.Details = res.data.Data.lstLedgerDetails;

                    $scope.Address = res.data.Data.Address;
                    $scope.City = res.data.Data.City;
                    $scope.State = res.data.Data.State;
                    $scope.GSTNo = res.data.Data.GSTNo;
                    $scope.PinCode = res.data.Data.PinCode;
                    $scope.COMGST = res.data.Data.COMGST;
                    $scope.COMPAN = res.data.Data.COMPAN;
                    $scope.TotalDebit = res.data.Data.TotalDebit;
                    $scope.TotalCredit = res.data.Data.TotalCredit;
                    $scope.CurDate = res.data.Data.CurDate;

                    $scope.Details2 = res.data.Data.lstLedgerDetailsFull;

                }
            });
        }
    });
})()