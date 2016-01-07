
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
		private ImageView firstPersonImage;
		private ImageView secondPersonImage;
		private ImageView thirdPersonImage;
		private ImageView fourthPersonImage;
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
		private Button nextButtonPersonCount;
		private Button backButtonMoodboard;
		private Button backButtonExplanation;
		private Button backButtonDateStart;
		private Button backButtonDateEnd;
		private Button backButtonPersonCount;

		private ViewFlipper flipper;

		private bool isImageFitToScreen;
		private bool isImageChoosen;
		private bool isSelected;

		private static string Tag = "NewJourney";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");

		private int imagesSelected;
		private int phase;

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
			// Images - Interests - New Journey
			firstImage = (ImageView)FindViewById (Resource.Id.imageView1);
			secondImage = (ImageView)FindViewById (Resource.Id.imageView2);
			thirdImage = (ImageView)FindViewById (Resource.Id.imageView3);
			fourthImage = (ImageView)FindViewById (Resource.Id.imageView4);
			// Images - Person Count Symbols
			firstPersonImage = (ImageView) FindViewById (Resource.Id.imageView5);
			secondPersonImage = (ImageView)FindViewById (Resource.Id.imageView6);
			thirdPersonImage = (ImageView)FindViewById (Resource.Id.imageView7);
			fourthPersonImage = (ImageView)FindViewById (Resource.Id.imageView8);

			// Layout - Special Preference - New Journey
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
			backButtonExplanation = (Button)FindViewById (Resource.Id.backButtonChoosing);
			backButtonDateStart = (Button)FindViewById (Resource.Id.backButtonDateStart);
			backButtonDateEnd = (Button)FindViewById (Resource.Id.backButtonDateEnd);
			backButtonPersonCount = (Button)FindViewById (Resource.Id.backButtonPersonCount);

			// Buttons - Next Buttons
			nextButtonMoodboard = (Button) FindViewById (Resource.Id.nextButton);
			nextButtonChoosing = (Button)FindViewById (Resource.Id.nextButtonChoosing);
			nextButtonDateStart = (Button)FindViewById (Resource.Id.nextButtonDateStart);
			nextButtonDateEnd = (Button)FindViewById (Resource.Id.nextButtonDateEnd);
			nextButtonPersonCount = (Button)FindViewById (Resource.Id.nextButtonPersonCount);

			// Remove Like Buttons from View (o t in Fullscreen View)
			fullScreen.RemoveView (takeItFirst);
			fullScreen.RemoveView (takeItSecond);
			fullScreen.RemoveView (takeItThird);
			fullScreen.RemoveView (takeItFourth);

			// Flipper - Main Preferences Main
			flipper = (ViewFlipper) FindViewById (Resource.Id.viewFlipper1);

			// Date Picker
			dateStart = (DatePicker)FindViewById (Resource.Id.DatePickerStart);
			dateEnd = (DatePicker)FindViewById (Resource.Id.DatePickerEnd);

			//-----------Method Variables---------//
			// bools
			isImageFitToScreen = true;
			isImageChoosen = true;
			isSelected = true;

			// ints
			imagesSelected = 0;
			phase = 1;

			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/

			nextButtonChoosing.Click += delegate {
				flipper.ShowNext();
			};
			// Only for testing
			// ONLY FOR TESTING - DELETED WHEN RELEASED
			//-----------Database is created---------//
			createDatabase(dbPath);

			// Phase Handler
			next (Tag, nextButtonMoodboard);
			next (Tag, nextButtonPersonCount);
			next (Tag, nextButtonDateStart);
			next (Tag, nextButtonDateEnd);
			back (backButtonMoodboard);
			back (backButtonExplanation);
			back (backButtonDateStart);
			back (backButtonDateEnd);

			// Zoom Methods - Interests
			ImageZoom (firstImage, Tag, fullScreen, layoutFirstImage, takeItFirst);
			ImageZoom (secondImage, Tag, fullScreen, layoutSecondImage, takeItSecond);
			ImageZoom (thirdImage, Tag, fullScreen, layoutThirdImage, takeItThird);
			ImageZoom (fourthImage, Tag, fullScreen, layoutFourthImage, takeItFourth);
			ImageZoom (firstPersonImage, Tag, fullScreen, layoutFirstImage, takeItFirst);
			ImageZoom (secondPersonImage, Tag, fullScreen, layoutFirstImage, takeItFirst);
			ImageZoom (thirdPersonImage, Tag, fullScreen, layoutFirstImage, takeItFirst);
			ImageZoom (fourthPersonImage, Tag, fullScreen, layoutFirstImage, takeItFirst);
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
						if (imageView == firstPersonImage) {
							imageView.SetImageResource(Resource.Drawable.test_test);
						}
						else if (imageView == secondPersonImage) {

						}
						else if (imageView == thirdPersonImage) {

						} 
						else if (imageView == fourthPersonImage) {

						}
						isImageChoosen = false;
						if(imagesSelected < 1) {
							imagesSelected += 1;
							phase = 3;
						}
						Log.Info(Tag, "Image Choosen" + imagesSelected.ToString());
						definingTag(imageView, true);
					} else {
						if (imageView == firstPersonImage) {
							imageView.SetImageResource(Resource.Drawable.test);
						}
						else if (imageView == secondPersonImage) {

						}
						else if (imageView == thirdPersonImage) {

						} 
						else if (imageView == fourthPersonImage) {

						}
						if(imagesSelected > 0) {
							imagesSelected -= 1;
							phase = 2;
						}
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
						phase = 2;
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
						phase = 1;
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
				if (phase == 1 && fNextButton == nextButtonChoosing) {
					Log.Info(Tag, "Phase 1");
				}
				else if (phase == 2 && fNextButton == nextButtonMoodboard) {
					Log.Info(Tag, "Phase 2");
					flipper.ShowNext();
				}
				else if (phase == 3 && fNextButton == nextButtonPersonCount) {
					flipper.ShowNext();
					Log.Info(Tag, "Phase 3");
				}
				else if (phase == 4 && fNextButton == nextButtonDateStart) {
					onDateChanged(dateStart);
				}
				else if (phase == 5) {
					Log.Info(Tag, "Phase 5");
					onDateChanged(dateEnd);
				}
			};

		}

		//-----------one step back---------//
		private void back (Button backButton) {

			backButton.Click += delegate {
				if(phase == 1 && backButton == backButtonExplanation) {
					StartActivity(typeof(GowlMain));
					Finish();
				}
				else if(phase == 1 && backButton == backButtonMoodboard) {
					flipper.ShowPrevious();
				}
				else if(phase == 2 && backButton == backButtonPersonCount) {
					flipper.ShowPrevious();
					phase -= 1;
				}
				else if(phase==3 && backButton == backButtonDateStart) {
					flipper.ShowPrevious();
					phase -= 1;
				}
				else if(phase == 4 && backButton == backButtonDateEnd) {
					flipper.ShowPrevious();
					phase -= 1;
				}
				Log.Info (Tag, "Phase: " + phase.ToString());
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

