using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiAppWeatherNow.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppWeatherNow.Services
{
    internal class DataService
    {
       public static async Task<Tempo?> GetPrevisao(string cidade) 
        {
            Tempo? t = null;

            string chave = "2b659183e24b6aa7f7a46de3bab4d57e";
            string url = $"http://api.openweathermap.org/data/2.5/weather?" + 
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode) 
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime rise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime set = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["long"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_min"],
                        speed = (double)rascunho["wind"]["speed"],
                        sunrise = rise.ToString(),
                        sunset = set.ToString(),
                        visibility= (int)rascunho["visibility"]
                    };
                }
            }

            return t;
        }

}
}
