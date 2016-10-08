var app = angular.module('scheduler', ['ngRoute']);

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/help', {
        templateUrl: 'spa/tmpl/help.html',
        controller: 'helpController'
    }).when('/jobs', {
        templateUrl: 'spa/tmpl/jobs.html',
        controller: 'jobsController'
    }).when('/new', {
        templateUrl: 'spa/tmpl/new.html',
        controller: 'newController'
    }).otherwise({
        redirectTo: "/jobs"
    });
}]);

app.factory('apiRepository', ['$http', function ($http) {
    var apiRepo = {};

    apiRepo.getJobs = function (pageIndex, pageSize) {
        return $http.get('/jobs/' + pageIndex + '/' + pageSize);
    };

    apiRepo.getCron = function () {
        return $http.get('/cron');
    };

    apiRepo.getSymbol = function () {
        return $http.get('/symbol');
    }

    apiRepo.updateJob = function (jobId, cron) {
        return $http.put('/update', { jobId: jobId, cron: cron });
    };
    apiRepo.addJob = function (job) {
        return $http.post('/jobs', job);
    };
    return apiRepo;
}]);

app.service('apiService', ['$http', function ($http) {
    this.getJobs = function (pageIndex, pageSize) {
        return $http.get('/jobs/' + pageIndex + '/' + pageSize);
    };

    this.getCron = function () {
        return $http.get('/cron');
    };

    this.getSymbol = function () {
        return $http.get('/symbol');
    }

    this.updateJob = function (jobId, cron) {
        return $http.put('/update', { jobId: jobId, cron: cron });
    };
    this.addJob = function (job) {
        return $http.post('/jobs', job);
    }
}]);

app.controller('helpController', ['$scope', 'apiRepository', function ($scope, apiRepository) {
    $scope.status = true;
    $scope.crons = {};
    $scope.symbols = {};

    initialize();

    function initialize() {
        apiRepository.getCron().success(function (data) {
            $scope.crons = data;
        }).error(function (error) {
            $scope.status = 'Unable to load help data: ' + error.message;
        });

        apiRepository.getSymbol().success(function (data) {
            $scope.symbols = data;
        }).error(function (error) {
            $scope.status = 'Unable to load help data: ' + error.message;
        });
    }
}]);

app.controller('jobsController', ['$scope', 'apiRepository', function ($scope, apiRepository) {
    $scope.status = true;
    $scope.jobs = {};
    $scope.total = 0;
    $scope.pageIndex = 0;
    $scope.pageSize = 10;

    $scope.getJobs = function () {
        apiRepository.getJobs($scope.pageIndex, $scope.pageSize).success(function (data) {
            $scope.jobs = data;
        }).error(function (error) {
            $scope.status = 'Unable to load help data: ' + error.message;
        });
    }

    $scope.getJobs();
    $scope.update = function (job) {
        apiRepository.updateJob(job.id, job.cron).success(function (data) {
            $scope.getJobs();
        }).error(function (error) {
            console.log(error.message);
        });
    }
}]);

app.controller('newController', ['$scope', 'apiRepository', function ($scope, apiRepository) {
    $scope.job = { Queue: "default", CallbackUrl: "http://www.baidu.com", Cron: "* * * * *", Method: "GET" };

    $scope.add = function (job) {
        apiRepository.addJob(job).success(function (data) {
            window.location.href = "#jobs";
        }).error(function (error) {
            alert(error.message);
        });
    }
}]);


