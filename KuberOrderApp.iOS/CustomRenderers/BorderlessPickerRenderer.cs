using System;
using System.ComponentModel;
using Foundation;
using KuberOrderApp.CustomControls;
using KuberOrderApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BorderlessPickerRenderer))]
namespace KuberOrderApp.iOS.CustomRenderers
{
    public class BorderlessPickerRenderer : PickerRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null)
                return;

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
            Control.TextColor = UIColor.FromRGB(92, 92, 92);
            var placeholderAttributes = new NSAttributedString(Control.Placeholder, new UIStringAttributes()
            { ForegroundColor = UIColor.FromRGB(92, 92, 92)});
            Control.AttributedPlaceholder = placeholderAttributes;
        }
    }
}