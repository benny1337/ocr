using Microsoft.ProjectOxford.Vision.Contract;
using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ocr
{
    public class Card
    {
        private OcrResults text;
        
        public BusinessCard BusinessCard { get; set; }
        

        public Card(OcrResults result)
        {
            text = result;
            Extract();
        }

        private void Extract()
        {
            BusinessCard = new BusinessCard();
            ExtractEmail();
            ExtractUrl();
            ExtractCompanyName();
            ExtractTitle();
            ExtractPhone();
            ExtractUrl();
            ExtractAddress();
            BusinessCard.FullText = GetFullText();
        }

        private void ExtractTitle()
        {
            var lines = text.Regions.SelectMany(x => x.Lines);

            for (var i = 0; i < lines.Count(); i++)
            {
                var line = lines.ElementAt(i);
                var words = string.Join(" ", line.Words.Select(x => x.Text));
                if (words.Equals(BusinessCard.Name) && (i + 1) < lines.Count())
                {
                    BusinessCard.Title = string.Join(" ", lines.ElementAt(i + 1).Words.Select(x => x.Text));
                    i++;
                }
            }
        }

        public void ExtractAddress()
        {
            var regexswedishpostal = new Regex(@"[\d]{1}[\d]{1}[\d]{1}[ ]{1}[\d]{1}[\d]{1}");
            var regexnumbers = new Regex(@"^[0-9]+$");
            var regextext = new Regex(@"^[a-zåäöA-ZÅÄÖ]+$");
            var lines = text.Regions.SelectMany(x => x.Lines.Where(y => y.Words.Any(z => regexnumbers.Matches(z.Text).Count > 0) && y.Words.Any(z => regextext.Matches(z.Text).Count > 0)));
            if (lines.Count() == 0)
                return;


            foreach(var line in lines)
            {
                var words = string.Join(" ", line.Words.Select(x => x.Text));
                var match = regexswedishpostal.Match(words);
                if(!string.IsNullOrEmpty(match.Value))
                {
                    var temp = words.Split(new string[] { match.Value }, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    if(temp.Count() > 1)
                    {
                        BusinessCard.AddressCity = $"{match.Value} {temp[1]}";
                        BusinessCard.AddreessStreet = temp.FirstOrDefault();
                    }
                    else
                    {
                        BusinessCard.AddressCity = $"{match.Value} {temp[0]}";
                    }

                    break;
                }
            }            

            if(string.IsNullOrEmpty(BusinessCard.AddreessStreet))
                BusinessCard.AddreessStreet = string.Join(" ", lines.FirstOrDefault()?.Words.Select(x => x.Text));

            if (string.IsNullOrEmpty(BusinessCard.AddressCity) && lines.Count() > 1)
                BusinessCard.AddressCity = string.Join(" ", lines.ElementAt(1).Words.Select(x => x.Text));            
            
        }

        private void ExtractPhone()
        {
            var regexnumbers = new Regex(@"^[0-9]+");
            var lineswithonlynumbers = text.Regions.SelectMany(x => x.Lines.Where(y => y.Words.All(z => regexnumbers.Matches(z.Text).Count > 0)));
            if(lineswithonlynumbers.Count() > 0)
            {
                BusinessCard.Phone = string.Join(" ", lineswithonlynumbers.First().Words.Select(x => x.Text));
                return;
            }

            var regex = new Regex(@"^[+|(00)]([0-9+-])+$");

            var lineswithnumbers = text.Regions.SelectMany(x => x.Lines.Where(y => y.Words.Any(z => regex.Matches(z.Text).Count > 0)));
            var words = lineswithnumbers.FirstOrDefault()?.Words.Where(x => regex.Matches(x.Text).Count > 0 || regexnumbers.Matches(x.Text).Count > 0);
            if (words != null && words.Count() > 0)
            {
                BusinessCard.Phone = string.Join(" ", words.Select(x => x.Text));
                return;
            }                
        }

        private void ExtractCompanyName()
        {
            var lettersregex = new Regex(@"^[a-zåäöA-ZÅÄÖ]+$", RegexOptions.IgnoreCase);

            
            var linesbyheight = text.Regions.SelectMany(x => 
                    x.Lines.Where(z => 
                        z.Words.All(w => lettersregex.Matches(w.Text).Count > 0) && 
                        !z.Words.Any(w =>
                            w.Text.Equals(BusinessCard.Email) || 
                            w.Text.Equals(BusinessCard.Website)))
                    .OrderByDescending(y => y.Rectangle.Height));

            if (linesbyheight.Count() < 1)
                return;

            var biggest = linesbyheight.FirstOrDefault().Words;

            var secondbiggest = new List<Word>();
            if (linesbyheight.Count() > 1)
                secondbiggest = linesbyheight.ToList()[1].Words.ToList();

            //the "biggest" line with only one word probably is company name
            if (biggest.Count() > 1)
            {
                if (secondbiggest.Count() > 1)
                {
                    BusinessCard.CompanyName = string.Join(" ", biggest.Select(x => x.Text).ToArray());
                    BusinessCard.Name = string.Join(" ", secondbiggest.Select(x => x.Text).ToArray());
                }
                else
                {
                    BusinessCard.Name = string.Join(" ", biggest.Select(x => x.Text).ToArray());
                    BusinessCard.CompanyName = string.Join(" ", secondbiggest.Select(x => x.Text).ToArray());
                }
            }
            else
            {
                BusinessCard.CompanyName = string.Join(" ", biggest.Select(x => x.Text).ToArray());
                BusinessCard.Name = string.Join(" ", secondbiggest.Select(x => x.Text).ToArray());
            }

            if(!BusinessCard.Name.Contains(" "))
            {
                //only one word in name -> try to find a more suitable name in list
                var nameline = linesbyheight.Where(x => x.Words.Count() == 2 && !x.Words.Any(y => y.Text.Equals(BusinessCard.CompanyName))).FirstOrDefault();                
                if (nameline != null)
                    BusinessCard.Name = string.Join(" ", nameline.Words.Select(x => x.Text).ToArray());
            }
        }

        private void ExtractEmail()
        {
            var emailregex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            var matches = emailregex.Matches(GetFullText());
            if (matches.Count < 1)
                return;
            BusinessCard.Email = matches[0].Value;
        }

        private void ExtractUrl()
        {
            var linkregex = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = linkregex.Matches(GetFullText());
            if (matches.Count > 0)
            {
                BusinessCard.Website = matches[0].Value;
                return;                
            }

            linkregex = new Regex("((|.)(se|com))+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var words = text.Regions.SelectMany(x => x.Lines).SelectMany(y => y.Words.Where(z => !z.Text.Equals(BusinessCard.Email) && linkregex.Matches(z.Text).Count > 0));
            if (words.Count() > 0)
            {
                BusinessCard.Website = words.First().Text;
                return;
            }
        }

        public string GetFullText()
        {
            var output = text.Regions.SelectMany(x => x.Lines).SelectMany(x => x.Words).Select(x => x.Text);
            return string.Join(" ", output);
        }
    }
}
