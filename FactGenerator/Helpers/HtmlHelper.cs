using FactGenerator.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FactGenerator.Helpers
{
    public class HtmlHelper
    {
        private static List<string> Links = new List<string>();
        private static List<Fact> Facts = new List<Fact>();
        public static async Task RunAsync()
        {
            LoadLinks();
            var count = 1;
            foreach (var link in Links)
            {
                var web = new HtmlWeb();
                var doc = web.Load(link);
                var factLink = doc.DocumentNode.SelectSingleNode("//div[@class='copy-container']//p//a").Attributes["href"].Value;

                if (factLink is not null || factLink != "")
                {
                    var newPage = web.Load(factLink);
                    var factNodes = newPage.DocumentNode.SelectNodes("//div[@class='entry-content']//p");

                    for (int i = 1; i < factNodes.Count; i++)
                    {
                        var details = link.Split('/');
                        var cat = details[5];
                        var fact = new Fact()
                        {
                            Id = Guid.NewGuid(),
                            Category = cat,
                            Link = factLink,
                            Content = factNodes[i].InnerText
                        };
                        Facts.Add(fact);
                        //await SaveToXml(fact);
                       // await Task.Delay(200);
                        Console.WriteLine($"{count} complete");
                        count++;
                    }

                }
               
            }
            await SaveToJson(Facts);

        }

        private static async Task SaveToJson(List<Fact> facts)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(facts, options);
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "fact.json");
            await File.WriteAllTextAsync(jsonPath, json);
        }

        private static void LoadLinks()
        {
            Links.Add("https://onlyfunfacts.com/category/history/war/");
            Links.Add("https://onlyfunfacts.com/category/history/politics/");
            Links.Add("https://onlyfunfacts.com/category/life/animals/");
            Links.Add("https://onlyfunfacts.com/category/life/food/");
            Links.Add("https://onlyfunfacts.com/category/life/human/");
            Links.Add("https://onlyfunfacts.com/category/life/world/");
            Links.Add("https://onlyfunfacts.com/category/entertainment/internet/");
            Links.Add("https://onlyfunfacts.com/category/science/health/");
            Links.Add("https://onlyfunfacts.com/category/science/technology/");
            Links.Add("https://onlyfunfacts.com/category/science/space/");
            Links.Add("https://onlyfunfacts.com/category/fun-facts/");
        }

        private static async Task SaveToXml(Fact fact)
        {
            var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "fact.xml");
            if (!File.Exists(xmlPath))
            {
                var doc = new XDocument(
                                new XDeclaration("1.0", "utf-8", "yes"),
                                new XElement("facts",
                                    new XElement("fact",
                                        new XElement("id", fact.Id),
                                        new XElement("link", fact.Link),
                                        new XElement("category", fact.Category),
                                        new XElement("content", fact.Content))));
                using (var fs = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    await doc.SaveAsync(fs, SaveOptions.None, default);
                }
            }
            else
            {

                var doc = XDocument.Load(xmlPath);
                var root = doc.Root!;
                var element = new XElement("fact",
                                new XElement("id", fact.Id),
                                new XElement("link", fact.Link),
                                new XElement("category", fact.Category),
                                new XElement("content", fact.Content));
                root.Add(element);
                using (var fs = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    await doc.SaveAsync(fs, SaveOptions.None, default);
                } 
            }
        }
    }
}
