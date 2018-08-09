$(function () {

    //标签
    $('#home_tabs').tabs({
        fit: true,
        border: false,
    });

    //导航栏
    $('#home_nav').tree({
        url: '/Home/GetHomeNav',
        lines: true,
        onLoadSuccess: function (node, data) {
            //展开所有子树
            if (data) {
                $(data).each(function (index, value) {
                    if (this.state == 'closed') {
                        $('#home_nav').tree('expandAll');
                    }
                });
            }
        },
        onClick: function (node) {
            if (node.url) {
                //tab已存在
                if ($('#home_tabs').tabs('exists', node.text)) {
                    //选中tab
                    $('#home_tabs').tabs('select', node.text);
                } else {
                    //进度条信息
                    $.messager.progress({
                        text: '页面正在加载中......',
                        interval: 20
                    });
                    //1s之后关闭进度条
                    window.setTimeout(function () {
                        try {
                            $.messager.progress('close');
                        }
                        catch (e) {

                        }
                    }, 1000);
                    //生成tab
                    $('#home_tabs').tabs('add', {
                        title: node.text,
                        iconCls: node.iconCls,
                        closable: true,
                        content: newIframe(node.url),
                    });
                }
            }
        }
    });
});

//页面的frame
function newIframe(url) {
    var ifrStr = "<iframe src='" + url + "' frameborder=0 style='width:100%;height:100%;border:0'></iframe>";
    return ifrStr;
}


