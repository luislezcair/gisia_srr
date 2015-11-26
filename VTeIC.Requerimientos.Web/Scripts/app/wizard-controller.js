var app = angular.module("WizardApp", []);

app.controller("QuestionController", function ($scope, $http) {
    $http.get("/api/QuestionRest/").success(function(data, status, headers, config) {
        $scope.questions = data;
        $scope.$broadcast("dataloaded");
    });

    $http.get("/api/QuestionRest/links").success(function(data, status, headers, config) {
        $scope.questionLinks = data;
    });
});


app.directive("afterRender", function($timeout) {
    return {
        link: function ($scope, element, attr) {
            $scope.$on("dataloaded", function() {
                $timeout(function () {
                    var wizard = angular.element("#wizard");
                    wizard.steps({
                        headerTag: "h3",
                        bodyTag: "section",
                        stepsOrientation: "vertical"
                    });

                    $scope.questions.forEach(function(q, i) {
                        q.step = wizard.steps("getStep", i);
                    });
                }, 0);
            });
        }
    }
});