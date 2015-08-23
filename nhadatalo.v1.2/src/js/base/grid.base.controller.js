function GridBaseController($scope) 
{
	//PROPERTIES
	var self = $scope;
	
    self.gridOptions = {
	    gridActions: [
		    {name:"Delete", value:"D"},
		    {name:"Export", value:"E"}
	    ],
	    filterOptions: {
	        filterText: "",
	        useExternalFilter: true
	    },
	    sortOptions: {
	    	sortField: 'Id',
	    	sortOrder: 'ASC'
	    },
	    totalServerItems : 0,
	    pagingOptions : {
	        pageSizes: [10, 500, 1000],
	        pageSize: 10,
	        currentPage: 1
	    }
	};

	self.gridSelectedAction = self.gridOptions.gridActions[0];

    self.gridSettings = {
        data: 'results',
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'gridOptions.totalServerItems',
        pagingOptions: self.gridOptions.pagingOptions,
        filterOptions: self.gridOptions.filterOptions,
        sortInfo: { 
        	fields: [],
        	directions: [],
        	columns: []
        },
        plugins: [ new ngGridDraggableRow('Id') ],
        useExternalSorting: true,
        selectedItems: []
    };
    //END PROPERTIES

    self.$watch('gridOptions.pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
          	$scope.search();
        }
    }, true);

    self.$watch('gridOptions.filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
          	$scope.search();
        }
    }, true);

    self.$watch('gridSettings.sortInfo', function (newVal, oldVal) {
	    if (newVal !== oldVal && newVal.fields.length) {
	    	$scope.gridOptions.sortOptions.sortField = newVal.fields[0];
	    	$scope.gridOptions.sortOptions.sortOrder = newVal.directions[0];
          	$scope.search();
        }
	}, true);

	self.setPagingData = function(data, pageNumber, pageSize){  
        $scope.results = data.Data;
        $scope.gridOptions.totalServerItems = data.TotalCount;
        var pageNumberMax = Math.ceil( data.TotalCount / pageSize);
        if($scope.gridOptions.pagingOptions.currentPage > pageNumberMax) $scope.gridOptions.pagingOptions.currentPage = 1;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };

 	  // start row drag
	$scope.$on("ngGrigDraggableRowEvent_rowDragStart", function(evt, args) {

		console.log("START row drag");

		// if drag multiple rows, set custom drag image.
		if (args.rowItemsMove.length > 1) {
		  	$scope.dragRowNum = args.rowItemsMove.length;
		  	$scope.$apply();
		  	if (args.dataTransfer.setDragImage) {
		    	var dragImage = document.getElementById('dragIconMultiRows');
		    	if(dragImage !== undefined)
		    		args.dataTransfer.setDragImage(dragImage, 16, 16);
		  	}
		}

	});

	// start reordering rows by drag
	$scope.$on('ngGrigDraggableRowEvent_changeRowOrderPre', function(evt, willSortInit) {
		console.log("START reordering rows by drag");
		if (willSortInit) {
		  	console.log("sort wiil change");
		}
	});


  
 
}

