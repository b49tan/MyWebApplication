var TodoListApp = angular.module("TodoListApp", ["ngRoute", "ngResource", "ds.clock"]).
    config(function ($routeProvider) {
        $routeProvider.
            when('/', { controller: 'Home', templateUrl: 'home.html' }).
            when('/Home', { controller: 'Home', templateUrl: 'home.html' }).
            when('/TodoList', { controller: ListCtrl, templateUrl: 'list.html' }).
            when('/Contact', { controller: 'Home', templateUrl: 'contact.html' }).
            when('/AboutUs', { controller: 'Home', templateUrl: 'aboutus.html' }).
            when('/Clock', { controller: 'Home', templateUrl: 'clock.html' }).
            when('/Sent', { controller: 'Home', templateUrl: 'sent.html' }).
           // when('/newest/', { controller: newestCtrl, templateUrl: 'newest.html'}).
            otherwise({ redirectTo: '/' });
    });

TodoListApp.controller('MenuCtrl', function ($scope, $log, $rootScope) {
    $scope.menuOptions = ['Contact', 'TodoList', 'Home', 'AboutUs', 'Clock'];
});
TodoListApp.controller('Home', function ($scope, $log, $rootScope) {
    
    var time = (new Date());
    var newyork = time;
    $scope.newyorktime = newyork - ( 1437636420000 - 1437602220000);

    var time = (new Date());
    var london = time;
    $scope.londontime = london - ( 1437636420000 - 1437620220000 ) ;
    
});
TodoListApp.factory('TodoList', function ($resource) {
    return $resource('/api/todolist/:id', { id: '@id' }, { update: { method: 'PUT' } });
});
// TodoListApp.factory('Newest', function ($resource) {
   // return $resource('/Newest');
//});
//var newestCtrl = function ($scope, $location, Newest) {
  //  $scope.textfromDB = Newest.get();
//};

var ListCtrl = function ($scope, $location, TodoList) {
    $scope.search = function () {
        $scope.todolists = TodoList.query({ sort: $scope.sort_order, desc: $scope.is_desc });
    };

    $scope.search();

    $scope.sort_by = function (col) {
        if ($scope.sort_order === col) {
            $scope.is_desc = !$scope.is_desc;
        }
        else {
            $scope.is_desc = false;
            $scope.is_order = col;
        }
        $scope.search();
    };
};

TodoListApp.directive('clock', function ($scope) {

    return {
        templateUrl: '/clock.html'
    }
});