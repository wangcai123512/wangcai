var SelectConfigSetting = {
    buttonWidth: '100%',
    maxHeight: '100%',
    enableFiltering: false,
    includeFilterNewBtn: false,
    nonSelectedText: '请选择'    

};

function InitCurrency(selector, defaultItem) {
    $(selector).multiselect(SelectConfigSetting);
    $(selector).multiselect('dataprovider', currency);
    $(selector).val("").multiselect("refresh");
    if (defaultItem == "") {
        $(selector).multiselect('select', standardCoin);
    } else {
        $(selector).multiselect('select', defaultItem);
    }

}


//function SumAmount(selector1,selector2,selector3)
//{
//    var p1 = $(selector1).val();
//    var p2 = $(selector2).val();

//    if (p1 == "")
//    {
//        p1 = 0;
//    }
//    if (p2 == "") {
//        p2 = 0;
//    }
//    p1 = parseFloat(p1.replace(/,/g, ""));
//    p2 = parseFloat(p2.replace(/,/g, ""));

//    $(selector3).val(Money(p1+p2));
//}



$(document).ready(function () {

    var url = window.location.pathname;
    $("li").removeClass("active");
    var ctl = url.substr(0, url.indexOf("/", 1));

    $("[href^='" + ctl + "']").parent().addClass("active");

   // $('select').multiselect(SelectConfigSetting);

})


