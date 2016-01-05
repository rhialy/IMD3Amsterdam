using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;

using SQLite;
using System;
using System.Data;
using System.IO;
using System.Collections.Generic;

namespace GOWL
{
	
	/************************************************|
	* This class sets the different preferences tags |
	* in the user-table (User.cs). After setting	 |
	* these tags this activity will be ignored		 |
	*************************************************/

	[Activity (Label = "GOWL")]
	public class MainPreferences : Activity
	{

		private ImageView firstImage;
		private ImageView secondImage;
		private ImageView thirdImage;
		private ImageView fourthImage;

		private LinearLayout layoutFirstImage;
		private LinearLayout layoutSecondImage;
		private LinearLayout layoutThirdImage;
		private LinearLayout layoutFourthImage;
		private LinearLayout fullScreen;
		private LinearLayout mainPreferences;
		private LinearLayout imageChoosingExplanation;

		private Button takeIt;
		private Button nextButton;
		private Button backButton;
		private Button nextButtonChoosing;

		private ViewFlipper flipper;

		private bool isImageFitToScreen;
		private bool hasStarted;
		private bool isSelected;
		private bool tagSet;
		private bool hasInserted = false;

		private static string Tag = "MainActivity";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");

		private int imagesSelected;
		private int phase = 1;

		// Variables for defining the User Tags
		private int isActive; // 1 = not active | 2 = a little bit active | 3 = moderately active | 4 = very active
		private int hasStandards; // 1 = doesn't care | 2 = doesn't want to sleep in a tent | 3 = moderate standards | 4 = high standards | 5 = very high standards
		// following variables are defined as follow
		// 1 = not very interested | 2 = moderately interested | 3 = very interested
		private int interestNature; 
		private int interestCity;
		private int interestCulture;
		private int interestSportActivities;
		private int interestEvents;


		protected ImageView [] imageArray;

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
			SetContentView(Resource.Layout.MainPreferences);

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

			// Buttons - Main Preferences
			takeIt = (Button)FindViewById (Resource.Id.takeIt);
			backButton = (Button)FindViewById (Resource.Id.backButton);
			nextButton = (Button) FindViewById (Resource.Id.nextButton);
			nextButtonChoosing = (Button)FindViewById (Resource.Id.nextButtonChoosing);
			fullScreen.RemoveView (takeIt);

			// Flipper - Main Preferences Main
			flipper = (ViewFlipper) FindViewById (Resource.Id.viewFlipper1);
			flipper.ShowNext ();

			//-----------Method Variables---------//
			// bools
			isImageFitToScreen = true;
			isSelected = true;
			hasStarted = true;

			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/

			// Creating the Databse
			createDatabase(dbPath);

			nextButtonChoosing.Click += delegate {
				flipper.ShowPrevious();
			};

			// Phase Handler
			next(Tag);

			// Zoom Methods - Interests
			ImageZoom (firstImage, Tag, fullScreen, layoutFirstImage);
			ImageZoom (secondImage, Tag, fullScreen, layoutSecondImage);
			ImageZoom (thirdImage, Tag, fullScreen, layoutThirdImage);
			ImageZoom (fourthImage, Tag, fullScreen, layoutFourthImage);

		}


