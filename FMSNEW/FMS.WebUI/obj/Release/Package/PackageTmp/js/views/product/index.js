
jQuery(document).ready(function () {

    GetInvType("#productType", "P");

    GetSubType("#productType", "#productTypeSub");

    GetMMTypeNA("#productTypeSub", "#MaterielManage");

    query("init");

    $('#sales').on('show.bs.modal', function (e) {

        $('#IEList').show();
        $('#btnSalesSubmit').show();
        $('#btnSalesAndIncome').show();


    })
    var pat;
    $.ajax({
        url: "/BusinessUnitSetting/GetBusinessTypeList",
        async: false,
        dataType: "json",
        success: function (d) {
            pat = d;
        }
    });

    $('#Business_GUID').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: "请选择业务单元"
    });
    $("#Business_GUID").multiselect('dataprovider', pat);
    $("#Business_GUID").val("").multiselect("refresh");

    $('#SubBusiness_GUID').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: ""
    });
    $("#Business_GUID").change(function () {
        var guid = $("#Business_GUID").val();
        var subList
        $.ajax({
            url: "/BusinessUnitSetting/GetBusinessChildTpyList?GUID=" + guid,
            async: false,
            dataType: "json",
            success: function (d) {
                subList = d;
            }
        });
        $("#SubBusiness_GUID").multiselect('dataprovider', subList);
        $("#SubBusiness_GUID").val("").multiselect("refresh");

    });
    $('#Business_GUID1').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: "请选择业务单元"
    });
    $("#Business_GUID1").multiselect('dataprovider', pat);
    $("#Business_GUID1").val("").multiselect("refresh");

    $('#SubBusiness_GUID1').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: ""
    });
    $("#Business_GUID1").change(function () {
        var guid = $("#editModal #Business_GUID1").val();
        var subList
        $.ajax({
            url: "/BusinessUnitSetting/GetBusinessChildTpyList?GUID=" + guid,
            async: false,
            dataType: "json",
            success: function (d) {
                subList = d;
            }
        });
        $("#SubBusiness_GUID1").multiselect('dataprovider', subList);
        $("#SubBusiness_GUID1").val("").multiselect("refresh");

    });
    $('#Business_GUID2').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: "请选择业务单元"
    });
    $("#Business_GUID2").multiselect('dataprovider', pat);
    $("#Business_GUID2").val("").multiselect("refresh");

    $('#SubBusiness_GUID2').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        nonSelectedText: ""
    });
    $("#Business_GUID2").change(function () {
        var guid = $("#Income_Form #Business_GUID2").val();
        var subList
        $.ajax({
            url: "/BusinessUnitSetting/GetBusinessChildTpyList?GUID=" + guid,
            async: false,
            dataType: "json",
            success: function (d) {
                subList = d;
            }
        });
        $("#SubBusiness_GUID2").multiselect('dataprovider', subList);
        $("#SubBusiness_GUID2").val("").multiselect("refresh");

    });

});

function queryParams(params) {  //配置参数
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        typeId: $("#productType").val(),
        subTypeId: $("#productTypeSub").val(),
        MaterielManage: $("#MaterielManage").val(),
        business_GUID: $('#Business_GUID').val(),
        subBusiness_GUID: $('#SubBusiness_GUID').val()
    };
    return temp;
}

var LinkUsed = function (value, row, index) {

    var link = ""
    if (value > 0) {
        link = "<a class='linkbtn btn-primary' onclick='GetUsedDetail(\"" + row.GUID + "\")'>  \ " + value + "\</a>"
    } else {
        link = value
    }
    return link;
};

var LinkSaled = function (value, row, index) {

    var link = ""
    if (value > 0) {
        link = "<a class='linkbtn btn-primary' onclick='GetSaledDetail(\"" + row.GUID + "\")'>  \ " + value + "\</a>"
    } else {
        link = value
    }

    return link;
};

