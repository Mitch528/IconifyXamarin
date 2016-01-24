using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IconifyXamarin.Internal
{
    public class IconFontDescriptorWrapper
    {
        private readonly object _locker = new object();

        private List<KeyValuePair<string, IIcon>> iconsByKey;

        private Typeface cachedTypeface;

        public IIconFontDescriptor IconFontDescriptor { get; }

        public IconFontDescriptorWrapper(IIconFontDescriptor iconFontDescriptor)
        {
            this.IconFontDescriptor = iconFontDescriptor;
            iconsByKey = new List<KeyValuePair<string, IIcon>>();
            IIcon[] characters = iconFontDescriptor.Characters;
            for (int i = 0, charactersLength = characters.Length; i < charactersLength; i++)
            {
                IIcon icon = characters[i];
                iconsByKey.Add(new KeyValuePair<string, IIcon>(icon.Key, icon));
            }
        }

        public IIcon GetIcon(string key)
        {
            IIcon icon = iconsByKey.SingleOrDefault(p => p.Key == key).Value;

            return icon;
        }

        public Typeface GetTypeface(Context context)
        {
            lock (_locker)
            {
                if (cachedTypeface != null) return cachedTypeface;
                cachedTypeface = Typeface.CreateFromAsset(context.Assets, IconFontDescriptor.TTFileName);
                return cachedTypeface;
            }
        }

        public bool HasIcon(IIcon icon)
        {
            return iconsByKey.Any(p => p.Value == icon);
        }
    }
}