var DecimalFmter = function (s) {
    if (s == null || s == "undefined"||s==0) {
        return "";
    }
    var h = '';
    s = s.toString();
    if (s.charAt(0) == '-') {
        h = '-';
        s = s.slice(1);
    }
    if (/[^0-9\.]/.test(s)) return "NaN";
    s = s.replace(/^(\d*)$/, "$1.");
    s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
    s = s.replace(".", ",");
    var re = /(\d)(\d{3},)/;
    while (re.test(s)) s = s.replace(re, "$1,$2");
    s = s.replace(/,(\d\d)$/, ".$1");
    return h + s.replace(/^\./, "0.");

}
var DecimalFmter1 = function (s, row, index) {
    var startAmount = 0;
    var endAmount = 0;
    if (row.asset_start_amount == null || row.asset_start_amount == "undefined" || row.asset_start_amount == 0) {
        startAmount = 0;
    } else {
        startAmount = row.asset_start_amount;
    } if (row.asset_end_amount == null || row.asset_end_amount == "undefined" || row.asset_end_amount == 0) {
        endAmount = 0;
    } else {
        endAmount = row.asset_end_amount;
    }

    s = startAmount + endAmount;
    if (s == 0) {
        return "";
    }
    var h = '';
    s = s.toString();
    if (s.charAt(0) == '-') {
        h = '-';
        s = s.slice(1);
    }
    if (/[^0-9\.]/.test(s)) return "NaN";
    s = s.replace(/^(\d*)$/, "$1.");
    s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
    s = s.replace(".", ",");
    var re = /(\d)(\d{3},)/;
    while (re.test(s)) s = s.replace(re, "$1,$2");
    s = s.replace(/,(\d\d)$/, ".$1");
    return h + s.replace(/^\./, "0.");

}
var DecimalFmter2 = function (s, row, index) {
    var startAmount = 0;
    var endAmount = 0;
    if (row.debt_start_amount == null || row.debt_start_amount == "undefined" || row.debt_start_amount == 0) {
        startAmount = 0;
    } else {
        startAmount = row.debt_start_amount;
    } if (row.debt_end_amount == null || row.debt_end_amount == "undefined" || row.debt_end_amount == 0) {
        endAmount = 0;
    } else {
        endAmount = row.debt_end_amount;
    }

    s = startAmount + endAmount;
    if (s == 0) {
        return "";
    }
    var h = '';
    s = s.toString();
    if (s.charAt(0) == '-') {
        h = '-';
        s = s.slice(1);
    }
    if (/[^0-9\.]/.test(s)) return "NaN";
    s = s.replace(/^(\d*)$/, "$1.");
    s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
    s = s.replace(".", ",");
    var re = /(\d)(\d{3},)/;
    while (re.test(s)) s = s.replace(re, "$1,$2");
    s = s.replace(/,(\d\d)$/, ".$1");
    return h + s.replace(/^\./, "0.");

}
var ChinaCost = function (numberValue) {
    if (!isNaN(numberValue) && numberValue !==null) {
        var numberValue = new String(Math.round(numberValue * 100)); // 数字金额
        var chineseValue = ""; // 转换后的汉字金额
        var String1 = "零壹贰叁肆伍陆柒捌玖"; // 汉字数字
        var String2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; // 对应单位
        var len = numberValue.length; // numberValue 的字符串长度
        var Ch1; // 数字的汉语读法
        var Ch2; // 数字位的汉字读法
        var nZero = 0; // 用来计算连续的零值的个数
        var String3; // 指定位置的数值
        if (len > 15) {
            alert("超出计算范围");
            return "";
        }
        if (numberValue == 0) {
            chineseValue = "零元整";
            return chineseValue;
        }

        String2 = String2.substr(String2.length - len, len); // 取出对应位数的STRING2的值
        for (var i = 0; i < len; i++) {
            String3 = parseInt(numberValue.substr(i, 1), 10); // 取出需转换的某一位的值
            if (i != (len - 3) && i != (len - 7) && i != (len - 11) && i != (len - 15)) {
                if (String3 == 0) {
                    Ch1 = "";
                    Ch2 = "";
                    nZero = nZero + 1;
                }
                else if (String3 != 0 && nZero != 0) {
                    Ch1 = "零" + String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else {
                    Ch1 = String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
            }
            else { // 该位是万亿，亿，万，元位等关键位
                if (String3 != 0 && nZero != 0) {
                    Ch1 = "零" + String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else if (String3 != 0 && nZero == 0) {
                    Ch1 = String1.substr(String3, 1);
                    Ch2 = String2.substr(i, 1);
                    nZero = 0;
                }
                else if (String3 == 0 && nZero >= 3) {
                    Ch1 = "";
                    Ch2 = "";
                    nZero = nZero + 1;
                }
                else {
                    Ch1 = "";
                    Ch2 = String2.substr(i, 1);
                    nZero = nZero + 1;
                }
                if (i == (len - 11) || i == (len - 3)) { // 如果该位是亿位或元位，则必须写上
                    Ch2 = String2.substr(i, 1);
                }
            }
            chineseValue = chineseValue + Ch1 + Ch2;
        }

        if (String3 == 0) { // 最后一位（分）为0时，加上“整”
            chineseValue = chineseValue + "整";
        }

        return '合计：' + chineseValue;
    }
    else {
        return numberValue;
    }


}
function Money(price) {
    if (price > 0) {
        price = parseFloat(price).toFixed(2);
        var priceString = price.toString();
        var priceInt = parseInt(price);
        var len = priceInt.toString().length;
        var num = len / 3;
        var remainder = len % 3;
        var priceStr = '';
        for (var i = 1; i <= len; i++) {
            priceStr += priceString.charAt(i - 1);
            if (i == (remainder) && len > remainder) priceStr += ',';
            if ((i - remainder) % 3 == 0 && i < len && i > remainder) priceStr += ',';
        }
        if (priceString.indexOf('.') < 0) {
            priceStr = priceStr + '.00';
        } else {
            priceStr += priceString.substr(priceString.indexOf('.'));
            if (priceString.length - priceString.indexOf('.') - 1 < 2) {
                priceStr = priceStr + '0';
            }
        }
        return priceStr;
    } else {
        return price;
    }
}

/** 
 ** 加法函数，用来得到精确的加法结果 
 ** 说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。 
 ** 调用：accAdd(arg1,arg2) 
 ** 返回值：arg1加上arg2的精确结果 
 **/
function accAdd(arg1, arg2) {
    if (!arg1) {
        arg1 = 0;
    }
    if (!arg2) {
        arg2 = 0;
    }
    arg1 = arg1.toString().replace(/\,/g, "");
    arg2 = arg2.toString().replace(/\,/g, "");
    
    var r1, r2, m, c;
    try {
        r1 = arg1.toString().split(".")[1].length;
    }
    catch (e) {
        r1 = 0;
    }
    try {
        r2 = arg2.toString().split(".")[1].length;
    }
    catch (e) {
        r2 = 0;
    }
    c = Math.abs(r1 - r2);
    m = Math.pow(10, Math.max(r1, r2));
    if (c > 0) {
        var cm = Math.pow(10, c);
        if (r1 > r2) {
            arg1 = Number(arg1.toString().replace(".", ""));
            arg2 = Number(arg2.toString().replace(".", "")) * cm;
        } else {
            arg1 = Number(arg1.toString().replace(".", "")) * cm;
            arg2 = Number(arg2.toString().replace(".", ""));
        }
    } else {
        arg1 = Number(arg1.toString().replace(".", ""));
        arg2 = Number(arg2.toString().replace(".", ""));
    }
    return (arg1 + arg2) / m;
}

//给Number类型增加一个add方法，调用起来更加方便。  
Number.prototype.add = function (arg) {
    return accAdd(arg, this);
};

//凭证导出
//function GetPdfs(a, b, c) {
//    //alert(b);exit;
//    var companyname = '核算单位：' + a;
//    var myTable = $("#VoucherTable");
//    var currency = '本币：' + b + '单位：元';
//    var jizhang = '记账：' + c;
//    var zhidan = '制单：' + c;
//    // 获取title
//    var tableThs = myTable.find("thead th");
//    //获取每个tr
//    var tableTrs = myTable.find("tbody tr");
//    var columns = [];
//    //处理title数组
//    tableThs.each(function () {
//        columns.push({ title: $(this).text(), key: $(this).text() });
//    });
//    //处理数据数组
//    var data = [];
//    tableTrs.each(function () {
//        var tds = $(this).children();
//        var object = {};
//        //生成数据对象
//        $.each(columns, function (i, r) {
//            var tdTitle = columns[i].key;
//            //'object'跟上文对象名称一致，动态件加属性和值
//            eval('object.' + tdTitle + '="' + $(tds).eq(i).text() + '"');
//        });
//        data.push(object);
//    });

    
//    var doc = new jsPDF('p', 'pt');
//    doc.addFont('simhei.ttf', 'simhei', 'normal');
//    doc.setFont('simhei');
//    doc.autoTableText('记账凭证', 260, 5, { styles: { cellPadding: 0.5, fontSize: 10, font: "msyh" } });
//    doc.autoTableText(currency, 400, 15, { styles: { cellPadding: 0.5, fontSize: 4, font: "msyh" } });
//    doc.autoTableText(companyname, 40, 35, { styles: { cellPadding: 0.5, fontSize: 4, font: "msyh" } });
//    doc.autoTableText('财务主管：', 10, 200, { styles: { cellPadding: 0.5, fontSize: 4, font: "msyh" } });

//    doc.autoTableText(jizhang, 120, 200, { styles: { cellPadding: 0.5, fontSize: 4, font: "msyh" } });

//    doc.autoTableText('复核：', 250, 200, { styles: { cellPadding: 0.5, fontSize: 4, font: "msyh" } });
//    doc.autoTableText('出纳：', 300, 200, { styles: { cellPadding: 0.5, fontSize: 4, font: "msyh" } });

//    doc.autoTableText(zhidan, 400, 200, { styles: { cellPadding: 0.5, fontSize: 4, font: "msyh" } });

//    doc.autoTableText('经办人：', 500, 200, { styles: { cellPadding: 0.5, fontSize: 4, font: "msyh" } });
//    doc.autoTable(columns, data, { styles: { cellPadding: 1, fontSize: 10, font: "msyh" }, bodyStyles: { font: "msyh" }, margin: [60, 40, 40, 40] });

//    doc.save("会计凭证" + ".pdf");
//}