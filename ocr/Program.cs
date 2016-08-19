using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ocr
{
    class Program
    {
        public static int PromtForPoints()
        {            
            do
            {
                Console.WriteLine("Skriv in poäng:");
                var res = -1;
                var success = Int32.TryParse(Console.ReadLine(), out res);

                if (success && (res > -1 && res <= 20))
                    return res; 
                else
                    Console.WriteLine("Talet måste vara större än 0 och mindre än 20");
            }
            while (true);
        }

        static void Main(string[] args)
        {
            var y = PromtForPoints();

            var client = new VisionServiceClient("b982c39d840645b3ade5a62588656306");

            
            //var pathSource = "c:\\temp\\card.png";

            var sources = new List<string>()
            {
                "c:\\temp\\cards\\card.png",
                "c:\\temp\\cards\\card1.png",
                "c:\\temp\\cards\\card.jpg",
                "c:\\temp\\cards\\card2.jpg",
                "c:\\temp\\cards\\card3.jpg",
                "c:\\temp\\cards\\card4.jpg",
                "c:\\temp\\cards\\card5.jpg",
                "c:\\temp\\cards\\card6.jpg",
            };

            foreach (var pathSource in sources)
            {
                using (Stream imageFileStream = File.OpenRead(pathSource))
                {
                    var text = client.RecognizeTextAsync(imageFileStream).Result;
                    var x = new Card(text);
                    var card = x.BusinessCard;
                                        
                    Console.WriteLine($"Name: {card.Name}");
                    Console.WriteLine($"Title: {card.Title}");
                    Console.WriteLine($"Company: {card.CompanyName}");
                    Console.WriteLine($"Phone: {card.Phone}");
                    Console.WriteLine($"Address: {card.AddreessStreet}, {card.AddressCity}");
                    Console.WriteLine($"Website: {card.Website}");
                    Console.WriteLine($"Email: {card.Email}");

                    Console.WriteLine($"----------\n{card.FullText}");
                }
                Console.WriteLine("---------------------------------");
            }            
            Console.ReadKey();                       
        }

        
    }
}