var LinkHandle = function (value, row, index) {

    var delLink = "";
    var detailLink = "<a    onclick='Detail(\"" + row.GUID + "\")'>明细</a> "
    var sealedLink = "<a class='linkbtn' onclick='SalesShow(\"" + row.GUID + "\")'>核销</a> "

    //if (row.Parent_GUID == null) {
    //    useLink = " <a class='linkbtn' onclick='UseShow(\"" + row.GUID + "\")'>使用</a> ";
    //}

    if (row.saled_count == 0) {
        delLink = " <a class='linkbtn' onclick='DeleteProduct(\"" + row.GUID + "\")'>删除</a> <a   href='Edit/" + row.GUID + "'>编辑</a> ";

        //editLink = "<a class='linkbtn' onclick='EditShow(\"" + row.GUID + "\")'>编辑</a>";

    } else if (row.stock_count == 0) {
        sealedLink = "";
    }

    return detailLink + sealedLink + delLink;
};

function query(IsInit) {
    if (IsInit == undefined) {
        if ($('#Business_GUID').val() == "" || $('#SubBusiness_GUID').val() == "" || $('#productType').val() == "" || $('#productTypeSub').val() == "" || $('#MaterielManage').val() == "") {
            alert("请选择产品类别和业务单元条件");
            return false;
        }
    }

    var cols = [[
            { field: 'Create_Date', title: '制造日期', align: 'center', width: '70px', remoteSort: true },
            { field: 'stock_count', title: '库存', align: 'center', width: '70px' },
            { field: 'saled_count', title: '已核销', formatter: LinkSaled, align: 'center', width: '70px' },
             { field: 'type_name', title: '产品类别', align: 'center' },
            { field: 'sub_type_name', title: '产品子类别', align: 'center' },
            { field: 'MM_Name', title: '产品', align: 'center' },
            { field: 'GUID', title: '', formatter: LinkHandle }
    ]]
    DataBind("#dataList", "/ProductManage/GetProductList", cols, "queryParams");
}


function GetUsedDetail(guid) {
    var url = "/ProductManage/GetUsedDetail"
    var cols = [[
           { field: 'Create_Date', title: '制造日期', align: 'center', width: '70px' },
           { field: 'Amount', title: '使用金额', align: 'center', width: '70px', formatter: DecimalFmter },
           { field: 'Currency', title: '货币', align: 'center' },
            { field: 'type_name', title: '产品类别', align: 'center' },
           { field: 'sub_type_name', title: '产品子类别', align: 'center' }
    ]]
    DataBind("#usedDetail", url, cols, { pguid: guid });
    $("#amyModalLabel").text("已使用明细");

    $('#AmountUsed').modal({ show: true, backdrop: 'static' });
}


function GetSaledDetail(guid) {
    var url = "/ProductManage/GetSaledDetail"
    var cols = [[
           { field: 'saled_date', title: '核销日期', align: 'center', width: '70px' },
           { field: 'saled_amount', title: '核销产品数量', align: 'center', width: '70px', formatter: DecimalFmter },
           { field: 'Currency', title: '货币', align: 'center' },
            { field: 'ie_guid', title: '收入的ID', align: 'center' },
            { field: 'SumAmount', title: '收入总金额', align: 'center' },
            { field: 'customer_name', title: '客户姓名', align: 'center' },

    ]]
    DataBind("#usedDetail", url, cols, { pguid: guid });
    $("#amyModalLabel").text("已核销明细");
    $('#saledDetail').modal({ show: true, backdrop: 'static' });
}


