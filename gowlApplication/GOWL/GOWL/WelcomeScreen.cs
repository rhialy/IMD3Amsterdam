
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
			createDatabase(dbPath);
			createSystemUser ();
			RetrieveDatafromDB ();

			if (existingUser == true) {
				StartActivity (typeof(GowlMain));
			}

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

		//-----------Update one insert for the Database---------//
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
	}
}

