
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

namespace SQLCipherSpeedAndroid
{
	[Activity (Label = "ResultActivity")]			
	public class ResultActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Result);
			var label = FindViewById<TextView> (Resource.Id.screen2Label);
			label.Text = Intent.GetStringExtra("MainData") ?? "Data not available";
		}
	}
}

