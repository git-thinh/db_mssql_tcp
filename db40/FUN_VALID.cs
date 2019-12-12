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
            if (ok == false) return ok;

        }


    }
}
