app.controller('SettingsController', ['$scope', 'settingService', 
    function($scope, settingService) 
    {
    	var base = this;
    	angular.extend(this, new GridBaseController($scope));

    	var self = $scope;


    	
    	//PROPERTIES
    	self.gridSettings.columnDefs = [
    		{field: 'Id', displayName: 'Id' },
    		{field: 'OrderNumber', displayName: 'OrderNumber' },
    		{field: 'Name', displayName: 'Name' },
    		{field: 'Text', displayName: 'Text' },
    		{field: 'Value', displayName: 'Value' },
	    	{displayName: 'Edit', 
	    		cellTemplate: 
	    		'<div class="ui-grid-cell-contents">' +
	    		'	<button class="btn btn-xs btn-info" ng-click="edit(row.entity)" >Edit</button>' + 
	    		'	<button class="btn btn-xs btn-danger" ng-click="remove(row.entity)">Del</button>' +
	    		'</div>',
	    		cellClass: 'ng-grid-acction-cell'
	    	}
	  	]
    	//END PROPERTIES

    	

    	

    	//PRIVATE FUNCTIONS
    	function updateArrayIndex(rows) {
			for (var i = 0; i < rows.length; i++) {
				rows[i].OrderNumber = i;
			}
			return rows;
		}
    	//END PRIVATE FUNCTIONS

    	//PUBLIC FUNCTIONS
	    self.gridApply = function(){
	    	if (self.gridSelectedAction.value=='D') {
	    		var selectedItems = self.gridSettings.selectedItems;
	    		if (selectedItems.length) {
		    		var ids = _.pluck(selectedItems, "Id")
		    		settingService.removeList(ids).$promise.then(function(){
		    			self.search();
		    		})
	    		};
	    	};
	    };

	    self.edit = function(setting){
	    	var settingId = setting.Id;
	    	//self.$state.go('management.settings.edit',{settingId: settingId });
	    	self.$state.go('management.settings.edit', {setting: setting });
	    }
		
		self.remove = function(setting){
			if(confirm('Do you want to delete the item?')){
				var id = setting.Id;
				settingService.remove({id: id}).$promise
				.then(function(){
					self.search();
				});
			}
		};

	    self.loadGrid = function(){
	    	var pageSize = self.gridOptions.pagingOptions.pageSize;
	    	var pageNumber = self.gridOptions.pagingOptions.currentPage;
	    	
	    	settingService.getPaged({pageSize:pageSize, pageNumber:pageNumber, sortField: "OrderNumber"})
            	.$promise.then(function(data){
	            	self.setPagingData(data,pageNumber,pageSize);
	            });
	    };

	    self.search = function () {
	    	angular.element('.butterbar').removeClass('hide').addClass('active');
	    	var pageSize =   self.gridOptions.pagingOptions.pageSize; 
	    	var pageNumber =  self.gridOptions.pagingOptions.currentPage; 
	    	var sortField =  self.gridOptions.sortOptions.sortField; 
	    	var sortOrder =  self.gridOptions.sortOptions.sortOrder; 
	    	var searchText =  self.gridOptions.filterOptions.filterText;
	    	settingService.search({
    			Name:searchText,
    			PageSize:pageSize, 
        		PageNumber:pageNumber, 
        		SortField:sortField, 
        		SortOrder:sortOrder
    		})
        	.$promise.then(function(data){
        		angular.element('.butterbar').addClass('hide').removeClass('active');
            	self.setPagingData(data,pageNumber,pageSize);
            });
	    };

	    //END PUBLIC FUNCTIONS

	    //WATCH FUNCTIONS
	    // end reordering rows by drag
		$scope.$on('ngGrigDraggableRowEvent_changeRowOrderPost', function(evt, doneSortInit) {
			console.log("END reordering rows by drag");
			var rows = updateArrayIndex(self.results);
			var settingOrdered = _.map(rows, function(item){
				return {Id: item.Id, OrderNumber: item.OrderNumber};
			});
			settingService.order(settingOrdered)
		});
	    //END WATCH FUNCTIONS

	    //INIT 
    	self.loadGrid();
    	//END INIT
	}
]);
