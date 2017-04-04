function DeleteProjectViewModel() {
    var self = this;
    self.Projects = ko.observableArray([]);
    self.ProjectNames = ko.observableArray([]);
    self.UPCCode = ko.observableArray([]);
    self.Message = ko.observable();
    self.ReleaseDate = ko.observableArray([]);
    self.Catalog = ko.observableArray([]);
    self.ConfigCode = ko.observableArray([]);
    self.Projectid = ko.observable();

    var ProjectViewModel = function (projectName, selected, projectUPC, productid, productUID, releaseDate, catalog,configCode) {
        this.ProjectTitle = projectName;        
        this.Checked = ko.observable(selected);
        this.ProjectUPC = projectUPC;
        this.ProductId = productid;
        this.ProjectUID = productUID;
		
        if (releaseDate != null && releaseDate != undefined && releaseDate != "") {
            var dateStringVal = moment(releaseDate).format('MM/DD/YYYY');
            this.ReleaseDate = dateStringVal;
        }
        else {
            this.ReleaseDate = null;
        }
		
        this.Catalog = catalog;
        this.ConfigCode = configCode;
        this.highlighted = ko.observable(false);
        this.styleclass = ko.observable(false);
		
        this.hoverIn = function () {
            this.highlighted(true);
            this.styleclass("ms-itmHoverEnabled ms-itmhover s4-itm-selected");
        },
		
        this.hoverOut = function () {
            if (!this.Checked()) {
                this.highlighted(false);
                this.styleclass("ms-itmHoverEnabled ms-itmhover");
            }  
        }
    };

    self.showSpinner = ko.observable(true);
    self.currentProject = ko.observable();
    self.removedProject = ko.observable();
    self.Ent_PodType = "";
    self.ProjectUID = "";
    self.SearchText = ko.observable();
    self.searchThrottledValue = ko.computed(self.SearchText).extend({ throttle: 1000 }); // wait for one second
    self.searchThrottledValue.subscribe(function (val) {
        self.SearchText(val);

        // only search for products if search text is more than 2 characters
        if (self.SearchText().length > 2)
            self.getProjects(val);

        $("#btnSave").attr('disabled', 'disabled');
    });

    self.ProjectName = ko.observableArray([]);

    self.load = function () {
        self.showSpinner(true);
        self.getProjects();

        self.Projects.sort(compare);
        self.showSpinner(false);
    };
    
    self.SingleSelectDeleteProject = function (data) {
        ko.utils.arrayForEach(self.Projects(), function (item) {
            if (item.Checked() && item.ProjectTitle != data.ProjectTitle) {
                item.Checked(false);
                item.highlighted(false);
                item.styleclass("ms-itmHoverEnabled ms-itmhover");
            }
        });

        return true;
    }

    self.DeleteProducts = function () {
        if (confirm('Are you sure you want to permanently delete this item?')) {
            self.Message("");
            var itemArray = [];
            var url = _spPageContextInfo.webAbsoluteUrl;
            var clientContext = new SP.ClientContext(url);
            var selectedItem = ko.utils.arrayFilter(self.Projects(), function (item) { return item.Checked() });
           
            var confirmmessage = "Are you sure you want to delete the Product " + selectedItem[0].ProjectTitle + " – " + selectedItem[0].ProjectUPC + " - " + selectedItem[0].Catalog + " – " + selectedItem[0].ReleaseDate;
            if (confirm(confirmmessage))
            {
                self.Projectid(selectedItem[0].ProjectUID);
                self.removedProject(selectedItem[0]);
                ExecuteProjectUpdate();
            }
        }
    };

    self.getProjects = function (filter) {
        self.showSpinner(true);
        var textToFind = filter;
        if (textToFind == undefined) {
            self.showSpinner(false);
            return;

        }
        self.Projects([])

        var text = textToFind.toLowerCase();
        var url = _odataUrl + PROJDATA + PROJQUERY + "?$filter=substringof('" + textToFind + "',ProjectName) or substringof('" + textToFind + "',Ent_UPC) or substringof('" + textToFind + "',Ent_Selection)";

        return $.ajax({
            url: url,
            type: "GET",
            async: true,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("ACCEPT", accept);

            },
            success: function (xhr, textStatus) {
                ko.utils.arrayForEach(xhr.d.results, function (item) {
                    self.Projects.push(new ProjectViewModel(item.ProjectName, false, item.Ent_UPC, item.Ent_ProductID, item.ProjectId, item.Ent_ReleaseDate, item.Ent_Selection, item.Ent_ConfigCode));
                });

                if (self.removedProject() != "" && self.removedProject() != undefined)
                {
                    var index = self.Projects.indexOf(self.removedProject());
                    if (index > -1) {
                        self.Projects.splice(index, 1);
                    }
                }
                
                self.showSpinner(false);
                self.Projects.sort(compare);
            }
        });
    }

    self.disableDeleteButton = ko.computed(function () {
        if (self.Projects() == undefined) return true;
        var disabled = true;
		
        ko.utils.arrayForEach(self.Projects(), function (item) {
            if (item.Checked()) {
                disabled = false;
            }
        });
		
        return disabled;
    });
}
