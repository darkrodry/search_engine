using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAI.Crawler.Run
{
    class Program
    {
        static void Main(string[] args)
        {
            RAI.Crawler.GoogleCrawler crawler = new RAI.Crawler.GoogleCrawler();
            crawler.AddSeed("http://www.google.com/search?q=information+retrieval");
            crawler.Run();
            crawler.Dispose();
        }
    }
}
