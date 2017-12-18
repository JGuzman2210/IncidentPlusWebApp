/// <reference path="../angular.js" />

app.config(["$routeProvider", 'localStorageServiceProvider', '$httpProvider',
function ($routeProvider, localStorageServiceProvider,$httpProvider) {

    $routeProvider
        .when('/', {
            templateUrl: 'Views/home.html',
            controller: 'HomeCtrl'
        })
        .when('/incident', {
            templateUrl: 'Views/incident.html',
            controller: 'IncidentCtrl'
        })
        .when('/history', {
            templateUrl: 'Views/history.html',
            controller: 'HistoryCtrl'
        })
        .when('/login/:error?', {
            templateUrl: 'Views/login.html',
            controller: 'LoginCtrl'
        })
        .when('/projects', {
            templateUrl: 'Views/projects.html',
            controller:'ProjectsCtrl'
        })
        .when('/users', {
            templateUrl: 'Views/users.html',
            controller:'UsersCtrl'
        })
        .when('/errorcode/:codeError', {
            templateUrl: 'Views/errorcode.html',
            controller:'ErrorCodeCtrl'
        })
        .otherwise({
            redirectTo: '/'
        });
    localStorageServiceProvider.setPrefix('authapp');
    $httpProvider.interceptors.push('httpInterceptor')
}]);