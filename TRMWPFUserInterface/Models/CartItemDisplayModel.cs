using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        private int _quantityInCart;

        public ProductDisplayModel Product { get; set; }
        public int QuantityInCart 
        {
            get { return _quantityInCart; }
            set
            {
                _quantityInCart = value;
                CallPropertyChanged(nameof(QuantityInCart));
                CallPropertyChanged(nameof(DisplayText));
            }
        }
        public string DisplayText
        {
            get
            { return $"{Product.ProductName} ({QuantityInCart})"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
