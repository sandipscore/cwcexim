var operatorFunction = {
    '+' : function(x, y) {
        return x + y;
    },
    '-' : function(x, y) {
        return x - y;
    },
    '*' : function(x, y) {
        return x * y;
    },
        '/' : function(x, y) {
        return x / y;
    }
};

function accumul(list1,list, operator) {
   var sum = operator=='+'?0:operator=='-'?(Number($('#'+list[0]).val())*2):operator=='*'?1:operator=='/'?(Number($('#'+list[0]).val())*Number($('#'+list[0]).val())):0;
    list.forEach(function(item) {
        var value=Number($('#'+item).val());
        sum = operatorFunction[operator](sum, value);
    });
    // $(element).val(sum);
    list1.forEach(function (item) {
        $('#' + item).val(sum);
    });

}