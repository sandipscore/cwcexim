(function () {
    angular.module('CWCApp').
    controller('Dnd_CrInvLedgerCtrl', function ($scope, Dnd_CrInvLedgerService) {

        $scope.PartyList = JSON.parse($('#hdnPartyPayee').val());
        //$scope.Rights = JSON.parse($("#hdnRights").val());
        $scope.PartyName = '';
        $scope.PartyId = 0;
        $scope.OpenningBal = 0;
        $scope.ClosingBal = 0;
        $scope.FromDate = '';
        $scope.ToDate = '';
        $scope.Summary = [];
        $scope.Details = [];
        $scope.Details2 = [];
        $scope.Details3 = [];
        $scope.SelectParty = function (obj) {
            $scope.PartyName = obj.PartyName;
            $scope.PartyId = obj.PartyId;
            $('#PartyModal').modal('hide');
        }
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
            $scope.Details3 = [];

            Dnd_CrInvLedgerService.GetLedgerReport($scope.PartyId, $scope.FromDate, $scope.ToDate).then(function (res) {
                console.log(res.data);
                if (res.data.Status == 1) {
                    $scope.OpenningBal = res.data.Data.OpenningBalance;
                    $scope.ClosingBal = res.data.Data.ClosingBalance;
                    $scope.ComName = res.data.Data.CompanyName;
                    $scope.Address = res.data.Data.CompanyAddress;
                    $scope.Summary = res.data.Data.lstDndLedgerSummary;
                    $scope.Details = res.data.Data.lstDndLedgerDetails;

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

                    $scope.Details2 = res.data.Data.lstDndLedgerDetailsFull;
                    $scope.Details3 = res.data.Data.lstDndLedgerTotal;
                }
            });
        }
    });
})()