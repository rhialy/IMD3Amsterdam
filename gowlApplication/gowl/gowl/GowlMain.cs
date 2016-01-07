﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Android.Media;
using SQLite;

namespace GOWL
{
	[Activity (Label = "GOWL")]			
	public class GowlMain : Activity
	{
		private static string dbFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
		private static string dbPath = System.IO.Path.Combine(dbFolder, "gowl_user.db");

		MediaPlayer mediaPlayer;

		private const int VOICE = 10;

		private string gowlVoiceHallo = "Hallo";
		private string gowlVoiceStatus = "wie geht es dir";
		private string gowlVoiceErleben = "alles zu erleben";
		private string gowlVoiceStrand = "zum strand";
	
		private bool existingJourney = true;

		private int phase;

		private ViewFlipper flipper;

		private Button newJourneyBtn;
		private Button userDataBtn;
		private Button backUserDataBtn;
		private Button resetBtn;
		private Button speechListener;

		private TextView voiceResultText;

		private static string Tag = "GowlMain";

		public NewJourney newJourney;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RetrieveDatafromDB ();

			if (existingJourney == false) {
				SetContentView (Resource.Layout.Main);
				newJourneyBtn = (Button)FindViewById (Resource.Id.NewJourneyButton);
				userDataBtn = (Button)FindViewById (Resource.Id.UserDataButton);
				backUserDataBtn = (Button)FindViewById (Resource.Id.backButtonUserData);
				resetBtn = (Button)FindViewById (Resource.Id.NewMainPreferences);
				flipper = (ViewFlipper)FindViewById (Resource.Id.viewFlipper1);
				Menu ();
			}
		

			if (existingJourney == true) {
				SetContentView (Resource.Layout.ExistingJourney);
				Log.Info (Tag, "existing Journey false");
				voiceResultText = (TextView)FindViewById (Resource.Id.VoiceResultText);
				speechListener = (Button)FindViewById (Resource.Id.voiceButton);
				speechListener.Click += RecordVoice;
			}
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

				existingJourney = User.ExistingJourney;

			}
		}

		//----------------Main Menu function----------------//
		private void Menu() {
			Log.Info (Tag, newJourneyBtn.ToString ());
			newJourneyBtn.Click += ((object sender, System.EventArgs e) => {
				Log.Info(Tag, "Clicked New Journey");
				StartActivity(typeof(NewJourneySpecificPreference));
				existingJourney = true;
			});

			userDataBtn.Click += (sender, e) => {
				flipper.ShowNext();
			};

			backUserDataBtn.Click += (object sender, EventArgs e) => {
				flipper.ShowPrevious();
			};

			resetBtn.Click += (object sender, EventArgs e) => {
				StartActivity(typeof(MainPreferencesPhaseOne));
			};

		}

		private void RecordVoice(object s, EventArgs e)
		{
			var result = voiceResultText;
			result.Text = string.Empty;
			var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
			voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak Now :)");
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
			voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
			StartActivityForResult(voiceIntent, VOICE);
		}

		protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
		{
			//mediaPlayer.Stop ();
			//mediaPlayer.Release ();
			if (requestCode == VOICE)
			{
				if (resultVal == Result.Ok)
				{
					var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
					var result = voiceResultText;
					result.Text = matches[0].ToString();
					if (result.Text == gowlVoiceHallo) {
						MediaPlayer mediaPlayer = MediaPlayer.Create (this, Resource.Raw.IchBinGowl);
						mediaPlayer.Start ();
					} else if (result.Text.Contains (gowlVoiceStatus)) {
						MediaPlayer mediaPlayer = MediaPlayer.Create (this, Resource.Raw.AkkuAufladen2);
						mediaPlayer.Start ();
					} else if (result.Text.Contains (gowlVoiceErleben)) {
						MediaPlayer mediaPlayer = MediaPlayer.Create (this, Resource.Raw.NationalparkUndSneglehuset);
						mediaPlayer.Start ();
					} else if (result.Text.Contains (gowlVoiceStrand)) {
						MediaPlayer mediaPlayer = MediaPlayer.Create (this, Resource.Raw.StrandVonSkagen);
						mediaPlayer.Start ();
					}
				}
			}
			base.OnActivityResult(requestCode, resultVal, data);
		}
			
	}
}

