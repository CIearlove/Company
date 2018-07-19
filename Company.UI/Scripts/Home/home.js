$(function () {

    obj = {
        editRow: undefined,

        search: function () {
            $('#tab1').datagrid('load', {
                name: $.trim($('input[name="name"]').val()),
                date_from: $('input[name="date_from"]').val(),
                date_to: $('input[name="date_to"]').val(),
            });
        },
        //添加一行
        add: function () {
            //出现两个按钮
            $('#save,#redo').show();

            if (this.editRow == undefined) {
                $('#tab1').datagrid('insertRow', {
                    index: 0,
                    row: {

                    }
                });
                //将第一行设置为可编辑状态
                $('#tab1').datagrid('beginEdit', 0);
                //有正在编辑的行
                this.editRow = 0;
            }
        },

        save: function () {
            //结束某一行的编辑状态
            $('#tab1').datagrid('endEdit', this.editRow);
        },
        redo: function () {
            //回滚
            $('#tab1').datagrid('rejectChanges');
            //取消编辑时应当隐藏这两个按钮
            $('#save,#redo').hide();
            //没有正在编辑的行
            this.editRow = undefined;
        },

        edit: function () {
            //返回所有被选中的行
            var rows = $('#tab1').datagrid('getSelections');
            //console.log(rows);
            //只能修改一行
            if (rows.length == 1) {
                if (this.editRow == undefined) {
                    //获取到当前行的索引
                    var index = $('#tab1').datagrid('getRowIndex', rows[0]);
                    $('#save,#redo').show();
                    $('#tab1').datagrid('beginEdit', index);
                    this.editRow = index;
                    //取消指定选择的行
                    $('#tab1').datagrid('unselectRow', index);
                }
                else {
                    //结束某一行的编辑状态
                    $('#tab1').datagrid('endEdit', this.editRow);
                }
            } else {
                $.messager.alert('警告', '只能修改一行!', 'warning');
            };
            
        },

        remove: function () {
            //返回所有被选中的行
            var rows = $('#tab1').datagrid('getSelections');
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
                            url: '/Home/Delete',
                            data: {
                                Ids: ids,
                            },
                            beforeSend: function () {
                                //显示载入状态
                                $('#tab1').datagrid('loading');
                            },
                            success: function (data) {
                                if (data) {
                                    //隐藏载入状态
                                    $('#tab1').datagrid('loaded');
                                    //加载和显示第一页的所有行，即刷新当前页。
                                    $('#tab1').datagrid('load');
                                    //取消所有当前页中的所有行。
                                    $('#tab1').datagrid('unselectAll');
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
                        $('#tab1').datagrid('rejectChanges');
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的记录!', 'info');
            }
        }
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
                align: 'center',
                checkbox:true,
            },
            {
                field: 'Name',
                title: '名称',
                width: 150,
                align: 'center',
                editor: {
                    type: 'validatebox',
                    options: {
                        required:true,
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
                        textField:'sex',
                        required: true,
                        panelHeight: 60,
                        editable: false,
                        data: [
                            {
                                "sexValue": "男",
                                "sex":"男"
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
                    },
                }
            },
        ]],
        pagination: true,
        pageSize: 5,
        pageList: [5, 10, 15],
        sortName:'Id',
        sortOrder: 'asc',
        rownumbers: true,
        //结束某一行的编辑状态之后才执行这个事件
        onAfterEdit: function (rowIndex, rowData, changes) {
            //编辑完成时应当隐藏这两个按钮
            $('#save,#redo').hide();
            //没有正在编辑的行
            obj.editRow = undefined;

            //返回被改变的所有行的数组
            //新增行
            var inserted = $('#tab1').datagrid('getChanges', 'inserted');
            //更新行
            var updated = $('#tab1').datagrid('getChanges', 'updated');
            var url = '';
            var change = '';
            //新增用户
            if (inserted.length > 0) {
                url = '/Home/Add';
                change = inserted;
                info = '条记录被新增!';
            }
            //修改用户
            if (updated.length > 0) {
                url = '/Home/Update';
                change = updated;
                info = '条记录被修改!'
            }

            $.ajax({
                type: 'POST',
                //服务器程序地址
                url: url,
                //发送出去的数据
                data: {
                    Name: change[0].Name,
                    Age: change[0].Age,
                    Sex: change[0].Sex,
                    CreateTime: change[0].CreateTime
                },
                beforeSend: function () {
                    //显示载入状态
                    $('#box').datagrid('loading');
                },
                //服务器端返回的数据在data中
                success: function (data) {
                    if (data) {
                        //隐藏载入状态
                        $('#box').datagrid('loaded');
                        //加载和显示第一页的所有行，即刷新当前页。
                        $('#box').datagrid('load');
                        //取消所有当前页中的所有行。
                        $('#box').datagrid('unselectAll');
                        //提示框
                        $.messager.show({
                            title: '提示',
                            msg: data + info,
                        });
                        obj.editRow = undefined;
                    } else {

                    };
                },
            });
        }
    });
});


