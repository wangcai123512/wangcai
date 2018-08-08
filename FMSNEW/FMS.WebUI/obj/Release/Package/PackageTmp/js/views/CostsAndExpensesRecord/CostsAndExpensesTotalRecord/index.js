//初始化柱状图（可以拿到公共类中）
$(document).ready(function () {
    var radarChartData = {
        // x轴显示的label
        labels: ['1', '2'],
        datasets: [
            {
                label: '总成本与费用',
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
//查询一级费用科目分类的总成本与费用
function OnceTotalRecordQuery() {
    //获取时间
    var DateBegin = $('#CostsAndExpensesOnceTotalRecord #dateBegin').val();
    var DateEnd = $('#CostsAndExpensesOnceTotalRecord #dateEnd').val();
    if (DateBegin == "" || DateEnd == "") {
        alert("请选择开始日期和截止日期!");
    } else if (Date.parse(DateBegin) > Date.parse(DateEnd)) {
        alert("开始日期不能大于截止日期，请重新选择！");
    }
    else {
        $('#OnceTotalCollectList').bootstrapTable('destroy');
        $('#OnceTotalCollectList').bootstrapTable({
            url: '/CostsAndExpensesTotalRecord/GetOnceTotalCollectList',
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
                { field: 'IE_GUID', width: '130', align: 'center', title: '分类', formatter: LinkHandleInvType},
                { field: 'SumAmount', width: '130', align: 'center', title:'金额', formatter: DecimalFmter },
                { field: 'IE_GUID', width: '130', align: 'center', title: '货币' ,formatter: LinkHandleCurrency },
                { field: 'IE_GUID', width: '130', align: 'center', title: '占总比例', formatter: LinkHandleRatio}
            ],
            queryParams: queryParamscollect
        });
        $('#OnceTotalList').bootstrapTable('destroy');
        $('#OnceTotalList').bootstrapTable({
            url: '/CostsAndExpensesTotalRecord/GetOnceTotalList',
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
            exportTypes: ['xml', 'txt', 'excel', 'word'],
            columns: [
                { field: 'InvType', width: '130', align: 'center', title:'' },
                { field: 'SumAmount', width: '130', align: 'center', title:'',formatter: DecimalFmter},
                { field: 'Currency', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkRatioValue }
            ],
            queryParams: queryParamsOnceTotal
        });
        var VdateBegin = $('#CostsAndExpensesOnceTotalRecord #dateBegin').val();
        var VdateEnd = $('#CostsAndExpensesOnceTotalRecord #dateEnd').val();
        $("#CostsAndExpensesOnceTotalModel #dateBegin").val(VdateBegin);
        $("#CostsAndExpensesOnceTotalModel #dateEnd").val(VdateEnd);
        $('#CostsAndExpensesOnceTotalModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数1(公共)
function queryParamscollect(params) {
  var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
    };
    return temp;
}
//查询条件参数2
function queryParamsOnceTotal(params) {
    var temp = {
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#CostsAndExpensesOnceTotalRecord #dateBegin').val(),
        dateEnd: $('#CostsAndExpensesOnceTotalRecord #dateEnd').val(),
    };
    return temp;   
}
//占比率
var LinkRatioValue = function (value, row, index) {
    var link1 = "<p class='' >  \ " + (row.ReceiveRatio * 100).toFixed(2) + '%' + "\</p>";
    return link1;
};
//客户（首行）
var LinkHandleInvType = function (value, row, index) {
    var link1 = " <p class=''>总成本与费用</a> ";
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
//一级费用科目分类的总成本与费用(导出功能)
function OnceTotalExcelExport() {
    $('#onceform').tableExport({
        type: 'excel',
        fileName: '一级费用科目分类的总成本与费用',
        escape: false
    });
}
//查询一级费用科目分类的总成本与费用
function SecondTotalRecordQuery() {
    //获取时间
    var DateBegin = $('#CostsAndExpensesSecondTotalRecord #dateBegin').val();
    var DateEnd = $('#CostsAndExpensesSecondTotalRecord #dateEnd').val();
    if (DateBegin == "" || DateEnd == "") {
        alert("请选择开始日期和截止日期!");
    } else if (Date.parse(DateBegin) > Date.parse(DateEnd)) {
        alert("开始日期不能大于截止日期，请重新选择！");
    }
    else {
        $('#SecondTotalCollectList').bootstrapTable('destroy');
        $('#SecondTotalCollectList').bootstrapTable({
            url: '/CostsAndExpensesTotalRecord/GetOnceTotalCollectList',
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
                { field: 'IE_GUID', width: '130', align: 'center', title: '分类', formatter: LinkHandleInvType },
                { field: 'SumAmount', width: '130', align: 'center', title: '金额', formatter: DecimalFmter },
                { field: 'IE_GUID', width: '130', align: 'center', title: '货币', formatter: LinkHandleCurrency },
                { field: 'IE_GUID', width: '130', align: 'center', title: '占总比例', formatter: LinkHandleRatio }
            ],
            queryParams: queryParamscollect
        });
        $('#SecondTotalList').bootstrapTable('destroy');
        $('#SecondTotalList').bootstrapTable({
            url: '/CostsAndExpensesTotalRecord/GetSecondTotalList',
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
                { field: 'InvType', width: '130', align: 'center', title: '' },
                { field: 'SumAmount', width: '130', align: 'center', title: '', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkRatioValue }
            ],
            queryParams: queryParamsTotal
        });
        var VdateBegin = $('#CostsAndExpensesSecondTotalRecord #dateBegin').val();
        var VdateEnd = $('#CostsAndExpensesSecondTotalRecord #dateEnd').val();
        $("#CostsAndExpensesSecondTotalModel #dateBegin").val(VdateBegin);
        $("#CostsAndExpensesSecondTotalModel #dateEnd").val(VdateEnd);
        $('#CostsAndExpensesSecondTotalModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数2
function queryParamsTotal(params) {
    var temp = {
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#CostsAndExpensesSecondTotalRecord #dateBegin').val(),
        dateEnd: $('#CostsAndExpensesSecondTotalRecord #dateEnd').val(),
    };
    return temp;
}
//二级费用科目分类的总成本与费用(导出功能)
function secondTotalExcelExport() {
    $('#secondform').tableExport({
        type: 'excel',
        fileName: '二级费用科目分类的总成本与费用',
        escape: false
    });
}
//每月总成本与费用比较
function CompareTotalRecordQuery() {
    var dateBegin = $('#CostsAndExpensesCompareTotalRecord #monthsStart').val();
    if (dateBegin == '0') {
        alert("请选择开始日期");
        error = true;
        return false;
    }
    var dateEnd = $('#CostsAndExpensesCompareTotalRecord #monthsEnd').val();
    if (dateEnd == '0') {
        alert("请选择结束日期");
        error = true;
        return false;
    }
    $.ajax({
        url: '/CostsAndExpensesTotalRecord/GetCompareTotalList',
        type: "POST",
        async: false,
        dataType: "JSON",
        data: { dateBegin: dateBegin, dateEnd: dateEnd },
        ContentType: "application/json",
        success: function (ret) {
            var a = new Array();
            var b = new Array();
            for (var i = 0; i < ret.length; i++) {
                a[i] = ret[i]["CompareMonth"] + "月";
                b[i] = ret[i]["SumAmount"];
            }
            var radarChartData = {
                // x轴显示的label
                labels: a,
                datasets: [
                    {
                        label: '总成本与费用',
                        fillColor: '#62a8ea',// 填充色              
                        data: b, // 数据     
                    }
                ]
            };
            // 柱状图数据
            myChart.data.labels = a;
            myChart.data.datasets[0].data = b;
            myChart.update();
            var VdateBegin = $('#CostsAndExpensesCompareTotalRecord #monthsStart').val();
            var VdateEnd = $('#CostsAndExpensesCompareTotalRecord #monthsEnd').val();
            $("#CostsAndExpensesCompareTotalModel #dateBegin").val(VdateBegin);
            $("#CostsAndExpensesCompareTotalModel #dateEnd").val(VdateEnd);
            $('#CostsAndExpensesCompareTotalModel').modal({ show: true, backdrop: 'static' });
            $(".boxed-layout").css("padding-right", "0px");
        },
        error: function () {
            alert('@General.Resource.Common.NoResponse');
        }
    })
    $(".boxed-layout").css("padding-right", "0px");

}