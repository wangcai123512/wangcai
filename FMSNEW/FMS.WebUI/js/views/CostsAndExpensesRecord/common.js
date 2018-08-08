/**初始化选择日期下拉框*/
$(document).ready(function () {
    InitalDateInput();
    $('#lstMonths').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
        includeSelectAllOption: true

    });
    var SelectConfigSetting = {
        buttonWidth: 95,
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择'
    };
    var months = "";
    var now = new moment(); //当前日期   
    var nowMonth = now.month() + 1; //当前月 
    var nowYear = now.year(); //当前年 
    var nowQuarter = now.quarter()

    var lstMonths = new Array();
    var lstQuarter = new Array();
    var lstYear = new Array();

    var temp = new Object;
    temp.label = "选择 ";
    temp.value = "0";
    lstMonths.push(temp);
    lstQuarter.push(temp);
    lstYear.push(temp);
    for (var i = 1; i <= nowMonth; i++) {
        temp = new Object;
        temp.label = nowYear + "-" + i;
        temp.value = nowYear + "-" + i;
        lstMonths.push(temp);
    }
    for (var i = 1; i <= nowQuarter; i++) {
        temp = new Object;
        temp.label = nowYear + "-" + i;
        temp.value = nowYear + "-" + i;
        lstQuarter.push(temp);
    }
    temp = new Object;
    temp.label = nowYear;
    temp.value = nowYear;
    lstYear.push(temp);
    $(".report-date").multiselect(SelectConfigSetting);
    $("#CostsAndExpensesCompareTotalRecord #monthsStart").multiselect('dataprovider', lstMonths); //月份   
    $("#CostsAndExpensesCompareTotalRecord #monthsEnd").multiselect('dataprovider', lstMonths); //月份
    $("#OnceClassifyCompareRecord #monthsStart").multiselect('dataprovider', lstMonths); //月份   
    $("#OnceClassifyCompareRecord #monthsEnd").multiselect('dataprovider', lstMonths); //月份
    $("#SecondClassifyCompareRecord  #monthsStart").multiselect('dataprovider', lstMonths); //月份   
    $("#SecondClassifyCompareRecord  #monthsEnd").multiselect('dataprovider', lstMonths); //月份
    $("#OnceSonClassifyCompareRecord  #monthsStart").multiselect('dataprovider', lstMonths); //月份   
    $("#OnceSonClassifyCompareRecord  #monthsEnd").multiselect('dataprovider', lstMonths); //月份
    //获取所有二级分类
    var sonType;
    $.ajax({
        url: "/InternalAPI/GetAllSonType",
        async: false,
        dataType: "json",
        success: function (d) {

            sonType = d;
        },
        error: function (a, b, c) {

            alert(a);
        }
    });
    //给下拉框赋值
    $('#SecondClassifyRecord #IEGroup').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择'

    });
    $("#SecondClassifyRecord #IEGroup").multiselect('dataprovider', sonType);
    $("#SecondClassifyRecord #IEGroup").val("").multiselect("refresh");
    $('#SecondClassifyCompareRecord #IEGroup').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择'
    });
    $("#SecondClassifyCompareRecord #IEGroup").multiselect('dataprovider', sonType);
    $("#SecondClassifyCompareRecord #IEGroup").val("").multiselect("refresh");

});
//选择一级科目查询并且绑定二级科目
function GetSubType(parentSelector, subSelector) {
    var coloum = "";
    var invtype = $(parentSelector).find("option:selected").text();
    if (invtype == "营业成本") {
        coloum = 'ExpenseFlag'
    }
    if (invtype == "销售费用") {
        coloum = 'SaleFlag'
    }
    if (invtype == "管理费用") {
        coloum = 'ManageFlag'
    }
    if (invtype == "财务费用") {
        coloum = 'FinanceFlag'
    }
    if (invtype == "税费") {
        coloum = 'TaxFlag'
    }
    var detailType = "";
    $.ajax({
        url: "/InternalAPI/GetDetailType?coloum=" + coloum,
        async: false,
        dataType: "json",
        success: function (d) {
            detailType = d
        }
    });
    $(subSelector).multiselect('dataprovider', detailType);
}
//下拉框绑定
function InitSelect(selector, dataprovider) {
    $(selector).multiselect(SelectConfigSetting);
    $(selector).multiselect('dataprovider', dataprovider);
}
//转换时间格式
var DateHandle = function (jsondate) {

    jsondate = jsondate.replace("/Date(", "").replace(")/", "");
    if (jsondate < 0) {
        return "";
    } else {
        if (jsondate.indexOf("+") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("+"));
        } else if (jsondate.indexOf("-") > 0) {
            jsondate = jsondate.substring(0, jsondate.indexOf("-"));
        }

        var date = new Date(parseInt(jsondate, 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "/" + month + "/" + currentDate;
    }
}
