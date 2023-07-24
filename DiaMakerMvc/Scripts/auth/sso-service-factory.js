(function (angular) {
    'use strict';
    /**
     * Coordinates with the sso directive to perform a refresh of the authentication cookie
     */

    function SsoService($document, $q, $rootScope, $timeout, $location) {
       
        var deferred;
        var promiseCompleted = false;
        /**
         * Receive events from the sso directive telling us the refresh is complete
         */
        $rootScope.$on('dn:sso-refresh-complete', function () {
            if (deferred !== null) {
                deferred.resolve();
                promiseCompleted = true;
                deferred = null;
                $rootScope.loading = false;
            }
        });
       

        function refreshToken() {
            if (!deferred) {
                promiseCompleted = false;
                // Create a deferred/promise so that other systems can wait on the refresh
                deferred = $q.defer();
                $timeout(function () {
                    if ((deferred !== null) && !promiseCompleted) {
                        deferred.reject();
                        promiseCompleted = true;
                        deferred = null;
                        $(window).off('beforeunload');
                    }
                },
                30 * 1000);
                // Broadcast the need for a refresh. The sso directive will receive this 
                // event and begin a navigation to refresh the cookie
                
                $rootScope.$broadcast('dn:sso-refresh-start');
            }
            return deferred.promise;
        }
        return {
            refreshToken: refreshToken
        };
    }
    SsoService.$inject = ['$document', '$q', '$rootScope', '$timeout', '$location'];


    angular.module('LRA.SsoService', [])
    .factory('SsoService', SsoService);
})(window.angular);