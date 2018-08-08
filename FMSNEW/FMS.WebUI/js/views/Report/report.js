var reportViewUrl

jQuery(document).ready(function () {  
    $('#btnPreview').hide();

});

function DataBind(control,view)
{

    if (!DateCheck())
    {
        $('#Reports').show();
        return false;
    }
    $('#Reports').hide();

    reportViewUrl =  "/"+control + "/"+ view;
    var param=GetParams(); 
    $('#dataList').bootstrapTable({
        url: "/"+control + "/GetReportList",
        method: 'get',
        pageSize: 10,
        pageList: [10, 20, 50, 100, 200], // 自定义分页列表
        pageNumber: 1,
        singleSelect: true,
        clickToSelect: true,
        checkOnSelect: true,
        selectOnCheck: true,
        pagination: true,
        search: false, // 开启搜索功能
        showColumns: false,
        showRefresh: false,
        showQuery: false,
        showToggle: false,
        showExport: false,
        uniqueId: "Rep_GUID",
        exportTypes: ['xml', 'txt', 'excel'],
        columns: [[
            { field: 'RepNo', title: '报表编号', formatter: LinkHandle },
            { field: 'rep_date', title: '日期' },
            { field: 'rep_status', title: '状态' },
        ]],
        pagination: true,
        rownumbers: true,
        queryParams: {reportDate:param.date,type:param.type}
    });
}


function RepDateFmter(val, row) {
    return row.Year + '/' + row.Month;
}

var LinkHandle = function (value, row) {
    return "<a class='link' onclick='ShowDetails(\"" + row.Rep_GUID + "\")'>" + row.RepNo + "</a>";
    
};

function ShowDetails(id) {
    var param = GetParams();

    var row = $('#dataList').bootstrapTable('getRowByUniqueId', id); 
 

    window.location.href = reportViewUrl + "?id=" + id + "&reportDate=" + param.date + "&type=" + param.type + "&status=" + row.rep_status + "&period=" + row.Type


}

function Preview() {

    if (!DateCheck())
    {
        return false;
    }

    var param=GetParams();  

    reportViewUrl = reportViewUrl + "?reportDate=" + param.date + "&type=" + param.type + "&end=" + param.end;
    window.location.href = reportViewUrl;
}

function GetParams()
{
    var param=new Object();

    if ($("#months").val() != 0)
    {
        param.date = $("#months").val();
        param.type = "month";
    }
    else if ($("#quarters").val() != 0)
    {
        param.date = $("#quarters").val();
        param.type = "quarter";

    }
    else if ($("#years").val() != 0) {
        param.date = $("#years").val();
        param.type = "year";
    } else if ($("#dateEnd").val() != 0)
    {
        param.end = $("#dateEnd").val();
        param.type = "seach";
    }
    else
    {
        param.date = "0";
        param.type = "0";
    }
    return param;
}

function DateCheck()
{
    if ($("#months").val() == "0" &&
       $("#quarters").val() == "0" &&
       $("#years").val() == "0" && 
       $("#dateEnd").val()=="") {
        alert("请选择一个报表周期");
        return false;
    }
    return true;
}

function Create(createUrl, successUrl) {
  
    // window.location.href = "/CashFlowStatements/UpdCashFlowStatement";
    $.ajax(
    {
        url: createUrl,
        type: "post",
        async: false,
        dataType: "json",
        data: { repDate: $('#hidRepDate').val(), type: $('#hidType').val() },
        success: function (d) {
            if (d.success) {
                alert("报表创建成功。");
                window.location = successUrl
            } else {
                alert("报表创建失败：" + d.msg);
            }
        }

    })
}

function DateInit(control)
{
    var months = "";
    
    var now = new moment(); //当前日期   
    var nowMonth = now.month() + 1; //当前月 
    var nowYear = now.year(); //当前年 
    var nowQuarter = now.quarter()

    var lstMonths = new Array();
    var lstQuarter = new Array();
    var lstYear = new Array();

    var temp = new Object;
    temp.label = "选择 ";
    temp.value = "0";
    lstMonths.push(temp);
    lstQuarter.push(temp);
    lstYear.push(temp);
     
    for (var i = 1; i <= nowMonth; i++)
    {
        temp = new Object;
        temp.label = nowYear + "/" + i;
        temp.value = nowYear + "/" + i;
        lstMonths.push(temp);
    }

    for (var i = 1; i <= nowQuarter; i++) {
        temp = new Object;
        temp.label = nowYear + "/" + i;
        temp.value = nowYear + "/" + i;
        lstQuarter.push(temp);
    }

    temp = new Object;
    temp.label = nowYear;
    temp.value = nowYear ;
    lstYear.push(temp);

    $(".report-date").multiselect(SelectConfigSetting);  

    
    $("#months").multiselect('dataprovider', lstMonths);
    $("#quarters").multiselect('dataprovider', lstQuarter);
    $("#years").multiselect('dataprovider', lstYear);
    
    $('.report-date').change(function (option) {
        switch (option.delegateTarget.id)
        {
            case "months":
               

                $("#quarters").multiselect('deselect', $("#quarters").val())
                $("#years").multiselect('deselect', $("#years").val()) 

                $("#quarters").multiselect('select', "0",true)
                $("#years").multiselect('select', "0")
                $("#dateEnd").val("");
                break;
            case "quarters":

                $("#months").multiselect('deselect', $("#months").val())
                $("#years").multiselect('deselect', $("#years").val())

                $("#months").multiselect('select', "0")
                $("#years").multiselect('select', "0")
                $("#dateEnd").val("");
                break;
            case "years":
                $("#months").multiselect('deselect', $("#months").val())
                $("#quarters").multiselect('deselect', $("#quarters").val())

                $("#quarters").multiselect('select', "0")
                $("#months").multiselect('select', "0")
                $("#dateEnd").val("");
                break;
            default:

                break;
        }

        $('#dataList').bootstrapTable('destroy');

        ReportIsExist( control)
    })
    $("#dateEnd").change(function()
    {
        $("#quarters").multiselect('deselect', $("#quarters").val())
        $("#years").multiselect('deselect', $("#years").val())

        $("#quarters").multiselect('select', "0", true)
        $("#years").multiselect('select', "0")
        $("#months").multiselect('deselect', $("#months").val())
        $("#months").multiselect('select', "0")
        $('#dataList').bootstrapTable('destroy');

        ReportIsExist(control)
    })
}


function ReportIsExist(control)
{
    var param = GetParams();
    if (param.date == "0")
    {
        return false;
    }
    $.ajax(
   {
       url:  "/"+control + "/GetReportList",
       type: "post",
       async: false,
       dataType: "json",
       data: { reportDate: param.date, type: param.type },
       success: function (d) {
           if (d.length>0) {
               $('#btnPreview').hide();
           } else {
               $('#btnPreview').show();
           }
       }

   })
   
}

function queryParams(params) {  //配置参数
    var temp = {
        id: $('#repId').val(),
        reportDate: $('#hidRepDate').val(),
        type: $("#hidType").val()
    };
    return temp;
}
