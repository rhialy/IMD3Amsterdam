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
		private View[] unfoldPreviewScreen = new View[12];
		private ImageView[] unfoldImage = new ImageView[12];
		private TextView[] unfoldDescription = new TextView[12];
		private TextView[] unfoldDurationText = new TextView[12];
		private SeekBar[] unfoldDurationBar = new SeekBar[12];

		private TextView TstartingDate;
		private TextView TendDate;

		private LinearLayout scrollViewMain;
		private LinearLayout[] journeyPartLtoR = new LinearLayout[12];
		private LinearLayout[] journeyPartRtoL = new LinearLayout[12];
		//private ImageView[] connectionLineLtoR = new ImageView[20];
		private ImageView[] connectionLine = new ImageView[12];
		private ImageView[] accomodationView = new ImageView[12];
		private ImageButton[] buttonOne = new ImageButton[12];
		private ImageButton[] buttonTwo = new ImageButton[12];
		private TextView[] specDate = new TextView[12];

		private Button newSearchButton;
		private Button backToMenuBtn;
		private Button finishPreviewBtn;
		private Button backToNewJourneyBtn;

		private static string Tag = "Journey Preview";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");
		private static string destinationDBPath = System.IO.Path.Combine(dbFolder, "gowl_destination.db");

		protected SQLiteConnection db;

		private int intStartDay;
		private int intStartMonth;
		private int intStartYear;
		private int intEndDay;
		private int intEndMonth;
		private int intEndYear;

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

		private string[] description = new string[12];
		private int[] imageUnfoldId = new int[12];
		private bool[] isUnfold = new bool[12];
		private int[] tempDateDay = new int[12];
		private int[] tempDateMonth = new int[12];
		private int[] tempDateYear = new int[12];
		private int[] progress = new int[12];
		private string[] newDate = new string[12];

		private bool isClicked = false;
		private bool hasStarted = false;

		private int duration;
		private int durationPerDestination;
		private int specDuration;
		private int partsCount;

		private int connectionLineColor;

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
			backToMenuBtn = (Button)FindViewById (Resource.Id.BackToMenu);
			backToNewJourneyBtn = (Button)FindViewById (Resource.Id.BackToNewJourney);
			finishPreviewBtn = (Button)FindViewById (Resource.Id.FinishPreviewButton);
			scrollViewMain = (LinearLayout)FindViewById (Resource.Id.ScrollViewMain);
			RetrieveDatafromDB ();

			TstartingDate.Text = "Start: " + startingDate;
			TendDate.Text = "Ende: " + endDate;

			// drawing the necessary amount of destinations
			if (!hasStarted) {
				draw ();
				hasStarted = true;
			}

			newSearchButton.Click += delegate {
				StartActivity(typeof(JourneyPreview));
				Finish();
			};

			backToMenuBtn.Click += delegate {
				StartActivity(typeof(GowlMain));
				Finish();
			};

			backToNewJourneyBtn.Click += delegate {
				StartActivity(typeof(NewJourneySpecificPreference));
				Finish();
			};

			finishPreviewBtn.Click += delegate {
				using (var connection = new SQLiteConnection(dbPath)) {

					var currUser = connection.Get<User> (1);

					currUser.ExistingJourney = true;

					connection.Update(currUser);
				}
				StartActivity(typeof(GowlMain));
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
			//int whichReturn = random.Next (1, 7);
			//for testing
			int whichReturn = 1;

			using (var db = new SQLiteConnection (destinationDBPath)) {

				var rowCount = db.Table<Destination> ().Count ();

				switch (whichReturn) {

				case 1:
					command = "SELECT * FROM Destination WHERE VacationTarget = ?";
					var imageResIdOne = db.Query<Destination> (command, whichReturn);
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
			intStartDay = Convert.ToInt32(startingDate.Split('.')[0]);
			intStartMonth = Convert.ToInt32 (startingDate.Split ('.') [1]) + 1;
			intStartYear = Convert.ToInt32(startingDate.Split('.')[2]);

			intEndDay = Convert.ToInt32(endDate.Split('.')[0]);
			intEndMonth = Convert.ToInt32 (endDate.Split ('.') [1]) + 1;
			intEndYear = Convert.ToInt32(endDate.Split('.')[2]);

			if (intStartMonth != intEndMonth) {
				duration = (30 - intStartDay) + intEndDay;
			} else {
				duration = intEndDay - intStartDay;
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

			durationPerDestination = duration / partsCount;
				
			// fill layout with destinations
			for (int i = 1; i <= partsCount; i++) {

				if (i == 1) {
					if (intStartDay + durationPerDestination > 30) {
						tempDateDay [i] = durationPerDestination - (30 - intStartDay);
						tempDateMonth [i] = intStartMonth + 1;
						tempDateYear [i] = intEndYear;
						newDate [i] = "Bis zum: " + tempDateDay [i].ToString () + "." + tempDateMonth [i].ToString () + "." + tempDateYear [i].ToString (); 
					} else {
						tempDateDay [i] = intStartDay + durationPerDestination;
						tempDateMonth [i] = intStartMonth;
						tempDateYear [i] = intStartYear;
						newDate [i] = "Bis zum: " + tempDateDay [i].ToString () + "." + tempDateMonth [i].ToString () + "." + tempDateYear [i].ToString (); 
					}
				} else {
					if (tempDateDay [i - 1] + durationPerDestination > 30) {
						tempDateDay [i] = durationPerDestination - (30 - tempDateDay [i - 1]);
						tempDateMonth [i] = tempDateMonth [i - 1] + 1;
						tempDateYear [i] = intEndYear;
						newDate [i] = "Bis zum: " + tempDateDay [i].ToString () + "." + tempDateMonth [i].ToString () + "." + tempDateYear [i].ToString (); 
					} else {
						tempDateDay [i] = tempDateDay [i - 1] + durationPerDestination;
						tempDateMonth [i] = tempDateMonth [i - 1];
						tempDateYear [i] = intStartYear;
						newDate [i] = "Bis zum: " + tempDateDay [i].ToString () + "." + tempDateMonth [i].ToString () + "." + tempDateYear [i].ToString (); 
					}
				}


				if (i % 2 == 0) {
					
					journeyPartLtoR[i] = (LinearLayout)inflater.Inflate (Resource.Drawable.PreviewLtoRTemplate, null);
					scrollViewMain.AddView (journeyPartLtoR[i], i+2);
					Log.Info (Tag, "executed: " + i.ToString());

					buttonTwo[i] = (ImageButton)journeyPartLtoR[i].FindViewById (Resource.Id.ImageButtonPreviewLtoR);
					specDate [i] = (TextView)journeyPartLtoR[i].FindViewById (Resource.Id.date);
					connectionLine [i] = (ImageView)journeyPartLtoR [i].FindViewById (Resource.Id.ConnectionLineLtoR);
					accomodationView [i] = (ImageView)journeyPartLtoR [i].FindViewById (Resource.Id.AccomodationView);

					specDate [i].Text = newDate [i];
					buttonTwo[i].SetImageResource (findDestinationFoldPreview(i));
					clickingFoldImage (buttonTwo[i], journeyPartLtoR[i], i);

					accomodation (i);

					switch (connectionLineColor) {
					case 1:
						connectionLine [i].SetImageResource (Resource.Drawable.orangeLnR);
						break;
					case 2:
						connectionLine [i].SetImageResource (Resource.Drawable.greyLnR);
						break;
					case 3:
						connectionLine [i].SetImageResource (Resource.Drawable.babyblueLnR);
						break;
					}

				} else if (i % 2 == 1) {
					
					journeyPartRtoL[i] = (LinearLayout)inflater.Inflate (Resource.Drawable.PreviewRtoLTemplate, null);
					scrollViewMain.AddView (journeyPartRtoL[i], i+2);
					Log.Info (Tag, "executed: " + i.ToString());

					buttonOne[i] = (ImageButton)journeyPartRtoL[i].FindViewById (Resource.Id.ImageButtonPreviewRtoL);
					specDate [i] = (TextView)journeyPartRtoL[i].FindViewById (Resource.Id.date);
					connectionLine [i] = (ImageView)journeyPartRtoL [i].FindViewById (Resource.Id.ConnectionLineRtoL);
					accomodationView [i] = (ImageView)journeyPartRtoL [i].FindViewById (Resource.Id.AccomodationView);

					specDate [i].Text = newDate [i];
					buttonOne[i].SetImageResource (findDestinationFoldPreview(i));
					clickingFoldImage (buttonOne[i], journeyPartRtoL[i], i);

					accomodation (i);

					switch (connectionLineColor) {
					case 1:
						connectionLine [i].SetImageResource (Resource.Drawable.orangeRnL);
						break;
					case 2:
						connectionLine [i].SetImageResource (Resource.Drawable.greyRnL);
						break;
					case 3:
						connectionLine [i].SetImageResource (Resource.Drawable.babyblueRnL);
						break;
					}
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

						unfoldDurationBar[rightOrder].Progress = durationPerDestination;
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

							progress[rightOrder] = e.Progress;

							if (e.Progress != durationPerDestination) {
								if (e.Progress > durationPerDestination) {
									tempDateDay[rightOrder] += 1;
									if(tempDateDay[rightOrder] > 30) {
										tempDateDay[rightOrder] = 1;
										tempDateMonth[rightOrder] += 1;
									}
									for(int i = rightOrder + 1; i <= partsCount; i++) {
										tempDateDay[i] = tempDateDay[i] + 1;
										if (tempDateDay[i] > 30) {
											tempDateDay[i] = 1;
											tempDateMonth[i] += 1;
										}
										if (tempDateMonth[i] > 12) {
											tempDateYear[i] += 1;
										}
									}
									for (int i = rightOrder; i <= partsCount; i++) {
										specDate[i].Text = "Bis zum: " + tempDateDay[i].ToString () + "." + tempDateMonth[i].ToString () + "." + tempDateYear[i].ToString ();
										TendDate.Text = "Ende: " + (tempDateDay[i]+1).ToString () + "." + tempDateMonth[i].ToString () + "." + tempDateYear[i].ToString ();
									}
								} else if (e.Progress < durationPerDestination) {
									tempDateDay[rightOrder] -= 1;
									if(tempDateDay[rightOrder] < 1) {
										tempDateDay[rightOrder] = 30;
										tempDateMonth[rightOrder] -= 1;
									}
									for(int i = rightOrder - 1; i > 0; i--) {
										tempDateDay[i] -= 1;
										if(tempDateDay[i] < 1) {
											tempDateDay[i] = 30;
											tempDateMonth[i] -= 1;
										}
										if (tempDateMonth[i] < 1) {
											tempDateYear[i] -= 1;
										}
									}
									for (int i = rightOrder; i > 0; i--) {
										specDate[i].Text = "Bis zum: " + tempDateDay[i].ToString () + "." + tempDateMonth[i].ToString () + "." + tempDateYear[i].ToString (); 
										TstartingDate.Text = "Start: " + (tempDateDay[i]-1).ToString () + "." + tempDateMonth[i].ToString () + "." + tempDateYear[i].ToString (); 
									}
								}
							}
						} 
							Log.Info(Tag,"specduration: " + specDuration.ToString());
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

		private void unfoldAccomodation(ImageView accomodation) {

			accomodation.Click += delegate {
				
			};

		}

		private void accomodation(int _order) {

			int order = _order;

			switch (order) {
			case 1:
				accomodationView [order].SetImageResource (Resource.Drawable.accomodationOrange);
				unfoldAccomodation(accomodationView[order]);
				connectionLineColor = 1;
				break;
			case 3:
				accomodationView [order].SetImageResource (Resource.Drawable.accomodationGrey);
				unfoldAccomodation(accomodationView[order]);
				connectionLineColor = 2;
				break;
			case 7:
				accomodationView [order].SetImageResource (Resource.Drawable.accomodationBabyblue);
				unfoldAccomodation(accomodationView[order]);
				connectionLineColor = 3;
				break;
			case 9:
				accomodationView [order].SetImageResource (Resource.Drawable.accomodationOrange);
				connectionLineColor = 1;
				break;
			case 11:
				accomodationView [order].SetImageResource (Resource.Drawable.accomodationGrey);
				unfoldAccomodation(accomodationView[order]);
				connectionLineColor = 2;
				break;
			case 15:
				accomodationView [order].SetImageResource (Resource.Drawable.accomodationBabyblue);
				unfoldAccomodation(accomodationView[order]);
				connectionLineColor = 3;
				break;
			default:
				accomodationView [order].Visibility = ViewStates.Invisible;
				break;
			}
		}

	}
}

