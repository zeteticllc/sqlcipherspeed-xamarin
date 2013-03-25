using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;

namespace SQLCipherSpeedAndroid
{
	[Activity (Label = "SQLCipherSpeed-Android", MainLauncher = true)]
	public class MainActivity : Activity
	{
		private ProgressDialog _progress;
		private Handler _progressHandler;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_progress = new ProgressDialog(this);
			_progress.SetMessage(GetString(Resource.String.progress_text));
			_progressHandler = new ProgressHandler(_progress);

			SetContentView (Resource.Layout.Main);

			var button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += delegate {
				ThreadPool.QueueUserWorkItem (o => RunTrials());
			};
		}

		private void RunTrials() {
			RunOnUiThread(() => {
				_progress.Show();
			});

			var trials = new TrialSet();

			RunOnUiThread(() => {
							var second = new Intent(this, typeof(ResultActivity));
							second.PutExtra("MainData", "Results from First Screen");
							_progressHandler.SendEmptyMessage(0);
							StartActivity(second);
			});
		}


	}

	class ProgressHandler : Handler
	{
		private ProgressDialog _dialog;
		
		public ProgressHandler (ProgressDialog dialog)
		{
			_dialog = dialog;
		}
		
		public override void HandleMessage (Message msg)
		{
			base.HandleMessage (msg);
			_dialog.Dismiss();
		}
	}
}


