//初始化下拉框已经其他
$(document).ready(function () {
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
    //客户First
    $('#CustomerFirst').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        selectAllText: '全部',
        selectAllValue: 'select-all-value',
        includeFilterNewBtn: false,
        nonSelectedText: '全部',
        includeSelectAllOption: true,
    });
    $("#CustomerFirst").multiselect('dataprovider', rper);
    $("#CustomerFirst").val("").multiselect("refresh");

    //客户second
    $('#CustomerSecond').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        selectAllText: '全部',
        selectAllValue: 'select-all-value',
        includeFilterNewBtn: false,
        nonSelectedText: '全部',
        includeSelectAllOption: true,
       
    });
    $("#CustomerSecond").multiselect('dataprovider', rper);

    $("#CustomerSecond").val("").multiselect("refresh");

    //客户third
    $('#CustomerThird').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: false,
        selectAllText: '全部',
        selectAllValue: 'select-all-value',
        includeFilterNewBtn: false,
        nonSelectedText: '全部',
        includeSelectAllOption: true,
    });
    $("#CustomerThird").multiselect('dataprovider', rper);   
    $("#CustomerThird").val("").multiselect("refresh");
    //给时间初始化赋值
    document.getElementById("dateBegin").value = '1';
    document.getElementById("dateEnd").value = '365';

});
//查询逾期应收款总金额
function TotalAmountRQuery() {
    var CustomerFirst = $('#TotalAmountReceivables #CustomerFirst').val();
    if (CustomerFirst == null) {
        alert("请选择客户");
        error = true;
        return false;
    }
    else {
        $('#AllAmountReceivablesList').bootstrapTable('destroy');
        $('#AllAmountReceivablesList').bootstrapTable({
            url: '/AccountReceiveRecord/GetAllAmountReceivablesList',
            method: 'get',//请求方式（*）
            pageSize: 10,//每页的记录行数（*）
            pageList: [10, 20, 50, 100, 200], // 自定义分页列表
            pageNumber: 1, //初始化加载第一页，默认第一页
            showFooter:false,
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,    
            selectOnCheck: true,
            cardView: false,//是否显示详细视图
            pagination: false, //是否显示分页（*）
            sortName: 'RPerName', // 设置默认排序为 Date
            sortOrder: 'desc', // 设置排序为反序 desc
            search: false, // 开启搜索功能
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,//是否显示详细视图和列表视图的切换按钮
            showExport: false,
            uniqueId: "IE_GUID",
            exportTypes: ['xml', 'txt', 'excel'],
            columns: [
                { field: 'IE_GUID', width: '130', align: 'center', title: '客户', formatter: LinkHandleRper },
                { field: 'SumAmount', width: '130', align: 'center', title: '应收款金额', formatter: DecimalFmter },
                { field: 'IE_GUID', width: '130', align: 'center', title: '货币' ,formatter: LinkHandleCurrency },
                { field: 'Sumday', width: '130', align: 'center', title: '总天数'},
                { field:  '', width: '130', align: 'center', title: '逾期天数' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '占总比例', formatter: LinkHandleRatio }
            ],
            queryParams: queryParamsAll
        });
        $('#TotalAmountReceivablesList').bootstrapTable('destroy'); 
        $('#TotalAmountReceivablesList').bootstrapTable({
            url: '/AccountReceiveRecord/GetTotalAmountReceivablesList',
            method: 'get',//请求方式（*）
            pageSize: 10,//每页的记录行数（*）
            pageList: [10, 20, 50, 100, 200], // 自定义分页列表
            pageNumber: 1, //初始化加载第一页，默认第一页
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,
            cardView: false,//是否显示详细视图
            pagination: true, //是否显示分页（*）
            sortName: 'RPerName', // 设置默认排序为 Date 
            sortOrder: '', // 设置排序为反序 desc
            search: false, // 开启搜索功能
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,//是否显示详细视图和列表视图的切换按钮
            showExport: false,
            uniqueId: "IE_GUID",
            exportTypes: ['xml', 'txt', 'excel'],
            columns: [
                { field: 'RPerName', width: '130', align: 'center', title: '' },
                { field: 'SumAmount', width: '130', align: 'center', title: '',formatter: DecimalFmter},
                { field: 'Currency', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkHandleValue },
                { field: 'OverdueDays', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkRatioValue }
            ],
            queryParams: queryParamsTotal
        });
        $('#TotalAmountReceivablesModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
        $("#TotalAmountReceivablesList").css("margin-top", "-30px");
    }
}
//查询条件参数1
function queryParamsAll(params) {
  var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
    };
    return temp;
}
//查询条件参数2
function queryParamsTotal(params) {
    var str = "";
    CustomerFirst = $('#TotalAmountReceivables #CustomerFirst').val();
    for (var i = 0; i < CustomerFirst.length; i++) {
        str += CustomerFirst[i] + ",";
    }
    //去掉最后一个逗号(如果不需要去掉，就不用写)
    if (str.length > 0) {
        str = str.substr(0, str.length - 1);
    }
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码\\\
        RPerS: str,
    };
    return temp;
}
var LinkHandleValue = function (value, row, index) {
    var link1 = "<p class='' >  \ " + row.TotalDays + "\</p>";
    return link1;
};
//占比率
var LinkRatioValue = function (value, row, index) {
    var link1 = "<p class='' >  \ " + (row.ReceiveRatio*100).toFixed(2)+ '%' + "\</p>";
    return link1;
};  
//客户（首行）
var LinkHandleRper = function (value, row, index) {     
    var link1 = " <p class=''>全部</a> ";
    return link1;
};
//货币（首行）
var LinkHandleCurrency = function (value, row, index) {
    var code = $("#code").val()
    var link1 = "<p  class='' style='margin-top:-3px'>  \ " + code + "\</p>"
    return link1;
};
//占总比例（首行）
var LinkHandleRatio = function (value, row, index) {   
    var link1 = " <p class=''>100%</p> ";
    return link1;
};
//查询逾期应收款总金额
function TotalAmountOverdueRQuery() {
    var CustomerSecond = $('#TotalAmountOverdueReceivables #CustomerSecond').val();
    if (CustomerSecond == null) {
        alert("请选择客户");
        error = true;
        return false;
    }
    else {
        $('#AllAmountOverdueRList').bootstrapTable('destroy');
        $('#AllAmountOverdueRList').bootstrapTable({
            url: '/AccountReceiveRecord/GetAllAmountOverdueRList',
            method: 'get',//请求方式（*）
            pageSize: 10,//每页的记录行数（*）
            pageList: [10, 20, 50, 100, 200], // 自定义分页列表
            pageNumber: 1, //初始化加载第一页，默认第一页
            showFooter: false,
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,
            cardView: false,//是否显示详细视图
            pagination: false, //是否显示分页（*）
            sortName: 'RPerName', // 设置默认排序为 Date
            sortOrder: 'desc', // 设置排序为反序 desc
            search: false, // 开启搜索功能
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,//是否显示详细视图和列表视图的切换按钮
            showExport: false,
            uniqueId: "IE_GUID",
            exportTypes: ['xml', 'txt', 'excel'],
            columns: [    
                { field: 'IE_GUID', width: '130', align: 'center', title: '客户', formatter: LinkHandleRper },
                { field: 'SumAmount', width: '130', align: 'center', title: '逾期应收款金额', formatter: DecimalFmter },
                { field: 'IE_GUID', width: '130', align: 'center', title: '货币', formatter: LinkHandleCurrency },
                { field: '', width: '130', align: 'center', title: '逾期天数' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '占总比例', formatter: LinkHandleRatio }
            ],
            queryParams: queryParamsAllOverdue
        });
        $('#TotalAmountOverdueRList').bootstrapTable('destroy');
        $('#TotalAmountOverdueRList').bootstrapTable({
            url: '/AccountReceiveRecord/GetTotalAmountOverdueRList',
            method: 'get',//请求方式（*）
            pageSize: 10,//每页的记录行数（*）
            pageList: [10, 20, 50, 100, 200], // 自定义分页列表
            pageNumber: 1, //初始化加载第一页，默认第一页
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,
            cardView: false,//是否显示详细视图
            pagination: true, //是否显示分页（*）
            sortName: 'RPerName', // 设置默认排序为 Date
            sortOrder: 'desc', // 设置排序为反序 desc
            search: false, // 开启搜索功能
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,//是否显示详细视图和列表视图的切换按钮
            showExport: false,
            uniqueId: "IE_GUID",
            exportTypes: ['xml', 'txt', 'excel'],
            columns: [
                { field: 'RPerName', width: '130', align: 'center', title: '' },
                { field: 'SumAmount', width: '130', align: 'center', title: '', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '' },
                //{ field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkHandleValue },
                { field: 'OverdueDays', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkRatioValue }
            ],
            queryParams: queryParamsTotalOverdue
        });
        $('#TotalAmountOverdueRModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
        $("#TotalAmountOverdueRList").css("margin-top", "-30px");
    }
}
//逾期应收款总金额查询条件1
function queryParamsAllOverdue(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
    };
    return temp;
}
//逾期应收款总金额查询条件2
function queryParamsTotalOverdue(params) {
    var str = "";
    CustomerSecond = $('#TotalAmountOverdueReceivables #CustomerSecond').val();
    for (var i = 0; i < CustomerSecond.length; i++) {
        str += CustomerSecond[i] + ",";
    }
    //去掉最后一个逗号(如果不需要去掉，就不用写)
    if (str.length > 0) {
        str = str.substr(0, str.length - 1);
    }
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码\\\
        RPerS: str,
    };
    return temp;
}
//查询m天到n天逾期应收款总金额
function TodayAmountOverdueRQuery() {
    var CustomerThird = $('#TodayAmountOverdueReceivables #CustomerThird').val();
    var dateBegin = $('#TodayAmountOverdueReceivables #dateBegin').val();
    var dateEnd = $('#TodayAmountOverdueReceivables #dateEnd').val();
    if (CustomerThird == null) {
        alert("请选择客户"); 
        error = true;
        return false;
    }
    if (dateBegin == '') {
        alert("请输入开始日期");
        error = true;
        return false;
    }
    if (dateEnd == '') {
        alert("请输入结束日期");
        error = true;
        return false;
    }
    else {
        $('#AllTodayAmountOverdueRList').bootstrapTable('destroy');
        $('#AllTodayAmountOverdueRList').bootstrapTable({
            url: '/AccountReceiveRecord/GetAllAmountOverdueRList',//查询结果为逾期应收款金额
            method: 'get',//请求方式（*）
            pageSize: 10,//每页的记录行数（*）
            pageList: [10, 20, 50, 100, 200], // 自定义分页列表
            pageNumber: 1, //初始化加载第一页，默认第一页
            showFooter: false,
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,

            cardView: false,//是否显示详细视图
            pagination: false, //是否显示分页（*）
            sortName: 'RPerName', // 设置默认排序为 Date
            sortOrder: 'desc', // 设置排序为反序 desc
            search: false, // 开启搜索功能
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,//是否显示详细视图和列表视图的切换按钮
            showExport: false,
            uniqueId: "IE_GUID",
            exportTypes: ['xml', 'txt', 'excel'],
            columns: [
                { field: 'IE_GUID', width: '130', align: 'center', title: '客户', formatter: LinkHandleRper },
                { field: 'SumAmount', width: '130', align: 'center', title: '应收款金额', formatter: DecimalFmter },
                { field: 'IE_GUID', width: '130', align: 'center', title: '货币', formatter: LinkHandleCurrency },
                { field: '', width: '130', align: 'center', title: '逾期天数' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '占总比例', formatter: LinkHandleRatio }
            ],
            queryParams: queryParamsAllToday
        });
        $('#TotalTodayAmountOverdueRList').bootstrapTable('destroy');
        $('#TotalTodayAmountOverdueRList').bootstrapTable({
            url: '/AccountReceiveRecord/GetTotalTodayAmountOverdueRList',
            method: 'get',//请求方式（*）
            pageSize: 10,//每页的记录行数（*）
            pageList: [10, 20, 50, 100, 200], // 自定义分页列表
            pageNumber: 1, //初始化加载第一页，默认第一页
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,
            cardView: false,//是否显示详细视图
            pagination: true, //是否显示分页（*）
            sortName: 'RPerName', // 设置默认排序为 Date
            sortOrder: 'desc', // 设置排序为反序 desc
            search: false, // 开启搜索功能
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,//是否显示详细视图和列表视图的切换按钮
            showExport: false,
            uniqueId: "IE_GUID",
            exportTypes: ['xml', 'txt', 'excel'],
            columns: [
                { field: 'RPerName', width: '130', align: 'center', title: '' },
                { field: 'SumAmount', width: '130', align: 'center', title: '', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '' },
               // { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkHandleValue },
                { field: 'OverdueDays', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkRatioValue }
            ],
            queryParams: queryParamsTotalToday
        });
        $('#TodayAmountOverdueRModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
        $("#TotalTodayAmountOverdueRList").css("margin-top", "-30px");
    }
}
//查询m天到n天逾期应收款总金额条件1
function queryParamsAllToday(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
    };
    return temp;
}
//查询m天到n天逾期应收款总金额条件2
function queryParamsTotalToday(params) {
    var str = "";
    CustomerThird = $('#TodayAmountOverdueReceivables #CustomerThird').val();
    for (var i = 0; i < CustomerThird.length; i++) {
        str += CustomerThird[i] + ",";
    }
    //去掉最后一个逗号(如果不需要去掉，就不用写)
    if (str.length > 0) {
        str = str.substr(0, str.length - 1);
    }
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码\\\
        RPerS: str,
        dateBegin: $("#TodayAmountOverdueReceivables #dateBegin").val(),
        dateEnd: $("#TodayAmountOverdueReceivables #dateEnd").val(),
    };
    return temp;
}