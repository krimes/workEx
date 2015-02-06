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

namespace WorkEX
{
	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]			
	public class SplashActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			StartActivity(typeof(MainActivity));
		}
	}
}

