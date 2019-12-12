using Mustache;
using System;
using System.Collections.Generic;
using System.Text;

namespace db40
{
    public static class _FUN_RENDER
    {
        public static string Render(string str_format, object obj_value)
        {
            if (obj_value == null) obj_value = new object();
            
            try
            {
                FormatCompiler compiler = new FormatCompiler();
                Generator generator = compiler.Compile(str_format);
                return generator.Render(obj_value);
            }
            catch { }

            return string.Empty;
        }
    }
}
