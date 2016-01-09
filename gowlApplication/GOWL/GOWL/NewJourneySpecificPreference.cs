
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
	[Activity (Label = "NewJourneySpecificPreference")]			
	public class NewJourneySpecificPreference : Activity
	{
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

		private Button takeItFirst;
		private Button takeItSecond;
		private Button takeItThird;
		private Button takeItFourth;

		private Button nextButton;
		private Button backButton;
		private Button nextButtonExplanation;
		private Button backButtonExplanation;

		private ViewFlipper flipper;

		private static string Tag = "NewJourney";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");

		private int imagesSelected;
		private int vacationTarget;

		private bool isImageFitToScreen;
		private bool isSelected;
		private bool canBeSelected;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			/************************************************|
			* 				DECLARING VARIABLES				 |
			* 					for global class use		 |
			* 												 |
			*************************************************/

			Log.Info (Tag, "Choosing Phase began");
			SetContentView(Resource.Layout.NewJourneySpecificPreferences);
			//--------Layout Variables---------//
			// Images - Interests - New Journey
			firstImage = (ImageView)FindViewById (Resource.Id.imageView1);
			secondImage = (ImageView)FindViewById (Resource.Id.imageView2);
			thirdImage = (ImageView)FindViewById (Resource.Id.imageView3);
			fourthImage = (ImageView)FindViewById (Resource.Id.imageView4);

			// Buttons - Moodboard
			takeItFirst = (Button)FindViewById (Resource.Id.takeItFirst);
			takeItSecond = (Button) FindViewById (Resource.Id.takeItSecond);
			takeItThird = (Button)FindViewById (Resource.Id.takeItThird); 
			takeItFourth = (Button)FindViewById (Resource.Id.takeItFourth);

			// Buttons - Transitions
			nextButton = (Button) FindViewById (Resource.Id.nextButton);
			backButton = (Button)FindViewById (Resource.Id.backButton);
			nextButtonExplanation = (Button)FindViewById (Resource.Id.nextButtonChoosing);
			backButtonExplanation = (Button)FindViewById (Resource.Id.backButtonChoosing);

			// Layout - Special Preference - New Journey
			layoutFirstImage = (LinearLayout)FindViewById (Resource.Id.layoutFirstImage);
			layoutSecondImage = (LinearLayout)FindViewById (Resource.Id.layoutSecondImage);
			layoutThirdImage = (LinearLayout)FindViewById (Resource.Id.layoutThirdImage);
			layoutFourthImage = (LinearLayout)FindViewById (Resource.Id.layoutFourthImage);
			fullScreen = (LinearLayout)FindViewById (Resource.Id.fullScreenLayout);

			// Flipper - Specific Preferences - New Journey
			flipper = (ViewFlipper) FindViewById (Resource.Id.viewFlipper1);

			//-----------Method Variables---------//
			// bools
			isImageFitToScreen = true;
			isSelected = true;
			canBeSelected = false;

			// ints
			imagesSelected = 0;

			// Remove Like Buttons from View (o t in Fullscreen View)
			fullScreen.RemoveView (takeItFirst);
			fullScreen.RemoveView (takeItSecond);
			fullScreen.RemoveView (takeItThird);
			fullScreen.RemoveView (takeItFourth);

			nextButtonExplanation.Click += delegate {
				flipper.ShowNext();
			};
			backButton.Click += delegate {
				flipper.ShowPrevious();
			};

			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/

			// Zoom Methods - Interests
			ImageZoom (firstImage, Tag, fullScreen, layoutFirstImage, takeItFirst);
			ImageZoom (secondImage, Tag, fullScreen, layoutSecondImage, takeItSecond);
			ImageZoom (thirdImage, Tag, fullScreen, layoutThirdImage, takeItThird);
			ImageZoom (fourthImage, Tag, fullScreen, layoutFourthImage, takeItFourth);

			// Like Methods - Interests
			clickingImage (firstImage, takeItFirst);
			clickingImage (secondImage, takeItSecond);
			clickingImage (thirdImage, takeItThird);
			clickingImage (fourthImage, takeItFourth);

			// Transition Methods
			Transition (nextButton);
			Transition (backButtonExplanation);
		}


		//-----------clicking on images -> image on fullscreen---------//
		protected void ImageZoom (ImageView imageView, string Tag, LinearLayout fullScreen, LinearLayout downScreen, Button takeIt) {

			imageView.Click += ((object sender, System.EventArgs e) => {
				Log.Info(Tag, "isImageFitToScreen: " + isImageFitToScreen.ToString());
				if (isImageFitToScreen) {
					downScreen.RemoveView(imageView);
					imageView.SetMaxHeight (1500);
					imageView.SetMaxWidth (1500);
					fullScreen.AddView(imageView);
					fullScreen.AddView(takeIt);
					Log.Info(Tag, "maximize");
					canBeSelected = true;
					isImageFitToScreen = false;
				} else {
					fullScreen.RemoveView(takeIt);
					fullScreen.RemoveView(imageView);
					imageView.SetMaxHeight (450);
					imageView.SetMaxWidth (450);
					downScreen.AddView(imageView);
					Log.Info(Tag, "minimize");
					canBeSelected = false;
					isImageFitToScreen = true;
				}
			});
		}


		//----------image is clicked and color filter is set---------//
		private void clickingImage(ImageView imageView, Button takeIt) {

			// TODO: If one image is zoomed in multiple times, this function is also executed this amount of times.
			//		 This function should only be executed one time no matter what. 

			takeIt.Click += ((object sender, System.EventArgs e) => {
				Log.Info(Tag, "takeIt Button: " + takeIt.ToString());
				if(isSelected && canBeSelected) {
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
				} else if (!isSelected && canBeSelected){
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
			if (isSet == true) {
				if (imageView == firstImage) {
					vacationTarget = 1;
				} else if (imageView == secondImage) {
					vacationTarget = 2;
				} else if (imageView == thirdImage) {
					vacationTarget = 3;
				} else if (imageView == fourthImage) {
					vacationTarget = 4;
				}
			} else {
				if (imageView == firstImage) {
					vacationTarget = 0;
				} else if (imageView == secondImage) {
					vacationTarget = 0;
				} else if (imageView == thirdImage) {
					vacationTarget = 0;
				} else if (imageView == fourthImage) {
					vacationTarget = 0;
				}
			}
		}


		//-----------sets the tag in the user.cs for further utilization in the application---------//
		protected void settingTags() {

			using (var connection = new SQLiteConnection (dbPath)) {

				var rowCount = connection.Table<User> ().Count ();

				var presentUser = connection.Get<User> (1);

				Log.Info (Tag, "rowCount: " + rowCount.ToString ());

				if (rowCount > 2) {
					Log.Info (Tag, "Database cleared");
					for (int i = rowCount; i > 0-1; i--) {
						var del = connection.Delete<User> (i);
						//var deleted = connection.Query<User> ("DELETE FROM User WHERE ID = ?", i);
						Log.Info (Tag, "for loop i: " + i.ToString ());
						Log.Info (Tag, "Rows deleted: " + del.ToString ());
					}
				}

				rowCount = connection.Table<User> ().Count ();

				if (rowCount <= 2) {
					presentUser.VacationTarget = vacationTarget;
					connection.Update (presentUser);
					//var Users = connection.Query<User>("UPDATE User SET Persons = ? WHERE ID = 1", persons);
					Log.Info (Tag, "User Data Updated");
				}

			}
		}

		private void Transition (Button transitionButton) {
			transitionButton.Click += delegate {
				if (transitionButton == backButtonExplanation) {
					StartActivity(typeof(GowlMain));
					Finish();
				} else if (transitionButton == nextButton) {
					settingTags();
					StartActivity(typeof(NewJourneyPersonCount));
				}
			};
		}
	}
}

