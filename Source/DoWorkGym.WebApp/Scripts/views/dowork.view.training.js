var dowork = dowork || {};
dowork.views = dowork.views || {};

dowork.views.training = function () {

    var addtrainingViewModel = {
        trainings: ko.mapping.fromJS([])
    };

    var prefixApiPath = '/api/training';


    var listTrainingOnSuccess = function (data) {
        dowork.log(data);
        ko.mapping.fromJS(data, addtrainingViewModel.trainings);
    };

    var listTraining = function () {
        dowork.log('listTraining');
        
        var url = prefixApiPath + '/gettraininglist/';
        dowork.call('GET', url, null, listTrainingOnSuccess);
    };


    var trainingAddOnSuccess = function(data) {
        dowork.log(data);
        
        listTraining();
        
        dowork.notify.success('Training added successfull');
    };

    var trainingAdd = function() {
        var data = {
            Id: '',
            Name: $('#training').val()
        };
        
        dowork.log(data);

        dowork.call(
            'POST',
            prefixApiPath + '/addtraining',
            data,
            trainingAddOnSuccess);
    };
    


    var deleteTrainingOnSuccess = function (data) {
        dowork.log(data);
        listTraining();

        dowork.notify.warning('Training deleted successfull');
    };

    var deleteTraining = function (e) {
        var id = $(e.target).parent().data('id');

        dowork.log(id);

        var data = {
            Id: id
        };

        dowork.call(
            'DELETE',
            prefixApiPath + '/deletetraining',
            data,
            deleteTrainingOnSuccess);
    };



    var initialize = function () {
        // ApplyBindings
        ko.applyBindings(addtrainingViewModel);

        $('#addtrainingbtn').click(trainingAdd);
        $('.table').on('click', '.delete-link', deleteTraining);

        listTraining();
    };


    return {
        init: function () {
            initialize();
        }
    };
}(); // the parens here cause the anonymous function to execute and return


$(function () {
    dowork.log('dowork.views.training.init()');
    dowork.views.training.init();
});