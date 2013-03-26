using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Common;

namespace SQLCipherSpeed
{
	public class TrialRunner 
	{
		public List<TimedTrial> Trials = new List<TimedTrial>() {
			new TimedTrial() { Name="Key Derivation 1", Sql = "SELECT count() FROM sqlite_master;" },
			new TimedTrial() { Name="Key Derivation 2", Sql = "SELECT count() FROM sqlite_master;" }
		};

		public void Run ()
		{
			var normalPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "normal.db");
			var encryptedPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "encrypted.db");
			File.Delete(normalPath);
			File.Delete(encryptedPath);

			using(var normalConn = new Mono.Data.Sqlite.SqliteConnection(String.Format("Data Source={0}",normalPath))) 
			{
				using(var encryptedConn = new Mono.Data.Sqlcipher.SqliteConnection(String.Format("Data Source={0}",encryptedPath)))
				{
					normalConn.Open();

					((Mono.Data.Sqlcipher.SqliteConnection) encryptedConn).SetPassword("test");
					encryptedConn.Open();

					foreach(var trial in Trials)
					{
						trial.RunTests(normalConn, encryptedConn);
					}
				}
			}
		}

	}
}
