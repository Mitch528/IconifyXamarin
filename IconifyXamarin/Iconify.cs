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
using IconifyXamarin.Internal;
using Java.Lang;

namespace IconifyXamarin
{
    public class Iconify
    {
        private static List<IconFontDescriptorWrapper> iconFontDescriptors = new List<IconFontDescriptorWrapper>();

        public static IconifyInitializer With(IIconFontDescriptor iconFontDescriptor)
        {
            return new IconifyInitializer(iconFontDescriptor);
        }

        public static void AddIcons(params TextView[] textViews)
        {
            foreach (TextView textView in textViews)
            {
                if (textView == null) continue;
                textView.TextFormatted = Compute(textView.Context, textView.Text, textView);
            }
        }

        private static void AddIconFontDescriptor(IIconFontDescriptor iconFontDescriptor)
        {
            // Prevent duplicates
            if (iconFontDescriptors.Any(wrapper => wrapper.IconFontDescriptor.TTFileName
                .Equals(iconFontDescriptor.TTFileName)))
            {
                return;
            }

            // Add to the list
            iconFontDescriptors.Add(new IconFontDescriptorWrapper(iconFontDescriptor));
        }

        public static ICharSequence Compute(Context context, string text)
        {
            return Compute(context, text, null);
        }

        public static ICharSequence Compute(Context context, string text, TextView target)
        {
            return ParsingUtil.Parse(context, iconFontDescriptors, text, target);
        }

        public class IconifyInitializer
        {

            public IconifyInitializer(IIconFontDescriptor iconFontDescriptor)
            {
                Iconify.AddIconFontDescriptor(iconFontDescriptor);
            }

            /**
             * Add support for a new icon font.
             * @param iconFontDescriptor The IconDescriptor holding the ttf file reference and its mappings.
             * @return An initializer instance for chain calls.
             */
            public IconifyInitializer With(IIconFontDescriptor iconFontDescriptor)
            {
                Iconify.AddIconFontDescriptor(iconFontDescriptor);
                return this;
            }
        }

        public static IconFontDescriptorWrapper FindTypefaceOf(IIcon icon)
        {
            return iconFontDescriptors.FirstOrDefault(iconFontDescriptor => iconFontDescriptor.HasIcon(icon));
        }

        public static IIcon FindIconForKey(string iconKey)
        {
            for (int i = 0, iconFontDescriptorsSize = iconFontDescriptors.Count; i < iconFontDescriptorsSize; i++)
            {
                IconFontDescriptorWrapper iconFontDescriptor = iconFontDescriptors[i];
                IIcon icon = iconFontDescriptor.GetIcon(iconKey);
                if (icon != null) return icon;
            }
            return null;
        }
    }
}