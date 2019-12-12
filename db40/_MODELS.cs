using System;
using System.Collections.Generic;
using System.Text;

namespace db40
{
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
        public _ERROR int_code { set; get; }

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
}
