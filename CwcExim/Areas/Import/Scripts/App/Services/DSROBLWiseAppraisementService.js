(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSROBLWiseAppraisementService', function ($http) {
        
        this.GetContainerDetails = function (OBLNo, OBLDate) {
            return $http({
                url: "/Import/DSR_CWCImport/GetOBLWiseContainer/",
                method: "GET",
                params: { OBLNo: OBLNo, OBLDate: OBLDate },
            });
        }
        
        this.GetContainerList = function (Type) {
            return $http({
                url: "/Import/DSR_OblEntry/GetTContainerNoForOBLEntryByType/",
                method: "GET",
                params: { CONTCBT: Type },
            });
        }
        

        this.AddEditOBLCustomAppraisement = function (Obj, ContAppraisement, CAOrdDtl) {
            return $http({
                url: "/Import/DSR_CWCImport/AddEditOBLCustomAppraisement/",
                method: "POST",
                data: { ObjAppraisement: Obj, ContAppraisement: ContAppraisement, CAOrdDtl: CAOrdDtl },
            });
        }

        this.GetContainerDetailsOnEditMode = function (CustomAppraisementId) {
            return $http({
                url: "/Import/DSR_CWCImport/GetContainerDetailsOnEditMode/",
                method: "GET",
                params: { CustomAppraisementId: CustomAppraisementId },
            });
        }

        this.LoadOBLCustomAppraisement = function (CustomAppraisementId) {
            return $http({
                url: "/Import/DSR_CWCImport/OBLWiseCustomAppraisement/",
                method: "GET",
                params: { CustomAppraisementId: CustomAppraisementId },
            });
        }

    });
})()