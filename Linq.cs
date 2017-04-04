/// <summary>
/// Update existing project with incoming project data
/// </summary>
/// <param name="project">Project from legacy database to be matched with Project Server Project</param>
public static void MatchProject(DmgProject project)
{

	List<DmgProject> epmProjectList = MigrationSettings.Instance.epmProjectList;

	List<DmgProject> matchedProject = epmProjectList.Where(p => p.Upc == project.Upc
		&& p.Territory == project.Territory
		&& p.Lyrics == project.Lyrics
		&& p.ReleaseType == project.ReleaseType
		&& p.Artist == project.Artist
		&& p.ProductName == project.ProductName
		&& p.VersionTitle == project.VersionTitle
		&& p.Selection == project.Selection
		&& Helper.ExtractBeforeDash(p.Config) == project.Config
		&& p.ProductId == project.ProductId
		&& p.ReleaseDate.ToShortDateString() == project.ReleaseDate.ToShortDateString()
		&& p.PhysicalDigital == project.PhysicalDigital
		&& p.ProductFormatDig == project.ProductFormatDig
		&& p.ProductFormatPhy == project.ProductFormatPhy
		&& Helper.ExtractAfterDash(p.Label) == project.Label
		&& p.Genre == project.Genre).ToList();

	if (matchedProject.Count() != 1)
		return;

	project.ProjectGuid = matchedProject[0].ProjectGuid;
	project.projectDataSet = project.ProjectDataAccess.ReadProject(project.ProjectGuid, DataStoreEnum.WorkingStore);
}