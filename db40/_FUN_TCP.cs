using Mustache;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace db40
{
    public static class _FUN_TCP
    {
        public static bool Send(String host, Int32 port, String message)
        {
            try
            {
                TcpClient client = new TcpClient();
                //client.Connect("52.77.82.145", 80);
                //client.Connect("127.0.0.1", 3456);
                client.Connect(host, port);

                //var client = new TcpClient("apimobi.f88.vn", 80);
                //var client = new TcpClient("localhost", 9015);
                //var client = new TcpClient("localhost", 3456);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length); //sends bytes to server

                stream.Close();
                client.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
