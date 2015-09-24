app.factory('profileProductService', ['$http', '$resource', '$q', 'serviceHelperSvc', function ($http, $resource, $q, serviceHelper) {
    //DECLARES
    var self = this;
    var urls = {
        getAllByUser :  "api/Products/GetAllByUser",
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
            },
            changePassword: {
                url: serviceHelper.buildUrl(urls.changePasswordUrl), 
                method: 'post'
            },
            changeAvatar : {
                url: serviceHelper.buildUrl(urls.changeAvatarUrl), 
                method: 'post'
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

    service.changePassword = function (model) {
        return resource.changePassword(model);
    };

    service.changeAvatar = function (model) {
        return resource.changeAvatar(model);
    };

    service.uploadFile = serviceHelper.buildUrl("api/files/Upload");
    
    //END METHODS


    return service;

}]);