/// <summary>
/// Singleton for Migration Settings
/// Holds global UMG Schedule upon instantiation
/// </summary>
public class MigrationSettings
{
	private static MigrationSettings instance;
	public static MigrationSettings Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new MigrationSettings();
				instance.umgScheduleList = UmgScheduleCreation.BuildUmgSchedule();
			}
			return instance;
		}
	}
}