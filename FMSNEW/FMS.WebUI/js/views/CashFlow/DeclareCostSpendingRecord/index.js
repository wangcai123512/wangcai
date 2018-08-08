//查询净现金流
function DeclareCostSpendingQuery() {
    //查询汇总净现金流账
    $('#DeclareCostSpendingValue').bootstrapTable('destroy');
    $('#DeclareCostSpendingValue').bootstrapTable({
        //查询需要修改
        url: '/DeclareCostSpendingRecord/GetDeclareCostSpendingRecordList',
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
             { field: 'RPerName', width: '130', align: 'center', title: '账号' },
             { field: 'Amount', width: '130', align: 'center', title: '金额', formatter: DecimalFmter },
             { field: 'Currency', width: '130', align: 'center', title: '货币' },
             { field: 'InvType', width: '130', align: 'center', title: '描述' }
        ],
        queryParams: queryParams
    });
}
//查询条件参数
function queryParams(params) {
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        BA_GUID: $('#DeclareCostSpending #SupplierList').val(),
    };
    return temp;
}
//导出
function DeclareCostSpendingExport() {
    $('#DeclareCostSpendingValue').tableExport({
        type: 'excel',
        fileName: '成本外支出',
        escape: false
    });
}







