using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace IconifyXamarin.Internal
{
    public interface IHasOnViewAttachListener
    {
        void SetOnViewAttachListener(OnViewAttachListener listener);
    }

    public class OnViewAttachListener
    {
        public event EventHandler Attach;

        public event EventHandler Detach;

        internal void OnAttach()
        {
            Attach?.Invoke(this, EventArgs.Empty);
        }

        internal void OnDetach()
        {
            Detach?.Invoke(this, EventArgs.Empty);
        }
    }

    public class HasOnViewAttachListenerDelegate
    {
        private TextView view;
        private OnViewAttachListener listener;

        public HasOnViewAttachListenerDelegate(TextView view)
        {
            this.view = view;
        }

        public void SetOnViewAttachListener(OnViewAttachListener listener)
        {
            this.listener?.OnDetach();
            this.listener = listener;
            if (ViewCompat.IsAttachedToWindow(view))
            {
                listener?.OnAttach();
            }
        }

        public void OnAttachedToWindow()
        {
            listener?.OnAttach();
        }

        public void OnDetachedFromWindow()
        {
            listener?.OnDetach();
        }
    }
}