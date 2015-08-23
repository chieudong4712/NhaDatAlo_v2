app.factory('serviceHelperSvc', ['$http', '$resource', function ($http, $resource) {
    var baseUrl = config.apiurl;
    
    function buildUrl(resourceUrl) {
        return baseUrl + resourceUrl;
    };

    return {
        buildUrl: buildUrl,
        get: function(serviceUrl, params){
            var resource = $resource(buildUrl(serviceUrl), null, params)
            return resource;
        },
        get2: function(serviceUrl, success, error){
            var deferred = $q.defer();

            $http({
                method: 'GET',
                url: serviceUrl
            })
            .success(function (data, status, headers, config) {
                if (angular.isDefined(success))
                {
                    success(data);
                }

                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                if (angular.isDefined(error)) {
                    error(data);
                }

                deferred.resolve({ result: "SERVICE_ERR_0001", message: "Service failed." });
            });

            return deferred.promise;
        },
        AuthorizationToken: $resource(buildUrl("Token"), null,
        {
            requestToken: { method: 'POST', headers: { "Content-Type": "application/x-www-form-urlencoded" } }
        }),
        User: $resource(buildUrl('api/Account/'), null, 
        {
            get:{method: 'get'}
        }),
        Account: $resource(buildUrl('api/Account/'), null,
            {
                register: { method: 'post' },
                logOff: { method: 'put' }
            }),
        Resource: $resource(buildUrl('api/Resources/:resourceId'),
            { resourceId: '@Id' },
            {
                'update': { method: 'PUT' },
                getPagedItems: { url: buildUrl("api/Resources?count=:count&page=:page&sortField=:sortField&sortOrder=:sortOrder"), method: 'GET', params: { count: '@count', page: '@page', sortField: '@sortField', sortOrder: '@sortOrder' } }
            }),
        Location: $resource(buildUrl('api/Locations/:locationId'), { locationId: '@Id' }, { 'update': { method: 'PUT' } }),

        ResourceActivity: $resource(buildUrl('api/Resources/:resourceId/Activities/:activityId'),
                { resourceId: '@ResourceId', activityId: '@Id' })
    };
}]);