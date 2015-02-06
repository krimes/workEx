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
		Button button;

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
				inAnimation.Duration = 350;
				_Main.prgBarMain.Animation  = inAnimation;
				_Main.taskListView.Visibility = ViewStates.Gone;
				_Main.prgBarMain.Visibility = ViewStates.Visible;
			}

			protected override void OnPostExecute(Java.Lang.Object result) {
				base.OnPostExecute(result);
				outAnimation = new AlphaAnimation(1f, 0f);
				outAnimation.Duration = 350;
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

		private class GetUserIdTask : AsyncTask {
			private MainActivity _Main;
			AlphaAnimation inAnimation;
			AlphaAnimation outAnimation;
			public GetUserIdTask(MainActivity a)
			{
				_Main = a;
			}
				
			protected override void OnPreExecute() {
				base.OnPreExecute();
				inAnimation = new AlphaAnimation(0f, 1f);
				inAnimation.Duration = 500;
				_Main.button.Animation  = inAnimation;
				_Main.button.Visibility = ViewStates.Gone;
			}

			protected override void OnPostExecute(Java.Lang.Object result) {
				base.OnPostExecute(result);
				outAnimation = new AlphaAnimation(1f, 0f);
				outAnimation.Duration = 500;
				_Main.button.Animation = outAnimation;
				_Main.button.Visibility = ViewStates.Visible;
			}

			protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params) {
				try {

					var props = new Props(); //Хранение настроек
					props.ReadXml();

					if (props.Fields.UserIDValue.Length>0){
						UserId = props.Fields.UserIDValue;
					}else{
						UserId = WorkEX.GetJSON.GetUserId ();
						props.Fields.UserIDValue = UserId;
						props.WriteXml();
					}

					//Hook up our adapter to our ListView
					_Main.RunOnUiThread(delegate {
						new UpdateMainTask (_Main).Execute ();
					});

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
			button = FindViewById<Button> (Resource.Id.myButton);
			prgBarMain = FindViewById<ProgressBar> (Resource.Id.progressBarMain);
			taskListView = FindViewById<ListView> (Resource.Id.listView1);

			new GetUserIdTask (this).Execute ();

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
			if (UserId != "") {
				new UpdateMainTask (this).Execute ();
			}
		}
	}
}


