(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('ContIndentService', function ($http) {
        this.SelectForm1 = function (ReqNo) {
            return $http.get('/Import/Hdb_CWCImport/GetCntrDetForForm1/?IndentId=' + ReqNo);
        }

        this.AddContainerIndent = function (FormOneId, IndentDate, TrailerNo, ICDIn, ICDOut, Remarks) {
            return $http({
                url: "/Import/Hdb_CWCImport/AddEditContainerIndent/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType },
                data: JSON.stringify(conatiners)
            });
        }

        this.AddContainerIndent = function (FormOneId, IndentDate, TrailerNo, ICDIn, ICDOut, Remarks) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Import/Hdb_CWCImport/AddEditContainerIndent/",
                method: "POST",
                params: { Form1Id: FormOneId, IndentDate: IndentDate, TrailerNo: TrailerNo, ICDIn: ICDIn, ICDOut: ICDOut, Remarks: Remarks },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
    });
})()