using System;
using KuberOrderApp.CustomControls;
using KuberOrderApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BorderlessPickerRenderer))]
namespace KuberOrderApp.Droid.CustomRenderers
{
    [Obsolete]
    public class BorderlessPickerRenderer : PickerRenderer
    {
        public static void Init() { }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                Control.SetHintTextColor(Android.Graphics.Color.ParseColor("#5C5C5C"));
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#5C5C5C"));
                SetPadding(0, 0, 0, 0);
            }
        }
    }
}