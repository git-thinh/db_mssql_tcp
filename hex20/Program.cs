using System;
using System.Collections.Generic;
using System.IO; 
using System.Text; 

namespace dll_hex
{
    class Program
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba) hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        static void Main(string[] args)
        {
            string[] fs = Directory.GetFiles(".", "*.dll");

            StringBuilder bi = new StringBuilder();
            foreach(string fi in fs) {
                string f = fi.Substring(2);
                string name = f.Substring(0, f.Length - 4);

                byte[] b1 = File.ReadAllBytes(f);
                string h1 = ByteArrayToString(b1);

                string sql =
                    "IF EXISTS (SELECT * FROM sys.assemblies WHERE name = '" + name + "') DROP ASSEMBLY [" + name + "]; " + Environment.NewLine + Environment.NewLine +
                    "CREATE ASSEMBLY [" + name + "]" + Environment.NewLine +
                    "FROM 0x" + h1 + Environment.NewLine +
                    "WITH PERMISSION_SET = UNSAFE" + Environment.NewLine + Environment.NewLine;

                bi.Append(sql); 
            }

            File.WriteAllText("dll.sql", bi.ToString(), Encoding.ASCII);
        }

    }
}
