using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SQLCipherSpeed
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		MainViewController viewController;

		public TrialRunner Runner {get; set;}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			var rootNavigationController = new UINavigationController(); 
			var mainController = new MainViewController();
			rootNavigationController.PushViewController(mainController, false); 
			this.window.RootViewController = rootNavigationController; 
			this.window.MakeKeyAndVisible (); 
			
			return true;
		}
	}
}

