using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Essentials;
using System;
using Android.Views;

namespace andoridApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {

        MapFragment mapFragment;
        Button but;
        GoogleMap googlemap;
        TextView text;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            mapFragment = (MapFragment) FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            text = FindViewById(Resource.Id.textView1) as TextView;
            
            

            View but = FindViewById(Resource.Id.button1);
            but.Click += Click;

           
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async void OnMapReady(GoogleMap map)
        {
            googlemap = map;
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var locationXam = await Geolocation.GetLocationAsync(request);
            

            // Do something with the map, i.e. add markers, move to a specific location, etc.
            LatLng location = new LatLng(locationXam.Latitude,locationXam.Longitude);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(18);


            CameraPosition cameraPosition = builder.Build();

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            map.MoveCamera(cameraUpdate);
        }

        async void Click(object sender, EventArgs args)
        {
            //var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            //var locationXam = await Geolocation.GetLocationAsync(request);
            LatLng location = googlemap.CameraPosition.Target;



            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(location);

            markerOpt1.SetTitle("point");
            markerOpt1.SetSnippet(location.ToString());


            googlemap.AddMarker(markerOpt1);

            Connector.Post("point", location.Latitude.ToString(), location.Longitude.ToString(), text);
        }

    }
}