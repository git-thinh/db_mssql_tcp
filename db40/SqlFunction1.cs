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
}
