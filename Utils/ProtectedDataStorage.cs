using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace SqlQueryTool.Utils
{
	public class ProtectedDataStorage
	{
		private static readonly Encoding STORAGE_ENCODING = Encoding.UTF8;

		public static string ReadString(string fileName)
		{
			return STORAGE_ENCODING.GetString(ReadBytes(fileName));
		}

		public static void WriteString(string fileName, string contents)
		{
			WriteBytes(fileName, STORAGE_ENCODING.GetBytes(contents));
		}

		public static object Read(string fileName)
		{
			var bytes = ReadBytes(fileName);

			if (bytes.Length == 0) {
				return null;
			}

			using (var ms = new MemoryStream(bytes)) {
				return new BinaryFormatter().Deserialize(ms);
			}
		}

		public static void Write(string fileName, object data)
		{
			using (var ms = new MemoryStream()) {
				new BinaryFormatter().Serialize(ms, data);

				WriteBytes(fileName, ms.ToArray());
			}
		}

		public static void Delete(string fileName)
		{
			var storage = OpenStorage();
			if (storage.GetFileNames(fileName).Length > 0) {
				storage.DeleteFile(fileName);
			}
		}

		private static void WriteBytes(string fileName, byte[] contents)
		{
			using (var storageStream = OpenStorageStream(fileName, FileMode.Create)) {
				var protectedBytes = ProtectedData.Protect(contents, null, DataProtectionScope.CurrentUser);
				storageStream.Write(protectedBytes, 0, protectedBytes.Length);
			}
		}

		private static byte[] ReadBytes(string fileName)
		{
			using (var storageStream = OpenStorageStream(fileName, FileMode.OpenOrCreate)) {
				var protectedBytes = ReadStreamToBytes(storageStream);

				if (protectedBytes.Length == 0) {
					return protectedBytes;
				}

				return ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
			}
		}

		private static byte[] ReadStreamToBytes(Stream input)
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

		private static IsolatedStorageFileStream OpenStorageStream(string fileName, FileMode openMode)
		{
			return new IsolatedStorageFileStream(fileName, openMode, OpenStorage());
		}

		private static IsolatedStorageFile OpenStorage()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);
		}
	}
}
