    //初始化柱状图（可以拿到公共类中）
$(document).ready(function () {
    var radarChartData = {
        // x轴显示的label
        labels: ['1', '2'],
        datasets: [
            {
                label: '流出现金',
                fillColor: '#62a8ea',// 填充色              
                data: ['1', '2'] // 数据  
            }
        ]
    };
    ctx = document.getElementById("myChart");
    myChart = new Chart(ctx,{
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
function AccountOutFlowCashQuery() {
    //查询汇总净现金流账号
    var dateBeginInFlows = $('#AccountOutFlowCash #dateBegin').val();
    var dateEndInFlows = $('#AccountOutFlowCash #dateEnd').val();
    if (dateBeginInFlows == "") {
        alert("请选择开始日期");
        error = true;
        return false;
    }
    if (dateEndInFlows == "") {
        alert("请选择结束日期");
        error = true;
        return false;
    }
    else {
        $('#AccountOutFlowCashValue').bootstrapTable('destroy');
        $('#AccountOutFlowCashValue').bootstrapTable({
            //查询需要修改
            url: '/OutFlowCashRecord/GetAccountOutFlowCashRecordList',
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
            sortName: 'Date', // 设置默认排序为 Date
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
                { field: 'SumAmount', width: '130', align: 'center', title: '金额', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '货币' },
                { field: 'InvTypeDts', width: '250', align: 'center', title: '描述' }
            ],
            queryParams: queryParams
        });
        var VdateBegin = $('#AccountOutFlowCash #dateBegin').val();
        var VdateEnd = $('#AccountOutFlowCash #dateEnd').val();
        var VAccountCash = $("#AccountCash").find("option:selected").text();//选中的文本
        $("#AccountOutFlowCashModel #statisticsdateBegin").val(VdateBegin);
        $("#AccountOutFlowCashModel #statisticsdateEnd").val(VdateEnd);
        if (VAccountCash == "") {
            $("#AccountOutFlowCashModel #statisticsCash").val('全部');
        } else {
            $("#AccountOutFlowCashModel #statisticsCash").val(VAccountCash);
        }
        $('#AccountOutFlowCashModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数
function queryParams(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        datebegin: $('#AccountOutFlowCash #dateBegin').val(),
        dateEnd: $('#AccountOutFlowCash #dateEnd').val(),
        BA_GUID: $('#AccountOutFlowCash #AccountCash').val(),
    };
    return temp;
}
//导出功能
function AccountOutFlowCashExport() {
    $('#AccountOutFlowCashValue').tableExport({
        type: 'excel',
        fileName: '账号流出现金',
        escape: false
    });
}
//查询净现金流
function CustomerOutFlowCashQuery() {
    //查询汇总净现金流账号
    var dateBeginInFlows = $('#CustomerOutFlowCash #dateBegin').val();
    var dateEndInFlows = $('#CustomerOutFlowCash #dateEnd').val();
    if (dateBeginInFlows == "") {
        alert("请选择开始日期");
        error = true;
        return false;
    }
    if (dateEndInFlows == ""){
        alert("请选择结束日期");
        error = true;
        return false;
    }
    else {
        $('#CustomerOutFlowCashValue').bootstrapTable('destroy');
        $('#CustomerOutFlowCashValue').bootstrapTable({
            url: '/OutFlowCashRecord/GetSupplierOutFlowCashRecordList',
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
            sortName: 'Date', // 设置默认排序为 Date
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
                { field: 'SumAmount', width: '130', align: 'center', title: '金额', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '货币' },
                { field: 'InvTypeDts', width: '250', align: 'center', title: '描述' }
            ],
            queryParams: queryParamsA
        });
        var VdateBegin = $('#CustomerOutFlowCash #dateBegin').val();
        var VdateEnd = $('#CustomerOutFlowCash #dateEnd').val();
        var VAccountCash = $("#SupplierList").find("option:selected").text();//选中的文本
        $("#CustomerOutFlowCashModel #statisticsdateBegin").val(VdateBegin);
        $("#CustomerOutFlowCashModel #statisticsdateEnd").val(VdateEnd);
        if (VAccountCash == "") {
            $("#CustomerOutFlowCashModel #statisticsCash").val('全部');
        } else {
            $("#CustomerOutFlowCashModel #statisticsCash").val(VAccountCash);
        }
        $('#CustomerOutFlowCashModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数
function queryParamsA(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        datebegin: $('#CustomerOutFlowCash #dateBegin').val(),
        dateEnd: $('#CustomerOutFlowCash #dateEnd').val(),
        BA_GUID: $('#CustomerOutFlowCash #SupplierList').val(),
    };
    return temp;
}
//导出功能
function CustomerOutFlowCashExport() {
    $('#CustomerOutFlowCashValue').tableExport({
        type: 'excel',
        fileName: '供应商流出现金',
        escape: false
    });
}
//现金流比较
function CompareOutFlowCashQuery() {
    var datebegin = $('#CompareOutFlowCash #monthsStart').val();    
    if (datebegin == '0') {
        alert("请输入开始日期");
        error = true;
        return false;
    }
    var dateEnd = $('#CompareOutFlowCash #monthsEnd').val();
    if (dateEnd == '0') {
        alert("请输入结束日期");
        error = true;
        return false;
    }
    var BAGUID = $('#CompareOutFlowCash #AccountCompare').val();
    $.ajax({
        url: "/OutFlowCashRecord/GetCompareOutFlowCashRecordList",
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
                        label: '流出现金',
                        fillColor: '#62a8ea',// 填充色              
                        data: b, // 数据     
                    }
                ]
            };
            // 柱状图数据
            myChart.data.labels = a;
            myChart.data.datasets[0].data = b;
            myChart.update();
            var VdateBegin = $('#CompareOutFlowCash #monthsStart').val();
            var VdateEnd = $('#CompareOutFlowCash #monthsEnd').val();
            var VAccountCash = $("#AccountCompare").find("option:selected").text();//选中的文本

            $("#CompareOutFlowCashModel #statisticsdateBegin").val(VdateBegin);
            $("#CompareOutFlowCashModel #statisticsdateEnd").val(VdateEnd);
            if (VAccountCash == "") {
                $("#CompareOutFlowCashModel #statisticsCash").val('全部');
            } else {
                $("#CompareOutFlowCashModel #statisticsCash").val(VAccountCash);
            }
            $('#CompareOutFlowCashModel').modal({ show: true, backdrop: 'static' });
            $(".boxed-layout").css("padding-right", "0px");
        },
        error: function () {
            alert('@General.Resource.Common.NoResponse');
        }
    })
    $(".boxed-layout").css("padding-right", "0px");
}





