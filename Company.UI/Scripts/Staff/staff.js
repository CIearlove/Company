$(function () {
    staffButton = {
        editRow: undefined,

        //导出
        export: function () {
            $.ajax({
                type:'POST',
                url: '/Staff/Export',
                data: {
                    name: $.trim($('input[name="name"]').val()),
                    date_from: $('input[name="date_from"]').val(),
                    date_to: $('input[name="date_to"]').val(),
                },
            });
        },
        //清空
        reload: function () {
            
            $('#staff_tabTools').find('input').val('');
            $('#staff_table').datagrid('load', {});
        },

        search: function () {
            $('#staff_table').datagrid('load', {
                name: $.trim($('input[name="name"]').val()),
                date_from: $('input[name="date_from"]').val(),
                date_to: $('input[name="date_to"]').val(),
            });
        },
        //添加一行
        add: function () {
            //出现两个按钮
            $('#staff_tabTools_save,#staff_tabTools_redo').show();

            if (this.editRow == undefined) {
                $('#staff_table').datagrid('insertRow', {
                    index: 0,
                    row: {

                    }
                });
                //将第一行设置为可编辑状态
                $('#staff_table').datagrid('beginEdit', 0);
                //有正在编辑的行
                this.editRow = 0;
            }
        },

        save: function () {
            //结束某一行的编辑状态
            $('#staff_table').datagrid('endEdit', this.editRow);
        },
        redo: function () {
            //回滚
            $('#staff_table').datagrid('rejectChanges');
            //取消编辑时应当隐藏这两个按钮
            $('#staff_tabTools_save,#staff_tabTools_redo').hide();
            //没有正在编辑的行
            this.editRow = undefined;
        },

        edit: function () {
            //返回所有被选中的行
            var rows = $('#staff_table').datagrid('getSelections');
            //console.log(rows);
            //只能修改一行
            if (rows.length == 1) {
                if (this.editRow == undefined) {
                    //获取到当前行的索引
                    var index = $('#staff_table').datagrid('getRowIndex', rows[0]);
                    $('#staff_tabTools_save,#staff_tabTools_redo').show();
                    $('#staff_table').datagrid('beginEdit', index);
                    this.editRow = index;
                    //取消指定选择的行
                    $('#staff_table').datagrid('unselectRow', index);
                }
                else {
                    //结束某一行的编辑状态
                    $('#staff_table').datagrid('endEdit', this.editRow);
                }
            } else {
                $.messager.alert('警告', '只能修改一行!', 'warning');
            };

        },

        remove: function () {
            //返回所有被选中的行
            var rows = $('#staff_table').datagrid('getSelections');
            if (rows.length > 0) {
                $.messager.confirm('确定操作', '您真的要删除所选的记录么？', function (flag) {
                    //确认删除flag=true;
                    if (flag) {
                        var ids = [];
                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].Id);
                        }

                        //将选择的id传入后台。
                        $.ajax({
                            type: 'POST',
                            url: '/Staff/Delete',
                            data: {
                                Ids: ids,
                            },
                            beforeSend: function () {
                                //显示载入状态
                                $('#staff_table').datagrid('loading');
                            },
                            success: function (data) {
                                if (data) {
                                    //隐藏载入状态
                                    $('#staff_table').datagrid('loaded');
                                    //加载和显示第一页的所有行，即刷新当前页。
                                    $('#staff_table').datagrid('load');
                                    //取消所有当前页中的所有行。
                                    $('#staff_table').datagrid('unselectAll');
                                    $.messager.show({
                                        title: '提示',
                                        msg: data.deletedNum + '条记录被删除！',
                                    })
                                } else {

                                };
                            },
                        });
                    }
                    else {
                        //回滚
                        $('#staff_table').datagrid('rejectChanges');
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的记录!', 'info');
            }
        }
    };

    $('#staff_table').datagrid({
        title: '员工列表',
        iconCls: 'icon-search',
        url: '/Staff/GetList',
        method: 'post',
        fitColumns: true,
        fixed:true,
        toolbar: "#staff_tabTools",
        columns: [[
            {
                field: 'Id',
                title: 'ID',
                width: 50,
                align: 'center',
                checkbox: true,
            },
            {
                field: 'Name',
                title: '名称',
                width: 150,
                align: 'center',
                editor: {
                    type: 'validatebox',
                    options: {
                        required: true,
                    },
                }
            },
            {
                field: 'Age',
                title: '年龄',
                width: 80,
                align: 'center',
                editor: {
                    type: 'validatebox',
                    options: {
                        required: true,
                    },
                }
            },
            {
                field: 'Sex',
                title: '性别',
                width: 150,
                align: 'center',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'sexValue',
                        textField: 'sex',
                        required: true,
                        panelHeight: 60,
                        editable: false,
                        data: [
                            {
                                "sexValue": "男",
                                "sex": "男"
                            },
                            {
                                "sexValue": "女",
                                "sex": "女"
                            }
                        ]
                    },
                }
            },
            {
                field: 'CreateTime',
                title: '进入公司时间',
                width: 150,
                align: 'center',
                formatter: formatDateBoxFull,
                editor: {
                    type: 'datetimebox',
                    options: {
                        required: true,
                        panelHeight: 250
                    },
                }
            },
        ]],
        pagination: true,
        pageSize: 15,
        pageList: [5, 10,15],
        sortName: 'Id',
        sortOrder: 'asc',
        rownumbers: true,
        //结束某一行的编辑状态之后才执行这个事件
        onAfterEdit: function (rowIndex, rowData, changes) {
            //编辑完成时应当隐藏这两个按钮
            $('#save,#redo').hide();
            //没有正在编辑的行
            staffButton.editRow = undefined;

            //返回被改变的所有行的数组
            //新增行
            var inserted = $('#staff_table').datagrid('getChanges', 'inserted');
            //更新行
            var updated = $('#staff_table').datagrid('getChanges', 'updated');
            var url = '';
            var change = '';
            //新增用户
            if (inserted.length > 0) {
                url = '/Staff/Add';
                change = inserted;
                info = '条记录被新增!';
            }
            //修改用户
            if (updated.length > 0) {
                url = '/Staff/Update';
                change = updated;
                info = '条记录被修改!'
            }

            $.ajax({
                type: 'POST',
                //服务器程序地址
                url: url,
                //发送出去的数据
                data: {
                    Id: change[0].Id,
                    Name: change[0].Name,
                    Age: change[0].Age,
                    Sex: change[0].Sex,
                    CreateTime: change[0].CreateTime
                },
                beforeSend: function () {
                    //显示载入状态
                    $('#staff_table').datagrid('loading');
                },
                //服务器端返回的数据在data中
                success: function (data) {
                    if (data) {
                        //隐藏载入状态
                        $('#staff_table').datagrid('loaded');
                        //加载和显示第一页的所有行，即刷新当前页。
                        $('#staff_table').datagrid('load');
                        //取消所有当前页中的所有行。
                        $('#staff_table').datagrid('unselectAll');
                        //提示框
                        $.messager.show({
                            title: '提示',
                            msg: data + info,
                        });
                        staffButton.editRow = undefined;
                    } else {

                    };
                },
            });
        }
    });
});