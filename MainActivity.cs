using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;


namespace WorkEX
{

	[Activity (Label = "WorkEX", Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		Adapters.BidsListAdapter taskList;
		IList<ListBids> tasks;
		ListView taskListView;

		public static string UserId = "";
		public List<string> ListCatalog;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
//			Button buttonAddBid = FindViewById<Button> (Resource.Id.btnAddBids);
//			EditText editText1 = FindViewById<EditText> (Resource.Id.editText1);

			TextView userIdTest = FindViewById<TextView> (Resource.Id.userIdTest);

			TextView TextView1 = FindViewById<TextView> (Resource.Id.textView1);
			TextView textReq = FindViewById<TextView> (Resource.Id.textRequest);
			taskListView = FindViewById<ListView> (Resource.Id.listView1);
			/*
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};*/
			var props = new Props(); //Хранение настроек
			props.ReadXml();

//			string test;
//			test = WorkEX.GetJSON.GetUserId ();

			if (props.Fields.UserIDValue.Length>0){
				UserId = props.Fields.UserIDValue;
			}else{
				textReq.Text = WorkEX.GetJSON.GetUserId();

				UserId = WorkEX.GetJSON.GetUserId ();
				textReq.Text = UserId;
				props.Fields.UserIDValue = UserId;
				props.WriteXml();
			}

			userIdTest.Text = string.Format("My id={0}",UserId);

			// Show List Tickets
//			WorkEX.MainActivity.updateListBids (TextView1);

			button.Click += (object sender, EventArgs e) =>
			{
				if (ListCatalog == null) {
					ListCatalog = WorkEX.GetJSON.GetCatalog ();
				}

				var intent = new Intent(this, typeof(ActivityAddBids));
				intent.PutStringArrayListExtra("Cate_List", ListCatalog);
				StartActivity(intent);
			};
		}
		protected override void OnResume ()
		{
			base.OnResume ();
			tasks = new List<ListBids>();
			tasks = WorkEX.GetJSON.ShowBidsByUserId ();
			taskList = new Adapters.BidsListAdapter(this, tasks);

			//Hook up our adapter to our ListView
			taskListView.Adapter = taskList;

		}
	}
}


