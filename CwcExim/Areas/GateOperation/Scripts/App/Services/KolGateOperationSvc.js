(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('KolGateOperationSvc', function ($http) {
        this.GetContainerList = function (gateindate) {
            return $http.get('/GateOperation/kol_CWCGateOperation/GetContainersForLWB/?GateInDate=' + gateindate);
        }
        this.GetLWBEntrydetails = function (id) {
            return $http.get('/GateOperation/kol_CWCGateOperation/GetLWBEntrydetails/?Id=' + id);
        }
        this.AddEditLWB = function (objLwb) {
            return $http({
                url: "/GateOperation/kol_CWCGateOperation/AddEditContainersForLWB",
                method: "POST",
                data: JSON.stringify(objLwb)
            });
        }
/*
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
        }*/
    });
})()