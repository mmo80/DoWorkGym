var dowork = dowork || {};

dowork = function () {

    var apiContentType = 'application/json; charset=utf-8';//,
        //cookieName = 'tasession',
        //verificationTokenSelector = '#aftoken';


    var consoleLog = function (message) {
        if (window.console && window.console.log) {
            window.console.log(message);
        }
    };


    var onSuccessDefault = function (data) {
        consoleLog(data);
    };


    var onSuccessBeforeCallback = function (data, onCallSuccess, callbackData) {
        hideLoader();

        if (callbackData != null) {
            if (data == null) {
                onCallSuccess(callbackData);
            } else {
                onCallSuccess(data, callbackData);
            }
        }
        else {
            onCallSuccess(data);
        }
    };


    var onErrorDefault = function (jqXhr, textStatus, err) {
        hideLoader();

        // TODO: Send javascript error to a rest api service?
        if (jqXhr != undefined) {
            if (jqXhr.responseText != undefined) {
                consoleLog(jqXhr.responseText);
            }
        }
        consoleLog(textStatus);
        consoleLog(err);
        consoleLog(jqXhr);
        
        notify('An error occured. ' + textStatus, 'danger');
    };


    var notify = function(message, notifytype) {
        // success, info, warning, danger
        $.bootstrapGrowl(message + ' ', {
            type: notifytype,
            allow_dismiss: true,
            width: 'auto'
        });
        
        /*
        $.bootstrapGrowl("another message, yay!", {
            ele: 'body', // which element to append to
            type: 'info', // (null, 'info', 'error', 'success')
            offset: {from: 'top', amount: 20}, // 'top', or 'bottom'
            align: 'right', // ('left', 'right', or 'center')
            width: 250, // (integer, or 'auto')
            delay: 4000,
            allow_dismiss: true,
            stackup_spacing: 10 // spacing between consecutively stacked growls.
        });
        */
    };


    var showLoader = function () {
        $.blockUI({ message: '<h4>Loading...</h4>' });
        consoleLog('Loading...');
    };


    var hideLoader = function () {
        $.unblockUI();
        consoleLog('Load Complete!');
    };


    var setHttpHeader = function (xhr) {
        //var vToken = $(verificationTokenSelector).val();
        //var cookieValue = $.cookie(cookieName);
        
        // Used only for anti-forgery token.
        //xhr.setRequestHeader(cookieName, cookieValue);
    };


    var callApi = function (type, url, data, onCallSuccess, callbackData, onCallError, contextData) {
        if (onCallSuccess == undefined) {
            onCallSuccess = onSuccessDefault;
        }

        if (type != 'GET') {
            data = JSON.stringify(data);
        }

        showLoader();

        if (data == undefined || data == null || data == 'null') {
            $.ajax({
                url: url,
                type: type,
                contentType: apiContentType,
                context: contextData,
                success: function (returndata) {
                    onSuccessBeforeCallback(returndata, onCallSuccess, callbackData);
                },
                error: function (jqXhr, textStatus, err) {
                    onErrorDefault(jqXhr, textStatus, err);
                    if (onCallError != undefined && onCallError != null) {
                        onCallError();
                    }
                },
                beforeSend: function (xhr, settings) {
                    if (settings.context != undefined && settings.context.hasClass('btn')) {
                        settings.context.button('loading');
                    }
                    setHttpHeader(xhr);
                },
                complete: function () {
                    if ($.isFunction(this.button)) {
                        this.button('reset');
                    }
                },
                timeout: 8000
            });
        } else {
            $.ajax({
                url: url,
                type: type,
                data: data,
                contentType: apiContentType,
                context: contextData,
                success: function (returndata) {
                    onSuccessBeforeCallback(returndata, onCallSuccess, callbackData);
                },
                error: function (jqXhr, textStatus, err) {
                    onErrorDefault(jqXhr, textStatus, err);
                    if (onCallError != undefined) {
                        onCallError();
                    }
                },
                beforeSend: function (xhr, settings) {
                    if (settings.context != undefined && settings.context.hasClass('btn')) {
                        settings.context.button('loading');
                    }
                    setHttpHeader(xhr);
                },
                complete: function () {
                    if ($.isFunction(this.button)) {
                        this.button('reset');
                    }
                },
                timeout: 8000
            });
        }
        
    };


    var initPlugins = function () {
        // unblock when ajax activity stops 
        //$(document).ajaxStop($.unblockUI);
    };


    //var initialize = function () {
    //};


    //var todaysDate = function() {
    //    var today = new Date();
    //    var dd = today.getDate();
    //    var mm = today.getMonth() + 1; //January is 0!
    //    var yyyy = today.getFullYear();

    //    if (dd < 10) {
    //        dd = '0' + dd;
    //    }

    //    if (mm < 10) {
    //        mm = '0' + mm;
    //    }

    //    return yyyy + '-' + mm + '-' + dd;
    //};


    var parseJsonValue = function (value, decode) {
        if (decode !== undefined && decode === true) {
            value = decodeURIComponent(value);
        }
        var jsonObj = JSON.parse(value);
        return jsonObj;
    };


    return {
        call: function (type, url, data, onSuccess, callbackData, onError, contextData, isFileUpload) {
            callApi(type, url, data, onSuccess, callbackData, onError, contextData, isFileUpload);
        },
        log: function (message) {
            consoleLog(message);
        },
        setHttpHeader: function (xhr) {
            setHttpHeader(xhr);
        },
        init: function () {
            initPlugins();
            //initialize();
        },
        notify: {
            // success, info, warning, danger
            danger: function (message) {
                notify(message, 'danger');
            },
            warning: function (message) {
                notify(message, 'warning');
            },
            info: function (message) {
                notify(message, 'info');
            },
            success: function (message) {
                notify(message, 'success');
            }
        },
        //today: function() {
        //    return todaysDate();
        //},
        parseJson: function (value, decode) {
            return parseJsonValue(value, decode);
        }
    };

}(); // the parens here cause the anonymous function to execute and return

/*

dowork.notify.success()
dowork.notify.info()
dowork.notify.warning()
dowork.notify.danger()

*/

$(function () {
    dowork.log('dowork.init()');
    dowork.init();
});