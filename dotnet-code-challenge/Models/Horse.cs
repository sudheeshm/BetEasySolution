using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet_code_challenge.Models
{
    public class Horse
    {
        public string Name { get; private set; }
        public string Number { get; private set; }
        public decimal Price { get; set; }

        public Horse(string name, string number)
        {
            Name = name;
            Number = number;
        }

        public void Print()
        {
            Console.WriteLine(Name + " " + Price.ToString());
        }
    }
}
