app.controller('SettingController', ['$scope','toaster', 'settingService', 
    function($scope,toaster, settingService) 
    {

	    //VARIABLES
	    var self = $scope;
	    var setting = self.$stateParams.setting;
        var isEditing = setting ? true : false;
        if (isEditing) {
        	self.setting = setting;
        };

        self.isEditing = isEditing;
	    //END VARIABLES

	    //PUBLIC FUNCTIONS
	   	

	    self.save = function(setting){
	    	angular.element('.butterbar').removeClass('hide').addClass('active');
	    	var req = {};
	    	if(isEditing) req = settingService.update(setting.Id, setting);
	    	else req = settingService.add(setting);

	    	req.$promise.then(
	    		function(){
	    			angular.element('.butterbar').addClass('hide').removeClass('active');
		    		self.$state.go('management.settings.list');
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
     
	}
]);
