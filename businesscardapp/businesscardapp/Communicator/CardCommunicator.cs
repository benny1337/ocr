using models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace businesscardapp.Communicator
{
    public class CardCommunicator
    {
        public async Task<BusinessCard> GetCardAsync(string Base64)
        {            
            var client = new HttpClient();
            var url = "http://ocrbusinesscard.azurewebsites.net/api/ocr";

            var img = new Image()
            {
                Base64 = Base64
            };
            var json = JsonConvert.SerializeObject(img);                
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("Error: " + response.ReasonPhrase);
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BusinessCard>(jsonResult);            
        }
    }
}
