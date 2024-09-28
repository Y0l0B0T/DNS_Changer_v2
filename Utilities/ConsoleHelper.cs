using System;
using System.Collections.Generic;
using DNSChanger.Models;
using DNSChanger.Services;

namespace DNSChanger.Utilities
{
    public static class ConsoleHelper
    {
        public static void Clear() => Console.Clear();

        public static void DisplayNetworkInterfaces(List<string> networks)
        {
            Console.WriteLine("Available Network interfaces:");
            for (int i = 0; i < networks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {networks[i]}");
            }
        }

        public static string SelectNetwork(List<string> networks)
        {
            Console.Write("Select a network interface by number (number or 0 to exit): ");
            int selection = int.Parse(Console.ReadLine()) - 1;

            if (selection == -1)
                return null;

            if (selection < 0 || selection >= networks.Count)
            {
                Console.WriteLine("Invalid Selection !! Press any key to try again.");
                Console.ReadKey();
                return SelectNetwork(networks);
            }

            return networks[selection].Split('-')[0].Trim();
        }

        public static void DisplayAvailableDNSOptions(List<DNSOption> dnsOptions)
        {
            Console.WriteLine("\nAvailable DNS with Ping Results !:");
            int colorToggle = 0;
            foreach (var dns in dnsOptions)
            {
                Console.ForegroundColor = (colorToggle % 2 == 0) ? ConsoleColor.Cyan : ConsoleColor.Magenta;
                Console.Write($"{dns.Name}\t");
                var pingResult = new DNSManager().PingDNS(dns.Preferred);
                if (pingResult.Item1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{pingResult.Item2} ms");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ping failed !!");
                }
                colorToggle++;
            }
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("16. Clear All DNS ");
            Console.ResetColor();
            Console.WriteLine("17. Return to Network Selection");
            Console.WriteLine("0. Exit App");
        }

        public static int SelectDNSOption()
        {
            Console.Write("Select a DNS by Number : ");
            return int.Parse(Console.ReadLine());
        }

        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void WaitForKey()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
