
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
using Android.Graphics;

namespace GOWL
{

	/************************************************|
	* This class sets the different preferences tags |
	* in the user-table (User.cs). After setting	 |
	* these tags this activity will be ignored		 |
	*************************************************/

	[Activity (Label = "GOWL")]
	public class NewJourney : Activity
	{
		private DatePicker dateStart;
		private DatePicker dateEnd;

		private ImageView firstImage;
		private ImageView secondImage;
		private ImageView thirdImage;
		private ImageView fourthImage;
		private ImageView choosenImage;

		private LinearLayout layoutFirstImage;
		private LinearLayout layoutSecondImage;
		private LinearLayout layoutThirdImage;
		private LinearLayout layoutFourthImage;
		private LinearLayout fullScreen;
		private LinearLayout mainPreferences;
		private LinearLayout imageChoosingExplanation;

		private Button takeItFirst;
		private Button takeItSecond;
		private Button takeItThird;
		private Button takeItFourth;
		private Button nextButtonMoodboard;
		private Button nextButtonChoosing;
		private Button nextButtonDateStart;
		private Button nextButtonDateEnd;
		private Button backButtonMoodboard;

		private ViewFlipper flipper;

		private bool isImageFitToScreen;
		private bool isImageChoosen;
		private bool hasStarted;
		private bool isSelected;
		private bool tagSet;

		private static string Tag = "NewJourney";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");

		private int imagesSelected;
		private int phase = 1;

		protected ImageView [] imageArray;

		// Variables for Database
		// Date
		private int startYear;
		private int startMonth;
		private int startDay;
		private int endYear;
		private int endMonth;
		private int endDay;
		// Preferences - Specific Activities

		// Person - Count
		private int persons;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			Log.Warn (Tag, "OnCreate");


			/************************************************|
			* 				DECLARING VARIABLES				 |
			* 					for global class use		 |
			* 												 |
			*************************************************/

			Log.Info (Tag, "Choosing Phase began");
			SetContentView(Resource.Layout.NewJourney);

			//--------Layout Variables---------//
			// Images - Interests - Main Preferences
			firstImage = (ImageView)FindViewById (Resource.Id.imageView1);
			secondImage = (ImageView)FindViewById (Resource.Id.imageView2);
			thirdImage = (ImageView)FindViewById (Resource.Id.imageView3);
			fourthImage = (ImageView)FindViewById (Resource.Id.imageView4);
			//imageArray = {firstImage, secondImage, thirdImage, fourthImage}

			// Layout - MainPreferences - Main Preferences
			layoutFirstImage = (LinearLayout)FindViewById (Resource.Id.layoutFirstImage);
			layoutSecondImage = (LinearLayout)FindViewById (Resource.Id.layoutSecondImage);
			layoutThirdImage = (LinearLayout)FindViewById (Resource.Id.layoutThirdImage);
			layoutFourthImage = (LinearLayout)FindViewById (Resource.Id.layoutFourthImage);
			fullScreen = (LinearLayout)FindViewById (Resource.Id.fullScreenLayout);
			mainPreferences = (LinearLayout)FindViewById (Resource.Id.MainPreferences);
			imageChoosingExplanation = (LinearLayout)FindViewById (Resource.Id.ImageChoosingExplanation);

			// Buttons - Moodboard
			takeItFirst = (Button)FindViewById (Resource.Id.takeItFirst);
			takeItSecond = (Button) FindViewById (Resource.Id.takeItSecond);
			takeItThird = (Button)FindViewById (Resource.Id.takeItThird); 
			takeItFourth = (Button)FindViewById (Resource.Id.takeItFourth);
			// Buttons - Back Buttons
			backButtonMoodboard = (Button)FindViewById (Resource.Id.backButton);
			// Buttons - Next Buttons
			nextButtonMoodboard = (Button) FindViewById (Resource.Id.nextButton);
			nextButtonChoosing = (Button)FindViewById (Resource.Id.nextButtonChoosing);
			nextButtonDateStart = (Button)FindViewById (Resource.Id.nextButtonDateStart);
			nextButtonDateEnd = (Button)FindViewById (Resource.Id.nextButtonDateEnd);
			// Remove Like Buttons from View (o t in Fullscreen View)
			fullScreen.RemoveView (takeItFirst);
			fullScreen.RemoveView (takeItSecond);
			fullScreen.RemoveView (takeItThird);
			fullScreen.RemoveView (takeItFourth);

			// Flipper - Main Preferences Main
			flipper = (ViewFlipper) FindViewById (Resource.Id.viewFlipper1);
			flipper.ShowNext ();

			// Date Picker
			dateStart = (DatePicker)FindViewById (Resource.Id.DatePickerStart);
			dateEnd = (DatePicker)FindViewById (Resource.Id.DatePickerEnd);

