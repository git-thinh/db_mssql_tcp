using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    const string CONST___TABLE_COFIG_COLUMN = "mobile.sys_col_config";
    const string CONST___TABLE_NOTIFY = "mobile.pol_notify";
    const string CONST___TABLE_NOTIFY_STATE = "mobile.pol_notify_state";

    #region [ BASE FUNCTIONS: ___find_index ]

    //static int ___find_index<T>(IEnumerable<T> items, Func<T, bool> predicate)
    //{
    //    if (items == null) throw new ArgumentNullException("items");
    //    if (predicate == null) throw new ArgumentNullException("predicate");

    //    int retVal = 0;
    //    foreach (var item in items)
    //    {
    //        if (predicate(item)) return retVal;
    //        retVal++;
    //    }
    //    return -1;
    //}



    #endregion 

    #region [ MODELS: CLS_RESULT, CLS_COLUMN, CLS_NOTIFY ]

    public class CLS_NOTIFY
    {
        public string str_actions { set; get; }
        public int? int_type { set; get; }
        public int? step_id { set; get; }
        public string ref_ids { set; get; }
        public string user_ids { set; get; }
        public string group_ids { set; get; }
        public string message { set; get; }
        public int? int_date { set; get; }
        public int? int_time { set; get; }
        public int? int_status { set; get; }
    }

    public class CLS_RESULT
    {
        public bool ok { set; get; }
        public string str_message { set; get; }
        public object obj_data { set; get; }
        public int? int_code { set; get; }

        public CLS_RESULT()
        {
            ok = false;
            int_code = 0;
            str_message = string.Empty;
            obj_data = null;
        }
    }

    public class CLS_COLUMN
    {
        public string str_table_name { set; get; }
        public string str_col_name { set; get; }
        public string str_title { set; get; }
        public string str_type_data { set; get; }
        public string str_format_data { set; get; }
        public string str_kit_form { set; get; }
        public string str_join_table { set; get; }
        public string str_join_col { set; get; }
        public int? int_join_type { set; get; }
        public string str_value_def { set; get; }
        public int? int_visiable { set; get; }
        public int? int_index { set; get; }
        public int? int_size { set; get; }
        public int? int_status { set; get; }
        public int? bit_nullable { set; get; }
        public string str_config_valids { set; get; }
        public string str_config_notify { set; get; }

        public object obj_value { set; get; }
        public string str_value_type { set; get; }

        public void CopyFrom(CLS_COLUMN o)
        {
            this.str_table_name = o.str_table_name;
            this.str_col_name = o.str_col_name;
            this.str_title = o.str_title;
            this.str_type_data = o.str_type_data;
            this.str_format_data = o.str_format_data;
            this.str_kit_form = o.str_kit_form;
            this.str_join_table = o.str_join_table;
            this.str_join_col = o.str_join_col;
            this.int_join_type = o.int_join_type;
            this.str_value_def = o.str_value_def;
            this.int_visiable = o.int_visiable;
            this.int_index = o.int_index;
            this.int_size = o.int_size;
            this.int_status = o.int_status;
            this.bit_nullable = o.bit_nullable;
            this.str_config_valids = o.str_config_valids;
            this.str_config_notify = o.str_config_notify;
        }
    }

    #endregion

    #region [ NOTIFY ]

    [SqlProcedure]
    public static SqlInt32 notify___all(Int32 ref_id, String str_message, ref String str_result)
    {
        if ( string.IsNullOrWhiteSpace(str_message))
        {
            str_result = JsonConvert.SerializeObject(new CLS_RESULT() { ok = false, str_message = "Error: user_id > 0 and str_message is not null or empty" }, Formatting.Indented);
            return new SqlInt32(0);
        }
         
        int id = 0;
        int id_state = 0;
        string str_error = string.Empty;
        string str_sql = string.Empty;

        try
        {

            str_sql = @" BEGIN TRY
                            BEGIN TRANSACTION
                                ------------------------------------------------------------------------------------
                                set @id = -1;
                                set @id_state = -1;

                                declare @table_ids table(id int);

                                insert into " + CONST___TABLE_NOTIFY + @"(str_actions,int_type,step_id,ref_ids,user_ids,group_ids,[message],int_date,int_time,int_status)
                                output INSERTED.id into @table_ids(id)
                                values(@str_actions,@int_type,@step_id,@ref_ids,@user_ids,@group_ids,@message,@int_date,@int_time,@int_status);

                                select top 1 @id = id from @table_ids; 
                                delete from @table_ids;

                                if @id > 0
                                begin
                                    insert into mobile.pol_notify_state(user_id,notify_id,int_status,int_date,int_time,int_last_date,int_last_time)
                                    output INSERTED.id into @table_ids(id)
                                    values(@user_id,@id,0,@int_date,@int_time,@int_date,@int_time);

                                    select top 1 @id_state = id from @table_ids; 
                                end
                                ------------------------------------------------------------------------------------
                                COMMIT
                        END TRY
                        BEGIN CATCH
                            ROLLBACK
                        END CATCH ";


            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                using (SqlCommand c = new SqlCommand(str_sql, connection))
                {
                    c.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    c.Parameters.Add("@id_state", SqlDbType.Int).Direction = ParameterDirection.Output;

                    c.Parameters.AddWithValue("@str_actions", string.Empty);
                    c.Parameters.AddWithValue("@int_type", 0);
                    c.Parameters.AddWithValue("@step_id", 0);
                    c.Parameters.AddWithValue("@ref_ids", "," + ref_id + ","); 
                    c.Parameters.AddWithValue("@group_ids", string.Empty);
                    c.Parameters.AddWithValue("@message", str_message);
                    c.Parameters.AddWithValue("@int_date", int.Parse(DateTime.Now.ToString("yyyyMMdd")));
                    c.Parameters.AddWithValue("@int_time", int.Parse(DateTime.Now.ToString("HHmmss")));
                    c.Parameters.AddWithValue("@int_status", 0);

                    connection.Open();
                    c.ExecuteNonQuery();

                    id = Convert.ToInt32(c.Parameters["@id"].Value);
                    id_state = Convert.ToInt32(c.Parameters["@id_state"].Value);
                    connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            str_error = ex.Message;
        }

        str_result = JsonConvert.SerializeObject(new CLS_RESULT()
        {
            ok = (id > 0 && id_state > 0),
            str_message = str_error,
            obj_data = new { notify_id = id, notify_state_id = id_state }
        }, Formatting.Indented);

        return new SqlInt32((id > 0 && id_state > 0) ? 1 : 0);
    }

    [SqlProcedure]
    public static SqlInt32 notify___user(Int32 ref_id, Int32 user_id, String str_message, ref String str_result)
    {
        if (user_id <= 0 || string.IsNullOrWhiteSpace(str_message))
        {
            str_result = JsonConvert.SerializeObject(new CLS_RESULT() { ok = false, str_message = "Error: user_id > 0 and str_message is not null or empty" }, Formatting.Indented);
            return new SqlInt32(0);
        }

        int id = 0;
        int id_state = 0;
        string str_error = string.Empty;
        string str_sql = string.Empty;

        try
        {

            str_sql = @" BEGIN TRY
                            BEGIN TRANSACTION
                                ------------------------------------------------------------------------------------
                                set @id = -1;
                                set @id_state = -1;

                                declare @table_ids table(id int);

                                insert into " + CONST___TABLE_NOTIFY + @"(str_actions,int_type,step_id,ref_ids,user_ids,group_ids,[message],int_date,int_time,int_status)
                                output INSERTED.id into @table_ids(id)
                                values(@str_actions,@int_type,@step_id,@ref_ids,@user_ids,@group_ids,@message,@int_date,@int_time,@int_status);

                                select top 1 @id = id from @table_ids; 
                                delete from @table_ids;

                                if @id > 0
                                begin
                                    insert into mobile.pol_notify_state(user_id,notify_id,int_status,int_date,int_time,int_last_date,int_last_time)
                                    output INSERTED.id into @table_ids(id)
                                    values(@user_id,@id,0,@int_date,@int_time,@int_date,@int_time);

                                    select top 1 @id_state = id from @table_ids; 
                                    delete from @table_ids;
                                end
                                ------------------------------------------------------------------------------------
                                COMMIT
                        END TRY
                        BEGIN CATCH
                            ROLLBACK
                        END CATCH ";

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                using (SqlCommand c = new SqlCommand(str_sql, connection))
                {
                    c.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    c.Parameters.Add("@id_state", SqlDbType.Int).Direction = ParameterDirection.Output;

                    c.Parameters.AddWithValue("@str_actions", string.Empty);
                    c.Parameters.AddWithValue("@int_type", 0);
                    c.Parameters.AddWithValue("@step_id", 0);
                    c.Parameters.AddWithValue("@ref_ids", "," + ref_id + ",");
                    c.Parameters.AddWithValue("@user_ids", "," + user_id + ",");
                    c.Parameters.AddWithValue("@user_id", user_id);
                    c.Parameters.AddWithValue("@group_ids", string.Empty);
                    c.Parameters.AddWithValue("@message", str_message);
                    c.Parameters.AddWithValue("@int_date", int.Parse(DateTime.Now.ToString("yyyyMMdd")));
                    c.Parameters.AddWithValue("@int_time", int.Parse(DateTime.Now.ToString("HHmmss")));
                    c.Parameters.AddWithValue("@int_status", 0);

                    connection.Open();
                    c.ExecuteNonQuery();

                    id = Convert.ToInt32(c.Parameters["@id"].Value);
                    id_state = Convert.ToInt32(c.Parameters["@id_state"].Value);
                    connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            str_error = ex.Message;
        }

        str_result = JsonConvert.SerializeObject(new CLS_RESULT() { 
            ok = (id > 0 && id_state > 0), 
            str_message = str_error, 
            obj_data = new { notify_id = id, notify_state_id = id_state } 
        }, Formatting.Indented);

        return new SqlInt32((id > 0 && id_state > 0) ? 1 : 0);
    }
    
    [SqlProcedure]
    public static SqlInt32 notify___users(Int32 ref_id, String user_ids, String str_message, ref String str_result)
    {
        if (string.IsNullOrWhiteSpace(user_ids) || string.IsNullOrWhiteSpace(str_message))
        {
            str_result = JsonConvert.SerializeObject(new CLS_RESULT() { ok = false, str_message = "Error: user_id > 0 and str_message is not null or empty" }, Formatting.Indented);
            return new SqlInt32(0);
        }

        string[] a = user_ids.Split(',').Where(x => x.Trim().Length > 0).ToArray();
        if (a.Length == 0)
        {
            str_result = JsonConvert.SerializeObject(new CLS_RESULT() { ok = false, str_message = "Error: user_ids are id1,id2,..." }, Formatting.Indented);
            return new SqlInt32(0);
        }
        string str_user_ids = "," + string.Join(",", a) + ",";

        int id = 0;
        int id_state = 0;
        string str_error = string.Empty;
        string str_sql = string.Empty;

        try
        {

            str_sql = @" BEGIN TRY
                            BEGIN TRANSACTION
                                ------------------------------------------------------------------------------------
                                set @id = -1;
                                set @id_state = -1;

                                declare @table_ids table(id int);

                                insert into " + CONST___TABLE_NOTIFY + @"(str_actions,int_type,step_id,ref_ids,user_ids,group_ids,[message],int_date,int_time,int_status)
                                output INSERTED.id into @table_ids(id)
                                values(@str_actions,@int_type,@step_id,@ref_ids,@user_ids,@group_ids,@message,@int_date,@int_time,@int_status);

                                select top 1 @id = id from @table_ids; 
                                delete from @table_ids;

                                if @id > 0
                                begin
                                    insert into mobile.pol_notify_state(user_id,notify_id,int_status,int_date,int_time,int_last_date,int_last_time)
                                    output INSERTED.id into @table_ids(id)
                                    values(@user_id,@id,0,@int_date,@int_time,@int_date,@int_time);

                                    select top 1 @id_state = id from @table_ids; 
                                end
                                ------------------------------------------------------------------------------------
                                COMMIT
                        END TRY
                        BEGIN CATCH
                            ROLLBACK
                        END CATCH ";


            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                using (SqlCommand c = new SqlCommand(str_sql, connection))
                {
                    c.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    c.Parameters.Add("@id_state", SqlDbType.Int).Direction = ParameterDirection.Output;

                    c.Parameters.AddWithValue("@str_actions", string.Empty);
                    c.Parameters.AddWithValue("@int_type", 0);
                    c.Parameters.AddWithValue("@step_id", 0);
                    c.Parameters.AddWithValue("@ref_ids", "," + ref_id + ",");
                    c.Parameters.AddWithValue("@user_ids", str_user_ids);
                    c.Parameters.AddWithValue("@group_ids", string.Empty);
                    c.Parameters.AddWithValue("@message", str_message);
                    c.Parameters.AddWithValue("@int_date", int.Parse(DateTime.Now.ToString("yyyyMMdd")));
                    c.Parameters.AddWithValue("@int_time", int.Parse(DateTime.Now.ToString("HHmmss")));
                    c.Parameters.AddWithValue("@int_status", 0);

                    connection.Open();
                    c.ExecuteNonQuery();

                    id = Convert.ToInt32(c.Parameters["@id"].Value);
                    id_state = Convert.ToInt32(c.Parameters["@id_state"].Value);
                    connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            str_error = ex.Message;
        }

        str_result = JsonConvert.SerializeObject(new CLS_RESULT()
        {
            ok = (id > 0 && id_state > 0),
            str_message = str_error,
            obj_data = new { notify_id = id, notify_state_id = id_state }
        }, Formatting.Indented);

        return new SqlInt32((id > 0 && id_state > 0) ? 1 : 0);
    }

    #endregion

    #region [ TCP CLIENT: PUSH TO CACHE ENGINE ]

    #endregion
}
