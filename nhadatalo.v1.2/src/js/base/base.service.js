function BaseService($http, $resource, $q, serviceHelper) {
    var resource;
            
    var setResource = function(res){
        resource = res;
    }
            
    var find = function(id){
        return resource.get({id: id});
    }
    
    var getAll = function () {
        return resource.query();
    }
    
    var getPaged = function(params){
        return resource.getPaged(params);
    }

    var search = function(se){
        return resource.search(se)
    }
    
    var update = function(id, item){
        return resource.update({id:id}, item);
    }
    
    var add = function(item){
        return resource.add(item);
    }

    var remove = function(id){
        return resource.remove(id);
    }

    var removeList = function(ids){
        return resource.remove({ids:ids});
    }
  
    
    return {
        setResource: setResource,
        find: find,
        getAll: getAll,
        getPaged: getPaged,
        add: add,
        update: update,
        remove: remove,
        removeList: removeList,
        search: search
    };

    
}