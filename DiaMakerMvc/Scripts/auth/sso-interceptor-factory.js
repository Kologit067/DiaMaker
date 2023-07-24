(function (angular) {
    'use strict';

    function SsoInterceptorFactory($q, $injector, SsoService) {
        return {
            responseError: function (response) {
                if (response.status === 401
                    || response.status === -1) {
                    var promise = SsoService.refreshToken();
                    return promise.then(function () {
                        // Success
                        // Get $http from the injector instead of as a injected dependency
                        // Otherwise we run into a circular dependency injection problem
                        var $http = $injector.get('$http');
                        // Replay the request
                        return $http(response.config);
                    }, function () {
                        // Failed, probably due to a timeout
                        return $q.reject(response);
                    });
                }
                return $q.reject(response);
            }
        };
    }
    SsoInterceptorFactory.$inject = ['$q', '$injector', 'SsoService'];
    angular.module('LRA.SsoInterceptorService', []).factory('SsoInterceptorService', SsoInterceptorFactory);
})(window.angular);