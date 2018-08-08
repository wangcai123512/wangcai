
$(document).ready(function () {
    $("#productDetail").hide();
    $("#message").hide();

})


function ProductView() {
    var productData;
    if ($("#Business_GUID").val() == "") {
        alert("请输入业务单元");
        return false;
    }

    if ($("#SubBusiness_GUID").val() == "") {
        alert("请输入业务子单元");
        return false;
    }
    if ($("#productType").val() == "") {
        alert("请输入产品类别");
        return false;
    }

    if ($("#productTypeSub").val() == "") {
        alert("请输入产品子类");
        return false;
    }
    if ($("#MaterielManage").val() == "") {
        alert("请输入产品");
        return false;
    }
    
    if ($("#productCount").val() == "") {
        alert("请输入产品数量");
        return false;
    }
    productData = getProductDetail();
    if (productData == "") {
        alert("该产品未设置产品结构，请先设置产品结构");
        return false;
    }
    var productDataArray = JSON.parse(productData);
    var list = '<div class="row" id="as_Form">';
    for(var i=0;i<productDataArray.length;i++){
        list+= '<div class="col-md-12" id="stock_'+i+'">';
        list+= '<div class="col-md-6">';
        list+= '<div class="form-group">';
        list+= '<div class="input-group input-group-sm">';
        list+= '<span class="input-group-addon">'+productDataArray[i].node_name +'<数量></span>';
        list+= '<input type="number"  id="' + productDataArray[i].nodes + '" name="item_counts" data-flag="D" class="form-control" value="' + productDataArray[i].item_counts + '" onchange="checknum(this.value,\'' + productDataArray[i].stock_num + '\')" />';
        list += '<input type="hidden"  id="MaterielManage_GUID" value="1">';
        list+= '</div></div></div>';
        list+= '<div class="col-md-6">';
        list+= '<div class="form-group">';
        list+= '<div class="input-group input-group-sm">';
        list+= '<span class="input-group-addon">库存数量</span>';
        list+= '<input type="number" name="stocks_num" class="form-control" readonly value="' + productDataArray[i].stock_num + '"/>';
        list+= '</div></div></div></div>';
    }
    list+= '</div>';
    $("#productData").html(list);
    $("#productModal").modal('show');
}

function AddNewRow() {
    var o = document.getElementById("stock");
    var div = document.createElement("div");
    var rows = $("[id^=stock_]");
    var newIndex = rows.length;
    var guid = NewGUID();
    div.id = "stock_" + newIndex;
    div.className = "col-md-12";
    div.innerHTML = o.innerHTML.replace(/\{0\}/ig, newIndex).replace("{0}/g", "#" + div.id);
    $("#as_Form").append(div);
    //为新增的file控件初始化

    var typeSelector = '#' + div.id + ' #parentnodes';

    var subTypeSelector = '#' + div.id + ' #nodesas';

    var subMMSelector = '#' + div.id + ' #MaterielManage_GUID';

    GetInvType(typeSelector, "D");
    GetSubType(typeSelector, subTypeSelector)

    GetMMType(subTypeSelector, subMMSelector);

    $(typeSelector).change(function () { GetSubType(typeSelector, subTypeSelector) });
    $(subTypeSelector).change(function () { GetMMType(subTypeSelector, subMMSelector) });
    $(subMMSelector).change(function () { GetStock(div.id, this.value) });

    $("#" + div.id + " .delete").click(function () {
        RemoveRow(div.id);
    });



}
function GetStock(id, MM_GUID) {
    var i = 0;
    var ret1 = false;
    $("[id^=" + id + "]").each(function () {
        $(this).find($("input[name^='item_counts']")).attr('id', MM_GUID);
    });
    var arr = new Array();
    $("input[name='item_counts']").each(function (index, item) {
        if (index != 0) {
            arr[i] = $(this).attr('id');
            i++;
        } 
    });
    
    for (var i = 0; i < arr.length; i++) {
        if (arr.indexOf(arr[i]) != arr.lastIndexOf(arr[i])) {
            ret1 = true;
            break;
        }
    };
    if (ret1) {
        alert("抱歉,不能添加相同物料！");
        return false;
    }
    var productData = "";
    $.ajax({
        url: "/ProductManage/getProductNum",
        type: "POST",
        async: false,
        data: { BusId: $('#Business_GUID').val(), SBusId: $('#SubBusiness_GUID').val(), typeId: $('#productType').val(), subId: $('#productTypeSub').val(), mmId: MM_GUID },
        dataType: "JSON",
        ContentType: "application/json",
        success: function (data) {
            if (data.success == true) {
                productData = data.msg;
                var productDataArray = JSON.parse(productData);
                $("[id^=" + id + "]").each(function () {
                    $(this).find($("input[name^='item_counts']")).val("1")
                    
                    $(this).find($("input[name^='item_counts']")).change(function () { checknum(this.value,productDataArray[0].stock_num) })
                    $(this).find($("input[name^='stocks_num']")).val(productDataArray[0].stock_num)
                });
            } else {
                $("[id^=" + id + "]").each(function () {
                    $(this).find($("input[name^='item_counts']")).val("1")
                    $(this).find($("input[name^='item_counts']")).change(function () { checknum(this.value, "0") })
                    $(this).find($("input[name^='stocks_num']")).val("0")
                });
            }

        },
        error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
            alert(event);
        }

    })
    return productData;
    
}
function RemoveRow(id) {
        $('#' + id).remove();
}
function checknum(a,b) {
    if (parseInt(a) > parseInt(b)) {
        alert("物料库存数量不足，请先添加库存，再进行产品制成！");
        $("#productModal").modal('hide');
    }
}

