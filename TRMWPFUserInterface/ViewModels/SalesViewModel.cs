using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<string> _products;
		private string _itemQuantity;
		private BindingList<string> _cart;

		public BindingList<string> Products
		{
			get { return _products; }
			set 
			{ 
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}


		public string ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => Products);
			}
		}


		public BindingList<string> Cart
		{
			get { return _cart; }
			set 
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public string Subtotal 
		{
			get
			{
				// TODO replace with calculation
				return "0,00 Ft";
			}
		}

		public string Tax
		{
			get
			{
				// TODO replace with calculation
				return "0,00 Ft";
			}
		}
		public string Total
		{
			get
			{
				// TODO replace with calculation
				return "0,00 Ft";
			}
		}

		public void AddToCart()
		{

		}

		public bool CanAddToCart 
		{
			get
			{
				bool output = false;
				
				//Make sure something is in the cart
				//Make sure the required item quantity is available
				if ()
				{
					output = true;
				}
				return output;
			}
		}
		
		public void RemoveFromCart()
		{

		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				//Make sure something is selected in the cart
				if ()
				{
					output = true;
				}
				return output;
			}
		}

		public void CheckOut()
		{

		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				//Make sure checkout is possible
				if (Cart.Count > 0)
				{
					output = true;
				}
				return output;
			}
		}

	}
}
