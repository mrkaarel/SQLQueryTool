using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SqlQueryTool.Connections
{
	public class ConnectionDataStorage
	{
		private const string STORAGE_KEY = "slokoroirsirraequurqemio";
		private const string STORAGE_IV = "biiteher";
		private const string STORAGE_FILE_NAME = "userSettings.dat";


		public static List<ConnectionData> LoadSavedSettings()
		{
			var previousSettings = new List<ConnectionData>();

			using (var storageStream = OpenStorageStream(FileMode.OpenOrCreate)) {
				if (storageStream.Length > 0) {
					var cryptoStream = new CryptoStream(storageStream, GetCryptoAlgorithm().CreateDecryptor(), CryptoStreamMode.Read);

					using (var rdr = new StreamReader(cryptoStream)) {
						while (!rdr.EndOfStream) {
							previousSettings.Add(new ConnectionData(rdr.ReadLine()));
						}
					}
				}
			}

			return previousSettings;
		}

		public static void WriteSettingsToFile(IEnumerable<ConnectionData> settings)
		{
			using (var storageStream = OpenStorageStream(FileMode.Create)) {
				var cryptoStream = new CryptoStream(storageStream, GetCryptoAlgorithm().CreateEncryptor(), CryptoStreamMode.Write);

				using (var writer = new StreamWriter(cryptoStream)) {
					foreach (ConnectionData setting in settings) {
						writer.WriteLine(setting.SettingString);
					}
				}
			}
		}

		private static byte[] StringToByteArray(string s)
		{
			ASCIIEncoding enc = new ASCIIEncoding();
			return enc.GetBytes(s);
		}

		public static IEnumerable<ConnectionData> AddSetting(ConnectionData setting)
		{
			var settings = LoadSavedSettings();
			settings.Insert(0, setting);
			WriteSettingsToFile(settings);

			return settings;
		}

		public static IEnumerable<ConnectionData> DeleteSetting(ConnectionData settingToDelete)
		{
			var settings = LoadSavedSettings().Where(s => s.SettingString != settingToDelete.SettingString).ToList();
			WriteSettingsToFile(settings);

			return settings;
		}

		public static IEnumerable<ConnectionData> MoveFirst(ConnectionData setting)
		{
			DeleteSetting(setting);
			return AddSetting(setting);
		}

		private static IsolatedStorageFileStream OpenStorageStream(FileMode openMode)
		{
			var userData = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);
			return new IsolatedStorageFileStream(STORAGE_FILE_NAME, openMode, userData);
		}

		private static SymmetricAlgorithm GetCryptoAlgorithm()
		{
			var tripleDes = TripleDESCryptoServiceProvider.Create();
			tripleDes.Key = StringToByteArray(STORAGE_KEY);
			tripleDes.IV = StringToByteArray(STORAGE_IV);

			return tripleDes;
		}

	}
}
