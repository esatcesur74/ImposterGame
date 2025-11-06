using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http.HttpResults;
namespace ImposterGame.Services
{
    public interface INetworkService
    {
        string GetLocalIPAddress();
    }

    public class NetworkService : INetworkService
    {
        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }

            }
            return "No IPv4 address found";
        }
    }






}