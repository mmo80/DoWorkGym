var dowork = dowork || {};
dowork.views = dowork.views || {};

dowork.views.workoutdetail = function () {

    var addWorkoutDetailViewModel = {
        workout: {
            trainingId: ko.observable(null),
            exerciseId: ko.observable(null),
            workoutDate: ko.observable(null),
            set: ko.observable(1),
            weight: ko.observable(0),
            reps: ko.observable(1),
            unit: ko.observable('kg'),
            units: ko.observableArray(['kg', 'weight', 'hrs', 'km']),
            note: ko.observable(null)
        },
        workouts: ko.mapping.fromJS([])
    };

    var prefixApiPath = '/api/workout';


    var addWorkoutOnSuccess = function() {
        dowork.log('addWorkoutOnSuccess');
        
        dowork.notify.success('Workout added successfull');

        var set = addWorkoutDetailViewModel.workout.set();
        set = parseInt(set) + 1;
        addWorkoutDetailViewModel.workout.set(set);

        workoutList();
    };

    var addWorkout = function() {
        var jsonData = ko.toJSON(addWorkoutDetailViewModel.workout);

        var obj = addWorkoutDetailViewModel.workout;

        dowork.log(jsonData);

        var data = {
            TrainingId: obj.trainingId(),
            ExerciseId: obj.exerciseId(),
            Date: obj.workoutDate(),
            Set: obj.set(),
            Weight: obj.weight(),
            Reps: obj.reps(),
            Unit: obj.unit(),
            Note: obj.note()
        };

        dowork.log(data);

        if (data.Weight != undefined && String(data.Weight).indexOf(',') != -1) {
            data.Weight = data.Weight.replace(',', '.');
            data.Weight = parseFloat(data.Weight).toFixed(2);
        }
        
        dowork.call(
            'POST',
            prefixApiPath + '/addworkout',
            data,
            addWorkoutOnSuccess);
    };


    var deleteWorkoutOnSuccess = function (data) {
        dowork.log(data);
        workoutList();

        dowork.notify.warning('Workout deleted successfull');
    };

    var deleteWorkout = function (e) {
        var id = $(e.target).data('id');

        dowork.log(id);

        var data = {
            Id: id
        };
        
        dowork.call(
            'DELETE',
            prefixApiPath + '/deleteworkout',
            data,
            deleteWorkoutOnSuccess);
    };


    var workoutListOnSuccess = function (data) {
        dowork.log(data);
        ko.mapping.fromJS(data, addWorkoutDetailViewModel.workouts);
    };

    var workoutList = function() {

        var data = {
            ExerciseId: $('#exerciseid').val()
        };
        
        dowork.call(
            'GET',
            prefixApiPath + '/getworkouts',
            data,
            workoutListOnSuccess);
    };


    var initialize = function () {
        // ApplyBindings
        ko.applyBindings(addWorkoutDetailViewModel);

        // init values
        addWorkoutDetailViewModel.workout.trainingId($('#trainingid').val());
        addWorkoutDetailViewModel.workout.exerciseId($('#exerciseid').val());
        addWorkoutDetailViewModel.workout.workoutDate($('#today').val());

        $('#addWorkout').on('click', addWorkout);
        $('#workouts').on('click', '.delete-link', deleteWorkout);

        workoutList();
    };


    return {
        init: function () {
            initialize();
        }
    };
}();


$(function () {
    dowork.log('dowork.views.workoutdetail.init()');
    dowork.views.workoutdetail.init();
});