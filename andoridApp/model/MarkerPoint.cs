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

namespace andoridApp.model
{
    class MarkerPoint
    {
        public string name { get; set; }
        public double lat { get; set; }
        public double longlat { get; set; }
        public DateTime time { get; set; }


        public MarkerPoint(string name, double lat, double longlat) {

            this.name = name;
            this.lat = lat;
            this.longlat = longlat;
        }
        public MarkerPoint() {
            name = "";
            lat = 0;
            longlat = 0;
        }

        public override string ToString()
        {

            return $"name={name}&lat={lat}&longlat={longlat}".Replace(",",".");
        }
    }
}