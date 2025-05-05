(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRHTChargesService', function ($http) {
        //this.GetSlab = function (Size, ChargesFor, OperationCode) {
        //    debugger;
        //    return $http.get("/Master/DSRMaster/GetSlabData?Size=" + Size + "&ChargesFor=" + ChargesFor + "&OperationCode=" + OperationCode);
        //}
        this.SaveHTCharges = function (Obj, ChargeList) {
            if (confirm("Are you sure you want to save?")) {
                //var Token = $('input[name="__RequestVerificationToken"]').val();
                console.log(Obj);
                return $http({
                    url: "/Master/DSRMaster/AddEditHTCharges/",
                    method: "POST",
                    data: { objCharges: Obj, ChargeList: JSON.stringify(ChargeList) },
                    //params: { ChargeList: JSON.stringify(ChargeList) }
                    //headers: {
                    //    '__RequestVerificationToken': Token
                    //}
                });
            }
        }
        this.EditHTChargesOther = function (HTChargesId) {
            return $http.get("/Master/DSRMaster/EditHTChargesOther/?HTChargesId=" + HTChargesId);

        }
        this.GetAllHTCharges = function () {
            return $http.get("/Master/DSRMaster/GetAllHTCharges");
        }
        this.GetSlabChargeDtl = function (HTChargesId) {
            return $http.get("/Master/DSRMaster/GetHTSlabChargesDtl/?HTChargesId=" + HTChargesId);
        }
    });
})()