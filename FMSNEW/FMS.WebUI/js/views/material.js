 
function GetInvType(selector, Aid_Flag) {
     
    $.ajax({
        url: "/PurchasingTypeRecord/GetParentAidType?AID_FLAG=" + Aid_Flag,
        async: false,
        dataType: "json",
        success: function (d) {
            InitSelect(selector, d);
            
        }
    });
    
}
 
function GetSubType(parentSelector, subSelector) {  

    var type = $(parentSelector).val();
   
    $.ajax({
        url: "/PurchasingTypeRecord/GetSonAidType?parentId=" + type,
        async: false,
        dataType: "json",
        success: function (d) {
            InitSelect(subSelector,d);
        }
    });
   
}

function GetMMType(parentSelector, subSelector) {

    var type = $(parentSelector).val();

    $.ajax({
        url: "/MaterielManage/GetMMType?parentId=" + type,
        async: false,
        dataType: "json",
        success: function (d) {
            InitSelect(subSelector, d);
        }
    });

}
function GetMMTypeNA(parentSelector, subSelector) {

    var type = $(parentSelector).val();

    $.ajax({
        url: "/MaterielManage/GetMMTypeNA?parentId=" + type,
        async: false,
        dataType: "json",
        success: function (d) {
            InitSelect(subSelector, d);
        }
    });

}
function GetMMTypeA(parentSelector, subSelector) {

    var type = $(parentSelector).val();

    $.ajax({
        url: "/MaterielManage/GetMMTypeA?parentId=" + type,
        async: false,
        dataType: "json",
        success: function (d) {
            InitSelect(subSelector, d);
        }
    });

}
function InitSelect(selector, dataprovider) {
    $(selector).multiselect(SelectConfigSetting);
    $(selector).multiselect('dataprovider', dataprovider); 
   // $(selector).val("").multiselect('refresh');    
}


var FJHandle = function (value, row, index) {
    if (value == "" || value == null) {
        return "";
    } else {
        var v = "../Content/EasyUI/themes/icons/hxz.png";
        return '<img style="height: 16px;width: 16px;" src="' + v + '" />';
    }
};