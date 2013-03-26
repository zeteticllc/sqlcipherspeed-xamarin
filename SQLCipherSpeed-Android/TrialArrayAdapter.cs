
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
	class TrialArrayAdapter : BaseAdapter<TimedTrial>
	{
		TimedTrial[] items;
		Activity context;

		public TrialArrayAdapter(Activity context, TimedTrial[] items) : base() {
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override TimedTrial this[int position] {  
			get { return items[position]; }
		}

		public override int Count {
			get { return items.Length; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view = context.LayoutInflater.Inflate(Resource.Layout.ResultItem, null);

			view.FindViewById<TextView>(Resource.Id.textViewResultItemReportName).Text = items[position].Name;
			view.FindViewById<TextView>(Resource.Id.textViewResultItemNormalMs).Text = Convert.ToString(items[position].NormalTime);
			view.FindViewById<TextView>(Resource.Id.textViewResultItemEncryptedMs).Text = Convert.ToString(items[position].EncryptedTime);
			view.FindViewById<TextView>(Resource.Id.textViewResultItemDifference).Text = items[position].DifferenceAsPercentString;
			view.FindViewById<TextView>(Resource.Id.textViewResultItemSql).Text = items[position].Sql;

			return view;
		}
	}
}

