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

namespace IconifyXamarin.Widget
{
    public class IconButton : Button, IHasOnViewAttachListener
    {
        private HasOnViewAttachListenerDelegate _delegate;

        public IconButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public IconButton(Context context) : base(context)
        {
            Init();
        }

        public IconButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public IconButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public IconButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        private void Init()
        {
            TransformationMethod = null;
        }

        public void SetOnViewAttachListener(OnViewAttachListener listener)
        {
            if (_delegate == null) _delegate = new HasOnViewAttachListenerDelegate(this);
            _delegate.SetOnViewAttachListener(listener);
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