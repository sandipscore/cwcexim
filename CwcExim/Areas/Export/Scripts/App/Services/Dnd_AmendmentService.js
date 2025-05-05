(function () {
    angular.module('CWCApp')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .service('Dnd_AmendmentService', function ($http) {
        this.GetSBDetails = function (SBNo, SBDate) {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetSBDetailsBySBNo/?SBid=' + SBNo + '&SbDate=' + SBDate + '');
        }

        this.GetAllCommodityName=function(CommodityName,Page)
        {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetAllCommodityDetailsForAmendmend/?CommodityName=' + CommodityName+'&Page='+Page);
        }
        this.GetAllChaName = function (CHAName, Page) {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetAllChaDetailsForAmendmend/?ChaName=' + CHAName + '&Page=' + Page);
        }

        /*this.GetAmendDetails = function (AmendNo) {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetAmendDetails/?AmendNo=' + AmendNo);
        }*/
        this.GetAmendDetailsByAmendNo = function (AmendNo) {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetAmendDetailsByAmendNo/?AmendNo=' + AmendNo);
        }

        this.loadMoreCommodity = function (partyCode, pageNo) {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetAllCommodityDetailsForAmendmend?CommodityName=' + partyCode + '&Page=' + pageNo);
        }
        this.loadMoreCha = function (partyCode, pageNo) {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetAllChaDetailsForAmendmend?CHAName=' + partyCode + '&Page=' + pageNo);
        }
        this.saveAmendment = function (OldInfoSBNo, NewInfoSBNo, Date, AmendmentNO, InvoiceId, InvoiceNo, InvoiceDate, FlagMerger) {
            debugger;
            //  var datapost = [];
            // datapost.push({})
            var postData = JSON.stringify({ 'vm': OldInfoSBNo, 'newvm': NewInfoSBNo });

            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/Dnd_CWCExport/SaveAmendement/",
                method: "POST",
                data: postData,
                params: { Date: Date, InvoiceId: InvoiceId, InvoiceNo: InvoiceNo, InvoiceDate: InvoiceDate, FlagMerger: FlagMerger },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }

        this.savetabAmendment = function (vm) {
            debugger;
            //  var datapost = [];
            // datapost.push({})
            //var postData = JSON.stringify({ 'vm': OldInfoSBNo, 'newvm': NewInfoSBNo });

            debugger;
            var Token = $('input[name="__RequestVerificationToken"]').val();
            return $http({
                url: "/Export/Dnd_CWCExport/SubmitAmedData/",
                method: "POST",
                data: vm,
                // params: { Date: Date },
                headers: {
                    '__RequestVerificationToken': Token
                }
            });
        }


        this.GetAmendLst = function () {
            debugger;
            return $http.get('/Export/Dnd_CWCExport/GetAmendList/');
        }

    });
})()