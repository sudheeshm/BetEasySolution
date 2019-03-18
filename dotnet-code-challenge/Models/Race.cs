using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotnet_code_challenge.Models
{
    public interface IRace
    {
        string GetName();
        void SetName(string name);
        List<Horse> GetHorses();
    }

    public class Race : IRace
    {
        private string name;
        private List<Horse> horses = new List<Horse>();

        public string GetName() => name;
        public List<Horse> GetHorses() => horses;
        public void SetName(string name) { this.name = name; }

        public void Print()
        {
            var SortedList = horses.OrderBy(o => o.Price).ToList();
            Console.WriteLine("Race Name: " + name);
            SortedList.ForEach(x => x.Print());

            Console.WriteLine();
        }
    }
}
