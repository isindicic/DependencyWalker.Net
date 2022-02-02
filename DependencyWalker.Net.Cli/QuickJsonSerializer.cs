using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SindaSoft.DependencyWalker
{
    public class QuickJsonSerializer
    {
        public static string Serialize(object o, int ident = 0)
        {
            string spaces = Ident(ident);

            if (o is IEnumerable)
            {
                List<string> items = new List<string>();
                foreach (object v in (o as IEnumerable))
                    items.Add(Serialize(v, ident + 1));

                return spaces + "[\n" +
                       spaces + String.Join(",\n", items.ToArray()) +
                       "\n" +
                       spaces + "]\n";
            }
            else if (o is IDictionary)
            {
                Dictionary<string, string> items = new Dictionary<string, string>();

                foreach (object k in (o as IDictionary).Keys)
                    items[Convert.ToString(k)] = Serialize((o as IDictionary)[k], ident + 1);

                return spaces + "{\n" +
                       spaces + String.Join(",\n", items.Select(x => "\"" + x.Key + "\" : " + x.Value).ToArray()) +
                       "\n" +
                       spaces + "}\n";
            }
            else
            {
                Dictionary<string, string> items = new Dictionary<string, string>();
                Type t = o.GetType();
                PropertyInfo[] pis = t.GetProperties();

                foreach (PropertyInfo pi in pis)
                {
                    object v = pi.GetValue(o, null);
                    if( v == null)
                        items[pi.Name] = "null";
                    else if (v is String)
                        items[pi.Name] = "\"" + Convert.ToString(v) + "\"";
                    else if (v is int || v is uint || v is long || v is ulong || v is bool)
                        items[pi.Name] = Convert.ToString(v);
                    else if (v is double || v is float)
                        items[pi.Name] = Convert.ToString(v, CultureInfo.InvariantCulture);
                    else
                        items[pi.Name] = Serialize(v, ident + 1);
                }
                return spaces + "{\n" +
                       spaces + String.Join(",\n", items.Select(x => "\"" + x.Key + "\" : " + x.Value).ToArray()) +
                       "\n" +
                       spaces + "}\n";
            }
        }

        private static string Ident(int level)
        {
            return new String(' ', level * 3);
        }
    }
}
