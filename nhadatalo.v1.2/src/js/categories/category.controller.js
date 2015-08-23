app.controller('CategoryController', ['$scope','toaster', 'categoryService', 'data', 
    function($scope,toaster, categoryService, data) 
    {

	    //VARIABLES
        
	    var self = $scope;
	    var category = self.$stateParams.category;
        var isEditing = category ? true : false;
        

        self.isEditing = isEditing;
        self.parents = data;
	    //END VARIABLES
        //PRIVATE FUNCTIONS
        var bindCategory = function(category){
            category.Parent = category.ParentId != null ? _.find(self.parents,{"Value": category.ParentId.toString()}):null;
            return category;
        };
        
        var prepareCategory = function(category){
            category.ParentId = self.category.Parent != null ? self.category.Parent.Value: null;
            return category;
        };
        //END PRIVATE FUNCTIONS
	    //PUBLIC FUNCTIONS
        
	   	self.reset = function(){
            self.category = {};
        };

	    self.save = function(category){
            category = prepareCategory(category);
	    	angular.element('.butterbar').removeClass('hide').addClass('active');
	    	var req = {};
	    	if(isEditing) req = categoryService.update(category.Id, category);
	    	else req = categoryService.add(category);

	    	req.$promise.then(
	    		function(){
	    			angular.element('.butterbar').addClass('hide').removeClass('active');
		    		self.$state.go('management.categories.list');
		    	}, 
		    	function(rep){
		    		angular.element('.butterbar').addClass('hide').removeClass('active');
		    		var errors = "";
		    		var modelState = rep.data.ModelState;
		    		if (modelState) {
		    			for (var key in modelState) {
	                        for (var i = 0; i < modelState[key].length; i++) {
	                            errors+= modelState[key][i]+" ";
	                        }
	                    }
		    			toaster.pop('error', "Error", errors);
		    		}
		    		else{
		    			toaster.pop('error', "Error", rep.data.Message);
		    		};
		    	});
	    	
	    }
		//END PUBLIC FUNCTIONS
        
        //INIT
        if (isEditing) {
        	self.category = bindCategory(category);
        };
        //END INIT
     
	}
]);
