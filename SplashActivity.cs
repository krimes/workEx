//
//  SplashActivity.cs
//
//  Author:
//       Krimes <neolus2@ya.ru>
//
//  Copyright (c) 2015 Krimes
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//

using Android.App;
using Android.OS;
using Android.Webkit;
using System.Timers;

namespace WorkEX
{
//	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]			
	[Activity(Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen", MainLauncher = true, NoHistory = true)]			
	public class SplashActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.splash);
			WebView web1 = FindViewById<WebView> (Resource.Id.webView1);
			WebSettings webSettings = web1.Settings;
			webSettings.JavaScriptEnabled = true;
			web1.LoadUrl("file:///android_asset/index.html");
			//web1.LoadUrl("http://codepen.io/squrler/full/WbXxjx/");

		}
		protected override void OnResume ()
		{
			base.OnResume ();

			Timer tmrShow;
			tmrShow = new Timer();
			tmrShow.Interval =5000;
			tmrShow.Enabled = true;
			tmrShow.Elapsed += (object sender, ElapsedEventArgs e) => {
				StartActivity (typeof(MainActivity));
				tmrShow.Enabled = false;
			};
			//Thread.Sleep (1000);
			// Create your application here
			//StartActivity (typeof(MainActivity));
		}
	}
}