function getProductDetail() {
    var productData = "";
    $.ajax({
        url: "/ProductManage/getProductDetails",
        type: "POST",
        async: false,
        data: { BusId: $('#Business_GUID').val(), SBusId: $('#SubBusiness_GUID').val(), typeId: $('#productType').val(), subId: $('#productTypeSub').val(), mmId: $('#MaterielManage').val(), cnt: $('#productCount').val() },
        dataType: "JSON",
        ContentType: "application/json",
        success: function (data) {
            if (data.success == true) {
                productData = data.msg;
            } else {
                productData = "";
            }

        },
        error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
            alert(event);
        }

    })
    return productData;
}
function createProduct() {
    var error = false;
    
    var c = 1;
    $("[id^=stock_]").each(function () {
        var a = $(this).find($("[id^='MaterielManage_GUID']"));
        if (typeof($(this).find($("[id^='MaterielManage_GUID']")).val()) == "" || typeof($(this).find($("[id^='MaterielManage_GUID']")).val()) == "undefined") {
                alert("请输入物料名称");
                error = true;
                return false;
            }
        
        var a = $(this).find($("input[name^='item_counts']")).val()
        var b = $(this).find($("input[name^='stocks_num']")).val()
        if (parseInt(a) > parseInt(b)) {
            c = 2;
        }
    });
    if (c == 2) {
        alert("物料库存数量不足，请先添加库存，再进行产品制成！");
        return false;
    }
    var item_counts ="";
    $("input[name='item_counts']").each(function (index, item) {
        if (index != 0) {
            item_counts += $(this).attr('id');
            item_counts += ":";
            item_counts += $(this).val();
            item_counts += ":";
            item_counts += $(this).attr("data-flag");
            item_counts += ";";
        }
    });
    if (error) {
        return false;
    }

    item_counts = item_counts.substring(0, item_counts.length - 1);
    getTree(item_counts);
    //treeData = getTree(item_counts);
    //if (treeData != "") {
    //    ShowTree(treeData);

    //    ShowMessage("成功创建" + $('#productCount').val() + "个产品") 
    //}
}
function getTree(item_counts) {
    var treeData = "";

    // Some logic to retrieve, or generate tree structure
    $.ajax({
        url: "/ProductManage/CreateProduct",
        type: "POST",
        async: false,
        data: { item_counts :item_counts,BusId: $('#Business_GUID').val(), SBusId: $('#SubBusiness_GUID').val(), typeId: $('#productType').val(), subId: $('#productTypeSub').val(), mmId: $('#MaterielManage').val(), cnt: $('#productCount').val() },
        dataType: "JSON",
        ContentType: "application/json",
        success: function (data) {
            if (data.success == true) {
                alert(data.msg);
                location.reload();
            } else {
                alert(data.msg);
            }

        },
        error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
            alert(event);
        }

    })
    return treeData;
}
function ShowMessage(message)
{
    $("#message").html(message)

    $("#message").show();
}
function ShowTree(treeData) {
    $('#tree').treeview({
        color: "#428bca",
        showTags: true,

        icon: "glyphicons glyphicons-beer",
        // selectedIcon: "glyphicons glyphicons-beer",
        color: "#000000",
        backColor: "#FFFFFF",
        href: "#node-1",
        selectable: true,
        state: {
            checked: false,
            disabled: true,
            expanded: true,
            selected: true
        },
        data: treeData
    });

    $('#tree').treeview('expandAll', { levels: 3, silent: true })

    $('#tree').on('nodeSelected', NodeSelected);

    $("#productDetail").show();

    root = $('#tree').treeview('getNode', 0);

    $("#nodeNames").val(root.text);
    $("#count").attr("min", root.tags[0])
    $("#count").attr("readonly", true)
    $("#count").val(root.tags[0]);
}

function NodeSelected(event, data) {
    $("#nodeNames").val(data.text);
    $("#count").attr("min", data.tags[0])
    $("#count").val(data.tags[0]);
    if (data.nodeId == 0) {
        $("#count").attr("readonly", true)
    } else {
        $("#count").attr("readonly", false)
    }

}



function Update() {
    var count = $("#count").val();
    var selNode = $('#tree').treeview('getSelected')[0];
    var min = $("#count").attr("min");
  
    if (count < min)
    {
        ShowMessage("数量必须大于等于" + min);
        return false;
    }

    var nodeId = selNode.id;
    $.ajax({
        url: "/ProductManage/UpdateProduct",
        type: "POST",
        async: false,
        data: { id: nodeId, count: count },
        dataType: "JSON",
        ContentType: "application/json",
        success: function (data) {
            if (data.success == true) {
                treeData = data.msg;
                ShowTree(treeData);

                ShowMessage("数量更新成功！")

                $('#tree').treeview('selectNode', selNode.nodeId);
            } else {
                alert(data.msg);
            }

        },
        error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
            alert(event);
        }

    })
    
}


