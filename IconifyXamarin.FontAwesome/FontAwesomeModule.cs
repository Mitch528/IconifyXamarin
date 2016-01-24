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

namespace IconifyXamarin.FontAwesome
{
    public class FontAwesomeModule : IIconFontDescriptor
    {
        public string TTFileName { get; } = "iconify/android-iconify-fontawesome.ttf";

        public IIcon[] Characters { get; } = FontAwesomeIcons.Icons;
    }
}