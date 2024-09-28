using System;
using System.Collections.Generic;
using System.Management;
using System.Net.NetworkInformation;
using DNSChanger.Interfaces;
using DNSChanger.Models;

namespace DNSChanger.Services
{
    public class DNSManager : IDNSManager
    {
        private Dictionary<int, DNSOption> dnsOptions = new Dictionary<int, DNSOption>
        {
            { 1, new DNSOption("Electro DNS  ", "78.157.42.100", "78.157.42.101") },
            { 2, new DNSOption("Shecan  DNS  ", "178.22.122.100", "185.51.200.2") },
            { 3, new DNSOption("Radar Game   ", "10.202.10.10", "10.202.10.11") },
            { 4, new DNSOption("403 Online   ", "10.202.10.202", "10.202.10.102") },
            { 5, new DNSOption("Begzar DNS   ", "185.55.226.26", "185.55.225.25") },
            { 6, new DNSOption("Google DNS   ", "8.8.8.8", "8.8.4.4") },
            { 7, new DNSOption("Cloudflare   ", "1.1.1.1", "1.0.0.1") },
            { 8, new DNSOption("Quad9 DNS    ", "9.9.9.9", "9.9.9.10") },
            { 9, new DNSOption("OpenDNS      ", "208.67.222.222", "208.67.220.220") },
            { 10, new DNSOption("Neustar DNS ", "156.154.71.5", "156.154.70.5") },
            { 11, new DNSOption("Alternate   ", "76.76.19.19", "76.223.122.150") },
            { 12, new DNSOption("NTT DNS     ", "129.250.35.250", "129.250.35.251") },
            { 13, new DNSOption("Sprintlink  ", "204.117.214.10", "199.2.252.10") },
            { 14, new DNSOption("Adguard DNS ", "94.140.14.14", "94.140.15.15") },
            { 15, new DNSOption("Level3 DNS  ", "209.244.0.3", "209.244.0.4") }
        };

        public List<DNSOption> GetDNSOptions()
        {
            return new List<DNSOption>(dnsOptions.Values);
        }

        public bool SetDNS(string settingID, string preferredDNS, string alternateDNS)
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE SettingID = '{settingID}'");
                foreach (ManagementObject obj in searcher.Get())
                {
                    ManagementBaseObject newDNS = obj.GetMethodParameters("SetDNSServerSearchOrder");
                    newDNS["DNSServerSearchOrder"] = new string[] { preferredDNS, alternateDNS };
                    ManagementBaseObject setDNSResult = obj.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                    if (setDNSResult != null && (uint)setDNSResult["ReturnValue"] == 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting DNS: {ex.Message}");
            }
            return false;
        }

        public bool ClearDNS(string settingID)
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE SettingID = '{settingID}'");
                foreach (ManagementObject obj in searcher.Get())
                {
                    ManagementBaseObject clearDNSResult = obj.InvokeMethod("SetDNSServerSearchOrder", null, null);
                    if (clearDNSResult != null && (uint)clearDNSResult["ReturnValue"] == 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Clearing DNS: {ex.Message}");
            }
            return false;
        }

        public (bool, long) PingDNS(string dnsAddress)
        {
            Ping ping = new Ping();
            try
            {
                PingReply reply = ping.Send(dnsAddress, 1000);
                if (reply.Status == IPStatus.Success)
                {
                    return (true, reply.RoundtripTime);
                }
            }
            catch { }
            return (false, -1);
        }
    }
}
