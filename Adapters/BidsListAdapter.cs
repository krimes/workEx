﻿using System.Collections.Generic;
using Android.App;
using Android.Widget;

namespace WorkEX.Adapters
{
	public class BidsListAdapter: BaseAdapter<ListBids> {
		Activity context = null;
		IList<ListBids> tasks = new List<ListBids>();

		public BidsListAdapter (Activity context, IList<ListBids> tasks) : base ()
		{
			this.context = context;
			this.tasks = tasks;
		}
		public override ListBids this[int position]
		{
			get { return tasks[position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return tasks.Count; }
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = tasks [position];			

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ??
			           context.LayoutInflater.Inflate ( Resource.Layout.listBids, parent, false)) as LinearLayout;

			// Find references to each subview in the list item's view
			var txtName = view.FindViewById<TextView> (Resource.Id.NameText);
			var txtDescription = view.FindViewById<TextView> (Resource.Id.NotesText);

			//Assign item's values to the various subviews
			txtName.SetText (item.Date, TextView.BufferType.Normal);
			txtDescription.SetText (item.Text, TextView.BufferType.Normal);

			//Finally return the view
			return view;
		}
	}
}