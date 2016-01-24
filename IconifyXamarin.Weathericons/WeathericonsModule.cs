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

namespace IconifyXamarin.Weathericons
{
    public class WeathericonsModule : IIconFontDescriptor
    {
        public string TTFileName { get; } = "iconify/android-iconify-weathericons.ttf";
        public IIcon[] Characters { get; } = WeathericonsIcons.Icons;
    }
}