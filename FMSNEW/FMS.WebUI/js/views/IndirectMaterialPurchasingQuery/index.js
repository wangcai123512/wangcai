//获取物料状态
 var AssetState = [{ label: "请选择", value: "" },
                 { label: "库存", value: "库存" },
                 { label: "已转售", value: "已转售" },
 ];
//初始化值
$(document).ready(function () {
    GetInvType("#PurchasingType", "I");
    GetSubType("#PurchasingType", "#PurchasingTypeSub");
    GetInvType("#ManufacturedType", "P");
    GetSubType("#ManufacturedType", "#ManufacturedTypeSub");
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

    $('#state').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false
    });
    $("#state").multiselect('dataprovider', AssetState);
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
        includeFilterNewBtn: false,
      
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
    $('#RealseCurrency').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    $("#RealseCurrency").multiselect('dataprovider', currency);
    $("#RealseCurrency").val("").multiselect("refresh");
    $("#RealseCurrency").multiselect('select', standardCoin);

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
});
//获取列表
jQuery(document).ready(function () {
        $('#dataList').bootstrapTable({
            url: "/IndirectMaterialPurchasingQuery/GetIndirectMaterialPurchasingList",
            method: 'get',
            pageSize: 10,
            pageList: [10, 20, 50, 100, 200], // 自定义分页列表
            pageNumber: 1,
            singleSelect: true,
            clickToSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,
            pagination: true,
            sortName: 'Date', // 设置默认排序为 name
            sortOrder: 'desc', // 设置排序为反序 desc
            search: false, // 开启搜索功能
            showColumns: false,
            showRefresh: false,
            showQuery: false,
            showToggle: false,
            showExport: false,
            uniqueId: "GUID",
            exportTypes: ['xml', 'txt', 'excel'],
            columns: [[
				{ field: 'Date', title: '购入日期', align: 'center', width: '70px', remoteSort: true },
                { field: 'Amount', title: '初始金额', align: 'center', width: '70px', formatter: DecimalFmter },
                //{ field: 'GUID', title: '已使用', formatter: LinkHandleAmountUsed, align: 'center', width: '70px' },
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
        state: $("#state").val(),
        business_GUID: $('#Business_GUID').val(),
        subBusiness_GUID:$('#SubBusiness_GUID').val()
    };
    return temp;
}
//金额js
var DecimalFmter = function (s) {
    if (s == null || s == "undefined") {
        return "";
    }
    var h = '';
    s = s.toString();
    if (s.charAt(0) == '-') {
        h = '-';
        s = s.slice(1);
    }
    if (/[^0-9\.]/.test(s)) return "NaN";
    s = s.replace(/^(\d*)$/, "$1.");
    s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
    s = s.replace(".", ",");
    var re = /(\d)(\d{3},)/;
    while (re.test(s)) s = s.replace(re, "$1,$2");
    s = s.replace(/,(\d\d)$/, ".$1");
    return h + s.replace(/^\./, "0.");

}
//已转售
var LinkHandleResaleValue = function (value, row, index) {
    if (row.ResaleValue == 0) {
        var link1 = "<p  class='linkbtn ' style='margin-top:-3px'>  \ " + row.ResaleValue + "\</p>"
    } else {
        var link1 = "<a class='linkbtn btn-primary' onclick='ResaleValue(\"" + value + "\")'>  \ " + row.ResaleValue + "\</a>"
    }
    return link1;
};
//剩余金额
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
        var link1 = "<a class='linkbtn' onclick='EditClick(\"" + value + "\")'>编辑</a>";
    } else {
        link1 = "";
    }
    var link3;
    if (row.ResidualAmount <= 0) {
        link3 = "";
    } else {
        link3 = " <a class='linkbtn' onclick='ResaleClick(\"" + row.GUID + "\")'>转售</a> ";
    }

    if (row.ResaleValue == 0 & row.AmountUsed == 0) {
        var link4 = " <a class='linkbtn' onclick='DelClick(\"" + value + "\")'>删除</a> ";
    } else {
        link4 = "";
    }
    return link1 + link3 + link4;
};
//附件
var FJHandle = function (value, row, index) {
    if (value == "" || value == null) {
        return "";
    } else {
        var v = "../Content/EasyUI/themes/icons/hxz.png";
        return '<img style="height: 16px;width: 16px;" src="' + v + '" />';
    }
};

function query() {
    $('#dataList').bootstrapTable('refresh');
}
$(function () {
        $('#editModal').on('hide.bs.modal', function () {
            $.ajax({
                url: "/IndirectMaterialPurchasingQuery/DelPic/" + $('#picpath').val(),
                async: false,
                contentType: 'application/json',
                dataType: 'html',
                success: function (data) {
                },
                error: function (err) {
                    alert(err);
                }
            })
        })
});
//编辑按钮
function EditClick(id) {        
    $.ajax({
        url: "/IndirectMaterialPurchasingQuery/GetAIDRecord/" + id,
        async: false,
        contentType: 'application/json',
        dataType: 'html',
        success: function (data) {
            var obj = JSON.parse(data);
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
            GetInvType("#EditPurchasingType", "I");
            $("#EditPurchasingType").multiselect('select', obj.InvType);
            GetSubType("#EditPurchasingType", "#EditPurchasingTypeSub");
            $("#EditPurchasingTypeSub").multiselect('select', obj.SubType);
            GetMMType("#EditPurchasingTypeSub", "#EditMaterielManage");

            $("#EditMaterielManage").multiselect('select', obj.MaterielManage);
            DownLoadFile(id);
        },
        error: function (err) {
            alert(err);
        }
    })
   
    $('#editModal').modal({ show: true, backdrop: 'static' });
}
//下载附件
function DownLoadFile(id) {
    $.ajax({
        url: "/IndirectMaterialPurchasingQuery/DownLoadFile/" + id,
        async: false,
        contentType: 'application/json',
        dataType: 'html',
        success: function (data) {
            if (data == '') {
                ShowFile('');
            } else {
                showName = data.substring(id.length, data.length);
                ShowFile("<img src='/img/temp/" + data + "' class='file-preview-image'>", showName);
                $('#picpath').val(data);
            }
        },
        error: function (err) {
            alert(err);
        }
    })
}
//查看附件
function ShowFile(data, name) {
    $('#inputAttach').fileinput('destroy');
    FileInputBaseSetting("#inputAttach");
    $('#inputAttach').on('filecleared', function (event) {
        $("#changing").val('change');
    });
    $('#inputAttach').on('fileloaded', function (event, file, previewId, index, reader) {
        $("#changing").val('change');
    });
    $("#inputAttach").fileinput('refresh', {
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
            caption: name,
            width: '120px',
            url: '/AssetPurchaseQuery/DelAttachment/' + $('#GUID').val(),
            key: 100,
            extra: { id: 100 },
            previewAsData: true
        }]
    });

    if (data != '') {
        $("#inputAttach").fileinput('refresh', {
            initialPreview: [data]
        });
    } else {
        $("#inputAttach").fileinput('refresh', {
            initialPreview: []
        });
    }

    $('#inputAttach').on('change', function (event) {
        $("#changing").val('change');
    });
}
//编辑model框提交
function SaveChanges() {

    if ($("#EditPurchasingType").val() == null || $("#EditPurchasingTypeSub").val() == null || $("#EditMaterielManage").val() == null) {
        alert("物料类别或物料子类别，物料名称必填");
        return false;
    }
    $.ajax({
        cache: true,
        type: "POST",
        url: "/IndirectMaterialPurchasingQuery/UpdIndirectMaterialPurchasingRecord",
        data: $('#Edit_Form').serialize(),
        async: false,
        onSubmit: function () {
            return $("#Edit_Form").form('validate');
        },
        error: function (request) {
            alert("Connection error");
        },
        success: function (data) {
            if ($("#changing").val() == 'change') {
                delAttach($('#GUID').val());
                $("#inputAttach").each(function () {
                    $(this).fileinput('upload')
                });
            } else {   
            }
        }
    });
    $('#editModal').modal('hide');
    $('#dataList').bootstrapTable('refresh');
}
//转售按钮给转售model赋值
function ResaleClick(guid) {
        //初始化fileinput控件（第一次初始化）
        var row = $('#dataList').bootstrapTable('getRowByUniqueId', guid);
        $("#resaleModal #tmpAID_GUID2").val(guid);
        $("#resaleModal #IE_GUID2").val(guid);
        $("#resaleModal #InventoryQuantity").val(row.Inventory_Number);
        $("#resaleModal #InitialAmount").val(row.ResidualAmount);
        $("#resaleModal #Inventory_Number").val(row.Inventory_Number);
        $("#resaleModal #Amountinfosd").val(row.Amount);
        $("#resaleModal #RealseCurrency").val("").multiselect("refresh");
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
//删除附件
function delAttach(id) {
        $.ajax({
            url: "/AssetPurchaseQuery/DelAttachment/" + id,
            type: "POST",
            success: function (data) {
                $("#inputAttach").each(function () {
                    $(this).fileinput('upload')
                });
            },
            error: function () {
                alert('@General.Resource.Common.NoResponse');
            }
        });
}
//删除物料
function DelClick(id) {
        if (confirm('确认删除?')) {
            $.ajax({
                url: "/IndirectMaterialPurchasingQuery/DelIndirectMaterialPurchasingRecord/" + id,
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
};
//已转售方法的跳转以及model的弹出
function ResaleValue(id) {
        $('#dataListAmountUsed').bootstrapTable('destroy');
        $('#dataListResaleValue').bootstrapTable({
            /*待修改*/
            url: "/IndirectMaterialPurchasingQuery/GetIndirectMaterialResaleValuePurchasingList/" + id,//请求后台的URL（*）   
            /**/
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
                { field: 'Resale_Amount', title: '转售金额', align: 'center', width: '100px', formatter: DecimalFmter },
                  { field: 'Amount', title: '收入金额', align: 'center', width: '100px' },
                { field: 'Currency', title: '货币', align: 'center' },
                { field: 'RPerName', title: '客户', align: 'center' },
            ]],
            queryParams: queryParams
        });
        $('#ResaleValue').modal({ show: true, backdrop: 'static' });
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
//更改状态
 function ChangeStatus(guid, state) {
        //成功以后更改状态
        $('#dataList').bootstrapTable('refresh');
 }
//转售model提交
 function ResaleSubmit() {
        var StateResaleType = $("#StateResaleType").val();
        var client = $("#client").val();
        var Currency2 = $("#RealseCurrency").val();
        var affirmDate = $("#AffirmDate2").val();
        var date = $("#Date2").val();
        //库存金额        
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
                url: "/IndirectMaterialPurchasingQuery/UpdResaleExpenseRecord",
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
                   
                }
            });
        }
 }
//转售model关闭
 function ResaleClose(){
        //还没修改完 12.19 1842
        $("#StateResaleType").val("").multiselect("refresh");
        $("#client").val("").multiselect("refresh");
        $("#RealseCurrency").val("").multiselect("refresh");
        $('#AffirmDate2').val('');
        $('#Date2').val('');
        $('#Amountinfos').val('');
        $('#ResaleActualAmount').val('');
        $("#TaxationType").val("").multiselect("refresh");
        $('#TaxationAmounts').val('');
        $('#ResaleNumber').val('');
        $('#SumAmounts').val('');     
        $('#Remarkinfo').val('');
        $('#PZ2 #Pnumber2').val('');
        $('#PZ2_1 #Pnumber2').val('');       
    }
 //新客户登记
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
 //转售model框更改状态
 function ResaleTypeChange() {
        var StateResaleType = document.getElementById('StateResaleType');
        var StateResaleTypes = StateResaleType.value;
        if (StateResaleTypes == "用完") {
            ResaleInitialAmount = $("#resaleModal #ResaleInitialAmount").val();
            $('#Amountinfos').val(ResaleInitialAmount);
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