		/************************************************|
		* 				DEFINING METHODS				 |
	 	* 												 |
		* 												 |
		*************************************************/

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
		private string insertUpdateData(User data, SQLiteConnection conn)
		{
			try
			{
				var db = conn;
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
		protected void ImageZoom (ImageView imageView, string Tag, LinearLayout fullScreen, LinearLayout downScreen) {

			imageView.Click += ((object sender, System.EventArgs e) => {
				if (isImageFitToScreen) {
					downScreen.RemoveView(imageView);
					imageView.SetMaxHeight (1500);
					imageView.SetMaxWidth (1500);
					fullScreen.AddView(imageView);
					fullScreen.AddView(takeIt);
					Log.Info(Tag, "maximize");
					clickingImage(imageView);
					isImageFitToScreen = false;
				} else {
					fullScreen.RemoveView(takeIt);
					fullScreen.RemoveView(imageView);
					imageView.SetMaxHeight (450);
					imageView.SetMaxWidth (450);
					downScreen.AddView(imageView);
					Log.Info(Tag, "minimize");
					isImageFitToScreen = true;
				}
			});
		}

		//----------image is clicked and color filter is set---------//
		private void clickingImage(ImageView imageView) {
			
			takeIt.Click += ((object sender, System.EventArgs e) => {
				if(isSelected) {
					if(imagesSelected < 1) {
						imagesSelected += 1;
					}
					imageView.SetImageResource(Resource.Drawable.test_test);
					Log.Info(Tag, "Images Selected: " + imagesSelected.ToString());
					Log.Info(Tag, "set color filter");
					definingTag(imageView, true);
					isSelected = false;
				} else {
					if(imagesSelected > 0) {
						imagesSelected -= 1;
					}
					imageView.SetImageResource(Resource.Drawable.test);
					Log.Info(Tag, "Images Selected: " + imagesSelected.ToString());
					Log.Info(Tag, "remove color filter");
					definingTag(imageView, false);
					isSelected = true;
				}
			});
		}

		//-----------when image is selected check which image and sets bool for the setting tags - method ---------//
		private void definingTag(ImageView imageView, bool isSet) {

			// TODO: when isSet is true prepare variable for settingTags method
			// 		 when isSet is false clear the preparation
			if (phase == 1) {
				if (imageView == firstImage) {
					hasStandards = 1;
				} else if (imageView == secondImage) {
					hasStandards = 2;
				} else if (imageView == thirdImage) {
					hasStandards = 3;
				} else if (imageView == fourthImage) {
					hasStandards = 4;
				}
			} else if (phase == 2) {
				if (imageView == firstImage) {

				} else if (imageView == secondImage) {

				} else if (imageView == thirdImage) {

				} else if (imageView == fourthImage) {

				}
			} else if (phase == 3) {
				if (imageView == firstImage) {

				} else if (imageView == secondImage) {

				} else if (imageView == thirdImage) {

				} else if (imageView == fourthImage) {

				}
			}

		}

		//-----------sets the tag in the user.cs for further utilization in the application---------//
		protected void settingTags() {

			var db = new SQLiteConnection (dbPath);

			db.DeleteAll<User> ();

			if (!hasInserted) {
				Log.Info (Tag, "libber");
				var result = insertUpdateData(new User{ ID = 1, FirstName = string.Format("xxxxx", System.DateTime.Now.Ticks), 
					LastName = "Smith", 
					Standards = hasStandards,
					IsActive = isActive,
					InterestNature = interestNature,
					InterestCity = interestCity,
					InterestCulture = interestCulture,
					InterestSportActivities = interestSportActivities,
					InterestEvents = interestEvents
				}, db);
				hasInserted = true;
			} else {
				var rowcount = db.Delete(new User(){ID=1});
				settingTags ();
			}
		}

		//-----------next step in main preferences setup---------//
		private void next (string Tag) {
			
			nextButton.Click += (object sender, System.EventArgs e) => {
				if (imagesSelected == 1) {
					imagesSelected = 0;
					phase += 1;
					Log.Info(Tag, "Next Button Clicked");
				}
				if (phase == 1) {
					Log.Info(Tag, "Phase 1");
					Log.Info(Tag, nextButton.ToString());
				}
				if (phase == 2) {
					firstImage.SetImageResource(Resource.Drawable.test_test);
					secondImage.SetImageResource(Resource.Drawable.test_test);
					thirdImage.SetImageResource(Resource.Drawable.test_test);
					fourthImage.SetImageResource(Resource.Drawable.test_test);
					Log.Info(Tag, "Phase 2");
				}
				if (phase == 3) {
					firstImage.SetImageResource(Resource.Drawable.test);
					secondImage.SetImageResource(Resource.Drawable.test);
					thirdImage.SetImageResource(Resource.Drawable.test);
					fourthImage.SetImageResource(Resource.Drawable.test);
					Log.Info(Tag, "Phase 3");
				}
				if (phase == 4) {
					SetContentView(Resource.Layout.MainPreferencesName);
					nextButton = (Button) FindViewById(Resource.Id.nextButtonName);
					phase += 1;
					// TODO: How can Name be identified from already existing Smartphone Resources
				}
				if (phase >= 5) {
					settingTags();
					nextButton.Click += delegate {
						Log.Info (Tag, "nextActivity");
						StartActivity(typeof(GowlMain));
						//Finish();
					};
					//StartActivity(typeof(GowlMain));
				}
			};

		}

		private void settingChoosingPhase () {

			if (hasStarted == true) {


			}

		}
			
	}
}


