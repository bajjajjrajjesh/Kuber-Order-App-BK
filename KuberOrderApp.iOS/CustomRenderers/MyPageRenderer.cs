using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using KuberOrderApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(MyPageRenderer))]
namespace KuberOrderApp.iOS.CustomRenderers
{
    public class MyPageRenderer : PageRenderer
    {
        //I used UITableView for showing the menulist of secondary toolbar items.
        List<ToolbarItem> _secondaryItems;
        UITableView table;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            //Get all secondary toolbar items and fill it to the gloabal list variable and remove from the content page.
            if (e.NewElement is ContentPage page)
            {
                _secondaryItems = page.ToolbarItems.Where(i => i.Order == ToolbarItemOrder.Secondary).ToList();
                _secondaryItems.ForEach(t => page.ToolbarItems.Remove(t));
            }

            base.OnElementChanged(e);
        }

        public override void ViewWillAppear(bool animated)
        {

            if (NavigationController != null)
                NavigationController.TopViewController.NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null);

            //UIApplication.SharedApplication.SetStatusBarHidden(true, UIStatusBarAnimation.Fade);
            var element = (ContentPage)Element;
            //If global secondary toolbar items are not null, I created and added a primary toolbar item with image(Overflow) I         
            // want to show.
            if (element.ToolbarItems != null && element.ToolbarItems.Count <= 0)
            {
                if (_secondaryItems != null && _secondaryItems.Count > 0)
                {
                    element.ToolbarItems.Add(new ToolbarItem()
                    {
                        Order = ToolbarItemOrder.Primary,
                        IconImageSource = "ic_dots_vertical_white.png",
                        Priority = 1,
                        Command = new Command(() =>
                        {
                            ToolClicked();
                        })
                    });
                }
            }
            base.ViewWillAppear(animated);
        }

        //Create a table instance and added it to the view.
        private void ToolClicked()
        {
            if (table == null)
            {
                //Set the table position to right side. and set height to the content height.
                var childRect = new RectangleF((float)View.Bounds.Width - 250, 0, 250, _secondaryItems.Count() * 56);
                table = new UITableView(childRect)
                {
                    BackgroundColor = Xamarin.Forms.Color.Gray.ToUIColor(),
                    Source = new TableSource(_secondaryItems) // Created Table Source Class as Mentioned in the Xamarin.iOS   Official site
                };
                table.SeparatorColor = UIColor.Gray;

                Add(table);
                return;
            }

            foreach (var subview in View.Subviews)
            {
                if (subview == table)
                {
                    table.RemoveFromSuperview();
                    return;
                }
            }
            Add(table);
        }
    }
}
