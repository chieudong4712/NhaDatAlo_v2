app.factory('userService', ['$http', '$resource', '$q', 'serviceHelperSvc', function ($http, $resource, $q, serviceHelper) {
    //DECLARES
    var self = this;
    var urls = {
        getPagedUrl :  "api/Account?pageSize=:pageSize&pageNumber=:pageNumber&sortField=:sortField",
        searchUrl :  "api/Account/search",
        updateProfile :  "api/Account/updateProfile",
        getProfile: "api/Account/getProfile",
        serviceUrl :  "api/Account",
    }


    var resource = $resource(serviceHelper.buildUrl(urls.serviceUrl+'/:id'), null, 
        {
            get:{method: 'get'},
            getPaged: {
                url: serviceHelper.buildUrl(urls.getPagedUrl), 
                method:'get', params: { pageSize: '@pageSize', pageNumber: '@pageNumber',sortField: '@sortField' }
            },
            search: {
                url: serviceHelper.buildUrl(urls.searchUrl), 
                method: 'post'
            },
            update: {method: 'put'},
            add:{method: 'post'},
            remove: {method: 'delete'},
            updateProfile: {
                url: serviceHelper.buildUrl(urls.updateProfile), 
                method: 'post'
            },
            getProfile: {
                url: serviceHelper.buildUrl(urls.getProfile), 
                method: 'get'
            }
        });
        
    var baseService = new BaseService($http, $resource, $q, serviceHelper);
    baseService.setResource(resource);
    var service     = angular.extend(this, baseService);
    //END DECLARES
    
    //METHODS
    service.updateProfile = function (profile) {
        return resource.updateProfile(profile);
    };

    service.getProfile = function () {
        return resource.getProfile();
    };
    
    //END METHODS


    return service;

}]);