using System;
using System.ComponentModel;
using KuberOrderApp.CustomControls;
using KuberOrderApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessDatePicker), typeof(BorderlessDatePickerRenderer))]
namespace KuberOrderApp.iOS.CustomRenderers
{
    public class BorderlessDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null)
                return;

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
            Control.TintColor = UIColor.FromRGB(92,92,92);
            Control.TextColor = UIColor.FromRGB(92, 92, 92);
        }
    }
}
