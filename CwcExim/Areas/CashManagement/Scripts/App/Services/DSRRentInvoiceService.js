(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('DSRRentInvoiceService', function ($http) {
        this.GetYardInvoiceDetails = function (InvoiceId) {
            return $http.get('/CashManagement/DSR_CashManagement/GetYardInvoiceDetails/?InvoiceId=' + InvoiceId);
        }
        this.ContainerSelect = function (InvoiceId, InvoiceDate, AppraisementId, TaxType, conatiners) {
            return $http({
                url: "/Import/DSR_CashManagement/GetContainerPaymentSheet/?InvoiceId=" + InvoiceId,
                method: "POST",
                params: { InvoiceDate: InvoiceDate, AppraisementId: AppraisementId, TaxType: TaxType },
                data: JSON.stringify(conatiners)
            });
        }
        this.GenerateIRNNo = function (InvoiceNo, SupplyType) {
            return $http({
                url: "/Import/DSR_CWCImport/GetIRNForYardInvoice",
                method: "POST",
                params: { InvoiceNo: InvoiceNo, SupplyType: SupplyType },

            });
        }

        this.ResetField = function () {
            return $http({
                url: "/CashManagement/DSR_CashManagement/CreateRentInvoice",
                method: "POST",
                params: { }

            });
        }

        this.PopulateMonthYear = function (month, year, flg) {
            return $http({
                url: "/CashManagement/DSR_CashManagement/GetMonthYearRentData",
                method: "POST",
                params: { Month: month, Year: year, Flag: flg }

            });
        }


        this.GenerateInvoice = function (InvoiceObj, mon, yr, vm) {
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/CashManagement/DSR_CashManagement/AddEditRentInvoice/",
                method: "POST",
                params: { Month: mon, Year: yr },
                data: JSON.stringify(InvoiceObj),
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.GeneratePrint = function (InvoiceObj) {
            debugger;
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!

            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var today = dd + '/' + mm + '/' + yyyy;

            var dt1 = today;
            var dt2 = today;
            // for (i = 0; i < InvoiceObj.length; i++) {
            var invno = InvoiceObj[0].InvoiceNo;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Report/DSR_ReportCWC/GetBulkInvoiceReport/",
                method: "POST",
                data: {
                    InvoiceNumber: invno,
                    InvoiceModule: 'Rent',
                    PeriodFrom: dt1,
                    PeriodTo: dt2,
                    InvoiceModuleName: 'Rent',
                },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }
        //}
    });
})()




