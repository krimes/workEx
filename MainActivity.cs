using System;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views.Animations;



namespace WorkEX
{

	[Activity (Label = "WorkEX", Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
//		Adapters.BidsListAdapter taskList;
//		IList<ListBids> tasks;
		ListView taskListView;
		ProgressBar prgBarMain;

		public static string UserId = "";
		public List<string> ListCatalog;

		private class UpdateMainTask : AsyncTask {
			private MainActivity _Main;
			private ListView List1;
			AlphaAnimation inAnimation;
			AlphaAnimation outAnimation;
			Adapters.BidsListAdapter taskList;
			IList<ListBids> tasks;
			public UpdateMainTask(MainActivity a)
			{
				_Main = a;
				List1 = _Main.taskListView;
//				taskList = _Main.taskList;
//				tasks = _Main.tasks;
			}


			protected override void OnPreExecute() {
				base.OnPreExecute();
				inAnimation = new AlphaAnimation(0f, 1f);
				inAnimation.Duration = 200;
				_Main.prgBarMain.Animation  = inAnimation;
				_Main.taskListView.Visibility = ViewStates.Gone;
				_Main.prgBarMain.Visibility = ViewStates.Visible;
			}

			protected override void OnPostExecute(Java.Lang.Object result) {
				base.OnPostExecute(result);
				outAnimation = new AlphaAnimation(1f, 0f);
				outAnimation.Duration = 200;
				_Main.prgBarMain.Animation = outAnimation;
				_Main.prgBarMain.Visibility = 	ViewStates.Gone;
				_Main.taskListView.Visibility = ViewStates.Visible;
			}

			protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params) {
				try {
					tasks = new List<ListBids>();
					tasks = WorkEX.GetJSON.ShowBidsByUserId ();
					taskList = new Adapters.BidsListAdapter(_Main, tasks);

					//Hook up our adapter to our ListView
					_Main.RunOnUiThread(delegate {
						_Main.taskListView.Adapter = taskList;
					});
					//List1.Adapter = taskList;

				} catch {
				}

				return null;
			}
		}

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

			prgBarMain = FindViewById<ProgressBar> (Resource.Id.progressBarMain);

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
			new UpdateMainTask (this).Execute ();
		}
	}
}


