var dowork = dowork || {};
dowork.views = dowork.views || {};

dowork.views.history = function () {

    var historyViewModel = {
        historyList: ko.mapping.fromJS([])
    };

    var prefixApiPath = '/api/history';
    var calendar;

    var historicalOnSuccess = function (data) {
        dowork.log(data);
        ko.mapping.fromJS(data, historyViewModel.historyList);

        
    };

    var historical = function () {
        dowork.call(
            'GET',
            prefixApiPath + '/gethistory',
            null,
            historicalOnSuccess);

    };


    var loadCalendarOnSuccess = function (data) {
        dowork.log(data);

        calendar = $("#calendar").calendar(
            {
                tmpl_path: "/Scripts/plugins/calendar/tmpls/",
                language: 'sv-SE',
                //events_source: prefixApiPath + '/GetHistoryCalendar'
                events_source: data.Result,
                onAfterViewLoad: function (view) {
                    $('#date-month-name').text(this.getTitle());
                },
            });

        events();
    };


    var loadCalendar = function () {
        dowork.call(
            'GET',
            prefixApiPath + '/GetHistoryCalendar',
            null,
            loadCalendarOnSuccess);
    };


    var events = function () {
        

        $('.btn-group button[data-calendar-nav]').each(function () {
            var $this = $(this);
            $this.click(function () {
                calendar.navigate($this.data('calendar-nav'));
            });
        });

        $('.btn-group button[data-calendar-view]').each(function () {
            var $this = $(this);
            $this.click(function () {
                calendar.view($this.data('calendar-view'));
            });
        });
    };


    var initialize = function () {
        // ApplyBindings
        ko.applyBindings(historyViewModel);

        //historical();
        loadCalendar();
    };


    return {
        init: function () {
            initialize();
        }
    };
}(); // the parens here cause the anonymous function to execute and return


$(function () {
    dowork.log('dowork.views.history.init()');
    dowork.views.history.init();
});