(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('Ppg_CargoShiftingApprSrvc', function ($http) {
        this.CargoShiftingNoForApproval = function ()
        {
            return $http.get('/Export/Ppg_CWCExport/CargoShiftingNoForApproval');
        }
        this.CargoShiftingDetForApproval = function (CargoShiftingId)
        {
            return $http.get('/Export/Ppg_CWCExport/CargoShiftingDetForApproval?CargoShiftingId=' + CargoShiftingId);
        }
        this.AddEditCargoShiftingApproval = function (CargoShiftingId, IsApproved, ShiftingDt) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/Ppg_CWCExport/AddEditCargoShiftingApproval/",
                method: "POST",
                data: { CargoShiftingId: CargoShiftingId, IsApproved: IsApproved, ShiftingDate: ShiftingDt },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        this.ShowList = function ()
        {
            return $http.get('/Export/Ppg_CWCExport/ListofCargoShiftingApproved');
        }
    });
})()