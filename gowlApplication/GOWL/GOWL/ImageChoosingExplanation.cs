
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
	[Activity (Label = "ImageChoosingExplanation")]			
	public class ImageChoosingExplanation : Activity
	{

		private Button nextButton;
		private int situation;

		public ImageChoosingExplanation (int _situation) {
			situation = _situation;
		}


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (ImageChoosingExplanation);
			nextButton = (Button)FindViewById (Resource.Id.nextButtonChoosing);

			nextButton.Click += (object sender, EventArgs e) => {
				if (situation == 1) {
					StartActivity(typeof(MainPreferences));
				}
				if (situation == 2)  {
					StartActivity(typeof(NewJourney));
				}
			};

			// Create your application here
		}
	}
}

