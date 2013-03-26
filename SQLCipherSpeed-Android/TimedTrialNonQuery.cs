
using System;
using System.Data.Common;

namespace SQLCipherSpeed
{
	class TimedTrialNonQuery : TimedTrial
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

					command.ExecuteNonQuery();
				}
			}

			if(UseTransaction) trans.Commit();
		}
	}
}

