function PageChange(path) {
    $("#main").panel(
        {
            href: path
        });
    }

    var ShowTitle = function () {
        $("#InsideTitle").text($("#title").val());
    };

    var fun = function (i, n) {
        var opt = $(n).validatebox("options").validType;
        if (opt == null) {
            opt = new Array();
        }
        else {

        }
        opt.push("spChar");
        $(n).validatebox("options").validType = opt;
    };

    var txtVaild = function () {
        $(".easyui-validatebox[type!='file']").each(fun);
    };

    function GenerateDateString() {
        var date = new Date();
        var strDate = date.getFullYear().toString()
        + pad(date.getMonth(), 2)
        + pad(date.getDate(), 2)
        + pad(date.getHours(), 2)
        + pad(date.getMinutes(), 2)
        + pad(date.getSeconds(), 2);
        return strDate;
    }
    
    function pad(num, n) {
        var len = num.toString().length;
        while (len < n) {
            num = "0" + num;
            len++;
        }
        return num;
    }

    function ChangeDateFormat(jsondate) {
        jsondate = jsondate.replace("/Date(", "").replace(")/", "");
        if (jsondate.indexOf("+") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("+"));
        }
        else if (jsondate.indexOf("-") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("-"));
        }

        var date = new Date(parseInt(jsondate, 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }

    $.fn.serializeObject = function () {
        var obj = new Object();
        $.each(this.serializeArray(), function (index, param) {
            if (!(param.name in obj)) {
                obj[param.name] = param.value;
            }
        });
        return obj;
    };

    $.fn.serializeObjects = function () {
        var obj = new Object();
        $.each(this.serializeArray(), function (index, param) {
            if (param.name != 'Date') {
                if (!(param.name in obj)) {
                    obj[param.name] = param.value;
                }
            }

        });
        return obj;
    };
    function InitLinkBtn() {
        $('.linkbtn').css("color", "#074592");
        $('.linkbtn').linkbutton({ plain: true });
    }

    $.extend($.fn.validatebox.defaults.rules, {
        spChar: {
            validator: function (value, param) {
                var reg = /\\/;
                return reg.exec(value) >= 0;
            },
            message: 'Please enter a valid Value.'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        pwd: {
            validator: function (value, param) {
                var reg = /^(?![0-9]+$)(?![A-Za-z]+$)[^\s]{6,}$/;
                return reg.test(value);
            },
            message: 'Password length of at least 6, and contain both letters and numbers.'
        }
    });

    var errorHandle = function (msg) {
        $.messager.alert('Error', msg.statusText, 'info');
    };

    var DecimalFmter = function (s) {
        if (s == null || s == "undefined") {
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


    //为Date类型拓展一个format方法，用于格式化日期  
    Date.prototype.format = function (format) //author: meizz   
    {
        var o = {
            "M+": this.getMonth() + 1, //month   
            "d+": this.getDate(),    //day   
            "h+": this.getHours(),   //hour   
            "m+": this.getMinutes(), //minute   
            "s+": this.getSeconds(), //second   
            "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter   
            "S": this.getMilliseconds() //millisecond   
        };
        if (/(y+)/.test(format))
            format = format.replace(RegExp.$1,
                    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(format))
                format = format.replace(RegExp.$1,
                        RegExp.$1.length == 1 ? o[k] :
                            ("00" + o[k]).substr(("" + o[k]).length));
        return format;
    };  