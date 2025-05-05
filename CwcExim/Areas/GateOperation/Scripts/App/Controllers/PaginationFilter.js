(function () {

    angular.module('CWCApp')
        .filter('range', function () {
            return function (input, total) {

                var _total = 1;
                if (total !== null) {
                    if (!isNaN(total)) {
                        _total = total;
                    }
                }

                _total = Math.ceil(_total);

                for (var i = 1; i < _total + 1; i++) {
                    input.push(i);
                }

                return input;
            };
        })

    .filter('cust_pagination', function () {
        return function (items, pageSize, step) {

            if (pageSize == null) {
                pageSize = 9;
            }

            var nPageSize = items.length;
            var nStep = 0;
            if (!isNaN(pageSize) && pageSize) {
                nPageSize = pageSize;

                if (!isNaN(step) && step) {
                    nStep = step - 1;
                }
            }

            if (nPageSize > 0) {
                var startIndex = nStep * nPageSize;
                var endIndex = startIndex + nPageSize;
                var arr = items.slice(startIndex, endIndex);
                return arr;
            } else {
                return items;
            }
        }
    })
})()