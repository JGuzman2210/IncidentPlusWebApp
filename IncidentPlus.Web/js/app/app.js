/// <reference path="../angular.js" />


var app = angular.module('inciden-plus', [
    'ngRoute',
    'LocalStorageModule',
    'angular-loading-bar',
    'ngAnimate',
    'ui.bootstrap'
]);

app.constant("URLAPI", {
    URL: "http://localhost:8723"
});

app.run(['authService', '$rootScope', '$location', function(authService, $rootScope, $location) {

    $rootScope.$on('$locationChangeStart', function(event, next, current) {

        var result = authService.checkSession();
        if (!result) {
            $location.path('/login');

        }


    });
}]);