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
		private View[] unfoldPreviewScreen = new View[8];
		private ImageView[] unfoldImage = new ImageView[8];
		private TextView[] unfoldDescription = new TextView[8];
		private TextView[] unfoldDurationText = new TextView[8];
		private SeekBar[] unfoldDurationBar = new SeekBar[8];

		private TextView TstartingDate;
		private TextView TendDate;

		private LinearLayout scrollViewMain;
		private LinearLayout[] journeyPartLtoR = new LinearLayout[8];
		private LinearLayout[] journeyPartRtoL = new LinearLayout[8];
		private ImageButton[] buttonOne = new ImageButton[8];
		private ImageButton[] buttonTwo = new ImageButton[8];

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

		private string[] description = new string[8];
		private int[] imageUnfoldId = new int[8];
		private bool[] isUnfold = new bool[8];
		private bool isClicked = false;

		private int duration;
		private int specDuration;
		private int partsCount;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.JourneyPreview);

			inflater = (LayoutInflater)this.GetSystemService (Context.LayoutInflaterService);

			for (int i = 0; i < isUnfold.Length; i++) {
				isUnfold [i] = false;
			}

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

				persons = User.Persons;
				standards = User.Standards;
				isActive = User.IsActive;
				interestCity = User.InterestCity;
				interestCulture = User.InterestCulture;
				interestNature = User.InterestNature;
				interestSportActivities = User.InterestSportActivities;
				interestEvents = User.InterestEvents;
				vacationTarget = User.VacationTarget;
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
					command = "SELECT * FROM Destination WHERE VacationTarget = ?";
					var imageResIdOne = db.Query<Destination> (command, vacationTarget);
					foreach (var s in  imageResIdOne) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							description[rightOder] = s.Description;
							imageUnfoldId [rightOder] = s.ImageResID;
							return imageResourceId;
						} else {
							//findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 2:
					command = "SELECT * FROM Destination WHERE InterestCulture = ?";
					var imageResIdTwo = db.Query<Destination> (command, interestCulture);
					foreach (var s in  imageResIdTwo) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							description[rightOder] = s.Description;
							imageUnfoldId [rightOder] = s.ImageResID;
							return imageResourceId;
						} else {
							//findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 3:
					command = "SELECT * FROM Destination WHERE InterestCity = ?";
					var imageResIdThree = db.Query<Destination> (command, interestCity);
					foreach (var s in  imageResIdThree) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							description[rightOder] = s.Description;
							imageUnfoldId [rightOder] = s.ImageResID;
							return imageResourceId;
						} else {
							//findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 4:
					command = "SELECT * FROM Destination WHERE InterestNature = ?";
					var imageResIdFour = db.Query<Destination> (command, interestNature);
					foreach (var s in  imageResIdFour) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							description[rightOder] = s.Description;
							imageUnfoldId [rightOder] = s.ImageResID;
							return imageResourceId;
						} else {
							//findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 5:
					command = "SELECT * FROM Destination WHERE InterestCulture = ?";
					var imageResIdFive= db.Query<Destination> (command, interestCulture);
					foreach (var s in  imageResIdFive) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							description[rightOder] = s.Description;
							imageUnfoldId [rightOder] = s.ImageResID;
							return imageResourceId;
						} else {
							//findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 6:
					command = "SELECT * FROM Destination WHERE InterestEvents = ?";
					var imageResIdSix= db.Query<Destination> (command, interestEvents);
					foreach (var s in  imageResIdSix) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							description[rightOder] = s.Description;
							imageUnfoldId [rightOder] = s.ImageResID;
							return imageResourceId;
						} else {
							//findDestinationFoldPreview (rightOder);
						}
					}
					break;

				case 7:
					command = "SELECT * FROM Destination WHERE Standards = ?";
					var imageResIdSeven= db.Query<Destination> (command, standards);
					foreach (var s in  imageResIdSeven) {
						int imageResourceId = s.ImageResID;
						if (s.Order == rightOder) {
							description[rightOder] = s.Description;
							imageUnfoldId [rightOder] = s.ImageResID;
							return imageResourceId;
						} else {
							//findDestinationFoldPreview (rightOder);
						}
					}
					break;
				}
			}
			return exception;
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
					journeyPartLtoR[i] = (LinearLayout)inflater.Inflate (Resource.Drawable.PreviewLtoRTemplate, null);
					scrollViewMain.AddView (journeyPartLtoR[i], i);
					Log.Info (Tag, "executed: " + i.ToString());
					buttonTwo[i] = (ImageButton)journeyPartLtoR[i].FindViewById (Resource.Id.ImageButtonPreviewLtoR);
					buttonTwo[i].SetImageResource (findDestinationFoldPreview(i));
					clickingFoldImage (buttonTwo[i], journeyPartLtoR[i], i);
				} else if (i % 2 == 1) {
					journeyPartRtoL[i] = (LinearLayout)inflater.Inflate (Resource.Drawable.PreviewRtoLTemplate, null);
					scrollViewMain.AddView (journeyPartRtoL[i], i);
					Log.Info (Tag, "executed: " + i.ToString());
					buttonOne[i] = (ImageButton)journeyPartRtoL[i].FindViewById (Resource.Id.ImageButtonPreviewRtoL);
					buttonOne[i].SetImageResource (findDestinationFoldPreview(i));
					clickingFoldImage (buttonOne[i], journeyPartRtoL[i], i);
				}
				 
			}
		}


		// clicking round imagebutton for getting unfolded preview about specific destination
		private void clickingFoldImage(ImageButton button, LinearLayout currentPart, int _rightOrder) {

			int rightOrder = _rightOrder;

			button.Click += delegate {
				if(!isClicked) {

					if (isUnfold[rightOrder] == false) {
						unfoldPreviewScreen[rightOrder] = inflater.Inflate(Resource.Drawable.JourneyPreviewUnfold, null);
						unfoldImage[rightOrder] = (ImageView) unfoldPreviewScreen[rightOrder].FindViewById(Resource.Id.imageView1);
						unfoldDescription[rightOrder] = (TextView) unfoldPreviewScreen[rightOrder].FindViewById (Resource.Id.UnfoldDescription);
						unfoldDurationText[rightOrder] = (TextView) unfoldPreviewScreen[rightOrder].FindViewById (Resource.Id.UnfoldDuration);
						unfoldDurationBar[rightOrder] = (SeekBar) unfoldPreviewScreen[rightOrder].FindViewById(Resource.Id.UnfoldSeeker);
						unfoldDescription[rightOrder].Text = description[rightOrder];
						unfoldImage[rightOrder].SetImageResource(imageUnfoldId[rightOrder]);						
						currentPart.AddView(unfoldPreviewScreen[rightOrder], 0);
						isUnfold [rightOrder] = true;
					} else {

						unfoldPreviewScreen[rightOrder].Visibility = ViewStates.Visible;
					}

					unfoldDurationBar[rightOrder].ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
						if(e.FromUser && duration > 0) {
							unfoldDurationText[rightOrder].Text = string.Format("{0} Tage", e.Progress);
							specDuration += 1;
							Log.Info(Tag,"specduration: " + specDuration.ToString());
						}
					};
	
					isClicked = true;
				} else {
					unfoldPreviewScreen[rightOrder].Visibility = ViewStates.Gone;
					//currentPart.RemoveView(unfoldPreviewScreen);
					isClicked = false;
					duration = duration - specDuration;
					specDuration = 0;
					Log.Info(Tag, "duration: " + duration.ToString());
				}
			};

		}

	}
}

