var dowork = dowork || {};
dowork.views = dowork.views || {};

dowork.views.exercise = function () {

    var addExerciseViewModel = {
        trainings: ko.mapping.fromJS([]),
        exercises: ko.mapping.fromJS([])
    };

    var prefixApiPath = '/api/exercise',
        state = {
            trainingId: ''
        };


    var listTrainingOnSuccess = function(data) {
        dowork.log(data);
        ko.mapping.fromJS(data, addExerciseViewModel.trainings);
    };

    var listTraining = function () {
        var url = prefixApiPath + '/gettraininglist';
        dowork.call('GET', url, null, listTrainingOnSuccess);
    };


    var onTrainingListOnSuccess = function (data) {
        dowork.log(data);
        ko.mapping.fromJS(data, addExerciseViewModel.exercises);
    };

    var refreshExerciseList = function () {
        var data = {
            Id: state.trainingId
        };

        // getexercisebytraining
        dowork.call(
            'GET',
            prefixApiPath + '/getexercisebytraining',
            data,
            onTrainingListOnSuccess);
    };
    
    var onTrainingListTrigger = function () {
        var id = $(this).find("option:selected").val();
        dowork.log(id);
        state.trainingId = id;

        refreshExerciseList();
    };


    var addExerciseOnSuccess = function(data) {
        dowork.log(data);
        refreshExerciseList();

        $('#exercise').val('');
        dowork.notify.success('Exercise added successfull');
    };

    var addExercise = function() {
        var value = $('#exercise').val();
        
        dowork.log(state.trainingId);
        dowork.log(value);
        
        var data = {
            TrainingId: state.trainingId,
            Name: value
        };

        dowork.call(
            'POST',
            prefixApiPath + '/addexercise',
            data,
            addExerciseOnSuccess);
    };


    var deleteExerciseOnSuccess = function (data) {
        dowork.log(data);
        refreshExerciseList();

        dowork.notify.warning('Exercise deleted successfull');
    };

    var deleteExercise = function(e) {
        dowork.log('delete');

        var id = $(e.target).parent().data('id');

        dowork.log(id);

        var data = {
            TrainingId: state.trainingId,
            Id: id
        };

        // DELETE api/exercise/deleteexercise
        dowork.call(
            'DELETE',
            prefixApiPath + '/deleteexercise',
            data,
            deleteExerciseOnSuccess);
    };


    var initialize = function () {
        // ApplyBindings
        ko.applyBindings(addExerciseViewModel);

        $('#trainings').on('change', onTrainingListTrigger);
        $('#addexercisebtn').on('click', addExercise);
        $('.table').on('click', '.delete-link', deleteExercise);

        listTraining();
    };


    return {
        init: function () {
            initialize();
        }
    };
}(); // the parens here cause the anonymous function to execute and return


$(function () {
    dowork.log('dowork.views.exercise.init()');
    dowork.views.exercise.init();
});