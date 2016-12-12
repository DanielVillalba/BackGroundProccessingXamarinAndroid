using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Locations;
using System;
using Android.Content;
using System.Collections.Generic;

namespace Location.Droid
{
    [Activity(Label = "Location.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        readonly string logTag = "MainActivity";

        TextView latText;
        TextView longText;
        TextView altText;
        TextView speedText;
        TextView bearText;
        TextView accText;
        Button callHistoryButton;

        static readonly List<string> trackChanges = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Log.Debug(logTag, "OnCreate: Location app is becoming active");

            SetContentView (Resource.Layout.Main);

            App.Current.LocationServiceConnected += (object sender, Services.ServiceConnectedEventArgs e) =>
            {
                Log.Debug(logTag, "ServiceConnected Event Raised");
                App.Current.LocationService.LocationChanged += HandleLocationChanged;
                App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                App.Current.LocationService.StatusChanged += HandleStatusChanged;
            };

            latText = FindViewById<TextView>(Resource.Id.lat);
            longText = FindViewById<TextView>(Resource.Id.longx);
            altText = FindViewById<TextView>(Resource.Id.alt);
            speedText = FindViewById<TextView>(Resource.Id.speed);
            bearText = FindViewById<TextView>(Resource.Id.bear);
            accText = FindViewById<TextView>(Resource.Id.acc);
            callHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);

            callHistoryButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(EventList));
                intent.PutStringArrayListExtra("trackChanges", trackChanges);
                StartActivity(intent);
            };

            altText.Text = "altitude";
            speedText.Text = "speed";
            bearText.Text = "bearing";
            accText.Text = "accuracy";

            App.StartLocationService();
        }

        protected override void OnPause()
        {
            Log.Debug(logTag, "OnPause: Location app is moving to background");
            base.OnPause();
        }

        protected override void OnResume()
        {
            Log.Debug(logTag, "OnResume: Location app is moving to foreground");
            base.OnResume();
        }

        


        private void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Log.Debug(logTag, "Location status changed, event raised");
        }

        private void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
            Log.Debug(logTag, "Location provider enabled event raised");
        }

        private void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
            Log.Debug(logTag, "Location provider disabled event raised");
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Android.Locations.Location location = e.Location;
            Log.Debug(logTag, "Foreground updating");

            // these events are on a background thread, need to update on the UI thread
            RunOnUiThread(() => {
                latText.Text = String.Format("Latitude: {0}", location.Latitude);
                longText.Text = String.Format("Longitude: {0}", location.Longitude);
                altText.Text = String.Format("Altitude: {0}", location.Altitude);
                speedText.Text = String.Format("Speed: {0}", location.Speed);
                accText.Text = String.Format("Accuracy: {0}", location.Accuracy);
                bearText.Text = String.Format("Bearing: {0}", location.Bearing);
                int numberOfEvents = trackChanges.Count + 1;
                trackChanges.Add(numberOfEvents.ToString() + ".- Latitude " + location.Latitude.ToString()  + " - Longitude " + location.Longitude.ToString());
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            App.StopLocationService();
        }
    }
}

