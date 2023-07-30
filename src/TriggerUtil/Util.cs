using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil
{
    public static class Util
    {
        public static string To26(this int number)
        {
            number++;
            string s = string.Empty;
            while (number > 0)
            {
                int m = number % 26;
                if (m == 0) m = 26;
                s = (char)(m + 64) + s;
                number = (number - m) / 26;
            }
            return s;
        }

        public static StreamReader ToStreamReader(this string input)
        {
            using MemoryStream stream = new MemoryStream();
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return new StreamReader(stream);
        }

        public static string GetKey(this Enum e)
        {
            var type = e.GetType();
            string enumName = Enum.GetName(type, e)!; 
            FieldInfo field = type.GetField(enumName)!;
            var key = field.GetCustomAttribute<EnumKeyAttribute>();
            return key is not null ? key.Name : e.ToString();
        }

    }
}
