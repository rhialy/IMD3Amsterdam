using System;
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

namespace GOWL
{
	[Activity (Label = "GOWL")]			
	public class GowlMain : Activity
	{
		MediaPlayer mp;

		private const int VOICE = 10;

		private string gowlVoiceTest = "Hallo";
		private string gowlVoiceTest2 = "Test";
		private static string musicFolder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyMusic);

		//private static string IchBinGowl = System.IO.Path.Combine(musicFolder, "IchBinGowl.mp3");
		private static string WirSehenUns = System.IO.Path.Combine(musicFolder, "WirSehenUns.mp3");
		private static string AkkuAufladen = System.IO.Path.Combine(musicFolder, "AkkuAufladen.mp3");
		private static string Recherchieren = System.IO.Path.Combine(musicFolder, "RecherchierenRestaurant.mp3");

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
			MediaPlayer mediaPlayer = MediaPlayer.Create (this, Resource.Raw.IchBinGowl);
			/*SetContentView (Resource.Layout.Main);

			newJourneyBtn = (Button)FindViewById (Resource.Id.NewJourneyButton);
			userDataBtn = (Button)FindViewById (Resource.Id.UserDataButton);
			backUserDataBtn = (Button)FindViewById (Resource.Id.backButtonUserData);
			resetBtn = (Button)FindViewById (Resource.Id.NewMainPreferences);
			flipper = (ViewFlipper)FindViewById (Resource.Id.viewFlipper1);*/
			// All Media Player Files



			//existingJourney = false;

			if (existingJourney == true) {
				SetContentView (Resource.Layout.ExistingJourney);
				Log.Info (Tag, "existing Journey false");
				voiceResultText = (TextView)FindViewById (Resource.Id.VoiceResultText);
				speechListener = (Button)FindViewById (Resource.Id.voiceButton);
				speechListener.Click += RecordVoice;
			}

			//Menu ();
				

		}


		private void Menu() {
			Log.Info (Tag, newJourneyBtn.ToString ());
			newJourneyBtn.Click += ((object sender, System.EventArgs e) => {
				Log.Info(Tag, "Clicked New Journey");
				StartActivity(typeof(NewJourney));
				existingJourney = true;
			});

			userDataBtn.Click += (sender, e) => {
				flipper.ShowNext();
			};

			backUserDataBtn.Click += (object sender, EventArgs e) => {
				flipper.ShowPrevious();
			};

			resetBtn.Click += (object sender, EventArgs e) => {
				StartActivity(typeof(MainPreferences));
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
					if (result.Text == gowlVoiceTest) {
						mp = MediaPlayer.Create (this, Resource.Raw.IchBinGowl);
						//mediaPlayer.Prepare ();
						mp.Start ();
					}
					if (result.Text == gowlVoiceTest2) {
						mp.SetDataSource (AkkuAufladen);
						mp.Prepare ();
						mp.Start ();
					}
				}
			}
			base.OnActivityResult(requestCode, resultVal, data);
		}
			
	}
}

