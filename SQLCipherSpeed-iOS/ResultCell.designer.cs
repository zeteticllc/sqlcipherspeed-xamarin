// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SQLCipherSpeed
{
	[Register ("ResultCell")]
	partial class ResultCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel labelName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel labelNormalMs { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel labelEncyptedMs { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel labelDifference { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel labelSql { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (labelName != null) {
				labelName.Dispose ();
				labelName = null;
			}

			if (labelNormalMs != null) {
				labelNormalMs.Dispose ();
				labelNormalMs = null;
			}

			if (labelEncyptedMs != null) {
				labelEncyptedMs.Dispose ();
				labelEncyptedMs = null;
			}

			if (labelDifference != null) {
				labelDifference.Dispose ();
				labelDifference = null;
			}

			if (labelSql != null) {
				labelSql.Dispose ();
				labelSql = null;
			}
		}
	}
}
