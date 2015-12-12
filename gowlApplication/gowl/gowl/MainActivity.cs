using Android.App;
using Android.Widget;
using Android.OS;

namespace GOWL
{
	[Activity (Label = "GOWL", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

<<<<<<< HEAD
			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};
=======
			colorItems.Add (new ColorItem () { Color = Android.Graphics.Color.DarkRed,
				ColorName = "Dark Red", Code = "8B0000" });
			colorItems.Add (new ColorItem () { Color = Android.Graphics.Color.SlateBlue,
				ColorName = "Slate Blue", Code = "6A5ACD" });
			colorItems.Add (new ColorItem () { Color = Android.Graphics.Color.ForestGreen,
				ColorName = "Forest Green", Code = "228B22" });
			colorItems.Add (new ColorItem () { Color = Android.Graphics.Color.DarkSalmon,
				ColorName = "Favorite Blubber", Code = "8BCCDD" });

			listView.Adapter = new ColorAdapter (this, colorItems);
>>>>>>> 12efe36e2cb38dff4fadf18efab167ebcf403ce1
		}
	}
}


