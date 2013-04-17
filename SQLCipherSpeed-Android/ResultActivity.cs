
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SQLCipherSpeed
{
	[Activity (Label = "ResultActivity")]			
	public class ResultActivity : ListActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			var app =  ((App) this.ApplicationContext);
			ListAdapter = new TrialArrayAdapter(this, app.Runner.Trials.ToArray());
		}
	}
}

