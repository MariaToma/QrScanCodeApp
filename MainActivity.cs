using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using ZXing;
using ZXing.Mobile;

using Android.Content;//pentru vibratie


namespace QRit1._1
{
    [Activity(Label = "QRit", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden)]
    public class MainActivity : Activity
    {
        Button button1;
        Button button2;
        Button button3;
        Button button4;
        Button button5;

        MobileBarcodeScanner scanner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Initialize the scanner first so we can track the current context
            MobileBarcodeScanner.Initialize(Application);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            

            //Create a new instance of our Scanner
            scanner = new MobileBarcodeScanner();
            Button flashButton;
            View zxingOverlay;


            button1 = this.FindViewById<Button>(Resource.Id.butto1);
            button1.Click += async delegate {

                //Tell our scanner we want to use a custom overlay instead of the default
                scanner.UseCustomOverlay = true;

                //Inflate our custom overlay from a resource layout
                zxingOverlay = LayoutInflater.FromContext(this).Inflate(Resource.Layout.ZxingOverlay, null);

                //Find the button from our resource layout and wire up the click event
                flashButton = zxingOverlay.FindViewById<Button>(Resource.Id.buttonZxingFlash);
                flashButton.Click += (sender, e) => scanner.ToggleTorch();

                //Set our custom overlay
                scanner.CustomOverlay = zxingOverlay;

                //Start scanning!
                var result = await scanner.Scan();

                HandleScanResult(result);
            };

        }


        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
                msg = "Codul Scanat: " + result.Text;
            else
                msg = "Scanning Canceled!";

            this.RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Short).Show());
            TextView textViewScan = FindViewById<TextView>(Resource.Id.textViewScan);
            textViewScan.Post(() => {
                Vibrator vib = (Vibrator)GetSystemService(Context.VibratorService);
                vib.Vibrate(1000);
                textViewScan.Text = msg;
            });
           
        }
    }
}

