define("login", function (e) {
    var loginObj = {
        $ele: $(".login"),
        _init: function () {
            if (this.$el.length > 0) {
                this._bindEvent();
            }
        },
        _bindEvent: function () {
            var _this = this;
            var $root = _this.$el;
            $root.on('click', '.submit', function () {
                if (_this._checkForm()) {
                    var userName = $root.find('input[name=username]').val();
                    var password = $root.find('input[name=password]').val();
                    $.ajax({
                        url: YouTravel.info.loginurl,
                        type: "POST",
                        data: { "grant_type": "password", "username": userName, "password": password },
                        dataType: "json",
                        headers: {
                            "Content-Type": "application/x-www-form-urlencoded"
                        }
                    }).done(function (data) {
                        console.log(data);
                        sessionStorage.removeItem("authorizationData");
                        sessionStorage.removeItem("accessToken");
                        sessionStorage.setItem("authorizationData", JSON.stringify(data));
                        sessionStorage.setItem("accessToken", JSON.stringify(data.access_token));
                        location.href = 'index.html';
                    });

                }
            }).on('keydown', 'input[type="text"], input[type="password"]', function (e) {
                if (e.keyCode == 13) {
                    $root.find('input.submit').trigger('click');
                }
            }).on('click', 'a.forgotpass', function () {
                $(this).closest('.box').removeClass('active');
                $('.forgot').addClass('active');
                Forgot._init();
            });
        },
        //验证数据
        _checkForm: function () {
            var error = 0;
            var userName = this.$el.find('input[name=username]');
            var password = this.$el.find('input[name=password]');

            if (userName.val().length == 0) {
                userName.closest('.form-group').addClass('has-error');
                $('.login .erromes').html('请输入用户名');
                $('.login .errors').fadeIn(200);
                userName.focus();
                error++;
            } else {
                if (password.val().length == 0) {
                    password.closest('.form-group').addClass('has-error');
                    $('.login .erromes').html('请输入密码');
                    $('.login .errors').fadeIn(200);
                    password.focus();
                    error++;
                } else {
                    $('.login .errors').fadeOut(200);
                }
            }
            this.$el.find('input.form-control').on('keyup', function () {
                $(this).closest('.form-group').removeClass('has-error');
            });
            return error == 0;
        }
    };
});