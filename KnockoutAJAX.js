self.ProjectIsCheckedOut = function (projectUid) {
	return $.ajax({
		type: "POST",
		url: '/_layouts/15/dmg.CreateProject/WebMethods.aspx/IsProjectCheckedOut',
		contentType: "application/json; charset=utf-8",
		data: ko.toJSON({ 'projectUid': projectUid }),
		dataType: "json",
		success: function (results) {
			if (results.d) {
				self.ProjectIsCheckOut(true);
			}
			else if (results.d == false) {
				self.ProjectIsCheckOut(false);
			}
			else {
				self.Message("Product information not found for project: " + self.ProjectUID());
			}
		},
		error: function (err) {
			alert(err.responseText);
		}
	}); 
}