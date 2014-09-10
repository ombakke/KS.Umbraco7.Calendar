angular.module("umbraco").controller("KS.CalendarController", function ($scope, $parse, assetsService, KSCalendarResource) {
    assetsService.loadCss("/app_plugins/KS.Umbraco7.Calendar/css/bootstrap-datetimepicker.min.css");
    assetsService.load([
       "/app_plugins/KS.Umbraco7.Calendar/js/bootstrap-datetimepicker.min.js"
    ])
    .then(function () {
        $("#StartDateWrapper").datetimepicker({
            pickSeconds: false
        });
        $("#EndDateWrapper").datetimepicker({
            pickSeconds: false
        });

        $("#dateRecurUntilWrapper").datetimepicker({
            pickTime: false
        });

        $("#dateExceptWrapper").datetimepicker({
            pickTime: false
        });
        

        $(".datepicker").on('changeDate', function () {
            var $inp = $(this).find("input");
            var mod = $parse($inp.attr("ng-model"));
            if ($inp.val() != '') {
                var date = convertPickerDateTime($(this).data('datetimepicker').getLocalDate(), $(this).data('datetimepicker').pickTime);
                mod.assign($scope, date);
                $scope.$apply();
                if ($inp.val() != date) {
                    $inp.val(date);
                }
            }
            else {
                mod.assign($scope, "");
                $scope.$apply();
            }
           validateEndDate($scope);
            
        });
    });

    //using this as default data
    var emptyModel = '{ recurrence: "1", weekInterval: "1", monthYearOption: "1", interval: "1", weekDay: "1", month: "1", monthOption: "1" }';

    if (!angular.isObject($scope.model.value)) {
        $scope.model.value = eval('(' + emptyModel + ')');
    }


    $scope.data = $scope.model.value;

    //Load language-fields from external files
    KSCalendarResource.getLanguagefile().then(function (data) { populateVars(data); });

    $scope.toggleDay = function (id) {
        if (typeof $scope.data.days == 'undefined') {
            $scope.data.days = [];
        }

        var i = $scope.data.days.indexOf(id);
        if (i < 0) {
            $scope.data.days.push(id);
        }
        else {
            $scope.data.days.splice(i, 1);
        }
    };

    $scope.toggleMonth = function (id) {
        if (typeof $scope.data.months == 'undefined') {
            $scope.data.months = [];
        }

        var i = $scope.data.months.indexOf(id);
        if (i < 0) {
            $scope.data.months.push(id);
        }
        else {
            $scope.data.months.splice(i, 1);
        }
    };

    $scope.addExceptDate = function(){
        if (typeof $scope.data.exceptDates == 'undefined') {
            $scope.data.exceptDates = [];
        }
        if($("#dateExcept").val() != "" && !isNaN(new Date($("#dateExcept").val()).getDate())){
            $scope.data.exceptDates.push($("#dateExcept").val());
            $("#dateExcept").val("");
        }
    };

    $scope.removeExceptDate = function(date){
        var i = $scope.data.exceptDates.indexOf(date);
        if (i > -1) {
            $scope.data.exceptDates.splice(i, 1);
        }
    };

    $scope.selectMonthYearOption = function (id) {
        $scope.data.monthYearOption = id;
    };

    $scope.selectMonthOption = function (id) {
        $scope.data.monthOption = id;
    };

    $scope.$watch('data.recurrence', function () {
        validateEndDate($scope);
    });

    $scope.$watch('data.endDate', function () {
        validateEndDate($scope);
    });

    $scope.$watch('data.startDate', function () {
        validateEndDate($scope);
    });

    function populateVars(lang) {
        $scope.language = lang;

        $scope.days = [
            {
                id: '1',
                name: lang.monday
            },
            {
                id: '2',
                name: lang.tuesday
            },
            {
                id: '3',
                name: lang.wednesday
            },
            {
                id: '4',
                name: lang.thursday
            },
            {
                id: "5",
                name: lang.friday
            },
            {
                id: "6",
                name: lang.saturday
            },
            {
                id: "0",
                name: lang.sunday
            }
        ];

        $scope.weeks = [
            {
                id: '1',
                name: lang.single
            },
            {
                id: '2',
                name: lang.second
            },
            {
                id: '3',
                name: lang.third
            },
            {
                id: '4',
                name: lang.fourth
            },
            {
                id: '5',
                name: lang.fifth
            }
        ];

        $scope.months = [
            {
                id: "1",
                name: lang.january
            },
            {
                id: "2",
                name: lang.february
            },
            {
                id: "3",
                name: lang.march
            },
            {
                id: "4",
                name: lang.april
            },
            {
                id: "5",
                name: lang.may
            },
            {
                id: "6",
                name: lang.june
            },
            {
                id: "7",
                name: lang.july
            },
            {
                id: "8",
                name: lang.august
            },
            {
                id: "9",
                name: lang.september
            },
            {
                id: "10",
                name: lang.october
            },
            {
                id: "11",
                name: lang.november
            },
            {
                id: "12",
                name: lang.december
            }
        ];

        $scope.options = [
            {
                id: "1",
                name: lang.none
            },
            {
                id: "2",
                name: lang.daily
            },
            {
                id: "3",
                name: lang.weekly
            },
            {
                id: "4",
                name: lang.monthly
            },
            {
                id: "5",
                name: lang.yearly
            }
        ];

        $scope.intervals = [
            {
                id: '1',
                name: lang.first
            },
            {
                id: '2',
                name: lang.second
            },
            {
                id: '3',
                name: lang.third
            },
            {
                id: '4',
                name: lang.fourth
            },
            {
                id: '5',
                name: lang.fifth
            },
            {
                id: '6',
                name: lang.last
            }
        ];

        $scope.monthYearOptions = [
            {
                id: '1',
                name: lang.useStartDate
            },
            {
                id: '2',
                name: lang.specify
            }
        ];

        $scope.monthOptions = [
            {
                id: '1',
                name: lang.everyMonth
            },
            {
                id: '2',
                name: lang.chooseMonth
            }
        ];
    }



});
    

