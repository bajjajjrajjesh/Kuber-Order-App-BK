using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using KuberOrderApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(EditorCustomRenderer))]
namespace KuberOrderApp.Droid.CustomRenderers
{
    public class EditorCustomRenderer : EditorRenderer
    {
        public EditorCustomRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
                //this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                this.Control.VerticalScrollBarEnabled = true;
                Control.SetHintTextColor(Android.Graphics.Color.ParseColor("#5C5C5C"));
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#5C5C5C"));
            }
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            bool ret;
            ret = base.DispatchTouchEvent(e);
            if (ret)
            {
                this.RequestDisallowInterceptTouchEvent(true);
            }

            return ret;
        }
    }
}