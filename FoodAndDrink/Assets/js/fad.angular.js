/****** MODULE DECLARATIONS ******/

angular.module('fad.common', []);
angular.module('fad.resources', []);
angular.module('fad.filters', []);
angular.module('fad.directives', []);

/****** MODULE IMPLEMENTATIONS ******/

// COMMON
angular.module('fad.common').factory('fadConst', [function () {
    return {
        urls: {
            chef: '/umbraco/api/chef/',
        },
    };
}]);

angular.module('fad.common').factory('fadUtils', [
    '$timeout',
    function ($timeout) {
        var getParameterByName = function (name, url) {
            if (!url) {
                url = window.location.href;
            }
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        };

        return {
            getQueryString: getParameterByName,
            plainEquals: function (obj1, obj2) {
                return JSON.stringify(obj1) === JSON.stringify(obj2);
            }            
        };
    }]
);

// RESOURCES / AJAX
angular.module('fad.resources').factory('resourcesHelper',
    function ($http) {

        var notificationService;

        var handleException = function (response) {
            //notificationService.error('<strong>' + response.status + ' - ' + response.statusText + '</strong><hr />' + 'There was an error while processing your request.');
            // TODO: Send to server logging?
            console.log(angular.toJson(response.data));
        };

        var handleApiResult = function (response, callback) {
            if (response.Message) {
                //if (response.Success) {
                //    notificationService.success(response.Message);
                //}
                //else {
                //    notificationService.error(response.Message);
                //}
                console.log(response.Message);
            }

            if (typeof (callback) === 'function') {
                callback(response.Data);
            }
        };

        //the factory object returned
        return {
            get: function (url, callback, notification) {
                //notificationService = notification || toastr;

                $http.get(url).then(
                    function (response) {
                        handleApiResult(response.data, callback);
                    }, handleException
                );
            },
            post: function (url, data, callback, notification) {
                //notificationService = notification || toastr;

                $http.post(url, data).then(
                    function (response) {
                        handleApiResult(response.data, callback);
                    }, handleException
                );
            }
        };
    }
);

// FILTERS

angular.module('fad.filters').filter('decimal', [function () {
    return function (input) {
        return (!(input) ? 0.00 : parseFloat(input, 10)).toFixed(2);
    };
}]);

// DIRECTIVES

angular.module('fad.directives').directive('preventDefault', [function () {
    return function (scope, element, attrs) {
        $(element).click(function (event) {
            event.preventDefault();
        });
    };
}]);

