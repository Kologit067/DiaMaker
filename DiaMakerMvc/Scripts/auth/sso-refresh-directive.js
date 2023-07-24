(function (angular) {
    'use strict';

    /**
     * @ngdoc directive
     * @name bodApp.directive:ssoRefresh
     * @description
     * # header
     */
    function SsoRefreshDirective($rootScope, $window, $timeout,$location) {
   
        return {
            
            restrict: 'EA',
            link: function ($rootScope, $element, $attrs) {
                var iframe;
                var loadCount = 0;
                var startEvent;
                var frameTimeout;
                function redirectToHomePage() {
                    $rootScope.$broadcast('dn:sso-refresh-complete');
                    $window.location.href = '/';
                }
                function isFirefox()
                {
                    return navigator.userAgent.toLowerCase().indexOf('firefox') > -1;
                }
                function isUrlValid() {
                    var iframeContentWindow;
                    var href;
                    var urlValid = true;
                    var extendSessionIframe = $window.frames['extendSessionIframe'];
                    try {
                        iframeContentWindow = extendSessionIframe.contentWindow === undefined ? extendSessionIframe : extendSessionIframe.contentWindow;
                        href = iframeContentWindow.location.href;
                        urlValid = href;
                    } catch (c) {
                        // TODO: Raise an error probably
                    }
                    return urlValid;
                }
               
                function refreshSuccess() {
                    // TODO: Ensure that we are on the correct page before we raise a success.
                    //       If we aren't on the correct page, raise a failure event
                    $element.html('');
                    $timeout.cancel(frameTimeout);
                    $rootScope.$broadcast('dn:sso-refresh-complete');
                }
                function checkRefreshComplete() {
                    loadCount++;
                    if (loadCount === 2) {
                        if (isUrlValid()) {
                            refreshSuccess();
                        }
                    }
                    else if ((loadCount === 1) && isFirefox()) {
                        frameTimeout = $timeout(function () {
                            if (isUrlValid()) {
                                refreshSuccess();
                            }
                        }, 1000*5);
                    }
                }
                startEvent = $rootScope.$on('dn:sso-refresh-start', function () {
                    $rootScope.loading = true;
                         iframe = angular.element('<iframe id="extendSessionIframe" class="hide" src="/Home/ExtendSession"/>');
                          
                        /**
                         * When the element loads, notify the system of the refresh success
                         */
                        iframe.on('load', function (event) {
                            checkRefreshComplete();
                        });
                        $element.append(iframe);                    
                    $(window).off('beforeunload');
                });
                $rootScope.$on('$destroy', function () {
                    // Unbind events
                    startEvent();
                });
                $rootScope.$on('dn:sso-refresh-complete', function () {
                    loadCount = 0;
                });
            }
        };
    }
    SsoRefreshDirective.$inject = ['$rootScope', '$window', '$timeout','$location'];
  
    angular.module('LRA.SsoRefresh', [])
    .directive('ssorefresh', SsoRefreshDirective);
})(window.angular);