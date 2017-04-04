public class ProjectDataAccess : IDisposable
{
	private WebSvcProject.ProjectSoapClient clientProject { get; set; }

	public ProjectDataAccess()
	{
		clientProject = new ProjectSoapClient();
	}

	public ProjectDataSet ReadProject(Guid projectUid, DataStoreEnum store)
	{
		return clientProject.ReadProject(projectUid, store);
	}

	public void QueueUpdateProject(Guid jobId, Guid session, ProjectDataSet dataSet, bool validateOnly)
	{
		clientProject.QueueUpdateProject(jobId, session, dataSet, validateOnly);
	}

	public void QueuePublish(Guid jobId, Guid projectUid, bool fullpublish, string wssUrl)
	{
		clientProject.QueuePublish(jobId, projectUid, fullpublish, wssUrl);
	}

	public void QueueAddToProject(Guid jobId, Guid session, ProjectDataSet dataset, bool validateOnly)
	{
		clientProject.QueueAddToProject(jobId, session, dataset, validateOnly);
	}

	public void QueueCheckInProject(Guid guid, Guid projectUid, bool force, Guid session, string sessionDesc)
	{
		clientProject.QueueCheckInProject(guid, projectUid, force, session, sessionDesc);
	}

	public ProjectDataSet ReadProjectStatus(Guid guid, DataStoreEnum store, string projName, int projType)
	{
		return clientProject.ReadProjectStatus(guid, store, projName, projType);
	}

	public Guid CreateProjectFromTemplate(Guid templateGuid, string projectName)
	{
		return clientProject.CreateProjectFromTemplate(templateGuid, projectName);
	}

	public void CheckOutProject(Guid projectUid, Guid session, string sessionDesc)
	{
		clientProject.CheckOutProject(projectUid, session, sessionDesc);
	}
	public ProjectDataSet ReadProjectList()
	{
		return clientProject.ReadProjectList();
	}

	public void QueueDeleteProjects(Guid jobGuid, bool deleteWSS, Guid[] projectUids, bool deleteboth)
	{
		clientProject.QueueDeleteProjects(jobGuid, deleteWSS, projectUids, deleteboth);
	}

	public void Dispose()
	{
		try
		{
			if (clientProject == null) return;
			if (clientProject.State != System.ServiceModel.CommunicationState.Faulted)
				clientProject.Close();
		}
		finally
		{
			if (clientProject.State != System.ServiceModel.CommunicationState.Closed)
				clientProject.Abort();
		}

	}
}