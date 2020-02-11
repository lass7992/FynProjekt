using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using andoridApp.model;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
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
                    result = result.Replace("\"", "");
                    result = result.Replace("\\", "");

                    result = result.Remove(0, 1);
                    result = result.Remove(result.Length-1, 1);

                    callback(result);
                }
            }
            catch {

            }
        }

        static public async void Delete()
        {

            try
            {
                var request = new HttpRequestMessage();

                request.RequestUri = new Uri("https://fynprojekt.azurewebsites.net/Api/values");
                request.Method = HttpMethod.Delete;

                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.SendAsync(request);



                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                }
            }
            catch
            {
            }
        }

        static public async void Post(MarkerPoint mark, TextView text)
        {
            try
            {
                var request = new HttpRequestMessage();

                request.RequestUri = new Uri("https://fynprojekt.azurewebsites.net/Api/values");
                request.Method = HttpMethod.Post;


                // request.Content = new ByteArrayContent(new byte[] { 1, 1, 1, 1 });

               string json = JsonConvert.SerializeObject(mark,Formatting.Indented);



                request.Content = new StringContent(mark.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");

                //request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");



                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string temp = await response.Content.ReadAsStringAsync();
                    string temp2 = await request.Content.ReadAsStringAsync();
                    text.Text = temp2 + " \n " +temp;
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