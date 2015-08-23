'use strict';

/* Controllers */
  // signin controller
app.controller('SigninFormController', ['$scope', '$http', '$state', '$location', 'accountSvc', function($scope, $http, $state, $location, accountSvc) {
    $scope.user = {};
    $scope.authError = null;
    $scope.login = function() {
      $scope.authError = null;

      accountSvc.login($scope.user).$promise
        .then(function (data) {
            $scope.$emit('logOn');
            $location.url('/app/dashboard-v1');
        }).catch(function (errorResponse) {
            if (errorResponse.status == 404) {
                $scope.errorMessage = errorResponse.data;
            }
            if (errorResponse.status === 400) {
                $scope.errorMessage = "Invalid Email/Password";
            }
            else {
                $scope.errorMessage = "An error occured while performing this action. Please try after some time.";
            }
        });

      // Try to login
      // $http.post('api/login', {email: $scope.user.username, password: $scope.user.password})
      // .then(function(response) {
      //   if ( !response.data.user ) {
      //     $scope.authError = 'Email or Password not right';
      //   }else{
      //     $state.go('app.dashboard-v1');
      //   }
      // }, function(x) {
      //   $scope.authError = 'Server Error';
      // });

    };
  }])
;