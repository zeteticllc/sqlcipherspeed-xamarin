
using System;
using System.Drawing;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SQLCipherSpeed
{
	public partial class MainViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MainViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MainViewController_iPhone" : "MainViewController_iPad", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		partial void runButtonClick (MonoTouch.Foundation.NSObject sender)
		{
			this.runButton.SetTitle("Running Tests", UIControlState.Normal);
			this.runButton.Enabled = false;
			ThreadPool.QueueUserWorkItem (o => RunTrials());
		}

		private void RunTrials() {

			AppDelegate app = 
				(AppDelegate)UIApplication.SharedApplication.Delegate;
			app.Runner = new TrialRunner();
			app.Runner.Run();

			InvokeOnMainThread(() => {
				this.runButton.Enabled = true;
				var controller = new ResultViewController();
				NavigationController.PushViewController(controller, true);
			});
		}
	}
}

