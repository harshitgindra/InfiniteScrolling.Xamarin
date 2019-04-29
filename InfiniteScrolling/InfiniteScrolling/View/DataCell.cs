using System;
using Xamarin.Forms;

namespace InfiniteScrolling.View
{
    public class DataCell: TextCell
    {
        public DataCell()
        {
            this.SetBinding(TextCell.TextProperty, ".");
        }
    }
}
