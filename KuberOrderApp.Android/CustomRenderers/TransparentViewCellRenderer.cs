using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using KuberOrderApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ViewCell), typeof(TransparentViewCellRenderer))]
namespace KuberOrderApp.Droid.CustomRenderers
{
    public class TransparentViewCellRenderer : ViewCellRenderer
    {
        private Android.Views.View cellCore;
        private Drawable unselectedBackground;
        private bool selected;

        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            cellCore = base.GetCellCore(item, convertView, parent, context);
            var listView = parent as Android.Widget.ListView;

            if (listView != null)
            {
                //Disable native cell selection color style -set as *Transparent*
                listView.SetSelector(Android.Resource.Color.Transparent);
                listView.CacheColorHint = Android.Graphics.Color.Transparent;
                //cell.SetBackgroundColor(Android.Graphics.Color.Transparent);
                selected = false;
                unselectedBackground = cellCore.Background;

                return cellCore;
            }

            return cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            if (args.PropertyName == "IsSelected")
            {
                // I had to create a property to track the selection because cellCore.Selected is always false.
                // Toggle selection
                selected = !selected;

                if (selected)
                {
                    var customTextCell = sender as ViewCell;
                    cellCore.SetBackgroundColor(Android.Graphics.Color.White);
                }
                else
                {
                    cellCore.SetBackground(unselectedBackground);
                }
            }
        }
    }
}