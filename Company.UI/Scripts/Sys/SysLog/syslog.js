
$(function () {
   
    $('#syslog_detail').hide();

    syslogButton = {
        detail: function () {
            //debugger
            var rows = $('#syslog_table').datagrid('getSelections');
            console.log(rows);
            if (rows.length > 1) {
                $.messager.alert(' 警告操作 ', '只能展示一条记录！', 'warning');
            } else if (rows.length == 1) {

                $('#syslog_detail').show();
                //详细dialog弹窗
                $('#syslog_detail').dialog({
                    width: 500,
                    title: '日志详细',
                    iconCls: 'icon-search',
                    modal: true,
                });
               
                $.ajax({
                    type: 'POST',
                    url: '/SysLog/GetDetail',
                    data: {
                        id:rows[0].Id,
                    },
                    beforeSend: function () {
                        $.messager.progress({
                            text:'正在尝试获取数据...',
                        });
                    },
                    success: function (data) {
                        $.messager.progress('close');
                        console.log(data);
                        if (data) {
                            $('#syslog_detail').form('load', {
                                id: data.row[0].Id,
                                operator_detail: data.row[0].Operator,
                                message_detail: data.row[0].Message,
                                result_detail: data.row[0].Result,
                                type_detail: data.row[0].Type,
                                module_detail: data.row[0].Module,
                                createTime_detail: formatDateBoxFull(data.row[0].CreateTime),
                            })
                        }
                    }
                })
            }
        },
    };

    $('#syslog_table').datagrid({
        title: '系统日志',
        iconCls: 'icon-search',
        url: '/SysLog/GetList',
        method: 'post',
        fitColumns: true,
        //toolbar: "#staff_tabTools",
        columns: [[
            {
                field: 'Id',
                title: 'ID',
                width: 40,
                align: 'center',
                checkbox: true,
                hidden: true,
            },
            {
                field: 'Operator',
                title: '操作人',
                width: 40,
                align: 'center',
            },
            {
                field: 'Message',
                title: '信息',
                width: 280,
                align: 'center',
            },
            {
                field: 'Result',
                title: '结果',
                width: 40,
                align: 'center',
            },
            {
                field: 'Type',
                title: '类型',
                width: 40,
                align: 'center',
            },
            {
                field: 'Module',
                title: '模块',
                width: 60,
                align: 'center',
            },
            {
                field: 'CreateTime',
                title: '添加时间',
                width: 65,
                align: 'center',
                formatter: formatDateBoxFull,
            },
        ]],
        pagination: true,
        pageSize: 15,
        pageList: [5, 10, 15],
        sortName: 'Id',
        sortOrder: 'asc',
        rownumbers: true,
        //在用户点击一行的时候触发
        onClickRow: function (rowIndex, rowData) {
            
            
        },
        //在鼠标右击一行记录的时候触发。
        onRowContextMenu: function (e, rowIndex, rowData) {
            $('#syslog_menu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
            e.preventDefault();
        },
    });
});


