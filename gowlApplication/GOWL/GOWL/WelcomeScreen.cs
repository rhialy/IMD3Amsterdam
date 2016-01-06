﻿
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

namespace GOWL
{
	[Activity (Label = "GOWL", MainLauncher = true, Icon = "@mipmap/icon")]			
	public class WelcomeScreen : Activity
	{

		Button startButton;
		private int situation;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.WelcomeScreen);

			startButton = (Button)FindViewById (Resource.Id.startButton);
			situation = 1;

			startButton.Click += delegate {
				StartActivity(typeof(GowlMain));
			};
		}
	}
}

