app.controller('UserSettingController', ['$scope', '$http', '$filter', 'userService', 'userProfileService','FileUploader', 'data', 
	function($scope, $http, $filter, userService, userProfileService, FileUploader, data) {
	

	//VARIABLES
    var self = $scope;
    self.user = data;
    var uploader = $scope.uploader = new FileUploader({
        url: 'js/controllers/upload.php'
    });

    //END VARIABLES

    // FILTERS

    uploader.filters.push({
        name: 'customFilter',
        fn: function(item /*{File|FileLikeObject}*/, options) {
            return this.queue.length < 10;
        }
    });
    // END FILTERS

    //PUBLIC FUNCTIONS
   	
    self.upload = function(){
        uploader.uploadAll();
    };
    self.save = function(){
    	angular.element('.butterbar').removeClass('hide').addClass('active');
    	var req = userService.updateProfile(self.user);
    	

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

	//BIRTHDAY PICKER
    $scope.today = function() {
        $scope.dt = new Date();
    };
    $scope.today();

    $scope.clear = function() {
        $scope.dt = null;
    };

    // Disable weekend selection
    $scope.disabled = function(date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.toggleMin = function() {
        $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();

    $scope.open = function($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    $scope.dateOptions = {
        formatYear: 'yyyy',
        startingDay: 1,
        class: 'datepicker',
        showWeeks: false,
    };

    $scope.initDate = new Date('2016-15-20');
    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
    //END BIRTHDAY PICKER
}]);
