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
                if ($('#home_tabs').tabs('exists', node.text)) {
                    $('#home_tabs').tabs('select', node.text);
                } else {
                    $('#home_tabs').tabs('add', {
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


