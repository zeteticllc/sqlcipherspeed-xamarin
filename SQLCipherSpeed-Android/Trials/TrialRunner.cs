using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Common;

namespace SQLCipherSpeed
{
	public class TrialRunner 
	{
		private static Random _random = new Random();

		private static void SetParameters(DbCommand command, IDictionary<string,object> parameters) 
		{
			foreach(var kvp in parameters) 
			{
				var p = command.CreateParameter();
				p.ParameterName = kvp.Key;
				p.Value = kvp.Value;
				command.Parameters.Add(p);
			}
		}

		public List<TimedTrial> Trials = new List<TimedTrial>() {
			new TimedTrialQuery() { 
				Name = "Initialize", 
				Sql = @"SELECT count(*) FROM sqlite_master;" },
			new TimedTrialNonQuery() { 
				Name = "Create Table (1st)", 
				Sql = @"CREATE TABLE t1(a INTEGER, b INTEGER, c VARCHAR(100));" },
			new TimedTrialNonQuery() { 
				Name = "Create Table (2nd)", 
				Sql = @"CREATE TABLE t2(a INTEGER, b INTEGER, c VARCHAR(100));" },
			new TimedTrialNonQuery() { 
				Name="500 Inserts (no transaction)", 
				Sql = @"INSERT INTO t1 VALUES (@a,@b,@c);", 
				Iterations = 500,
				Bind = (c, i) => { 
					var random = _random.Next();
					SetParameters(c, new Dictionary<string, object>() {
						{"@a", i},
						{"@b", random},
						{"@c", Convert.ToString(random)},
					});
				}
			},
			new TimedTrialNonQuery() { 
				Name="15000 Inserts (with transaction)", 
				Sql = @"INSERT INTO t1 VALUES (@a,@b,@c);", 
				Iterations = 15000, 
				UseTransaction = true, 
				Bind = (c,i) => { 
					var random = _random.Next();
					SetParameters(c, new Dictionary<string, object>() {
						{"@a", i},
						{"@b", random},
						{"@c", Convert.ToString(random)},
					});
				}
			},
			new TimedTrialQuery() { 
				Name="50 Selects (no index)", 
				Sql = @"SELECT count(*), avg(b) FROM t2 WHERE b >= @a AND b < @b;", 
				Iterations = 50, 
				Bind = (c,i) => { 
					SetParameters(c, new Dictionary<string, object>() {
						{"@a", i * 50},
						{"@b", (i + 10) * 50}
					});
				}
			},
			new TimedTrialQuery() { 
				Name="50 SELECTs on string comparison", 
				Sql = @"SELECT count(*), avg(b) FROM t2 WHERE c LIKE '%' || @a || '%'", 
				Iterations = 50, 
				Bind = (c,i) => { 
					SetParameters(c, new Dictionary<string, object>() {
						{"@a", Convert.ToString(i)}
					});
				}
			}

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

					/*
					using(var command = encryptedConn.CreateCommand()) {
						command.CommandText = "PRAGMA kdf_iter = 20000;";
						command.ExecuteNonQuery();
					}
					*/

					foreach(var trial in Trials)
					{
						trial.RunComparison(normalConn, encryptedConn);
					}
				}
			}
		}

	}
}
