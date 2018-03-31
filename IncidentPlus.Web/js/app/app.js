/// <reference path="../angular.js" />


var app = angular.module('inciden-plus', [
    'ngRoute',
    'LocalStorageModule',
    'angular-loading-bar',
    'ngAnimate',
    'ui.bootstrap',
    'jcs-autoValidate'
]);

app.constant("URLAPI", {
    URL: "http://localhost:8723"
});

app.run(['authService', '$rootScope', '$location', 'bootstrap3ElementModifier', function (authService, $rootScope, $location, bootstrap3ElementModifier) {

    bootstrap3ElementModifier.enableValidationStateIcons(true);

    $rootScope.$on('$locationChangeStart', function (event, next, current) {

        var result = authService.checkSession();
        if (!result) {
            $location.path('/login');

        }


    });
}]);