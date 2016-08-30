using System;
using System.IO;
using CarKitProject.Droid.OBD;
using CarKitProject.OBD;

[assembly: Xamarin.Forms.Dependency(typeof(FileService))]
namespace CarKitProject.Droid.OBD
{
	public class FileService : IFileService
	{
		public string LoadFromLog()
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, "ObdLogFile.txt");
			return File.ReadAllText(filePath);
		}

		public void DeleteLog()
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, "ObdLogFile.txt");
			File.Delete(filePath);
		}

		public void SaveToLog(string logText)
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, "ObdLogFile.txt");
			File.AppendAllText(filePath, logText);
		}

		public void SaveToSdCard(string logText, string fileName)
		{
			var externalStoragePath = Android.OS.Environment.ExternalStorageDirectory.Path;
			var directoryname = Path.Combine(externalStoragePath, "CarKitAppCashe");
			var exists = Directory.Exists(directoryname);
			if (!exists)
				Directory.CreateDirectory(directoryname);

			var filePath = Path.Combine(directoryname, fileName + ".txt");
			File.AppendAllText(filePath, logText);
		}
	}
}