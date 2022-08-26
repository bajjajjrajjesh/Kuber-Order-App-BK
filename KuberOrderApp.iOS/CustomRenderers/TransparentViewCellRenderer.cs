using System;
using KuberOrderApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(TransparentViewCellRenderer))]
namespace KuberOrderApp.iOS.CustomRenderers
{
    public class TransparentViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
            {
                // Disable native cell selection color style - set as *Transparent*
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            return cell;
        }
    }
}