			//-----------Method Variables---------//
			// bools
			isImageFitToScreen = true;
			isImageChoosen = true;
			isSelected = true;
			hasStarted = true;

			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/

			nextButtonChoosing.Click += delegate {
				flipper.ShowPrevious();
			};
			// Only for testing
			// ONLY FOR TESTING - DELETED WHEN RELEASED
			//-----------Database is created---------//
			createDatabase(dbPath);

			// Phase Handler
			next (Tag, nextButtonMoodboard);

			// Zoom Methods - Interests
			ImageZoom (firstImage, Tag, fullScreen, layoutFirstImage, takeItFirst);
			ImageZoom (secondImage, Tag, fullScreen, layoutSecondImage, takeItSecond);
			ImageZoom (thirdImage, Tag, fullScreen, layoutThirdImage, takeItThird);
			ImageZoom (fourthImage, Tag, fullScreen, layoutFourthImage, takeItFourth);

		}


		/************************************************|
		* 				DEFINING METHODS				 |
	 	* 												 |
		* 												 |
		*************************************************/
		// ONLY FOR TESTING - DELETED WHEN RELEASED
		//-----------Database is created---------//
		private string createDatabase(string path)
		{
			try
			{
				var connection = new SQLiteAsyncConnection(path);{
					connection.CreateTableAsync<User>();
					return "Database created";
				}
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}


		//-----------Update one insert for the Database---------//
		private string insertUpdateData(User data, string path)
		{
			try
			{
				var db = new SQLiteConnection(path);
				if (db.Insert(data) != 0)
					db.Update(data);
				return "Single data file inserted or updated";
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		//-----------Insert an ArrayList and update Databse---------//
		private string insertUpdateAllData(IEnumerable<User> data, string path)
		{
			try
			{
				var db = new SQLiteConnection(path);
				if (db.InsertAll(data) != 0)
					db.UpdateAll(data);
				return "List of data inserted or updated";
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		//-----------clicking on images -> image on fullscreen---------//
		protected void ImageZoom (ImageView imageView, string Tag, LinearLayout fullScreen, LinearLayout downScreen, Button takeIt) {

			imageView.Click += ((object sender, System.EventArgs e) => {
				if(phase == 1) {
					Log.Info(Tag, "isImageFitToScreen: " + isImageFitToScreen.ToString());
					if (isImageFitToScreen) {
						downScreen.RemoveView(imageView);
						imageView.SetMaxHeight (1500);
						imageView.SetMaxWidth (1500);
						fullScreen.AddView(imageView);
						fullScreen.AddView(takeIt);
						Log.Info(Tag, "maximize");
						clickingImage(imageView, takeIt);
						isImageFitToScreen = false;
					} else {
						fullScreen.RemoveView(takeIt);
						fullScreen.RemoveView(imageView);
						imageView.SetMaxHeight (450);
						imageView.SetMaxWidth (450);
						downScreen.AddView(imageView);
						Log.Info(Tag, "minimize");
						isImageFitToScreen = true;
						isSelected = true;
					}
				}
				if(phase == 2) {
					if(isImageChoosen) {
						downScreen.SetMinimumWidth(550);
						downScreen.SetMinimumHeight(550);
						isImageChoosen = false;
						if(imagesSelected < 1) {
							imagesSelected += 1;
						}
						Log.Info(Tag, "Image Choosen" + imagesSelected.ToString());
						definingTag(imageView, true);
					} else {
						if(imagesSelected > 0) {
							imagesSelected -= 1;
						}
						downScreen.SetMinimumWidth (450);
						downScreen.SetMinimumHeight (450);
						isImageChoosen = true;
						Log.Info(Tag, "Image De-Choosen" + imagesSelected.ToString());
					}
				}
			});
		}

		//----------image is clicked and color filter is set---------//
		private void clickingImage(ImageView imageView, Button takeIt) {

			// TODO: If one image is zoomed in multiple times, this function is also executed this amount of times.
			//		 This function should only be executed one time no matter what. 

			takeIt.Click += ((object sender, System.EventArgs e) => {
				Log.Info(Tag, "takeIt Button: " + takeIt.ToString());
				if(isSelected && imagesSelected == 0 || isSelected && choosenImage == imageView) {
					if(imagesSelected < 1) {
						imagesSelected += 1;
					}
					if (takeIt == takeItFirst) {
						imageView.SetImageResource(Resource.Drawable.test_test);
					}
					if (takeIt == takeItSecond) {

					}
					if (takeIt == takeItThird) {
						imageView.SetImageResource(Resource.Drawable.test_test);
					}
					if (takeIt == takeItFourth) {

					}
					Log.Info(Tag, "Images Selected: " + imagesSelected.ToString());
					if(choosenImage != imageView) {
						choosenImage = imageView;
					} else {
						choosenImage = null;
					}
					definingTag(imageView, true);
					isSelected = false;
				} else if (!isSelected && imagesSelected == 1){
					if(imagesSelected > 0) {
						imagesSelected -= 1;
					}
					if (takeIt == takeItFirst) {
						imageView.SetImageResource(Resource.Drawable.test);
					}
					if (takeIt == takeItSecond) {

					}
					if (takeIt == takeItThird) {
						imageView.SetImageResource(Resource.Drawable.test);
					}
					if (takeIt == takeItFourth) {

					}
					Log.Info(Tag, "Images Selected: " + imagesSelected.ToString());
					definingTag(imageView, false);
					isSelected = true;
				}
				if (choosenImage != null) {
					Log.Info(Tag, "choosenImage: " + choosenImage.ToString());
				} else {
					Log.Info(Tag, "choosen Image: null");
				}
				Log.Info(Tag, "isSelected: " + isSelected.ToString());
			});
		}

		//-----------when image is selected check which image and sets bool for the setting tags - method ---------//
		private void definingTag(ImageView imageView, bool isSet) {

			// TODO: when isSet is true prepare variable for settingTags method
			// 		 when isSet is false clear the preparation
			if (phase == 1) {
				if (imageView == firstImage) {
					//hasStandards = 1;
				} else if (imageView == secondImage) {
					//hasStandards = 2;
				} else if (imageView == thirdImage) {
					//hasStandards = 3;
				} else if (imageView == fourthImage) {
					//hasStandards = 4;
				}
			} else if (phase == 2) {
				if (imageView == firstImage) {
					persons = 1;
				} else if (imageView == secondImage) {
					persons = 2;
				} else if (imageView == thirdImage) {
					persons = 3;
				} else if (imageView == fourthImage) {
					persons = 4;
				}
			}

		}

		//-----------sets the tag in the user.cs for further utilization in the application---------//
		protected void settingTags() {
			
			using (var connection = new SQLiteConnection (dbPath)) {

				var rowCount = connection.Table<User> ().Count ();
	
				var presentUser = connection.Get<User> (1);

				Log.Info (Tag, "rowCount: " + rowCount.ToString ());

				if (rowCount > 1) {
					Log.Info (Tag, "Database cleared");
					for (int i = rowCount; i > 0-1; i--) {
						var del = connection.Delete<User> (i);
						//var deleted = connection.Query<User> ("DELETE FROM User WHERE ID = ?", i);
						Log.Info (Tag, "for loop i: " + i.ToString ());
						Log.Info (Tag, "Rows deleted: " + del.ToString ());
					}
				}

				rowCount = connection.Table<User> ().Count ();

				if (rowCount <= 1) {
					presentUser.Persons = persons;
					connection.Update (presentUser);
					//var Users = connection.Query<User>("UPDATE User SET Persons = ? WHERE ID = 1", persons);
					Log.Info (Tag, "User Data Updated");
				}

			}
		}

		//-----------next step in main preferences setup---------//
		private void next (string Tag, Button fNextButton) {

			fNextButton.Click += (object sender, System.EventArgs e) => {
				if (imagesSelected == 1) {
					imagesSelected = 0;
					phase += 1;
					Log.Info(Tag, "Next Button Clicked");
				}
				if (phase == 1) {
					Log.Info(Tag, "Phase 1");
				}
				if (phase == 2) {
					firstImage.SetImageResource(Resource.Drawable.test);
					secondImage.SetImageResource(Resource.Drawable.test);
					thirdImage.SetImageResource(Resource.Drawable.test);
					fourthImage.SetImageResource(Resource.Drawable.test);
					Log.Info(Tag, "Phase 2");
				}
				if (phase == 3) {
					flipper.ShowNext();
					flipper.ShowNext();
					onDateChanged(dateStart);
					Log.Info(Tag, "Phase 3");
					//phase += 1;
				}
				if (phase == 4) {
					Log.Info(Tag, "Phase 4");
					onDateChanged(dateEnd);
				}
			};

		}
			
		protected void onDateChanged(DatePicker view) {	
			
			nextButtonDateStart.Click += delegate {
				startYear = dateStart.Year;
				startMonth = dateStart.Month;
				startDay = dateStart.DayOfMonth;
				Log.Info(Tag, "Current Day: " + startDay.ToString());	
				phase += 1;
				flipper.ShowNext();
			};

			nextButtonDateEnd.Click += delegate {
				endYear = dateEnd.Year;
				endMonth = dateEnd.Month;
				endDay = dateEnd.DayOfMonth;
				Log.Info(Tag, "Current Day - End: " + endDay.ToString());	
				settingTags();
				Log.Info(Tag, "Persons - Pre DB: " + persons.ToString());
				StartActivity(typeof(JourneyPreview));
			};

		} 

	}
}

