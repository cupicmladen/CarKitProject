namespace CarKitProject.OBD
{
	public interface IFileService
	{
		string LoadFromLog();
		void DeleteLog();
		void SaveToLog(string logText);
	}
}
