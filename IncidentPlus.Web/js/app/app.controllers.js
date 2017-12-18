app
    .controller("IndexCtrl", ['$scope', 'authService', '$location', function($scope, authService, $location) {

        $scope.setActive = function(option) {
            $scope.home = '';
            $scope.incident = '';
            $scope.history = '';
            $scope.projects = '';
            $scope.users = '';

            $scope[option] = 'active';
        }

        $scope.authentication = authService.authUser;

        $scope.logout = function() {
            authService.logout();
            $location.path('/login');
            
        }
    }])
    .controller("HomeCtrl", ['$scope', function($scope) {
        $scope.setActive('home');
    }])
    .controller("IncidentCtrl", ['$scope', function($scope) {
        $scope.setActive('incident');
    }])
    .controller("HistoryCtrl", ['$scope', 'authService', function($scope) {
        $scope.setActive('history');

    }])
    .controller('LoginCtrl', ['$scope', 'authService', '$location', 'cfpLoadingBar', function($scope, authService, $location, cfpLoadingBar) {

        if (authService.authUser.isAuth)
            return $location.path('/')

        $scope.displayMessage = {}

        $scope.start = function() {
            cfpLoadingBar.start();
        };

        $scope.complete = function() {
            cfpLoadingBar.complete();
        }


        $scope.login = function() {
            $scope.start();
            authService.login($scope.login).then(function(respose) {
                SetDisplayMessage(true, 'Usuario Logeado correctamente:', 'Bienvenido a IncidenPlus, Le estamos redireccionando a la pagina principal', 'alert-success')

                setTimeout(function() {
                    $scope.complete();
                    window.location.href = '/';
                }, 1000)

            }, function(error) {
                var data = error.data;
                SetDisplayMessage(true, 'Credenciales invalida:', 'Por favor ingrese un Usuario y una Clave validad', 'alert-danger')
            })

        }

        function SetDisplayMessage(state, message, description, classes) {
            $scope.displayMessage.state = state;
            $scope.displayMessage.message = message;
            $scope.displayMessage.description = description;
            $scope.displayMessage.class = classes
        }

    }])
    .controller('ProjectsCtrl', ['$scope', 'projectService', '$location', '$uibModal', '$route', function($scope, projectService, $location, $uibModal, $route) {
        $scope.setActive('projects');
        $scope.errorMessage = {};
        $scope.projects = {};
        /*
            var getAllProjects = function () {
                projectService.getAllProjects().then(function (success) {
                    $scope.projects = success.data;

                }, function (error) {
                    $scope.projects = [];
                    $location.path('/errorcode/' + error.status);
                });
            }*/
        var getAllProjects =  function() {
             projectService.getAllProjects()
                .then((response) => {
                    $scope.projects = response.data;
                }).catch((error) => console.log(error));


        }
        var getAllCategoriesByProjectId = function(projectID) {
            projectService.getAllCategoriesByProjectId(projectID).then(function(success) {
                $scope.currentProject.categories = success.data;
            }, function(error) {
                setError(error, true)
            });
        }
        var getAllLevelsByProjectId = function(projectID) {
            projectService.getAllLevelsByProjectId(projectID).then(function(success) {
                $scope.currentProject.levels = success.data;
            }, function(error) { setError(error, true) })
        }
        var setError = function(message, status) {
            $scope.errorMessage = {
                error: message,
                isError: status
            }
        }


        $scope.displayFeatures = function(currentProject) {

                var projectID = currentProject.Id;

                $scope.indexRow = projectID;
                $scope.currentProject = currentProject;

                getAllCategoriesByProjectId(projectID);
                getAllLevelsByProjectId(projectID)

            }
            //For Aviable Project by ID
        $scope.processEnableProject = function(projectID) {
                projectService.enableProject(projectID).then(function(success) {
                    getAllProjects();
                    $route.reload();
                }, function(error) {
                    console.log('Error');
                    console.log(error)
                })
            }
            //For Deleting project by ID
        $scope.processDisableProject = function(projectID) {
                var result = confirm('Esta seguro de deshabilitar el projecto selecciondo?');
                if (result) {
                    projectService.deleteProject(projectID).then(function(success) {
                        getAllProjects();
                        $route.reload();
                    }, function(error) {
                        console.log('Error');
                    })
                }
            }
            //For Editing a project by ID
        $scope.processEditProject = function(projectID) {
                var modal = $uibModal.open({
                    templateUrl: 'modalProjectForm.html',
                    controller: 'ProcessProjectFormModal',
                    resolve: {
                        projectId: function() { return projectID }
                    }
                });
                modal.result.then(function() { //Execute when press ok button
                    getAllProjects();

                }, function(data) { //Execute when press cancel button

                })
            }
            //For creating a new Project
        $scope.openProjectForm = function() {
                var modal = $uibModal.open({
                    templateUrl: 'modalProjectForm.html',
                    controller: 'ProcessProjectFormModal',
                    resolve: {
                        projectId: function() { return undefined }
                    }
                });
                modal.result.then(function() { //Execute when press ok button
                    getAllProjects();

                }, function(data) { //Execute when press cancel button

                })
            }
            //For creating a new Category by X project
        $scope.openCategoryForm = function() {
            var modal = $uibModal.open({
                templateUrl: 'modalCategoryForm.html',
                controller: 'ProcessCategoryFormModal',
                resolve: {
                    currentProjectID: function() {
                        return $scope.currentProject.Id || null;
                    }
                }
            });
            modal.result.then(function() { //Execute when press ok button
                getAllCategoriesByProjectId($scope.currentProject.Id);

            }, function(data) { //Execute when press cancel button

            })
        }

        getAllProjects();

    }])
    .controller('UsersCtrl', ['$scope', function($scope) {
        $scope.setActive('users');
    }])
    .controller('ErrorCodeCtrl', ['$scope', '$routeParams', function($scope, $routeParams) {
        var codeError = $routeParams.codeError;
        $scope.message = '';
        console.log('VALOR ' + codeError)
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
    .controller('ProcessProjectFormModal', ['$scope', '$uibModalInstance', 'projectService', 'projectId',
        function($scope, $uibModalInstance, projectService, projectId) {
            $scope.project = {};
            var setBtnStatus = function(state) {
                $scope.btnStatus = state;
            }

            var getProjectById = function(id) {
                projectService.getProjectById(id).then(function(success) {
                    $scope.project = {
                        name: success.data.Name,
                        id: success.data.Id,
                        description: success.data.Description
                    }
                }, function(error) {
                    alert(error);
                })
            }

            if (projectId != undefined && projectId > 0) {
                getProjectById(projectId);
            }


            $scope.close = function() {
                $uibModalInstance.dismiss('cancel');
            }

            $scope.processProjectForm = function() {
                setBtnStatus(true)
                $scope.project.State = 1;

                if (projectId != undefined && projectId > 0) {
                    projectService.updateProject($scope.project).then(function(success) {

                        setBtnStatus(false)
                        $uibModalInstance.close();
                    }, function(error) {
                        console.log('error', error)
                        setBtnStatus(false)
                    });
                } else {
                    projectService.addProject($scope.project).then(function(success) {
                        if (success.status == 200) {
                            alert('Projecto creado')
                            $uibModalInstance.close();
                        }
                        setBtnStatus(false)
                    }, function(error) {
                        setBtnStatus(false)
                        alert(error.data.Message);
                    });
                }
            }
        }
    ])
    .controller('ProcessCategoryFormModal', ['$scope', '$uibModalInstance', 'categoryService', 'currentProjectID',
        function($scope, $uibModalInstance, categoryService, currentProjectID) {
            $scope.category = {};

            var setBtnS,tatus = function(state) {
                $scope.btnStatus = state;
            }

            $scope.processCategoryForm = function() {
                setBtnStatus(true)
                $scope.category.State = 1;
                $scope.category.ProjectID = currentProjectID;
                categoryService.addCategory($scope.category).then(function(success) {
                    if (success.status == 200) {
                        alert('Categoría creada satisfactoriamente')
                        $uibModalInstance.close();
                    }
                    setBtnStatus(false)
                }, function(error) {
                    setBtnStatus(false)
                    alert(error.data.Message);
                });
            }

            $scope.close = function() {
                $uibModalInstance.dismiss('cancel');
            }
        }
    ])