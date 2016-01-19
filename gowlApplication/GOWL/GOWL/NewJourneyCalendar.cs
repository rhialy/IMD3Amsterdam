
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

			/*backButtonDateEnd.Click += delegate {
				flipper.ShowPrevious();
			};*/


			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/

			// Listener for Date
			onDateChanged (dateStart);
			onDateChanged (dateEnd);

			// Transitions
			Transitions (backButtonDateEnd);
			Transitions (backButtonDateStart);
		}

		//-----------sets the tag in the user.cs for further utilization in the application---------//
		protected void settingTags() {

			string startingDate = startDay.ToString () + "." + startMonth.ToString () + "." + startYear.ToString ();
			string endDate = endDay.ToString () + "." + endMonth.ToString () + "." + endYear.ToString ();

			using (var connection = new SQLiteConnection (dbPath)) {

				var rowCount = connection.Table<User> ().Count ();

				var presentUser = connection.Get<User> (1);

				if (rowCount <= 2) {
					presentUser.StartingDate = startingDate;
					presentUser.EndDate = endDate;
					connection.Update (presentUser);
					Log.Info (Tag, "User Data Updated");
				}

			}
		}

		//-----------Saving Date when clicking next----------------//
		protected void onDateChanged(DatePicker view) {	

			nextButtonDateStart.Click += delegate {
				if(view == dateStart) {
					startYear = view.Year;
					startMonth = view.Month;
					startDay = view.DayOfMonth;
					Log.Info(Tag, "Current Day: " + startDay.ToString());	
					flipper.ShowNext();
				}
			};

			nextButtonDateEnd.Click += delegate {
				if(view == dateEnd) {
					endYear = view.Year;
					endMonth = view.Month;
					endDay = view.DayOfMonth;
					Log.Info(Tag, "Current Day - End: " + endDay.ToString());	
					settingTags();
					Intent intent = new Intent(this, typeof(JourneyPreview));
					intent.AddFlags(ActivityFlags.ClearTop);
					StartActivity(intent);
					//StartActivity(typeof(JourneyPreview));
					//Finish();
				}
			};
		}

		//-----------transitions without need to save data in db----------------//
		private void Transitions (Button transitionButton) {
															
			transitionButton.Click += delegate {
				if (transitionButton == backButtonDateEnd) {
					flipper.ShowPrevious();
				} else if (transitionButton == backButtonDateStart) {
					StartActivity(typeof(NewJourneyPersonCount));
					Finish();
				}
			};
		}


	}
}

