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
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace IconifyXamarin.Internal
{
    public class CustomTypefaceSpan : ReplacementSpan
    {
        private static int ROTATION_DURATION = 2000;
        private static Rect TEXT_BOUNDS = new Rect();
        private static Paint LOCAL_PAINT = new Paint();
        private static float BASELINE_RATIO = 1 / 7f;

        private readonly string icon;
        private readonly Typeface type;
        private readonly float iconSizePx;
        private readonly float iconSizeRatio;
        private readonly int iconColor;
        private readonly bool rotate;
        private readonly bool baselineAligned;
        private readonly long rotationStartTime;

        public bool Animated => rotate;

        public CustomTypefaceSpan(IIcon icon, Typeface type,
            float iconSizePx, float iconSizeRatio, int iconColor,
            bool rotate, bool baselineAligned)
        {
            this.rotate = rotate;
            this.baselineAligned = baselineAligned;
            this.icon = icon.Character.ToString();
            this.type = type;
            this.iconSizePx = iconSizePx;
            this.iconSizeRatio = iconSizeRatio;
            this.iconColor = iconColor;
            this.rotationStartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public override void Draw(Canvas canvas, ICharSequence text, int start, int end, float x, int top, int y, int bottom, Paint paint)
        {
            ApplyCustomTypeFace(paint, type);
            paint.GetTextBounds(icon, 0, 1, TEXT_BOUNDS);
            canvas.Save();
            float baselineRatio = baselineAligned ? 0f : BASELINE_RATIO;
            if (rotate)
            {
                long time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                float rotation = (time - rotationStartTime) / (float)ROTATION_DURATION * 360f;
                float centerX = x + TEXT_BOUNDS.Width() / 2f;
                float centerY = y - TEXT_BOUNDS.Height() / 2f + TEXT_BOUNDS.Height() * baselineRatio;
                canvas.Rotate(rotation, centerX, centerY);
            }

            canvas.DrawText(icon,
                    x - TEXT_BOUNDS.Left,
                    y - TEXT_BOUNDS.Bottom + TEXT_BOUNDS.Height() * baselineRatio, paint);
        }

        public override int GetSize(Paint paint, ICharSequence text, int start, int end, Paint.FontMetricsInt fm)
        {
            LOCAL_PAINT.Set(paint);
            ApplyCustomTypeFace(LOCAL_PAINT, type);
            LOCAL_PAINT.GetTextBounds(icon, 0, 1, TEXT_BOUNDS);
            if (fm != null)
            {
                float baselineRatio = baselineAligned ? 0 : BASELINE_RATIO;
                fm.Descent = (int)(TEXT_BOUNDS.Height() * baselineRatio);
                fm.Ascent = -(TEXT_BOUNDS.Height() - fm.Descent);
                fm.Top = fm.Ascent;
                fm.Bottom = fm.Descent;
            }
            return TEXT_BOUNDS.Width();
        }

        private void ApplyCustomTypeFace(Paint paint, Typeface tf)
        {
            paint.FakeBoldText = false;
            paint.TextSkewX = 0f;
            paint.SetTypeface(tf);
            if (rotate) paint.ClearShadowLayer();
            if (iconSizeRatio > 0) paint.TextSize = paint.TextSize * iconSizeRatio;
            else if (iconSizePx > 0) paint.TextSize = iconSizePx;
            if (iconColor < int.MaxValue) paint.Color = new Color(iconColor);
        }
    }
}