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

namespace IconifyXamarin.Ionicons
{
    public class IoniconsModule : IIconFontDescriptor
    {
        public string TTFileName { get; } = "iconify/android-iconify-ionicons.ttf";
        public IIcon[] Characters { get; } = IoniconsIcons.Icons;
    }
}