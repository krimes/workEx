using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;

namespace WorkEX
{
	[Activity (Label = "Оставить заявку")]			
	public class ActivityAddBids : Activity
	{
		 
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Create your application here
			SetContentView(Resource.Layout.LAddBids);

			Button buttonAddBid = FindViewById<Button> (Resource.Id.btnAddBids);
			Spinner Spiner1 = FindViewById<Spinner> (Resource.Id.spinner1);
			EditText editText1 = FindViewById<EditText> (Resource.Id.editText1);


			var Cate_List = Intent.Extras.GetStringArrayList("Cate_List") ?? new string[0];

			// Create an ArrayAdapter using the string array and a default spinner layout
			ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, Cate_List);
			// Specify the layout to use when the list of choices appears
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			// Apply the adapter to the spinner
			Spiner1.Adapter = adapter;	

			buttonAddBid.Click += (object sender, EventArgs e) =>
			{
				Save();
			};

		}
		void Save()
		{
			Spinner Spiner1 = FindViewById<Spinner> (Resource.Id.spinner1);
			EditText editText1 = FindViewById<EditText> (Resource.Id.editText1);

//			api.php?uid=e26589dc4f619165a300d7f316e05951&action=add-bids&title=название&text=мой_текст&cate_id=0
			string s = Spiner1.SelectedItem.ToString();
			int i = WorkEX.GetJSON.GetCatalogId (s);
			if (!WorkEX.GetJSON.AddBidsByUserId (i, editText1.Text)) {
				ShowDialog (0);
			};
			Finish();
		}
	}
}

