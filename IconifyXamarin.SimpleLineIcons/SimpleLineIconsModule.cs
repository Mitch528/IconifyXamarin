using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IconifyXamarin.SimpleLineIcons
{
    public class SimpleLineIconsModule : IIconFontDescriptor
    {
        public string TTFileName { get; } = "iconify/android-iconify-simplelineicons.ttf";
        public IIcon[] Characters { get; } = SimpleLineIconsIcons.Icons;
    }
}