using System.Collections.Generic;
using DNSChanger.Models;

namespace DNSChanger.Interfaces
{
    public interface IDNSManager
    {
        bool SetDNS(string settingID, string preferredDNS, string alternateDNS);
        bool ClearDNS(string settingID);
        List<DNSOption> GetDNSOptions();
        (bool, long) PingDNS(string dnsAddress);
    }
}