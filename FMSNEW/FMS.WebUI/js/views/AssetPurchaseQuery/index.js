//获取查询列表中的状态
var AssetType = [{ label: "请选择", value: "" },
        { label: "固定资产", value: "固定资产" },
        { label: "无形资产", value: "无形资产" }
];
var EditAssetType = [{ label: "固定资产", value: "固定资产" },
                { label: "无形资产", value: "无形资产" }
];
var AssetState = [{ label: "请选择", value: "" },
              { label: "折旧中", value: "折旧中" },
              { label: "折旧完", value: "折旧完" },
              { label: "转售", value: "转售" },
];
//初始化
$(document).ready(function () { 
    InitalDateInput();
    GetAssetInvType("#AssetType", "#PurchasingType");
    GetSubType("#PurchasingType", "#PurchasingTypeSub");
    GetMMTypeA('#PurchasingTypeSub', '#MaterielManage');
    TaxBind();
    var tax;
    var customer;
    var client;
    var rper = GetSupplier()
    $.ajax({
        url: "/InternalAPI/GetCustomer",
        async: false,
        cache: false,
        dataType: "json",
        success: function (d) {
            client = d;
        }
    });
    $('#client').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    $("#client").multiselect('dataprovider', client);
    $("#client").val("").multiselect("refresh");
    $("#client").multiselect('select', standardCoin);

    $('#state').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false
    });
    $("#state").multiselect('dataprovider', AssetState);
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

    $('#RPer2').multiselect({
        buttonWidth: '100%',
        maxHeight: 200,
        enableFiltering: true,
        includeFilterNewBtn: false
    });
    $("#RPer2").multiselect('dataprovider', rper);
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


    $('#EditAssetType').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: true,
        includeFilterNewBtn: false,
    });
    $("#EditAssetType").multiselect('dataprovider', EditAssetType);
    $("#EditAssetType").val("").multiselect("refresh");
    $("#EditAssetType").multiselect('select', standardCoin);
    $('#AssetType').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: true,
        includeFilterNewBtn: false,
    });
    $("#AssetType").multiselect('dataprovider', AssetType);
    $('#StateResaleType').multiselect({
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
        $('#customer').multiselect({
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
//获取当前列表
jQuery(document).ready(function () {
    $('#dataList').bootstrapTable({
        url: "/AssetPurchaseQuery/GetAssetPurchaseList",
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
        uniqueId: "GUID",
        showExport: false,
        exportTypes: ['xml', 'txt', 'excel'],
        columns:[[
            { field: 'Date', title: '购入日期', align: 'center',width:'60px',  remoteSort: true },
            { field: 'Amount', title: '初始金额', align: 'center',width:'60px', formatter: DecimalFmter },
            { field: 'GUID', title: '已转售', formatter: LinkHandleResaleValue, align: 'center',width:'60px'},
            { field: 'GUID', title: '库存金额', formatter: LinkHandleResidualAmount, align: 'center',width:'70px'},
            { field: 'Inventory_Number', title: '库存数量', align: 'center',width:'70px'},
            { field: 'Currency', title: '货币',align: 'center'},
            { field: "BusinessName" , align: 'center', title:"业务单元",width:'70px'},
            { field: "SubBusinessName", align: 'center', title:"业务子单元",width:'90px'},
            { field: 'Asset_class', title: '资产分类', align: 'center',width:'70px'},
            { field: 'AidTypeName', title: '资产类别', align: 'center',width:'70px'},
            { field: 'ASTTypeName', title: '资产子类别', align: 'center', width: '90px' },
            { field: 'MM_Name', title: '资产名称', align: 'center' },
            { field: 'RPerName', title: '供应商', align: 'center',width:'60px'},
            { field: 'GUID', title: '', formatter: LinkHandle ,align: 'center',width:'60px'}
        ]],
        pagination: true,
        rownumbers: true,
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
        AssetType: $("#AssetType").val(),
        Type: $("#PurchasingType").val(),
        TypeSub: $("#PurchasingTypeSub").val(),
        MaterielManage: $("#MaterielManage").val(),
        state: $("#state").val(),
        business_GUID: $('#Business_GUID').val(),
        subBusiness_GUID:$('#SubBusiness_GUID').val()
    };
    return temp;
}
//时间控件
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
//库存金额
var LinkHandleResaleValue = function (value, row, index) {
    if (row.ResaleValue == 0) {
        var link1 = "<p  class='linkbtn ' style='margin-top:-3px'>  \ " + row.ResaleValue + "\</p>"
    } else {
        var link1 = "<a class='linkbtn btn-primary' onclick='ResaleValue(\"" + value + "\")'>  \ " + row.ResaleValue + "\</a>"
    }
    return link1;
};
//编辑转售删除
var LinkHandle = function (value, row, index) {
    if (row.ResaleValue == 0) {
        var link1 = " <a class='linkbtn' onclick='EditClick(\"" + value + "\")'>编辑</a> ";
    } else {
        link1 = "";
    }
    var link3;
    if (row.Asset_class == '无形资产' || row.ResidualAmount <= 0) {
        link3 = "";
    } else {
        link3 = "<a class='linkbtn' onclick='ResaleClick(\"" + value + "\")'>转售</a> ";
    }
    if (row.ResaleValue == 0) {
        var link4 = " <a class='linkbtn' onclick='DelClick(\"" + value + "\")'>删除</a> ";
    } else {
        link4 = "";
    }
    return link1 + link3 + link4;
};
//转售按钮
var LinkHandleResidualAmount = function (value, row, index) {
    if (row.ResidualAmount < 0) {
        var link1 = "<a >  \ " + '0 ' + "\</a>"
    }
    else if (row.ResidualAmount >= 0) {
        var link1 = "<a >  \ " + row.ResidualAmount + "\</a>"
    }
    return link1;
};
//附件控件
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
            url: "/AssetPurchaseQuery/DelPic/" + $('#picpath').val(),
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
        url: "/AssetPurchaseQuery/GetAIDRecord/" + id,
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
            $('#EditAssetType').multiselect('select', obj.Asset_class);
            EditGetAssetInvType("#EditAssetType", "#EditPurchasingType")
            $("#EditPurchasingType").multiselect('select', obj.InvType);
            EditGetSubType("#EditPurchasingType", "#EditPurchasingTypeSub");
            $("#EditPurchasingTypeSub").multiselect('select', obj.SubType);
            GetMMTypeA("#EditPurchasingTypeSub", "#EditMaterielManage");

            $("#EditMaterielManage").multiselect('select', obj.MaterielManage);
            $('#Description').val(obj.Description);
            $('#DepreciationPeriod').val(obj.Depreciation_year);


            DownLoadFile(id)
        },
        error: function (err) {
            alert(err);
        }
    })

    $('#editModal').modal({ show: true, backdrop: 'static' });
    $(".boxed-layout").css("padding-right", "0px");
}
//下载附件
function DownLoadFile(id) {
    $.ajax({
        url: "/AssetPurchaseQuery/DownLoadFile/" + id,
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
//通过修改子类别获取折旧周期
function GetPM(parentSelector, subSelector, mmSelector) {

    GetPeriodAidType(parentSelector, subSelector);
    GetMMTypeA(parentSelector, mmSelector);
}
//通过修改子类别获取折旧周期
function GetPeriodAidType(parentSelector, subSelector) {

    var type = $(parentSelector).val();

    $.ajax({
        url: "/PurchasingTypeRecord/GetPeriodAidType?parentId=" + type,
        async: false,
        dataType: "json",
        success: function (d) {
            Depreciation_year = d[0]['Depreciation_year'];
            $('#DepreciationPeriod').val(Depreciation_year);

        }
    });
}
//删除附件
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
//转售按钮给转售页面赋值
function ResaleClick(guid) {
    var row = $('#dataList').bootstrapTable('getRowByUniqueId', guid); 
    $("#resaleModal #AID_GUID").val(guid);
    $("#resaleModal #IE_GUID").val(guid);
    $("#resaleModal #Remarkinfo").val(row.Remark);
    $("#resaleModal #InventoryQuantity").val(row.Inventory_Number);
    $("#resaleModal #Inventory_Number").val(row.Inventory_Number);
    $("#resaleModal #Amountinfo").val(row.Amount);
    $("#resaleModal #Detailed_Categories").val(row.Asset_class);
    $("#resaleModal #Amounts").val(row.Amount);
    $("#resaleModal #InitialAmount").val(row.ResidualAmount);
    $("#resaleModal #Descriptioninfo").val(row.Description);
    $("#resaleModal #Currency2").val("").multiselect("refresh");
    //初始化日期控件
    $('#AffirmDate').datepicker();
    $('#Date2').datepicker();
    IntiFileUpload("#PZ_1 #certificate");
    DownLoadFile(guid);
    //通过库存金额和库存数量计算单价
    CalculatePrice(row);
    $('#resaleModal').modal({ show: true, backdrop: 'static' });
    $(".boxed-layout").css("padding-right", "0px");
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
    $('#Amount2').val("");
    $('#TaxationAmounts').val("");
    $('#SumAmounts').val("");
    $('#Remark2').val("");
    $('#PZ_1 #Pnumber').val("");
}
//编辑model提交
function SaveChanges() {
    if ($("#EditAssetType").val() == null || $("#EditPurchasingType").val() == null || $("#EditPurchasingTypeSub").val() == null) {
        alert("资产分类或资产类别或资产子类别必填");
        return false;
    }

    if ($("#EditMaterielManage").val() == null) {
        alert("资产名称必填");
        return false;
    }
    $.ajax({
        cache: true,
        type: "POST",
        url: "/AssetPurchaseQuery/UpdAssetPurchaseRecord",
        data: $('#Edit_Form').serialize(),
        async: false,
        onSubmit: function () {
            return $("#Edit_Form").form('validate');
        },
        error: function (request) {
            alert("Connection error");
        },
        success: function (data) {
            //提交成功
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
    $('#dataList').bootstrapTable('refresh', { url: "/AssetPurchaseQuery/GetAssetPurchaseList" });
}
//删除附件
function delAttach(id) {
    alert(id);
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
//删除资产
function DelClick(id) {
    if (confirm('确认删除?')) {
        $.ajax({
            url: "/AssetPurchaseQuery/DelAssetPurchaseRecord/" + id,
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
//点击从收入中的营业外收入中获取
function ShowRevenuelist() {
    $('#RevenuelistTable').bootstrapTable({
        url: '/AssetPurchaseQuery/GetNonoperatingIncomeList',
        method: 'get',
        pageSize: 5,
        pageList: [5, 10, 25, 50, 100, 200], // 自定义分页列表
        pageNumber: 1,
        clickToSelect: true,
        checkOnSelect: true,
        selectOnCheck: true,
        pagination: true,
        sortName: '', // 设置默认排序为 AffirmDate
        sortOrder: 'desc', // 设置排序为反序 desc
        search: false,
        showColumns: true,
        showRefresh: false,
        showQuery: false,
        showToggle: false,
        showExport: false,
        exportTypes: ['xml', 'txt', 'excel'],
        uniqueId: "IE_GUID",
        columns: [
            { field: 'ck', checkbox: true, title: '选择', width: '50' },
            { field: 'RPerName', title: '客户', align: 'center' },
            { field: 'InvType', title: '收入类别', align: 'center' },
            { field: 'Currency', title: '货币', align: 'center' },
            { field: 'Amount', title: '收入金额', align: 'center' },
            { field: 'SumAmount', title: '含税总收入', align: 'center' },
        ]
    });
    $('#RevenuelistModal').modal({ show: true, backdrop: 'static' });
    $(".boxed-layout").css("padding-right", "0px");
}
//获取从收入列表中选择的数据给转售model赋值
function RevenuelistPayable() {
    var items = $('#RevenuelistTable').bootstrapTable('getSelections');
    var item = items[0];
    $("#resaleModal #IncomeIE_GUID").val(item.IE_GUID);
    alert(item.IE_GUID);
    $("#resaleModal #client").multiselect('select', item.RPerName);
    $("#resaleModal #AffirmDate2").val(DateHandle(item.AffirmDate));
    $("#resaleModal #Date2").val(DateHandle(item.Date));
    $("#resaleModal #Currency2").multiselect('select', item.Currency);
    $("#resaleModal #ResaleActualAmount").val(parseFloat(item.Amount));
    $("#resaleModal #TaxationType").multiselect('select', item.TaxationType);
    $("#resaleModal #TaxationAmounts").val(parseFloat(item.TaxationAmount));
    $("#resaleModal #SumAmounts").val(Money(parseFloat(item.SumAmount)));
    $("#resaleModal #Remarkinfo").val(item.Remark);
    $("#resaleModal #Pnumber").val(item.InvNo);
    $('#RevenuelistModal').modal('hide');
}
//转售model提交
function Submit() {
    // var files = $("#PZ_1 #certificate").fileinput('getFileStack');
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
    if (Date.parse(affirmDate) > Date.parse(date)) {
        alert("收入确认日期不能大于账期截止日期，请重新选择！");
    } else {
        $.ajax({
            cache: true,
            type: "POST",
            url: "/AssetPurchaseQuery/UpdResaleExpenseRecord",
            data: $('#Resale_Form').serialize(),
            async: false,
            onSubmit: function () {
                return $("#Resale_Form").form('validate');
            },
            error: function (request) {
                alert("Connection error");
            },
            success: function (data) {
                //提交成功，开始上传附件
                $("[name=certificate]").each(function () {
                    $(this).fileinput('upload')
                })
                $('#dataList').bootstrapTable('refresh');
                SubmitClose();
                $('#resaleModal').modal('hide');
                alert("转售成功");
            }
        });
    }
}
//转售model关闭
function SubmitClose() {
    $("#StateResaleType").val("").multiselect("refresh");
    $("#client").val("").multiselect("refresh");
    $("#Currency2").val("").multiselect("refresh");
    $('#AffirmDate2').val('');
    $('#Date2').val('');
    $('#Amountinfos').val('');
    $('#IncomeIE_GUID').val('');
    $('#ResaleActualAmount').val('');
    $("#TaxationType").val("").multiselect("refresh");
    $('#TaxationAmounts').val('');
    $('#ResaleNumber').val('');
    $('#SumAmounts').val('');
    $('#PZ #Pnumber').val('');
    $('#PZ_1 #Pnumber').val('');
    $('#Remarkinfo').val('');
}
//更改状态
function ChangeStatus() {
    //成功以后更改状态
    $.ajax({
        cache: true,
        type: "POST",
        url: "/AssetPurchaseQuery/UpdAssetPurchaseRecordState?id=" + $('#AID_GUID').val() + "&state=转售",
        async: false,
        onSubmit: function () {
        },
        error: function (request) {
            alert("Update Error");
        },
        success: function (data) {
            $('#dataList').bootstrapTable('refresh');
            $('#resaleModal').modal('hide');
        }
    });
}
//已转售方法的跳转以及model的弹出
function ResaleValue(id) {
    $('#dataListResaleValue').bootstrapTable('destroy');
    $('#dataListResaleValue').bootstrapTable({
        /*待修改*/
        url: "/AssetPurchaseQuery/GetAssetPurchaseRecordResaleValuePurchasingList/" + id,//请求后台的URL（*）   
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
            { field: 'Date', title: '转售时间', align: 'center', width: '150px', formatter: DateHandle,remoteSort: true },
            { field: 'Resale_Amount', title: '转售金额', align: 'center', width: '100px' },
            { field: 'Amount', title: '收入金额', align: 'center', width: '100px' },
            { field: 'Currency', title: '货币', align: 'center' },
            { field: 'RPerName', title: '客户', align: 'center' },

        ]],
        queryParams: queryParams
    });
    $('#ResaleValue').modal({ show: true, backdrop: 'static' });
    $(".boxed-layout").css("padding-right", "0px");
}
//初始化fileinput控件（第一次初始化）
function IntiFileUpload(selector) {
    FileInputBaseSetting(selector);
    $(selector).fileinput('refresh', {
        uploadUrl: '/InternalAPI/FileUpload',
        allowedFileExtensions: ['jpg', 'png', 'gif'],
        maxFileCount: 5,
        mainClass: "input-group-lg",
        browseClass: "btn bg-purple2 btn-sm",
        uploadExtraData: function () { // callback example
            var out = {};
            out['frGuid'] = $('#IE_GUID').val();
            return out;
        }
    });
}
//新客户登记
function AddCustomer(obj) {
    var c_AffirmDate = $('#AffirmDate').val();
    var c_Date = $('#Date').val();
    var c_Amount = $('#Amount').val();
    var c_Currency = $('#Currency').val();
    var c_TaxationType = $('#TaxationType').val();
    var c_TaxationAmount = $('#TaxationAmounts').val();
    var c_SumAmount = $('#SumAmounts').val();
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
//编辑model资产类别初始化
function EditGetSubType(parentSelector, subSelector) {
    var type = $(parentSelector).val();

    $.ajax({
        url: "/PurchasingTypeRecord/GetSonAidType?parentId=" + type,
        async: false,
        dataType: "json",
        success: function (d) {
            InitSelect(subSelector, d);
            InitializationPeriod();
        }
    });

}
//初始化折扣周期
function InitializationPeriod() {
    $("#DepreciationPeriod").val("");
}
//资产分类select获取值
function GetAssetInvType(assetSelector, invTypeSelector) {
    var AssetType = $(assetSelector).val();
    $.ajax({
        url: "/PurchasingTypeRecord/GetAssetsAidType",
        data: { assetType: AssetType },
        async: false, 
        dataType: "json",
        success: function (d) {
            InitSelect(invTypeSelector, d);
            ClassSub();
        }
    });    
}
//编辑model资产分类初始化
function EditGetAssetInvType(assetSelector, invTypeSelector) {
    var AssetType = $(assetSelector).val();
    $.ajax({
        url: "/PurchasingTypeRecord/GetAssetsAidType",
        data: { assetType: AssetType },
        async: false,
        dataType: "json",
        success: function (d) {
            InitSelect(invTypeSelector, d);
            EditClassSub();
        }
    });
}
//select框修改后的初始化
function ClassSub() {
    $('#PurchasingTypeSub').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: true,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    $("#PurchasingTypeSub").val("").multiselect("refresh");
}
//select框修改后的初始化
function EditClassSub() {
    $('#EditPurchasingTypeSub').multiselect({
        buttonWidth: '100%',
        maxHeight: 150,
        enableFiltering: true,
        includeFilterNewBtn: false,
        nonSelectedText: '请选择',
    });
    $("#EditPurchasingTypeSub").val("").multiselect("refresh");
    $("#DepreciationPeriod").val("");
}

function ImportPayable() {
    var items = $('#ClientlistTable').bootstrapTable('getSelections');
    $.each(items, function (index, item) {
        $("#IE_GUID").val(item.GUID);
        $("#RPer2").multiselect('select', item.RPerName);
        $("#Currency2").multiselect('select', item.Currency);
        $('#Amountinfo').val(parseFloat(item.Amount));
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
//转售model修改状态
function ResaleTypeChange() {
    var StateResaleType = document.getElementById('StateResaleType');
    var StateResaleTypes = StateResaleType.value;
    if (StateResaleTypes == "转售") {
        InitialAmount= $("#resaleModal #InitialAmount").val();
        $('#Amountinfos').val(InitialAmount);
        var EditAmountinfos = document.getElementById("Amountinfos");
        EditAmountinfos.setAttribute("readOnly", 'true');
    }
    if (StateResaleTypes == "折旧中") {
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

