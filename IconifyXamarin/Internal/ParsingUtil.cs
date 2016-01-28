using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using String = System.String;

namespace IconifyXamarin.Internal
{
    public static class ParsingUtil
    {
        private const string ANDROID_PACKAGE_NAME = "android";

        public static ICharSequence Parse(
                Context context,
                List<IconFontDescriptorWrapper> iconFontDescriptors,
                string text,
                 TextView target)
        {
            context = context.ApplicationContext;

            // Analyse the text and replace {} blocks with the appropriate character
            // Retain all transformations in the accumulator
            SpannableStringBuilder spannableBuilder = new SpannableStringBuilder(text);
            RecursivePrepareSpannableIndexes(context,
                text, spannableBuilder,
                iconFontDescriptors, 0);
            bool isAnimated = HasAnimatedSpans(spannableBuilder);

            if (isAnimated)
            {
                if (target == null)
                    throw new IllegalArgumentException("You can't use \"spin\" without providing the target TextView.");
                if (!target.GetType().IsInstanceOfType(typeof(IHasOnViewAttachListener)))
                    throw new IllegalArgumentException(target.GetType().Name + " does not implement " +
                            "HasOnViewAttachListener. Please use IconTextView, IconButton or IconToggleButton.");

                bool isAttached = false;
                var listener = new OnViewAttachListener();
                listener.Attach += (s, e) =>
                {
                    isAttached = true;

                    Runnable runnable = null;
                    runnable = new Java.Lang.Runnable(() =>
                    {
                        if (isAttached)
                        {
                            target.Invalidate();
                            ViewCompat.PostOnAnimation(target, runnable);
                        }
                    });

                    ViewCompat.PostOnAnimation(target, runnable);
                };

                listener.Detach += (s, e) => isAttached = false;

                ((IHasOnViewAttachListener)target).SetOnViewAttachListener(listener);
            }
            else if (target.GetType().IsInstanceOfType(typeof(IHasOnViewAttachListener)))
            {
                ((IHasOnViewAttachListener)target).SetOnViewAttachListener(null);
            }

            return spannableBuilder;
        }

        private static bool HasAnimatedSpans(SpannableStringBuilder spannableBuilder)
        {
            CustomTypefaceSpan[] spans = spannableBuilder.GetSpans(0, spannableBuilder.Length(),
                Class.FromType(typeof(CustomTypefaceSpan))).Cast<CustomTypefaceSpan>().ToArray();

            return spans.Any(span => span.Animated);
        }

