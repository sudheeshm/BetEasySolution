using System;
using dotnet_code_challenge.DataService;
using System.Collections.Generic;
using dotnet_code_challenge.Models;


namespace dotnet_code_challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BetEasy Solution!");
            Console.WriteLine();

            List<Race> races = new List<Race>();
            new RaceService(races).GetAllRaces();

            races.ForEach(x => x.Print());
            Console.ReadKey();
        }
    }
}
