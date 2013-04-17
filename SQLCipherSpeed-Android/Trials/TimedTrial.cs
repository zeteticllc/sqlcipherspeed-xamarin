using System;
using System.Data.Common;

namespace SQLCipherSpeed
{
	public abstract class TimedTrial
	{
		public string Name {get;set;}
		
		public string Sql {get;set;}
		
		public long NormalTime {get; set;}
		
		public long EncryptedTime {get; set;}

		public int Iterations {get; set;}

		public bool UseTransaction {get; set;}

		public bool Report {get; set;}

		public delegate void OnBind(DbCommand command, int iteration);

		public OnBind Bind {get; set;}

		public TimedTrial() 
		{
			Iterations = 1;
			UseTransaction = false;
			Report = true;
		}

		public decimal Difference 
		{
			get 
			{
				if(NormalTime == 0) return 0;
 				return (Convert.ToDecimal(EncryptedTime) - Convert.ToDecimal(NormalTime)) / Convert.ToDecimal(NormalTime);
			}
		}

		public decimal DifferenceAsPercent 
		{
			get 
			{
				return Difference * Convert.ToDecimal(100);
			}
		}

		public string DifferenceAsPercentString 
		{
			get 
			{
				return string.Format("{0:0.0}%", DifferenceAsPercent);
			}
		}

		public void RunComparison(DbConnection normalConn, DbConnection encryptedConn)
		{
			NormalTime = TimedRun (normalConn);
			EncryptedTime = TimedRun (encryptedConn);
		}

		public long TimedRun(DbConnection connection) 
		{
			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch ();
			stopwatch.Start ();

			Run (connection);

			stopwatch.Stop();
			return stopwatch.ElapsedMilliseconds;
		}

		public abstract void Run(DbConnection connection);

	}
}

