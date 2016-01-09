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

using SQLite;
using Android.Util;


namespace GOWL
{
	[Activity (Label = "JourneyPreview")]			
	public class JourneyPreview : Activity
	{
		LayoutInflater inflater;

		// Variables for the unfold preview screen
		private View unfoldPreviewScreen;
		private ImageView unfoldImage;
		private TextView unfoldDescription;
		private TextView unfoldDurationText;
		private SeekBar unfoldDurationBar;

		private TextView TstartingDate;
		private TextView TendDate;

		private LinearLayout scrollViewMain;
		private LinearLayout journeyPartLtoR;
		private LinearLayout journeyPartRtoL;

		private Button newSearchButton;

		private static string Tag = "Journey Preview";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");
		private static string destinationDBPath = System.IO.Path.Combine(dbFolder, "gowl_destination.db");

		protected SQLiteConnection db;

		private int persons;
		private int standards;
		private int isActive;
		private int interestNature;
		private int interestCity;
		private int interestCulture;
		private int interestSportActivities;
		private int interestEvents;
		private int vacationTarget;

		private string startingDate;
		private string endDate;

		private string description;

		private bool isClicked = false;

		private int duration;
		private int partsCount;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.JourneyPreview);

			inflater = (LayoutInflater)this.GetSystemService (Context.LayoutInflaterService);

			TstartingDate = (TextView)FindViewById (Resource.Id.StartingDate);
			TendDate = (TextView)FindViewById (Resource.Id.EndDate);
			newSearchButton = (Button)FindViewById (Resource.Id.NewSearchButton);
			scrollViewMain = (LinearLayout)FindViewById (Resource.Id.ScrollViewMain);
			RetrieveDatafromDB ();

			TstartingDate.Text = startingDate;
			TendDate.Text = endDate;

			// drawing the necessary amount of destinations
			draw ();

