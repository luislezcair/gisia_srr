var app = angular.module("WizardApp", []);

//app.factory("wizardService", function () {
//    return function () {
//        console.log(angular.element("#wizard").innerHTML);
//        return angular.element("#wizard").innerHTML;
//    };
//});

app.controller("QuestionController", ["$scope", "$http", "$timeout", "$log", function ($scope, $http, $timeout, $log) {
    $http.get("/api/QuestionRest/").success(function (data, status, headers, config) {

        //var questions = [];
        //data.map(function(q) {
        //    //q.answerYes = function () {
        //    //    $timeout(function() {
        //    //        console.log("YES");
        //    //        alert("NO");
        //    //    });
        //    //}

        //    //q.answerNo = function() {
        //    //    console.log("NO");
        //    //    alert("YES");
        //    //}

        //    questions.push(q);
        //});
        //$scope.questions = questions;
        $scope.questions = data;
        $scope.$broadcast("dataloaded");
    });

    $scope.answerYes = function () {
        console.log(q);
    }

    //$log.log("A");
}]);


app.directive("afterRender", ["$timeout", "$compile", function($timeout, $compile) {
    return {
        link: function ($scope, element, attr) {
            $scope.$on("dataloaded", function() {
                $timeout(function () {
                    function focusFirstElement(step) {
                        var question = questionFromStep($scope.questions, step);
                        var input = angular.element("#input_q" + question.id);
                        if (input)
                            input.focus();
                    };

                    var wizard = angular.element("#wizard");
                    wizard.steps({
                        headerTag: "h3",
                        bodyTag: "section",
                        stepsOrientation: "vertical",

                        //onStepChanging: function (event, currentIndex, newIndex) {
                        //    $timeout(function () { focusFirstElement(newIndex); });
                        //    return true;
                        //},

                        onInit: function (event, currentIndex) {
                            $timeout(function() {
                                focusFirstElement(currentIndex);
                            });
                        }
                    });

                    var w = $compile(angular.element("#wizard").html())($scope);
                    console.log(w);
                    //var w = $compile(angular.element("#wizard").html())($scope);
                    wizard.html("");
                    wizard.html(w);
                        
                    $scope.questions.forEach(function (q, i) {
                        q.step = wizard.steps("getStep", i);
                    });

                    //console.log(angular.element("#wizard").html());


                }, 0);
            });
        }
    }
}]);

function questionFromStep(questions, stepIndex) {
    var i = 0;
    for (i = 0; i < questions.length; i++) {
        if (stepIndex === i)
            return questions[i];
    }
    return null;
}

//app.directive("compileData", function ( $compile ) {
//    return {
//        scope: true,
//        link: function ( scope, element, attrs ) {

//            var elmnt;

//            attrs.$observe("template", function ( myTemplate ) {
//                if ( angular.isDefined( myTemplate ) ) {
//                    // compile the provided template against the current scope
//                    elmnt = $compile( myTemplate )( scope );

//                    element.html(""); // dummy "clear"

//                    element.append( elmnt );
//                }
//            });
//        }
//    };
//});

