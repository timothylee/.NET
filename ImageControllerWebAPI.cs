/// <summary>
/// Retrieves a profile's image from the database
/// </summary>
/// <param name="id">The profile's id</param>
/// <returns>The profile's image</returns>
public HttpResponseMessage Get(int id)
{
	using (var db = DatabaseTools.GetDbContext())
	{
		var data = from i in db.Images
				   where i.ImageId == id
				   select i;

		Image img = (Image)data.SingleOrDefault();
		byte[] imgData = img.ImageData;
		MemoryStream ms = new MemoryStream(imgData);
		HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
		response.Content = new StreamContent(ms);
		response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
		return response;
	}
}

/// <summary>
/// Uploads an image for a specific profile using an HTTP request 
/// </summary>
/// <returns>The id of the uploaded image</returns>
public Task<IEnumerable<int>> Post()
{
	if (Request.Content.IsMimeMultipartContent())
	{
		string fullPath = HttpContext.Current.Server.MapPath("~/uploads");
		MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(fullPath);
		var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
		{
			if (t.IsFaulted || t.IsCanceled)
				Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
			var fileInfo = streamProvider.FileData.Select(i =>
			{
				var info = new FileInfo(i.LocalFileName);
				using (var db = DatabaseTools.GetDbContext())
				{
					Image img = new Image();
					img.ImageData = File.ReadAllBytes(info.FullName);
					db.Images.Add(img);
					db.SaveChanges();
					return img.ImageId;
				}
			});
			return fileInfo;
		});
		return task;
	}
	else
	{
		throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
	}
}