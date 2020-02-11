using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Essentials;



namespace andoridApp
{
    class Connector
    {
        static public async void Get(Action<string> callback) 
        {

            try
            {
                var request = new HttpRequestMessage();

                request.RequestUri = new Uri("https://fynprojekt.azurewebsites.net/Api/values");
                request.Method = HttpMethod.Get;

                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.SendAsync(request);



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    callback(result);
                }
            }
            catch {

            }
        }


        static public async void Post(string lok, string lat, string latLong, TextView text)
        {
            try
            {
                var request = new HttpRequestMessage();

                request.RequestUri = new Uri("https://fynprojekt.azurewebsites.net/Api/values");
                request.Method = HttpMethod.Post;


                //request.Content = new ByteArrayContent(new byte[] { 1, 1, 1, 1 });
                
                //request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                request.Content = new StringContent(":'olla',200,200", Encoding.UTF8, "application/x-www-form-urlencoded");



                
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string temp = await response.Content.ReadAsStringAsync();
                    text.Text = temp;
                }
                else 
                {
                    text.Text = response.StatusCode.ToString();
                }
            }
            catch(Exception e)
            {
                text.Text = "error: " + e.Message;
            }
        }
    }
}