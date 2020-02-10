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
using andoridApp.model;

namespace andoridApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {

        MapFragment mapFragment;
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

            View but2 = FindViewById(Resource.Id.button2);
            but2.Click += DeleteClick;

            View but3 = FindViewById(Resource.Id.button3);
            but3.Click += UpdateClick;


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


            Connector.Get(LoadMarkers);
        }

        async void LoadMarkers(string markersJson) {
            


            text.Text = "totototo";
            JavaList<MarkerPoint> markers = new JavaList<MarkerPoint>();


            //opretter alle markersne så de kan indsættes på mappet
            foreach (string item in markersJson.Split(","))
            {
                MarkerPoint tempMarker = new MarkerPoint();
                try
                {
                    string[] tempMarkerString = item.Split("|");

                    string[] dateString = tempMarkerString[1].Split(" ");
                    string[] date = dateString[0].Split("/");
                    string[] time = dateString[1].Split(":");

                    if (dateString[2].Equals("PM"))
                    {
                        time[0] = (Convert.ToInt32(time[0]) + 12).ToString();
                    }

                    tempMarker.time = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), Convert.ToInt32(time[2]));
                    tempMarker.name = tempMarkerString[2];
                    tempMarker.lat = Convert.ToDouble(tempMarkerString[3].Replace(".",","));
                    tempMarker.longlat = Convert.ToDouble(tempMarkerString[4].Replace(".", ","));

                    markers.Add(tempMarker);
                }
                catch {
                    text.Text = "gejm med marker";
                }
            }


            MarkerOptions[] tempMarkerOption = new MarkerOptions[markers.Count];
            LatLng tempPos = new LatLng(0,0);
            int counter = -1;
            foreach (MarkerPoint item in markers) 
            {
                counter++;
                tempMarkerOption[counter] = new MarkerOptions();

                tempPos.Latitude = item.lat;
                tempPos.Longitude = item.longlat;

                tempMarkerOption[counter].SetPosition( tempPos );

                tempMarkerOption[counter].SetTitle(item.name);
                tempMarkerOption[counter].SetSnippet( item.time.ToString() );

                googlemap.AddMarker(tempMarkerOption[counter]);
            }
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

            MarkerPoint tempMark = new MarkerPoint("name",location.Latitude, location.Longitude);

            Connector.Post(tempMark, text);
        }

        async void DeleteClick(object sender, EventArgs args)
        {

            googlemap.Clear();

            Connector.Delete();
        }

        async void UpdateClick(object sender, EventArgs args)
        {

            googlemap.Clear();

            Connector.Get(LoadMarkers);
        }
    }
}