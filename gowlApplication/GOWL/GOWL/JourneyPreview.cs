
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
		private ImageView partOne;

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


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.JourneyPreview);

			partOne = (ImageView)FindViewById (Resource.Id.ImagePart1);
			RetrieveDatafromDB ();
			// Create your application here
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
				interestNature = User.InterestNature;
				interestSportActivities = User.InterestSportActivities;
				interestEvents = User.InterestEvents;

				Log.Info (Tag, "Query - Result: " + persons.ToString ());

			}
		}

		//-----------Searching for right destination---------//
		private void findDestination() {



		}
			

	}
}

