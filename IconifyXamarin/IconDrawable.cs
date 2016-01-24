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
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using IconifyXamarin.Internal;
using Java.Lang;

namespace IconifyXamarin
{
    public class IconDrawable : Drawable
    {
        public static int ANDROID_ACTIONBAR_ICON_SIZE_DP = 24;

        private Context context;

        private IIcon icon;

        private TextPaint paint;

        private int size = -1;

        private int alpha = 255;

        public IconDrawable(Context context, string iconKey)
        {
            IIcon icon = Iconify.FindIconForKey(iconKey);
            if (icon == null)
            {
                throw new ArgumentException("No icon with that key \"" + iconKey + "\".");
            }

            Init(context, icon);
        }

        public IconDrawable(Context context, IIcon icon)
        {
            Init(context, icon);
        }

        private void Init(Context context, IIcon icon)
        {
            this.context = context;
            this.icon = icon;
            paint = new TextPaint();
            IconFontDescriptorWrapper descriptor = Iconify.FindTypefaceOf(icon);
            if (descriptor == null)
            {
                throw new IllegalStateException("Unable to find the module associated " +
                        "with icon " + icon.Key + ", have you registered the module " +
                        "you are trying to use with Iconify.with(...) in your Application?");
            }
            paint.SetTypeface(descriptor.GetTypeface(context));
            paint.SetStyle(Paint.Style.Fill);
            paint.TextAlign = Paint.Align.Center;
            paint.UnderlineText = false;
            paint.Color = new Android.Graphics.Color(0, 0, 0);
            //paint.Color = Color.Black;
            paint.AntiAlias = true;
        }

        public override void Draw(Canvas canvas)
        {
            Rect bounds = Bounds;
            int height = bounds.Height();
            paint.TextSize = height;
            Rect textBounds = new Rect();
            string textValue = icon.Character.ToString();
            paint.GetTextBounds(textValue, 0, 1, textBounds);
            int textHeight = textBounds.Height();
            float textBottom = bounds.Top + (height - textHeight) / 2f + textHeight - textBounds.Bottom;
            canvas.DrawText(textValue, bounds.ExactCenterX(), textBottom, paint);
        }

        public override bool SetState(int[] stateSet)
        {
            int oldValue = paint.Alpha;
            int newValue = IsEnabled(stateSet) ? alpha : alpha / 2;
            paint.Alpha = newValue;
            return oldValue != newValue;
        }

        public override void SetAlpha(int alpha)
        {
            this.alpha = alpha;
            paint.Alpha = alpha;
        }

        public override void SetColorFilter(ColorFilter colorFilter)
        {
            paint.SetColorFilter(colorFilter);
        }

        public override void ClearColorFilter()
        {
            paint.SetColorFilter(null);
        }

        public void SetStyle(Paint.Style style)
        {
            paint.SetStyle(style);
        }

        public override int Opacity => alpha;

        public IconDrawable SizeRes(int dimenRes)
        {
            return SizePx(context.Resources.GetDimensionPixelSize(dimenRes));
        }

        public IconDrawable SizeDp(int size)
        {
            return SizePx(ConvertDpToPx(context, size));
        }

        public IconDrawable SizePx(int size)
        {
            this.size = size;
            SetBounds(0, 0, size, size);
            InvalidateSelf();
            return this;
        }

        public IconDrawable Color(int color)
        {
            paint.Color = new Color(color);
            InvalidateSelf();
            return this;
        }

        public IconDrawable ColorRes(int colorRes)
        {
#pragma warning disable 618
            paint.Color = context.Resources.GetColor(colorRes);
#pragma warning restore 618
            InvalidateSelf();
            return this;
        }

        public new IconDrawable Alpha(int alpha)
        {
            SetAlpha(alpha);
            InvalidateSelf();
            return this;
        }

        public override int IntrinsicHeight => size;
        public override int IntrinsicWidth => size;
        public override bool IsStateful => true;

        // Util
        private bool IsEnabled(int[] stateSet)
        {
            foreach (int state in stateSet)
                if (state == Android.Resource.Attribute.StateEnabled)
                    return true;
            return false;
        }

        // Util
        private int ConvertDpToPx(Context context, float dp)
        {
            return (int)TypedValue.ApplyDimension(
                    ComplexUnitType.Dip, dp,
                    context.Resources.DisplayMetrics);
        }

    }
}