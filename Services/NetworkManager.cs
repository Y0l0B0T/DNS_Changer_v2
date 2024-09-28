using System.Collections.Generic;
using System.Management;
using DNSChanger.Interfaces;

namespace DNSChanger.Services
{
    public class NetworkManager : INetworkManager
    {
        public List<string> GetNetworkInterfaces()
        {
            var interfaces = new List<string>();
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = TRUE");

            foreach (ManagementObject obj in searcher.Get())
            {
                interfaces.Add(obj["SettingID"].ToString() + " - " + obj["Caption"].ToString());
            }

            return interfaces;
        }
    }
}