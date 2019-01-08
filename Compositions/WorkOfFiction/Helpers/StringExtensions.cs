using System.Collections.Generic;

namespace WorkOfFiction.Helpers
{
    public static class StringExtensions
    {
        public static string[] ToStringExtension(this object obj, bool removeFirst = true)
        {
            var properties = new List<string>();

            foreach (System.Reflection.PropertyInfo property in obj.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    properties.Add("'" + property.GetValue(obj, null) + "'");
                }
                else
                {
                    var temp = property.GetValue(obj, null);
                    if (temp != null)
                    {
                        properties.Add(temp.ToString());
                    }
                }
            }

            if (removeFirst)
            {
                properties.RemoveAt(0);
            }

            return properties.ToArray();
        }
    }
}