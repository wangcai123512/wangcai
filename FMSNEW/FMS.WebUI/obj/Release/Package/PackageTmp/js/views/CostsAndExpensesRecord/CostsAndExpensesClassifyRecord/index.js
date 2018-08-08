//初始化柱状图（可以拿到公共类中）
$(document).ready(function () {
    $('#OnceSonClassifyRecord #IEGroup').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    GetSubType("#OnceSonClassifyRecord #IncomeGrp", "#OnceSonClassifyRecord #IEGroup");
    $('#OnceSonClassifyCompareRecord #IEGroup').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    GetSubType("#OnceSonClassifyCompareRecord #IncomeGrp", "#OnceSonClassifyCompareRecord #IEGroup");
    var radarChartData = {
        // x轴显示的label
        labels: ['1', '2'],
        datasets: [
            {
                label: '成本与费用',
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
    ctx = document.getElementById("SecondmyChart");
    SecondmyChart = new Chart(ctx, {
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
    ctx = document.getElementById("OnceSonmyChart");
    OnceSonmyChart = new Chart(ctx, {
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
//查询一级费用科目分类的成本与费用
function OnceClassifyRecordQuery() {
    //获取时间
    var DateBegin = $('#OnceClassifyRecord #dateBegin').val();
    var DateEnd = $('#OnceClassifyRecord #dateEnd').val();
    var InvType = $('#OnceClassifyRecord #InvType').val();
    if (DateBegin == "" || DateEnd == "") {
        alert("请选择开始日期和截止日期!");
    }else if(Date.parse(DateBegin) > Date.parse(DateEnd))  {
        alert("开始日期不能大于截止日期，请重新选择！");
    }  
    else if (InvType == "") {
        alert("请选择成本与费用类型！"); 
    }
    else {
        $('#OnceClassifyTotalCollectList').bootstrapTable('destroy');
        $('#OnceClassifyTotalCollectList').bootstrapTable({
            url: '/CostsAndExpensesClassifyRecord/GetOnceClassifyTotalCollectList',
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
            queryParams: queryParamsOnceCTC
        });
        $('#OnceClassifyTotalList').bootstrapTable('destroy');
        $('#OnceClassifyTotalList').bootstrapTable({
            url: '/CostsAndExpensesClassifyRecord/GetOnceClassifyTotalList',
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
                { field: 'IEGroup', width: '130', align: 'center', title: '' },
                { field: 'SumAmount', width: '130', align: 'center', title: '', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkRatioValue }
            ],
            queryParams: queryParamsOnceCT
        });
        var VdateBegin = $('#OnceClassifyRecord #dateBegin').val();
        var VdateEnd = $('#OnceClassifyRecord #dateEnd').val();
        var VInvType = $('#OnceClassifyRecord #InvType').val();
        $("#OnceClassifyModel #dateBegin").val(VdateBegin);
        $("#OnceClassifyModel #dateEnd").val(VdateEnd);
        $("#OnceClassifyModel #InvType").val(VInvType);
        $('#OnceClassifyModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数1
function queryParamsOnceCTC(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#OnceClassifyRecord #dateBegin').val(),
        dateEnd: $('#OnceClassifyRecord #dateEnd').val(),
        InvType: $('#OnceClassifyRecord #InvType').val(),
    };
    return temp;
}
//查询条件参数2
function queryParamsOnceCT(params) {
    var temp = {
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#OnceClassifyRecord #dateBegin').val(),
        dateEnd: $('#OnceClassifyRecord #dateEnd').val(),
        InvType: $('#OnceClassifyRecord #InvType').val(),
    };
    return temp;
}
//占比率（公共）
var LinkRatioValue = function (value, row, index) {
    var link1 = "<p class='' >  \ " + (row.ReceiveRatio * 100).toFixed(2) + '%' + "\</p>";
    return link1;
};
//分类（首行）
var LinkHandleInvType = function (value, row, index) {
    var InvType = $('#OnceClassifyRecord #InvType').val();
    var link1 = " <p class=''> \ " + InvType + "\</a> ";
    return link1;
};
//货币（首行）
var LinkHandleCurrency = function (value, row, index) {
    var code = $("#code").val()
    var link1 = "<p  class='' style='margin-top:-3px'>  \ " + code + "\</p>"
    return link1;
};
//占总比例（公共）
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
//每月总成本与费用比较
function OnceClassifyCompareRecordQuery() {
    var dateBegin = $('#OnceClassifyCompareRecord #monthsStart').val();
    if (dateBegin == '0') {
        alert("请选择开始日期");
        error = true;
        return false;
    }
    var dateEnd = $('#OnceClassifyCompareRecord #monthsEnd').val();
    if (dateEnd == '0') {
        alert("请选择结束日期");
        error = true;
        return false;
    }
    var InvType = $('#OnceClassifyCompareRecord #InvType').val();
    if (InvType == '') {
        alert("请选择成本与费用");
        error = true;
        return false;
    }
    $.ajax({
        url: '/CostsAndExpensesClassifyRecord/GetOnceClassifyCompareList',
        type: "POST",
        async: false,
        dataType: "JSON",
        data: { dateBegin: dateBegin, dateEnd: dateEnd, InvType: InvType },
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
                        label: '成本与费用',
                        fillColor: '#62a8ea',// 填充色              
                        data: b, // 数据     
                    }
                ]
            };
            // 柱状图数据
            myChart.data.labels = a;
            myChart.data.datasets[0].data = b;
            myChart.update();
            var VdateBegin = $('#OnceClassifyCompareRecord #monthsStart').val();
            var VdateEnd = $('#OnceClassifyCompareRecord #monthsEnd').val();
            var VInvType = $('#OnceClassifyCompareRecord #InvType').val();
            $("#OnceClassifyCompareModel #dateBegin").val(VdateBegin);
            $("#OnceClassifyCompareModel #dateEnd").val(VdateEnd);
            $("#OnceClassifyCompareModel #InvType").val(VInvType);
            $('#OnceClassifyCompareModel').modal({ show: true, backdrop: 'static' });
            $(".boxed-layout").css("padding-right", "0px");
        },
        error: function () {
            alert('@General.Resource.Common.NoResponse');
        }
    }) 
    $(".boxed-layout").css("padding-right", "0px");

}
//查询二级费用科目分类的成本与费用
function SecondClassifyRecordQuery() {
    //获取时间
    var DateBegin = $('#SecondClassifyRecord #dateBegin').val();
    var DateEnd = $('#SecondClassifyRecord #dateEnd').val();
    var IEGroup = $('#SecondClassifyRecord #IEGroup').val();
    if (DateBegin == "" || DateEnd == "") {
        alert("请选择开始日期和截止日期!");
    } else if (Date.parse(DateBegin) > Date.parse(DateEnd)) {
        alert("开始日期不能大于截止日期，请重新选择！");
    } else if (IEGroup == "") {
        alert("请选择成本与费用类型！");
    }
    else {
        $('#SecondClassifyTotalCollectList').bootstrapTable('destroy');
        $('#SecondClassifyTotalCollectList').bootstrapTable({
            url: '/CostsAndExpensesClassifyRecord/GetSecondClassifyTotalCollectList',
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
                { field: 'IE_GUID', width: '130', align: 'center', title: '分类', formatter: LinkSecondInvType },
                { field: '', width: '130', align: 'center', title: '费用日期' },
                { field: 'SumAmount', width: '130', align: 'center', title: '金额', formatter: DecimalFmter },
                { field: 'IE_GUID', width: '130', align: 'center', title: '货币', formatter: LinkHandleCurrency },
                { field: 'IE_GUID', width: '130', align: 'center', title: '占总比例', formatter: LinkHandleRatio }
            ],
            queryParams: queryParamsSecondCTC
        });
        $('#SecondClassifyTotalList').bootstrapTable('destroy');
        $('#SecondClassifyTotalList').bootstrapTable({
            url: '/CostsAndExpensesClassifyRecord/GetSecondClassifyTotalList',
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
                { field: 'IEDescription', width: '130', align: 'center', title: '' },
                { field: 'AffirmDate', width: '130', align: 'center', title: '', formatter: DateHandle },
                { field: 'SumAmount', width: '130', align: 'center', title: '', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkRatioValue }
            ],
            queryParams: queryParamsSecondCT
        });
        var VdateBegin = $('#SecondClassifyRecord #dateBegin').val();
        var VdateEnd = $('#SecondClassifyRecord #dateEnd').val();
        var VIEGroup = $('#SecondClassifyRecord #IEGroup').val();
        $("#SecondClassifyModel #dateBegin").val(VdateBegin);
        $("#SecondClassifyModel #dateEnd").val(VdateEnd);
        $("#SecondClassifyModel #IEGroup").val(VIEGroup);
        $('#SecondClassifyModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数1
function queryParamsSecondCTC(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#SecondClassifyRecord #dateBegin').val(),
        dateEnd: $('#SecondClassifyRecord #dateEnd').val(),
        IEGroup: $('#SecondClassifyRecord #IEGroup').val(),
    };
    return temp;
}
//查询条件参数2
function queryParamsSecondCT(params) {
    var temp = {
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#SecondClassifyRecord #dateBegin').val(),
        dateEnd: $('#SecondClassifyRecord #dateEnd').val(),
        IEGroup: $('#SecondClassifyRecord #IEGroup').val(),
    };
    return temp;
}
//二级费用科目分类的总成本与费用(导出功能)
function OnceTotalExcelExport() {
    $('#secondform').tableExport({
        type: 'excel',
        fileName: '二级费用科目分类的总成本与费用',
        escape: false
    });
}
//分类（首行）
var LinkSecondInvType = function (value, row, index) {
    var IEGroup = $('#SecondClassifyRecord #IEGroup').val();
    var link1 = " <p class=''> \ " + IEGroup + "\</a> ";
    return link1;
};
//二级费用科目分类的成本与费用比较
function SecondClassifyCompareRecordQuery() {
    var dateBegin = $('#SecondClassifyCompareRecord #monthsStart').val();
    if (dateBegin == '0') {
        alert("请选择开始日期");
        error = true;
        return false;
    }
    var dateEnd = $('#SecondClassifyCompareRecord #monthsEnd').val();
    if (dateEnd == '0') {
        alert("请选择结束日期");
        error = true;
        return false;
    }
    var IEGroup = $('#SecondClassifyCompareRecord #IEGroup').val();
    if (IEGroup == '') {
        alert("请选择成本与费用");
        error = true;
        return false;
    }
    $.ajax({
        url: '/CostsAndExpensesClassifyRecord/GetSecondClassifyCompareList',
        type: "POST",
        async: false,
        dataType: "JSON",
        data: { dateBegin: dateBegin, dateEnd: dateEnd, IEGroup: IEGroup },
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
                        label: '成本与费用',
                        fillColor: '#62a8ea',// 填充色              
                        data: b, // 数据     
                    }
                ]
            };
            // 柱状图数据
            SecondmyChart.data.labels = a;
            SecondmyChart.data.datasets[0].data = b;
            SecondmyChart.update();
            var VdateBegin = $('#SecondClassifyCompareRecord #monthsStart').val();
            var VdateEnd = $('#SecondClassifyCompareRecord #monthsEnd').val();
            var VIEGroup = $('#SecondClassifyCompareRecord #IEGroup').val();
            $("#SecondClassifyCompareModel #dateBegin").val(VdateBegin);
            $("#SecondClassifyCompareModel #dateEnd").val(VdateEnd);
            $("#SecondClassifyCompareModel #IEGroup").val(VIEGroup);
            $('#SecondClassifyCompareModel').modal({ show: true, backdrop: 'static' });
            $(".boxed-layout").css("padding-right", "0px");
        },
        error: function () {
            alert('@General.Resource.Common.NoResponse');
        }
    })
    $(".boxed-layout").css("padding-right", "0px");

}
//查询一级分类下面二级费用科目分类的成本与费用
function OnceSonClassifyRecordQuery() {
    //获取时间
    var DateBegin = $('#OnceSonClassifyRecord #dateBegin').val();
    var DateEnd = $('#OnceSonClassifyRecord #dateEnd').val();
    var IncomeGrp = $('#OnceSonClassifyRecord #IncomeGrp').val();
    var IEGroup = $('#OnceSonClassifyRecord #IEGroup').val();
    if (DateBegin == "" || DateEnd == "") {
        alert("请选择开始日期和截止日期!");
    } else if (Date.parse(DateBegin) > Date.parse(DateEnd)) {
        alert("开始日期不能大于截止日期，请重新选择！");
    } else if (IncomeGrp == "") {
        alert("请选择一级成本与费用类型！");
    } else if (IEGroup == "") {
        alert("请选择二级成本与费用类型！");
    }
    else {
        $('#OnceSonClassifyTotalCollectList').bootstrapTable('destroy');
        $('#OnceSonClassifyTotalCollectList').bootstrapTable({
            url: '/CostsAndExpensesClassifyRecord/GetOnceSonClassifyTotalCollectList',
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
                { field: 'IE_GUID', width: '130', align: 'center', title: '分类', formatter: LinkOnceSonInvType },
                { field: '', width: '130', align: 'center', title: '费用日期' },
                { field: 'SumAmount', width: '130', align: 'center', title: '金额', formatter: DecimalFmter },
                { field: 'IE_GUID', width: '130', align: 'center', title: '货币', formatter: LinkHandleCurrency },
                { field: 'IE_GUID', width: '130', align: 'center', title: '占总比例', formatter: LinkHandleRatio }
            ],
            queryParams: queryParamsOnceSCTC
        });
        $('#OnceSonClassifyTotalList').bootstrapTable('destroy');
        $('#OnceSonClassifyTotalList').bootstrapTable({
            url: '/CostsAndExpensesClassifyRecord/GetOnceSonClassifyTotalList',
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
                { field: 'IEDescription', width: '130', align: 'center', title: '' },
                { field: 'AffirmDate', width: '130', align: 'center', title: '', formatter: DateHandle },
                { field: 'SumAmount', width: '130', align: 'center', title: '', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkRatioValue }
            ],
            queryParams: queryParamsOnceSCT
        });
        var VdateBegin = $('#OnceSonClassifyRecord #dateBegin').val();
        var VdateEnd = $('#OnceSonClassifyRecord #dateEnd').val();
        var VIncomeGrp = $('#OnceSonClassifyRecord #IncomeGrp').val();
        var VIEGroup = $('#OnceSonClassifyRecord #IEGroup').val();
        $("#OnceSonClassifyModel #dateBegin").val(VdateBegin);
        $("#OnceSonClassifyModel #dateEnd").val(VdateEnd);
        $("#OnceSonClassifyModel #IncomeGrp").val(VIncomeGrp);
        $("#OnceSonClassifyModel #IEGroup").val(VIEGroup);
        $('#OnceSonClassifyModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数1
function queryParamsOnceSCTC(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#OnceSonClassifyRecord #dateBegin').val(),
        dateEnd: $('#OnceSonClassifyRecord #dateEnd').val(),
        InvType: $('#OnceSonClassifyRecord #IncomeGrp').val(),
        IEGroup: $('#OnceSonClassifyRecord #IEGroup').val(),
    };
    return temp;
}
//查询条件参数2
function queryParamsOnceSCT(params) {
    var temp = {
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#OnceSonClassifyRecord #dateBegin').val(),
        dateEnd: $('#OnceSonClassifyRecord #dateEnd').val(),
        InvType: $('#OnceSonClassifyRecord #IncomeGrp').val(),
        IEGroup: $('#OnceSonClassifyRecord #IEGroup').val(),
    };
    return temp;
}
//一级分类下面二级费用科目分类的成本与费用(导出功能)
function OnceSonExcelExport() {
    $('#oncesonform').tableExport({
        type: 'excel',
        fileName: '一级分类内二级费用科目分类的成本与费用',
        escape: false
    });
}
//分类（首行）
var LinkOnceSonInvType = function (value, row, index) {
    var IEGroup = $('#OnceSonClassifyRecord #IEGroup').val();
    var link1 = " <p class=''> \ " + IEGroup + "\</a> ";
    return link1;
};
//查询一级分类下面二级费用科目分类的成本与费用比较
function OnceSonClassifyCompareRecordQuery() {
    var dateBegin = $('#OnceSonClassifyCompareRecord #monthsStart').val();
    if (dateBegin == '0') {
        alert("请选择开始日期");
        error = true;
        return false;
    }
    var dateEnd = $('#OnceSonClassifyCompareRecord #monthsEnd').val();
    if (dateEnd == '0') {
        alert("请选择结束日期");
        error = true;
        return false;
    }
    var InvType = $('#OnceSonClassifyCompareRecord #IncomeGrp').val();
    if (InvType == '') {
        alert("请选择一级成本与费用");
        error = true;
        return false;
    }
    var IEGroup = $('#OnceSonClassifyCompareRecord #IEGroup').val();
    if (IEGroup == '') {
        alert("请选择二级成本与费用");
        error = true;
        return false;
    }
    $.ajax({
        url: '/CostsAndExpensesClassifyRecord/GetOnceSonClassifyCompareList',
        type: "POST",
        async: false,
        dataType: "JSON",
        data: { dateBegin: dateBegin, dateEnd: dateEnd, InvType: InvType, IEGroup: IEGroup },
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
                        label: '二级费用科目分类的成本与费用比较',
                        fillColor: '#62a8ea',// 填充色              
                        data: b, // 数据     
                    }
                ]
            };
            // 柱状图数据
            OnceSonmyChart.data.labels = a;
            OnceSonmyChart.data.datasets[0].data = b;
            OnceSonmyChart.update();
            var VdateBegin = $('#OnceSonClassifyCompareRecord #monthsStart').val();
            var VdateEnd = $('#OnceSonClassifyCompareRecord #monthsEnd').val();
            var VIncomeGrp = $('#OnceSonClassifyCompareRecord #IncomeGrp').val();
            var VIEGroup = $('#OnceSonClassifyCompareRecord #IEGroup').val();
            $("#OnceSonClassifyCompareModel #dateBegin").val(VdateBegin);
            $("#OnceSonClassifyCompareModel #dateEnd").val(VdateEnd);
            $("#OnceSonClassifyCompareModel #InvType").val(VIncomeGrp);
            $("#OnceSonClassifyCompareModel #IEGroup").val(VIEGroup);
            $('#OnceSonClassifyCompareModel').modal({ show: true, backdrop: 'static' });
            $(".boxed-layout").css("padding-right", "0px");
        },
        error: function () {
            alert('@General.Resource.Common.NoResponse');
        }
    })
    $(".boxed-layout").css("padding-right", "0px");

}