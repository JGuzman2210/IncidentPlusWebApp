app
    .factory('authService', ['$http', '$q', 'URLAPI', 'localStorageService', function($http, $q, URLAPI, localStorageService) {
        var _authUser = {}

        var _login = function(userLogin) {
            var data = `grant_type=password&username=${userLogin.userName}&password=${userLogin.password}`;
            var deferred = $q.defer();
            $http.post(URLAPI.URL + '/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                .then(function(successRespose) {
                    _logOut();
                    var result = successRespose.data;
                    var dataToken = {
                        token: result.access_token,
                        tokenType: result.token_type,
                        tokenExpires: result.expires_in,
                        userName: result.userName,
                        name: result.name,
                        email: result.email,
                        role: result.role
                    };
                    localStorageService.set('userAuthorization', dataToken);

                    fillAuthUser(result)

                    deferred.resolve(result);
                }, function(errorResponse) {

                    deferred.reject(errorResponse)
                })

            return deferred.promise;
        }

        var _logOut = function() {
            localStorageService.remove("userAuthorization");
            fillAuthUser(undefined);
        }

        var _checkSession = function() {

            var result = localStorageService.get('userAuthorization');
            if (result) {
                fillAuthUser(result)
                return true;
            } else {
                fillAuthUser(undefined);
                return false;
            }
        }

        function fillAuthUser(currentUser) {
            if (currentUser == undefined) {
                _authUser = {};

            } else {
                _authUser.isAuth = true;
                _authUser.userName = currentUser.userName || '';
                _authUser.role = currentUser.role || '';
                _authUser.name = currentUser.name || '';
                _authUser.email = currentUser.email || '';
            }
        }
        return {
            authUser: _authUser,
            login: _login,
            logout: _logOut,
            checkSession: _checkSession
        }
    }])
    .factory('projectService', ['$http', '$q', 'URLAPI', function($http, $q, URLAPI) {

        var _getAllProjects = function() {
            return $http.get(URLAPI.URL + '/api/projects');
        }
        var _getAllCategoriesByProjectId = function(id) {
            return $http.get(URLAPI.URL + '/api/project/' + id + '/categories')
        }
        var _getAllLevelsByProjectId = function(id) {
            return $http.get(URLAPI.URL + '/api/project/' + id + '/levels')
        }
        var _addProject = function(project) {
            return $http({
                url: URLAPI.URL + '/api/project',
                method: 'post',
                data: project
            });

        }
        var _getProjectById = function(projectId) {
            return $http({
                url: URLAPI.URL + '/api/project/' + projectId,
                method: 'get'
            });
        }
        var _updateProject = function(projectUpdated) {
            return $http({
                url: URLAPI.URL + '/api/project',
                headers: {
                    'Content-Type': 'application/json'
                },
                method: 'put',
                data: projectUpdated
            })
        }
        var _deleteProject = function(projectId) {
            return $http({
                url: URLAPI.URL + '/api/project/' + projectId,
                method: 'delete'
            })
        }
        var _enableProject = function(projectId) {
            return $http({
                url: URLAPI.URL + '/api/project/' + projectId + '/enable',
                method: 'get'
            })
        }

        return {
            getAllProjects: _getAllProjects,
            getAllCategoriesByProjectId: _getAllCategoriesByProjectId,
            getAllLevelsByProjectId: _getAllLevelsByProjectId,
            addProject: _addProject,
            getProjectById: _getProjectById,
            updateProject: _updateProject,
            deleteProject: _deleteProject,
            enableProject: _enableProject
        }
    }])
    .factory('categoryService', ['$http', '$q', 'URLAPI', function($http, $q, URLAPI) {
        var _addCategory = function(category) {
            return $http({
                url: URLAPI.URL + '/api/category',
                method: 'post',
                data: category
            });
        }

        return {
            addCategory: _addCategory
        }
    }])
    .factory('httpInterceptor', ['$location', '$q', 'localStorageService', function($location, $q, localStorageService) {


        var _request = function(config) {
            var authToken = localStorageService.get('userAuthorization');


            if (authToken) {
                config.headers = config.headers || {};
                config.headers.Authorization = 'bearer ' + authToken.token
            }
            return config;
        }

        var _responseError = function(responseError) {
            if (responseError.status === 401 || responseError.status === 403) {
                $location.path('/login')
            }
            return $q.reject(responseError);
        }

        var _response = function(response) {
            return response;
        }
        return {
            request: _request,
            response: _response,
            responseError: _responseError
        }
    }])