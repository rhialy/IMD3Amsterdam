
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
	[Activity (Label = "GOWL", MainLauncher = true, Icon = "@mipmap/icon")]			
	public class WelcomeScreen : Activity
	{
		private static string Tag = "WelcomeScreen";
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");
		private static string destinationDBPath = System.IO.Path.Combine(dbFolder, "gowl_destination.db");

		private Button startButton;

		private bool existingUser;
		private bool hasInserted;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.WelcomeScreen);
			startButton = (Button)FindViewById (Resource.Id.startButton);

			hasInserted = false;
		
			startButton.Click += delegate {
				settingExistingUser ();
				StartActivity(typeof(MainPreferencesPhaseOne));
			};

			/************************************************|
			* 				INVOKING METHODS				 |
		 	* 												 |
			* 												 |
			*************************************************/
			// Create Databse
			createUserDatabase (dbPath);
			createDestinationDatabase (destinationDBPath);
			createSystemUser ();
			UpdateDestinationDB ();
			RetrieveDatafromDB ();

			if (existingUser == true) {
				StartActivity (typeof(JourneyPreview));
			}

		}



		/************************************************|
		* 				DEFINING METHODS				 |
	 	* 												 |
		* 												 |
		*************************************************/

		//-----------Database is created---------//
		private string createUserDatabase(string path)
		{
			try
			{
				var connection = new SQLiteConnection(path);{
					connection.CreateTable<User>();
					return "Database created";
				}
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		//-----------Database is created---------//
		private string createDestinationDatabase(string path)
		{
			try
			{
				var connection = new SQLiteConnection(path);{
					connection.CreateTable<Destination>();
					return "Database created";
				}
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		//-----------Update one insert for the Database (USER)---------//
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

		//-----------Update one insert for the Database (DESTINATION)---------//
		private string insertUpdateDestinationData(Destination data, SQLiteConnection conn)
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

		//-----------sets an fake user for recognizing if welcome screen should be ignored or not---------//
		protected void createSystemUser() {

			var db = new SQLiteConnection (dbPath);

			var rowCount = db.Table<User> ().Count ();

			if (rowCount > 2) {
				db.DeleteAll<User> ();
			}

			if (!hasInserted) {
				Log.Info (Tag, "libber");
				var result = insertUpdateData(new User{ ID = 2, FirstName = string.Format("SystemUser", System.DateTime.Now.Ticks), 
					LastName = "Mr.Anderson", 
				}, db);
				hasInserted = true;
			} else {
				var rowcount = db.Delete(new User(){ID=1});
			}
		}

		//--------------sets system user to existing user--------------------//
		private void settingExistingUser() {
			using (var connection = new SQLiteConnection (dbPath)) {

				var rowCount = connection.Table<User> ().Count ();

				var presentUser = connection.Get<User> (2);

				if (rowCount <= 1) {
					presentUser.ExistingJourney = true;
					connection.Update (presentUser);
					//var Users = connection.Query<User>("UPDATE User SET Persons = ? WHERE ID = 1", persons);
					Log.Info (Tag, "User Data Updated");
				}
			}
		}

		//--------------retrieving user data and storing in variables---------------//
		private void RetrieveDatafromDB() {
			using (var connection = new SQLiteConnection(dbPath)) {

				var User = connection.Get<User> (2);

				existingUser = User.ExistingJourney;

			}
		}

		//--------------retrieving user data and storing in variables---------------//
		private void UpdateDestinationDB() {

			using (var db = new SQLiteConnection (destinationDBPath)) {

				db.DeleteAll<Destination> ();

				var rowZero = insertUpdateDestinationData(new Destination{ ID = 1, Name = string.Format("SystemUser", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Description = " Ein kleiner Testi mesti eintrag",
					ImageResID = Resource.Drawable.journeyTest
				}, db);

				var rowOne = insertUpdateDestinationData(new Destination{ ID = 1, Name = string.Format("SystemUser", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 2,
					InterestCity = 2,
					InterestCulture = 2,
					InterestEvents = 2,
					InterestSportActivities = 2,
					VacationTarget = 2,
					Description = "Zweiter Test test eintrag",
					ImageResID = Resource.Drawable.test
				}, db);

			}

		}
	}
}

