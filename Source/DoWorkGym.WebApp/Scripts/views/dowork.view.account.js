var dowork = dowork || {};
dowork.views = dowork.views || {};

dowork.views.account = function () {

    var accountViewModel = {
        user: {
            email: ko.observable(null),
            password: ko.observable(null),
            rememberMe: ko.observable(false)
        }
    };
    
    var registerViewModel = {
        register: {
            email: ko.observable(null),
            password: ko.observable(null)
        }
    };

    var prefixApiPath = '/api/account';


    var registerUserOnSuccess = function() {
        dowork.notify.success('Registered user successfully');
        location.href = '/home/training';
    };

    var registerUser = function() {
        var user = registerViewModel.register;

        var data = {            
            Email: user.email(),
            Password: user.password()
        };
        
        dowork.log(data);
        
        dowork.call(
            'POST',
            prefixApiPath + '/register',
            data,
            registerUserOnSuccess);
    };


    var loginUserOnSuccess = function() {
        dowork.notify.success('User is logged in!');
        location.reload(true);
    };

    var loginUser = function() {
        var user = accountViewModel.user;

        var data = {
            Email: user.email(),
            Password: user.password(),
            RememberMe: user.rememberMe()
        };

        dowork.log(data);

        dowork.call(
            'POST',
            prefixApiPath + '/login',
            data,
            loginUserOnSuccess);
    };


    var logoutUserOnSuccess = function () {
        dowork.notify.success('User logged out');
        location.href = '/';
    };

    var logoutUser = function () {
        dowork.call(
            'POST',
            prefixApiPath + '/logout',
            null,
            logoutUserOnSuccess);
    };


    var initialize = function () {

        if ($('#loginForm').length) {
            ko.applyBindings(accountViewModel, document.getElementById('loginForm'));
            
            // cookie for remember me
            var cookieUser = $.cookie('dowork');
            dowork.log('dowork cookie: ' + cookieUser);
            if (cookieUser !== undefined) {
                var jsonObj = dowork.parseJson(cookieUser);
                if (jsonObj !== undefined && jsonObj.em !== undefined) {
                    if ($('#email').length) {
                        accountViewModel.user.email(jsonObj.em);
                        accountViewModel.user.rememberMe(true);
                        //$('#email').val(jsonObj.em);
                        //$('#rememberme').prop('checked', true);
                    }
                    dowork.log(jsonObj.em);
                }
            }
            // end

            dowork.log('tasession cookie: ' + $.cookie('tasession'));
        }
        
        if ($('#registerForm').length) {
            ko.applyBindings(registerViewModel, document.getElementById('registerForm'));
        }

        if ($('#register-btn').length) {
            $('#register-btn').on('click', registerUser);
        }
        
        if ($('#login-btn').length) {
            $('#login-btn').on('click', loginUser);
        }
        // logout-call
        if ($('#logout-call').length) {
            $('#logout-call').on('click', logoutUser);
        }
    };


    return {
        init: function () {
            initialize();
        }
    };
}();


$(function () {
    dowork.log('dowork.views.account.init()');
    dowork.views.account.init();
});