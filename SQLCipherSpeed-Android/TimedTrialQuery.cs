
using System;
using System.Data.Common;

namespace SQLCipherSpeed
{
	class TimedTrialQuery : TimedTrial
	{
		public override void Run(DbConnection connection)
		{

			DbTransaction trans = null;
			if(UseTransaction) trans = connection.BeginTransaction();

			using (var command = connection.CreateCommand()) 
			{
				command.CommandText = Sql;

				for(int i = 0; i < Iterations; i++) 
				{
					if(Bind != null) Bind(command, i);

					var reader = command.ExecuteReader ();
					while (reader.Read()) {}
					reader.Close();
				}
			}

			if(UseTransaction) trans.Commit();
		}
	}
}

