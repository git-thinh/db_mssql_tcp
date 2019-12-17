using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net.Sockets;
using Microsoft.SqlServer.Server;
using db40;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO.MemoryMappedFiles;
using Newtonsoft.Json.Linq;
using System.Collections;

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
        Dictionary<string, object> obj = null;

        try {
            obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(str_json_value);
        } catch(Exception ex) {
            if (bit_log_error) {
                return new SqlString(JsonConvert.SerializeObject(new CLS_RESULT { int_code = _ERROR.JSON_CONVERT, str_message = ex.Message }));
            }
        }

        str_result = _FUN_RENDER.Render(str_template, obj);
        return new SqlString(str_result);
    }

    #endregion

    #region [ TCP ]

    //[SqlFunction(DataAccess = DataAccessKind.Read)]
    //public static SqlString tcp___setup(String host, Int32 port)
    //{
    //    //try
    //    //{
    //    //    const string NAME = "MyMappedFile";
    //    //    MemoryMappedFile map = MemoryMappedFile.Create(MapProtection.PageReadWrite, MAX_BYTES, NAME);

    //    //    MemoryMappedFile map2 = MemoryMappedFile.Open(MapAccess.FileMapRead, NAME);
    //    //    map2.Close();
    //    //    map.Close();
    //    //}
    //    //catch //(FileMapIOException e)
    //    //{
    //    //    //Assert.Fail("Failed Named MMF: " + e);
    //    //}
    //}

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlBoolean tcp___number(String host, Int32 port, Double number)
    {
        return _FUN_TCP.Send(host, port, number.ToString());
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlBoolean tcp___text(String host, Int32 port, String message)
    {
        return _FUN_TCP.Send(host, port, message);
    }

    #endregion



    #region [ VERSION OLD ]

    static int count = 0;

    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlString ___query_json(String query)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //string json = _convert_Json(dt);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                return new SqlString(json);
                //return new SqlString("");
            }
        }
        catch (Exception ex)
        {
            return new SqlString(string.Format("!:{0}", ex.Message));
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlBoolean ___send_message(String host, Int32 port, String message)
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


    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString JsonValue(SqlString json, SqlString path)
    {
        try
        {
            JObject ja = (JObject)JsonConvert.DeserializeObject(json.Value);
            JToken token = ja.SelectToken(path.Value);

            return token.ToString();
        }
        catch
        {
            return null;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString JsonArrayValue(SqlString json, SqlInt32 rowindex, SqlString key)
    {
        JArray ja = (JArray)JsonConvert.DeserializeObject(json.Value);
        string re = ja[rowindex.Value][key.Value].ToString();
        return new SqlString(re);
    }

    public static void FillRowFromJson(Object token, out SqlString path, out SqlString value, out SqlString type, out SqlBoolean hasvalues, out SqlInt32 index)
    {
        JToken item = (JToken)token;
        path = item.Path;
        type = item.Type.ToString();
        hasvalues = item.HasValues;
        value = item.ToString();
        index = count;
        count++;
    }

    [SqlFunction(FillRowMethodName = "FillRowFromJson", TableDefinition = "[path] nvarchar(4000), [value] nvarchar(max), [type] nvarchar(4000), hasvalues bit, [index] int")]
    public static IEnumerable JsonTable(SqlString json, SqlString path)
    {
        ArrayList TokenCollection = new ArrayList();
        count = 0;

        try
        {
            JObject ja = (JObject)JsonConvert.DeserializeObject(json.Value);
            IEnumerable<JToken> tokens = ja.SelectTokens(path.Value);

            foreach (JToken token in tokens)
            {
                if (token.Type == JTokenType.Object || token.Type == JTokenType.Array)
                {
                    foreach (JToken item in token.Children<JToken>())
                    {
                        TokenCollection.Add(item);
                    }
                }
                else
                {
                    TokenCollection.Add(token);
                }
            }

            return TokenCollection;

        }
        catch
        {
            return null;
        }
    }

    #endregion
}
