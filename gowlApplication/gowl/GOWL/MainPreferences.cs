using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;

using SQLite;

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

		ImageView firstImage_1;
		ImageView secondImage_1;
		ImageView thirdImage_1;
		ImageView fourthImage_1;

		LinearLayout layoutFirstImage;
		LinearLayout layoutSecondImage;
		LinearLayout layoutThirdImage;
		LinearLayout layoutFourthImage;
		LinearLayout fullScreen;

		Button takeIt;
		Button nextButton;
		Button backButton;

		private bool isImageFitToScreen;
		private bool hasStarted;
		private bool isSelected;
		private bool tagSet;

		private static string Tag = "MainActivity";
		private string dbPath = Environment.DirectoryDocuments;

		private int imagesSelected;
		private int phase;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			createDatabase (dbPath);

			Log.Warn (Tag, "OnCreate");
			SetContentView (Resource.Layout.MainPreferences);

			/************************************************|
			 * 				DECLARING VARIABLES				 |
			 * 					for global class use		 |
			 * 												 |
			 * **********************************************/

			//--------Layout Variables---------//
			// Images - Interests - Main Preferences
			firstImage_1 = (ImageView)FindViewById (Resource.Id.imageView1);
			secondImage_1 = (ImageView)FindViewById (Resource.Id.imageView2);
			thirdImage_1 = (ImageView)FindViewById (Resource.Id.imageView3);
			fourthImage_1 = (ImageView)FindViewById (Resource.Id.imageView4);

			// Images - Activities - Main Preferences

			// Images - Standards - Main Preferences

			// Layout - MainPreferences - Main Preferences
			layoutFirstImage = (LinearLayout)FindViewById (Resource.Id.layoutFirstImage);
			layoutSecondImage = (LinearLayout)FindViewById (Resource.Id.layoutSecondImage);
			layoutThirdImage = (LinearLayout)FindViewById (Resource.Id.layoutThirdImage);
			layoutFourthImage = (LinearLayout)FindViewById (Resource.Id.layoutFourthImage);
			fullScreen = (LinearLayout)FindViewById (Resource.Id.fullScreenLayout);

			// Buttons - Main Preferences
			takeIt = (Button)FindViewById (Resource.Id.takeIt);
			nextButton = (Button)FindViewById (Resource.Id.nextButton);
			backButton = (Button)FindViewById (Resource.Id.backButton);

			//-----------Method Variables---------//
			// bools
			isImageFitToScreen = true;
			hasStarted = true;

			if (hasStarted == true) {
				fullScreen.RemoveView (takeIt);
				isSelected = true;
				hasStarted = false;
			}

			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/

			// Zoom Methods - Interests
			ImageZoom (firstImage_1, Tag, fullScreen, layoutFirstImage);
			ImageZoom (secondImage_1, Tag, fullScreen, layoutSecondImage);
			ImageZoom (thirdImage_1, Tag, fullScreen, layoutThirdImage);
			ImageZoom (fourthImage_1, Tag, fullScreen, layoutFourthImage);

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
					isSelected = true;
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
					imagesSelected += 1;
					imageView.SetColorFilter(Color.DimGray, PorterDuff.Mode.Lighten);
					Log.Info(Tag, "set color filter");
					definingTag(imageView, true);
					isSelected = false;
				} else {
					imagesSelected -= 1;
					imageView.SetColorFilter(null);
					Log.Info(Tag, "remove color filter");
					definingTag(imageView, false);
				}
			});
		}

		//-----------when image is selected check which image and sets bool for the setting tags - method ---------//
		private void definingTag(ImageView imageView, bool isSet) {

			// TODO: when isSet is true prepare variable for settingTags method
			// 		 when isSet is false clear the preparation

			if (imageView == firstImage_1) {
				
			} else if (imageView == secondImage_1) {

			} else if (imageView == thirdImage_1) {

			} else if (imageView == fourthImage_1) {

			}

		}

		//-----------sets the tag in the user.cs for further utilization in the application---------//
		protected void settingTags(bool isSet) {

		}

		//-----------next step in main preferences setup---------//
		private void next (Button nextButton) {
			
			nextButton.Click += (object sender, System.EventArgs e) => {
				if (imagesSelected == 1) {
					imagesSelected = 0;
					phase += 1;
				}
				if (phase > 4) {
					// Name and few other infos or immediately to GowlMain acitivity?
				}
			};

		}
	}
}


