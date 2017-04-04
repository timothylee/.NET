/// <summary>
/// Queries EPM Database to retrieve a strongly typed list of tasks
/// </summary>
/// <returns>a list of ProjectTask objects</returns>
public static List<ProjectTask> GetEpmTaskList()
{
	List<ProjectTask> epmTaskList = new List<ProjectTask>();
	
	var sqlQuery = Constants.EpmTaskListQuery;

	using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectServerData"].ToString()))
	{
		conn.Open();
		SqlCommand command = new SqlCommand(sqlQuery, conn);

		using (SqlDataReader reader = command.ExecuteReader())
		{
			while (reader.Read())
			{
				ProjectTask newEpmTask = ProjectController.BuildEpmDigitalPartnerTask(reader);
				epmTaskList.Add(newEpmTask);
			}
		}
	}

	return epmTaskList;
}