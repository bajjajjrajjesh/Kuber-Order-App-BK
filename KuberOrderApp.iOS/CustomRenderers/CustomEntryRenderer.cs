using System;
using System.ComponentModel;
using CoreGraphics;
using KuberOrderApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace KuberOrderApp.iOS.CustomRenderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control != null)
            {
                Control.TintColor = UIColor.FromRGB(159, 119, 60);
                Control.BorderStyle = UITextBorderStyle.None;
                //Control.LeftView = new UIView(new CGRect(0, 0, 15, 0));
               // Control.LeftViewMode = UITextFieldViewMode.Always;
            }
        }
    }
}