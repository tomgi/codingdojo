using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace BooksPricingFrenzy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bookName = args[0];
            using (var client = new WebClient())
            {
                var downloadString = client.DownloadString(string.Format("https://www.googleapis.com/books/v1/volumes?q=intitle:={0}", bookName));
                dynamic json = JObject.Parse(downloadString);
                var isbn = json.items[0].volumeInfo.industryIdentifiers[0].identifier;
                var address = string.Format("http://www.amazon.com/s/field-keywords={0}", isbn.ToString());
                string bookPage = client.DownloadString(address);
                File.WriteAllText(@"c:\a\page.html", bookPage);
                var pageFromPriceInfoToEnd = bookPage.Substring(bookPage.IndexOf("<li class=\"newp\">"));
                var priceXml = pageFromPriceInfoToEnd.Substring(0, pageFromPriceInfoToEnd.IndexOf("</li>") + "</li>".Length);
                var price = XDocument.Parse(priceXml).Element("li").Element("div").Element("a").Element("span").Value;
                Console.WriteLine(price);


                address = string.Format("http://www.apress.com/{0}", isbn.ToString());
                bookPage = client.DownloadString(address);
                pageFromPriceInfoToEnd = bookPage.Substring(bookPage.IndexOf("<ul class=\"prices\">"));
                priceXml = pageFromPriceInfoToEnd.Substring(0, pageFromPriceInfoToEnd.IndexOf("</ul>") + "</ul>".Length);
                price = XDocument.Parse(priceXml).Element("ul").Element("li").Element("strong").Value;
                Console.WriteLine(price);
            }
        }
    }
}