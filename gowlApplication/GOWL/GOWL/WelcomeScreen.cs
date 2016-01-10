
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
				StartActivity (typeof(NewJourneySpecificPreference));
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

			/*using (var db = new SQLiteConnection (destinationDBPath)) {

				db.DeleteAll<Destination> ();

				var rowZero = insertUpdateDestinationData(new Destination{ ID = 1, Name = string.Format("Kabelpark", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 3,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 1,
					InterestSportActivities = 4,
					VacationTarget = 1,
					Order = 3,
					Description = "Kabelpark in Hvide Sande - ein beliebte Wakeboardbahn.",
					ImageResID = Resource.Drawable;
				}, db);

				var rowOne = insertUpdateDestinationData(new Destination{ ID = 2, Name = string.Format("Blavandshuk Fyr", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 5,
					Description = "Blåvandshuk Fyr, der westlichste Leuchtturm Dänemarks.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowTwo = insertUpdateDestinationData(new Destination{ ID = 3, Name = string.Format("Skagen Bunker Museum", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 2,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 3,
					Description = "Skagen's Bunker Musem, gut erhaltener Bunker aus dem zweiten Weltkrieg.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowThree = insertUpdateDestinationData(new Destination{ ID = 4, Name = string.Format("Skagens Museum", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 2,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 3,
					Description = "Skagen's Museum, Kunst aus ganz Dänemark, beschäftigt sich vor allem mit den Skagen-Malern.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowFour = insertUpdateDestinationData(new Destination{ ID = 5, Name = string.Format("Nationalpark Thy", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 4,
					InterestCity = 1,
					InterestCulture = 2,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 2,
					Description = "Nationalpark Thy, an der Küste Dänemarks, bietet viele Aktivitäten und Sehenswürdigkeiten.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowFive = insertUpdateDestinationData(new Destination{ ID = 6, Name = string.Format("Strand von Skagen", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 2,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 3,
					Description = "Der Strand von Skagen bildet den nördlichsten Punkt Dänemarks, ein Ausflug wert.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowSix = insertUpdateDestinationData(new Destination{ ID = 7, Name = string.Format("Skagens Fischrestaurant", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 3,
					Description = "Skagens Fiskrestaurant - bietet eine ausgezeichnete Aussicht und Küche.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowSeven = insertUpdateDestinationData(new Destination{ ID = 8, Name = string.Format("Den Gemle By", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 3,
					InterestCulture = 4,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 4,
					Description = "Den Gamle By in Aarhus ist ein Freilichtmuseum über Kultur und Geschichte von Aarhus und Dänemark.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowEight = insertUpdateDestinationData(new Destination{ ID = 9, Name = string.Format("Nationalpark Mols Bjerge", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 4,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 1,
					InterestSportActivities = 2,
					VacationTarget = 1,
					Order = 2,
					Description = "Moljs Bjerge, ein Nationalpark mit großen Grünflächen und Wäldern, sehr flach, spiegelt gut die Fauna in Dänemark wieder.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowNine = insertUpdateDestinationData(new Destination{ ID = 10, Name = string.Format("Rosenborg Schloss", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 2,
					InterestNature = 4,
					InterestCity = 4,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 5,
					Description = "Rosenborg, Schloss von Kopenhagen, hier findet sich der Königsgarten. Hier kann man auch die dänischen Kronjuwelwen bewundern.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowTen = insertUpdateDestinationData(new Destination{ ID = 11, Name = string.Format("Die kleine Meerjungfrau", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 2,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 6,
					Description = "Die kleine Meerjungfrau - eine Bronzefigur an der Uferprominade von Kopenhagen. Vorbild des gleichnamigen Märchens.",
						ImageResID = Resource.Drawable.test
				}, db);

				var rowEleven = insertUpdateDestinationData(new Destination{ ID = 12, Name = string.Format("Wikingermuseum Foteviken", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 6,
					Description = "Museum Foteviken - ein Museum über die Kultur und Geschichte der Wikinger.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowTwelve = insertUpdateDestinationData(new Destination{ ID = 13, Name = string.Format("UFO Monument Ängelholm", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 5,
					Description = "UFO-Denkmal Ängelholm ist das erste Denkmal an eine vermeintliche UFO-Landung 1963.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowThirteen = insertUpdateDestinationData(new Destination{ ID = 14, Name = string.Format("Pio Country Club", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 2,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 7,
					Description = "Der Pio Country Club ist ein empfehlenswertes Restaurant, dass sich auf amerikanische Speisen spezialisiert hat.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowFourteen = insertUpdateDestinationData(new Destination{ ID = 15, Name = string.Format("Schloss Kronborg", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 4,
					InterestEvents = 3,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 5,
					Description = "Schloss Kronborg auf einer Insel Dänemarks ist durch seine Verbindung zu William Shakesspeare bekannt.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowFiveteen = insertUpdateDestinationData(new Destination{ ID = 16, Name = string.Format("Breezanddijk", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 1,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 4,
					Description = "Breezanddijk ist eine 12 Kilometer lange Straße die von beiden Seiten direkt von Meer umgeben ist.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowSixteen = insertUpdateDestinationData(new Destination{ ID = 17, Name = string.Format("Hamburger Hafen", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 3,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 1,
					Description = "Der Hamburger Hafen, der größte Hafen Deutschlands, wartet mit vielen Events und Einkehrmöglichkeiten auf. ",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowSeventeen = insertUpdateDestinationData(new Destination{ ID = 18, Name = string.Format("Romo", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 4,
					InterestCity = 1,
					InterestCulture = 4,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 5,
					Description = "Romo, die südlichste Insel Dänemarks bietet wunderschöne Dünen und Sandstrände.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowEightteen = insertUpdateDestinationData(new Destination{ ID = 19, Name = string.Format("Naturgewalten Sylt", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 2,
					InterestEvents = 3,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 1,
					Description = "Naturgewalten Sylt, ist ein Veranstaltungs und Ausstellungszentrum in Sylt, dass sich mit Meer- und Küstendynamik beschäftigt.",
					ImageResID = Resource.Drawable.test
				}, db);

				var rowNineteen = insertUpdateDestinationData(new Destination{ ID = 20, Name = string.Format("Sneglehuset", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 2,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 2,
					Description = "Das Sneglehuset in Thyboron ist ein Gebäudekomplex dekoriert mit Millionen Schneckenhäusern.",
					ImageResID = Resource.Drawable.test
				}, db);

			}*/

		}
	}
}

