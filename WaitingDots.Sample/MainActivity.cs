using Android.App;
using Android.OS;
using Android.Widget;
using blocke.waitingdots;

namespace WaitingDots.Sample
{
    [Activity(Label = "WaitingDots.Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity
    {
        private Button buttonHide;
        private Button buttonHideAndStop;
        private Button buttonPlay;
        private DotsTextView dotsTextView;

        protected override int LayoutResource
        {
            get { return Resource.Layout.activity_main; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            dotsTextView = FindViewById<DotsTextView>(Resource.Id.dots);
            buttonPlay = FindViewById<Button>(Resource.Id.buttonPlay);
            buttonHide = FindViewById<Button>(Resource.Id.buttonHide);
            buttonHideAndStop = FindViewById<Button>(Resource.Id.buttonHideAndStop);

            buttonPlay.Click += (sender, args) =>
            {
                if (dotsTextView.Playing)
                {
                    dotsTextView.Stop();
                }
                else
                {
                    dotsTextView.Start();
                }
            };

            buttonHide.Click += (sender, args) =>
            {
                if (dotsTextView.IsHide)
                {
                    dotsTextView.Show();
                }
                else
                {
                    dotsTextView.Hide();
                }
            };

            buttonHideAndStop.Click += (sender, args) =>
            {
                if (dotsTextView.IsHide)
                {
                    dotsTextView.ShowAndPlay();
                }
                else
                {
                    dotsTextView.HideAndStop();
                }
            };


            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);
        }
    }
}