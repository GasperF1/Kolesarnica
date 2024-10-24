using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder
{
    public class GeoService
    {
        private static readonly string apiKey = "3f7e50c80075450e85f91d1c145d92b4";
        private static readonly string apiUrl = "https://api.opencagedata.com/geocode/v1/json?";

        public static async Task<string> GetAddressFromCoordinates(double latitude, double longitude)
        {
            using (HttpClient client = new HttpClient())
            {
                //q=52.3877830%2C9.7334394&key=
                string requestUrl = $"{apiUrl}q={latitude}%2C{longitude}&key={apiKey}";
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(json);


                    // Pridobi prvi naslov iz rezultata
                    string address = data["results"][0]["formatted"].ToString();
                    return address;

                }
                else
                {
                    throw new Exception("Napaka pri pošiljanju zahteve API-ju.");
                }
            }
        }
    }
}