(function () {
    angular.module('CWCApp').
    controller('ChequeDepositCtrl', function ($scope, ChequeDepositSvc) {
       $('#DivChequeDepositList').load('/CashManagement/Ppg_CashManagement/ListChequeDeposit');

        $scope.paymentModes = [
            { Text : "--- Select ---", Value : ""},
            //{ Text : "CASH", Value : "CASH"},
            //{ Text : "NEFT", Value : "NEFT"},
            { Text : "CHEQUE", Value : "CHEQUE"},
            { Text : "DRAFT", Value : "DRAFT"},
            { Text : "PO", Value : "PO"},
            // { Text : "CREDIT NOTE", Value : "CREDITNOTE"}
        ];

        $scope.details = new ChequeModel();
        $scope.banks = JSON.parse($('#hdnBank').val());
        $scope.DepositDate = $('#hdnCurDate').val();
        $scope.Rights = JSON.parse($("#hdnRights").val());
        
        $scope.listDetails = [];
        $scope.IsAddClicked = false;
        $scope.IsSaveClicked = false;
        $scope.chqEditIndex = -1;
        

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
            $scope.listDetails.splice(i, 1);
            $scope.chqEditIndex = i;
        }

        $scope.ChequeDelete = function (i) {
            $scope.listDetails.splice(i,1);
        }

        $scope.ResetCheque = function () {
            $scope.details = new ChequeModel();
            $scope.IsAddClicked = false;
            $scope.chqEditIndex = -1;
        }

        $scope.FullReset = function () {
            $('#DivBody').load('/CashManagement/Ppg_CashManagement/CreateChequeDeposit');
        }

        $scope.Save = function () {
            $scope.IsSaveClicked = true;
            if ($scope.listDetails.length<=0) {
                return false;
            }
            if ($scope.chqEditIndex >=0) {
                $scope.Message = "Complete Edit Operation Before Save.";
                return false;
            }
            if (confirm("Are you sure to Save? ")) {
                var obj = {
                    ChequeDepositId: 0,
                    ChequeDepositNo: '',
                    EntryDate: $scope.DepositDate,
                    ChequeDetails: $scope.listDetails
                }
                //console.log(obj);
                ChequeDepositSvc.SaveEntry(obj).then(function (res) {
                    if (res.data.Status == 1) {
                        $scope.DepositNo = res.data.Data;
                        $('#DivChequeDepositList').load('/CashManagement/Ppg_CashManagement/ListChequeDeposit');
                    }
                    else {
                        //alert(res.data.Message);
                    }
                    $scope.Message = res.data.Message;
                });
            }
        }
    })
})()