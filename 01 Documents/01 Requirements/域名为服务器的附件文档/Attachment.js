
    //获取第一条id
    function showUploadFile() {
        var id = $("#Example_1 #GUID").val();
        showUploadFiles(id);
    }
//获取row>1记录id
function showUploadFileMore(id) {
    array = id.split("_");
    id = array[1];
    showUploadFiles(id);
}
//通过id查询附件并且显示
function showUploadFiles(id) {
    $("#showhaveuploaded").empty()
    $('#showhaveuploaded').bootstrapTable('destroy');
    $('#showhaveuploaded').bootstrapTable({
        url: "/InternalAPI/ShowUploadFile/" + id,//请求后台的URL（*）   
        method: 'get',//请求方式（*）
        pageSize: 10,//每页的记录行数（*）
        pageList: [10, 20, 50, 100, 200], // 自定义分页列表
        pageNumber: 1, //初始化加载第一页，默认第一页
        singleSelect: true,
        clickToSelect: true,
        checkOnSelect: true,
        selectOnCheck: true,
        cardView: false,//是否显示详细视图
        pagination: false, //是否显示分页（*）
        sortName: 'Date', // 设置默认排序为 name
        sortOrder: 'desc', // 设置排序为反序 desc
        search: false, // 开启搜索功能
        showColumns: false,
        showRefresh: false,
        showQuery: false,
        showToggle: false,//是否显示详细视图和列表视图的切换按钮
        showExport: false,
        exportTypes: ['xml', 'txt', 'excel'],
        uniqueId: "A_GUID",
        columns: [[
             //由于IE_Record的model没有修改date类型所以在已转售的时间格式需要转换
            { field: 'A_GUID', title: '附件', align: 'center', width: '400px', remoteSort: true, formatter: ShowUploadedLinkHandle },
            { field: 'A_GUID', title: '操作', formatter: UploadedLinkHandle, remoteSort: true, align: 'center'}
        ]],
        queryParams: queryParams
    });
    $('#showhaveuploadedModal').modal({ show: true, backdrop: 'static' });
	$(".boxed-layout").css("padding-right", "0px");
}
//配置参数
function queryParams(params) {  //配置参数
    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.pageNumber,  //页码
        dateBegin: $("#dateBegin").val(),
        dateEnd: $("#dateEnd").val(),
    };
    return temp;
}
var UploadedLinkHandle = function (value, row, index) {
    var link1 = " <a class='' onclick='UploadedDelClick(\"" + value + "\")' style='cursor:pointer'>删除</a> ";
    return link1;
};
//下载附件
var ShowUploadedLinkHandle = function (value, row, index) {
    var link2 = "<a class='' href='/InternalAPI/DownLoadFile?fileID=" + value + "'>  \ " + row.FileName + "\</a>"
    return link2;
};
//删除附件
function UploadedDelClick(id) {
    if (confirm('确认删除?')) {
        $.ajax({
            url: "/InternalAPI/DelUploadAttachment/" + id,
            type: "POST",
            success: function (data) {
                var res = JSON.parse(data);
                alert(res.Msg);
                $('#showhaveuploaded').bootstrapTable('refresh');
            },
            error: function () {
                alert('@General.Resource.Common.NoResponse');
            }
        });
    }
}
//点击Row>1记录附件按钮给上传附件model传值
function ShowUpLoadDialog(id) {
    array = id.split("_");
    GUID = array[1];
    ShowUpLoadDialogs(GUID);
    $("#AttachmentImportModal #uploadformid").val(GUID);
}
//点击第一条记录附件按钮给上传附件model传值
function ShowFirstUpLoadDialog() {
    var GUID = $("#Example_1 #GUID").val();
    ShowUpLoadDialogs(GUID);
    $("#AttachmentImportModal #uploadformid").val(GUID);
}
//点击“附件”按钮显示对应的模态框（链接二维码地址）
function ShowUpLoadDialogs(GUID) {
    $('#AttachmentImportModal #frGuid').val(GUID);
    //获取当前时间戳
    var timestamp = new Date().getTime();
    //随机生成一个编号
    var number = _getRandomString(10);
    $('#assignment').val("");
    //当前二维码为空
    $("#qrcode").empty();
    var id = $("#id").val();
    var urladdress = "http://net-accounting.cn/UploadAttachment/UploadAttachment?belongid=" + GUID + "&tamp=" + timestamp + "&number=" + number;
    jQuery('#qrcode').qrcode({
        render: "canvas",
        width: 100,
        height: 100,
        correctLevel: 0,
        text: urladdress
    });
    //公共还是私有的标示
    //当前文件域是否为空
    $('#thelist').empty();
    //显示上传合同附件的模态框
    $('#AttachmentImportModal').modal('show');
    //获取当前Id
    $("#assignment").val(GUID);
    //选择按钮样式
    $("#picker").find("div[id^='rt_rt_']").css({ "height": "100%", "width": "100%" });
    window.iCount = "";
    window.iCount = window.setInterval(function () { showTime(GUID); }, 8000);
}
// 获取长度为len的随机字符串
function _getRandomString(len) {
    len = len || 32;
    var $chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678'; // 默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1
    var maxPos = $chars.length;
    var pwd = '';
    for (i = 0; i < len; i++) {
        pwd += $chars.charAt(Math.floor(Math.random() * maxPos));
    }
    return pwd;
}
//监控实时查询
function showTime(GUID) {
    $.ajax({
        url: "/InternalAPI/ShowTime?FR_GUID=" + GUID,
        type: "POST",
        success: function (data) {
            if (data == 1) {
                document.getElementById("span" + belongid).innerHTML = "<i class='glyphicon glyphicon-paperclip icon-white'></i>";
                $("#AttachmentImportModal").modal('hide');
                clearInterval(window.iCount);
            }
        },
        //error: function () {
          //  alert('@General.Resource.Common.NoResponse');
        //}
    });
}
//当关闭model监控实时查询关闭
function CloseAttachmentImportModal() {
    clearInterval(window.iCount);
}
// 文件上传
jQuery(function () {
    var $ = jQuery,
            $list = $('#thelist'),
            $btn = $('#ctlBtn'),
            state = 'pending',               
            uploader;
    uploader = WebUploader.create({
        // 不压缩image
        resize: false,

        // 文件路径（播放视频是需要）
        swf: '../webuploader/Uploader.swf',

        // 文件接收服务端。
        server: '/InternalAPI/FileUpload',
                        
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: {
            id: '#picker',
            multiple: true  
        },
        //fileSingleSizeLimit:,
        accept: {
            title: '文件类型',
            extensions: 'gif,jpg,jpeg,bmp,png,pdf,xls,xlsx,doc,docx,ppt,pptx,,mp4,avi,rmvb',
            mimeTypes: 'image/jpeg,image/png,image/gif,application/pdf,application/msword,application/vnd.ms-excel,application/vnd.ms-powerpoint,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.openxmlformats-officedocument.presentationml.presentation'
        },
        fileVal:'file[]',
        //50M
        fileSingleSizeLimit: 50 * 1024 * 1024
    });
    uploader.on('fileQueued', function (file) {
        /*$('#thelist').empty();*/
        $list.append('<div id="' + file.id + '" class="item"  >' +
                '<h4 class="info" style="margin-left:20px;margin-right:20px">' + file.name + '</h4>' + '</div>');
    });
    uploader.on('startUpload', function () {
        var belongid = $("#AttachmentImportModal  #uploadformid").val();
        uploader.options.formData.frGuid = belongid;
        $("#uploadMesg").text("上传中");
    });
    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
                $percent = $li.find('.progress .progress-bar');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<div class="progress progress-striped active">' +
                    '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                    '</div>' +
                    '</div>').appendTo($li).find('.progress-bar');
        }

        $li.find('p.state').text('文件');

        $percent.css('width', percentage * 100 + '%');
    });

    uploader.on('uploadAccept', function (file, response) {
    });
    uploader.on('uploadSuccess', function (file, response) {
        uploader.reset();       
        $("#uploadMesg").text("");
        //把上传文件中的文件名删除
        $('#thelist').empty();
        //关闭上传合同附件的模态框
        $('#AttachmentImportModal').modal('hide');      
    });
    uploader.on('uploadError', function (file, reason) {
    });
    uploader.on('uploadFinished', function (file) {
    });
    uploader.on('uploadComplete', function (file) {
        
        $('#' + file.id).find('.progress').fadeOut();
        /* uploader.reset();*/
        $("#uploadMesg").text("");
        //把上传文件中的文件名删除
        $('#thelist').empty();
        //关闭上传合同附件的模态框
        $('#AttachmentImportModal').modal('hide');
        var fileflag = $('#fileflag').val();
		CloseAttachmentImportModal();
        alert('上传成功');
    });

    uploader.on('all', function (type) {
        if (type === 'startUpload') {
            state = 'uploading';
        } else if (type === 'stopUpload') {
            state = 'paused';
        } else if (type === 'uploadFinished') {
            state = 'done';
        }
    });

    $btn.on('click', function () {
        if (state === 'uploading') {
            uploader.stop();
        } else {
            uploader.upload();
        }
    });
});   