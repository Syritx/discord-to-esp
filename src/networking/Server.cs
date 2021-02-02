using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Threading;

namespace esp_discord_bot.src.networking {
    
    class Server {

        Socket serverSocket;
        Socket ESP;

        public Server() {
            
            IPAddress HOST = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            const int PORT = 6060;
            IPEndPoint endPoint = new IPEndPoint(HOST, PORT);

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(endPoint);
            serverSocket.Listen(5);

            Task.Run(() => WaitForClients());
        }

        public void SendToESP(string command) {

            byte[] buffer = Encoding.UTF8.GetBytes(command);
            ESP.Send(buffer);
        }

        void WaitForClients() {

            Console.WriteLine("waiting for clients");

            while (true) {
                Socket client = serverSocket.Accept();
                if (ESP != null) ESP.Close();
                ESP = client;
                Console.WriteLine("connection");
            }
        }
    }
}