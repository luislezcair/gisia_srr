var app = angular.module("WizardApp", []);

app.controller("QuestionController", function ($scope, $http) {
    $http.get("/api/QuestionRest/").success(function(data, status, headers, config) {
        $scope.questions = data;
        $scope.$broadcast("dataloaded");
    });
});


app.directive("afterRender", function($timeout) {
    return {
        link: function ($scope, element, attr) {
            $scope.$on("dataloaded", function() {
                $timeout(function() {
                    $("#wizard").steps({
                        headerTag: "h3",
                        bodyTag: "section",
                        stepsOrientation: "vertical"
                    });
                }, 0);
            });
        }
    }
});