using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using dotnet_code_challenge.Models;
using System.Xml.Linq;
using System.Linq;

namespace dotnet_code_challenge.DataService
{
    public class RaceService
    {
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

            var xmlFiles = ReadFileName.ReadFileNames(dataFolderPath, "xml");
            var jsonFiles = ReadFileName.ReadFileNames(dataFolderPath, "json");

            if (xmlFiles != null)
            {
                xmlFiles.ForEach(s => {
                    var fs = new FileStream(s, FileMode.Open, FileAccess.Read);
                    processXMLFile(fs);
                });
            }

            //if(jsonFiles != null)
            //jsonFiles.ForEach(s => processXMLFile(s));

            return races;
        }

        private void processXMLFile1(Stream fileStream)
        {
            if (fileStream == null)
                return;

            var xmlData = XElement.Load(fileStream);
            xmlData.Descendants("races")?.ToList<XElement>().ForEach(x => addRaceDetail(x));
        }

        private void processXMLFile(Stream fileStream)
        {
            if (fileStream == null)
                return;

            var xmlData = XElement.Load(fileStream);
            xmlData.Descendants("races")?.ToList<XElement>().ForEach(x => addRaceDetail(x));

            return;
        }


        private void addRaceDetail(XElement raceData)
        {
            if (raceData == null)
                return;

            var name = raceData.Element("race").Attribute("name").Value;
            //var horses = raceData.Element("race").Descendants("horses");
            var horses = raceData.Element("race").Element("horses").Descendants("horse");
            var prices = raceData.Element("race").Element("prices").Element("price").Element("horses").Descendants("horse");

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
