angular.module("umbraco").controller("KS.CalendarController", function ($scope, assetsService) {

    assetsService.loadCss("/app_plugins/KS.Umbraco7.Calendar/css/bootstrap-datetimepicker.min.css");
    assetsService.load([
       "/app_plugins/KS.Umbraco7.Calendar/js/bootstrap-datetimepicker.min.js"
    ])
    .then(function () {
        
        $("#StartDateWrapper").datetimepicker()
                .on("changeDate", function (ev) {
                    $scope.data.startDate = $("#dtStartDate").val();
                });
        $("#EndDateWrapper").datetimepicker()
                .on("changeDate", function (ev) {
                    $scope.data.endDate = $("#dtEndDate").val();
                });
    });



    var emptyModel = '{ recursive: "1", weekInterval: "1", monthYearOption: "1", interval: "1", weekDay: "1", month: "1" }';//using this as default data

    if (!angular.isObject($scope.model.value)) {
        $scope.model.value = eval('(' + emptyModel + ')');
    }
    

    $scope.data = $scope.model.value;


    $scope.toggleDay = function (id) {
        if (typeof $scope.data.days == 'undefined') {
            $scope.data.days = [];
        }

        var i = $scope.data.days.indexOf(id);
        //console.log("IndexOf " + id + " - " + i);
        if (i < 0) {
            $scope.data.days.push(id);
        }
        else {
            //console.log("Heidu!");
            $scope.data.days.splice(i, 1);
        }

    };

    $scope.selectMonthYearOption = function (id) {
        $scope.data.monthYearOption = id;
    };
    
    $scope.days = [
        {
            id: '1',
            name: "Mandag"
        },
        {
            id: '2',
            name: "Tirsdag"
        },
        {
            id: '3',
            name: "Onsdag"
        },
        {
            id: '4',
            name: "Torsdag"
        },
        {
            id: "5",
            name: "Fredag"
        },
        {
            id: "6",
            name: "Lørdag"
        },
        {
            id: "7",
            name: "Søndag"
        }
    ];

    $scope.weeks = [
        {
            id: '1',
            name: 'eneste'
        },
        {
            id: '2',
            name: '2.'
        },
        {
            id: '3',
            name: '3.'
        },
        {
            id: '4',
            name: '4.'
        },
        {
            id: '5',
            name: '5.'
        }
    ];

    $scope.months = [
        {
            id: "1",
            name: "Januar"
        },
        {
            id: "2",
            name: "Februar"
        },
        {
            id: "3",
            name: "Mars"
        },
        {
            id: "4",
            name: "April"
        },
        {
            id: "5",
            name: "Mai"
        },
        {
            id: "6",
            name: "Juni"
        },
        {
            id: "7",
            name: "Juli"
        },
        {
            id: "8",
            name: "August"
        },
        {
            id: "9",
            name: "September"
        },
        {
            id: "10",
            name: "Oktober"
        },
        {
            id: "11",
            name: "November"
        },
        {
            id: "12",
            name: "Desember"
        }
    ];

    $scope.options = [
        {
            id: "1",
            name: "Ingen"
        },
        {
            id: "2",
            name: "Daglig"
        },
        {
            id: "3",
            name: "Ukentlig"
        },
        {
            id: "4",
            name: "Månedlig"
        },
        {
            id: "5",
            name: "Årlig"
        }
    ];

    $scope.intervals = [
        {
            id: '1',
            name: 'Første'
        },
        {
            id: '2',
            name: 'Andre'
        },
        {
            id: '3',
            name: 'Tredje'
        },
        {
            id: '4',
            name: 'Fjerde'
        },
        {
            id: '5',
            name: 'Femte'
        },
        {
            id: '6',
            name: 'Siste'
        }
    ];

    $scope.weekDays = [
        {
            id: '1',
            name: 'Mandag'
        },
        {
            id: '2',
            name: 'Tirsdag'
        },
        {
            id: '3',
            name: 'Onsdag'
        },
        {
            id: '4',
            name: 'Torsdag'
        },
        {
            id: '5',
            name: 'Fredag'
        },
        {
            id: '6',
            name: 'Lørdag'
        },
        {
            id: '0',
            name: 'Søndag'
        }
    ];

    $scope.monthYearOptions = [
        {
            id: '1',
            name: 'Bruk startdato'
        },
        {
            id: '2',
            name: 'Spesifiser'
        }
    ];
});