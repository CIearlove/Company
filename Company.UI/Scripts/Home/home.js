$(function () {

    obj = {
        search: function () {
            $('#tab1').datagrid('load', {
                name: $.trim($('input[name="name"]').val()),
                date_from: $('input[name="date_from"]').val(),
                date_to: $('input[name="date_to"]').val(),
            });
        },
    };

    $('#tab1').datagrid({
        title: '用户列表',
        iconCls: 'icon-search',
        url: '/Home/GetList',
        method: 'post',
        fitColumns: true,
        toolbar:"#tabTools",
        columns: [[
            {
                field: 'Id',
                title: 'ID',
                width: 50,
                align:'center',
            },
            {
                field: 'Name',
                title: '名称',
                width: 150,
                align: 'center',
            },
            {
                field: 'Age',
                title: '年龄',
                width: 80,
                align: 'center',
            },
            {
                field: 'Sex',
                title: '性别',
                width: 150,
                align: 'center',
            },
            {
                field: 'CreateTime',
                title: '进入公司时间',
                width: 150,
                align: 'center',
                formatter: formatDateBoxFull,
            },
        ]],
        pagination: true,
        pageSize: 5,
        pageList: [5, 10, 15],
        sortName:'Id',
        sortOrder: 'asc',
        rownumbers:true,
    });
});


