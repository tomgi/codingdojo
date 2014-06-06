using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BooksPricingFrenzy
{
    class Program
    {

        public static void Upload(string isbn)
        {
            using (var client = new HttpClient())
            {
//                client.BaseAddress = new Uri(string.Format("http://www.amazon.com/s/field-keywords={0}", isbn));
                var response = client.GetAsync(string.Format("http://www.amazon.com/s/field-keywords={0}", isbn)).Result;
                //Console.WriteLine(logonResponse.Content.ReadAsStringAsync());
                //var response = client.GetAsync(@"restore?archivedpath=C:\archive\").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var responseString = responseContent.ReadAsStringAsync().Result;
                    File.WriteAllText(@"c:\a\page.html", responseString);
                }
            }
        }

        static void Main(string[] args)
        {
            var bookName = args[0];
            Console.WriteLine(bookName);
            using (WebClient client = new WebClient())
            {
                var downloadString = client.DownloadString(string.Format("https://www.googleapis.com/books/v1/volumes?q=intitle:={0}", bookName));
                dynamic json = JObject.Parse(downloadString);
                var isbn = json.items[0].volumeInfo.industryIdentifiers[0].identifier;
                Console.WriteLine(isbn.ToString());
                Upload(isbn.ToString());
//                var address = string.Format("http://www.amazon.com/s/field-keywords={0}", isbn.ToString());
//                Console.WriteLine(address);
//                string amazonBookPage = client.DownloadString(address);
                //Console.WriteLine(amazonBookPage);
//                File.WriteAllText(@"c:\a\page.html",amazonBookPage);
//                var pageFromPriceInfoToEnd = amazonBookPage.Substring(amazonBookPage.IndexOf("<li class=\"newp\">"));
//                Console.WriteLine(pageFromPriceInfoToEnd);
                //var priceXml = pageFromPriceInfoToEnd.Substring(pageFromPriceInfoToEnd.IndexOf("</li>"));
                //Console.WriteLine(priceXml);

            }
            
        }
    }
}
