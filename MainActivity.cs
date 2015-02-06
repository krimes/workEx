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
using System.Threading.Tasks;



namespace WorkEX
{

	[Activity (Label = "WorkEX", Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		Adapters.BidsListAdapter taskList;
		IList<ListBids> tasks;
		ListView taskListView;
		ProgressBar prgBarMain;
		Button button;
		int listMainId;

		public static string UserId = "";
		public List<string> ListCatalog;

		private class UpdateMainTask : AsyncTask {
			private MainActivity _Main;
			private ListView List1;
			AlphaAnimation inAnimation;
			AlphaAnimation outAnimation;
//			IList<ListBids> Intasks;
//			Adapters.BidsListAdapter IntaskList;


			public UpdateMainTask(MainActivity a)
			{
				_Main = a;
				List1 = _Main.taskListView;
//				IntaskList = _Main.taskList;
//				Intasks = _Main.tasks;
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
					_Main.tasks = new List<ListBids>();
					_Main.tasks = WorkEX.GetJSON.ShowBidsByUserId ();
					_Main.taskList = new Adapters.BidsListAdapter(_Main, _Main.tasks);

					//Hook up our adapter to our ListView
					_Main.RunOnUiThread(delegate {
						_Main.taskListView.Adapter = _Main.taskList;
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
			if (button != null) {
				button.Click += (object sender, EventArgs e) => {
					if (ListCatalog == null) {
						ListCatalog = WorkEX.GetJSON.GetCatalog ();
					}
					var intent = new Intent (this, typeof(ActivityAddBids));
					intent.PutStringArrayListExtra ("Cate_List", ListCatalog);
					StartActivity (intent);
				};
			}
			// wire up task click handler
			if(taskListView != null) {
				taskListView.ItemLongClick += (object sender, AdapterView.ItemLongClickEventArgs e) => {
//					var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
					listMainId = e.Position;
//					taskDetails.PutExtra ("TaskID", tasks[e.Position].ID);
//					StartActivity (taskDetails);
					//String[] mActionName ={"Посмотреть", "Изменить", "Удалить"};

					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle("Выбирите действие"); // заголовок для диалога

					//builder.SetItems(mActionName, (EventHandler<DialogClickEventArgs>)null);
					builder.SetNegativeButton("Посмотреть",(EventHandler<DialogClickEventArgs>)null);
					builder.SetNeutralButton("Изменить",(EventHandler<DialogClickEventArgs>)null);
					builder.SetPositiveButton("Удалить",(EventHandler<DialogClickEventArgs>)null);


					var dialog = builder.Create();
					dialog.Show();
					//Ищем кнопки
					var EditBtn = dialog.GetButton((int)DialogButtonType.Neutral);
					var DeleteBtn = dialog.GetButton((int)DialogButtonType.Positive);
					var ShowBtn = dialog.GetButton((int)DialogButtonType.Negative);
					//Назначаем их
					ShowBtn.Click += (sender2, args) =>
					{
						// Don't dismiss dialog.
						Console.WriteLine("Действие просмотра");
						dialog.Dismiss();
					};
					EditBtn.Click += (sender2, args) =>
					{
						// Dismiss dialog.
						Console.WriteLine("Действие редактирования");
						dialog.Dismiss();
					};
					DeleteBtn.Click += async (sender2, args) => 
					{
						dialog.Dismiss();
						prgBarMain.Visibility = ViewStates.Visible;
						taskListView.Visibility = ViewStates.Gone;
						var result =  await Task<bool>.Factory.StartNew(() => GetJSON.DeleteBidsByUserId (tasks [e.Position].ID));

						new UpdateMainTask (this).Execute ();

					};

				};
			}
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


