(function () {
    angular.module('CWCApp').
    controller('ChequeDepositViewCtrl', function ($scope, ChequeDepositSvc) {
        //$('#DivChequeDepositList').load('/CashManagement/Ppg_CashManagement/ListChequeDeposit');

        $scope.paymentModes = [
            { Text: "--- Select ---", Value: "" },
            //{ Text : "CASH", Value : "CASH"},
            //{ Text : "NEFT", Value : "NEFT"},
            { Text: "CHEQUE", Value: "CHEQUE" },
            { Text: "DRAFT", Value: "DRAFT" },
            { Text: "PO", Value: "PO" },
            // { Text : "CREDIT NOTE", Value : "CREDITNOTE"}
        ];

        $scope.details = new ChequeModel();
        //$scope.banks = JSON.parse($('#hdnBank').val());

        $scope.chequeDeposit = JSON.parse($('#hdnChqDetails').val())

        $scope.listDetails = [];
        $scope.IsAddClicked = false;
        $scope.IsSaveClicked = false;
        $scope.chqEditIndex = -1;

        $scope.DepositId = $scope.chequeDeposit.ChequeDepositId;
        $scope.DepositNo = $scope.chequeDeposit.ChequeDepositNo;
        $scope.DepositDate = $scope.chequeDeposit.EntryDate; //$('#hdnCurDate').val();
        $scope.listDetails = $scope.chequeDeposit.ChequeDetails;

        $scope.selectBank = function (obj) {
            $scope.details.BankId = obj.BankId;
            $scope.details.BankName = obj.BankName;
            $scope.details.AccountNo = obj.AccountNo;
            $('#BankModal').modal('hide');
        }

        $scope.ChequeAdd = function (obj) {
            $scope.IsAddClicked = true;
            if ($scope.details.ChequeDate == '' && $scope.details.BankId == 0 && $scope.details.AccountNo == ''
                && $scope.details.ChequeNo == '' && $scope.details.Mode == '' && ($scope.details.Amount == 0 || $scope.details.Amount == null)) {
                return false;
            }
            else {
                $scope.listDetails.push(obj);
                $scope.ResetCheque();
            }
        }
        $scope.ChequeEdit = function (i, obj) {
            $scope.details = obj;
            //$scope.listDetails.splice(i, 1);
            $scope.chqEditIndex = i;
        }

        $scope.ChequeDelete = function (i) {
            $scope.listDetails.splice(i, 1);
        }

        $scope.ResetCheque = function () {
            $scope.details = new ChequeModel();
            $scope.IsAddClicked = false;
            $scope.chqEditIndex = -1;
        }

        $scope.FullReset = function () {
            $('#DivBody').load('/CashManagement/Ppg_CashManagement/CreateChequeDeposit');
        }

        
    })
})()