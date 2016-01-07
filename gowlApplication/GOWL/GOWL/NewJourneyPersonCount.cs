
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
	[Activity (Label = "NewJourneyPersonCount")]			
	public class NewJourneyPersonCount : Activity
	{
		private ImageView firstPersonImage;
		private ImageView secondPersonImage;
		private ImageView thirdPersonImage;
		private ImageView fourthPersonImage;
		private ImageView choosenImage;

		private Button nextButton;
		private Button backButton;

		private static string Tag = "NewJourney";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");

		private int imagesSelected;
		private int persons;

		private bool isImageChoosen;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.NewJourneyPersonCount);
			/************************************************|
			* 				DECLARING VARIABLES				 |
			* 					for global class use		 |
			* 												 |
			*************************************************/
			// Images - Person Count Symbols
			firstPersonImage = (ImageView)FindViewById (Resource.Id.imageView5);
			secondPersonImage = (ImageView)FindViewById (Resource.Id.imageView6);
			thirdPersonImage = (ImageView)FindViewById (Resource.Id.imageView7);
			fourthPersonImage = (ImageView)FindViewById (Resource.Id.imageView8);

			// Transitions - Buttons
			nextButton = (Button)FindViewById (Resource.Id.nextButtonPersonCount);
			backButton = (Button)FindViewById (Resource.Id.backButtonPersonCount);

			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/

			// Select functions for person symbols
			SelectSymbol (firstPersonImage);
			SelectSymbol (secondPersonImage);
			SelectSymbol (thirdPersonImage);
			SelectSymbol (fourthPersonImage);

			// Transitions
			Transition (nextButton);
			Transition (backButton);

		}


		//-----------clicking on images -> image on fullscreen---------//
		protected void SelectSymbol (ImageView imageView) {

			imageView.Click += ((object sender, System.EventArgs e) => {
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
					}
					isImageChoosen = true;
					definingTag(imageView, false);
					Log.Info(Tag, "Image De-Choosen" + imagesSelected.ToString());
				}
			});
		}

		//-----------when image is selected check which image and sets bool for the setting tags - method ---------//
		private void definingTag(ImageView imageView, bool isSet) {

			if (isSet == true) {
				if (imageView == firstPersonImage) {
					persons = 1;
				} else if (imageView == secondPersonImage) {
					persons = 2;
				} else if (imageView == thirdPersonImage) {
					persons = 3;
				} else if (imageView == fourthPersonImage) {
					persons = 4;
				}
			} else {
				if (imageView == firstPersonImage) {
					persons = 0;
				} else if (imageView == secondPersonImage) {
					persons = 0;
				} else if (imageView == thirdPersonImage) {
					persons = 0;
				} else if (imageView == fourthPersonImage) {
					persons = 0;
				}
			}
		}

		//-----------sets the tag in the user.cs for further utilization in the application---------//
		protected void settingTags() {

			using (var connection = new SQLiteConnection (dbPath)) {

				var rowCount = connection.Table<User> ().Count ();

				var presentUser = connection.Get<User> (1);

				Log.Info (Tag, "rowCount: " + rowCount.ToString ());

				if (rowCount <= 2) {
					presentUser.Persons = persons;
					connection.Update (presentUser);
					Log.Info (Tag, "User Data Updated");
				}

			}
		}

		//---------------transitioning back and forth------------------//
		private void Transition(Button transitionButton) {

			transitionButton.Click += delegate {
				if (transitionButton == nextButton) {
					settingTags();
					StartActivity(typeof(NewJourneyCalendar));
				} else if (transitionButton == backButton) {
					StartActivity(typeof(NewJourneySpecificPreference));
					Finish();
				}
			};

		}

	}
}