			newSearchButton.Click += delegate {
				StartActivity(typeof(JourneyPreview));
				Finish();
			};

		}


		/************************************************|
		* 				DEFINING METHODS				 |
	 	* 												 |
		* 												 |
		*************************************************/

		//--------------retrieving user data and storing in variables---------------//
		private void RetrieveDatafromDB() {
			using (var connection = new SQLiteConnection(dbPath)) {

				var User = connection.Get<User> (1);

				/*foreach (var x in newUser) {
					Log.Info (Tag, "List of Users: " + x.Persons.ToString ());
				}*/

				persons = User.Persons;
				standards = User.Standards;
				isActive = User.IsActive;
				interestCity = User.InterestCity;
				interestCulture = User.InterestCulture;
				interestNature = 1;
				interestSportActivities = User.InterestSportActivities;
				interestEvents = User.InterestEvents;
				vacationTarget = 1;
				startingDate = User.StartingDate;
				endDate = User.EndDate;
				Log.Info (Tag, "Query - Result: " + persons.ToString ());

			}
		}

		//-----------Searching for right destination---------//
		private int findDestinationFoldPreview(int _rightOder) {

			int exception = 0;
			string command = "SELECT * FROM Destination";
			int rightOder = _rightOder;

			Random random = new Random ();
			int whichReturn = random.Next (1, 7);

			using (var db = new SQLiteConnection (destinationDBPath)) {

				var rowCount = db.Table<Destination> ().Count ();

				switch (whichReturn) {

				case 1:
					command = "SELECT * FROM Destination WHERE VacationTarget = ? AND InterestSportActivities = ? AND InterestEvents = ?";
					var imageResIdOne = db.Query<Destination> (command, vacationTarget, interestSportActivities, interestEvents);
					foreach (var s in  imageResIdOne) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							return imageResourceId;
						} else {
							findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 2:
					command = "SELECT * FROM Destination WHERE Standards = ? AND InterestCulture = ? AND InterestCity = ?";
					var imageResIdTwo = db.Query<Destination> (command, standards, interestCulture, interestCity);
					foreach (var s in  imageResIdTwo) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							return imageResourceId;
						} else {
							findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 3:
					command = "SELECT * FROM Destination WHERE VacationTarget = ? AND InterestCity = ? AND InterestNature = ?";
					var imageResIdThree = db.Query<Destination> (command, vacationTarget, interestCity, interestNature);
					foreach (var s in  imageResIdThree) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							return imageResourceId;
						} else {
							findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 4:
					command = "SELECT * FROM Destination WHERE IsActive = ? AND InterestCity = ? AND InterestNature = ?";
					var imageResIdFour = db.Query<Destination> (command, isActive, interestCity, interestNature);
					foreach (var s in  imageResIdFour) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							return imageResourceId;
						} else {
							findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 5:
					command = "SELECT * FROM Destination WHERE InterestNature = ? AND InterestCity = ? AND InterestCulture = ?";
					var imageResIdFive= db.Query<Destination> (command, interestNature, interestCity, interestCulture);
					foreach (var s in  imageResIdFive) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							return imageResourceId;
						} else {
							findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 6:
					command = "SELECT * FROM Destination WHERE VacationTarget = ? AND InterestEvents = ? AND InterestCulture = ?";
					var imageResIdSix= db.Query<Destination> (command, vacationTarget, interestEvents, interestCulture);
					foreach (var s in  imageResIdSix) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							return imageResourceId;
						} else {
							findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 7:
					command = "SELECT * FROM Destination WHERE Standards = ? AND InterestCulture = ?";
					var imageResIdSeven= db.Query<Destination> (command, standards, interestCulture);
					foreach (var s in  imageResIdSeven) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							return imageResourceId;
						} else {
							findDestinationFoldPreview (rightOder);
						}
					}
					break;

				}

			}

			return exception;
		}

		private string getDescription (string _description) {
			string description = _description;
			return description;
		}


		//----------drawing journey preview----------//
		private void draw() {

			// how long will the journey last?
			int tempStringStartDay = Convert.ToInt32(startingDate.Split('.')[0]);
			int tempStringStartMonth = Convert.ToInt32(startingDate.Split('.')[1]);
			//int tempStringStartYear = Convert.ToInt32(startingDate.Split('.')[2]);

			int tempStringEndDay = Convert.ToInt32(endDate.Split('.')[0]);
			int tempStringEndMonth = Convert.ToInt32(endDate.Split('.')[1]);
			//int tempStringEndYear = Convert.ToInt32(endDate.Split('.')[2]);

			if (tempStringStartMonth != tempStringEndMonth) {
				duration = (30 - tempStringStartDay) + tempStringEndDay;
			} else {
				duration = tempStringEndDay - tempStringStartDay;
			}

			if (duration <= 10) {
				partsCount = (int)Math.Round (duration / 1.5f);
			} else if (duration > 10 && duration <= 20) {
				partsCount = duration / 2;
			} else if (duration > 20 && duration <= 40) {
				partsCount = duration / 3;
			} else if (duration > 40) {
				partsCount = duration / 5;
			}


			// fill layout with destinations
			for (int i = 1; i < partsCount; i++) {

				if (i % 2 == 0) {
					LinearLayout partOne = (LinearLayout)inflater.Inflate (Resource.Drawable.PreviewRtoLTemplate, null);
					scrollViewMain.AddView (partOne, i);
					Log.Info (Tag, "executed: " + i.ToString());
					ImageButton buttonOne = (ImageButton)partOne.FindViewById (Resource.Id.ImageButtonPreviewRtoL);
					buttonOne.SetImageResource (findDestinationFoldPreview(i));
					clickingFoldImage (buttonOne, partOne);
				} else if (i % 2 == 1) {
					LinearLayout partTwo = (LinearLayout)inflater.Inflate (Resource.Drawable.PreviewLtoRTemplate, null);
					scrollViewMain.AddView (partTwo, i);
					Log.Info (Tag, "executed: " + i.ToString());
					ImageButton buttonTwo = (ImageButton)partTwo.FindViewById (Resource.Id.ImageButtonPreviewLtoR);
					buttonTwo.SetImageResource (findDestinationFoldPreview(i));
					clickingFoldImage (buttonTwo, partTwo);
				}
				 
			}
		}


		// clicking round imagebutton for getting unfolded preview about specific destination
		private void clickingFoldImage(ImageButton button, LinearLayout currentPart) {

			button.Click += delegate {
				if(!isClicked) {
					unfoldPreviewScreen = inflater.Inflate(Resource.Drawable.JourneyPreviewUnfold, null);
					unfoldImage = (ImageView) unfoldPreviewScreen.FindViewById(Resource.Id.UnfoldImage);
					unfoldDescription = (TextView) unfoldPreviewScreen.FindViewById (Resource.Id.UnfoldDescription);
					unfoldDescription.Text = description;
					currentPart.AddView(unfoldPreviewScreen, 0);
					isClicked = true;
				} else {
					currentPart.RemoveView(unfoldPreviewScreen);
					isClicked = false;
				}
			};

		}

	}
}

