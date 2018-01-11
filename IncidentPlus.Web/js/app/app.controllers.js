app
    .controller("IndexCtrl", ['$scope', 'authService', '$location', '$log',
        function ($scope, authService, $location, $log) {

            $scope.setActive = function (option) {
                $scope.home = '';
                $scope.incident = '';
                $scope.history = '';
                $scope.projects = '';
                $scope.users = '';

                $scope[option] = 'active';
            }

            $scope.authentication = authService.authUser;

            $scope.logout = function () {
                authService.logout();
                window.location.reload();
            }
        }])
    .controller("HomeCtrl", ['$scope', '$log', function ($scope, $log) {
        $scope.setActive('home');
    }])
    .controller("IncidentCtrl", ['$scope', '$log', function ($scope, $log) {
        $scope.setActive('incident');
    }])
    .controller("HistoryCtrl", ['$scope', 'authService', '$log', function ($scope, authService, $log) {
        $scope.setActive('history');

    }])
    .controller('LoginCtrl', ['$scope', 'authService', '$location', 'cfpLoadingBar', '$log', function ($scope, authService, $location, cfpLoadingBar, $log) {

        if (authService.authUser.isAuth)
            $location.path('/')

        $scope.displayMessage = {}

        $scope.start = function () {
            cfpLoadingBar.start();
        };

        $scope.complete = function () {
            cfpLoadingBar.complete();
        }

        $scope.login = function () {
            $scope.start();
            authService.login($scope.login).then(function (respose) {
                SetDisplayMessage(true, 'Usuario Logeado correctamente:', 'Bienvenido a IncidenPlus, Le estamos redireccionando a la pagina principal.', 'alert-success');

                setTimeout(function () {
                    $scope.complete();
                    window.location.href = '/';
                }, 1000);
            }).catch((error) => {
                $log.info("Obteniendo datos del catch", error)
                if (error.data != null) {
                    var data = error.data;
                    $log.error(`Error: ${data.error} - ${data.error_description}`);
                    SetDisplayMessage(true, 'Credenciales invalida:', 'Por favor ingrese un Usuario y una Clave valida.', 'alert-danger');
                }else {
                    SetDisplayMessage(true,'Conexión al servidor:','No se pudo conectar con el servidor, vuelva a intentarlo más tarde.','alert alert-warning')
                }
            });
        }

        function SetDisplayMessage(state, message, description, classes) {
            $scope.displayMessage.state = state;
            $scope.displayMessage.message = message;
            $scope.displayMessage.description = description;
            $scope.displayMessage.class = classes;
        }

    }])
    .controller('ProjectsCtrl', ['$scope', 'projectService', '$location', '$uibModal', '$route', '$log',
                        function ($scope, projectService, $location, $uibModal, $route, $log) {
        $scope.setActive('projects');
        $scope.errorMessage = {};
        $scope.projects = {};

        var getAllProjects = function () {
            projectService.getAllProjects()
            .then((response) => {
                $scope.projects = response.data;
            }).catch((error) => $log.error('Error: ', error));
        }

        var getAllCategoriesByProjectId = function (projectID) {
            projectService.getAllCategoriesByProjectId(projectID).then(function (success) {
                $scope.currentProject.categories = success.data;
            }).catch((error) => $log.error('Error: ', error));;
        }

        var getAllLevelsByProjectId = function (projectID) {
            projectService.getAllLevelsByProjectId(projectID).then(function (success) {
                $scope.currentProject.levels = success.data;
            }).catch((error) =>  { setError(error, true) });
        }

        var setError = function (message, status) {
            $scope.errorMessage = {
                error: message,
                isError: status
            }
        }

        //Displaying All Categories and Levels When Selecting any project
        $scope.displayFeatures = function (currentProject) {

            var projectID = currentProject.Id;

            $scope.indexRow = projectID;
            $scope.currentProject = currentProject;

            getAllCategoriesByProjectId(projectID);
            getAllLevelsByProjectId(projectID)

        }
        //For Aviable Project by ID
        $scope.processEnableProject = function (projectID) {
            projectService.enableProject(projectID).then(function (success) {
                getAllProjects();
                $route.reload();
            }).catch((error) => $log.error('Error: ', error));
        }
        //For Deleting project by ID
        $scope.processDisableProject = function (projectID) {
            var result = confirm('Esta seguro de deshabilitar el projecto selecciondo?');
            if (result) {
                projectService.deleteProject(projectID).then(function (success) {
                    getAllProjects();
                    $route.reload();
                }).catch((error) => $log.error('Error: ', error));
            }
        }
        //For Editing a project by ID
        $scope.processEditProject = function (projectID) {
            var modal = $uibModal.open({
                templateUrl: 'modalProjectForm.html',
                controller: 'ProcessProjectFormModal',
                resolve: {
                    projectId: function () { return projectID }
                }
            });
            modal.result.then(function () { //Execute when press ok button
                getAllProjects();

            }, function (data) { //Execute when press cancel button

            }).catch((error) => $log.error('Error: ', error));
        }
        //For creating a new Project
        $scope.openProjectForm = function () {
            var modal = $uibModal.open({
                templateUrl: 'modalProjectForm.html',
                controller: 'ProcessProjectFormModal',
                resolve: {
                    projectId: function () { return undefined }
                }
            });
            modal.result.then(function () { //Execute when press ok button
                getAllProjects();

            }, function (data) { //Execute when press cancel button
            })
        }
        //For creating a new Category by selecting project
        $scope.openCategoryForm = function () {
            var modal = $uibModal.open({
                templateUrl: 'modalCategoryForm.html',
                controller: 'ProcessCategoryFormModal',
                resolve: {
                    currentProjectID: function () {
                        return $scope.currentProject.Id || null;
                    }
                }
            });
            modal.result.then(function () { //Execute when press ok button
                getAllCategoriesByProjectId($scope.currentProject.Id);

            }, function (data) { //Execute when press cancel button

            }).catch((error) => $log.error('Error: ', error));
        }
        //For creating a new Level by selecting project
        $scope.openLevelForm = function () {
            var modal = $uibModal.open({
                templateUrl: 'modalLevelForm.html',
                controller: 'ProcessLevelFormModal',
                resolve: {
                    currentProjectID: function () {
                        return $scope.currentProject.Id || null;
                    }
                }
            });
            modal.result.then(function () { //Execute when press ok button
                getAllLevelsByProjectId($scope.currentProject.Id);

            }, function (data) { //Execute when press cancel button

            }).catch((error) => $log.error('Error: ', error));
        }
     
        getAllProjects();

    }])
    .controller('UsersCtrl', ['$scope', '$uibModal', 'userService', 'rolService', '$log', function ($scope,$uibModal, userService, rolService, $log) {
        $scope.setActive('users');

        userService.getAllUsers().then((successData) => {
            $scope.userList = successData.data;
            rolService.getAllRoles().then((successDataRol) => {
                $scope.roles = successDataRol.data;
            })
        }).catch((errorResponse) => {
            $log.error('Error', errorResponse);
        })

        $scope.openNewUserForm = function () {
            var modal = $uibModal.open({
                templateUrl: 'modalNewUserForm.html',
                controller: 'ProcessUserFormModal',
            });
            modal.result.then(function () { //Execute when press ok button
             

            }, function (data) { //Execute when press cancel button

            }).catch((error) => $log.error('Error: ', error));
        }
    }])
    .controller('ErrorCodeCtrl', ['$scope', '$routeParams', '$log', function ($scope, $routeParams, $log) {
        var codeError = $routeParams.codeError;
        switch (codeError) {
            case '401':
                $scope.message = "Usted, no tienes permisos para accesar al recurso solicitado";
                break;

            case '404':
                $scope.message = "Recurso no encontrado";
                break;
            default:

                break;
        }
    }])
    .controller('ProcessProjectFormModal', ['$scope', '$uibModalInstance', 'projectService', 'projectId', '$log',
        function ($scope, $uibModalInstance, projectService, projectId, $log) {
            $scope.project = {};
            var setBtnStatus = function (state) {
                $scope.btnStatus = state;
            }

            var getProjectById = function (id) {
                projectService.getProjectById(id).then(function (success) {
                    $scope.project = {
                        name: success.data.Name,
                        id: success.data.Id,
                        description: success.data.Description
                    }
                }, function (error) {
                    alert(error);
                })
            }

            if (projectId != undefined && projectId > 0) {
                getProjectById(projectId);
            }


            $scope.close = function () {
                $uibModalInstance.dismiss('cancel');
            }

            $scope.processProjectForm = function () {
                setBtnStatus(true)
                $scope.project.State = 1;

                if (projectId != undefined && projectId > 0) {
                    projectService.updateProject($scope.project).then(function (success) {

                        setBtnStatus(false)
                        $uibModalInstance.close();
                    }, function (error) {
                        console.log('error', error)
                        setBtnStatus(false)
                    });
                } else {
                    projectService.addProject($scope.project).then(function (success) {
                        if (success.status == 200) {
                            alert('Projecto creado')
                            $uibModalInstance.close();
                        }
                        setBtnStatus(false)
                    }, function (error) {
                        setBtnStatus(false)
                        alert(error.data.Message);
                    });
                }
            }
        }
    ])
    .controller('ProcessCategoryFormModal', ['$scope', '$uibModalInstance', 'categoryService', 'currentProjectID', '$log',
        function ($scope, $uibModalInstance, categoryService, currentProjectID, $log) {
            $scope.category = {};

            $scope.processCategoryForm = function () {
                $scope.category.State = 1;
                $scope.category.ProjectID = currentProjectID;
                categoryService.addCategory($scope.category).then(function (success) {
                    if (success.status == 200) {
                        alert('Categoría creada satisfactoriamente')
                        $uibModalInstance.close();
                    }
                }, function (error) {
                    alert(error.data.Message);
                });
            }

            $scope.close = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }
    ])
    .controller('ProcessLevelFormModal', ['$scope', '$uibModalInstance', 'levelService', 'currentProjectID', '$log',
        function ($scope, $uibModalInstance, levelService, currentProjectID, $log) {
            $scope.level = {};
            

            $scope.processLevelForm = function () {

                $scope.level.State = 1;
                $scope.level.ProjectID = currentProjectID;
                levelService.addLevel($scope.level).then(function (success) {
                    if (success.status == 200) {
                        alert('Level creado satisfactoriamente')
                        $uibModalInstance.close();
                    }
                }).catch((error) => {
                    if (error.data != null) {
                        alert(error.data.Message)
                    }
                    $log.error(error);
                });
            }

            $scope.close = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }
    ])
    .controller('ProcessUserFormModal', ['$scope', '$uibModalInstance', '$log', 'rolService', function ($scope, $uibModalInstance, $log, rolService) {

        rolService.getAllRoles().then((successDataRol) => {
            $scope.roles = successDataRol.data;
        })


        $scope.close = function () {
            $uibModalInstance.dismiss('cancel');
        }

    }])