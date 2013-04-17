using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Common;
using System.Linq;

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

		private List<TimedTrial> _trials = new List<TimedTrial>() {
			new TimedTrialQuery() { 
				Name = "Initialize", 
				Report = false,
				Sql = @"SELECT count(*) FROM sqlite_master;" },
			new TimedTrialNonQuery() { 
				Name = "Create Table (1st operation)", 
				Sql = @"CREATE TABLE t1(a INTEGER, b INTEGER, c VARCHAR(100));" },
			new TimedTrialNonQuery() { 
				Name = "Create Table (2nd)", 
				Report = false,
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
				Name="30000 Inserts (with transaction)", 
				Sql = @"INSERT INTO t2 VALUES (@a,@b,@c);", 
				Iterations = 30000, 
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
			new TimedTrialNonQuery() { 
				Name="500 Updates (w/o index, w/o transaction)", 
				Sql = @"UPDATE t2 SET b=b*2 WHERE a = @a", 
				Iterations = 500, 
				Bind = (c,i) => { 
					SetParameters(c, new Dictionary<string, object>() {
						{"@a", i * 60}
					});
				}
			},
			new TimedTrialNonQuery() { 
				Name = "Create Index", 
				Report = false,
				Sql = @"CREATE INDEX i2a ON t2(a)" },
			new TimedTrialQuery() { 
				Name="30000 Selects (w/ index)", 
				Sql = @"SELECT * FROM t2 WHERE a = @a", 
				Iterations = 30000, 
				Bind = (c,i) => { 
					SetParameters(c, new Dictionary<string, object>() {
						{"@a", i}
					});
				}
			},
			new TimedTrialNonQuery() { 
				Name="2500 Updates (w/ index + transaction)", 
				Sql = @"UPDATE t2 SET b = @b WHERE a = @a", 
				Iterations = 2500,
				UseTransaction = true,
				Bind = (c,i) => { 
					var random = _random.Next();
					SetParameters(c, new Dictionary<string, object>() {
						{"@a", i * 10},
						{"@b", random},
					});
				}
			}
		};

		public IEnumerable<TimedTrial> Trials
		{
			get
			{
				return _trials.Where(t => t.Report);
			}
		}

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

					foreach(var trial in _trials)
					{
						trial.RunComparison(normalConn, encryptedConn);
					}
				}
			}
		}

	}
}
