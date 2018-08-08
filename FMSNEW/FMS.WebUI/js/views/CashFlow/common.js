$(document).ready(function () {
    InitalDateInput();
    var bankaccount;
    $.ajax({
        url: "/InternalAPI/GetBankAccountss",
        async: false,
        dataType: "json",
        success: function (d) {
            for (var i = 0; i < d.length; i++) {
                d[i]["value"] = d[i]["value"];

            }
            bankaccount = d;
        }
    });
    //客户
    var rper;
    $.ajax({
        url: "/InternalAPI/GetCustomer",
        async: false,
        dataType: "json",
        success: function (d) {
            rper = d;
        }
    });
    //供应商
    var supplier;
    $.ajax({
        url: "/InternalAPI/GetPayeeJson",
        async: false,
        dataType: "json",
        success: function (d) {
            supplier = d;
        }
    });
    //First Account
    $('#AccountCash').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
        includeSelectAllOption: true
    });
    $("#AccountCash").multiselect('dataprovider', bankaccount);
    $("#AccountCash").val("").multiselect("refresh");
    //Second Account
    $('#AccountCompare').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
        includeSelectAllOption: true
    });
    $("#AccountCompare").multiselect('dataprovider', bankaccount);
    $("#AccountCompare").val("").multiselect("refresh");
    //Third Account
    $('#AccountList').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
        includeSelectAllOption: true

    });
    $("#AccountList").multiselect('dataprovider', bankaccount);
    $("#AccountList").val("").multiselect("refresh");
    //客户
    $('#CustomerList').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '全部',
        includeSelectAllOption: true
    });
    $("#CustomerList").multiselect('dataprovider', rper);
    $("#CustomerList").val("").multiselect("refresh");
    //供应商
    $('#SupplierList').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '全部',
        includeSelectAllOption: true
    });
    $("#SupplierList").multiselect('dataprovider', supplier);
    $("#SupplierList").val("").multiselect("refresh");
    $('#lstMonths').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '全部',
        includeSelectAllOption: true

    });
    var SelectConfigSetting = {
        buttonWidth: 120,
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '全部'
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
    temp.label = "请选择";
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
    $("#monthsStart").multiselect('dataprovider', lstMonths); //月份   
    $("#monthsEnd").multiselect('dataprovider', lstMonths); //月份
    $("#quarters").multiselect('dataprovider', lstQuarter); //季度
    $("#years").multiselect('dataprovider', lstYear); //年份



});
