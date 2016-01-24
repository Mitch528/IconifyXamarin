using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IconifyXamarin
{
    public static class IconReflectionUtils
    {
        public static List<KeyValuePair<char, string>> GetIcons<T>()
        {
            var characters = new List<KeyValuePair<char, string>>();

            foreach (FieldInfo fi in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static)
               .Where(p => p.FieldType == typeof(char)))
            {
                char key = (char)fi.GetValue(null);
                string value = fi.Name;

                characters.Add(new KeyValuePair<char, string>(key, value));
            }

            return characters;
        }
    }
}