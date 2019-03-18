using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using dotnet_code_challenge.Models;
using dotnet_code_challenge.Helpers;


namespace dotnet_code_challenge.DataService
{
    public class RaceService
    {
        private static readonly ILogger logger = new LoggerImpl();
        private List<Race> races;

        public RaceService(List<Race> raceList)
        {
            races = raceList;
        }

        public List<Race> GetAllRaces()
        {
            var dataFolderPath = ConfigurationManager.AppSettings.Get("feedDataFolder");
            if (string.IsNullOrEmpty(dataFolderPath))
                Console.WriteLine("No data file path provided");

            processDataFiles(ReadFileName.ReadFileNames(dataFolderPath, "xml"), "xml");
            processDataFiles(ReadFileName.ReadFileNames(dataFolderPath, "json"), "json");
    
            return races;
        }

        private void processDataFiles(List<string> fileNames, string type)
        {
            if (fileNames != null)
            {
                fileNames.ForEach(s =>
                {
                    using ( var fs = new FileStream(s, FileMode.Open, FileAccess.Read))
                    {
                        if (type == "xml")
                            processXMLFile(fs);
                        else if(type == "json")
                            processJsonFile(fs);
                    }
                });
            }
        }

        private void processXMLFile(Stream fileStream)
        {
            if (fileStream == null)
                return;

            try
            {
                var xmlData = XElement.Load(fileStream);
                xmlData.Descendants("races").ToList<XElement>().ForEach(x => addRaceDetail(x));
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return;
        }

        private void processJsonFile(Stream fileStream)
        {
            if (fileStream == null)
                return;

            string jsonData;
            try
            {
                byte[] temp = new byte[fileStream.Length];
                fileStream.Read(temp, 0, temp.Length);
                jsonData = new UTF8Encoding(true).GetString(temp);

                var jsonObj = JObject.Parse(jsonData);
                var jsonFeed = JsonConvert.DeserializeObject<JsonFeed>(jsonData);
                jsonFeed.RawData.Markets.ForEach(x => addRaceDetail(x, jsonFeed.RawData.FixtureName));
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);

            }
        }

        private void addRaceDetail(Market raceData, string raceName)
        {
            if (raceData == null)
                return;

            var race = new Race();
            race.SetName(raceName);
            race.GetHorses().AddRange(raceData.GetHorses());
            races.Add(race);
        }

        private void addRaceDetail(XElement raceData)
        {
            if (raceData == null)
                return;

            var raceElement = raceData.Element("race");
            var name = raceElement.Attribute("name").Value;
            var horses = raceElement.Element("horses").Descendants("horse");
            var prices = raceElement.Element("prices").Element("price").Element("horses").Descendants("horse");

            var horseList = horses.Select(x => getHorse(x)).ToList<Horse>();

            horseList.ForEach(x =>
            {
                var price = prices.FirstOrDefault(p => p.Attribute("number").Value == x.Number);
                if (price != null)
                    x.Price = Convert.ToDecimal(price.Attribute("Price").Value);

            });

            var race = new Race();
            race.SetName(name);
            race.GetHorses().AddRange(horseList);
            races.Add(race);
        }

        private Horse getHorse(XElement horseElement)
        {
            if (horseElement == null)
                return null;

            var name = horseElement.Attribute("name").Value;
            var number = horseElement.Element("number").Value;
            var horse = new Horse(name, number);

            return horse;
        }

    }
}
