//页面初始化
$(document).ready(function () {
    InitalDateInput();
    GetInvType("#PurchasingType", "D");
    GetSubType("#PurchasingType", "#PurchasingTypeSub");
    GetInvType("#ManufacturedType", "P");
    GetSubType("#PurchasingType", "#ManufacturedTypeSub");
    GetMMType('#PurchasingTypeSub', '#MaterielManage');

    TaxBind();
    var tax;
    var customer;
    $.ajax({
        url: "/InternalAPI/GetCustomer",
        async: false,
        dataType: "json",
        success: function (d) {
            customer = d;
        }
    });
    var rper = GetSupplier()
    $('#client').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    $("#client").multiselect('dataprovider', customer);
    $("#client").val("").multiselect("refresh");
    $("#client").multiselect('select', standardCoin);

    $('#customer').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    $("#customer").multiselect('dataprovider', rper);
    $("#customer").val("").multiselect("refresh");
    $("#customer").multiselect('select', standardCoin);

    $('#RPer').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false
    });
    $("#RPer").multiselect('dataprovider', rper);
    $('#RPer1').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false
    });
    $("#RPer1").multiselect('dataprovider', rper);
    $('#RPer2').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false
    });
    $("#RPer2").multiselect('dataprovider', rper);

    var currency;
    $.ajax({
        url: "/InternalAPI/GetCommonCurrency",
        async: false,
        dataType: "json",
        success: function (d) {
            currency = d;
        }
    });

    $('#Currency').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false
    });
    $("#Currency").multiselect('dataprovider', currency);
    $("#Currency").val("").multiselect("refresh");

    $('#Currency1').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false
    });
    $("#Currency1").multiselect('dataprovider', currency);
    $("#Currency1").val("").multiselect("refresh");
    $("#Currency1").multiselect('select', standardCoin);
    $('#Currency2').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    $("#Currency2").multiselect('dataprovider', currency);
    $("#Currency2").val("").multiselect("refresh");
    $("#Currency2").multiselect('select', standardCoin);

    $('#customer').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        includeSelectAllOption: true
    });
    $('#state').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        includeSelectAllOption: true
    });
    $('#incomeGrp').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        includeSelectAllOption: true
    });
    $('#InvType').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        includeSelectAllOption: true
    });
    $('#TaxationType').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        includeSelectAllOption: true
    });
    $('#StateResaleType').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: false,
        includeFilterNewBtn: false,
        includeSelectAllOption: true
    });

    $("#ResaleActualAmount").blur(function () {
        SumAmount("#ResaleActualAmount", "#TaxationAmounts", "#SumAmounts")
    });
    $("#TaxationAmounts").blur(function () {
        SumAmount("#ResaleActualAmount", "#TaxationAmounts", "#SumAmounts")
    });

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
            enableFiltering: true,
            includeFilterNewBtn: false,
            nonSelectedText: "请选择业务单元"
        });
        $("#Business_GUID").multiselect('dataprovider', pat);
        $("#Business_GUID").val("").multiselect("refresh");

        $('#SubBusiness_GUID').multiselect({
            buttonWidth: '100%',
            maxHeight: 200,
            enableFiltering: true,
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
            enableFiltering: true,
            includeFilterNewBtn: false,
            nonSelectedText: "请选择业务单元"
        });
        $("#Business_GUID1").multiselect('dataprovider', pat);
        $("#Business_GUID1").val("").multiselect("refresh");

        $('#SubBusiness_GUID1').multiselect({
            buttonWidth: '100%',
            maxHeight: 200,
            enableFiltering: true,
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
            enableFiltering: true,
            includeFilterNewBtn: false,
            nonSelectedText: "请选择业务单元"
        });
        $("#Business_GUID2").multiselect('dataprovider', pat);
        $("#Business_GUID2").val("").multiselect("refresh");

        $('#SubBusiness_GUID2').multiselect({
            buttonWidth: '100%',
            maxHeight: 200,
            enableFiltering: true,
            includeFilterNewBtn: false,
            nonSelectedText: ""
        });
        $("#Business_GUID2").change(function () {
            var guid = $("#Resale_Form #Business_GUID2").val();
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
//获取列表
jQuery(document).ready(function () {
    $('#dataList').bootstrapTable({
        url: "/DirectMaterialPurchasingQuery/GetDirectMaterialPurchasingList",//请求后台的URL（*）
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
        uniqueId: "GUID",
        exportTypes: ['xml', 'txt', 'excel'],
        columns: [[
            { field: 'Date', title: '购入日期', align: 'center', width: '70px', remoteSort: true },
            { field: 'Amount', title: '初始金额', align: 'center', width: '70px', formatter: DecimalFmter },
            { field: 'GUID', title: '已转售', formatter: LinkHandleResaleValue, align: 'center', width: '70px' },
            { field: 'GUID', title: '库存金额', formatter: LinkHandleResidualAmount, align: 'center', width: '70px' },
            { field: 'Inventory_Number', title: '库存数量', align: 'center', width: '70px' },
            { field: 'Currency', title: '货币', align: 'center' },
            { field: "BusinessName" , align: 'center', title:"业务单元" },
            { field: "SubBusinessName", align: 'center', title:"业务子单元" },
            { field: 'AidTypeName', title: '物料类别', align: 'center' },
            { field: 'ASTTypeName', title: '物料子类别', align: 'center' },
            { field: 'MM_Name', title: '物料名称', align: 'center' },
            { field: 'RPerName', title: '供应商', align: 'center' },
            { field: 'GUID', title: '', formatter: LinkHandle }
        ]],
        queryParams: queryParams
    });
});
//配置参数
function queryParams(params) {  //配置参数
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $("#dateBegin").val(),
        dateEnd: $("#dateEnd").val(),
        customer: $("#customer").val(),
        Type: $("#PurchasingType").val(),
        TypeSub: $("#PurchasingTypeSub").val(),
        MaterielManage: $("#MaterielManage").val(),
        //grp: $("#incomeGrp").val(),
        state: $("#state").val(),
        business_GUID: $('#Business_GUID').val(),
        subBusiness_GUID:$('#SubBusiness_GUID').val()
    };
    return temp;
}
//已转售值
var LinkHandleResaleValue = function (value, row, index) {
    if (row.ResaleValue == 0) {
        var link1 = "<p  class='linkbtn ' style='margin-top:-3px'>  \ " + row.ResaleValue + "\</p>"
    } else {
        var link1 = "<a class='linkbtn btn-primary' onclick='ResaleValue(\"" + value + "\")'>  \ " + row.ResaleValue + "\</a>"
    }
    return link1;
};
//库存金额
var LinkHandleResidualAmount = function (value, row, index) {
    if (row.ResidualAmount < 0) {
        var link1 = "<p class='linkbtn' style='margin-top:-3px'>  \ " + '0 ' + "\</p>"
    }
    else if (row.ResidualAmount >= 0) {
        var link1 = "<p class='linkbtn' style='margin-top:-3px'>  \ " + row.ResidualAmount + "\</p>"
    }
    return link1;
};
//编辑转售删除按钮
var LinkHandle = function (value, row, index) {
    if (row.ResaleValue == 0 & row.AmountUsed == 0) {
        var link1 = "<a class='linkbtn' onclick='Edit(\"" + value + "\")'>编辑</a>";
    } else {
        link1 = "";
    }
    if (row.Inventory_Number <= 0) {
        link3 = "";
    } else {
        link3 = " <a class='linkbtn' onclick='Resale(\"" + value + "\")'>转售</a> ";
    }

    if (row.ResaleValue == 0 & row.AmountUsed == 0) {
        var link4 = " <a class='linkbtn' onclick='DelClick(\"" + value + "\")'>删除</a> ";
    } else {
        link4 = "";
    }
    return link1 + link3 + link4;
};

function query() {
    $('#dataList').bootstrapTable('refresh');
}

$(function () {
    $('#editModal').on('hide.bs.modal', function () {
        $.ajax({
            url: "/DirectMaterialPurchasingQuery/DelPic/" + $('#picpath').val(),
            async: false,
            contentType: 'application/json',
            dataType: 'html',
            success: function (data) {
            },
            error: function (err) {
                alert('hide' + err);
            }
        })
    })
});

function DownLoadFile(id) {
    $.ajax({
        url: "/DirectMaterialPurchasingQuery/DownLoadFile/" + id,
        async: false,
        contentType: 'application/json',
        dataType: 'html',
        success: function (data) {
            if (data == '') {
                ShowFile('');
            } else {
                ShowFile("<img src='/img/temp/" + data + "' class='file-preview-image' alt='Desert' title='Desert'>");
                $('#picpath').val(data);
                //alert(data);
            }
        },
        error: function (err) {
            alert(err);
        }
    })
}

function ShowFile(data) {
    $('#input-24').fileinput('destroy');
    FileInputBaseSetting("#input-24");
    $('#input-24').on('filecleared', function (event) {
        $("#changing").val('change');
        //alert('cleared');
    });
    $('#input-24').on('fileloaded', function (event, file, previewId, index, reader) {
        $("#changing").val('change');
        //alert('load');
    });
    $("#input-24").fileinput('refresh', {
        showPreview: true,
        initialPreviewCount: 1,
        showRemove: true,
        initialPreviewShowDelete: false,
        initialPreviewAsData: true,
        overwriteInitial: true,
        initialCaption: "文件不大于2M",
        uploadUrl: '/InternalAPI/FileUpload',
        allowedFileExtensions: ['jpg', 'png'],
        uploadExtraData: function () { // callback example
            var out = {};
            out['frGuid'] = $('#GUID').val();
            return out;
        },
        initialPreviewConfig: [
        {
            caption: 'desert.jpg',
            width: '120px',
            url: '/DirectMaterialPurchasingQuery/DelAttachment/' + $('#GUID').val(),
            key: 100,
            extra: { id: 100 },
            previewAsData: true
        }]
    });

    if (data != '') {
        $("#input-24").fileinput('refresh', {
            initialPreview: [data]
        });
    } else {
        $("#input-24").fileinput('refresh', {
            initialPreview: []
        });
    }

    $('#input-24').on('change', function (event) {
        $("#changing").val('change');
    });
}

var a = 2;
function add(id) {
    var o = document.getElementById("PZ" + id);
    var div = document.createElement("div");
    div.id = "PZ" + id + "_" + a;
    div.className = "col-md-12";
    div.innerHTML = o.innerHTML.replace(/\{0\}/ig, a);
    document.getElementById("AddLayout" + id).appendChild(div);
    //为新增的file控件初始化
    IntiFileUpload('#' + div.id + " #certificate" + id, $('#IE_GUID' + id).val());
    a++;
}

//初始化fileinput控件（第一次初始化）

function IntiFileUpload(selector, frguid) {

    FileInputBaseSetting(selector);
    $(selector).fileinput('refresh', {
        uploadUrl: '/InternalAPI/FileUpload',
        allowedFileExtensions: ['jpg', 'png', 'gif'],
        maxFileCount: 5,
        mainClass: "input-group-lg",
        browseClass: "btn bg-purple2 btn-sm",
        uploadExtraData: function () { // callback example
            var out = {};
            out['frGuid'] = frguid;
            return out;
        }
    });
}

//产生GUID
function NewGUID() {
    var GUID;
    $.ajax({
        url: "/ReceivablesRecord/NewGuid",
        async: false,
        dataType: "text",
        success: function (d) {
            GUID = d.toString();
        }
    });
    return GUID;
}
//已转售方法的跳转以及model的弹出
function ResaleValue(id) {
    $('#dataListResaleValue').bootstrapTable('destroy');
    $('#dataListResaleValue').bootstrapTable({
        url: "/DirectMaterialPurchasingQuery/GetDirectMaterialResaleValuePurchasingList/" + id,//请求后台的URL（*）   
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
        sortName: 'Date', // 设置默认排序为 name
        sortOrder: 'desc', // 设置排序为反序 desc
        search: false, // 开启搜索功能
        showColumns: false,
        showRefresh: false,
        showQuery: false,
        showToggle: false,//是否显示详细视图和列表视图的切换按钮
        showExport: false,
        exportTypes: ['xml', 'txt', 'excel'],
        columns: [[
             //由于IE_Record的model没有修改date类型所以在已转售的时间格式需要转换
            { field: 'Date', title: '转售时间', align: 'center', width: '150px', formatter: DateHandle, remoteSort: true },
            { field: 'Resale_Amount', title: '转售金额', align: 'center', width: '100px' },
            { field: 'Amount', title: '收入金额', align: 'center', width: '100px' },
            { field: 'Currency', title: '货币', align: 'center' },
            { field: 'RPerName', title: '客户', align: 'center' },
        ]],
        queryParams: queryParams
    });
    $('#ResaleValue').modal({ show: true, backdrop: 'static' });
}
function Edit(id) {
    $.ajax({
        url: "/DirectMaterialPurchasingQuery/GetAIDRecord/" + id,
        async: false,
        contentType: 'application/json',
        dataType: 'html',
        success: function (data) {
            var obj = JSON.parse(data);
            if (obj.InvType == "" || obj.InvType == null) {
                GetInvType("#EditPurchasingType", "D");
                GetSubType("#EditPurchasingType", "#EditPurchasingTypeSub");
                GetMMType("#EditPurchasingTypeSub", "#EditMaterielManage");

                $("#EditMaterielManage").multiselect('select', obj.MaterielManage);
            } else {
                GetInvType("#EditPurchasingType", "D");

                $("#EditPurchasingType").multiselect('select', obj.InvType);

                GetSubType("#EditPurchasingType", "#EditPurchasingTypeSub");

                $("#EditPurchasingTypeSub").multiselect('select', obj.SubType);

                GetMMType("#EditPurchasingTypeSub", "#EditMaterielManage");

                $("#EditMaterielManage").multiselect('select', obj.MaterielManage);
            }
            if(obj.Business_GUID != null){
            $('#Business_GUID1').multiselect('select', obj.Business_GUID);
                        var guid = obj.Business_GUID;
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
                        $('#SubBusiness_GUID1').multiselect('select', obj.SubBusiness_GUID);
                        }else{
                            $('#Business_GUID1').val("").multiselect("refresh");
                            $('#SubBusiness_GUID1').val("").multiselect("refresh");
                              }
            $('#GUID').val(obj.GUID);
            $('#C_GUID').val(obj.C_GUID);
            $('#AID_Flag').val(obj.AID_Flag);
            $('#Remark').val(obj.Remark);
            $('#CostType').val(obj.CostType);
            $('#SurplusValue').val(obj.SurplusValue);
            $('#State').val(obj.State);
            $('#Date').val(obj.Date);
            $('#Date').datepicker('update');
            $('#Amount').val(obj.Amount);
            $('#Currency').multiselect('select', obj.Currency);
            $('#RPer').multiselect('select', obj.RPer);
            $('#Description').val(obj.Description);
            $('#DepreciationPeriod').val(obj.DepreciationPeriod);
            DownLoadFile(id)
        },
        error: function (err) {
            alert(err);

        }
    })

    $('#editModal').modal({ show: true, backdrop: 'static' });

}
//编辑model框提交
function SaveChanges() {
    if ($("#EditMaterielManage").val() == null) {
        alert("物料类别或物料子类别，物料名称必填");
        return false;
    }
    $.ajax({
        cache: true,
        type: "POST",
        url: "/DirectMaterialPurchasingQuery/UpdDirectMaterialPurchasingRecord",
        data: $('#D_Form').serialize(),
        async: false,
        onSubmit: function () {
            return $("#D_Form").form('validate');
        },
        error: function (request) {
            alert("Connection error");
        },
        success: function (data) {
            //提交成功
            if ($("#changing").val() == 'change') {
                delAttach($('#GUID').val());
                $("#input-24").each(function () {
                    $(this).fileinput('upload')
                });
            }
        }
    });
    $('#editModal').modal('hide');
    $('#dataList').bootstrapTable('refresh');
}
//删除附件
function delAttach(id) {
    $.ajax({
        url: "/DirectMaterialPurchasingQuery/DelAttachment/" + id,
        type: "POST",
        success: function (data) {
            $("#input-24").each(function () {
                $(this).fileinput('upload')
            });
        },
        error: function () {
            alert('@General.Resource.Common.NoResponse');
        }
    });
}
//点击转售按钮给转售model赋值
function Resale(guid) {
    clear(2)
    //初始化fileinput控件（第一次初始化）
    var row = $('#dataList').bootstrapTable('getRowByUniqueId', guid);
    $("#resaleModal #tmpAID_GUID2").val(guid);
    $("#resaleModal #IE_GUID2").val(guid);
    $("#resaleModal #InventoryQuantity").val(row.Inventory_Number);
    $("#resaleModal #InitialAmount").val(row.ResidualAmount);
    $("#resaleModal #Inventory_Number").val(row.Inventory_Number);
    $("#resaleModal #Amountinfosd").val(row.Amount);
    $("#resaleModal #Currency2").val("").multiselect("refresh");
    $('#AffirmDate2').datepicker();
    $('#Date2').datepicker();
    IntiFileUpload("#PZ2_1 #certificate2");
    DownLoadFile(guid);
    //通过库存金额和库存数量计算单价
    CalculatePrice(row);
    $('#resaleModal').modal({ show: true, backdrop: 'static' });

}
//通过库存金额和库存数量计算单价
function CalculatePrice(row) {
    //初始金额
    var Amount = row.Amount;
    //初始数量
    var MaterialNumber = row.MaterialNumber;
    var unitprice = Amount / MaterialNumber;
    $("#resaleModal #unitprice").val(unitprice);
}
function clear(id) {
    for (i = 2; i <= 20; i++) {
        $('#PZ_' + i).remove();
    }
    $('#Amount' + id).val("");
    $('#TaxationAmount' + id).val("");
    $('#SumAmount' + id).val("");
    $('#Remark' + id).val("");
    $('#IEDescription' + id).val("");
    $('#PZ' + id + '_1 #Pnumber' + id).val("");
    $('#PZ' + id + ' #Pnumber' + id).val("");
}
//转售model框提交
function ResaleSubmit() {
    var StateResaleType = $("#StateResaleType").val();
    var client = $("#client").val();
    var Currency2 = $("#Currency2").val();
    var affirmDate = $("#AffirmDate2").val();
    var date = $("#Date2").val();
    var InitialAmount = $("#InitialAmount").val();
    var Amountinfos = $("#Amountinfos").val();
    //库存数量        
    var Inventory_Number = $("#Inventory_Number").val();
    //转售数量
    var ResaleNumber = $("#ResaleNumber").val();
    var ResaleActualAmount = $("#ResaleActualAmount").val();
    var TaxationType = $("#TaxationType").val();
    var TaxationAmounts = $("#TaxationAmounts").val();
    if (StateResaleType == "") {
        alert("请选择转售类型");
        error = true;
        return false;
    }
    if (client == null) {
        alert("请选择客户");
        error = true;
        return false;
    }
    if (Currency2 == null) {
        alert("请选择货币");
        error = true;
        return false;
    }
    if (Amountinfos == "") {
        alert("请输入转售金额");
        error = true;
        return false;
    }
    //转售金额不得大于库存金额       
    if (parseInt(Amountinfos) > parseInt(InitialAmount)) {
        alert("转售金额不得大于库存金额");
        error = true;
        return false;
    }
    if (ResaleNumber == "") {
        alert("请输入转售数量");
        error = true;
        return false;
    }
    //转售数量不得大于库存数量       
    if (parseInt(ResaleNumber) > parseInt(Inventory_Number)) {
        alert("转售数量不得大于库存数量");
        error = true;
        return false;
    }
    if (ResaleActualAmount == "") {
        alert("请输入收入金额");
        error = true;
        return false;
    }
    if (TaxationType == "") {
        alert("请选择税种");
        error = true;
        return false;
    }
    if (TaxationAmounts == "") {
        alert("请输入税费金额");
        error = true;
        return false;
    }
    if (affirmDate == "" || date == "") {
        alert("请选择收入日期和截止日期!");
    } else if (Date.parse(affirmDate) > Date.parse(date)) {
        alert("收入确认日期不能大于账期截止日期，请重新选择！");
    } else {
        $.ajax({
            cache: true,
            type: "POST",
            url: "/DirectMaterialPurchasingQuery/UpdResaleExpenseRecord",
            data: $('#Resale_Form').serialize(),
            async: false,
            onSubmit: function () {
                return $("#Resale_Form").form('validate');
            },
            error: function (request) {
                alert(request.toString());
            },
            success: function (data) {
                //提交成功，开始上传附件
                $("[name=certificate2]").each(function () {
                    $(this).fileinput('upload')
                })
                ChangeStatus($('#tmpAID_GUID2').val(), '转售');
                ResaleClose();
                $('#resaleModal').modal('hide');
                alert("转售成功");
            }
        });
    }
}
//转售model框的关闭
function ResaleClose() {
    $("#StateResaleType").val("").multiselect("refresh");
    $("#client").val("").multiselect("refresh");
    $("#Currency2").val("").multiselect("refresh");
    $('#AffirmDate2').val('');
    $('#Date2').val('');
    $('#Amountinfos').val('');
    $('#ResaleActualAmount').val('');
    $("#TaxationType2").val("").multiselect("refresh");
    $('#TaxationAmounts').val('');
    $('#ResaleNumber').val('');
    $('#SumAmounts').val('');
    $('#PZ2 #Pnumber2').val('');
    $('#PZ2_1 #Pnumber2').val('');
    clear();
    $('#Remarkinfo').val('');
}
function clear(id) {
    for (i = 2; i <= 20; i++) {
        $('#PZ2_' + i).remove();
    }
}
//更改状态
function ChangeStatus(guid, state) {
    $('#dataList').bootstrapTable('refresh');
}
//删除采购物料记录
function DelClick(id) {
    if (confirm('确认删除?')) {
        $.ajax({
            url: "/DirectMaterialPurchasingQuery/DelDirectMaterialPurchasingRecord/" + id,
            type: "POST",
            success: function (data) {
                var res = JSON.parse(data);
                alert(res.Msg);
                query();
            },
            error: function () {
                alert('@General.Resource.Common.NoResponse');
            }
        });
    }
}
//新客户登记跳转以及session的记录
function AddCustomer(obj) {
    var c_AffirmDate = $('#AffirmDate').val();
    var c_Date = $('#Date').val();
    var c_Amount = $('#Amount').val();
    var c_Currency = $('#Currency').val();
    var c_TaxationType = $('#TaxationType').val();
    var c_TaxationAmount = $('#TaxationAmount').val();
    var c_SumAmount = $('#SumAmount').val();
    var c_Remark = $('#Remark').val();
    var c_InvNo = $('#InvNo').val();
    var c_InvType = $('#InvType').val();
    var c_IE_GUID = $('#IE_GUID').val();
    var c_State = $('#State').val();
    var c_SumAmount1 = $('#SumAmount1').val();
    var c_TaxationAmount1 = $('#TaxationAmount1').val();
    var c_Amount1 = $('#Amount1').val();

    $.cookie('c_AffirmDate', c_AffirmDate, { expires: 7 }); // 存储 cookie 
    $.cookie('c_Date', c_Date, { expires: 7 }); // 存储 cookie 
    $.cookie('c_Amount', c_Amount, { expires: 7 }); // 存储 cookie 
    $.cookie('c_Currency', c_Currency, { expires: 7 }); // 存储 cookie 
    $.cookie('c_TaxationType', c_TaxationType, { expires: 7 }); // 存储 cookie 
    $.cookie('c_TaxationAmount', c_TaxationAmount, { expires: 7 }); // 存储 cookie 
    $.cookie('c_SumAmount', c_SumAmount, { expires: 7 }); // 存储 cookie 
    $.cookie('c_Remark', c_Remark, { expires: 7 }); // 存储 cookie 
    $.cookie('c_InvNo', c_InvNo, { expires: 7 }); // 存储 cookie 
    $.cookie('c_IE_GUID', c_IE_GUID, { expires: 7 }); // 存储 cookie 
    $.cookie('c_State', c_State, { expires: 7 }); // 存储 cookie 
    window.location.href = '/BusinessPartnerSetting/BusinessPartner?IsCustomer=1&CustomerType=' + obj;
}
//编辑model提交按钮
function ImportPayable() {
    var items = $('#ClientlistTable').bootstrapTable('getSelections');
    $.each(items, function (index, item) {
        $("#IE_GUID2").val(item.GUID);
        $("#RPer2").multiselect('select', item.RPerName);
        $("#Currency2").multiselect('select', item.Currency);
        $('#Amountinfos').val(parseFloat(item.Amount));
    });
    $('#ClientslistModal').modal('hide');
}
//日期控件
var DateHandle = function (jsondate) {
    jsondate = jsondate.replace("/Date(", "").replace(")/", "");
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
//判断物料状态
function ResaleTypeChange() {
    var StateResaleType = document.getElementById('StateResaleType');
    var StateResaleTypes = StateResaleType.value;
    if (StateResaleTypes == "用完") {
        InitialAmount = $("#resaleModal #InitialAmount").val();
        $('#Amountinfos').val(InitialAmount);
        var EditAmountinfos = document.getElementById("Amountinfos");
        EditAmountinfos.setAttribute("readOnly", 'true');
    }
    if (StateResaleTypes == "存货") {
        $('#Amountinfos').val("");
        var Amountinfos = document.getElementById("Amountinfos");
        Amountinfos.readOnly = false;
    }
    if (StateResaleTypes == "") {
        $('#Amountinfos').val("");
        var Amountinfos = document.getElementById("Amountinfos");
        Amountinfos.readOnly = false;
    }
}

