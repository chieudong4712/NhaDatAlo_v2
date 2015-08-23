app.factory('accountSvc', ['$http', 'serviceHelperSvc', 'userProfileService', 'localStorageService',
    function ($http, serviceHelper, userProfileService, localStorageService) {

    var Token = serviceHelper.AuthorizationToken;
    var Account = serviceHelper.Account;

    var buildFormData = function (formData) {
        var dataString = '';
        for (var prop in formData) {
            if (formData.hasOwnProperty(prop)) {
                dataString += (prop + '=' + formData[prop] + '&');
            }
        }
        return dataString.slice(0, dataString.length - 1);
    };

    return {
        login: function (userLogin) {
            var formData = { username: userLogin.email, password: userLogin.password, grant_type: 'password' };
            return Token.requestToken(buildFormData(formData), function (data) {
                $http.defaults.headers.common.Authorization = "Bearer " + data.access_token;
                userProfileService.role = data.role;
                userProfileService.userName = data.userName;
                localStorageService.set('authorizationData', { token: data.access_token, userName: data.userName, refreshToken: data.refresh_token, useRefreshTokens: true });
            })
            
        },
        registerUser: function (userRegistration) {
            var registration = Account.register(userRegistration);
            return registration;
        },
        logOffUser: function () {
            $http.defaults.headers.common.Authorization = null;
            localStorageService.remove('authorizationData');
        }
    };
}]);