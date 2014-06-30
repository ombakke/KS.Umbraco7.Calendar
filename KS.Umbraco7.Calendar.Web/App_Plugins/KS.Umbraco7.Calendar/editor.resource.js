angular.module("umbraco.resources").factory("KSCalendarResource", function ($http, $q, userService) {
    var service = {};
    
    service.getLanguagefile = function () {
        var deferred = $q.defer();
        userService.getCurrentUser().then(function (user) {
            //loading language file based on logged in users selected language
            $http.get("/app_plugins/KS.Umbraco7.Calendar/language/" + user.locale + ".js")
                .then(function (data) {
                        return deferred.resolve(data.data);
                    },
                    function () {
                        //using en-GB as fallback
                        $http.get("/app_plugins/KS.Umbraco7.Calendar/language/en-GB.js")
                            .then(function (d) {
                                return deferred.resolve(d.data);
                            }, function(err){
                                return deferred.reject("Couldn't load language file "  + user.local + ".js");
                            });
                });   
            });
        return deferred.promise;
    };

    return service;

});