function DataBind(selector, url, columns, queryParams) {

    $(selector).bootstrapTable('destroy');

    $(selector).bootstrapTable({
        url: url,//请求后台的URL（*）
        method: 'get',//请求方式（*）
        pageSize: 10,//每页的记录行数（*）
        pageList: [10, 20, 50, 100, 200], // 自定义分页列表
        pageNumber: 1, //初始化加载第一页，默认第一页  
        checkOnSelect: true,
        selectOnCheck: true,
        cardView: false,//是否显示详细视图
        pagination: true, //是否显示分页（*）        
        uniqueId: "GUID",
        singleSelect: true,   //单选
        search: false, // 开启搜索功能
        showColumns: false,
        showRefresh: false,
        showQuery: false,
        showToggle: false,//是否显示详细视图和列表视图的切换按钮
        showExport: false,
        exportTypes: ['xml', 'txt', 'excel'],
        columns: columns,
        queryParams: queryParams,
        silent: true
    });


}
/*
*   编辑页面
*/
function EditShow(guid) {
    $("#edit_form input").val("");
    var row = $('#dataList').bootstrapTable('getRowByUniqueId', guid);
    $("#editModal #create_date").val(row.Create_Date);
    $("#editModal #amount").val(row.Amount);
    $("#editModal #stock_amount").val(row.Stock_Amount);
    $("#editModal #currency").val(row.Currency);
    $("#editModal #hidGUID").val(row.GUID);

    GetInvType("#productTypeEdit", "P");

    $("#productTypeEdit").multiselect('select', row.TypeId);

    GetSubType("#productTypeEdit", "#productTypeSubEdit");
    $("#productTypeSubEdit").multiselect('select', row.SubTypeId);

    $('#editModal').modal({ show: true, backdrop: 'static' });
}

/*
编辑提交
*/
function EditSubmit() {
    if ($("#productTypeSubEdit").val() == "" || $("#productTypeSubEdit").val() == null) {
        alert("请选择产品子类");
        return false;
    }

    $.ajax({
        url: "/ProductManage/UpdateProduct",
        type: "POST",
        async: false,
        data: $("#edit_form").serialize(),
        dataType: "JSON",
        ContentType: "application/json",
        success: function (data) {
            if (data.success) {
                $('#editModal').modal("hide");
                query();
            } else {
                alert(data.msg);
            }
        },
        error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
            alert(event);
        }

    })
}



function DeleteProduct(delGuid) {
    if (!confirm("是否要删除该产品。")) {
        return false;
    }

    $.ajax({
        url: "/ProductManage/DeleteProduct",
        type: "POST",
        async: false,
        dataType: "JSON",
        data: { guid: delGuid },
        ContentType: "application/json",
        success: function (data) {
            if (data.success) {
                query();
            } else {
                alert(data.msg);
            }
        },
        error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
            alert(event);
        }

    })
}

/*
    核销
*/
function SalesShow(guid) {
    var url = "/ProductManage/GetIEList"
    var cols = [[
            { field: '', title: '', checkbox: true, width: '70px' },
            { field: 'CreateDate', title: '记录日期', align: 'center', width: '70px' },
            { field: 'Amount', title: '收入金额', align: 'center', width: '70px', formatter: DecimalFmter },
            { field: 'Currency', title: '货币', align: 'center' },
            { field: 'RPerName', title: '客户名称', align: 'center' },
            { field: 'IE_GUID', title: '客户名称', visible: false },

    ]]
    DataBind("#productSalesList", url, cols, "");

    var row = $('#dataList').bootstrapTable('getRowByUniqueId', guid);

    $("#stockAmount").val(row.stock_count);

    $("#salesAmount").val("");
    $("#salesAmount").attr("max", row.stock_count);

    $("#hidSalesPID").val(guid);

    $('#sales').modal({ show: true, backdrop: 'static' });
}

function SalesSubmit(IsCreate) {
    if (!$("#salesFrm").valid()) {
        return false;
    }
    if (IsCreate == false) {
        var selRows = $("#productSalesList").bootstrapTable('getSelections');
        if (selRows.length == 0) {
            alert("请选择要核销的对应收入");
            return false;
        }
        var ieId = new Array();

        for (var i = 0; i < selRows.length; i++) {
            ieId.push(selRows[i].IE_GUID);
        }

        var pid = $("#hidSalesPID").val();
        var amount = $("#salesAmount").val();
        var stockAmount = $("#stockAmount").val();
        var ieIds = ieId.join(",");
        $.ajax({
            url: "/ProductManage/ProductSales",
            type: "POST",
            async: false,
            dataType: "JSON",
            data: { productGuid: pid, stockAmount:stockAmount,saledCount: amount, ieGuidList: ieIds },
            ContentType: "application/json",
            success: function (data) {
                if (data.success) {
                    $('#sales').modal("hide");
                    location.reload()
                } else {
                    alert(data.msg);
                }
            },
            error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
                alert(ajaxOptions + event.responseText);
            }
        })
    }
    else {
        //$('#IEList').hide();
        //$('#btnSalesSubmit').hide();
        //$('#btnSalesAndIncome').hide();

        ShowIncom();


    }

} 

