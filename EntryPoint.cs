using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

internal class EntryPoint
{
    static void Main()
    {
        //ChromeOptions options = new ChromeOptions();
        //   options.AddArgument("--headless");
        //  IWebDriver driver = new ChromeDriver(options);

        IWebDriver driver = new PhantomJSDriver();
        IWebElement titleElement;
        IWebElement contentElement;



        string sitemapUrl = "http://testing.todvachev.com/sitemap-posttype-post.xml";
        string titleSelector = "#main-content > article > header > h1";
        string contentSelector = "#main-content > article > div";

        int linkLength = 0;
        int startIndex = 0;

        string[] pageSource;

        List<string> extractedLinks = new List<string>();
        List<string> extractedTitle = new List<string>();
        List<string> extractedContent = new List<string>();

        pageSource = driver.PageSource.Split(' ');

        driver.Navigate().GoToUrl(sitemapUrl);

        foreach (var item in pageSource)
        {
            if (item.Contains("<loc>"))
            {
                startIndex = item.IndexOf("<loc>") + 5;
                linkLength = item.IndexOf("</loc>") - startIndex;

                extractedLinks.Add(item.Substring(startIndex,linkLength));

               
                Console.WriteLine(item.Substring(startIndex, linkLength));
                Thread.Sleep(70000);
            }

            foreach (var section in extractedLinks)
            {
                driver.Navigate().GoToUrl(section);

                titleElement = driver.FindElement(By.CssSelector(titleSelector));
                contentElement = driver.FindElement(By.CssSelector(contentSelector));

                

                extractedContent.Add(contentElement.Text);
                extractedTitle.Add(titleElement.Text);


            }

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\ExtractedContent");

            using (StreamWriter sw = File.CreateText(Directory.GetCurrentDirectory() + @"\ExtractedContent\ExtractedTest.txt"))
            {
                sw.WriteLine("TITLE: {0}", extractedTitle[0]);
                sw.WriteLine("CONTENT ");
                sw.Write(extractedContent[0]);
            }
        }

       

      //  Console.WriteLine(driver.PageSource);



    }
}

