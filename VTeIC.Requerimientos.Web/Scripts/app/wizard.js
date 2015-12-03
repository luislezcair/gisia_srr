var html = "<h3>$title</h3>" +
           "<section>" +
                "<div class=\"text-center col-md-12\">" +
                    "<p class=\"lead\" id=\"title_q$id\">$text</p>" +
                "</div>" +
                "$answerField" +
           "</section>";

var textField = "<div id=\"textQuestion\" class=\"col-md-12\">" +
                    "<input type=\"text\" class=\"form-control input-lg\" id=\"input_q$id\" required>" +
                "</div>";

var booleanField = "<div id=\"booleanQuestion\" class=\"btn-group-vertical col-md-12\" role=\"group\">" +
                      "<button class=\"btn btn-default\" type=\"button\" onclick=\"booleanAnswer($id, true)\">SÍ</button>" +
                      "<button class=\"btn btn-default\" type=\"button\" onclick=\"booleanAnswer($id, false)\">NO</button>" +
                   "</div>";

var choiceField = "<div id=\"choiceQuestion\" class=\"btn-group-vertical col-md-12\" role=\"group\">" +
                  "<table>$choices</table></div>";

var choiceItem = "<tr><td>" +
                 "<label><input type=\"checkbox\" value=\"$choiceValue\" id=\"input_q$id\"/>$choiceText</label>" + 
                 "</tr></td>";

var questionList = [];
var answers = [];
var wizard = $("form");
var pivotText = "";
var lastBooleanAnswer = false;

$(function () {
    $.ajax({
        url: "/api/QuestionRest",
        dataType: "json",
        type: "GET",
        success: function(questions) {
            questions.forEach(function (q) {
                addBehavior(q);
                renderQuestion(q);
            });

            createWizard(questions);

            questions.forEach(function (q, index) {
                q.step = wizard.steps("getStep", index);
                q.stepIndex = index;
            });

            questionList = questions;
        }
    });
});

function focusFirstInput(questions, step) {
    var question = questionFromStep(questions, step);
    $(question.inputId).focus();
}

function questionFromStep(questions, stepIndex) {
    for (var i = 0; i < questions.length; i++) {
        if (stepIndex === i)
            return questions[i];
    }
    return null;
}

function renderQuestion(question) {
    var answerField = "";

    if (question.questionType === 0) {
        answerField = textField;
    } else if (question.questionType === 1) {
        answerField = booleanField;
    } else if (question.questionType === 2) {
        var choices = "";

        question.choiceOptions.forEach(function (choice) {
            choices += choiceItem.replace("$choiceValue", choice.id)
                .replace("$choiceText", choice.text);
        });

        answerField = choiceField.replace("$choices", choices);
    }

    wizard.append(html.replace(/\$title/g, question.title)
        .replace(/\$text/g, question.text)
        .replace(/\$answerField/, answerField)
        .replace(/\$id/g, question.id));
}

function createWizard(questions) {
    wizard.steps({
        headerTag: "h3",
        bodyTag: "section",
        stepsOrientation: "vertical",

        onStepChanging: function (event, currentIndex, newIndex) {
            wizard.validate().settings.ignore = ":disabled,:hidden";
            var valid = wizard.valid();

            if (valid) {
                var question = questionFromStep(questions, currentIndex);
                var answer = createAnswer(question);
                var nextQuestion = findQuestionById(questions, answer.next);

                if (question.isPivot) {
                    pivotText = answer.text;
                }

                if (pivotText !== "") {
                    var titleElement = $(nextQuestion.titleId);
                    titleElement.html(nextQuestion.text.replace(/\[previous_answer\]/g, pivotText));
                }

                console.log(nextQuestion.stepIndex + " " + newIndex);

                //if (nextQuestion.stepIndex !== newIndex) {
                //    wizard.steps("setStep", nextQuestion.stepIndex);
                //}
            }

            return valid;
        },

        onStepChanged: function (event, currentIndex) {
            focusFirstInput(questions, currentIndex);
        },

        onInit: function (event, currentIndex) {
            focusFirstInput(questions, currentIndex);
        }
    });
}

function createAnswer(question) {
    var answer = {
        questionId: question.id,
        next: question.link.nextQuestionId
    };

    if (question.questionType === 0) {
        answer.text = $(question.inputId).val();
    } else if (question.questionType === 1) {
        answer.boolean = lastBooleanAnswer;
        answer.next = lastBooleanAnswer ? question.link.nextQuestionId : question.link.nextQuestionNegativeId;
    } else if (question.questionType === 2) {
        // TODO: poner las opciones marcadas...
        answer.choices = [];
    }
    answers.push(answer);
    return answer;
}

function booleanAnswer(questionId, value) {
    lastBooleanAnswer = value;

    var question = findQuestionById(questionList, questionId);
    var answer = createAnswer(question);
    var nextQuestion = findQuestionById(questionList, answer.next);

    wizard.steps("setStep", nextQuestion.stepIndex);
}

function addBehavior(q) {
    q.inputId = "#input_q" + q.id;
    q.titleId = "#title_q" + q.id;
}

function findQuestionById(questions, id) {
    for (var i = 0; i < questions.length; i++) {
        if (questions[i].id === id)
            return questions[i];
    }
    return null;
}

