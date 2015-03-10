var dowork = dowork || {};
dowork.views = dowork.views || {};

dowork.views.workout = function () {

    var addWorkoutViewModel = {
        trainings: ko.mapping.fromJS([]),
        exercises: ko.mapping.fromJS([]),
        trainingId: ko.observable(),
        hasTrainings: function () {
            return (trainings().length > 0);
        }
    };

    var prefixApiPath = '/api/workout',
        prefixExerciseApiPath = '/api/exercise',
        state = {
            trainingId: ''
        };
    

    var listTrainingOnSuccess = function (data) {
        dowork.log(data);
        ko.mapping.fromJS(data, addWorkoutViewModel.trainings);
    };

    var listTraining = function () {
        var url = prefixExerciseApiPath + '/gettraininglist';
        dowork.call('GET', url, null, listTrainingOnSuccess);
    };


    var onTrainingListOnSuccess = function (data) {
        dowork.log(data);
        ko.mapping.fromJS(data, addWorkoutViewModel.exercises);
    };

    var refreshExerciseList = function () {
        var data = {
            TrainingId: state.trainingId
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
        addWorkoutViewModel.trainingId(id);

        refreshExerciseList();
    };


    var initialize = function () {
        // ApplyBindings
        ko.applyBindings(addWorkoutViewModel);

        $('#trainings').on('change', onTrainingListTrigger);
        
        listTraining();
    };


    return {
        init: function () {
            initialize();
        }
    };
}(); // the parens here cause the anonymous function to execute and return


$(function () {
    dowork.log('dowork.views.workout.init()');
    dowork.views.workout.init();
});