        private static void RecursivePrepareSpannableIndexes(
                        Context context,
                        string fullText,
                        SpannableStringBuilder text,
                        List<IconFontDescriptorWrapper> iconFontDescriptors,
                        int start)
        {
            string stringText = text.ToString();
            // Try to find a {...} in the string and extract expression from it
            int startIndex = stringText.IndexOf("{", start, StringComparison.Ordinal);
            if (startIndex == -1) return;
            int endIndex = stringText.IndexOf("}", startIndex, StringComparison.Ordinal) + 1;
            string expression = stringText.Substring(startIndex + 1, endIndex - startIndex - 2);
            
            // Split the expression and retrieve the icon key
            string[] strokes = expression.Split(' ');
            string key = strokes[0];

            // Loop through the descriptors to find a key match
            IconFontDescriptorWrapper iconFontDescriptor = null;
            IIcon icon = null;
            for (int i = 0; i < iconFontDescriptors.Count; i++)
            {
                iconFontDescriptor = iconFontDescriptors[i];
                icon = iconFontDescriptor.GetIcon(key);
                if (icon != null) break;
            }

            // If no match, ignore and continue
            if (icon == null)
            {
                RecursivePrepareSpannableIndexes(context, fullText, text, iconFontDescriptors, endIndex);
                return;
            }

            // See if any more stroke within {} should be applied
            float iconSizePx = -1;
            int iconColor = int.MaxValue;
            float iconSizeRatio = -1;
            bool spin = false;
            bool baselineAligned = false;
            for (int i = 1; i < strokes.Length; i++)
            {
                string stroke = strokes[i];

                // Look for "spin"
                if (stroke.Equals("spin", StringComparison.OrdinalIgnoreCase))
                {
                    spin = true;
                }

                // Look for "baseline"
                else if (stroke.Equals("baseline", StringComparison.OrdinalIgnoreCase))
                {
                    baselineAligned = true;
                }

                // Look for an icon size
                else if (Regex.IsMatch(stroke, "([0-9]*(\\.[0-9]*)?)dp"))
                {
                    iconSizePx = DpToPx(context, float.Parse(stroke.Substring(0, stroke.Length - 2)));
                }
                else if (Regex.IsMatch(stroke, "([0-9]*(\\.[0-9]*)?)sp"))
                {
                    iconSizePx = SpToPx(context, float.Parse(stroke.Substring(0, stroke.Length - 2)));
                }
                else if (Regex.IsMatch(stroke, "([0-9]*)px"))
                {
                    iconSizePx = int.Parse(stroke.Substring(0, stroke.Length - 2));
                }
                else if (Regex.IsMatch(stroke, "@dimen/(.*)"))
                {
                    iconSizePx = GetPxFromDimen(context, context.PackageName, stroke.Substring(7));
                    if (iconSizePx < 0)
                        throw new IllegalArgumentException("Unknown resource " + stroke + " in \"" + fullText + "\"");
                }
                else if (Regex.IsMatch(stroke, "@android:dimen/(.*)"))
                {
                    iconSizePx = GetPxFromDimen(context, ANDROID_PACKAGE_NAME, stroke.Substring(15));
                    if (iconSizePx < 0)
                        throw new IllegalArgumentException("Unknown resource " + stroke + " in \"" + fullText + "\"");
                }
                else if (Regex.IsMatch(stroke, "([0-9]*(\\.[0-9]*)?)%"))
                {
                    iconSizeRatio = float.Parse(stroke.Substring(0, stroke.Length - 1)) / 100f;
                }

                // Look for an icon color
                else if (Regex.IsMatch(stroke, "#([0-9A-Fa-f]{6}|[0-9A-Fa-f]{8})"))
                {
                    iconColor = Color.ParseColor(stroke);
                }
                else if (Regex.IsMatch(stroke, "@color/(.*)"))
                {
                    iconColor = GetColorFromResource(context, context.PackageName, stroke.Substring(7));
                    if (iconColor == int.MaxValue)
                        throw new IllegalArgumentException("Unknown resource " + stroke + " in \"" + fullText + "\"");
                }
                else if (Regex.IsMatch(stroke, "@android:color/(.*)"))
                {
                    iconColor = GetColorFromResource(context, ANDROID_PACKAGE_NAME, stroke.Substring(15));
                    if (iconColor == int.MaxValue)
                        throw new IllegalArgumentException("Unknown resource " + stroke + " in \"" + fullText + "\"");
                }
                else
                {
                    throw new IllegalArgumentException("Unknown expression " + stroke + " in \"" + fullText + "\"");
                }
            }

            // Replace the character and apply the typeface
            text = (SpannableStringBuilder)text.Replace(startIndex, endIndex, "" + icon.Character);
            text.SetSpan(new CustomTypefaceSpan(icon,
                            iconFontDescriptor.GetTypeface(context),
                            iconSizePx, iconSizeRatio, iconColor, spin, baselineAligned),
                    startIndex, startIndex + 1,
                    SpanTypes.InclusiveExclusive);
            RecursivePrepareSpannableIndexes(context, fullText, text, iconFontDescriptors, startIndex);
        }

        public static float GetPxFromDimen(Context context, String packageName, String resName)
        {
            Resources resources = context.Resources;
            int resId = resources.GetIdentifier(
                    resName, "dimen",
                    packageName);
            if (resId <= 0) return -1;
            return resources.GetDimension(resId);
        }

        public static int GetColorFromResource(Context context, String packageName, String resName)
        {
            Resources resources = context.Resources;
            int resId = resources.GetIdentifier(
                    resName, "color",
                    packageName);
            if (resId <= 0) return int.MaxValue;
#pragma warning disable 618
            return resources.GetColor(resId);
#pragma warning restore 618
        }

        public static float DpToPx(Context context, float dp)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, dp,
                    context.Resources.DisplayMetrics);
        }

        public static float SpToPx(Context context, float sp)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Sp, sp,
                    context.Resources.DisplayMetrics);
        }
    }
}