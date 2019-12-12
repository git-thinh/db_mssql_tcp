using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net.Sockets;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlBoolean cache___(String host, Int32 port, String message)
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
            return new SqlBoolean(false);
        }

        return new SqlBoolean(true);
    }
}
