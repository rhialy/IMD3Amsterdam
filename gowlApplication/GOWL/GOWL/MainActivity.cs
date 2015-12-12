using Android.App;
using Android.Widget;
using Android.OS;

namespace GOWL
{
	[Activity (Label = "GOWL", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		ImageView firstImage;

		bool isImageFitToScreen;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);


			SetContentView (Resource.Layout.MainPreferences);

			firstImage = (ImageView)FindViewById (Resource.Id.imageView1);

			firstImage.Click += ((object sender, System.EventArgs e) => {
				if (isImageFitToScreen) {
					isImageFitToScreen = false;
					firstImage.SetMinimumHeight (450);
					firstImage.SetMinimumWidth (450);
				} else {
					isImageFitToScreen = true;
					firstImage.SetMinimumWidth (1500);
					firstImage.SetMinimumHeight (1500);
				}
			});
		}
	}
}