function ShowIncom() {
    TaxBind();
    CurrenyBind();

     $('#IncomeModal [type=text]').val("");

    $('#IncomeModal').modal("show");

}

/*
产品明细
*/
function Detail(guid) {
    // Some logic to retrieve, or generate tree structure
    $.ajax({
        url: "/ProductManage/GetProductDetail",
        type: "POST",
        async: false,
        data: { pId: guid },
        dataType: "JSON",
        ContentType: "application/json",
        success: function (data) {
            if (data.success == true) {
                ShowTree(data.msg);
                $('#detailModal').modal({ show: true, backdrop: 'static' });
            } else {
                alert(data.msg);
            }

        },
        error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
            alert(event);
        }

    })
}


function CurrenyBind() {
    $('#Currency').multiselect(SelectConfigSetting);
    $("#Currency").multiselect('dataprovider', currency);
    $("#Currency").multiselect('select', standardCoin);

}

function SumintIncome() {
    if ($("#autocomplete").val() == "") {
        alert("请选择客户");
        return false;
    }
    if ($("#AffirmDate").val() == "") {
        alert("请选择收入确认日期");
        return false;
    }
    if ($("#Date").val() == "") {
        alert("请选择账期截止日期");
        return false;
    }
    if ($("#Amount").val() == "") {
        alert("请选择收入金额");
        return false;
    }
    if ($("#Currency").val() == null) {
        alert("请选择货币");
        return false;
    }


    if ($("#Amount").val() <= 0) {
        alert("收入金额必须大于0");
        return false;
    }

    var amount = document.getElementById('Amount').value;
    amount = amount.replace(/,/g, "");
    $("#Amount1").val(amount);

    var taxationAmount = document.getElementById('TaxationAmount').value;
    taxationAmount = taxationAmount.replace(/,/g, "");
    $("#TaxationAmount1").val(taxationAmount);

    var sumAmount = document.getElementById('SumAmount').value;
    sumAmount = sumAmount.replace(/,/g, "");
    $("#SumAmount1").val(sumAmount);


    var affirmDate = $("#AffirmDate").val();
    var endDate = $("#Date").val();

    if (Date.parse(affirmDate) > Date.parse(endDate)) {
        alert("收入确认日期不能大于账期截止日期，请重新选择！");
        return false;
    }

    var ieDetail = new Array()

    ieDetail.push($('#Log').val());
    ieDetail.push(affirmDate);
    ieDetail.push(endDate);
    ieDetail.push($("#Currency").val());
    ieDetail.push(amount);
    ieDetail.push($("#TaxationType").val());
    ieDetail.push(taxationAmount);
    ieDetail.push(sumAmount);
    ieDetail.push($("#Remark").val());
    ieDetail.push($("#Business_GUID2").val());
    ieDetail.push($("#SubBusiness_GUID2").val());

    var pid = $("#hidSalesPID").val();
    var amount = $("#salesAmount").val();
    var stockAmount = $("#stockAmount").val();
    var ieRec = ieDetail.join(",");

    $.ajax({
        url: "/ProductManage/ProductSales",
        type: "POST",
        async: false,
        dataType: "JSON",
        data: { productGuid: pid, stockAmount: stockAmount, saledCount: amount, IEDetail: ieRec },
        ContentType: "application/json",
        success: function (data) {
            if (data.success) {
                $('#sales').modal("hide");
                $('#IncomeModal').modal("hide");

                location.reload() 
            } else {
                alert(data.msg);
            }
        },
        error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
            alert(ajaxOptions + event.responseText);
        }
    })
}