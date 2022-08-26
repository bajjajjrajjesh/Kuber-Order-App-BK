using System;
using System.ComponentModel;
using Foundation;
using KuberOrderApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(Editor), typeof(EditorCustomRenderer))]
namespace KuberOrderApp.iOS.CustomRenderers
{
    public class EditorCustomRenderer : EditorRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Control == null)
                return;

            Control.TextColor = UIColor.FromRGB(92, 92, 92);
            Control.TintColor = UIColor.FromRGB(92, 92, 92);
            Control.TextColor = UIColor.FromRGB(92, 92, 92);

        }
    }
}