function validateEndDate($scope) {
    //if (1 < $scope.data.recurrence && ($scope.data.endDate == undefined || $scope.data.endDate == "")) {
    //    $scope.calendarForm.enddate.$setValidity('enddateRequired', false);
    //}
    //else
        if ($scope.data.endDate != "" && $scope.data.endDate != undefined) {
            //if ((convertDateTime($scope.data.endDate).getTime() < convertDateTime($scope.data.startDate).getTime()) || (1 < $scope.data.recurrence) || (isNaN(convertDateTime($scope.data.endDate).getTime()))) {
            if ((convertDateTime($scope.data.endDate).getTime() < convertDateTime($scope.data.startDate).getTime()) || (isNaN(convertDateTime($scope.data.endDate).getTime()))) {
                $scope.calendarForm.enddate.$setValidity('enddateError', false);
            }
        }
        else {
            $scope.calendarForm.enddate.$setValidity('enddateError', true);
        }
    //    $scope.calendarForm.enddate.$setValidity('enddateRequired', true);
    //}
    //else {
    //    $scope.calendarForm.enddate.$setValidity('enddateRequired', true);
    //}
}


function convertDateTime(dt) {
    var dateTime = dt.split(" ");

    var date = dateTime[0].split("-");
    var yyyy = date[0];
    var mm = date[1] - 1;
    var dd = date[2];

    var time = dateTime[1].split(":");
    var h = time[0];
    var m = time[1];
    var s = parseInt(time[2]); //get rid of that 00.0;

    return new Date(yyyy, mm, dd, h, m, s);
}


function convertPickerDateTime(fullDate, time) {
    var date = new Date(fullDate);
    var month = date.getMonth() + 1;
    if (month < 10) {
        month = "0" + month;
    }
    var day = date.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var hours = date.getHours();
    if (hours < 10) {
        hours = "0" + hours;
    }
    var min = date.getMinutes();
    if (min < 10) {
        min = "0" + min;
    }
    if (time) {
        return date.getFullYear() + "-" +
                month + "-" +
                day + " " +
                hours + ":" +
                min + ":00";
    }
    else {
        return date.getFullYear() + "-" +
                month + "-" +
                day;
    }
}