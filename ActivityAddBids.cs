using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;

namespace WorkEX
{
	[Activity (Label = "Оставить заявку", NoHistory = true)]			
	public class ActivityAddBids : Activity
	{
		EditText editText1;
		EditText editText2;
		EditText editText3;
		EditText editText4;
		EditText editText5;
		Spinner Spiner1;
		int BidsId = 0;
		 
		protected override void OnCreate (Bundle bundle)
		{
			//MainActivity _Main;

			base.OnCreate (bundle);
			// Create your application here
			SetContentView(Resource.Layout.LAddBids);

			int taskID = Intent.GetIntExtra("TaskID", -1);

			//_Main = Intent.GetType("MainActivity", null);
			//if(taskID > 0) {

			Button buttonAddBid = FindViewById<Button> (Resource.Id.btnAddBids);
			Spiner1 = FindViewById<Spinner> (Resource.Id.spinner1);
			editText1 = FindViewById<EditText> (Resource.Id.editText1);
			editText2 = FindViewById<EditText> (Resource.Id.editText2);
			editText3 = FindViewById<EditText> (Resource.Id.editText3);
			editText4 = FindViewById<EditText> (Resource.Id.editText4);
			editText5 = FindViewById<EditText> (Resource.Id.editText5);




			var Cate_List = Intent.Extras.GetStringArrayList("Cate_List") ?? new string[0];

			// Create an ArrayAdapter using the string array and a default spinner layout
			ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, Cate_List);
			// Specify the layout to use when the list of choices appears
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			// Apply the adapter to the spinner
			Spiner1.Adapter = adapter;	

			if (taskID >=0) {
				var l = MainActivity.tasks [taskID];
				BidsId = l.ID;
				editText1.Text = l.Title;
				editText2.Text = l.Text;
				editText3.Text = l.Adress;
				editText4.Text = l.Telefone;
				editText5.Text = l.Name;
				var icat = GetJSON.GetCatalogText (l.CateID);
				var cati = Cate_List.IndexOf (icat);
				Spiner1.SetSelection (cati);
			}
			buttonAddBid.Text = (taskID == -1 ? "Добавить" : "Обновить");
			buttonAddBid.Click += (object sender, EventArgs e) =>
			{
				if (taskID >=0) {
					Update(taskID);
				} else {
				Save();
				}
			};

		}
		void Save()
		{
			//Spinner Spiner1 = FindViewById<Spinner> (Resource.Id.spinner1);
			//EditText editText1 = FindViewById<EditText> (Resource.Id.editText1);

//			api.php?uid=e26589dc4f619165a300d7f316e05951&action=add-bids&title=название&text=мой_текст&cate_id=0
			string s = Spiner1.SelectedItem.ToString();
			int i = WorkEX.GetJSON.GetCatalogId (s);
			if (!WorkEX.GetJSON.AddBidsByUserId (i, editText1.Text, editText2.Text, editText3.Text, editText4.Text, editText5.Text)) {
				//ShowDialog (0);
			};
			Finish();
		}
		void Update(int taskID)
		{
			//Spinner Spiner1 = FindViewById<Spinner> (Resource.Id.spinner1);
			//EditText editText1 = FindViewById<EditText> (Resource.Id.editText1);

			//			api.php?uid=e26589dc4f619165a300d7f316e05951&action=add-bids&title=название&text=мой_текст&cate_id=0
			string s = Spiner1.SelectedItem.ToString();
			int i = WorkEX.GetJSON.GetCatalogId (s);
			if (!WorkEX.GetJSON.UpdateBidsByUserId (BidsId, i, editText1.Text, editText2.Text, editText3.Text, editText4.Text, editText5.Text)) {
				//ShowDialog (0);
			};
			Finish();
		}
	}
}

