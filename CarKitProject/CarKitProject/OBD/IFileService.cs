namespace CarKitProject.OBD
{
	public interface IFileService
	{
		string LoadFromLog();
		void DeleteLog();
		void SaveToLog(string logText);
		void SaveToSdCard(string logText, string fileName);
	}
}
