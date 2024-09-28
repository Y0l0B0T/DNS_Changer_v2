using System;
using System.Collections.Generic;
using DNSChanger.Interfaces;
using DNSChanger.Models;
using DNSChanger.Services;
using DNSChanger.Utilities;

namespace DNSChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            string selectedNetwork = null;
            INetworkManager networkManager = new NetworkManager();
            IDNSManager dnsManager = new DNSManager();

            while (running)
            {
                if (selectedNetwork == null)
                {
                    ConsoleHelper.Clear();
                    ConsoleHelper.DisplayNetworkInterfaces(networkManager.GetNetworkInterfaces());

                    selectedNetwork = ConsoleHelper.SelectNetwork(networkManager.GetNetworkInterfaces());
                    if (selectedNetwork == null)
                    {
                        running = false;
                        break;
                    }
                }

                ConsoleHelper.Clear();
                ConsoleHelper.DisplayAvailableDNSOptions(dnsManager.GetDNSOptions());

                int dnsSelection = ConsoleHelper.SelectDNSOption();
                if (dnsSelection == 0) // Exit
                {
                    running = false;
                    break;
                }
                else if (dnsSelection == 16) // Clear DNS
                {
                    if (dnsManager.ClearDNS(selectedNetwork))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        ConsoleHelper.DisplayMessage("All DNS Settings Have been Cleared.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        ConsoleHelper.DisplayMessage("Failed to clear DNS settings. Run as administrator.");
                        Console.ResetColor();
                    }
                    ConsoleHelper.WaitForKey();
                    continue;
                }
                else if (dnsSelection == 17) // Return to Network Selection
                {
                    selectedNetwork = null;
                    continue;
                }

                var selectedDNS = dnsManager.GetDNSOptions()[dnsSelection];
                if (dnsManager.SetDNS(selectedNetwork, selectedDNS.Preferred, selectedDNS.Alternate))
                {
                    ConsoleHelper.DisplayMessage($"DNS has been set to {selectedDNS.Name}.");
                    dnsManager.PingDNS(selectedDNS.Preferred);
                    dnsManager.PingDNS(selectedDNS.Alternate);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    ConsoleHelper.DisplayMessage("Failed to set DNS. Run as administrator.");
                    Console.ResetColor();
                }
                ConsoleHelper.WaitForKey();
            }

            ConsoleHelper.DisplayMessage("Exiting the program. Goodbye!");
        }
    }
}
