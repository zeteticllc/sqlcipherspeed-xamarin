using System;
using System.Data.Common;

namespace SQLCipherSpeed
{
	public class TimedTrial
	{
		public string Name {get;set;}
		
		public string Sql {get;set;}
		
		public long NormalTime {get; set;}
		
		public long EncryptedTime {get; set;}

		public decimal Difference {
			get 
			{
				return (Convert.ToDecimal(EncryptedTime) - Convert.ToDecimal(NormalTime)) / Convert.ToDecimal(NormalTime);
			}
		}

		public decimal DifferenceAsPercent {
			get 
			{
				return Difference * Convert.ToDecimal(100);
			}
		}

		public string DifferenceAsPercentString {
			get 
			{
				return string.Format("{0:0.0}%", DifferenceAsPercent);
			}
		}

		public void RunTests(DbConnection normalConn, DbConnection encryptedConn)
		{
			NormalTime = Run (normalConn);
			EncryptedTime = Run (encryptedConn);
		}
		
		private long Run(DbConnection connection) 
		{
			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch ();
			stopwatch.Start ();

			using (var command = connection.CreateCommand()) {
				command.CommandText = Sql;
				var reader = command.ExecuteReader ();
				while (reader.Read()) {}
			}
			stopwatch.Stop();
			return stopwatch.ElapsedMilliseconds;
		}
	}
}

