using System.Collections.Generic;

namespace DNSChanger.Interfaces
{
    public interface INetworkManager
    {
        List<string> GetNetworkInterfaces();
    }
}