using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace dotnet_code_challenge.Models
{
    public class JsonFeed
    {
        [JsonProperty("RawData")]
        public RawData RawData { get; set; }
    }

    public class RawData
    {
        [JsonProperty("FixtureName")]
        public string FixtureName { get; set; }

        [JsonProperty("Markets")]
        public List<Market> Markets { get; set; }
    }
    public class Market
    {
        [JsonProperty("Selections")]
        public List<Selection> Selections { get; set; }

        public List<Horse> GetHorses()
        {
            var horses = new List<Horse>();
            Selections.ForEach(x =>
            {
                horses.Add(new Horse(x.Tag.Name, x.Tag.Number, Convert.ToDecimal(x.Price)));
            });
            return horses;
        }
    }

    public class Selection
    {
        [JsonProperty("Price")]
        public string Price { get; set; }

        [JsonProperty("Tags")]
        public Tag Tag {get; set; }
    }

    public class Tag
    { 
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("participant")]
        public string Number { get; set; }
    }
}
