app.factory('categoryService', ['$http', '$resource', '$q', 'serviceHelperSvc', function ($http, $resource, $q, serviceHelper) {

    //DECLARES
    var self = this;
    var urls = {
        getPagedUrl :  "api/Categories?pageSize=:pageSize&pageNumber=:pageNumber&sortField=:sortField",
        searchUrl :  "api/Categories/search",
        orderUrl :  "api/Categories/order",
        serviceUrl :  "api/Categories",
    }
    
    var resource = $resource(serviceHelper.buildUrl(urls.serviceUrl+'/:id'), null, 
        {
            get:{method: 'get'},
            getParent: {method: 'get'},
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
            order: {
                url: serviceHelper.buildUrl(urls.orderUrl), 
                method: 'post'
            },
        });
        
    var baseService = new BaseService($http, $resource, $q, serviceHelper);
    baseService.setResource(resource);
	var service = angular.extend(this, baseService);
    //END DECLARES
    
    //METHODS
    service.order = function (items) {
        return resource.order(items);
    };
    
    service.getAllParent = function(){
        var se = {
            ParentId: null
        }
        return resource.getParent({se: se});
    }
    //END METHODS


    return service;

}]);