$(function(){
    //登录界面
    $('#login').dialog({
        title: '登录后台',
        width: 300,
        height: 180,
        modal: true,
        iconCls: 'icon-login',
        buttons: '#btn',
    });

    //管理员帐号
    $('#manager').validatebox({
        required: true,
        missingMessage: '请输入管理员帐号',
        invalidMessage: '管理员帐号不得为空',
    });

    //管理员密码
    $('#password').validatebox({
        required: true,
        validType: 'length[6,30]',
        missingMessage: '请输入管理员密码',
        invalidMessage: '管理员密码在6-30 位',
    });

    //加载页面时判断
    if (!$('#manager').validatebox('isValid')) {
        $('#manager').focus();
    } else if (!$('#password').validatebox('isValid')) {
        $('#password').focus();
    }

    //登录按钮
    $('#btn a').click(function () {
        //管理员账号不满足条件时
        if (!($('#manager').validatebox('isValid'))) {
            //光标停在管理员账号输入框中
            $('#manager').focus();
        }
        //密码不满足条件时
        else if (!($('#password').validatebox('isValid'))) {
            //光标停在密码输入框中
            $('#password').focus();
        }
        else {
            //服务器端验证
            $.ajax({
                url: '/Login/Validate',
                type: 'POST',
                data: {
                    manager: $('#manager').val(),
                    password: $('#password').val(),
                },
                beforeSend: function () {
                    $.messager.progress({
                        text: '正在尝试登录...',
                    });
                },
                success: function (data, response, status) {
                    $.messager.progress('close');

                    if (data == 1) {
                        //跳转页面
                        location.href = '/Home/Index';
                    } else {
                        $.messager.alert('登录失败', '用户名或密码错误！', 'warning',
                            function () {
                                $('#password').select();
                            });
                    }
                },
            })
        }
    });
});