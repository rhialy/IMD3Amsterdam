
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
using SQLite;

namespace GOWL
{
	[Activity (Label = "NewJourneyCalendar")]			
	public class NewJourneyCalendar : Activity
	{
		private DatePicker dateStart;
		private DatePicker dateEnd;

		private Button nextButtonDateStart;
		private Button nextButtonDateEnd;
		private Button backButtonDateStart;
		private Button backButtonDateEnd;

		private ViewFlipper flipper;

		// Variables for Database
		// Date
		private int startYear;
		private int startMonth;
		private int startDay;
		private int endYear;
		private int endMonth;
		private int endDay;

		private static string Tag = "NewJourney";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.NewJourneyCalendar);
			/************************************************|
			* 				DECLARING VARIABLES				 |
			* 					for global class use		 |
			* 												 |
			*************************************************/
			// Date Picker
			dateStart = (DatePicker)FindViewById (Resource.Id.DatePickerStart);
			dateEnd = (DatePicker)FindViewById (Resource.Id.DatePickerEnd);

			// Buttons - Next Buttons
			nextButtonDateStart = (Button)FindViewById (Resource.Id.nextButtonDateStart);
			nextButtonDateEnd = (Button)FindViewById (Resource.Id.nextButtonDateEnd);

			// Buttons - Back Buttons
			backButtonDateStart = (Button)FindViewById (Resource.Id.backButtonDateStart);
			backButtonDateEnd = (Button)FindViewById (Resource.Id.backButtonDateEnd);

			// View Flipper
			flipper = (ViewFlipper) FindViewById (Resource.Id.viewFlipper1);

			backButtonDateEnd.Click += delegate {
				flipper.ShowPrevious();
			};


			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/

			// Listener for Date
			onDateChanged (dateStart);
			onDateChanged (dateEnd);

			// Transitions
			Transitions (nextButtonDateEnd);
			Transitions (backButtonDateStart);
		}

		//-----------sets the tag in the user.cs for further utilization in the application---------//
		protected void settingTags() {

			string startingDate = startDay.ToString () + "." + startMonth.ToString () + "." + startDay.ToString ();

			using (var connection = new SQLiteConnection (dbPath)) {

				var rowCount = connection.Table<User> ().Count ();

				var presentUser = connection.Get<User> (1);

				if (rowCount <= 2) {
					presentUser.Date = startingDate;
					connection.Update (presentUser);
					Log.Info (Tag, "User Data Updated");
				}

			}
		}

		//-----------Saving Date when clicking next----------------//
		protected void onDateChanged(DatePicker view) {	

			nextButtonDateStart.Click += delegate {
				startYear = dateStart.Year;
				startMonth = dateStart.Month;
				startDay = dateStart.DayOfMonth;
				Log.Info(Tag, "Current Day: " + startDay.ToString());	
				flipper.ShowNext();
			};

			nextButtonDateEnd.Click += delegate {
				endYear = dateEnd.Year;
				endMonth = dateEnd.Month;
				endDay = dateEnd.DayOfMonth;
				Log.Info(Tag, "Current Day - End: " + endDay.ToString());	
				settingTags();
				StartActivity(typeof(JourneyPreview));
			};
		}

		private void Transitions (Button transitionButton) {
			transitionButton.Click += delegate {
				if (transitionButton == nextButtonDateEnd) {
					settingTags();
					StartActivity(typeof(JourneyPreview));
				} else if (transitionButton == backButtonDateStart) {
					StartActivity(typeof(NewJourneyPersonCount));
					Finish();
				}
			};
		}


	}
}

