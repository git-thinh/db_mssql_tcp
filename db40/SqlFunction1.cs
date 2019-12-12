using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net.Sockets;
using Microsoft.SqlServer.Server;
using db40;
using System.Collections.Generic;
using Newtonsoft.Json;

public partial class UserDefinedFunctions
{
    #region [ VALID ]

    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static SqlBoolean valid___number(String str_value)
    {
        bool ok = false;
        try { ok = _FUN_VALID.Is_Numeric(str_value); } catch { }
        return new SqlBoolean(ok);
    }

    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static SqlBoolean valid___number_yyyyMMdd(String str_value)
    {
        bool ok = false;
        try { ok = _FUN_VALID.Is_Numeric_yyyyMMdd(str_value); } catch { }
        return new SqlBoolean(ok);
    }

    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static SqlBoolean valid___number_HHmmss(String str_value)
    {
        bool ok = false;
        try { ok = _FUN_VALID.Is_Numeric_HHmmss(str_value); } catch { }
        return new SqlBoolean(ok);
    }

    #endregion

    #region [ RENDER ]

    [SqlFunction(DataAccess = DataAccessKind.None)]
    public static SqlString render___simple(String str_template, String str_json_value, Boolean bit_log_error = false)
    {
        string str_result = string.Empty;
        try {
            Dictionary<string, object> obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(str_json_value);
            str_result = _FUN_RENDER.Render(str_template, obj); 
        } catch(Exception ex) {
            if (bit_log_error) {
                return new SqlString(JsonConvert.SerializeObject(new CLS_RESULT { str_message = ex.Message }));
            }
        }
        return new SqlString(str_result);
    }
    
    #endregion

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
