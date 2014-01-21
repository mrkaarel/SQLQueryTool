using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace SqlQueryTool
{
    public class ConnectionData
    {
		private const string STORAGE_KEY = "slokoroirsirraequurqemio";
		private const string STORAGE_IV = "biiteher";
		private const string STORAGE_FILE_NAME = "userSettings.dat";

        public static int DefaultTimeout;

        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseIntegratedSecurity { get; set; }
        public int Timeout { get; set; }

        public ConnectionData(string settingString)
        {
            string[] settings = settingString.Split('|');
            this.ServerName = settings[0];
            this.DatabaseName = settings[1];
            this.UserName = settings[2];
            this.Password = settings[3];
            if (settings.Length > 4 && Int32.Parse(settings[4]) == 1) {
                this.UseIntegratedSecurity = true;
            }
            this.Timeout = DefaultTimeout;
        }

        public ConnectionData(string serverName, string databaseName, string userName, string password, bool useIntegratedSecurity)
            : this(serverName, databaseName, userName, password, useIntegratedSecurity, DefaultTimeout)
        { }

        public ConnectionData(string serverName, string databaseName, string userName, string password, bool useIntegratedSecurity, int timeout)
        {
            this.ServerName = serverName;
            this.DatabaseName = databaseName;
            this.UserName = userName;
            this.Password = password;
            this.UseIntegratedSecurity = useIntegratedSecurity;
            this.Timeout = timeout;
        }

		public static List<ConnectionData> LoadSavedSettings()
		{
			List<ConnectionData> previousSettings = new List<ConnectionData>();

			IsolatedStorageFile userData = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);
			using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(STORAGE_FILE_NAME, FileMode.OpenOrCreate, userData)) {
				if (storageFileStream.Length > 0) {
					TripleDES tripleDes = TripleDESCryptoServiceProvider.Create();
					tripleDes.Key = StringToByteArray(STORAGE_KEY);
					tripleDes.IV = StringToByteArray(STORAGE_IV);

					CryptoStream cryptoStream = new CryptoStream(storageFileStream, tripleDes.CreateDecryptor(), CryptoStreamMode.Read);

					using (StreamReader reader = new StreamReader(cryptoStream)) {
						while (!reader.EndOfStream) {
							previousSettings.Add(new ConnectionData(reader.ReadLine()));
						}
					}
				}
			}

			return previousSettings;
		}

		public static void WriteSettingsToFile(List<ConnectionData> settings)
		{
			IsolatedStorageFile userData = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);
			IsolatedStorageFileStream isolateStorageStream = new IsolatedStorageFileStream(STORAGE_FILE_NAME, FileMode.Create, userData);

			TripleDES tripleDes = TripleDESCryptoServiceProvider.Create();
			tripleDes.Key = StringToByteArray(STORAGE_KEY);
			tripleDes.IV = StringToByteArray(STORAGE_IV);

			CryptoStream cryptoStream = new CryptoStream(isolateStorageStream, tripleDes.CreateEncryptor(), CryptoStreamMode.Write);
			StreamWriter writer = new StreamWriter(cryptoStream);

			foreach (ConnectionData setting in settings) {
				writer.WriteLine(setting.SettingString);
			}

			writer.Close();
		}

		private static byte[] StringToByteArray(string s)
		{
			ASCIIEncoding enc = new ASCIIEncoding();
			return enc.GetBytes(s);
		}

		public static void AddSetting(ConnectionData setting)
		{
			List<ConnectionData> settings = LoadSavedSettings();
			settings.Insert(0, setting);
			WriteSettingsToFile(settings);
		}

		public static void DeleteSetting(ConnectionData settingToDelete)
		{
			List<ConnectionData> settings = LoadSavedSettings().Where(s => s.SettingString != settingToDelete.SettingString).ToList();
			WriteSettingsToFile(settings);
		}

        public string SettingString
        {
            get
            {
                return String.Format("{0}|{1}|{2}|{3}|{4}", this.ServerName, this.DatabaseName, this.UserName, this.Password, UseIntegratedSecurity ? 1 : 0);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}@{1}", this.DatabaseName, this.ServerName);
        }
    }
}
