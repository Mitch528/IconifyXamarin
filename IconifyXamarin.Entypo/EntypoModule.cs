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

namespace IconifyXamarin.Entypo
{
    public class EntypoModule : IIconFontDescriptor
    {
        public string TTFileName { get; } = "iconify/android-iconify-entypo.ttf";
        public IIcon[] Characters { get; } = EntypoIcons.Icons;
    }
}