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
using Android.Util;
using Android.Graphics;

namespace GOWL
{
	[Activity (Label = "GOWL")]			
	public class GowlMain : Activity
	{

		private bool existingJourney; 

		private int phase;

		private ViewFlipper flipper;

		private Button newJourneyBtn;
		private Button userDataBtn;
		private Button backUserDataBtn;
		private Button resetBtn;

		private static string Tag = "GowlMain";

		public NewJourney newJourney;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Main);

			newJourneyBtn = (Button)FindViewById (Resource.Id.NewJourneyButton);
			userDataBtn = (Button)FindViewById (Resource.Id.UserDataButton);
			backUserDataBtn = (Button)FindViewById (Resource.Id.backButtonUserData);
			resetBtn = (Button)FindViewById (Resource.Id.NewMainPreferences);
			flipper = (ViewFlipper)FindViewById (Resource.Id.viewFlipper1);




			existingJourney = false;

			if (existingJourney == true) {
				SetContentView (Resource.Layout.ExistingJourney);
				Log.Info (Tag, "existing Journey false");
			}

			Menu ();
				

		}


		private void Menu() {
			Log.Info (Tag, newJourneyBtn.ToString ());
			newJourneyBtn.Click += ((object sender, System.EventArgs e) => {
				Log.Info(Tag, "Clicked New Journey");
				StartActivity(typeof(NewJourney));
			});

			userDataBtn.Click += (sender, e) => {
				flipper.ShowNext();
			};

			backUserDataBtn.Click += (object sender, EventArgs e) => {
				flipper.ShowPrevious();
			};

			resetBtn.Click += (object sender, EventArgs e) => {
				StartActivity(typeof(MainPreferences));
			};

		}
			
	}
}

