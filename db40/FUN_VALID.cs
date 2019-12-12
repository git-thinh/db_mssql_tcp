using System;
using System.Collections.Generic;
using System.Text;

namespace db40
{
    public static class FUN_VALID
    {
        public static bool Is_Numeric(object value)
        {
            if (value == null) return false;

            double retNum;            
            bool isNum = Double.TryParse(Convert.ToString(value), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static bool Is_Numeric_yyyyMMdd(object value)
        {
            if (value == null) return false;

            bool ok = Is_Numeric(value);
            if (ok) {
                string s = value.ToString();
                if (s.Length != 8) return false;

                if (s[0] == '0' || s.StartsWith("0000")) return false;

                int m = int.Parse(s.Substring(4, 2));
                if (m > 12 || m == 0) return false;

                int d = int.Parse(s.Substring(6, 2));
                if (d > 31 || d == 0) return false;

                return true;
            }

            return false;
        }

        public static bool Is_Numeric_HHmmss(object value)
        {
            if (value == null) return false;

            bool ok = Is_Numeric(value);
            if (ok) {
                string s = value.ToString();
                if (s.Length == 5) s = "0" + s;
                if (s.Length != 6) return false;

                int h = int.Parse(s.Substring(0, 2));
                if (h > 12) return false;

                int m = int.Parse(s.Substring(2, 2));
                if (m > 60) return false;

                int se = int.Parse(s.Substring(4, 2));
                if (se > 60) return false;

                return true;
            }

            return false;
        }





    }
}
