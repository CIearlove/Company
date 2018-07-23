$(function () {

    //标签
    $('#tabs').tabs({
        fit: true,
        border: false,
    });

    //导航栏
    $('#nav').tree({
        url: '/Home/GetHomeNav',
        lines: true,
        onLoadSuccess: function (node, data) {
            //展开所有子树
            if (data) {
                $(data).each(function (index, value) {
                    if (this.state == 'closed') {
                        $('#nav').tree('expandAll');
                    }
                });
            }
        },
        onClick: function (node) {
            if (node.url) {
                if ($('#tabs').tabs('exists', node.text)) {
                    $('#tabs').tabs('select', node.text);
                } else {
                    $('#tabs').tabs('add', {
                        title: node.text,
                        iconCls: node.iconCls,
                        closable: true,
                        href: node.url,
                    });
                }
            }
        }
    });
});


