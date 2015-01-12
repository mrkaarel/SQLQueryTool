using SqlQueryTool.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlQueryTool.Connections
{
	public class ConnectionDataStorage
	{
		private static readonly string STORAGE_FILE_NAME = "SavedConnections";
		private static readonly string STORAGE_ITEM_SEPARATOR = Environment.NewLine;

		public static List<ConnectionData> LoadSavedSettings()
		{
			var previousSettings = new List<ConnectionData>();

			var settingsString = ProtectedDataStorage.ReadString(STORAGE_FILE_NAME);
			foreach (var serializedSettingsString in settingsString.Split(new string[] { STORAGE_ITEM_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries )) {
				previousSettings.Add(new ConnectionData(serializedSettingsString));
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
			string settingsString = String.Join(STORAGE_ITEM_SEPARATOR, settings.Select(s => s.SerializedString).ToArray());
			ProtectedDataStorage.WriteString(STORAGE_FILE_NAME, settingsString);
		}
	}
}
