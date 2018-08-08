$(document).ready(function () {
    //供应商
    InitalDateInput();
    var rper;
    $.ajax({
        url: "/InternalAPI/GetPayeeJson",
        async: false,
        dataType: "json",
        success: function (d) {
            rper = d;
        }
    });
    $('#Supplier').multiselect({
        buttonWidth: '100%',
        maxHeight:150,
        enableFiltering: false,
        selectAllText: '全部',
        selectAllValue:'select-all-value',
        includeFilterNewBtn:false,
        nonSelectedText:'请选择',
        includeSelectAllOption: true,
        
    });
    $("#Supplier").multiselect('dataprovider', rper);
    $("#Supplier").val("").multiselect("refresh");
});
//查询供应商成本与费用
function SupplierCostsExpensesRecordQuery() {
    //获取时间
    var DateBegin = $('#SupplierCostsExpensesRecord #dateBegin').val();
    var DateEnd = $('#SupplierCostsExpensesRecord #dateEnd').val();
    var Supplier = $('#SupplierCostsExpensesRecord #Supplier').val();
    if (DateBegin == "" || DateEnd == "") {
        alert("请选择开始日期和截止日期!");
    } else if (Date.parse(DateBegin) > Date.parse(DateEnd)) {
        alert("开始日期不能大于截止日期，请重新选择！");
    }
    else if (Supplier == "") {
        alert("请选择供应商！");
    }
    else {
        $('#SupplierTotalCollectList').bootstrapTable('destroy');
        $('#SupplierTotalCollectList').bootstrapTable({
            url: '/CostsAndExpensesSupplierRecord/GetSupplierTotalCollectList',
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
            queryParams: queryParamsSunpplierTC
        });
        $('#SupplierTotalList').bootstrapTable('destroy');
        $('#SupplierTotalList').bootstrapTable({
            url: '/CostsAndExpensesSupplierRecord/GetSupplierTotalList',
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
                { field: 'RPer', width: '130', align: 'center', title: '' },
                { field: 'SumAmount', width: '130', align: 'center', title: '', formatter: DecimalFmter },
                { field: 'Currency', width: '130', align: 'center', title: '' },
                { field: 'IE_GUID', width: '130', align: 'center', title: '', formatter: LinkHandleRatioValue }
            ],
            queryParams: queryParamsSunpplierT
        });
        var VdateBegin = $('#SupplierCostsExpensesRecord #dateBegin').val();
        var VdateEnd = $('#SupplierCostsExpensesRecord #dateEnd').val();
        var VSupplier = $("#SupplierCostsExpensesRecord #Supplier").find("option:selected").text();//选中的文本
        $("#SupplierCostsExpensesModel #dateBegin").val(VdateBegin);
        $("#SupplierCostsExpensesModel #dateEnd").val(VdateEnd);
        $("#SupplierCostsExpensesModel #Supplier").val(VSupplier);
        $('#SupplierCostsExpensesModel').modal({ show: true, backdrop: 'static' });
        $(".boxed-layout").css("padding-right", "0px");
    }
}
//查询条件参数1
function queryParamsSunpplierTC(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
    };
    return temp;
}
//查询条件参数2
function queryParamsSunpplierT(params) {
    var str = "";
    Supplier = $('#SupplierCostsExpensesRecord #Supplier').val();
    for (var i = 0; i < Supplier.length; i++) {
        str += Supplier[i] + ",";
    }
    //去掉最后一个逗号(如果不需要去掉，就不用写)
    if (str.length > 0) {
        str = str.substr(0, str.length - 1);
    }
    var temp = {
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $('#SupplierCostsExpensesRecord #dateBegin').val(),
        dateEnd: $('#SupplierCostsExpensesRecord #dateEnd').val(),
        RPers: str,
    };
    return temp;
}
//占比率
var LinkHandleRatioValue = function (value, row, index) {
    var link1 = "<p class='' >  \ " + (row.ReceiveRatio * 100).toFixed(2) + '%' + "\</p>";
    return link1;
};
//客户（首行）
var LinkHandleInvType = function (value, row, index) {
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
//一级费用科目分类的总成本与费用(导出功能)
function SupplierExport() {
    $('#supplierform').tableExport({
        type: 'excel',
        fileName: '供应商成本与费用',
        escape: false
    });
}