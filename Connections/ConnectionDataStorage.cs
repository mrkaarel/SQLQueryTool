using System;
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
		private static readonly string STORAGE_FILE_NAME = "SavedConnections";

		private static readonly Encoding STORAGE_ENCODING = Encoding.UTF8;
		private static readonly string STORAGE_ITEM_SEPARATOR = Environment.NewLine;

		public static List<ConnectionData> LoadSavedSettings()
		{
			var previousSettings = new List<ConnectionData>();
			using (var storageStream = OpenStorageStream(FileMode.OpenOrCreate)) {
				var protectedSettings = ReadFully(storageStream);
				
				if (protectedSettings.Length == 0) {
					return previousSettings;
				}

				var settingsString = STORAGE_ENCODING.GetString(ProtectedData.Unprotect(protectedSettings, null, DataProtectionScope.CurrentUser));
				foreach (var serializedSettingsString in settingsString.Split(new string[] { STORAGE_ITEM_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries )) {
					previousSettings.Add(new ConnectionData(serializedSettingsString));
				}
			}

			return previousSettings;
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
			var settings = LoadSavedSettings().Where(s => s.SerializedString != settingToDelete.SerializedString).ToList();
			WriteSettingsToFile(settings);

			return settings;
		}

		public static IEnumerable<ConnectionData> MoveFirst(ConnectionData setting)
		{
			DeleteSetting(setting);
			return AddSetting(setting);
		}

		private static void WriteSettingsToFile(IEnumerable<ConnectionData> settings)
		{
			using (var storageStream = OpenStorageStream(FileMode.Create)) {
				string settingsString = String.Join(STORAGE_ITEM_SEPARATOR, settings.Select(s => s.SerializedString).ToArray());
				var protectedSettings = ProtectedData.Protect(STORAGE_ENCODING.GetBytes(settingsString), null, DataProtectionScope.CurrentUser);

				storageStream.Write(protectedSettings, 0, protectedSettings.Length);
			}
		}

		private static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream()) {
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0) {
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		private static IsolatedStorageFileStream OpenStorageStream(FileMode openMode)
		{
			var userData = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);
			return new IsolatedStorageFileStream(STORAGE_FILE_NAME, openMode, userData);
		}
	}
}
