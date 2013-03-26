
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SQLCipherSpeed
{
	public class ResultViewSource : UITableViewSource
	{
		TrialRunner runner;

		public ResultViewSource ()
		{
			AppDelegate app = 
				(AppDelegate)UIApplication.SharedApplication.Delegate;

			runner = app.Runner;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return runner.Trials.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Trial Results";
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (ResultCell.Key) as ResultCell;
			if (cell == null)
				cell = ResultCell.Create();

			cell.Trial = runner.Trials[indexPath.Item];
			
			return cell;
		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath) 
		{
			return 120.0f;
		}
	}
}

