angular.module("umbraco").controller("KS.CalendarController", function ($scope, assetsService, KSCalendarResource) {

    assetsService.loadCss("/app_plugins/KS.Umbraco7.Calendar/css/bootstrap-datetimepicker.min.css");
    assetsService.load([
       "/app_plugins/KS.Umbraco7.Calendar/js/bootstrap-datetimepicker.min.js"
    ])
    .then(function () {
        $("#StartDateWrapper").datetimepicker();
        $("#EndDateWrapper").datetimepicker();
       
        $("#StartDateWrapper").on('changeDate', function () {
            $scope.data.startDate = $("#dtStartDate").val();
        });
        $("#EndDateWrapper").on('changeDate', function () {
            $scope.data.endDate = $("#dtEndDate").val();
        });

        $("#StartDateWrapper").on("change", "input", function () {
            $scope.data.startDate = $("#dtStartDate").val();
        });
        $("#EndDateWrapper").on("change", "input", function () {
            $scope.data.endDate = $("#dtEndDate").val();
        });
    });   

    //using this as default data
    var emptyModel = '{ recurrence: "1", weekInterval: "1", monthYearOption: "1", interval: "1", weekDay: "1", month: "1" }';

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

    $scope.selectMonthYearOption = function (id) {
        $scope.data.monthYearOption = id;
    };
    

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
                id: "7",
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
    }



}).directive('enddate', function () {
    //this directive is used for handling the enddate validation
    return {
        require: 'ngModel',
        link: function ($scope, elm, attrs, ctrl) {
            //we hook into the $formatters pipeline to get a chance to validate the enddate
            ctrl.$formatters.unshift(function (viewValue) {
                if ($scope.data.endDate != "" && new Date($scope.data.endDate).getTime() < new Date($scope.data.startDate).getTime()) {
                    $scope.data.endDate = $scope.data.startDate;
                    $("#dtEndDate").val($scope.data.endDate);
                    return $scope.data.startDate;
                }
                else {
                    //ctrl.$setValidity('enddateError', true);
                    return viewValue;
                }
            });
        }
    };
});