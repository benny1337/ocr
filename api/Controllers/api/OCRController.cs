using api.Models;
using Microsoft.ProjectOxford.Vision;
using models;
using ocr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace api.Controllers.api
{   
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OCRController : ApiController
    {        

        // POST: api/OCR
        public async Task<BusinessCard> Post([FromBody]Image img)
        {            
            var client = new VisionServiceClient("b982c39d840645b3ade5a62588656306");
            var ms = new MemoryStream(Convert.FromBase64String(img.Base64));
            var text = await client.RecognizeTextAsync(ms);
            return new Card(text).BusinessCard;            
        }
        
    }
}
