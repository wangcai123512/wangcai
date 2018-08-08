//初始化柱状图（可以拿到公共类中）
$(document).ready(function () {
    var radarChartData = {
        // x轴显示的label
        labels: ['1', '2'],
        datasets: [
            {
                label: '净现金流',
                fillColor: '#62a8ea',// 填充色              
                data: ['1', '2'],// 数据  
            }
        ]
    };
    ctx = document.getElementById("myChart");
    myChart = new Chart(ctx, {
        type: 'bar',
        data: radarChartData,
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            },
        }
    });
});
//查询净现金流
function CashInFlowsRecordQuery() {
    //查询汇总净现金流账号
    var dateBeginInFlows = $('#CashFlowsRecord #dateBegin').val();
    var dateEndInFlows = $('#CashFlowsRecord #dateEnd').val();
    if (dateBeginInFlows == "") {
        alert("请输入开始日期");
        error = true;
        return false;
    }
    if (dateEndInFlows == "") {
        alert("请输入结束日期");
        error = true;
        return false;
    }
    else {
        //检查当前账号是否有汇率
        $('#CashFlowsRecordList').bootstrapTable('destroy');
        $('#CashFlowsRecordList').bootstrapTable({
            url: '/NetCashFlowsRecord/GetNetCashFlowsRecordList',
            method: 'get',
            pageSize: 10,
            pageList: [5, 10, 25, 50, 100, 200], // 自定义分页列表 
            pageNumber: 1,
            //classes: 'table-no-bordered',
            sortClass: true,
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,
            pagination: false,
            sortName: '', // 设置默认排序为 name
            sortOrder: '', // 设置排序为反序 desc
            search: false,
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,
            showExport: false,
            exportTypes: ['xml', 'txt', 'excel'],
            uniqueId: "",
            columns: [
                { field: 'AccountAbbreviation', title: '', width: '130', formatter: LinkHandle },
                { field: 'SumAmount', width: '130', align: 'center', title: '金额', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '货币' },
            ],
            queryParams: queryParams
        });
        //查询汇总本币汇总
        $('#CashLocalCurrencyList').bootstrapTable('destroy');
        $('#CashLocalCurrencyList').bootstrapTable({
            url: '/NetCashFlowsRecord/GetNetCashLocalCurrencyList',
            method: 'get',
            pageSize: 10,
            pageList: [5, 10, 25, 50, 100, 200], // 自定义分页列表
            pageNumber: 1,
            //classes: 'table-no-bordered',
            sortClass: true,
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,
            pagination: false,
            // sortName: 'CreateDate', // 设置默认排序为 name
            // sortOrder: 'desc', // 设置排序为反序 desc
            search: false,
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,
            showExport: false,
            exportTypes: ['xml', 'txt', 'excel'],
            uniqueId: "TNEWGUID",
            columns: [
                 { field: 'TNEWGUID', title: '', width: '130', formatter: LinkHandleLocal },
                { field: 'SumAmount', width: '130', align: 'center', title: '',formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: ''},
            ],
            //使用同一个查询条件
            queryParams: queryParams
        });
        ////查询汇总统计货币
        $('#NetCashStatisticalCurrencyList').bootstrapTable('destroy');
        $('#NetCashStatisticalCurrencyList').bootstrapTable({
            url: '/NetCashFlowsRecord/GetNetCashStatisticalCurrencyList',
            method: 'get',
            pageSize: 10,
            pageList: [5, 10, 25, 50, 100, 200], // 自定义分页列表
            pageNumber: 1,
            singleSelect: true,
            clickToSelect: true,
            //classes: 'table-no-bordered',
            sortClass: true,
            checkOnSelect: true,
            selectOnCheck: true,
            pagination: false,
            // sortName: 'CreateDate', // 设置默认排序为 name
            // sortOrder: 'desc', // 设置排序为反序 desc
            search: false,
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,
            showExport: false,
            exportTypes: ['xml', 'txt', 'excel'],
            uniqueId: "",
            columns: [
               { field: '', title: '', width: '130', formatter: LinkHandleStical },
                { field: 'SumAmount', width: '130', align: 'center', title: '', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '' },
            ],
            //使用同一个查询条件
            queryParams: queryParams
        });
        var VdateBegin = $('#CashFlowsRecord #dateBegin').val();
        var VdateEnd = $('#CashFlowsRecord #dateEnd').val();
        var VAccountCash = $("#AccountCash").find("option:selected").text();//选中的文本
        $("#NetCashFlowStatisticsModel #statisticsdateBegin").val(VdateBegin);
        $("#NetCashFlowStatisticsModel #statisticsdateEnd").val(VdateEnd);
        if (VAccountCash == "") {
            $("#NetCashFlowStatisticsModel #statisticsCash").val('全部');
        } else {
            $("#NetCashFlowStatisticsModel #statisticsCash").val(VAccountCash);
        }
        $('#NetCashFlowStatisticsModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数
function queryParams(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        datebegin: $('#CashFlowsRecord #dateBegin').val(),
        dateEnd: $('#CashFlowsRecord #dateEnd').val(),
        BA_GUID: $('#CashFlowsRecord #AccountCash').val(),
    };
    return temp;
}
//查询按钮，将页面重新加载而不是再次请求
function CashFlowsRecordQuery() {
    $('#CashFlowsRecordList').bootstrapTable('refresh');
}
//显示账户名称
function LinkHandle(value, row, index) {
    var link1 = "<p  class='linkbtn ' style='margin-top:-3px'>  \ " + row.AccountAbbreviation + "\</p>"
    return link1;
}
//显示统计货币汇总
function LinkHandleStical(value, row, index) {
    var link1 = "<p  class='linkbtn ' style='margin-top:-3px'> 统计货币汇总</p>"
    return link1;
}
//显示本币汇总
function LinkHandleLocal(value, row, index) {
    if (index == 0) {
        var link1 = "<p  class='linkbtn ' style='margin-top:-3px'>   本币汇总</p>"
    } else {
        var link1 = "<p  class='linkbtn ' style='margin-top:-3px'> </p>"
    }
    return link1;
}
//导出功能需要修改
function excelexport() {
    $('.asd').tableExport({
        type: 'excel',
        fileName: '汇总',
        escape: false
    });
}
//现金流水账
function CashInFlowsAccountRecordQuery() {
    var dateBeginInFlows = $('#CashFlowsAccountRecord #dateBegin').val();
    var dateEndInFlows = $('#CashFlowsAccountRecord #dateEnd').val();
    if (dateBeginInFlows == "") {
        alert("请输入开始日期");
        error = true;
        return false;
    }
    if (dateEndInFlows == "") {
        alert("请输入结束日期");
        error = true;
        return false;
    }
    else {
        $('#cashFlowsAccountRecorddataList').bootstrapTable('destroy');
        $('#cashFlowsAccountRecorddataList').bootstrapTable({
            url: '/NetCashFlowsRecord/GetCashInFlowsAccountRecordList',
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
            sortName: 'CreateDate', // 设置默认排序为 Date
            sortOrder: 'desc', // 设置排序为反序 desc
            search: false, // 开启搜索功能
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,//是否显示详细视图和列表视图的切换按钮
            showExport: false,
            uniqueId: "",
            exportTypes: ['xml', 'txt', 'excel'],
            columns: [
                { field: 'Date', width: '130', align: 'center', title: '日期' },
                { field: 'AccountAbbreviation', width: '130', align: 'center', title: '账号' },
                { field: 'RecSumAmount', width: '130', align: 'center', title: '进',formatter: DecimalFmter },
                { field: 'PaySumAmount', width: '130', align: 'center', title: '出', formatter: DecimalFmter },
                { field: 'BalanceSumAmount', width: '130', align: 'center', title: '余额',formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '货币' },
            ],
            queryParams: queryParamsInFlows
        });
        var VdateBegin = $('#CashFlowsAccountRecord #dateBegin').val();
        var VdateEnd = $('#CashFlowsAccountRecord #dateEnd').val();
        var VAccountCash = $("#AccountList").find("option:selected").text();//选中的文本
        $("#CashFlowsAccountRecordModel #statisticsdateBegin").val(VdateBegin);
        $("#CashFlowsAccountRecordModel #statisticsdateEnd").val(VdateEnd);
        if (VAccountCash == "") {
            $("#CashFlowsAccountRecordModel #statisticsCash").val('全部');
        } else {
            $("#CashFlowsAccountRecordModel #statisticsCash").val(VAccountCash);
        }
        $('#CashFlowsAccountRecordModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数
function queryParamsInFlows(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        datebegin: $('#CashFlowsAccountRecord #dateBegin').val(),
        dateEnd: $('#CashFlowsAccountRecord #dateEnd').val(),
        BA_GUID: $('#CashFlowsAccountRecord #AccountList').val(),
    };
    return temp;
}
//导出功能
function excelexport() {
    $('#cashFlowsAccountRecorddataList').tableExport({
        type: 'excel',
        fileName: '现金流水账',
        escape: false
    });
}
//现金流比较
function CashInFlowsCompareRecordQuery() {
    var datebegin = $('#CashFlowsCompareRecord #monthsStart').val();
    if (datebegin == '0') {
        alert("请输入开始日期");
        error = true;
        return false;
    }
    var dateEnd = $('#CashFlowsCompareRecord #monthsEnd').val();
    if (dateEnd == '0') {
        alert("请输入结束日期");
        error = true;
        return false;
    }
    var BAGUID = $('#CashFlowsCompareRecord #AccountCompare').val();
    $.ajax({
        url: "/NetCashFlowsRecord/CashInFlowsCompareRecordQuery",
        type: "POST",
        async: false,
        dataType: "JSON",
        data: { datebegin: datebegin, dateEnd: dateEnd, BA_GUID: BAGUID },
        ContentType: "application/json",
        success: function (ret) {
            var a = new Array();
            var b = new Array();
            for (var i = 0; i < ret.length; i++) {
                a[i] = ret[i]["Date"] + "月";
                b[i] = ret[i]["SumAmount"];
            }
            var radarChartData = {
                // x轴显示的label
                labels: a,
                datasets: [
                    {
                        label: '净现金流',
                        fillColor: '#62a8ea',// 填充色              
                        data: b, // 数据     
                    }
                ]
            };
            // 柱状图数据
            myChart.data.labels = a;
            myChart.data.datasets[0].data = b;
            myChart.update();
            var VdateBegin = $('#CashFlowsCompareRecord #monthsStart').val();
            var VdateEnd = $('#CashFlowsCompareRecord #monthsEnd').val();
            var VAccountCash = $("#AccountCompare").find("option:selected").text();//选中的文本
            $("#CashInFlowsCompareRecordModel #statisticsdateBegin").val(VdateBegin);
            $("#CashInFlowsCompareRecordModel #statisticsdateEnd").val(VdateEnd);
            if (VAccountCash == "") {
                $("#CashInFlowsCompareRecordModel #statisticsCash").val('全部');
            } else {
                $("#CashInFlowsCompareRecordModel #statisticsCash").val(VAccountCash);
            }
            $('#CashInFlowsCompareRecordModel').modal({ show: true, backdrop: 'static' });
            $(".boxed-layout").css("padding-right", "0px");
        },
        error: function () {
            alert('@General.Resource.Common.NoResponse');
        }
    })
    $(".boxed-layout").css("padding-right", "0px");
}




