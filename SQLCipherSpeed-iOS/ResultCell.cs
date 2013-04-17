
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SQLCipherSpeed
{
	public partial class ResultCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("ResultCell");
		public static readonly UINib Nib;

		static ResultCell ()
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				Nib = UINib.FromName ("ResultCell_iPhone", NSBundle.MainBundle);
			else
				Nib = UINib.FromName ("ResultCell_iPad", NSBundle.MainBundle);
		}
		
		public ResultCell (IntPtr handle) : base (handle)
		{
		}
		
		public static ResultCell Create ()
		{
			return (ResultCell)Nib.Instantiate (null, null) [0];
		}

		public void SetTrial(TimedTrial trial)
		{
			this.labelName.Text = trial.Name;
			this.labelNormalMs.Text = Convert.ToString(trial.NormalTime);
			this.labelEncyptedMs.Text = Convert.ToString(trial.EncryptedTime);
			this.labelDifference.Text = trial.DifferenceAsPercentString;
			this.labelSql.Text = trial.Sql;
		}
	}
}

