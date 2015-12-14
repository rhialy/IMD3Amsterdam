using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;

using SQLite;

namespace GOWL
{
	[Activity (Label = "GOWL")]
	public class MainPreferences : Activity
	{

		ImageView firstImage_1;
		ImageView secondImage_1;
		ImageView thirdImage_1;
		ImageView fourthImage_1;

		LinearLayout imagesUpper;
		LinearLayout imagesLower;
		LinearLayout fullScreen;

		Space upperSpace;
		Space lowerSpace;

		Button takeIt;

		bool isImageFitToScreen;
		bool hasStarted;
		bool isSelected;

		private static string Tag = "MainActivity";
		private string dbPath = Environment.DirectoryDocuments;

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
			// Images - xx - Main Preferences

			// Images - xx - Main Preferences

			// Layout - MainPreferences - Main Preferences
			imagesUpper = (LinearLayout)FindViewById (Resource.Id.LayoutImagesUpper);
			imagesLower = (LinearLayout)FindViewById (Resource.Id.LayoutImagesLower);
			fullScreen = (LinearLayout)FindViewById (Resource.Id.fullScreenLayout);

			// Space - Main Preferences
			upperSpace = (Space)FindViewById (Resource.Id.upperSpace);
			lowerSpace = (Space)FindViewById (Resource.Id.lowerSpace);

			// Button - Main Preferences
			takeIt = (Button)FindViewById (Resource.Id.takeIt);

			//-----------Method Variables---------//
			// bools
			isImageFitToScreen = true;
			hasStarted = true;

			if (hasStarted == true) {
				fullScreen.RemoveView (takeIt);
				hasStarted = false;
				isSelected = true;
			}

			ImageZoom (firstImage_1, Tag, fullScreen, imagesUpper, upperSpace);
			ImageZoom (secondImage_1, Tag, fullScreen, imagesUpper, upperSpace);
			ImageZoom (thirdImage_1, Tag, fullScreen, imagesLower, lowerSpace);
			ImageZoom (fourthImage_1, Tag, fullScreen, imagesLower, lowerSpace);



			/*firstImage.Click += ((object sender, System.EventArgs e) => {
				if (isImageFitToScreen) {
					isImageFitToScreen = false;
					//firstImage.SetAdjustViewBounds(false);
					firstImage.SetMaxHeight (1500);
					firstImage.SetMaxWidth (1500);
					firstImage.SetMinimumWidth (1500);
					firstImage.SetMinimumHeight (1500);
					Log.Info(Tag, "minimize");
				} else {
					isImageFitToScreen = true;
					//firstImage.SetAdjustViewBounds(true);
					firstImage.SetMinimumWidth (450);
					firstImage.SetMinimumHeight (450);
					firstImage.SetMaxHeight (450);
					firstImage.SetMaxWidth (450);
					Log.Info(Tag, "maximize");
				}
			});*/

		}

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


		protected void ImageZoom (ImageView imageView, string Tag, LinearLayout fullScreen, LinearLayout downScreen, Space partingSpace) {

			imageView.Click += ((object sender, System.EventArgs e) => {
				if (isImageFitToScreen) {
					isImageFitToScreen = false;
					//firstImage.SetAdjustViewBounds(false);
					downScreen.RemoveView(imageView);
					downScreen.RemoveView(partingSpace);
					imageView.SetMaxHeight (1500);
					imageView.SetMaxWidth (1500);
					imageView.SetMinimumWidth (1500);
					imageView.SetMinimumHeight (1500);
					fullScreen.AddView(imageView);
					fullScreen.AddView(takeIt);
					Log.Info(Tag, "minimize");
				} else {
					isImageFitToScreen = true;
					fullScreen.RemoveView(takeIt);
					fullScreen.RemoveView(imageView);
					//firstImage.SetAdjustViewBounds(true);
					imageView.SetMinimumWidth (450);
					imageView.SetMinimumHeight (450);
					imageView.SetMaxHeight (450);
					imageView.SetMaxWidth (450);
					downScreen.AddView(partingSpace);
					downScreen.AddView(imageView);
					Log.Info(Tag, "maximize");
				}
			});

			takeIt.Click += ((object sender, System.EventArgs e) => {
				if(isSelected) {
					imageView.SetColorFilter(Color.DimGray, PorterDuff.Mode.Lighten);
					isSelected = false;
				} else {
					imageView.SetColorFilter(null);
					isSelected = true;
				}
			});
		}
	}
}


