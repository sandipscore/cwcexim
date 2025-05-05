(function ($) {
    $.fn.dynamicYear = function (options) {
        var settings = $.extend({
            onChangeCallback: null,
            defaultYear: new Date().getFullYear()
        }, options );
        var thisYear = settings.defaultYear;
        var htmlStr = '';
        for (var i = (thisYear - 9) ; i < thisYear; i++) {
            htmlStr += '<option value=' + i + '>' + i + '</option>';
        }
        htmlStr += '<option value=' + thisYear + ' selected>' + thisYear + '</option>';
        for (var i = (thisYear + 1) ; i <= (thisYear + 10) ; i++) {
            htmlStr += '<option value=' + i + '>' + i + '</option>';
        }
        $(this).html(htmlStr);
        $(this).on('change', function () {
            var index = $(this).find('option:selected').index();
            if (index === 0 || index === 19) {
                var resetyear = Number($(this).val());
                var newOpt = '';
                for (var i = (resetyear - 9) ; i < resetyear; i++) {
                    newOpt += '<option value=' + i + '>' + i + '</option>';
                }
                newOpt += '<option value=' + resetyear + ' selected>' + resetyear + '</option>';
                for (var i = (resetyear + 1) ; i <= (resetyear + 10) ; i++) {
                    newOpt += '<option value=' + i + '>' + i + '</option>';
                }
                $(this).html(newOpt);
            }
            if (settings.onChangeCallback != null) {
                settings.onChangeCallback(this);
            }
        });
    };
})(jQuery);