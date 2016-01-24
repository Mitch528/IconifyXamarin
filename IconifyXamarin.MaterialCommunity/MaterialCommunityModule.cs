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

namespace IconifyXamarin.MaterialCommunity
{
    public class MaterialCommunityModule : IIconFontDescriptor
    {
        public string TTFileName { get; } = "iconify/android-iconify-material-community.ttf";
        public IIcon[] Characters { get; } = MaterialCommunityIcons.Icons;
    }
}