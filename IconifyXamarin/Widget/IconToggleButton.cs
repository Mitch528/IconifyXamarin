using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IconifyXamarin.Internal;
using Java.Lang;

namespace IconifyXamarin.Widget
{
    public class IconToggleButton : ToggleButton, IHasOnViewAttachListener
    {
        private HasOnViewAttachListenerDelegate _delegate;

        public IconToggleButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public IconToggleButton(Context context) : base(context)
        {
            Init();
        }

        public IconToggleButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public IconToggleButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public IconToggleButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        public override void SetText(ICharSequence text, BufferType type)
        {
            base.SetText(Iconify.Compute(Context, text.ToString(), this), BufferType.Normal);
        }

        public void SetOnViewAttachListener(OnViewAttachListener listener)
        {
            if (_delegate == null) _delegate = new HasOnViewAttachListenerDelegate(this);
            _delegate.SetOnViewAttachListener(listener);
        }

        private void Init()
        {
            TransformationMethod = null;
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            _delegate.OnAttachedToWindow();
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            _delegate.OnDetachedFromWindow();
        }
    }
}