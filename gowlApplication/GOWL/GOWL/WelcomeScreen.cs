
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
		
		/************************************************|
		* 				DECLARING VARIABLES				 |
		* 					for global class use		 |
		* 												 |
		*************************************************/

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
			// Create Database
			createUserDatabase (dbPath);
			createDestinationDatabase (destinationDBPath);
			createSystemUser ();
			UpdateDestinationDB ();
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
				var rowcount = db.Delete(new User(){ID=2});
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

				var rowZero = insertUpdateDestinationData(new Destination{ ID = 1, Name = string.Format("Kabelpark", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 4,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 2,
					InterestSportActivities = 4,
					VacationTarget = 1,
					Order = 3,
					Description = "Kabelpark in Hvide Sande - ein beliebte Wakeboardbahn.",
					ImageResID = Resource.Drawable.kabelpark
				}, db);

				var rowOne = insertUpdateDestinationData(new Destination{ ID = 2, Name = string.Format("Blavandshuk Fyr", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 4,
					Order = 5,
					Description = "Blåvandshuk Fyr, der westlichste Leuchtturm Dänemarks.",
					ImageResID = Resource.Drawable.BlavandshukFyr
				}, db);

				var rowTwo = insertUpdateDestinationData(new Destination{ ID = 3, Name = string.Format("Skagen Bunker Museum", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 2,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 2,
					Order = 3,
					Description = "Skagen's Bunker Musem, gut erhaltener Bunker aus dem zweiten Weltkrieg.",
					ImageResID = Resource.Drawable.bunkermuseum
				}, db);

				var rowThree = insertUpdateDestinationData(new Destination{ ID = 4, Name = string.Format("Skagens Museum", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 2,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 3,
					Description = "Skagen's Museum, Kunst aus ganz Dänemark, beschäftigt sich vor allem mit den Skagen-Malern.",
					ImageResID = Resource.Drawable.skagensmuseum
				}, db);

				var rowFour = insertUpdateDestinationData(new Destination{ ID = 5, Name = string.Format("Nationalpark Thy", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 4,
					InterestCity = 1,
					InterestCulture = 2,
					InterestEvents = 3,
					InterestSportActivities = 3,
					VacationTarget = 2,
					Order = 2,
					Description = "Nationalpark Thy, an der Küste Dänemarks, bietet viele Aktivitäten und Sehenswürdigkeiten.",
					ImageResID = Resource.Drawable.nationalparkthy
				}, db);

				var rowFive = insertUpdateDestinationData(new Destination{ ID = 6, Name = string.Format("Strand von Skagen", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 2,
					InterestNature = 4,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 2,
					VacationTarget = 4,
					Order = 3,
					Description = "Der Strand von Skagen bildet den nördlichsten Punkt Dänemarks, ein Ausflug wert.",
					ImageResID = Resource.Drawable.skagenstrand
				}, db);

				var rowSix = insertUpdateDestinationData(new Destination{ ID = 7, Name = string.Format("Skagens Fischrestaurant", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 3,
					Description = "Skagens Fiskrestaurant - bietet eine ausgezeichnete Aussicht und Küche.",
					ImageResID = Resource.Drawable.fiskrestaurant
				}, db);

				var rowSeven = insertUpdateDestinationData(new Destination{ ID = 8, Name = string.Format("Den Gemle By", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 3,
					InterestCulture = 4,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 4,
					Description = "Den Gamle By in Aarhus ist ein Freilichtmuseum über Kultur und Geschichte von Aarhus und Dänemark.",
					ImageResID = Resource.Drawable.dengemleby
				}, db);

				var rowEight = insertUpdateDestinationData(new Destination{ ID = 9, Name = string.Format("Nationalpark Mols Bjerge", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 3,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 1,
					InterestSportActivities = 2,
					VacationTarget = 2,
					Order = 2,
					Description = "Moljs Bjerge, ein Nationalpark mit großen Grünflächen und Wäldern, sehr flach, spiegelt gut die Fauna in Dänemark wieder.",
					ImageResID = Resource.Drawable.molsbjerge
				}, db);

				var rowNine = insertUpdateDestinationData(new Destination{ ID = 10, Name = string.Format("Rosenborg Schloss", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 2,
					InterestNature = 4,
					InterestCity = 4,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 5,
					Description = "Rosenborg, Schloss von Kopenhagen, hier findet sich der Königsgarten. Hier kann man auch die dänischen Kronjuwelwen bewundern.",
					ImageResID = Resource.Drawable.schlossrosenborg
				}, db);

				var rowTen = insertUpdateDestinationData(new Destination{ ID = 11, Name = string.Format("Die kleine Meerjungfrau", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 2,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 6,
					Description = "Die kleine Meerjungfrau - eine Bronzefigur an der Uferprominade von Kopenhagen. Vorbild des gleichnamigen Märchens.",
					ImageResID = Resource.Drawable.kleinemeerjungfrau
				}, db);

				var rowEleven = insertUpdateDestinationData(new Destination{ ID = 12, Name = string.Format("Wikingermuseum Foteviken", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 2,
					VacationTarget = 3,
					Order = 6,
					Description = "Museum Foteviken - ein Museum über die Kultur und Geschichte der Wikinger.",
					ImageResID = Resource.Drawable.wikingermuseum
				}, db);

				var rowTwelve = insertUpdateDestinationData(new Destination{ ID = 13, Name = string.Format("UFO Monument Ängelholm", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 2,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 2,
					Order = 5,
					Description = "UFO-Denkmal Ängelholm ist das erste Denkmal an eine vermeintliche UFO-Landung 1963.",
					ImageResID = Resource.Drawable.ufomonument
				}, db);

				var rowThirteen = insertUpdateDestinationData(new Destination{ ID = 14, Name = string.Format("Pio Country Club", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 2,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 2,
					Order = 7,
					Description = "Der Pio Country Club ist ein empfehlenswertes Restaurant, dass sich auf amerikanische Speisen spezialisiert hat.",
					ImageResID = Resource.Drawable.piocountryclub
				}, db);

				var rowFourteen = insertUpdateDestinationData(new Destination{ ID = 15, Name = string.Format("Schloss Kronborg", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 3,
					InterestCulture = 4,
					InterestEvents = 3,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 5,
					Description = "Schloss Kronborg auf einer Insel Dänemarks ist durch seine Verbindung zu William Shakesspeare bekannt.",
					ImageResID = Resource.Drawable.schlosskronborg
				}, db);

				var rowFiveteen = insertUpdateDestinationData(new Destination{ ID = 16, Name = string.Format("Breezanddijk", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 1,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 2,
					Order = 4,
					Description = "Breezanddijk ist eine 12 Kilometer lange Straße die von beiden Seiten direkt von Meer umgeben ist.",
					ImageResID = Resource.Drawable.Breezanddijk
				}, db);

				var rowSixteen = insertUpdateDestinationData(new Destination{ ID = 17, Name = string.Format("Hamburger Hafen", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 3,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 1,
					Description = "Der Hamburger Hafen, der größte Hafen Deutschlands, wartet mit vielen Events und Einkehrmöglichkeiten auf. ",
					ImageResID = Resource.Drawable.hamburghafen
				}, db);

				var rowSeventeen = insertUpdateDestinationData(new Destination{ ID = 18, Name = string.Format("Romo", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 4,
					InterestNature = 4,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 2,
					InterestSportActivities = 2,
					VacationTarget = 1,
					Order = 5,
					Description = "Romo, die südlichste Insel Dänemarks bietet wunderschöne Dünen und Sandstrände.",
					ImageResID = Resource.Drawable.romo
				}, db);

				var rowEightteen = insertUpdateDestinationData(new Destination{ ID = 19, Name = string.Format("Naturgewalten Sylt", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 2,
					InterestEvents = 3,
					InterestSportActivities = 2,
					VacationTarget = 2,
					Order = 1,
					Description = "Naturgewalten Sylt, ist ein Veranstaltungs und Ausstellungszentrum in Sylt, dass sich mit Meer- und Küstendynamik beschäftigt.",
					ImageResID = Resource.Drawable.naturgewaltensylt
				}, db);

				var rowNineteen = insertUpdateDestinationData(new Destination{ ID = 20, Name = string.Format("Sneglehuset", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 2,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 2,
					Description = "Das Sneglehuset in Thyboron ist ein Gebäudekomplex dekoriert mit Millionen Schneckenhäusern.",
					ImageResID = Resource.Drawable.sneglehuset
				}, db);

				var rowTwenty = insertUpdateDestinationData(new Destination{ ID = 21, Name = string.Format("Aarhus", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 1,
					InterestCity = 4,
					InterestCulture = 2,
					InterestEvents = 4,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 4,
					Description = "Aarhus ist eine Universitätsstadt mit zahlreichen Möglichkeiten zum Weggehen und Spaß haben.",
					ImageResID = Resource.Drawable.arhus
				}, db);

				var rowTwentyOne = insertUpdateDestinationData(new Destination{ ID = 22, Name = string.Format("Tivoli", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 3,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 2,
					InterestEvents = 4,
					InterestSportActivities = 3,
					VacationTarget = 3,
					Order = 5,
					Description = "Der älteste Vergnügungspark der Welt. Bietet neben einer wunderschönen Kulisse auch zahlreiche Möglichkeiten sein Geld zu verschwenden und dabei Spaß zu haben.",
					ImageResID = Resource.Drawable.tivoli
				}, db);

				var rowTwentyTwo = insertUpdateDestinationData(new Destination{ ID = 23, Name = string.Format("Rubjerg Knude", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 3,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 3,
					VacationTarget = 4,
					Order = 3,
					Description = "Eine Wanderdüne, die einen Leuchtturm verschlingt. Man fühlt sich hier eher wie in der Sahara, als in Dänemark. Vor allem im Sommer.",
					ImageResID = Resource.Drawable.knuden
				}, db);

				var rowTwentyThree = insertUpdateDestinationData(new Destination{ ID = 24, Name = string.Format("Labyrinthia", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 4,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 3,
					InterestSportActivities = 2,
					VacationTarget = 3,
					Order = 4,
					Description = "Sieben verschiedene Labyrinte, die man erkunden und aus denen man wieder entkommen muss. In manchen muss man noch dazu Rätsel lösen um weiter zu kommen.",
					ImageResID = Resource.Drawable.labyrinthia
				}, db);

				var rowTwentyFour = insertUpdateDestinationData(new Destination{ ID = 25, Name = string.Format("Bremen", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 1,
					InterestNature = 2,
					InterestCity = 4,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 1,
					Description = "Eine deutsche Küstenstadt mit Fachwerkhäusern, die gut erhalten sind. Schöne Stadt zum entspannen und um norddeutsche Kulur kennenzulernen.",
					ImageResID = Resource.Drawable.bremen
				}, db);

				var rowTwentyFive = insertUpdateDestinationData(new Destination{ ID = 26, Name = string.Format("La Mer", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 2,
					InterestCulture = 4,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 1,
					Description = "Ein mit 3 Michelin Sternen ausgezeichnetes Gourmet-Restaurant, das sich auf Meeres-Spezialitäten fokussiert hat. Sehr schönes Ambiente.",
					ImageResID = Resource.Drawable.lamer
				}, db);

				var rowTwentySix = insertUpdateDestinationData(new Destination{ ID = 27, Name = string.Format("Surf In", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 4,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 4,
					InterestSportActivities = 4,
					VacationTarget = 4,
					Order = 1,
					Description = "Eine Surfschule an der Nordsee, die sich darauf spezialisiert hat Anfängern möglichst schnell das Surfen beizubringen. Man kann auch nur Ausrüstung ausleihen.",
					ImageResID = Resource.Drawable.surfin
				}, db);

				var rowTwentySeven = insertUpdateDestinationData(new Destination{ ID = 28, Name = string.Format("Reiterhof Hennings", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 3,
					InterestNature = 4,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 2,
					InterestSportActivities = 3,
					VacationTarget = 1,
					Order = 1,
					Description = "Eine Surfschule an der Nordsee, die sich darauf spezialisiert hat Anfängern möglichst schnell das Surfen beizubringen. Man kann auch nur Ausrüstung ausleihen.",
					ImageResID = Resource.Drawable.reiterhofhennings
				}, db);

				var rowTwentyEight = insertUpdateDestinationData(new Destination{ ID = 29, Name = string.Format("Groningen", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 2,
					InterestNature = 1,
					InterestCity = 4,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 2,
					Description = "Eine uriger Ort, mit einem wunderschönen Hafen, der vor allem Segelboote beherbergt. Bekannt für gutes Essen.",
					ImageResID = Resource.Drawable.groningen
				}, db);

				var rowTwentyNine = insertUpdateDestinationData(new Destination{ ID = 30, Name = string.Format("Den Helder - Alkmar", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 4,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 1,
					Order = 2,
					Description = "Eine anspruchsvolle Fahrradstrecke an der Küste der Niederlande. Die Anstrengung aber alle mal wert.",
					ImageResID = Resource.Drawable.biketour
				}, db);

				var rowThirty = insertUpdateDestinationData(new Destination{ ID = 31, Name = string.Format("DeBoer Bakkerij", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 4,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 4,
					Order = 2,
					Description = "Eine anspruchsvolle Fahrradstrecke an der Küste der Niederlande. Die Anstrengung aber alle mal wert.",
					ImageResID = Resource.Drawable.boerbakerij
				}, db);

				var rowThirtyOne = insertUpdateDestinationData(new Destination{ ID = 32, Name = string.Format("Esbjerg", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 1,
					InterestCity = 4,
					InterestCulture = 3,
					InterestEvents = 3,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 3,
					Description = "Eine große Stadt an der Westküste Dänemarks. Die Skulptur 'Mensch am Meer' kann hier besichtigt werden. Auch sonst bietet die Stadt einige Erlebnisse.",
					ImageResID = Resource.Drawable.esbjerg
				}, db);

				var rowThirtyTwo = insertUpdateDestinationData(new Destination{ ID = 33, Name = string.Format("Herning", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 2,
					InterestNature = 1,
					InterestCity = 3,
					InterestCulture = 2,
					InterestEvents = 4,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 3,
					Description = "Eine kulturreiche Stadt in der Mitte Dänemarks. Bietet einige Museen und ein modernes Ambiente.",
					ImageResID = Resource.Drawable.Herning
				}, db);

				var rowThirtyThree = insertUpdateDestinationData(new Destination{ ID = 34, Name = string.Format("Moesgard Museum", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 3,
					InterestNature = 4,
					InterestCity = 2,
					InterestCulture = 1,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 4,
					Description = "Zahllose riesige Allen mit hunderten verschiedener Arten an Bäumen und Pflanzen werden zu einem wunderschönen Gesamt-Bild.",
					ImageResID = Resource.Drawable.moesgardmuseum
				}, db);

				var rowThirtyFour = insertUpdateDestinationData(new Destination{ ID = 35, Name = string.Format("Tuno Trip", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 3,
					InterestNature = 2,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 4,
					VacationTarget = 1,
					Order = 4,
					Description = "Eine Fahrradtour rund um die Insel Tuno. Flaches Terrain, aber ein schöner, weitreichender Ausblick.",
					ImageResID = Resource.Drawable.tunotrip
				}, db);

				var rowThirtyFive = insertUpdateDestinationData(new Destination{ ID = 36, Name = string.Format("Restaurant Miro", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 3,
					InterestCulture = 2,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 4,
					Order = 4,
					Description = "Restaurant in der Mitte Aarhus mit feinen Speißen. Hat sich auf Essen spezialisiert, dass frisch und oft nicht gekocht ist.",
					ImageResID = Resource.Drawable.miro
				}, db);

				var rowThirtySix = insertUpdateDestinationData(new Destination{ ID = 37, Name = string.Format("SegWorld", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 3,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 2,
					InterestSportActivities = 3,
					VacationTarget = 2,
					Order = 4,
					Description = "Der erste und einzige SegWay Vergnügungspark. Mit SegWays fahren, hüpfen, essen ...",
					ImageResID = Resource.Drawable.segworld
				}, db);

				var rowThirtySeven = insertUpdateDestinationData(new Destination{ ID = 38, Name = string.Format("Restaurant Kok & Vin", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 2,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 5,
					Description = "Ein gehobenes Restaurant, das sich auf mittelalterliche Küche spezialisiert hat.",
					ImageResID = Resource.Drawable.kokvin
				}, db);

				var rowThirtyEight = insertUpdateDestinationData(new Destination{ ID = 39, Name = string.Format("Insel Fünen", System.DateTime.Now.Ticks), 
					Standards = 2,
					IsActive = 4,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 3,
					InterestSportActivities = 4,
					VacationTarget = 1,
					Order = 5,
					Description = "Im dänischen Südmeer gelegen ist diese Insel bekannt für ihre zahlreichen Sport-Events und -Aktivitäten.",
					ImageResID = Resource.Drawable.funenarchipel
				}, db);

				var rowFourty = insertUpdateDestinationData(new Destination{ ID = 41, Name = string.Format("Amager Strandpark", System.DateTime.Now.Ticks), 
					Standards = 1,
					IsActive = 3,
					InterestNature = 4,
					InterestCity = 1,
					InterestCulture = 3,
					InterestEvents = 3,
					InterestSportActivities = 2,
					VacationTarget = 4,
					Order = 6,
					Description = "Der größte Strand in der Nähe Kopenhagens mit anschliessendem Park. Ist ein Naturschutzgebiet, aber für viele sportliche Aktivitäten freigegeben.",
					ImageResID = Resource.Drawable.amagerstrandpark
				}, db);

				var rowFourtyOne = insertUpdateDestinationData(new Destination{ ID = 42, Name = string.Format("Dänisches Nationalmuseum", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 2,
					InterestNature = 1,
					InterestCity = 3,
					InterestCulture = 3,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 6,
					Description = "Ausstellung über die Geschichte und Fortentwicklung der dänischen und nordischen Kultur. In der Stadtmitte von Kopenhagen.",
					ImageResID = Resource.Drawable.denmarknationalmusem
				}, db);

				var rowFourtyTwo = insertUpdateDestinationData(new Destination{ ID = 43, Name = string.Format("Restaurant Noma", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 1,
					InterestNature = 1,
					InterestCity = 2,
					InterestCulture = 2,
					InterestEvents = 1,
					InterestSportActivities = 1,
					VacationTarget = 3,
					Order = 6,
					Description = "Restaurant in Brygge, dass sich auf die moderne Neuinterpretation regionaler Gerichte spezialisiert hat.",
					ImageResID = Resource.Drawable.nomarestaurant
				}, db);

				var rowFourtyThree = insertUpdateDestinationData(new Destination{ ID = 44, Name = string.Format("Passebækgård Golf", System.DateTime.Now.Ticks), 
					Standards = 4,
					IsActive = 4,
					InterestNature = 3,
					InterestCity = 1,
					InterestCulture = 1,
					InterestEvents = 4,
					InterestSportActivities = 4,
					VacationTarget = 2,
					Order = 6,
					Description = "Ein auf Nord Zeeland gelegener Park inklusive riesigem Golfplatz, der als einer der schönsten ganz Dänemarks gilt.",
					ImageResID = Resource.Drawable.passebaekgardgolf
				}, db);

				var rowFourtyFour = insertUpdateDestinationData(new Destination{ ID = 45, Name = string.Format("Fisketorvet", System.DateTime.Now.Ticks), 
					Standards = 3,
					IsActive = 2,
					InterestNature = 1,
					InterestCity = 4,
					InterestCulture = 1,
					InterestEvents = 3,
					InterestSportActivities = 1,
					VacationTarget = 1, // because shopping is so strenous. ;-P
					Order = 6,
					Description = "Kopenhagens größte Shopping-Mall. Hier gibt es alles was das Herz begehrt. Und noch mehr. Für einen Preis.",
					ImageResID = Resource.Drawable.fisketorvet
				}, db);

				Log.Info (Tag, "Destination DB updated");
			}

		}
	}
}

